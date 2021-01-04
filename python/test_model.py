from shutil import copyfile

from game_predictor import GamePredictor
from utils.database import read_dataset, filter_satisfaction

# copyfile('./data.csv', './data_train.csv')
# copyfile('./data.csv', './data_test.csv')

mp = GamePredictor("rf", single_output=False)
gp = GamePredictor("rf", single_output=True)

dataset = read_dataset("data_train.csv")
filtered_dataset = filter_satisfaction(dataset)

mp.train(filtered_dataset)
gp.train(filtered_dataset)

test_dataset = read_dataset("data_test.csv")

mr = mp.predict(test_dataset)
gr = gp.predict(test_dataset)

print(mr)
print(gr)
