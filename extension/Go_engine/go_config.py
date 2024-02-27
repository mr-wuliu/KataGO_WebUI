from typing import Any, Union, Literal, Tuple, List, Dict  
import sgfmill.boards

class BaseConfig:
    def __init__(self) -> None:
        self.KATAGO_PATH = "extension/KataGo/katago.exe"
        self.AYS_CONFIG_PATH = "extension/KataGo/analysis_example.cfg"
        self.MODEL_PATH = "extension/KataGo/models/kata1-b18c384nbt-s6582191360-d3422816034.bin.gz"
        self.additional_args: List[str] = []
        self.komi : float = 6.5
        self.board: sgfmill.boards.Board = sgfmill.boards.Board(19) # 棋盘
    
        
# 定义操作
Color = Union[Literal["b"],Literal["w"]]
Move = Union[Literal["pass"],Tuple[int, int]]
Action = Union[None, Literal["head"] ,Tuple[Color,Move]]
Board = List[List[int]]

