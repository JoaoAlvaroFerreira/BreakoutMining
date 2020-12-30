import pandas as pd
import numpy as np
import itertools

from pathlib import Path

radius = 10


def read_dataset(dataset_name):
    folder_path = Path(".")

    file_path = str(folder_path / dataset_name)
    dataset = pd.read_csv(file_path, sep=";", low_memory=False)

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

        if (row_i['satisfaction'] < 13):
            filtered_dataset = filtered_dataset.drop(i)
        elif (dist <= radius and row_j['satisfaction'] < 13):
            filtered_dataset = filtered_dataset.drop(i)

        if (row_j['satisfaction'] < 13):
            filtered_dataset = filtered_dataset.drop(j)
        elif (dist <= radius and row_i['satisfaction'] < 13):
            filtered_dataset = filtered_dataset.drop(j)

    return filtered_dataset


def features_selector(dataset):
    dataset_features = dataset[["playerAPM", "playerReactionTime",
                                "playerPaddleSafety", "type of personality"]]

    return dataset_features


def labels_selector(dataset):
    labels_features = dataset[["brick height", "paddle speed", "ball speed"]]

    return labels_features


def data_preparation_train(csv_path):
    dataset = read_dataset(csv_path)

    # dataset = filter_satisfaction(dataset)

    X_train = features_selector(dataset)
    y_train = labels_selector(dataset)

    return X_train, y_train


def data_preparation_predict(csv_path):
    dataset = read_dataset(csv_path)

    X_test = features_selector(dataset)
    y_test = labels_selector(dataset)

    return X_test, y_test
