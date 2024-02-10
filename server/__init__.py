from flask import Flask, render_template
from .blueprint import test

def create_app():
    app = Flask(__name__)
    # app.config.from_pyfile()
    register_blueprint(app)
    

    return app

def register_blueprint(app: Flask):
    """
    加载蓝图
    """
    app.register_blueprint(test.bp)
    app.add_url_rule('/', endpoint='test')