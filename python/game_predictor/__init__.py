from models import model_train, model_predict


class GamePredictor:
    def __init__(self, model_name):
        self.model_name = model_name

    def train(self, csv_path):
      
      self.model = model_train()