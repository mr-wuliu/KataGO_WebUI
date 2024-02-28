
from typing import Any, Union, Literal, Tuple, List, Dict
from extension.Go_engine.go_config import BaseConfig
import subprocess
<<<<<<< HEAD:engine/core.py
from engine.game import GoNode
from typing import Any, Union, Literal, Tuple, List, Dict, Optional
from engine.config import Color, Move, Action, Board
=======
from extension.Go_engine.game import GoNode
from extension.Go_engine.go_config import Color, Board, Move

>>>>>>> a1736f79c9f51cccdf73d123eb1a843ab147378a:extension/Go_engine/core.py

class katago(BaseConfig):
    def __init__(self) -> None:
        super().__init__()
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
<<<<<<< HEAD:engine/core.py
        
    def query():
        
=======
    def close(self):
        self.katago.stdin.close()
>>>>>>> a1736f79c9f51cccdf73d123eb1a843ab147378a:extension/Go_engine/core.py
    
    
