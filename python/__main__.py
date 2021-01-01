from math import ceil, floor
from shutil import copyfile

from stable_baselines3 import PPO
from stable_baselines3.ppo import MlpPolicy

from utils.environment import load_environment

from game_predictor import GamePredictor

# Control Variables
episodes = 10
test_ratio = 0.25

train_episodes = ceil(episodes * (1 - test_ratio))
test_episodes = floor(episodes * test_ratio)

# Init Training Environment
train_env = load_environment()

# Training Stage
print("Start Training Stage")
train_env.reset()

rl = PPO(MlpPolicy, train_env, verbose=1)
rl.learn(total_timesteps=train_episodes)
train_env.close()
print("Closed")

copyfile('./data.csv', './data_train.csv')

ml = GamePredictor('knn')
ml.train("data_train.csv")

print("### Models Finished Training")

# Init Test Environment

test_env = load_environment()

# Testing Stage
print("### Starting Testing Simulations")

obs = test_env.reset()
for i in test_episodes:
    action, _states = rl.predict(obs)
    obs, rewards, dones, info = test_env.step(action)
    test_env.render()

test_env.close()

print("### Testing Simulations Finished")

copyfile('./data.csv', './data_test.csv')
results = ml.predict("data_test.csv")

print(results)

print("### Models Finished Testing")
