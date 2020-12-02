import numpy as np
import matplotlib.pyplot as plt
import pandas as pd

from sklearn.preprocessing import StandardScaler
from sklearn.neighbors import KNeighborsRegressor
from sklearn.metrics import classification_report, confusion_matrix, roc_auc_score
from sklearn import metrics


def knn_train(X_train, y_train, X_test, y_test):
    auc = []

    # Calculating error for K values between 1 and 40
    for i in range(1, 40):
        knn = KNeighborsRegressor(n_neighbors=i)
        knn.fit(X_train, y_train)
        pred_i = knn.predict(X_test)
        auc.append(roc_auc_score(y_test, pred_i))

    best_k = auc.index(max(auc)) + 1

    fpr, tpr, _thresholds = metrics.roc_curve(y_test, pred_i)
    print(f"AUC: {metrics.auc(fpr, tpr)}")

    knn = KNeighborsRegressor(n_neighbors=best_k)
    knn.fit(X_train, y_train)

    y_pred = knn.predict(X_test)

    print(str(confusion_matrix(y_test, y_pred)))
    print(str(classification_report(y_test, y_pred, zero_division=0)))
    print(f"AUC: {roc_auc_score(y_test, y_pred)}")
    print(f"Best K: {best_k}")

    return knn


def knn_predict(X_pred, model):
    y_pred = model.predict(X_pred)

    return y_pred
