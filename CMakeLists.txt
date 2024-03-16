cmake_minimum_required(VERSION 3.6)
project(sgfparser)

add_subdirectory(extern/pybind11)
pybind11_add_module(sgfparser c/SGFParser.cpp)