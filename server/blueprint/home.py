
from flask_login import login_required, current_user
from werkzeug.security import check_password_hash, generate_password_hash
from flask import (
    Blueprint, jsonify, request,
      flash, render_template, redirect, session, url_for,
      g
)
from extension import db, User, GoHist
from flask_wtf import FlaskForm
from wtforms import StringField, IntegerField, validators
from extension import BaseGame
from extension import Action, Move, Color

class NewBoard(FlaskForm):
    game_name = StringField("Game Name",[validators.DataRequired()])
    white_player = StringField("White Player")
    black_player = StringField("Black Player")



bp = Blueprint('home', __name__, url_prefix='/home')

@bp.route('/',methods=['GET','POST'])
@login_required
def home():
    """主页面"""
    return render_template('index.html')

@bp.route('/hist_show', methods=['GET'])
@login_required
def hist_show():
    # 获取分页参数
    page = request.args.get('page', 1, type=int)
    per_page = request.args.get('per_page', 10, type=int)  # 每页显示的记录数

    # 实现分页查询，按对局创建时间降序排列
    pagination = GoHist.query.order_by(GoHist.create_at.desc()).paginate(page=page,
                                                                          per_page=per_page, error_out=False)
    go_hists = pagination.items

    # 将对局记录转换为字典列表
    go_hists_data = [{
        'id': hist.id,
        'create_at': hist.create_at.isoformat(),  # 转换为ISO格式字符串
        'play_datetime': hist.play_datetime.isoformat() if hist.play_datetime else None,
        'game_name': hist.game_name,
        'game_data': hist.game_data,
        'user_id': hist.user_id
    } for hist in go_hists]

    return jsonify(go_hists_data)


    # 返回分页对象和对局记录到模板，用于显示
    return jsonify(go_hists)
    return render_template('hist_show.html', pagination=pagination, go_hists=go_hists)

@bp.route('/create',methods=['GET','POST'])
@login_required
def create():
    form = NewBoard(request.form)
    if request.method == 'POST':
        game = BaseGame()
        game.game_name =form.game_name.data
        game.black_name = form.black_player.data
        game.white_name = form.white_player.data
        game.komi = 6.5
        game.rule = 'chinese'
        print("$$$$$$$$$$")
        print(form.game_name.data)
        print(form.black_player.data)
        print(form.white_player.data)
        go_hist = GoHist(user=current_user,
                        game_name=game.game_name,
                        game_data=game.toJSON())
        db.session.add(go_hist)
        db.session.commit()
        return jsonify({"msg":"create board successsfuly"}), 201
    else:
        return jsonify({"msg":"Methods is illagle"}), 502


@bp.route('/hist_play', methods=['POST', 'GET'])
@login_required
def hist_play():
    page = request.args.get('page', 1, type=int)
    per_page = 10  # 每页显示的项目数量
    
    if request.method == 'GET':
        user_games = GoHist.query.filter_by(user_id=current_user.id).paginate(page, per_page, False)
        user_games = GoHist.query.filter_by(user_id=current_user.id).all()
        game_info = [{'id': game.id, 'name': game.game_name} for game in user_games]
        return jsonify(game_info)
    else:
        return jsonify('ERROR METHODS'), 501

