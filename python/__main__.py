from math import ceil, floor
from shutil import copyfile

from stable_baselines3 import PPO
from stable_baselines3.ppo import MlpPolicy

from utils.environment import load_environment
from utils.database import read_dataset, filter_satisfaction
from utils.graphs import plot_satisfactions
from utils.evaluation import eval

from game_predictor import GamePredictor

# Control Variables
episodes = 15000
test_ratio = 0.25

train_episodes = ceil(episodes * (1 - test_ratio))
test_episodes = floor(episodes * test_ratio)

# Init Training Environment
train_env = load_environment()

# Training Stage
print("Start Training Stage")


rl = PPO(MlpPolicy, train_env, verbose=1, n_steps=10)
train_env.reset()
rl.learn(total_timesteps=episodes)
rl.save("breakout_model")
train_env.close()
print("Closed")

copyfile('./data.csv', './data_train.csv')

multi_output = GamePredictor('rf', single_output=False)
single_output = GamePredictor('rf', single_output=True)

dataset = read_dataset("data_train.csv")
#plot_satisfactions("train", dataset)
filtered_dataset = filter_satisfaction(dataset)

multi_output.train(filtered_dataset)
single_output.train(filtered_dataset)

print("### Models Finished Training")

# Init Test Environment

test_env = load_environment()

# Testing Stage
print("### Starting Testing Simulations")

obs = test_env.reset()
dones = False
for i in range(test_episodes):
    action, _states = rl.predict(obs)
    if dones:
        obs = test_env.reset()
        dones = False
    else:
        obs, rewards, dones, info = test_env.step(action)

test_env.close()

print("### Testing Simulations Finished")

copyfile('./data.csv', './data_test.csv')

test_dataset = read_dataset("data_test.csv")
#plot_satisfactions("test", test_dataset)

multi_output_results = multi_output.predict(test_dataset)
single_output_results = single_output.predict(test_dataset)

print("### Multi-Output Results")
print(multi_output_results)

print("### Single-Output Results")
print(single_output_results)

print("### Evaluating Models Performance")
eval(test_dataset, multi_output_results, single_output_results)

print("### Models Finished Testing")
