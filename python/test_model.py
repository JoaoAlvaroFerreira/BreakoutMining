from game_predictor import GamePredictor

gp = GamePredictor("knn")

gp.train("data.csv")

results = gp.predict("data_pred.csv")

print(results)
