import numpy as np
import pandas as pd
import itertools

from pathlib import Path


radius = 10
min_satisfaction = 14


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


def read_dataset(dataset_name):
    folder_path = Path(".")

    file_path = str(folder_path / dataset_name)
    dataset = pd.read_csv(file_path, sep=";", low_memory=False).dropna()

    return dataset
