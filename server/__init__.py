from flask import Flask
from server.blueprint import test, analysis
from extension.Go_engine.core import katago
from extension import db, migrate

def create_app():
    app = Flask(__name__)
    app.config.from_pyfile('../config.py')
    register_blueprint(app)
    register_config(app)

    return app

def register_config(app: Flask) ->None:
    app.config["KATAGO"] = katago()

def register_blueprint(app: Flask) ->None:
    """
    加载蓝图
    """
    app.register_blueprint(test.bp,url_prefix='/test')
    app.register_blueprint(analysis.bp, url_prefix='/ays')

def register_database(app : Flask) -> None:
    db.init_app(app)
    migrate.init_app(app,db)

def register_extension(app: Flask) ->None:
    pass

    