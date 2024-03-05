from flask import (
    Blueprint, current_app, jsonify, request,
    session, render_template, g
)
from server.services.katago_service import KataGO
from flask_login import login_required
from extension import BaseGame, Action, Board
from flask_wtf import FlaskForm
from wtforms import StringField, IntegerField
from wtforms import validators
from extension import db, GoHist, User
from extension import BaseGame

class LoadBoard(FlaskForm):
    id = IntegerField("id",[validators.DataRequired()])

bp = Blueprint('analysis',__name__, url_prefix='/asy')

# 定义一个函数，用于设置 g 对象的值
def set_global_Game(var):
    g.var = var

# 定义一个函数，用于获取 g 对象的值
def get_global_var():
    return g.var

@bp.route('/')
def home():
    return render_template('go/go.html')

@bp.route('/sample')
def sample():

    return current_app.config['KATAGO'].print()

@bp.route('/show',methods=['GET','POST'])
def show():
    """输入对局名称, 在数据库中搜索对应的对局, 插入到html中返回
    给前端

    :return _type_: _description_
    """
    load_form = LoadBoard(request.form)
    if request.method == 'POST' and load_form.validate():
        game_id = load_form.id.data
        game = GoHist.query.get(game_id)
        if game:
            return render_template('go/go.html', game_date=game.game_date)
        else :
            # 如果未找到对应的对局，返回错误消息
            return render_template('error.html', message='Game not found')
    # 如果是GET请求或表单验证失败，返回带有表单的模板
    return render_template('go/go.html', form=load_form)

@bp.route('/action',methods=['POST'])
@login_required
def doaction():
    if request.method == 'POST':
        go : BaseGame = g.go
        action_data = request.json
        assert action_data is not None
        action : Action = ( action_data.get('color'), action_data['move'])
        go.play_move(action)

        return jsonify({'message': 'Action processed successfully'})
    else:
        return jsonify({'message':'wrong method'}) , 501


@bp.route('/board', methods=['GET'])
@login_required
def getBoard():
    if request.method == 'GET':
        go : BaseGame = session['go']
        print(go.print_tree)
        return go.getJSONdate()
    else:
        return jsonify({'msg': 'ERROR method'}), 501
