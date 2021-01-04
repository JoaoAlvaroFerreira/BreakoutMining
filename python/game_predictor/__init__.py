from game_predictor.models import model_train, model_predict


class GamePredictor:
    def __init__(self, model_name, single_output=False):
        self.model_name = model_name
        self.single_output = single_output

    def train(self, dataset):
        self.model = model_train(self.model_name, dataset, self.single_output)

    def predict(self, dataset):
        return model_predict(self.model_name, self.model, dataset, self.single_output)
