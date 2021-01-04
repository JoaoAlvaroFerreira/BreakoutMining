import pandas as pd
import matplotlib.pyplot as plt


def plot_satisfactions(name, dataframe):
    satisfactions = dataframe['satisfaction']

    out = pd.cut(satisfactions, bins=[0, 2, 4, 6, 8,
                                      10, 12, 14, 16, 18, 20], include_lowest=True)
    ax = out.value_counts(sort=False).plot.bar(
        rot=0, color="b", figsize=(12, 4))

    plt.savefig(f"{name}_satisfactions.png")
    plt.close()
