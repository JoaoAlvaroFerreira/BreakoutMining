from game_predictor import GamePredictor

gp = GamePredictor("svm")

gp.train("data.csv")

results = gp.predict("data_pred.csv")

print(results)
