import os, random

N = 100

def select(i):
	while 1:
		r = random.randint(1, N)
		if r != i:
			return r

print "using System;"
print ""

print "class X"
print "{"
print "\tpublic static int N;"
print "}"

for i in xrange(0, N):
	name = "A" + str(i + 1)
	print "#region " + name
	print "class " + name
	print "{"

	print "\tstatic " + name + "()"
	print "\t{"
	print "\t\tX.N++;"
	print "\t\tA" + str(select(i + 1)) + ".Foo();"
	print "\t}"
	print "\tstatic bool foo_called;"

	print "\tpublic static void Foo()"
	print "\t{"
	print "\t\tif (foo_called) return;"
	print "\t\tfoo_called = true;"
	print "\t\tX.N++;"
	for j in xrange(0,N):
		if j != i:
			print "\t\tA" + str(j + 1) + ".Foo();"
	print "\t}"
	print ""
	print "}" #end of class
	print "#endregion"

print "class Test"
print "{"
print "\tstatic void Main()"
print "\t{"
print "\t\tint start = Environment.TickCount;"
for i in xrange(0, N):
	print "\t\tA" + str(i + 1) + ".Foo();"
print "\t\tint end = Environment.TickCount;"
print "\t\tConsole.WriteLine(end - start);"
print "\t\tConsole.WriteLine(\"<%END%>\");"
print "\t}"
print "}"


	
	