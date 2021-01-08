# BreakoutMining
The source code of a project to study the performance of Machine Learning to predict good parameters to improve the adaptivity of different AI players in a simulated Breakout game. The game adaptivity is done by a reinforcement learning agent, but alternative parameters are suggested by supervised learning model through the analysis of collected game data.

## Requirements
* `python 3.8`
* `pip 20.3`

## Build Unity Breakout Simulation
Open the game in the Unity Game Editor and build the game in directory called `build/` inside the root of the repo.

## Install dependencies
Run in the root of the repository:
```shell
pip install -r python/requirements.txt
```

## Run the simulation
Be sure to have the game built inside the `build/` directory and all the requirements and dependencies installed.

Run in the root of the repository:
```shell
python python
```

## Rerun only the Supervised Learning Models
This script runs the supervised learning models again, so it is necessary to have both `data_train.csv` and `data_test.csv` in the root of the repository.

Run in the root of the repository:
```shell
python python/test_model.py
```

## Authors
* João Álvaro Ferreira
* João Augusto Lima
* João Carlos Maduro
* Mariana Neto