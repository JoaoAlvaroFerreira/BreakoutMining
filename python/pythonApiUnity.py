from mlagents_envs.environment import UnityEnvironment
import numpy as np

env = UnityEnvironment(file_name=None, seed=1, side_channels=[])
# env = UnityEnvironment(file_name="./build/BreakoutMining", seed=1,side_channels=[], no_graphics=True)

env.reset()
for i in range(1000):
    behaviour_names = list(env.behavior_specs.keys())
    behaviour_specs = list(env.behavior_specs.values())
    print(behaviour_names)
    # https://github.com/Unity-Technologies/ml-agents/blob/master/docs/Python-API.md#terminalsteps-and-terminalstep
    terminated_steps = []
    # https://github.com/Unity-Technologies/ml-agents/blob/master/docs/Python-API.md#decisionsteps-and-decisionstep
    decision_steps = []
    while len(decision_steps) == 0:
        behaviour_tuple = env.get_steps(behavior_name=behaviour_names[0])
        decision_steps = behaviour_tuple[0]
        terminated_steps = behaviour_tuple[1]
    # A game is starting and asked for the parameters
    print("Game is starting")
    # TODO decidir aqui que parametros dar ao unity
    action = np.array([[10, i+1]])
    env.set_actions(behavior_name=behaviour_names[0], action=action)
    env.step()
    while len(terminated_steps) == 0:
        behaviour_tuple = env.get_steps(behavior_name=behaviour_names[0])
        decision_steps = behaviour_tuple[0]
        terminated_steps = behaviour_tuple[1]
    # The game has ended
    print("Game has ended")
    # TODO


env.close()
