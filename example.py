import json
import sgfmill
import sgfmill.ascii_boards
import sgfmill.boards
import time
import subprocess
from typing import Any, Union, Literal, Tuple, List, Dict
from threading import Thread

# 定义操作类型
Color = Union[Literal["b"],Literal["w"]]
Move = Union[None, Literal["pass"],Tuple[int, int]]

# 配置
# config = {"katago_path":"katrain/KataGo/katago.exe",
#           "type":"analysis",
#           "config_path":"katrain/KataGo/analysis_example.cfg",
#           "model_path":"katrain/models/kata1-b18c384nbt-s6582191360-d3422816034.bin.gz"}

class BaseConfig:
    def __init__(self) -> None:
        self.katago_path:str = "KataGo/katago.exe"
        self.config_path:str= "KataGo/analysis_example.cfg"
        self.model_path:str = "models/kata1-b18c384nbt-s6582191360-d3422816034.bin.gz"
        self.additional_args: List[str] = []
        self.komi : float = 6.5 # 贴目
        self.board:sgfmill.boards.Board = sgfmill.boards.Board(19) # 棋盘
        


        
class KataGO(BaseConfig):
    def __init__(self,moves:List[Tuple[Color,Move]] ):
        super().__init__()
        self.query_counter:int = 0
        self.moves = moves
        self.displayboard(self.board.copy())

        katago = subprocess.Popen(
            [self.katago_path,"analysis","-config",self.config_path,"-model",self.model_path,*self.additional_args],
            stdin=subprocess.PIPE,
            stdout=subprocess.PIPE,
            stderr=subprocess.PIPE,
        )
        self.katago = katago

        def printforever():
            while katago.poll() is None:
                # 有输出时一直打印
                data = katago.stderr.readline()
                time.sleep(0)
                if data:
                    print("KataGO: ",data.decode(), end="")
            data = katago.stderr.read()
            if data:
                print("KataGp: ", data.decode(), end="")
        self.stderrthread = Thread(target=printforever)
        self.stderrthread.start()

    
    def displayboard(self, board):
        # 展示初始面板
        for color, move in self.moves:
            if move != "pass":
                row, col = move
                board.play(row, col, color)
        print(sgfmill.ascii_boards.render_board(board)) # ascii初始棋盘

    def close(self):
        self.katago.stdin.close()

    def query(self, max_visits: int = None)->Dict:
        """
        查询分析结果
        """
        initial_board = self.board
        moves = self.moves

        query = {}
        query["id"] = str(self.query_counter)
        self.query_counter += 1
        
        query["moves"] = [(color, self.sgfmill_to_str(move)) for color, move in moves]
        query["initialStones"] = []
        for y in  range(initial_board.side):
            for x in range(initial_board.side):
                color = initial_board.get(y,x)
                if color:
                    query["initialStones"].append((color, self.sgfmill_to_str((y,x))))
        query["rules"] = "Chinese"
        query["komi"] = self.komi
        query["boardXSize"] = initial_board.side
        query["boardYSize"] = initial_board.side
        query["includePolicy"] = True
        query['maxVisits']=200
        if max_visits is not None:
            query["maxVisits"] = max_visits
        return self.query_raw(query)
    
    def query_raw(self, query: Dict[str, Any])-> dict:
        self.katago.stdin.write((json.dumps(query) + "\n").encode())
        self.katago.stdin.flush()

        line = ""
        while line == "" :
            if self.katago.poll():
                time.sleep(1)
                raise Exception("Unexpected katago exit")
            line = self.katago.stdout.readline()
            line = line.decode().strip()
            with open("output.json", "a", encoding="utf-8") as file:  # 打开文件以追加模式
                print(line, file=file)
                
        response = json.loads(line)
        return response
    

    @staticmethod
    def sgfmill_to_str(move: Move) -> str:
        if move is None:
            return "pass"
        if move == "pass":
            return "pass"
        (y,x) = move
        return "ABCDEFGHJKLMNOPQRSTUVWXYZ"[x] + str(y+1)

    
def main():

    moves = [("b",(3,3)), ("w",(15,15))] #,("b",(16,16)),("w",(16,15)),("b",(15,16))]
    # 启动
    katago = KataGO(moves)
    # 分析一局棋盘
    print("Query result: ")
    katago.query()
    time.sleep(10)
    katago.close()

if __name__ == '__main__':
    main()
    


