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


def features_selector(dataset):
    dataset_features = dataset[["playerAPM", "playerReactionTime",
                                "playerPaddleSafety", "type of personality"]]

    return dataset_features


def labels_selector(dataset):
    labels_features = dataset[[
        "brick height", "paddle speed", "ball speed", "paddle length", "ball size"]]

    return labels_features


def data_preparation_train(csv_path):
    dataset = read_dataset(csv_path)

    filtered_dataset = filter_satisfaction(dataset)

    train, test = train_test_split(
        filtered_dataset, test_size=0.25, random_state=42, shuffle=True)

    X_train = features_selector(train)
    y_train = labels_selector(train)
    X_test = features_selector(test)
    y_test = labels_selector(test)

    return X_train, y_train, X_test, y_test


def data_preparation_predict(csv_path):
    dataset = read_dataset(csv_path)

    X_pred = features_selector(dataset)

    return X_pred
