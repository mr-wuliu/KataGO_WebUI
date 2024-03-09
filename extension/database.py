from flask_sqlalchemy import SQLAlchemy
from flask_migrate import Migrate
from flask_login import UserMixin, login_manager

from werkzeug.security import generate_password_hash, check_password_hash
import datetime
db = SQLAlchemy()
migrate = Migrate()

class User(db.Model, UserMixin):
    __tablename__ = 'users'
    id = db.Column(db.Integer, primary_key=True)
    username = db.Column(db.String(50), unique=True, nullable=False)
    email = db.Column(db.String(120), unique=True)
    _password  = db.Column(db.String(128),nullable=False)

    go_hists = db.relationship("GoHist", back_populates="user")

    @property
    def password(self):
        return self._password
    
    @password.setter
    def password(self, password):
        self._password = generate_password_hash(password)

    def verify_password(self, password):
        return check_password_hash(self._password, password)
    

class GoHist(db.Model):
    __tablename__ = 'go_hist'
    id = db.Column(db.Integer, primary_key=True)
    create_at = db.Column(db.DateTime, default=datetime.datetime.now)
    play_datetime = db.Column(db.DateTime)
    game_name = db.Column(db.String(64),nullable=False) # 对局可以重名, 根据id查找
    game_data = db.Column(db.Text, nullable=False)
    # Other fields for GoHist
    user_id = db.Column(db.Integer, db.ForeignKey('users.id'))
    user = db.relationship("User", back_populates="go_hists")
