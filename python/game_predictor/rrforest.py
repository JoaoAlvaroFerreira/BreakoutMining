import pandas as pd
import numpy as np

from sklearn.ensemble import RandomForestRegressor
from sklearn.multioutput import MultiOutputRegressor
from sklearn.metrics import mean_squared_error
from math import sqrt


def forest_train(X_train, y_train, X_test, y_test, single_output=False):
    rfregressor = RandomForestRegressor()

    if single_output:
        rfregressor = MultiOutputRegressor(rfregressor)

    rfregressor.fit(X_train, y_train)

    y_pred = rfregressor.predict(X_test)

    print(f"MSE: {sqrt(mean_squared_error(y_test, y_pred))}")

    return rfregressor


def forest_predict(X_pred, model):
    y_pred = model.predict(X_pred)

    return y_pred
