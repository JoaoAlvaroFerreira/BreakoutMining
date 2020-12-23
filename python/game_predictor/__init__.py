from game_predictor.models import model_train, model_predict


class GamePredictor:
    def __init__(self, model_name):
        self.model_name = model_name

    def train(self, csv_path):
        self.model = model_train(self.model_name, csv_path)

    def predict(self, csv_path):
        return model_predict(self.model_name, self.model, csv_path)
