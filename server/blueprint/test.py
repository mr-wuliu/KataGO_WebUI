from flask import Blueprint


bp = Blueprint('test',__name__)

@bp.route('/',methods=['GET','POST'])
def test():
    return "HELLO!"