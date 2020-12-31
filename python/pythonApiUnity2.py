import gym

from stable_baselines3 import PPO
from stable_baselines3.ppo import MlpPolicy
from stable_baselines3.common.env_util import make_vec_env

from mlagents_envs.environment import UnityEnvironment
from gym_unity.envs import UnityToGymWrapper


print("Waiting for game to start")
unity_timeout = 1000000
unity_env = UnityEnvironment(file_name=None, seed=1,
                             timeout_wait=unity_timeout, side_channels=[])
env = UnityToGymWrapper(unity_env)

env.reset()

model = PPO(MlpPolicy, env, verbose=1)
model.learn(total_timesteps=25000)
model.save("ppo_cartpole")
