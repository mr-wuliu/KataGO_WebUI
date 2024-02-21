from typing import Any, Union, Literal, Tuple, List, Dict
from engine import Color, Move, Action, Board

class GoNode:
    def __init__(self,board, action: Action=None, parent=None) -> None:
        self.action = action
        self.parent = parent
        self.children = []
        self.board: Board = board
        
    
    def add_child(self, action : Action, board: Board) :
        child_node = GoNode(action=action,parent=self, board=board)
        self.children.append(child_node)
        return child_node
    
    def get_sequence(self):
        """
        获取当前分支行棋序列
        """
        node, sequence = self, []
        while node is not None and node.action is not None:
            sequence.append(node.action)
            node = node.parent
        return sequence[::-1] # 反转序列
    

class BaseGame:
    def __init__(self, board_size=19) -> None:
        self.board_size:int = board_size
        self.move_tree:GoNode = GoNode(board=[[0 for _ in range(board_size)] for _ in range(board_size)])
        self.current_node :GoNode = self.move_tree
        self.current_color = 1 # 1 for black and 2 for white , start with black

    def play_move(self, action: Action) -> bool:
        # 落子判断
        if action == None: return False
        if action[1] != "pass":
            x, y = action[1] # type: ignore
        # 合法性检查
        if not (0 <= x < self.board_size and 0 <= y < self.board_size):
            return False
        elif self.current_node.board[x][y] != 0 :
            return False
        
        # 提子
        new_borard: Board = [row[:] for row in self.current_node.board] # copy the current board state
        new_borard[x][y] = self.current_color # 
        new_borard = self.capture_stones(x, y, new_borard)
        
        new_node = self.current_node.add_child(action=action, board=new_borard)
        self.current_node = new_node
        self.switch_player()
        return True
    
    def capture_stones(self, x: int, y:int, board:Board)-> Board:
        """检查所有没提掉的子
        :param int x: _description_
        :param int y: _description_
        :param List[List[int]] board: _description_
        """
        opponent = 3 - self.current_color # 先提对方
        for nx, ny in self.get_neighbors(x,y):
            # 如果该点是对方落子, 且无气, 提掉
            if board[nx][ny] == opponent and not self.has_liberty(nx, ny, board,set() ):
                board = self.remove_group(nx, ny, board)
        return board
    
    def get_neighbors(self,x :int , y: int):
        """获取(x, y)位置的所有邻居（上下左右）。"""
        directions = [(-1, 0), (1, 0), (0, -1), (0, 1)]
        return [(x + dx, y + dy) for dx, dy in directions if 0 <= x + dx < self.board_size and 0 <= y + dy < self.board_size]
    
    def has_liberty(self, x: int, y:int,board: Board, visited):
        if (x,y) in visited: # prevent infinite recursion
            return False
        visited.add((x,y))
        for nx, ny in self.get_neighbors(x, y):
            if board[nx][ny] == 0:
                return True # An empty spot means there's a liberty
            elif board[nx][ny] == board[x][y] and self.has_liberty(nx, ny, board,visited):
                return True
        return False
    def remove_group(self, x: int, y: int, board: Board):
        """提掉和(x,y)相连的所有同色棋子

        :param int x: _description_
        :param int y: _description_
        :param Board board: _description_
        """
        stack = [(x,y)]
        color = board[x][y]
        while stack:
            cx, cy = stack.pop()
            board[cx][cy] = 0 # 提子
            for nx, ny in self.get_neighbors(cx, cy):
                if board[nx][ny] == color and (nx, ny) not in stack:
                    stack.append((nx, ny))
        return board
        
    def switch_branch(self, index: int):
        # 切换分支
        if 0 <= index < len(self.current_node.children):
            self.current_node = self.current_node.children[index]
            self.current_color = 3 - self.current_node
            self.switch_player()
        else :
            print("Invalid branch index")
    
    def switch_player(self):
        self.current_color = 3 - self.current_color

