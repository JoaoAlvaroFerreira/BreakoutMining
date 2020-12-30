import torch
import torch.nn as nn
from torch.distributions import MultivariateNormal
import gym
import numpy as np

# https://github.com/bentrevett/pytorch-rl/blob/master/5%20-%20Proximal%20Policy%20Optimization%20(PPO)%20%5BCartPole%5D.ipynb
# https://spinningup.openai.com/en/latest/spinningup/rl_intro2.html
# https://pytorch.org/tutorials/intermediate/reinforcement_q_learning.html
# https://github.com/nikhilbarhate99/PPO-PyTorch/blob/master/PPO_continuous.py


device = torch.device("cuda:0" if torch.cuda.is_available() else "cpu")


class Memory:
    def __init__(self):
        self.actions = []
        self.states = []
        self.logprobs = []
        self.rewards = []
        self.is_terminals = []

    def clear_memory(self):
        del self.actions[:]
        del self.states[:]
        del self.logprobs[:]
        del self.rewards[:]
        del self.is_terminals[:]


class ActorCritic(nn.Module):
    def __init__(self, state_dim, action_dim, action_std):
        super(ActorCritic, self).__init__()
        # action mean range -1 to 1
        self.actor = nn.Sequential(
            nn.Linear(state_dim, 64),
            nn.Tanh(),
            nn.Linear(64, 32),
            nn.Tanh(),
            nn.Linear(32, action_dim),
            nn.Tanh()
        )
        # critic
        self.critic = nn.Sequential(
            nn.Linear(state_dim, 64),
            nn.Tanh(),
            nn.Linear(64, 32),
            nn.Tanh(),
            nn.Linear(32, 1)
        )
        self.action_var = torch.full(
            (action_dim,), action_std*action_std).to(device)

    def forward(self):
        raise NotImplementedError

    def act(self, state, memory):
        action_mean = self.actor(state)
        cov_mat = torch.diag(self.action_var).to(device)

        dist = MultivariateNormal(action_mean, cov_mat)
        action = dist.sample()
        action_logprob = dist.log_prob(action)

        memory.states.append(state)
        memory.actions.append(action)
        memory.logprobs.append(action_logprob)

        return action.detach()

    def evaluate(self, state, action):
        action_mean = self.actor(state)

        action_var = self.action_var.expand_as(action_mean)
        cov_mat = torch.diag_embed(action_var).to(device)

        dist = MultivariateNormal(action_mean, cov_mat)

        action_logprobs = dist.log_prob(action)
        dist_entropy = dist.entropy()
        state_value = self.critic(state)

        return action_logprobs, torch.squeeze(state_value), dist_entropy


class PPO:
    def __init__(self, state_dim, action_dim, action_std, lr, betas, gamma, K_epochs, eps_clip):
        self.lr = lr
        self.betas = betas
        self.gamma = gamma
        self.eps_clip = eps_clip
        self.K_epochs = K_epochs

        self.policy = ActorCritic(state_dim, action_dim, action_std).to(device)
        self.optimizer = torch.optim.Adam(
            self.policy.parameters(), lr=lr, betas=betas)

        self.policy_old = ActorCritic(
            state_dim, action_dim, action_std).to(device)
        self.policy_old.load_state_dict(self.policy.state_dict())

        self.MseLoss = nn.MSELoss()

    def select_action(self, state, memory):
        state = torch.FloatTensor(state.reshape(1, -1)).to(device)
        return self.policy_old.act(state, memory).cpu().data.numpy().flatten()

    def update(self, memory):
        # Monte Carlo estimate of rewards:
        rewards = []
        discounted_reward = 0
        for reward, is_terminal in zip(reversed(memory.rewards), reversed(memory.is_terminals)):
            if is_terminal:
                discounted_reward = 0
            discounted_reward = reward + (self.gamma * discounted_reward)
            rewards.insert(0, discounted_reward)

        # Normalizing the rewards:
        rewards = torch.tensor(rewards).to(device)
        rewards = (rewards - rewards.mean()) / (rewards.std() + 1e-5)

        # convert list to tensor
        old_states = torch.squeeze(torch.stack(
            memory.states).to(device), 1).detach()
        old_actions = torch.squeeze(torch.stack(
            memory.actions).to(device), 1).detach()
        old_logprobs = torch.squeeze(torch.stack(
            memory.logprobs), 1).to(device).detach()

        # Optimize policy for K epochs:
        for _ in range(self.K_epochs):
            # Evaluating old actions and values :
            logprobs, state_values, dist_entropy = self.policy.evaluate(
                old_states, old_actions)

            # Finding the ratio (pi_theta / pi_theta__old):
            ratios = torch.exp(logprobs - old_logprobs.detach())

            # Finding Surrogate Loss:
            advantages = rewards - state_values.detach()
            surr1 = ratios * advantages
            surr2 = torch.clamp(ratios, 1-self.eps_clip,
                                1+self.eps_clip) * advantages
            loss = -torch.min(surr1, surr2) + 0.5 * \
                self.MseLoss(state_values, rewards) - 0.01*dist_entropy

            # take gradient step
            self.optimizer.zero_grad()
            loss.mean().backward()
            self.optimizer.step()

        # Copy new weights into old policy:
        self.policy_old.load_state_dict(self.policy.state_dict())


class PPOModel:
    def __init__(self, state_dim, action_dim, solve_average_reward=14, action_std=0.5, K_epochs=80, eps_clip=0.2, gamma=0.99, lr=0.0003, betas=(0.9, 0.999)):
        self.state_dim = state_dim
        self.action_dim = action_dim
        self.solve_average_reward = solve_average_reward
        self.action_std = action_std
        self.K_epochs = K_epochs
        self.eps_clip = eps_clip
        self.gamma = gamma
        self.lr = lr
        self.betas = betas
        self.time_step = 0
        self.episodes = 0
        self.memory = Memory()
        self.ppo = PPO(self.state_dim, self.action_dim, self.action_std,
                       self.lr, self.betas, self.gamma, self.K_epochs, self.eps_clip)

    def get_action(self, state):
        self.time_step += 1
        action = self.ppo.select_action(state, self.memory)
        return action

    def end_game(self, reward, done):
        # Saving reward and is_terminals:
        self.memory.rewards.append(reward)
        self.memory.is_terminals.append(done)

    def end_episode(self):
        self.episodes += 1
        self.ppo.update(self.memory)
        self.memory.clear_memory()
        if self.episodes == 10:
            self.episodes = 0
            torch.save(self.ppo.policy.state_dict(),
                       './PPO_safe_point_{}.pth'.format('breakout'))

    def save_mode(self):
        torch.save(self.ppo.policy.state_dict(),
                   './PPO_{}.pth'.format('breakout'))
