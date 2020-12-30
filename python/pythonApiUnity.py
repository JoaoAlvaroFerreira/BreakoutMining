from mlagents_envs.environment import UnityEnvironment
import numpy as np
from game_predictor import GamePredictor, ppo
from game_predictor.ppo import PPOModel

unity_timeout = 1000000

print("Waiting for game to start")
env = UnityEnvironment(file_name=None, seed=1,
                       timeout_wait=unity_timeout, side_channels=[])
# env = UnityEnvironment(file_name="./build/BreakoutMining", seed=1, side_channels=[], no_graphics=True)

gp = GamePredictor("knn")
episodes = 10
agent_number = 1
input_size = 14
output_size = 3


rl = PPOModel(input_size, output_size)

# https://docs.unity3d.com/Packages/com.unity.ml-agents@1.0/api/Unity.MLAgents.Agent.html#methods
# https://github.com/Unity-Technologies/ml-agents/tree/master/docs#python-tutorial-with-google-colab
for i in range(episodes):
    env.reset()
    behaviour_names = list(env.behavior_specs.keys())
    behaviour_specs = list(env.behavior_specs.values())
    print(behaviour_specs)

    # https://github.com/Unity-Technologies/ml-agents/blob/master/docs/Python-API.md#terminalsteps-and-terminalstep
    # https://github.com/Unity-Technologies/ml-agents/blob/master/docs/Python-API.md#decisionsteps-and-decisionstep

    decision_steps, terminated_steps = env.get_steps(
        behavior_name=behaviour_names[0])
    done = False
    while not done:
        # A game is starting and asked for the parameters
        print("Deciding, array of observation is:")
        print(decision_steps.obs)
        agent_id = decision_steps.agent_id[0]
        action = rl.get_action(decision_steps.obs[agent_id])
        print("Action decided:")
        print(action)
        action = np.array([action])
        env.set_action_for_agent(
            behaviour_names[0], agent_id, action)
        print("Decision sent, wait for next ones")
        env.step()
        print("Reading requests")
        decision_steps, terminated_steps = env.get_steps(
            behavior_name=behaviour_names[0])

        if len(terminated_steps) == agent_number:
            rl.end_game(
                terminated_steps.reward[agent_id], True)
            rl.end_episode()
            done = True
        else:
            print(f"Game reward: {decision_steps.reward[agent_id]}")
            rl.end_game(decision_steps.reward[agent_id], False)

        # The game has ended
        print("Game has ended")


env.close()

# gp.train("data.csv")
