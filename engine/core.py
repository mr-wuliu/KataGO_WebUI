
from typing import Any, Union, Literal, Tuple, List, Dict
from engine.config import BaseConfig
import subprocess
from engine.game import GoNode
from typing import Any, Union, Literal, Tuple, List, Dict, Optional
from engine.config import Color, Move, Action, Board

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
        
    def query():
        
    




        
    def print(self):
        return "::katag::"