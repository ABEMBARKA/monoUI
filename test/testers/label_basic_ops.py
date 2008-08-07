#!/usr/bin/env python

##############################################################################
# Written by:  Cachen Chen <cachen@novell.com>
# Date:        08/07/2008
# Description: Test accessibility of label widget 
#              Use the labelframe.py wrapper script
#              Test the samples/button_label_linklabel.py script
##############################################################################

# The docstring below  is used in the generated log file
"""
Test accessibility of label widget
"""

# imports
import sys
import os

from strongwind import *
from label import *
from sys import argv
from os import path

app_path = None 
try:
  app_path = argv[1]
except IndexError:
  pass #expected

# open the label sample application
try:
  app = launchLabel(app_path)
except IOError, msg:
  print "ERROR:  %s" % msg
  exit(2)

sleep(config.SHORT_DELAY)

# make sure we got the app back
if app is None:
  exit(4)

# just an alias to make things shorter
lFrame = app.labelFrame

#check Label's states list
lFrame.statesCheck(lFrame.label)

#click button2 to change label text
lFrame.click(lFrame.button)
sleep(config.SHORT_DELAY)
lFrame.assertLabel('You have clicked me 1 times')

#click button2 again to change label's text value
lFrame.click(lFrame.button)
sleep(config.SHORT_DELAY)
lFrame.assertText('You have clicked me 2 times')

print "INFO:  Log written to: %s" % config.OUTPUT_DIR

#close application frame window
lFrame.quit()
