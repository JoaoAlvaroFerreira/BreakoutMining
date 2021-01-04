<<<<<<< HEAD
=======
import pandas as pd
import numpy as np
import itertools

from pathlib import Path
from sklearn.model_selection import train_test_split

radius = 10
min_satisfaction = 14


def read_dataset(dataset_name):
    folder_path = Path(".")

    file_path = str(folder_path / dataset_name)
    dataset = pd.read_csv(file_path, sep=";", low_memory=False).dropna()

    return dataset


def filter_satisfaction(dataset):
    filtered_dataset = dataset
    print("Started Filtering data")
    for i, j in itertools.combinations(dataset.index, 2):
        row_i = dataset.loc[i]
        row_j = dataset.loc[j]

        if (i not in filtered_dataset.index or j not in filtered_dataset.index):
            continue

        dist = np.linalg.norm(row_i - row_j)

        if (row_i['satisfaction'] < min_satisfaction):
            filtered_dataset = filtered_dataset.drop(i)
        elif (dist <= radius and row_j['satisfaction'] < min_satisfaction):
            filtered_dataset = filtered_dataset.drop(i)

        if (row_j['satisfaction'] < min_satisfaction):
            filtered_dataset = filtered_dataset.drop(j)
        elif (dist <= radius and row_i['satisfaction'] < min_satisfaction):
            filtered_dataset = filtered_dataset.drop(j)

    return filtered_dataset


>>>>>>> 7df7389e78b6085313a9d71321ceb1d85becff6a
def features_selector(dataset):
    dataset_features = dataset[["playerAPM", "playerReactionTime",
                                "playerPaddleSafety", "type of personality"]]

    return dataset_features


def labels_selector(dataset):
    labels_features = dataset[[
        "brick height", "paddle speed", "ball speed", "paddle length", "ball size"]]

    return labels_features


def data_preparation_train(dataset):
    X_train = features_selector(dataset)
    y_train = labels_selector(dataset)

    return X_train, y_train


def data_preparation_predict(dataset):
    X_pred = features_selector(dataset)

    return X_pred
