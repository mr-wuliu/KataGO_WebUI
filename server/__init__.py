from flask import Flask, render_template, g
from server.blueprint import test, analysis
from engine import katago, config

def create_app():
    app = Flask(__name__)
    register_blueprint(app)
    register_config(app)

    return app

def register_blueprint(app: Flask) ->None:
    """
    加载蓝图
    """
    app.register_blueprint(test.bp,url_prefix='/test')
    app.register_blueprint(analysis.bp, url_prefix='/ays')

def register_extension(app: Flask) ->None:
    pass


def register_config(app: Flask) ->None:
    app.config["KATAGO"] = katago()