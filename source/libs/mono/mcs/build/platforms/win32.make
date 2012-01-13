# -*- makefile -*-
#
# Win32 platform-specific makefile rules.
#

PLATFORM_DEBUG_FLAGS = /debug+ /debug:full
PLATFORM_MCS_FLAGS = /nologo /optimize
PLATFORM_RUNTIME = 
PLATFORM_CORLIB = mscorlib.dll

EXTERNAL_MCS = mcs
EXTERNAL_MBAS = vbc.exe
EXTERNAL_RUNTIME =

# Disabled since it needs the SDK
#RESGEN = resgen.exe
#ILDISASM = ildasm.exe /test
RESGEN = MONO_PATH="$(topdir)/class/lib/$(PROFILE)$(PLATFORM_PATH_SEPARATOR)$$MONO_PATH" $(INTERNAL_RESGEN)

#ILDISASM = monodis.bat
## Gross hack
ILDISASM = $(topdir)/../mono/mono/dis/monodis

PLATFORM_MAKE_CORLIB_CMP = yes
PLATFORM_CHANGE_SEPARATOR_CMD=tr '/' '\\\\'
PLATFORM_PATH_SEPARATOR = ;

hidden_prefix = 
hidden_suffix = .tmp

platform-check:
	@:
