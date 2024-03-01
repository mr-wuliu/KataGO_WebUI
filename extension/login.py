from flask_login import LoginManager

login_manager = LoginManager()  # 实例化扩展类

@login_manager.user_loader
def load_user(id):
    from extension.database import User
    user = User.query.get(int(id))
    return user  # 返回用户对象

# 返回的登录页面
login_manager.login_view = 'auth.login' # type: ignore
