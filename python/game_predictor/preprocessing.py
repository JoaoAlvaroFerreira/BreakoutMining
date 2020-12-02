from pathlib import Path
import pandas as pd
from sklearn.model_selection import train_test_split


def read_dataset(dataset_name):
    folder_path = Path(".")

    file_path = str(folder_path / dataset_name)
    dataset = pd.read_csv(file_path, sep=",", low_memory=False)

    return dataset


def features_selector(dataset):
    dataset_features = dataset[["playerAPM", "playerReactionTime",
                                "playerPaddleSafety", "type of personality", "satisfaction"]]

    return dataset_features


def labels_selector(dataset):
    labels_features = dataset[["brick height", "paddle speed", "ball speed"]]

    return labels_features


def data_preparation_train(csv_path):
    dataset = read_dataset(csv_path)

    train, test = train_test_split(
        dataset, test_size=0.25, random_state=42, shuffle=True)

    X_train = features_selector(train)
    y_train = labels_selector(train)
    X_test = features_selector(test)
    y_test = labels_selector(test)

    return X_train, y_train, X_test, y_test


def data_preparation_predict(csv_path):
    dataset = read_dataset(csv_path)

    X_pred = features_selector(dataset)

    X_pred['satisfaction'] = 205329

    return X_pred
