from mlagents_envs.environment import UnityEnvironment
from gym_unity.envs import UnityToGymWrapper


def load_environment():
    unity_timeout = 1000000
    unity_env = UnityEnvironment(file_name="./build/BreakoutMining", seed=1,
                                 timeout_wait=unity_timeout, side_channels=[], no_graphics=False)
    return UnityToGymWrapper(unity_env)
