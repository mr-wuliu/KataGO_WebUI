// SGFParser.cpp
#include "SGFParser.h"
#include <pybind11/pybind11.h>
#include <pybind11/stl.h> 
#include <cctype>
#include <iostream>

namespace py = pybind11;

SGFNode::~SGFNode() {
    for (auto child : children) {
        delete child;
    }
}

void SGFNode::addProperty(const std::string& key, const std::string& value) {
    properties[key].emplace_back(value);
}

SGFNode* parseSGF(const std::string& sgf_content, size_t* pos_ptr) {
    size_t internal_pos = 0;
    if (!pos_ptr) {
        pos_ptr = &internal_pos;
    }
    size_t& pos = *pos_ptr;

    auto node = new SGFNode();
    while (pos < sgf_content.length()) {
        std::cout << "Current pos: " << pos << ", char: " << sgf_content[pos] << std::endl;
        while (pos < sgf_content.length() && std::isspace(sgf_content[pos])) {
            pos++;
        }
        char c = sgf_content[pos];

        if (c == ';') {
            pos++; // Skip the ';' to start parsing properties
            while (pos < sgf_content.length() && sgf_content[pos] != '(' && sgf_content[pos] != ')') {
                std::string key;
                // Collect the entire property key
                while (pos < sgf_content.length() && std::isupper(sgf_content[pos])) {
                    key += sgf_content[pos++];
                }
                // Now pos should be at '[' starting the value
                if (sgf_content[pos] == '[') {
                    std::string value;
                    pos++; // Skip '['
                    while (pos < sgf_content.length() && sgf_content[pos] != ']') {
                        value += sgf_content[pos++];
                    }
                    pos++; // Skip ']'
                    node->addProperty(key, value);
                }
            }
        } else if (c == '(') {
            pos++;
            node->children.push_back(parseSGF(sgf_content, pos_ptr));
        } else if (c == ')') {
            pos++;
            break; // End of this node
        } else {
            pos++; // Skip any other characters
        }
    }
    return node;
}



void deleteSGFNode(SGFNode* node) { // Implement if needed for explicit cleanup in Python
    delete node;
}

PYBIND11_MODULE(sgfparser, m) {
    py::class_<SGFNode>(m, "SGFNode")
        .def(py::init<>()) // 默认构造函数
        .def("addProperty", &SGFNode::addProperty, "Adds a property to the SGFNode.")
        .def_readwrite("properties", &SGFNode::properties, "A dictionary of SGF properties.")
        .def_readwrite("children", &SGFNode::children, "List of child SGFNodes.");

    m.def("parse_sgf", &parseSGF, py::arg("sgf_content"), py::arg("pos_ptr") = nullptr,
          "Parses SGF content and returns the root SGFNode.");
}