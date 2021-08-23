mkdir build_win32 & pushd build_win32
cmake -G "Visual Studio 16 2019" -ENABLE_JIT=ON ..
popd
cmake --build build_win32 --config Release
copy /Y build_win32\Release\slua.dll ..\Assets\Plugins\Slua\x86\slua.dll

mkdir build_win64 & pushd build_win64
cmake -G "Visual Studio 16 2019" -A "x64" -ENABLE_JIT=ON ..
popd
cmake --build build_win64 --config Release
copy /Y build_win64\Release\slua.dll ..\Assets\Plugins\Slua\x64\slua.dll