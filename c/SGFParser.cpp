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

struct returnBody
{
    std::string key;
    size_t pos;
};

/**
 * 连续读取字符, 以生成属性
*/
returnBody readProperty(const std::string& sgf_content, size_t pos) {
    returnBody result;
    std::string key;
    while ( pos < sgf_content.length()& sgf_content[pos] != '[') {
        // 如果是大写字符
        if (std::isupper(sgf_content[pos])) {
            key += sgf_content[pos];
        }
        pos++;
    }
    result.pos = pos;
    result.key = key;
    return result;
}

SGFNode* parseSGFM(const std::string& sgf_content) {
    size_t pos = 0;
    

    return parseSGFR(sgf_content, pos);

}

SGFNode* parseSGFR(const std::string& sgf_content, size_t pos) {

    auto node = new SGFNode();
    while ( pos < sgf_content.length()) {
        char current = sgf_content[pos];
        if ( current == ';') {
            pos++;
            std::string key;
            std::string value;
            returnBody resKey = readProperty(sgf_content, pos);
            pos += resKey.pos;
            key = resKey.key;
            returnBody resVal = readProperty(sgf_content, pos);
            pos += resVal.pos;
            key = resKey.key;

            // 添加属性
            node->addProperty(key, value);
        }
        else if (current == '(') {
            pos++;
            node->children.push_back( parseSGFR(sgf_content, pos));
        } else if ( current == ')') {
            pos++;
            break;
        }
    }
    return nullptr;
}

void SGFNode::addProperty(const std::string& key, const std::string& value) {
    properties[key].emplace_back(value);
}

// 解析SGF内容，支持带有分支的SGF文件
SGFNode* parseSGF_r(const std::string& sgf_content, size_t pos = 0) {
    auto node = new SGFNode(); // 创建一个新的SGF节点

    while (pos < sgf_content.length()) {
        char c = sgf_content[pos];
        
        if (c == ';') { // 遇到';'，开始解析一个属性
            pos++;
            std::string key;
            while (pos < sgf_content.length() && sgf_content[pos] != '[') {
                if (std::isupper(sgf_content[pos])) {
                    key += sgf_content[pos];
                }
                pos++;
            }

            if (sgf_content[pos] == '[') {
                pos++; // 跳过'['
                std::string value;
                while (pos < sgf_content.length() && sgf_content[pos] != ']') {
                    value += sgf_content[pos];
                    pos++;
                }
                pos++; // 跳过']'
                node->addProperty(key, value);
                std::cout << "add P" << std::endl;
            }
        } else if (c == '(') { // 遇到'('，表示一个分支的开始
            pos++;
            node->children.push_back(parseSGF_r(sgf_content, pos)); // 递归解析分支
        } else if (c == ')') { // 遇到')'，表示分支的结束
            pos++;
            break; // 退出当前分支的解析
        } else {
            pos++; // 跳过其他字符
        }
        std::cout << pos << sgf_content[pos]<< std::endl;
    }

    return node;
}

SGFNode* parseSGF(const std::string& sgf_content) {
    return parseSGF_r(sgf_content, 0);
}

// 打印所有分支的辅助函数
void printBranches(const SGFNode* node, const std::string& indent) {
    // 首先打印当前节点的属性
    for (const auto& prop : node->properties) {
        for (const auto& val : prop.second) {
            std::cout << indent << prop.first << ": " << val << std::endl;
        }
    }

    // 递归打印所有子节点（分支）
    for (const auto& child : node->children) {
        printBranches(child, indent + "  ");
    }
}

PYBIND11_MODULE(sgfparser, m) {
    py::class_<SGFNode>(m, "SGFNode")
        .def(py::init<>()) // 默认构造函数
        .def("addProperty", &SGFNode::addProperty, "Adds a property to the SGFNode.")
        .def_readwrite("properties", &SGFNode::properties, "A dictionary of SGF properties.")
        .def_readwrite("children", &SGFNode::children, "List of child SGFNodes.");

    m.def("parse_sgf", &parseSGF, py::arg("sgf_content"),
          "Parses SGF content and returns the root SGFNode.");
    // 在PYBIND11_MODULE中注册print_branches函数
    m.def("print_branches", &printBranches, 
        py::arg("node"), py::arg("indent") = "",
        "Prints all branches of a given SGFNode starting with an optional indent.");
}