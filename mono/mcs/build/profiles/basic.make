# -*- makefile -*-

with_mono_path = MONO_PATH="$(topdir)/class/lib/$(PROFILE)$(PLATFORM_PATH_SEPARATOR)$$MONO_PATH"
with_mono_path_monolite = MONO_PATH="$(topdir)/class/lib/monolite$(PLATFORM_PATH_SEPARATOR)$$MONO_PATH"

monolite_flag := $(depsdir)/use-monolite
use_monolite := $(wildcard $(monolite_flag))

ifdef use_monolite
PROFILE_RUNTIME = $(with_mono_path_monolite) $(RUNTIME)
BOOTSTRAP_MCS = $(PROFILE_RUNTIME) $(RUNTIME_FLAGS) $(topdir)/class/lib/monolite/mcs.exe
else
PROFILE_RUNTIME = $(EXTERNAL_RUNTIME)
BOOTSTRAP_MCS = $(EXTERNAL_MCS)
endif

MCS = $(with_mono_path) $(INTERNAL_MCS)

PROFILE_MCS_FLAGS = -d:NET_1_1 -d:ONLY_1_1 -d:BOOTSTRAP_WITH_OLDLIB
NO_SIGN_ASSEMBLY = yes
NO_TEST = yes
NO_INSTALL = yes
FRAMEWORK_VERSION = 1.0

.PHONY: profile-check do-profile-check
profile-check:
	@:

ifeq (.,$(thisdir))
all-recursive: do-profile-check
all-local: post-profile-cleanup
clean-local: clean-profile
endif

clean-profile:
	-rm -f $(PROFILE_CS) $(PROFILE_EXE) $(PROFILE_OUT) $(monolite_flag)

post-profile-cleanup:
	@rm -f $(monolite_flag)

PROFILE_CS  = $(depsdir)/basic-profile-check.cs
PROFILE_EXE = $(PROFILE_CS:.cs=.exe)
PROFILE_OUT = $(PROFILE_CS:.cs=.out)

do-profile-check:
	@ok=:; \
	rm -f $(PROFILE_EXE) $(PROFILE_OUT); \
	$(MAKE) -s $(PROFILE_OUT) > /dev/null || ok=false; \
	rm -f $(PROFILE_EXE) $(PROFILE_OUT); \
	if $$ok; then :; else \
	    if test -f $(topdir)/class/lib/monolite/mcs.exe; then \
		$(MAKE) -s do-profile-check-monolite ; \
	    else \
		echo "*** The compiler '$(BOOTSTRAP_MCS)' doesn't appear to be usable." 1>&2; \
                echo "*** You need a C# compiler installed to build MCS (make sure mcs works from the command line)" 1>&2 ; \
                echo "*** Read INSTALL.txt for information on how to bootstrap a Mono installation." 1>&2 ; \
	        exit 1; fi; fi


ifdef use_monolite

do-profile-check-monolite:
	echo "*** The contents of your 'monolite' directory may be out-of-date" 1>&2
	echo "*** You may want to try 'make get-monolite-latest'" 1>&2
	rm -f $(monolite_flag)
	exit 1

else

do-profile-check-monolite: $(depsdir)/.stamp
	echo "*** The compiler '$(BOOTSTRAP_MCS)' doesn't appear to be usable." 1>&2
	echo "*** Trying the 'monolite' directory." 1>&2
	echo dummy > $(monolite_flag)
	$(MAKE) do-profile-check

endif

$(PROFILE_CS): $(topdir)/build/profiles/basic.make $(depsdir)/.stamp
	echo 'class X { static int Main () { return 0; } }' > $@

$(PROFILE_EXE): $(PROFILE_CS)
	$(BOOTSTRAP_MCS) /out:$@ $<

$(PROFILE_OUT): $(PROFILE_EXE)
	$(PROFILE_RUNTIME) $< > $@ 2>&1
