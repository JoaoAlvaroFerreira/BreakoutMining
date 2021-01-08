from game_predictor import GamePredictor
from utils.database import read_dataset, filter_satisfaction
from utils.graphs import plot_satisfactions
from utils.evaluation import eval

mp = GamePredictor("rf", single_output=False)
gp = GamePredictor("rf", single_output=True)

dataset = read_dataset("data_train.csv")
plot_satisfactions("train", dataset)
dataset = filter_satisfaction(dataset)

mp.train(dataset)
gp.train(dataset)

test_dataset = read_dataset("data_test.csv")
plot_satisfactions("test", test_dataset)

mr = mp.predict(test_dataset)
gr = gp.predict(test_dataset)

eval(test_dataset, mr, gr)
