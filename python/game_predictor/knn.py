from sklearn.neighbors import KNeighborsRegressor
from sklearn.metrics import mean_squared_error
from math import sqrt


def knn_train(X_train, y_train, X_test, y_test):
    errors = []

    # Calculating error for K values between 1 and 40
    for i in range(1, 4):
        knn = KNeighborsRegressor(n_neighbors=i)
        knn.fit(X_train, y_train)
        pred_i = knn.predict(X_test)
        errors.append(mean_squared_error(y_test, pred_i))

    best_k = errors.index(min(errors)) + 1

    knn = KNeighborsRegressor(n_neighbors=best_k)
    knn.fit(X_train, y_train)

    y_pred = knn.predict(X_test)

    print(f"Error: {sqrt(mean_squared_error(y_test, y_pred))}")
    print(f"Best K: {best_k}")

    return knn


def knn_predict(X_pred, model):
    y_pred = model.predict(X_pred)

    return y_pred
