from sgfmill.ascii_boards import boards
import sgfmill.ascii_boards
from engine.config import Action, Move, Board, Color
from typing import List
def displayboard(board, moves):
    # 展示初始面板
    for color, move in moves:
        if move != "pass":
            row, col = move
            board.play(row, col, color)
    print(sgfmill.ascii_boards.render_board(board)) # ascii初始棋盘


from engine.game import BaseGame

    
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
    

if __name__ == '__main__':
    test2()