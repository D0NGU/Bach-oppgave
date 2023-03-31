import numpy as np
import matplotlib.pyplot as plt

temp = []
with open("corners.csv") as my_file:
    lines = my_file.readlines()
    for line in lines[:-1]:
        if len(line) > 1:
            a = line.replace("(", "")
            b = a.replace(")", "")
            c = b.replace("\n", "")
            rows = c.split(',')
            temp.append(rows)
    # work on last
    file_name = lines[-1]

x_points = []
y_points = []

for point in temp:
    x_points.append(point[0])
    y_points.append(point[1])

x = np.asarray(x_points, dtype=float)
y = np.asarray(y_points, dtype=float)

# "Gaussian" heatmap
heatmap, xedges, yedges = np.histogram2d(x, y, bins=(50, 50))
extent = [xedges[0], xedges[-1], yedges[0], yedges[-1]]

plt.clf()
plt.imshow(heatmap.T, extent=extent, origin='lower', cmap='jet')
plt.savefig(file_name+"_heatmap.png", bbox_inches='tight')
plt.show()

# Scatter map
plt.scatter(x, y, alpha=0.5)
# plt.yticks(np.arange(x[0], x[1], 1))
# plt.xticks(np.arange(y[0], y[1], 1))
plt.show()

""" # Bubble chart
# create data
x = np.random.rand(40)
y = np.random.rand(40)
z = np.random.rand(40)

# use the scatter function
plt.scatter(x, y, s=z*1000, c="red", alpha=0.5)

# show the graph
plt.show()

 """
