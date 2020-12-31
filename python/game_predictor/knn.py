from sklearn.neighbors import KNeighborsRegressor
from sklearn.metrics import mean_squared_error
from math import sqrt


def knn_train(X_train, y_train):
    knn = KNeighborsRegressor(n_neighbors=2)
    knn.fit(X_train, y_train)

    return knn


def knn_predict(X_test, y_test, model):
    y_pred = model.predict(X_test)

    print(f"Error: {sqrt(mean_squared_error(y_test, y_pred))}")

    return y_pred
