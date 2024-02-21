from flask import Flask, render_template
from .blueprint import test, analysis

def create_app():
    app = Flask(__name__)
    register_blueprint(app)
    

    return app

def register_blueprint(app: Flask):
    """
    加载蓝图
    """
    app.register_blueprint(test.bp,url_prefix='/test')
    app.register_blueprint(analysis.bp, url_prefix='/analysis')
