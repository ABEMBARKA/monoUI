##############################################################################
# Written by:  Brian G. Merrell <bgmerrell@novell.com>
# Date:        09/22/2008
# Description: A file where generic functions that help with testing can be
#              placed.  This file should be used to avoid duplicating the same
#              function over and over in the application wrappers.
#              
##############################################################################

# The docstring below  is used in the generated log file
"""
A file where generic functions that help with testing can be placed.  This file
should be used to avoid duplicating the same function over and over in the
application wrappers.
"""

import states
import actions
from strongwind import *

# check actions
def actionsCheck(accessible, control, invalid_actions=[],
                                            add_actions=[], pause=True):
    """Check the actions of an accessible using the default actions
    of the accessible (specified by control class in actions.py) as
    the default expected actions.
   
    Keyword arguments:
    accessible -- the accessible whose actions will be checked
    control -- the class name of the control whose actions we want to check

    """
    procedurelogger.action('Check %s\'s actions' % accessible)

    # get list of expected actions 
    actions_list = actions.__getattribute__(control).actions

    expected_actions = \
              [s for s in actions_list if s not in invalid_actions]
    expected_actions = set(expected_actions).union(set(add_actions))

    # get list of actual actions of the accessible
    qa = accessible._accessible.queryAction()
    actual_actions = [qa.getName(i) for i in range(qa.nActions)]

    procedurelogger.expectedResult('Actions: %s' % actual_actions)

    # get a list of actual states that are missing or extraneous
    missing_actions = set(expected_actions).difference(set(actual_actions))
    extra_actions = set(actual_actions).difference(set(expected_actions))

    # if missing_actions and extra_actions are empty, the test case passes
    # otherwise, throw an exception
    is_same = len(missing_actions) == 0 and len(extra_actions) == 0
    assert is_same, "\n  %s: %s\n  %s: %s" %\
                                         ("Missing actual actions: ",
                                           missing_actions,
                                          "Extraneous actual actions: ",
                                           extra_actions)

# check states
def statesCheck(accessible, control, invalid_states=[],
                                            add_states=[], pause=True):
    """Check the states of an accessible using the default states
    of the accessible (specified by control class in states.py) as
    the default expected states.
   
    Keyword arguments:
    accessible -- the accessible whose states will be checked
    control -- the class name of the control whose states we want to check
    invalid_states -- a list of states that should be removed from the
    list of default expected states
    add_states -- a list of states that should be added to the list of
    default expected states
    pause -- whether the function should sleep for a config.SHORT_DELAY seconds
    before continuing.  Useful when waiting for states to settle after
    performing an action.
    """
    procedurelogger.action('Check %s\'s states' % accessible)
    # give states a chance to settle
    if pause:
        sleep(config.SHORT_DELAY)
    # create a list of all states for button except "sensitive"
    states_list = states.__getattribute__(control).states
    expected_states = \
              [s for s in states_list if s not in invalid_states]
    expected_states = set(expected_states).union(set(add_states))

    procedurelogger.expectedResult('States:  %s' % expected_states)

    # get a list of all actual states for accessible
    actual_states = accessible._accessible.getState().getStates()
    # need to convert the numbers retrieved above into their associated
    # strings
    actual_states = [pyatspi.stateToString(s) for s in actual_states]

    # assert there are no elements in expected_states that are not
    # in actual_states
    missing_states = set(expected_states).difference(set(actual_states))

    # assert there are no elements in actual_states that are not
    # in expected_states
    extra_states = set(actual_states).difference(set(expected_states))

    is_same = len(missing_states) == 0 and len(extra_states) == 0
    assert is_same, "\n  %s: %s\n  %s: %s" %\
                                         ("Missing actual states: ",
                                           missing_states,
                                          "Extraneous actual states: ",
                                           extra_states)

# check Text is updated
def assertText(accessible, expected_text):
    """
    Make sure the accessible's text is expected
    """
    procedurelogger.expectedResult('update %s\'s text to "%s"' % \
                          (accessible._accessible.getRoleName(), expected_text))

    actual_text = accessible.text
    assert actual_text == expected_text, \
                                "actual text is %s, expected text is %s" % \
                                (actual_text, expected_text)

# check Name
def assertName(accessible, expected_name):
    """
    Check the name of accessible
    """
    procedurelogger.expectedResult("%s's name is update to %s" % \
                         (accessible._accessible.getRoleName(), expected_name))

    actual_name = accessible.name
    assert actual_name == expected_name, \
                                "actual name is %s, expected name is %s" % \
                                (actual_name, expected_name)

# check image size is expected
def assertImageSize(accessible, expected_width=-1, expected_height=-1):
    """
    Make sure the accessible's image size has expected width and height
    """
    procedurelogger.action("assert %s's image size" % accessible)
    size = accessible._accessible.queryImage().getImageSize()

    procedurelogger.expectedResult('"%s" image size is %s x %s' %
                                (accessible, expected_width, expected_height))

    assert expected_width == size[0], "%s (%s), %s (%s)" %\
                                            ("expected width",
                                              expected_width,
                                             "does not match actual width",
                                              size[0])
    assert expected_height == size[1], "%s (%s), %s (%s)" %\
                                            ("expected height",
                                              expected_height,
                                             "does not match actual height",
                                              size[1])
    
def quit(accessible):
    """
    Close the application
    """
    accessible.altF4()


