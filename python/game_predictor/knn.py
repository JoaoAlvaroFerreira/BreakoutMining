import numpy as np
import matplotlib.pyplot as plt
import pandas as pd

from sklearn.preprocessing import StandardScaler
from sklearn.neighbors import KNeighborsRegressor
from sklearn.metrics import mean_squared_error, confusion_matrix
from math import sqrt


def knn_train(X_train, y_train, X_test, y_test):
    aucs = []

    # Calculating error for K values between 1 and 40
    for i in range(1, 2):
        knn = KNeighborsRegressor(n_neighbors=i)
        knn.fit(X_train, y_train)
        pred_i = knn.predict(X_test)
        fpr, tpr, _thresholds = roc_curve(y_test, pred_i, pos_label=2)
        aucs.append(auc(fpr, tpr))

    best_k = aucs.index(max(aucs)) + 1

    knn = KNeighborsRegressor(n_neighbors=best_k)
    knn.fit(X_train, y_train)

    y_pred = knn.predict(X_test)

    fpr, tpr, _thresholds = roc_curve(y_test, y_pred, pos_label=2)
    print(auc(fpr, tpr))

    print(f"Error: {sqrt(mean_squared_error(y_test, y_pred))}")
    print(f"Best K: {best_k}")

    return knn


def knn_predict(X_pred, model):
    print(X_pred)
    y_pred = model.predict(X_pred)

    return y_pred
