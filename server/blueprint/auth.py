from werkzeug.security import check_password_hash, generate_password_hash
from flask import (
    Blueprint, jsonify, request, flash, render_template
)
from models.User import User
from flask_wtf import FlaskForm
from wtforms import StringField, PasswordField, validators,Form,BooleanField
from extension import db

bp = Blueprint('auth', __name__, url_prefix='/auth')

class RegistrationForm(Form):
    username = StringField('Username', [validators.Length(min=4, max=25)])
    email = StringField('Email Address', [validators.Length(min=6, max=35)])
    password = PasswordField('New Password', [
        validators.DataRequired(),
        validators.EqualTo('confirm', message='Passwords must match')
    ])
    confirm = PasswordField('Repeat Password')
    accept_tos = BooleanField('I accept the TOS', [validators.DataRequired()])


@bp.route('/register', methods=['GET','POST'])
def register():
    form = RegistrationForm(request.form)
    if request.method == 'POST' and form.validate():
        user = User(form.username.data, form.email.data)
        user.password = form.password.data
        if User.query.filter_by(username=form.username.data).first():
            return 'User {} is already registered.'.format(form.username.data)

        db.session.add(user)
        db.session.commit()
        return jsonify({"message": "User registered successfully"}), 201
    elif request.method == 'GET':
        # GET请求
        return jsonify({"message": "Please submit the registration form."}), 200
    else:
        return jsonify({"message": "User registered failure"}), 502
