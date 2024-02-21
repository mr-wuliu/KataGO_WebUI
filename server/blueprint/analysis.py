from flask import Blueprint
from ..services.katago_service import KataGO
bp = Blueprint('analysis',__name__)

@bp.route('/')
def home():
    return "analysis"

@bp.route('/sample')
def sample():
    moves = [("b",(3,3)), ("w",(15,15)), ("b",(16,16)),("w",(16,15)),("b",(15,16))]
    katago = KataGO(moves)
    text = katago.query()
    print(text)
    katago.close()
    return text

