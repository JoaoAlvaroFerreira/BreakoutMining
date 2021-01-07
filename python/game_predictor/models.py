import pandas as pd

from game_predictor.preprocessing import data_preparation_train, data_preparation_predict
from game_predictor.knn import knn_train, knn_predict
from game_predictor.svm import svm_train, svm_predict
from game_predictor.rrforest import forest_train, forest_predict


def model_train(model_name, dataset, single_ouput):
    train_switcher = {
        "knn": knn_train,
        "svm": svm_train,
        "rf": forest_train
    }

    trainer = train_switcher.get(model_name, forest_train)
    X_train, y_train = data_preparation_train(dataset)

    return trainer(X_train, y_train, single_ouput)


def model_predict(model_name, model, dataset, single_ouput):
    predict_switcher = {
        "knn": knn_predict,
        "svm": svm_predict,
        "rf": forest_predict
    }

    predictor = predict_switcher.get(model_name, forest_predict)
    X_pred = data_preparation_predict(dataset)

    predictions = predictor(X_pred, model)

    dataset = pd.DataFrame(predictions,
                           columns=['brick height', 'paddle speed', 'ball speed', "paddle length", "ball size"])

    model_type = "multi_output"
    if single_ouput:
        model_type = "single_output"

    dataset.to_csv(f"{model_type}_{model_name}_results.csv",
                   sep=";", index=False)

    return dataset
