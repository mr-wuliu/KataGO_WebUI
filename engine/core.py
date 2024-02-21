
from typing import Any, Union, Literal, Tuple, List, Dict
from engine.config import BaseConfig
import subprocess
from engine.game import GoNode
# 定义操作
Color = Union[Literal['b'], Literal['w']]
Move = Union[None, Literal['pass'], Tuple[int, int]]



class katago(BaseConfig):
    def __init__(self) -> None:
        self.queries = {}
        self.query_cpunter = 0
        self.katago_process = None
        self.base_priority = 0 
        self.katago = subprocess.Popen(
            [self.KATAGO_PATH, "analysis",
             "-config", self.AYS_CONFIG_PATH,
             "-model", self.MODEL_PATH,
             *self.additional_args],
             stdin=subprocess.PIPE,
             stdout=subprocess.PIPE,
             stderr=subprocess.PIPE,
        )
        self.moves = GoNode()
    
    def close(self):
        self.katago.stdin.close()
    def move(self, move: Tuple[Color,Move]) -> None:
        self.moves.add_child(move)
    def get_sequence(self):
        return self.moves.get_sequence()
    




        
    def print(self):
        return "::katag::"