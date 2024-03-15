import sys
import os

# 添加父目录到sys.path
parent_dir = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
sys.path.append(parent_dir)

from sgfmill.ascii_boards import boards
import sgfmill.ascii_boards
from extension.Go_engine.go_config import Action, Move, Board, Color
from typing import List
def displayboard(board, moves):
    # 展示初始面板
    for color, move in moves:
        if move != "pass":
            row, col = move
            board.play(row, col, color)
    print(sgfmill.ascii_boards.render_board(board)) # ascii初始棋盘


from extension.Go_engine.game import BaseGame

    
def test2():
    go = BaseGame()
    base: List[Action] = [('b',(0,0)),('w',(1,0)),('b',(1,1)),('w',(0,1)),('b',(2,0)),('w',(2,1))]
    actions : List[Action] = [('b',(0,0)),('w',(1,0))]
    go.play_move(base)
    go.print_board()
    print(go.get_sequence())
    result = go.play_move(actions)
    go.print_board()
    print(go.get_sequence())
    print(result)
    go.print_tree()

def test3():
    go =BaseGame()
    base: List[Action] = [('b',(0,0)),('w',(1,0)),('b',(1,1)),('w',(0,1)),('b',(2,0)),('w',(2,1))]
    actions : List[Action] = [('w',(14, 14)),('b',(15,15)),('w',(15,16))]
    actions2 : List[Action] = [('w',(13,12)), ('b',(15,17)),('w',(15,4))]
    go.play_move(base)
    go.new_branch(('b',(6,6)))
    go.print_tree()
    go.play_move(actions)
    print("$$$$$$$$$$$")
    go.print_tree()

def test4():
    go=BaseGame()
    base: List[Action] = [('b',(0,0)),('w',(1,0)),('b',(1,1)),('w',(0,1)),('b',(2,0)),('w',(2,1))]
    actions : List[Action] = [('w',(14, 14)),('b',(15,15)),('w',(15,16))]
    actions2 : List[Action] = [('w',(13,12)), ('b',(15,17)),('w',(15,4))]
    go.play_move(base)
    go.new_branch(('b',(6,6)))
    go.print_tree()
    
    print(go.show_branch())
def test5():
    go = BaseGame()
    base: List[Action] = [('b',(0,0)),('w',(1,0)),('b',(1,1)),('w',(0,1)),('b',(2,0)),('w',(2,1))]
    actions : List[Action] = [('w',(14, 14)),('b',(15,15)),('w',(15,16))]
    actions2 : List[Action] = [('w',(13,12)), ('b',(15,17)),('w',(15,4))]

    go.play_move(base)
    go.print_tree()
    print(go.get_sequence())
    go.reback(len(base)-2)
    go.new_branch(actions)
    print(go.get_sequence())
    go.print_tree()

def test6():
    go=BaseGame()
    base = [('b',(0,0)),('w',(1,1))]
    go.play_move(base)
    go.print_tree()
    # 游标回退
    go.reback(1)
    go.print_tree()
    go.play_move(('w',(1,2)))
    go.print_tree()

def test7():
    go=BaseGame()
    base: List[Action] = [('b',(0,0)),('w',(1,0)),('b',(1,1)),('w',(0,1)),('b',(2,0)),('w',(2,1))]
    actions : List[Action] = [('w',(14, 14)),('b',(15,15)),('w',(15,16))]
    actions2 : List[Action] = [('w',(14,14)), ('b',(15,17)),('w',(15,4))]
    go.play_move(base)
    go.reback(len(base)-1)
    go.play_move(actions)
    go.reback(len(actions)-1)
    go.play_move(actions2)
    go.print_tree()
    print(go.toJSON())

def test8():
    go=BaseGame()
    base: List[Action] = [('b',(0,0)),('w',(1,0)),('b',(1,1)),('w',(0,1)),('b',(2,0)),('w',(2,1))]
    go.play_move(base)
    go.reback(4)
    print(go.current_index)
    print(go.get_sequence())
    go.move_next(4)
    print(go.current_index)
    print(go.get_sequence())
    
def test9():
    go=BaseGame()
    base: List[Action] = [('b',(0,0)),('w',(1,0)),('b',(1,1)),('w',(0,1)),('b',(2,0)),('w',(2,1))]
    go.play_move(base)
    print(go.toJSON())

def testA():
    go=BaseGame()
    base: List[Action] = [('b',(0,0)),('w',(1,0)),('b',(1,1)),('w',(0,1)),('b',(2,0)),('w',(2,1))]
    go.play_move(base)
    print(go.toJSON())



def testB():
    import json
    go=BaseGame()
    base: List[Action] = [('b',(0,0)),('w',(1,0)),('b',(1,1)),('w',(0,1)),('b',(2,0)),('w',(2,1))]
    actions : List[Action] = [('w',(14, 14)),('b',(15,15)),('w',(15,16))]
    actions2 : List[Action] = [('w',(14,14)), ('b',(15,17)),('w',(15,4))]
    go.play_move(base)
    go.reback(len(base)-1)
    go.play_move(actions)
    go.reback(len(actions)-1)
    go.play_move(actions2)
    go.print_tree()
    jsondata =go.toJSON()

    def print_tree(data, parent_id=None, level=0):
        # 找到所有当前父节点的直接子节点
        children = [node for node in data["nodes"] if any(edge["from"] == parent_id and edge["to"] == node["id"] for edge in data["edges"])]
        for child in children:
            print('    ' * level + f"{child['action']} (Branch Index: {child['branch_index']})")
            # 递归打印子节点
            print_tree(data, parent_id=child["id"], level=level + 1)
    data = json.loads(jsondata)
    print_tree(data, parent_id=0)  # 假设根节点的ID为0
    
def testC():
    import json
    go=BaseGame()
    base: List[Action] = [('b',(0,0)),('w',(1,0)),('b',(1,1)),('w',(0,1)),('b',(2,0)),('w',(2,1))]
    actions : List[Action] = [('w',(14, 14)),('b',(15,15)),('w',(15,16))]
    actions2 : List[Action] = [('w',(14,14)), ('b',(15,17)),('w',(15,4))]
    go.play_move(base)
    go.reback(4)
    go.play_move(actions)
    
    go.print_tree()
    # go.print_board()

    goB = go.toJSON()
    # 从 json 构建对象
    game = BaseGame.from_json(goB)


    game.print_tree()
    game.print_board()

    print(go.current_branch)
    print(game.current_branch)
    print(go.current_color)
    print(game.current_color)
    print(go.current_node.action)
    print(game.current_node.action)

    go.reback(len(actions))
    go.play_move(actions2)
    game.reback(len(actions))
    game.play_move(actions2)
    go.print_tree()
    game.print_tree()

def parse_sgf(sgf_content):
    import re
    game = BaseGame()
    # 匹配属性和值
    properties = re.findall(r';(\w+)\[([^]]*)\]', sgf_content)
    print(properties)

    for prop, val in properties:
        print(prop)
        # 处理不同的属性
        if prop == 'B' or prop == 'W':
            print(val)
            # 添加棋子移动
            col = ord(val[0]) - ord('a')
            row = ord(val[1]) - ord('a')
            row = 19 - row
            color : Color = prop.lower() # type: ignore
            move : Move = (col, row)
            ac : Action = (color, move)
            game.play_move(ac)
        # else:
        #     print(prop)
            # 设置游戏属性
            # game.set_property(prop, val)

    return game
def testD():
    # 示例使用
    sgf_content = "(;GM[1]FF[4]SZ[19];B[pd];W[dd];B[qp];W[dp];B[cf];W[cc])"
    game = parse_sgf(sgf_content)
    game.print_tree()

if __name__ == '__main__':
    testD()
