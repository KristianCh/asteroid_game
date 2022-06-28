from PIL import ImageOps, ImageFilter
import numpy as np
import scipy.ndimage
import scipy.misc
from scipy import ndimage
from PIL import Image
from matplotlib import cm
import imageio
import argparse
import os
import time

im = imageio.imread("square_aoe.png")
height = im.shape[0]
width = im.shape[1]
new = Image.new("RGBA", (width, height))
new_arr = np.asarray(im)

for i in range(0, height):
    for j in range(0, width):
        if (new_arr[i][j][3] != 0):
            print(new_arr[i][j][3])
