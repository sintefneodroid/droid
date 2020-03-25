cmake_minimum_required(VERSION 3.0 FATAL_ERROR)
project(networks)

find_package(Torch REQUIRED)
#set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} ${TORCH_CXX_FLAGS}")

add_library(networks SHARED networks.cpp)
target_link_libraries(networks "${TORCH_LIBRARIES}")
set_property(TARGET networks PROPERTY CXX_STANDARD 11)

#if (MSVC)
#	file(GLOB TORCH_DLLS "${TORCH_INSTALL_PREFIX}/lib/*.dll")
#	add_custom_command(TARGET networks
#		POST_BUILD
#		COMMAND ${CMAKE_COMMAND} -E copy_if_different
#		${TORCH_DLLS}
#		$<TARGET_FILE_DIR:example-app>)
#endif (MSVC)
