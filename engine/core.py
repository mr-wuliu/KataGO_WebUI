"""
调用katago
"""

class engine:
    def __init__(self, config) -> None:
        self.queries = {}
        self.config = config
        self.query_cpunter = 0
        self.katago_process = None
        self.base_priority = None
        self.base_priority = 0 
        pass