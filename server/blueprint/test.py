from flask import Blueprint
from extension import User
from extension.database import db
from flask import jsonify
from flask_login import current_user


bp = Blueprint('test',__name__,url_prefix='/test')

@bp.route('/add',methods=['GET','POST'])
def test():
    # user =  User.query.all()
    # user = User()
    # user.username = "bad"
    # user.password = "admin"
    # db.session.add(user)
    # db.session.commit()
    # user = User(username="new_noe")
    # user.password="admin"
    # db.session.add(user)
    # db.session.commit()
    # users = User.query.all()
    users = User.query.all()

    # user_list = [{"id": user.id, "username": user.username, "email": user.email, "password":user.password} for user in users]
    user_games = current_user.go_hists
    # 遍历打印所有对局的游戏数据
    for game in user_games:
        print(game.game_data)

    return jsonify("success")
