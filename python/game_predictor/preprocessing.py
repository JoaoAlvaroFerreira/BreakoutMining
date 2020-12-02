from pathlib import Path
import pandas as pd


def read_dataset(dataset_name):
    folder_path = Path(".")

    file_path = str(folder_path / dataset_name)
    dataset = pd.read_csv(file_path, sep=";", low_memory=False)

    return dataset


def data_preparation_train(csv_path):
    X_train = ''
    y_train = ''
    X_test = ''
    y_test = ''

    return X_train, y_train, X_test, y_test


def data_preparation_test(csv_path):
    X_pred = ''

    return X_pred
