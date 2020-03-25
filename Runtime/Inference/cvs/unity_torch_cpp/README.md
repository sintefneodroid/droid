

for training: https://pytorch.org/tutorials/advanced/cpp_frontend.html

docs: https://pytorch.org/cppdocs/installing.html
To compile this, following the docs, make a build/ subdirectory and call:

mkdir build
cd build
cmake -DCMAKE_PREFIX_PATH=/absolute/path/to/libtorch ..
cmake --build . --config Release

If all goes well, you’ll get a libnetworks.so or networks.dll file that you can drop into Assets/Plugins/ in Unity.


