import build.Debug.sgfparser as sgfparser

# 假设你的SGF文件路径
sgf_file_path = 'test.sgf'

# 从文件读取SGF内容
with open(sgf_file_path, 'r', encoding='utf-8') as file:
    sgf_content = file.read()
# print(sgf_content)
# 使用你的C++库解析SGF内容
root_node = sgfparser.parse_sgf(sgf_content)
print(root_node.properties)
# 输出解析结果，现在应该能正确显示完整的属性名
for child in root_node.children:
    print(child.properties)

sgfparser.print_branches(root_node)
