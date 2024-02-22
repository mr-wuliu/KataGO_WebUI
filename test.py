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

def test1():
    go = BaseGame()
    actions :List[Action] = [('b',(0,0)),('w',(1,0)),('b',(1,1)),('w',(0,1))]
    actions2 :List[Action] = [('b',(3,3)),('w',(3,4)),('w',(4,3)),('w',(2,3)),('w',(3,2))]
    for action in actions2:
        go._play_single_move(action)
    go.print_board()
    print(go.get_sequence())
    go.new_branch(('b',(15,15)))
    for action in actions:
        go._play_single_move(action)
    go.print_board()
    print(go.get_sequence())
    
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
    go.print_tree(go.move_tree)

def test3():
    go =BaseGame()
    base: List[Action] = [('b',(0,0)),('w',(1,0)),('b',(1,1)),('w',(0,1)),('b',(2,0)),('w',(2,1))]
    actions : List[Action] = [('w',(14, 14)),('b',(15,15)),('w',(15,16))]
    actions2 : List[Action] = [('w',(13,12)), ('b',(15,17)),('w',(15,4))]
    go.play_move(base)
    go.new_branch(('b',(6,6)))
    go.show_branch()
    go.print_tree(go.move_tree)
    go.switch_branch(1)
    go.play_move(actions)
    

if __name__ == '__main__':
    test3()