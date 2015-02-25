TO USE:

Place a blank object in the inspector and attach the "VoxelTerrain.cpp" script. 
grid size is the length and the height of the object
The different levels are to indicate where to place said colors
needs a shader that supports vertex colors. The one I used for the images is included place this in "Part Material"
Water Material is the material to be used for the water mesh

TODO make the noise scale to the size of the island... currently it's set up for a grid size of ~50 modify these lines of code:

newY = Noise(x+seed,y+seed,10,3,2);
newY += Noise(x+seed,y+seed,2,1,1);