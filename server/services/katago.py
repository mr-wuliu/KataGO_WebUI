import json
import sgfmill
import sgfmill.ascii_boards
import sgfmill.boards
from typing import Any, Union, Literal, Tuple, List, Dict
import subprocess
# 定义操作类型
Color = Union[Literal['b'], Literal['w']]
Move = Union[None, Literal['pass'],Tuple[int, int]]

class BaseConfig:
    def __inti__(self) -> None:
        
        self.KATAGO_PATH = "KataGo/katago.exe"
        self.CONFIG_PATH = "KataGo/analysis_example.cfg"
        self.MODEL_PATH = "models/kata1-b18c384nbt-s6582191360-d3422816034.bin.gz"
        self.additional_args: List[str] = []

class KataGO(BaseConfig):
    def __init__(self) -> None:
        super().__init__()
        self.query_counter: int = 0
        self.moves: List[Tuple[Color, Move]] = []
        
        katago = subprocess.Popen(
            [self.KATAGO_PATH,"analysis",
             "-config",self.CONFIG_PATH,
             "-model",self.MODEL_PATH,
             *self.additional_args],
            stdin=subprocess.PIPE,
            stdout=subprocess.PIPE,
            stderr=subprocess.PIPE,
        )
        self.katago = katago

    def close(self):
        self.katago.stdin.close()
    def move(self, action: List[Tuple[Color, Move]]):
        self.moves.append(action)