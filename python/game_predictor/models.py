from game_predictor.preprocessing import data_preparation_train, data_preparation_predict
from game_predictor.knn import knn_train, knn_predict
from game_predictor.svm import svm_train, svm_predict


def model_train(model_name, csv_path):
    train_switcher = {
        "knn": knn_train,
        "svm": svm_train
    }

    trainer = train_switcher.get(model_name, knn_train)
    X_train, y_train, X_test, y_test = data_preparation_train(csv_path)

    return trainer(X_train, y_train, X_test, y_test)


def model_predict(model_name, model, csv_path):
    predict_switcher = {
        "knn": knn_predict,
        "svm": svm_predict
    }

    predictor = predict_switcher.get(model_name, knn_predict)
    X_pred = data_preparation_predict(csv_path)

    return predictor(X_pred, model)
