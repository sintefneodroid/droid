LIBPYTORCH_PATH=/home/heider/Applications/libtorch/
mkdir build
cd build
cmake -DCMAKE_PREFIX_PATH=$LIBPYTORCH_PATH ..
cmake --build . --config Release
#make
cd ..

