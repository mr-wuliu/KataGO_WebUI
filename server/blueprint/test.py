from flask import Blueprint
from extension import User
from extension.database import db
from flask import jsonify


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
    users = User.query.all()
    user_list = [{"id": user.id, "username": user.username, "email": user.email, "password":user.password} for user in users]

    return jsonify(user_list)
