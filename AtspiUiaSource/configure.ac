AC_PREREQ([2.54])
AC_INIT([AtspiUiaSource], [2.1.92])
AM_INIT_AUTOMAKE([foreign])

# pkg-config
AC_PATH_PROG(PKG_CONFIG, pkg-config, no)
if test "x$PKG_CONFIG" = "xno"; then
        AC_MSG_ERROR([You need to install pkg-config])
fi

SHAMROCK_EXPAND_LIBDIR
SHAMROCK_EXPAND_BINDIR
SHAMROCK_EXPAND_DATADIR

AC_PROG_INSTALL
AM_PROG_LIBTOOL

AC_PATH_PROG(MONO, mono, no)
if test "x$MONO" = "xno"; then
        AC_MSG_ERROR([mono Not found])
fi
AC_SUBST(MONO)

AC_PATH_PROG(GMCS, mcs, no)
if test "x$GMCS" = "xno"; then
        AC_MSG_ERROR([mcs Not found])
fi

AC_PATH_PROG(GACUTIL, gacutil, no)
if test "x$GACUTIL" = "xno" ; then
	AC_MSG_ERROR([No gacutil tool found])
fi

AC_ARG_ENABLE(tests,
	AC_HELP_STRING([--disable-tests],
		[Do not build unit tests [default=no]]),
		enable_tests=no, enable_tests=yes)

NUNIT_PATH=

PKG_CHECK_MODULES([MONO_UIA], [mono-uia >= 2.1])
PKG_CHECK_MODULES(NUNIT, mono-nunit >= 2.4, nunit_24=yes, nunit_24=no)
if test "x$nunit_24" = "xno"; then
	AC_MSG_WARN([Could not find mono-nunit >= 2.4; unit tests will not be available.])
	enable_tests=no
else
	NUNIT_PATH=`pkg-config --variable=libdir mono-nunit`/mono/2.0/
fi

AC_SUBST(NUNIT_PATH)

AM_CONDITIONAL(ENABLE_TESTS, test "x$enable_tests" = "xyes")

AC_ARG_ENABLE(debug,
	AC_HELP_STRING([--enable-debug],
		[Use 'DEBUG' Configuration [default=YES]]),
		enable_debug=yes, enable_debug=no)
AM_CONDITIONAL(ENABLE_DEBUG, test x$enable_debug = xyes)
if test "x$enable_debug" = "xyes" ; then
	CONFIG_REQUESTED="yes"
fi
AC_ARG_ENABLE(release,
	AC_HELP_STRING([--enable-release],
		[Use 'RELEASE' Configuration [default=NO]]),
		enable_release=yes, enable_release=no)
AM_CONDITIONAL(ENABLE_RELEASE, test x$enable_release = xyes)
if test "x$enable_release" = "xyes" ; then
	CONFIG_REQUESTED="yes"
fi
if test -z "$CONFIG_REQUESTED" ; then
	AM_CONDITIONAL(ENABLE_DEBUG, true)
	enable_debug=yes
fi


# package checks, common for all configs
PKG_CHECK_MODULES([GLIB_SHARP_20], [glib-sharp-2.0 >= 2.12.8])
PKG_CHECK_MODULES([ATSPI_SHARP], [at-spi-sharp >= 1.1.1])
PKG_CHECK_MODULES([GLIB_SHARP_20], [glib-sharp-2.0 >= 2.12.8])
PKG_CHECK_MODULES([GTK_SHARP_20], [gtk-sharp-2.0 >= 2.12.8])

TOPDIR=$PWD
AC_SUBST(TOPDIR)

AC_CONFIG_FILES([
Makefile
AtspiUiaSource/Makefile
AtspiUiaSourceTests/Makefile
AtspiUiaSourceTests/atspiuiasourcetest.sh
data/atspiuiasource.spec
data/atspiuiasource-Fedora_12.spec
data/atspiuiasource-Fedora_13.spec
])

AC_OUTPUT
