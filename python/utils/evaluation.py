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

    comparison.to_csv("results_comparison.csv", sep=";", index=False)

    filtered_rl_pred = comparison[[
        "brick height_rl", "paddle speed_rl", "ball speed_rl", "paddle length_rl", "ball size_rl"]]

    filtered_mo_pred = comparison[[
        "brick height_multi", "paddle speed_multi", "ball speed_multi", "paddle length_multi", "ball size_multi"]]

    filtered_so_pred = comparison[[
        "brick height_single", "paddle speed_single", "ball speed_single", "paddle length_single", "ball size_single"]]

    print("Multi-Ouput Evaluation")
    print(
        f"Global RMSE: {sqrt(mean_squared_error(filtered_rl_pred, filtered_mo_pred))}")
    print(
        f"Brick Height RMSE: {sqrt(mean_squared_error(filtered_rl_pred['brick height_rl'], filtered_mo_pred['brick height_multi']))}")
    print(
        f"Paddle Speed RMSE: {sqrt(mean_squared_error(filtered_rl_pred['paddle speed_rl'], filtered_mo_pred['paddle speed_multi']))}")
    print(
        f"Ball Speed RMSE: {sqrt(mean_squared_error(filtered_rl_pred['ball speed_rl'], filtered_mo_pred['ball speed_multi']))}")
    print(
        f"Paddle Length RMSE: {sqrt(mean_squared_error(filtered_rl_pred['paddle length_rl'], filtered_mo_pred['paddle length_multi']))}")
    print(
        f"Ball Size RMSE: {sqrt(mean_squared_error(filtered_rl_pred['ball size_rl'], filtered_mo_pred['ball size_multi']))}")

    print("Single-Ouput Evaluation")
    print(
        f"Global RMSE: {sqrt(mean_squared_error(filtered_rl_pred, filtered_so_pred))}")
    print(
        f"Brick Height RMSE: {sqrt(mean_squared_error(filtered_rl_pred['brick height_rl'], filtered_so_pred['brick height_single']))}")
    print(
        f"Paddle Speed RMSE: {sqrt(mean_squared_error(filtered_rl_pred['paddle speed_rl'], filtered_so_pred['paddle speed_single']))}")
    print(
        f"Ball Speed RMSE: {sqrt(mean_squared_error(filtered_rl_pred['ball speed_rl'], filtered_so_pred['ball speed_single']))}")
    print(
        f"Paddle Length RMSE: {sqrt(mean_squared_error(filtered_rl_pred['paddle length_rl'], filtered_so_pred['paddle length_single']))}")
    print(
        f"Ball Size RMSE: {sqrt(mean_squared_error(filtered_rl_pred['ball size_rl'], filtered_so_pred['ball size_single']))}")

    return comparison
