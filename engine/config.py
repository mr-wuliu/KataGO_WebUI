from typing import List
import sgfmill.boards
class BaseConfig:
    def __init__(self) -> None:
        self.KATAGO_PATH = "KataGo/katago.exe"
        self.AYS_CONFIG_PATH = "KataGo/analysis_example.cfg"
        self.MODEL_PATH = "models/kata1-b18c384nbt-s6582191360-d3422816034.bin.gz"
        self.additional_args: List[str] = []
        self.komi : float = 6.5
        self.board: sgfmill.boards.Board = sgfmill.boards.Board(19) # 棋盘