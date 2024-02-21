from sgfmill.ascii_boards import boards
from typing import Any, Union, Literal, Tuple, List, Dict
import sgfmill.ascii_boards
import copy
Color = Union[Literal["b"],Literal["w"]]
Move = Union[None, Literal["pass"],Tuple[int, int]]

def displayboard(board, moves):
    # 展示初始面板
    for color, move in moves:
        if move != "pass":
            row, col = move
            board.play(row, col, color)
    print(sgfmill.ascii_boards.render_board(board)) # ascii初始棋盘

board: boards.Board = boards.Board(19) # 棋盘
moves:List[Tuple[Color,Move]] = [('b',(1,1)),('w',(1,2)),('b',(1,3)),('w',(2,3)),('b',(2,2)),('w',(1,4)),('b',(0,2)),('w',(0,3)),('b',(15,15)),('w',(1,2)),('b',(1,3)),('w',(1,2)),('b',(1,3))]
displayboard(board.copy(), moves)

class test:
    def __init__(self) -> None:
        self.name = "hello"
    def change(self, name):
        self.name = name
    def print(self):
        print(self.name)

a = test()
a.print()
a.change("two")
a.print()
b = copy.copy(a)
b.change('b')
b.print()
a.print()


c = test()
c.print()
def change(some:test):
    some.change("daw")
change(c)
c.print()