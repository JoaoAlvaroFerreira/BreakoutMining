from pathlib import Path
import pandas as pd


def read_dataset(dataset_name):
    folder_path = Path(".")

    file_path = str(folder_path / dataset_name)
    dataset = pd.read_csv(file_path, sep=";", low_memory=False)

    return dataset


def filter_satisfaction(dataset):
    return dataset


def features_selector(dataset):
    dataset_features = dataset[["playerAPM", "playerReactionTime",
                                "playerPaddleSafety", "type of personality"]]

    return dataset_features


def labels_selector(dataset):
    labels_features = dataset[["brick height", "paddle speed", "ball speed"]]

    return labels_features


def data_preparation_train(csv_path):
    dataset = read_dataset(csv_path)

    filtered_datatset = filter_satisfaction(dataset)

    X_train = features_selector(filtered_datatset)
    y_train = labels_selector(filtered_datatset)

    return X_train, y_train


def data_preparation_predict(csv_path):
    dataset = read_dataset(csv_path)

    X_test = features_selector(dataset)
    y_test = labels_selector(dataset)

    return X_test, y_test
