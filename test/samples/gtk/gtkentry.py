#!/usr/bin/env python

# example gtkentry.py

import pygtk
pygtk.require('2.0')
import gtk

class Entry:
    def delete_event(self, widget, event, data=None):
        gtk.main_quit()
        return False

    def __init__(self):
        self.window = gtk.Window(gtk.WINDOW_TOPLEVEL)
        self.window.set_title("GtkEntry widget")
        self.window.connect("delete_event", self.delete_event)
        self.window.set_border_width(12)

        vbox = gtk.VBox()
        vbox.set_spacing(6)

        self.bar = gtk.Entry()
        self.bar.set_max_length(5)
        vbox.pack_start(self.bar, False, False, 0)

        button = gtk.Button("Pulse")
        button.connect("clicked", self.pulse_clicked)
        vbox.pack_start(button, False, False, 0)

        self.window.add(vbox)

        vbox.show_all()
        self.window.show()

    def pulse_clicked(self, widget):
        self.bar.insert_text ("pulse")


def main():
    gtk.main()
    return 0       

if __name__ == "__main__":
    Entry()
    main()
