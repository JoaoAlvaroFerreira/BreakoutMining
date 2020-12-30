from shutil import copyfile

from game_predictor import GamePredictor

copyfile('./data.csv', './data_train.csv')
copyfile('./data.csv', './data_test.csv')

gp = GamePredictor("knn")

gp.train("data_train.csv")

results = gp.predict("data_test.csv")

print(results)
