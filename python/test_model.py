from shutil import copyfile

from game_predictor import GamePredictor

copyfile('./data.csv', './data_train.csv')
copyfile('./data.csv', './data_test.csv')

mp = GamePredictor("rf", single_output=False)
gp = GamePredictor("rf", single_output=True)

mp.train("data_train.csv")
gp.train("data_train.csv")

mr = mp.predict("data_test.csv")
gr = gp.predict("data_test.csv")

print(mr)
print(gr)
