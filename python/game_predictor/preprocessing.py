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
