# very incomplete
class Button(object):
    CLICK = "click"
    PRESS = "press"
    RELEASE = "release"

    alist = ['click']

class TableColumnHeader(Button):
    pass

class RadioButton(Button):
    pass

class ComboBox(object):
    PRESS = "press"

class Entry(object):
    ACTIVATE = "activate"

class Expander(object):
    ACTIVATE = "activate"

class OptionMenu(object):
    PRESS = "press"

class Range(object):
    ACTIVATE = "activate"

class TreeView:
    EXPAND_OR_CONTRACT = "expand or contract"


