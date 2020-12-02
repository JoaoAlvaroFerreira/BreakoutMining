from knn import knn_train, knn_predict
from preprocessing import data_preparation_train, data_preparation_test


def model_train(model_name, csv_path):
    train_switcher = {
        "knn": knn_train
    }

    trainer = train_switcher.get(model_name, knn_train)
    X_train, y_train, X_test, y_test = data_preparation_train

    return trainer(X_train, y_train, X_test, y_test)


def model_predict(model_name, csv_path):
    predict_switcher = {
        "knn": knn_predict
    }

    predictor = predict_switcher.get(model_name, knn_predict)
    X_pred = data_preparation_test(csv_path)

    return predictor(X_pred)
