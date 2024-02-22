from __future__ import annotations
from typing import Any, Union, Literal, Tuple, List, Dict, Optional
from engine.config import Color, Move, Action, Board


class GoNode:
    def __init__(self,board, parent: GoNode=None) -> None: # type: ignore
        self.action : Action = None # 声明一个变量
        self.parent :GoNode= parent
        self.children = []
        self.board: Board = board
        
    
    def add_child(self, action : Action, board: Board) :
        child_node = GoNode(parent=self, board=board)
        child_node.action = action
        child_node.add_action(action)
        self.children.append(child_node)
        return child_node
        
    def add_action(self, action: Action):
        self.action = action
    def del_chile(self, index:int):
        x: int = len(self.children)
        if x <= index:
            return False
        del self.children[index]
        
    def get_sequence(self):
        """
        获取当前分支行棋序列
        """
        node: GoNode= self
        sequence = []
        while node.action  is not None:
            sequence.append(node.action)
            node = node.parent
        return sequence[::-1] # 反转序列
    def get_parent(self) -> GoNode:
        return self.parent
    

class BaseGame:
    def __init__(self, board_size=19) -> None:
        self.board_size = board_size
        self.move_tree:GoNode = GoNode(board=[[0 for _ in range(board_size)] for _ in range(board_size)])
        self.current_node : GoNode = self.move_tree
        self.current_color: int = 1 # 1 for black and 2 for white , start with black

    def play_move(self, actions: Action | List[Action])->bool:
        if isinstance(actions, list):
            # 深拷贝当前的棋盘状态
            temp_node = self.current_node
            temp_board = [row[:] for row in self.current_node.board]
            temp_color = self.current_color
            success = True

            for action in actions:
                if not self._play_single_move(action):
                    success = False
                    break
            if not success:
                self.current_node = temp_node
                self.current_color = temp_color
                self.current_node.board = temp_board
            return success
        else:
            self._play_single_move(actions)

    def _play_single_move(self, action: Action) -> bool:
        '''
        action == ('b',(0,0)) 位于棋盘左下角
        '''
        # 落子判断
        if action == None: return False
        assert action != 'head'
        if action[1] != "pass":
            x, y = action[1]
        # 合法性检查
        if not (0 <= x < self.board_size and 0 <= y < self.board_size):
            return False
        elif self.current_node.board[x][y] != 0 :
            return False
        '''
        打劫和提子
        '''
        # copy the current board state
        new_borard: Board = [row[:] for row in self.current_node.board] 
        self.current_color = 1 if action[0]== 'b' else 2
        new_borard[x][y] = self.current_color
        new_borard = self._capture_stones(x, y, new_borard)
        last = self.current_node.parent
        if last is not None and last.board == new_borard:
                # 打劫
                return False
        if self.current_node.board == new_borard:
            # 禁着点
            return False
        new_node = self.current_node.add_child(action=action, board=new_borard)
        self.current_node = new_node

        return True    

    def _capture_stones(self, x: int, y:int, board:Board)-> Board:
        """检查所有没提掉的子
        :param int x: _description_
        :param int y: _description_
        :param List[List[int]] board: _description_
        """
        opponent = 3 - self.current_color # 先提对方
        for nx, ny in self._get_neighbors(x,y):
            # 如果该点是对方落子, 且无气, 提掉
            if board[nx][ny] == opponent and not self._has_liberty(nx, ny, board,set() ):
                board = self._remove_group(nx, ny, board)
        return board

    def _get_neighbors(self,x :int , y: int):
        """获取(x, y)位置的所有邻居（上下左右）。"""
        directions = [(-1, 0), (1, 0), (0, -1), (0, 1)]
        return [(x + dx, y + dy) for dx, dy in directions if 0 <= x + dx < self.board_size and 0 <= y + dy < self.board_size]
    
    def _has_liberty(self, x: int, y:int,board: Board, visited):
        if (x,y) in visited: # prevent infinite recursion
            return False
        visited.add((x,y))
        for nx, ny in self._get_neighbors(x, y):
            if board[nx][ny] == 0:
                return True # An empty spot means there's a liberty
            elif board[nx][ny] == board[x][y] and self._has_liberty(nx, ny, board,visited):
                return True
        return False
    
    def _remove_group(self, x: int, y: int, board: Board):
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
            for nx, ny in self._get_neighbors(cx, cy):
                if board[nx][ny] == color and (nx, ny) not in stack:
                    stack.append((nx, ny))
        return board
    
    def get_sequence(self):
        return self.current_node.get_sequence()
    
    def print_board(self):
        column_labels = "ABCDEFGHJKLMNOPQRST"[:self.board_size]  # 跳过"I"

        for y in range(self.board_size, 0, -1):  # 从上到下打印行
            row = f"{y:3d} "  # 行号，保持对齐
            for x in range(self.board_size):
                stone = self.current_node.board[x][y-1]  # 获取棋子状态
                if stone == 0:
                    row += ".  "  # 空点
                elif stone == 1:
                    row += "#  "  # 黑子
                elif stone == 2:
                    row += "o  "  # 白子
            print(row)
        print("   ", "  ".join(column_labels), end='\n')  # 打印列标签
    
    def new_branch(self, action : Action)->bool:
        """新建一个分支

        :param Action action: 行为
        :return bool: 是否新建成功
        """
        if self.current_node.get_parent() == None:
            return False
        self.current_node = self.current_node.get_parent()
        self._play_single_move(action)
        return True
    def show_branch(self) -> bool | List[GoNode]:
        if self.current_node.parent is None:
            return False
        branch:List[GoNode] = self.current_node.parent.children
        for node in branch:
            print(node.action)
        return True
        
    def switch_branch(self, index: int):
        # 切换分支
        if 0 <= index < len(self.current_node.children):
            self.current_node = self.current_node.children[index]
            self.current_color = 3 - self.current_color
        else :
            print("Invalid branch index")
    def del_current_branch(self):
        if self.current_node.action == 'head' or self.current_node is None:
            return False
        partent = self.current_node.parent
        del self.current_node
        self.current_node = partent
        

    def print_tree(self, node: GoNode, depth=0):
        # 打印当前节点的信息，可以根据需要调整输出的格式
        if node.action is not None:
            print('    ' * depth + str(node.action))
        else:
            print('    ' * depth + "Root")
        for child in node.children:
            self.print_tree(child, depth + 1)
    def _switch_player(self):
        self.current_color = 3 - self.current_color 