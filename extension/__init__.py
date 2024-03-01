from extension.database import db
from extension.database import migrate
from extension.Go_engine.core import katago
from extension.Go_engine.game import BaseGame
from extension.login import login_manager
from extension.database import User, GoHist

# 引入类型
from extension.Go_engine.go_config import Action, Board, Move, Color
