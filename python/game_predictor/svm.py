import numpy as np
import matplotlib.pyplot as plt
import pandas as pd

from sklearn.preprocessing import StandardScaler
from sklearn.svm import SVR
from sklearn.multioutput import MultiOutputRegressor
from sklearn.metrics import mean_squared_error, confusion_matrix
from math import sqrt


def svm_train(X_train, y_train, X_test, y_test):
    svregressor = MultiOutputRegressor(SVR(gamma='auto'))
    svregressor.fit(X_train, y_train)

    print(f"Score: {svregressor.score(X_train, y_train)}")

    return svregressor


def svm_predict(X_test, y_test, model):
    y_pred = model.predict(X_test)

    print(f"Error: {sqrt(mean_squared_error(y_test, y_pred))}")

    return y_pred
