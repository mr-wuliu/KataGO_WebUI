
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
    blakc_player = StringField("Black Player")



bp = Blueprint('home', __name__, url_prefix='/home')

@bp.route('/',methods=['GET','POST'])
@login_required
def home():
    """主页面"""
    return render_template('index.html')


@bp.route('/create',methods=['GET','POST'])
@login_required
def create():
    form = NewBoard(request.form)
    if request.method == 'POST':
        game = BaseGame()
        game.black_name = form.blakc_player.data
        game.white_name = form.white_player.data
        game.komi = 6.5
        game.rule = 'chinese'
        
        go_hist = GoHist(user=current_user, game_data=game.toJSON())
        db.session.add(go_hist)
        db.session.commit()


        return jsonify({"msg":"create board successsfuly"}), 201
    else:
        return jsonify({"msg":"Methods is illagle"}), 502


