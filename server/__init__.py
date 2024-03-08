from flask import Flask, redirect, url_for

from server.blueprint import test, analysis, auth, home
from extension.Go_engine.core import katago
from extension import db, migrate, login_manager


def create_app():
    app = Flask(__name__)
    app.config.from_pyfile('../config.py') # 配置

    register_blueprint(app) # 蓝图
    register_database(app) # 数据库
    register_extension(app) # 登录验证


    return app

def register_blueprint(app: Flask) ->None:
    """
    加载蓝图
    """
    app.register_blueprint(test.bp)
    app.register_blueprint(auth.bp)
    app.register_blueprint(analysis.bp)
    app.register_blueprint(home.bp)
    @app.route('/')
    def index():
        return redirect(url_for('home.home'))

def register_database(app : Flask) -> None:
    db.init_app(app)
    migrate.init_app(app,db)

def register_extension(app:Flask)->None:
    login_manager.init_app(app)