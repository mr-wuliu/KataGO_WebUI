from engine.core import katago

from typing import Any, Union, Literal, Tuple, List, Dict

# 定义操作
Color = Union[Literal["b"],Literal["w"]]
Move = Union[Literal["pass"],Tuple[int, int]]
Action = Union[None, Tuple[Color,Move]]
Board = List[List[int]]
