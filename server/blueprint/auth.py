from flask import (
    Blueprint, jsonify, request, flash, render_template, redirect, session, url_for
)
from extension import User
from flask_wtf import FlaskForm
from wtforms import StringField, PasswordField, validators, SubmitField
from wtforms.validators import DataRequired
from extension import db
from flask_login import login_user


bp = Blueprint('auth', __name__, url_prefix='/user')

class RegistrationForm(FlaskForm):
    username = StringField('Username', [validators.Length(min=4, max=25)])
    email = StringField('Email Address', [validators.Length(min=6, max=35)])
    password = PasswordField('New Password', [
        validators.DataRequired(),
        validators.EqualTo('confirm', message='Passwords must match')
    ])
    confirm = PasswordField('Repeat Password')

class LoginForm(FlaskForm):
    username = StringField('Username', validators=[DataRequired()])
    password = PasswordField('Password', validators=[DataRequired()])
    submit = SubmitField('Login')

@bp.route('/register', methods=['GET','POST'])
def register():
    form = RegistrationForm(request.form)
    if request.method == 'POST' and form.validate():
        user = User(username=form.username.data, password=form.email.data)
        user.password = form.password.data
        if User.query.filter_by(username=form.username.data).first():
            return 'User {} is already registered.'.format(form.username.data)

        db.session.add(user)
        db.session.commit()
        return jsonify({"message": "User registered successfully"}), 201
    elif request.method == 'GET':
        # GET请求
        form = RegistrationForm()
        return render_template('register.html', form=form)
    else:
        return jsonify({"message": "Methods that are not supported"}), 502



@bp.route('/login', methods=['GET', 'POST'])
def login():
    if request.method == 'POST':
        username = request.form['username']
        password = request.form['password']

        if not username or not password:
            flash('Invalid input.')
            return redirect(url_for('user'))

        user = User.query.first()
        # 验证用户名和密码是否一致
        if user is None:
            return redirect(url_for('login'))         
        if username == user.username and user.verify_password(password):
            login_user(user)  # 登入用户
            flash('Login success.')
            return redirect(url_for('home.home'))  # 重定向到主页

        flash('Invalid username or password.')  # 如果验证失败，显示错误消息
        return redirect(url_for('auth.login'))  # 重定向回登录页面
    return render_template('login.html', form=LoginForm())

@bp.route('/logout', methods=['GET', 'POST'])
def logout():
    # 注销
    session.clear()
    return redirect(url_for('home.home'))