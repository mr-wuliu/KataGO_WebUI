// SGFParser.h
#ifndef SGFPARSER_H
#define SGFPARSER_H

#include <map>
#include <string>
#include <vector>

class SGFNode {
public:
    std::map<std::string, std::vector<std::string>> properties;
    std::vector<SGFNode*> children;

    ~SGFNode();
    void addProperty(const std::string& key, const std::string& value);
};

SGFNode* parseSGF(const std::string& sgf_content, size_t* pos_ptr = nullptr);
void deleteSGFNode(SGFNode* node); // Helper function for memory cleanup

#endif // SGFPARSER_H
