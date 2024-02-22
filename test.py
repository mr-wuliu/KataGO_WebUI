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


if __name__ == '__main__':
    go = BaseGame()
    actions :List[Action] = [('b',(0,0)),('w',(1,0)),('b',(1,1)),('w',(0,1))]
    actions2 :List[Action] = [('b',(3,3)),('w',(3,4)),('w',(4,3)),('w',(2,3)),('w',(3,2))]
    for action in actions2:
        go.play_move(action)
    go.print_board()
    print(go.get_sequence())
    go.new_branch(('b',(15,15)))
    for action in actions:
        go.play_move(action)
    go.print_board()
    print(go.get_sequence())
    
    
