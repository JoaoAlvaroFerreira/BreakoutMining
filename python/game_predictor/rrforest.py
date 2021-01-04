import pandas as pd
import numpy as np

from sklearn.ensemble import RandomForestRegressor
from sklearn.multioutput import MultiOutputRegressor
from sklearn.metrics import mean_squared_error
from math import sqrt


def forest_train(X_train, y_train, single_output=False):
    rfregressor = RandomForestRegressor()

    if single_output:
        rfregressor = MultiOutputRegressor(rfregressor)

    rfregressor.fit(X_train, y_train)

    return rfregressor


def forest_predict(X_pred, model):
    y_pred = model.predict(X_pred)

    return y_pred
