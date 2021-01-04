from sklearn.metrics import mean_squared_error
from math import sqrt


def eval(rl_dataset, mo_pred, so_pred):
    rl_pred = rl_dataset[[
        "brick height", "paddle speed", "ball speed", "paddle length", "ball size"]]

    temp_dataset = rl_dataset.drop(columns=[
        "brick height", "paddle speed", "ball speed", "paddle length", "ball size"])

    so_pred.columns = so_pred.columns.map(lambda x: str(x) + '_single')

    rl_dataset = temp_dataset.join(rl_pred)

    merged = rl_dataset.join(mo_pred, lsuffix='_rl',
                             rsuffix='_multi').join(so_pred)

    comparison = merged[merged['satisfaction'] >= 14]

    comparison.to_csv("results_comparison.csv")

    filtered_rl_pred = comparison[[
        "brick height_rl", "paddle speed_rl", "ball speed_rl", "paddle length_rl", "ball size_rl"]]

    filtered_mo_pred = comparison[[
        "brick height_multi", "paddle speed_multi", "ball speed_multi", "paddle length_multi", "ball size_multi"]]

    filtered_so_pred = comparison[[
        "brick height_single", "paddle speed_single", "ball speed_single", "paddle length_single", "ball size_single"]]

    print(
        f"Multi-Ouput Error: {sqrt(mean_squared_error(filtered_rl_pred, filtered_mo_pred))}")
    print(
        f"Single-Ouput Error: {sqrt(mean_squared_error(filtered_rl_pred, filtered_so_pred))}")

    return comparison
