# -*- makefile -*-
#
# The rules for building our class libraries.
#
# The NO_TEST stuff is not too pleasant but whatcha
# gonna do.

# All the dep files now land in the same directory so we
# munge in the library name to keep the files from clashing.

sourcefile = $(LIBRARY).sources

# If the directory contains the per profile include file, generate list file.
PROFILE_sources = $(PROFILE)_$(LIBRARY).sources
ifeq ($(wildcard $(PROFILE_sources)), $(PROFILE_sources))
PROFILE_excludes = $(wildcard $(PROFILE)_$(LIBRARY).exclude.sources)
COMMON_sourcefile := $(sourcefile)
sourcefile = $(depsdir)/$(PROFILE)_$(LIBRARY).sources
$(sourcefile): $(PROFILE_sources) $(PROFILE_excludes) $(COMMON_sourcefile)
	@echo Creating the per profile list $@ ...
	$(topdir)/tools/gensources.sh $(PROFILE_sources) $(PROFILE_excludes) > $@
endif

PLATFORM_excludes := $(wildcard $(LIBRARY).$(PLATFORM)-excludes)

ifndef PLATFORM_excludes
ifeq (cat,$(PLATFORM_CHANGE_SEPARATOR_CMD))
response = $(sourcefile)
endif
endif

ifndef response
response = $(depsdir)/$(PROFILE)_$(LIBRARY).response
library_CLEAN_FILES += $(response) $(LIBRARY).mdb $(BUILT_SOURCES)
endif

ifndef LIBRARY_NAME
LIBRARY_NAME = $(LIBRARY)
endif

ifdef LIBRARY_COMPAT
lib_dir = compat
else
lib_dir = lib
endif

makefrag = $(depsdir)/$(PROFILE)_$(LIBRARY).makefrag
the_libdir = $(topdir)/class/$(lib_dir)/$(PROFILE)/
the_lib = $(the_libdir)$(LIBRARY_NAME)
the_pdb = $(the_lib:.dll=.pdb)
the_mdb = $(the_lib).mdb
library_CLEAN_FILES += $(makefrag) $(the_lib) $(the_pdb) $(the_mdb)

ifdef LIBRARY_NEEDS_POSTPROCESSING
build_libdir = fixup/$(PROFILE)/
build_lib = $(build_libdir)$(LIBRARY_NAME)
library_CLEAN_FILES += $(build_lib) $(build_lib:.dll=.pdb)
else
build_lib = $(the_lib)
endif

ifndef NO_TEST
test_nunit_lib = nunit.framework.dll nunit.core.dll nunit.util.dll
test_nunit_dep = $(test_nunit_lib:%=$(topdir)/class/lib/$(PROFILE)/%)
test_nunit_ref = $(test_nunit_dep:%=-r:%)
library_CLEAN_FILES += TestResult*.xml

test_lib = $(LIBRARY:.dll=_test_$(PROFILE).dll)
test_sourcefile = $(LIBRARY:.dll=_test.dll.sources)
test_pdb = $(test_lib:.dll=.pdb)
test_response = $(depsdir)/$(test_lib).response
test_makefrag = $(depsdir)/$(test_lib).makefrag
test_flags = -r:$(the_lib) $(test_nunit_ref) $(TEST_MCS_FLAGS)
library_CLEAN_FILES += $(LIBRARY:.dll=_test*.dll) $(LIBRARY:.dll=_test*.pdb) $(test_response) $(test_makefrag)

btest_lib = $(LIBRARY:.dll=_btest_$(PROFILE).dll)
btest_sourcefile = $(LIBRARY:.dll=_btest.dll.sources)
btest_pdb = $(btest_lib:.dll=.pdb)
btest_response = $(depsdir)/$(btest_lib).response
btest_makefrag = $(depsdir)/$(btest_lib).makefrag
btest_flags = -r:$(the_lib) $(test_nunit_ref) $(TEST_MBAS_FLAGS)
library_CLEAN_FILES += $(LIBRARY:.dll=_btest*.dll) $(LIBRARY:.dll=_btest*.pdb) $(btest_response) $(btest_makefrag)

ifndef HAVE_CS_TESTS
HAVE_CS_TESTS := $(wildcard $(test_sourcefile))
endif
ifndef HAVE_VB_TESTS
HAVE_VB_TESTS := $(wildcard $(btest_sourcefile))
endif

endif

ifdef NO_INSTALL
GACUTIL = :
else
gacutil = $(topdir)/class/lib/net_1_1_bootstrap/gacutil.exe
GACUTIL = MONO_PATH="$(topdir)/class/lib/net_1_1_bootstrap$(PLATFORM_PATH_SEPARATOR)$$MONO_PATH" $(RUNTIME) $(RUNTIME_FLAGS) $(gacutil)
endif

ifdef NO_SIGN_ASSEMBLY
SN = :
else
sn = $(topdir)/class/lib/net_1_1_bootstrap/sn.exe
SN = MONO_PATH="$(topdir)/class/lib/net_1_1_bootstrap$(PLATFORM_PATH_SEPARATOR)$$MONO_PATH" $(RUNTIME) $(RUNTIME_FLAGS) $(sn)
SNFLAGS = -q -R
endif

ifeq ($(PLATFORM), win32)
GACDIR = `cygpath -w $(mono_libdir)`
GACROOT = `cygpath -w $(DESTDIR)$(mono_libdir)`
test_flags += -d:WINDOWS
else
GACDIR = $(mono_libdir)
GACROOT = $(DESTDIR)$(mono_libdir)
endif

all-local: $(the_lib)

install-local: all-local
test-local: all-local
uninstall-local:

ifdef NO_INSTALL
install-local uninstall-local:
	@:

else

ifdef LIBRARY_INSTALL_DIR
install-local:
	$(MKINSTALLDIRS) $(DESTDIR)$(LIBRARY_INSTALL_DIR)
	$(INSTALL_LIB) $(the_lib) $(DESTDIR)$(LIBRARY_INSTALL_DIR)/$(LIBRARY_NAME)
	test ! -f $(the_lib).mdb || $(INSTALL_LIB) $(the_lib).mdb $(DESTDIR)$(LIBRARY_INSTALL_DIR)/$(LIBRARY_NAME).mdb

uninstall-local:
	-rm -f $(DESTDIR)$(LIBRARY_INSTALL_DIR)/$(LIBRARY_NAME) $(DESTDIR)$(LIBRARY_INSTALL_DIR)/$(LIBRARY_NAME).mdb

else

# If RUNTIME_HAS_CONSISTENT_GACDIR is set, it implies that the internal GACDIR
# of the runtime is the same as the GACDIR we want.  So, we don't need to pass it
# to gacutil.  Note that the GACDIR we want may not be the same as the value of
# GACDIR set above, since the user could have overridden the value of $(prefix).
#
# This makes a difference only when we're building from the mono/ tree, since we
# have to ensure that the internal GACDIR of the in-tree runtime matches where we
# install the DLLs.

ifndef RUNTIME_HAS_CONSISTENT_GACDIR
gacdir_flag = /gacdir $(GACDIR)
endif

ifndef LIBRARY_PACKAGE
ifdef LIBRARY_COMPAT
LIBRARY_PACKAGE = compat-$(FRAMEWORK_VERSION)
else
LIBRARY_PACKAGE = $(FRAMEWORK_VERSION)
endif
endif

ifneq (none, $(LIBRARY_PACKAGE))
package_flag = /package $(LIBRARY_PACKAGE)
endif

install-local: $(gacutil)
	$(GACUTIL) /i $(the_lib) /f $(gacdir_flag) /root $(GACROOT) $(package_flag)

uninstall-local: $(gacutil)
	-$(GACUTIL) /u $(LIBRARY_NAME:.dll=) $(gacdir_flag) /root $(GACROOT) $(package_flag)

endif
endif

clean-local:
	-rm -f $(library_CLEAN_FILES) $(CLEAN_FILES)

test-local run-test-local run-test-ondotnet-local:
	@:

ifndef NO_TEST
$(test_nunit_dep): $(topdir)/build/deps/nunit-$(PROFILE).stamp
	@if test -f $@; then :; else rm -f $<; $(MAKE) $<; fi
$(topdir)/build/deps/nunit-$(PROFILE).stamp:
	cd ${topdir}/nunit20 && $(MAKE)
	echo "stamp" >$@
library_CLEAN_FILES += $(topdir)/build/deps/nunit-$(PROFILE).stamp
endif

test_assemblies :=

ifdef HAVE_CS_TESTS
test_assemblies += $(test_lib)
endif

ifdef HAVE_VB_TESTS
test_assemblies += $(btest_lib)
endif

ifdef test_assemblies
test-local: $(test_assemblies)
run-test-local: run-test-lib
run-test-ondotnet-local: run-test-ondotnet-lib

TEST_HARNESS_EXCLUDES = /exclude:NotWorking,ValueAdd,CAS,InetAccess
TEST_HARNESS_EXCLUDES_ONDOTNET = /exclude:NotDotNet,CAS

ifdef TEST_HARNESS_VERBOSE
TEST_HARNESS_OUTPUT = /labels
TEST_HARNESS_OUTPUT_ONDOTNET = /labels
TEST_HARNESS_POSTPROC = :
TEST_HARNESS_POSTPROC_ONDOTNET = :
else
TEST_HARNESS_OUTPUT = /output:TestResult-$(PROFILE).log
TEST_HARNESS_OUTPUT_ONDOTNET = /output:TestResult-ondotnet-$(PROFILE).log
TEST_HARNESS_POSTPROC = (echo ''; cat TestResult-$(PROFILE).log) | sed '1,/^Tests run: /d'
TEST_HARNESS_POSTPROC_ONDOTNET = (echo ''; cat TestResult-ondotnet-$(PROFILE).log) | sed '1,/^Tests run: /d'
endif

## FIXME: i18n problem in the 'sed' command below
run-test-lib: test-local
	ok=:; \
	MONO_REGISTRY_PATH="$(HOME)/.mono/registry" $(TEST_RUNTIME) $(RUNTIME_FLAGS) $(TEST_HARNESS) $(TEST_HARNESS_FLAGS) $(LOCAL_TEST_HARNESS_FLAGS) $(TEST_HARNESS_EXCLUDES) $(TEST_HARNESS_OUTPUT) /xml:TestResult-$(PROFILE).xml $(test_assemblies) || ok=false; \
	$(TEST_HARNESS_POSTPROC) ; $$ok

run-test-ondotnet-lib: test-local
	ok=:; \
	$(TEST_HARNESS) $(TEST_HARNESS_FLAGS) $(LOCAL_TEST_HARNESS_ONDOTNET_FLAGS) $(TEST_HARNESS_EXCLUDES_ONDOTNET) $(TEST_HARNESS_OUTPUT_ONDOTNET) /xml:TestResult-ondotnet-$(PROFILE).xml $(test_assemblies) || ok=false; \
	$(TEST_HARNESS_POSTPROC_ONDOTNET) ; $$ok
endif

DISTFILES = $(sourcefile) $(EXTRA_DISTFILES)

TEST_FILES =

ifdef HAVE_CS_TESTS
TEST_FILES += `sed -e '/^$$/d' -e 's,^,Test/,' $(test_sourcefile)`
DISTFILES += $(test_sourcefile)
endif
ifdef HAVE_VB_TESTS
TEST_FILES += `sed -e '/^$$/d' -e 's,^,Test/,' $(btest_sourcefile)`
DISTFILES += $(btest_sourcefile)
endif

dist-local: dist-default
	subs=' ' ; \
	for f in `cat $(sourcefile)` $(TEST_FILES) ; do \
	  case $$f in \
	  ../*) : ;; \
	  *) dest=`dirname $$f` ; \
	     case $$subs in *" $$dest "*) : ;; *) subs=" $$dest$$subs" ; $(MKINSTALLDIRS) $(distdir)/$$dest ;; esac ; \
	     cp -p $$f $(distdir)/$$dest || exit 1 ;; \
	  esac ; done ; \
	for d in . $$subs ; do \
	  case $$d in .) : ;; *) test ! -f $$d/ChangeLog || cp -p $$d/ChangeLog $(distdir)/$$d ;; esac ; done

ifdef LIBRARY_NEEDS_POSTPROCESSING
dist-local: dist-fixup
FIXUP_PROFILES = default net_2_0
dist-fixup:
	$(MKINSTALLDIRS) $(distdir)/fixup $(FIXUP_PROFILES:%=$(distdir)/fixup/%)
endif

ifndef LIBRARY_COMPILE
LIBRARY_COMPILE = $(CSCOMPILE)
endif

ifndef TEST_COMPILE
TEST_COMPILE = $(CSCOMPILE)
endif

ifndef BTEST_COMPILE
BTEST_COMPILE = $(BASCOMPILE)
endif

ifndef LIBRARY_SNK
LIBRARY_SNK = $(topdir)/class/mono.snk
endif

ifdef gacutil
$(gacutil):
	cd $(topdir) && $(MAKE) PROFILE=net_1_1_bootstrap
endif

ifdef sn
$(sn):
	cd $(topdir) && $(MAKE) PROFILE=net_1_1_bootstrap
endif

ifdef BUILT_SOURCES
ifeq (cat, $(PLATFORM_CHANGE_SEPARATOR_CMD))
BUILT_SOURCES_cmdline = $(BUILT_SOURCES)
else
BUILT_SOURCES_cmdline = `echo $(BUILT_SOURCES) | $(PLATFORM_CHANGE_SEPARATOR_CMD)`
endif
endif

# The library

$(the_libdir)/.stamp $(build_libdir:=/.stamp):
	$(MKINSTALLDIRS) $(@D)
	touch $@

$(the_lib): $(the_libdir)/.stamp

$(build_lib): $(response) $(sn) $(BUILT_SOURCES) $(build_libdir:=/.stamp)
ifdef LIBRARY_USE_INTERMEDIATE_FILE
	$(LIBRARY_COMPILE) $(LIBRARY_FLAGS) $(LIB_MCS_FLAGS) -target:library -out:$(LIBRARY_NAME) $(BUILT_SOURCES_cmdline) @$(response)
	$(SN) $(SNFLAGS) $(LIBRARY_NAME) $(LIBRARY_SNK)
	mv $(LIBRARY_NAME) $@
	test ! -f $(LIBRARY_NAME).mdb || mv $(LIBRARY_NAME).mdb $@.mdb
	test ! -f $(LIBRARY_NAME:.dll=.pdb) || mv $(LIBRARY_NAME:.dll=.pdb) $(dir $@)$(LIBRARY_NAME:.dll=.pdb)
else
	$(LIBRARY_COMPILE) $(LIBRARY_FLAGS) $(LIB_MCS_FLAGS) -target:library -out:$@ $(BUILT_SOURCES_cmdline) @$(response)
	$(SN) $(SNFLAGS) $@ $(LIBRARY_SNK)
endif

$(makefrag): $(sourcefile)
	@echo Creating $@ ...
	@sed 's,^,$(build_lib): ,' $< >$@

ifneq ($(response),$(sourcefile))
$(response): $(sourcefile) $(PLATFORM_excludes)
	@echo Creating $@ ...
	@sort $(sourcefile) $(PLATFORM_excludes) | uniq -u | $(PLATFORM_CHANGE_SEPARATOR_CMD) >$@
endif

-include $(makefrag)

# for now, don't give any /lib flags or set MONO_PATH, since we
# give a full path to the assembly.

ifdef HAVE_CS_TESTS

$(test_lib): $(the_lib) $(test_response) $(test_nunit_dep)
	$(TEST_COMPILE) -target:library -out:$@ $(test_flags) @$(test_response)

$(test_response): $(test_sourcefile)
	@echo Creating $@ ...
	@sed -e '/^$$/d' -e 's,^,Test/,' $(test_sourcefile) | $(PLATFORM_CHANGE_SEPARATOR_CMD) >$@

$(test_makefrag): $(test_response)
	@echo Creating $@ ...
	@sed 's,^,$(test_lib): ,' $< >$@

-include $(test_makefrag)

endif

ifdef HAVE_VB_TESTS

$(btest_lib): $(the_lib) $(btest_response) $(test_nunit_dep)
	$(BTEST_COMPILE) -target:library -out:$@ $(btest_flags) @$(btest_response)

$(btest_response): $(btest_sourcefile)
	@echo Creating $@ ...
	@sed -e '/^$$/d' -e 's,^,Test/,' $(btest_sourcefile) | $(PLATFORM_CHANGE_SEPARATOR_CMD) >$@

$(btest_makefrag): $(btest_response)
	@echo Creating $@ ...
	@sed 's,^,$(btest_lib): ,' $< >$@

-include $(btest_makefrag)

endif

all-local: $(makefrag) $(test_makefrag) $(btest_makefrag)
ifneq ($(response),$(sourcefile))
$(response): $(topdir)/build/library.make $(depsdir)/.stamp
endif
$(makefrag) $(test_response) $(test_makefrag) $(btest_response) $(btest_makefrag): $(topdir)/build/library.make $(depsdir)/.stamp
