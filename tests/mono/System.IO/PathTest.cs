//
// System.IO.Path Test Cases
//
// Authors:
// 	Marcin Szczepanski (marcins@zipworld.com.au)
// 	Gonzalo Paniagua Javier (gonzalo@ximian.com)
//	Ben Maurer (bmaurer@users.sf.net)
//	Gilles Freart (gfr@skynet.be)
//	Atsushi Enomoto (atsushi@ximian.com)
//	Sebastien Pouliot  <sebastien@ximian.com>
//
// (c) Marcin Szczepanski 
// (c) 2002 Ximian, Inc. (http://www.ximian.com)
// (c) 2003 Ben Maurer
// (c) 2003 Gilles Freart
// Copyright (C) 2005 Novell, Inc (http://www.novell.com)
//

using NUnit.Framework;
using System.IO;
using System;
using System.Text;

namespace MonoTests.System.IO
{

    enum OsType
    {
        Windows,
        Unix,
        Mac
    }

    [TestFixture]
    public class PathTest : Assertion
    {
        static string path1;
        static string path2;
        static string path3;
        static OsType OS;
        static char DSC = Path.DirectorySeparatorChar;

        [SetUp]
        public void SetUp()
        {
            if ('/' == DSC)
            {
                OS = OsType.Unix;
                path1 = "/foo/test.txt";
                path2 = "/etc";
                path3 = "init.d";
            }
            else if ('\\' == DSC)
            {
                OS = OsType.Windows;
                path1 = "c:\\foo\\test.txt";
                //path2 = Environment.GetEnvironmentVariable("SYSTEMROOT");
                path2 = "C:\\WINDOWS";
                path3 = "system32";
            }
            else
            {
                OS = OsType.Mac;
                //FIXME: For Mac. figure this out when we need it
                path1 = "foo:test.txt";
                path2 = "foo";
                path3 = "bar";
            }
        }

        bool Windows
        {
            get
            {
                return OS == OsType.Windows;
            }
        }

        bool Unix
        {
            get
            {
                return OS == OsType.Unix;
            }
        }

        bool Mac
        {
            get
            {
                return OS == OsType.Mac;
            }
        }

        public void TestChangeExtension()
        {
            string[] files = new string[3];
            files[(int)OsType.Unix] = "/foo/test.doc";
            files[(int)OsType.Windows] = "c:\\foo\\test.doc";
            files[(int)OsType.Mac] = "foo:test.doc";

            string testPath = Path.ChangeExtension(path1, "doc");
            AssertEquals("ChangeExtension #01", files[(int)OS], testPath);

            testPath = Path.ChangeExtension("", ".extension");
            AssertEquals("ChangeExtension #02", String.Empty, testPath);

            testPath = Path.ChangeExtension(null, ".extension");
            AssertEquals("ChangeExtension #03", null, testPath);

            testPath = Path.ChangeExtension("path", null);
            AssertEquals("ChangeExtension #04", "path", testPath);

            testPath = Path.ChangeExtension("path.ext", "doc");
            AssertEquals("ChangeExtension #05", "path.doc", testPath);

            testPath = Path.ChangeExtension("path.ext1.ext2", "doc");
            AssertEquals("ChangeExtension #06", "path.ext1.doc", testPath);

            testPath = Path.ChangeExtension("hogehoge.xml", ".xsl");
            AssertEquals("ChangeExtension #07", "hogehoge.xsl", testPath);
            testPath = Path.ChangeExtension("hogehoge", ".xsl");
            AssertEquals("ChangeExtension #08", "hogehoge.xsl", testPath);
            testPath = Path.ChangeExtension("hogehoge.xml", "xsl");
            AssertEquals("ChangeExtension #09", "hogehoge.xsl", testPath);
            testPath = Path.ChangeExtension("hogehoge", "xsl");
            AssertEquals("ChangeExtension #10", "hogehoge.xsl", testPath);
            testPath = Path.ChangeExtension("hogehoge.xml", String.Empty);
            AssertEquals("ChangeExtension #11", "hogehoge.", testPath);
            testPath = Path.ChangeExtension("hogehoge", String.Empty);
            AssertEquals("ChangeExtension #12", "hogehoge.", testPath);
            testPath = Path.ChangeExtension("hogehoge.", null);
            AssertEquals("ChangeExtension #13", "hogehoge", testPath);
            testPath = Path.ChangeExtension("hogehoge", null);
            AssertEquals("ChangeExtension #14", "hogehoge", testPath);
            testPath = Path.ChangeExtension(String.Empty, null);
            AssertEquals("ChangeExtension #15", String.Empty, testPath);
            testPath = Path.ChangeExtension(String.Empty, "bashrc");
            AssertEquals("ChangeExtension #16", String.Empty, testPath);
            testPath = Path.ChangeExtension(String.Empty, ".bashrc");
            AssertEquals("ChangeExtension #17", String.Empty, testPath);
            testPath = Path.ChangeExtension(null, null);
            AssertNull("ChangeExtension #18", testPath);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ChangeExtension_BadPath()
        {
            if (!Windows) throw new ArgumentException("Test Only On Windows");
            Path.ChangeExtension("<", ".extension");
        }

        [Test]
        public void ChangeExtension_BadExtension()
        {
            if (Windows)
            {
                string fn = Path.ChangeExtension("file.ext", "<");
                AssertEquals("Invalid filename", "file.<", fn);
            }
        }

        public void TestCombine()
        {
            string[] files = new string[3];
            files[(int)OsType.Unix] = "/etc/init.d";
            files[(int)OsType.Windows] = "" + @"C:\WINDOWS\system32";
            files[(int)OsType.Mac] = "foo:bar";

            string testPath = Path.Combine(path2, path3);
            AssertEquals("Combine #01", files[(int)OS], testPath);

            testPath = Path.Combine("one", "");
            AssertEquals("Combine #02", "one", testPath);

            testPath = Path.Combine("", "one");
            AssertEquals("Combine #03", "one", testPath);

            string current = Environment.CurrentDirectory;
            testPath = Path.Combine(current, "one");

            string expected = current + DSC + "one";
            AssertEquals("Combine #04", expected, testPath);

            testPath = Path.Combine("one", current);
            // LAMESPEC noted in Path.cs
            AssertEquals("Combine #05", current, testPath);

            testPath = Path.Combine(current, expected);
            AssertEquals("Combine #06", expected, testPath);

            testPath = DSC + "one";
            testPath = Path.Combine(testPath, "two" + DSC);
            expected = DSC + "one" + DSC + "two" + DSC;
            AssertEquals("Combine #06", expected, testPath);

            testPath = "one" + DSC;
            testPath = Path.Combine(testPath, DSC + "two");
            expected = DSC + "two";
            AssertEquals("Combine #06", expected, testPath);

            testPath = "one" + DSC;
            testPath = Path.Combine(testPath, "two" + DSC);
            expected = "one" + DSC + "two" + DSC;
            AssertEquals("Combine #07", expected, testPath);

            //TODO: Tests for UNC names
            try
            {
                testPath = Path.Combine("one", null);
                Fail("Combine Fail #01");
            }
            catch (Exception e)
            {
                AssertEquals("Combine Exc. #01", typeof(ArgumentNullException), e.GetType());
            }

            try
            {
                testPath = Path.Combine(null, "one");
                Fail("Combine Fail #02");
            }
            catch (Exception e)
            {
                AssertEquals("Combine Exc. #02", typeof(ArgumentNullException), e.GetType());
            }

            if (Windows)
            {
                try
                {
                    testPath = Path.Combine("a>", "one");
                    Fail("Combine Fail #03");
                }
                catch (Exception e)
                {
                    AssertEquals("Combine Exc. #03", typeof(ArgumentException), e.GetType());
                }

                try
                {
                    testPath = Path.Combine("one", "aaa<");
                    Fail("Combine Fail #04");
                }
                catch (Exception e)
                {
                    AssertEquals("Combine Exc. #04", typeof(ArgumentException), e.GetType());
                }
            }
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void EmptyDirectoryName()
        {
            string testDirName = Path.GetDirectoryName("");
        }

        public void TestDirectoryName()
        {
            string[] files = new string[3];
            files[(int)OsType.Unix] = "/foo";
            files[(int)OsType.Windows] = "c:\\foo";
            files[(int)OsType.Mac] = "foo";

            AssertEquals("GetDirectoryName #01", null, Path.GetDirectoryName(null));
            string testDirName = Path.GetDirectoryName(path1);
            AssertEquals("GetDirectoryName #02", files[(int)OS], testDirName);
            testDirName = Path.GetDirectoryName(files[(int)OS] + DSC);
            AssertEquals("GetDirectoryName #03", files[(int)OS], testDirName);

            if (Windows)
            {
                try
                {
                    testDirName = Path.GetDirectoryName("aaa>");
                    Fail("GetDirectoryName Fail #02");
                }
                catch (Exception e)
                {
                    AssertEquals("GetDirectoryName Exc. #02", typeof(ArgumentException), e.GetType());
                }
            }

            try
            {
                testDirName = Path.GetDirectoryName("   ");
                Fail("GetDirectoryName Fail #03");
            }
            catch (Exception e)
            {
                AssertEquals("GetDirectoryName Exc. #03", typeof(ArgumentException), e.GetType());
            }

            if (Windows)
            {
                AssertEquals("GetDirectoryName #04", null, Path.GetDirectoryName("C:"));
                AssertEquals("GetDirectoryName #05", null, Path.GetDirectoryName(@"C:\"));
                AssertEquals("GetDirectoryName #06", @"C:\", Path.GetDirectoryName(@"C:\dir"));
                AssertEquals("GetDirectoryName #07", @"C:\dir", Path.GetDirectoryName(@"C:\dir\"));
                AssertEquals("GetDirectoryName #08", @"C:\dir", Path.GetDirectoryName(@"C:\dir\dir"));
                AssertEquals("GetDirectoryName #09", @"C:\dir\dir", Path.GetDirectoryName(@"C:\dir\dir\"));

                // UNC tests
                AssertEquals("GetDirectoryName #10", null, Path.GetDirectoryName(@"\\"));
                AssertEquals("GetDirectoryName #11", null, Path.GetDirectoryName(@"\\server"));
                AssertEquals("GetDirectoryName #12", null, Path.GetDirectoryName(@"\\server\share"));

                AssertEquals("GetDirectoryName #13", @"\\server\share", Path.GetDirectoryName(@"\\server\share\"));
                AssertEquals("GetDirectoryName #14", @"\\server\share", Path.GetDirectoryName(@"\\server\share\dir"));
                AssertEquals("GetDirectoryName #15", @"\\server\share\dir", Path.GetDirectoryName(@"\\server\share\dir\subdir"));
            }

        }

        public void TestGetExtension()
        {
            string testExtn = Path.GetExtension(path1);

            AssertEquals("GetExtension #01", ".txt", testExtn);

            testExtn = Path.GetExtension(path2);
            AssertEquals("GetExtension #02", String.Empty, testExtn);

            testExtn = Path.GetExtension(String.Empty);
            AssertEquals("GetExtension #03", String.Empty, testExtn);

            testExtn = Path.GetExtension(null);
            AssertEquals("GetExtension #04", null, testExtn);

            testExtn = Path.GetExtension(" ");
            AssertEquals("GetExtension #05", String.Empty, testExtn);

            testExtn = Path.GetExtension(path1 + ".doc");
            AssertEquals("GetExtension #06", ".doc", testExtn);

            testExtn = Path.GetExtension(path1 + ".doc" + DSC + "a.txt");
            AssertEquals("GetExtension #07", ".txt", testExtn);

            testExtn = Path.GetExtension(".");
            AssertEquals("GetExtension #08", String.Empty, testExtn);

            testExtn = Path.GetExtension("end.");
            AssertEquals("GetExtension #09", String.Empty, testExtn);

            testExtn = Path.GetExtension(".start");
            AssertEquals("GetExtension #10", ".start", testExtn);

            testExtn = Path.GetExtension(".a");
            AssertEquals("GetExtension #11", ".a", testExtn);

            testExtn = Path.GetExtension("a.");
            AssertEquals("GetExtension #12", String.Empty, testExtn);

            testExtn = Path.GetExtension("a");
            AssertEquals("GetExtension #13", String.Empty, testExtn);

            testExtn = Path.GetExtension("makefile");
            AssertEquals("GetExtension #14", String.Empty, testExtn);

            if (Windows)
            {
                try
                {
                    testExtn = Path.GetExtension("hi<there.txt");
                    Fail("GetExtension Fail #01");
                }
                catch (Exception e)
                {
                    AssertEquals("GetExtension Exc. #01", typeof(ArgumentException), e.GetType());
                }
            }
        }

        public void TestGetFileName()
        {
            string testFileName = Path.GetFileName(path1);

            AssertEquals("GetFileName #01", "test.txt", testFileName);
            testFileName = Path.GetFileName(null);
            AssertEquals("GetFileName #02", null, testFileName);
            testFileName = Path.GetFileName(String.Empty);
            AssertEquals("GetFileName #03", String.Empty, testFileName);
            testFileName = Path.GetFileName(" ");
            AssertEquals("GetFileName #04", " ", testFileName);

            if (Windows)
            {
                try
                {
                    testFileName = Path.GetFileName("hi<");
                    Fail("GetFileName Fail #01");
                }
                catch (Exception e)
                {
                    AssertEquals("GetFileName Exc. #01", typeof(ArgumentException), e.GetType());
                }
            }
        }

        public void TestGetFileNameWithoutExtension()
        {
            string testFileName = Path.GetFileNameWithoutExtension(path1);

            AssertEquals("GetFileNameWithoutExtension #01", "test", testFileName);

            testFileName = Path.GetFileNameWithoutExtension(null);
            AssertEquals("GetFileNameWithoutExtension #02", null, testFileName);

            testFileName = Path.GetFileNameWithoutExtension(String.Empty);
            AssertEquals("GetFileNameWithoutExtension #03", String.Empty, testFileName);
        }

        [Ignore("This does not work under windows. See ERROR comments below.")]
        public void TestGetFullPath()
        {
            string current = Environment.CurrentDirectory;

            string testFullPath = Path.GetFullPath("foo.txt");
            string expected = current + DSC + "foo.txt";
            AssertEquals("GetFullPath #01", expected, testFullPath);

            testFullPath = Path.GetFullPath("a//./.././foo.txt");
            AssertEquals("GetFullPath #02", expected, testFullPath);
            string root = Windows ? "C:\\" : "/";
            string[,] test = new string[,] {		
				{"root////././././././../root/././../root", "root"},
				{"root/", "root/"},
				{"root/./", "root/"},
				{"root/./", "root/"},
				{"root/../", ""},
				{"root/../", ""},
				{"root/../..", ""},
				{"root/.hiddenfile", "root/.hiddenfile"},
				{"root/. /", "root/. /"},
				{"root/.. /", "root/.. /"},
				{"root/..weirdname", "root/..weirdname"},
				{"root/..", ""},
				{"root/../a/b/../../..", ""},
				{"root/./..", ""},
				{"..", ""},
				{".", ""},
				{"root//dir", "root/dir"},
				{"root/.              /", "root/.              /"},
				{"root/..             /", "root/..             /"},
				{"root/      .              /", "root/      .              /"},
				{"root/      ..             /", "root/      ..             /"},
				{"root/./", "root/"},
				//ERROR! Paths are trimmed
				{"root/..                      /", "root/..                   /"},
				{".//", ""}
			};

            //ERROR! GetUpperBound (1) returns 1. GetUpperBound (0) == 23
            //... so only the first test was being done.
            for (int i = 0; i < test.GetUpperBound(1); i++)
            {
                AssertEquals(String.Format("GetFullPath #{0}", i), root + test[i, 1], Path.GetFullPath(root + test[i, 0]));
            }

            if (Windows)
            {
                string uncroot = @"\\server\share\";
                string[,] testunc = new string[,] {		
					{"root////././././././../root/././../root", "root"},
					{"root/", "root/"},
					{"root/./", "root/"},
					{"root/./", "root/"},
					{"root/../", ""},
					{"root/../", ""},
					{"root/../..", ""},
					{"root/.hiddenfile", "root/.hiddenfile"},
					{"root/. /", "root/. /"},
					{"root/.. /", "root/.. /"},
					{"root/..weirdname", "root/..weirdname"},
					{"root/..", ""},
					{"root/../a/b/../../..", ""},
					{"root/./..", ""},
					{"..", ""},
					{".", ""},
					{"root//dir", "root/dir"},
					{"root/.              /", "root/.              /"},
					{"root/..             /", "root/..             /"},
					{"root/      .              /", "root/      .              /"},
					{"root/      ..             /", "root/      ..             /"},
					{"root/./", "root/"},
					{"root/..                      /", "root/..                   /"},
					{".//", ""}
				};
                for (int i = 0; i < test.GetUpperBound(1); i++)
                {
                    AssertEquals(String.Format("GetFullPath UNC #{0}", i), uncroot + test[i, 1], Path.GetFullPath(uncroot + test[i, 0]));
                }
            }

            try
            {
                testFullPath = Path.GetFullPath(null);
                Fail("GetFullPath Fail #01");
            }
            catch (Exception e)
            {
                AssertEquals("GetFullPath Exc. #01", typeof(ArgumentNullException), e.GetType());
            }

            try
            {
                testFullPath = Path.GetFullPath(String.Empty);
                Fail("GetFullPath Fail #02");
            }
            catch (Exception e)
            {
                AssertEquals("GetFullPath Exc. #02", typeof(ArgumentException), e.GetType());
            }
        }

        public void TestGetFullPath2()
        {
            if (Windows)
            {
                AssertEquals("GetFullPath w#01", @"Z:\", Path.GetFullPath("Z:"));
#if !TARGET_JVM // Java full (canonical) path always starts with caps drive letter
                AssertEquals("GetFullPath w#02", @"c:\abc\def", Path.GetFullPath(@"c:\abc\def"));
#endif
                Assert("GetFullPath w#03", Path.GetFullPath(@"\").EndsWith(@"\"));
                // "\\\\" is not allowed
                Assert("GetFullPath w#05", Path.GetFullPath("/").EndsWith(@"\"));
                // "//" is not allowed
                Assert("GetFullPath w#07", Path.GetFullPath("readme.txt").EndsWith(@"\readme.txt"));
                Assert("GetFullPath w#08", Path.GetFullPath("c").EndsWith(@"\c"));
                Assert("GetFullPath w#09", Path.GetFullPath(@"abc\def").EndsWith(@"abc\def"));
                Assert("GetFullPath w#10", Path.GetFullPath(@"\abc\def").EndsWith(@"\abc\def"));
                AssertEquals("GetFullPath w#11", @"\\abc\def", Path.GetFullPath(@"\\abc\def"));
#if NOT_PFX
				AssertEquals ("GetFullPath w#12", Directory.GetCurrentDirectory () + @"\abc\def", Path.GetFullPath (@"abc//def"));
				AssertEquals ("GetFullPath w#13", Directory.GetCurrentDirectory ().Substring (0,2) + @"\abc\def", Path.GetFullPath ("/abc/def"));
#endif
                AssertEquals("GetFullPath w#14", @"\\abc\def", Path.GetFullPath("//abc/def"));
            }
        }

#if NOT_PFX
		public void TestGetPathRoot ()
		{
			string current;
			string expected;
			if (!Windows){
				current = Directory.GetCurrentDirectory ();
				expected = current [0].ToString ();
			} else {
				current = @"J:\Some\Strange Directory\Name";
				expected = "J:\\";
			}

			string pathRoot = Path.GetPathRoot (current);
			AssertEquals ("GetPathRoot #01", expected, pathRoot);
		}
#endif

        [Test]
        public void TestGetPathRoot2()
        {
            // note: this method doesn't call Directory.GetCurrentDirectory so it can be
            // reused for partial trust unit tests in PathCas.cs

            string pathRoot = Path.GetPathRoot("hola");
            AssertEquals("GetPathRoot #02", String.Empty, pathRoot);

            pathRoot = Path.GetPathRoot(null);
            AssertEquals("GetPathRoot #03", null, pathRoot);

            if (Windows)
            {
                AssertEquals("GetPathRoot w#01", "z:", Path.GetPathRoot("z:"));
                AssertEquals("GetPathRoot w#02", "c:\\", Path.GetPathRoot("c:\\abc\\def"));
                AssertEquals("GetPathRoot w#03", "\\", Path.GetPathRoot("\\"));
                AssertEquals("GetPathRoot w#04", "\\\\", Path.GetPathRoot("\\\\"));
                AssertEquals("GetPathRoot w#05", "\\", Path.GetPathRoot("/"));
                AssertEquals("GetPathRoot w#06", "\\\\", Path.GetPathRoot("//"));
                AssertEquals("GetPathRoot w#07", String.Empty, Path.GetPathRoot("readme.txt"));
                AssertEquals("GetPathRoot w#08", String.Empty, Path.GetPathRoot("c"));
                AssertEquals("GetPathRoot w#09", String.Empty, Path.GetPathRoot("abc\\def"));
                AssertEquals("GetPathRoot w#10", "\\", Path.GetPathRoot("\\abc\\def"));
                AssertEquals("GetPathRoot w#11", "\\\\abc\\def", Path.GetPathRoot("\\\\abc\\def"));
                AssertEquals("GetPathRoot w#12", String.Empty, Path.GetPathRoot("abc//def"));
                AssertEquals("GetPathRoot w#13", "\\", Path.GetPathRoot("/abc/def"));
                AssertEquals("GetPathRoot w#14", "\\\\abc\\def", Path.GetPathRoot("//abc/def"));
                AssertEquals("GetPathRoot w#15", @"C:\", Path.GetPathRoot(@"C:\"));
                AssertEquals("GetPathRoot w#16", @"C:\", Path.GetPathRoot(@"C:\\"));
                AssertEquals("GetPathRoot w#17", "\\\\abc\\def", Path.GetPathRoot("\\\\abc\\def\\ghi"));
            }
            else
            {
                // TODO: Same tests for Unix.
            }
        }

        public void TestGetTempPath()
        {
            string getTempPath = Path.GetTempPath();
            Assert("GetTempPath #01", getTempPath != String.Empty);
            Assert("GetTempPath #02", Path.IsPathRooted(getTempPath));
            AssertEquals("GetTempPath #03", Path.DirectorySeparatorChar, getTempPath[getTempPath.Length - 1]);
        }

        public void TestGetTempFileName()
        {
            string getTempFileName = null;
            try
            {
                getTempFileName = Path.GetTempFileName();
                Assert("GetTempFileName #01", getTempFileName != String.Empty);
#if NOT_PFX
                Assert("GetTempFileName #02", File.Exists(getTempFileName));
#endif
            }
            finally
            {
#if NOT_PFX
                if (getTempFileName != null && getTempFileName != String.Empty)
                {
                    File.Delete(getTempFileName);
                }
#endif
            }
        }

        public void TestHasExtension()
        {
            AssertEquals("HasExtension #01", true, Path.HasExtension("foo.txt"));
            AssertEquals("HasExtension #02", false, Path.HasExtension("foo"));
            AssertEquals("HasExtension #03", true, Path.HasExtension(path1));
            AssertEquals("HasExtension #04", false, Path.HasExtension(path2));
            AssertEquals("HasExtension #05", false, Path.HasExtension(null));
            AssertEquals("HasExtension #06", false, Path.HasExtension(String.Empty));
            AssertEquals("HasExtension #07", false, Path.HasExtension(" "));
            AssertEquals("HasExtension #08", false, Path.HasExtension("."));
            AssertEquals("HasExtension #09", false, Path.HasExtension("end."));
            AssertEquals("HasExtension #10", true, Path.HasExtension(".start"));
            AssertEquals("HasExtension #11", true, Path.HasExtension(".a"));
            AssertEquals("HasExtension #12", false, Path.HasExtension("a."));
            AssertEquals("HasExtension #13", false, Path.HasExtension("Makefile"));
        }

        public void TestRooted()
        {
            Assert("IsPathRooted #01", Path.IsPathRooted(path2));
            Assert("IsPathRooted #02", !Path.IsPathRooted(path3));
            Assert("IsPathRooted #03", !Path.IsPathRooted(null));
            Assert("IsPathRooted #04", !Path.IsPathRooted(String.Empty));
            Assert("IsPathRooted #05", !Path.IsPathRooted(" "));
            Assert("IsPathRooted #06", Path.IsPathRooted("/"));
            if (Windows)
                Assert("IsPathRooted #07", Path.IsPathRooted("\\"));
            else
                Assert("IsPathRooted #07", !Path.IsPathRooted("\\"));
            Assert("IsPathRooted #08", Path.IsPathRooted("//"));
            if (Windows)
                Assert("IsPathRooted #09", Path.IsPathRooted("\\\\"));
            else
                Assert("IsPathRooted #09", !Path.IsPathRooted("\\\\"));
            Assert("IsPathRooted #10", !Path.IsPathRooted(":"));
            if (Windows)
                Assert("IsPathRooted #11", Path.IsPathRooted("z:"));
            else
                Assert("IsPathRooted #11", !Path.IsPathRooted("z:"));

            if (Windows)
            {
                Assert("IsPathRooted #12", Path.IsPathRooted("z:\\"));
                Assert("IsPathRooted #13", Path.IsPathRooted("z:\\topdir"));
                // This looks MS BUG. It is treated as absolute path
                Assert("IsPathRooted #14", Path.IsPathRooted("z:curdir"));
                Assert("IsPathRooted #15", Path.IsPathRooted("\\abc\\def"));
            }
        }

        public void TestCanonicalizeDots()
        {
            string current = Path.GetFullPath(".");
            Assert("TestCanonicalizeDotst #01", !current.EndsWith("."));
            string parent = Path.GetFullPath("..");
            Assert("TestCanonicalizeDotst #02", !current.EndsWith(".."));
        }

        public void TestDirectoryNameBugs()
        {
            if (Windows)
            {
                AssertEquals("Win #01", "C:\\foo", Path.GetDirectoryName("C:\\foo\\foo.txt"));
            }
            else
            {
                AssertEquals("No win #01", "/etc", Path.GetDirectoryName("/etc/hostname"));
            }
        }

        public void TestGetFullPathUnix()
        {
            if (Windows)
                return;

            AssertEquals("#01", "/", Path.GetFullPath("/"));
            AssertEquals("#02", "/hey", Path.GetFullPath("/hey"));
            AssertEquals("#03", Environment.CurrentDirectory, Path.GetFullPath("."));
            AssertEquals("#04", Path.Combine(Environment.CurrentDirectory, "hey"),
                         Path.GetFullPath("hey"));
        }

        [Test]
        public void GetFullPath_EndingSeparator()
        {
            string fp = Path.GetFullPath("something/");
            char end = fp[fp.Length - 1];
            Assert(end == Path.DirectorySeparatorChar);
        }

#if NOT_PFX
		[Test]
		public void WindowsSystem32_76191 ()
		{
			// check for Unix platforms - see FAQ for more details
			// http://www.mono-project.com/FAQ:_Technical#How_to_detect_the_execution_platform_.3F
			int platform = (int) Environment.OSVersion.Platform;
			if ((platform == 4) || (platform == 128))
				return;

			string curdir = Directory.GetCurrentDirectory ();
			try {
#if TARGET_JVM
                string system = "C:\\WINDOWS\\system32\\";
#else
				string system = Environment.SystemDirectory;
#endif
				Directory.SetCurrentDirectory (system);
				string drive = system.Substring (0, 2);
				AssertEquals ("current dir", system, Path.GetFullPath (drive));
			}
			finally {
				Directory.SetCurrentDirectory (curdir);
			}
		}

		[Test]
		public void WindowsSystem32_77007 ()
		{
			// check for Unix platforms - see FAQ for more details
			// http://www.mono-project.com/FAQ:_Technical#How_to_detect_the_execution_platform_.3F
			int platform = (int) Environment.OSVersion.Platform;
			if ((platform == 4) || (platform == 128))
				return;

			string curdir = Directory.GetCurrentDirectory ();
			try {
#if TARGET_JVM
                string system = "C:\\WINDOWS\\system32\\";
#else
				string system = Environment.SystemDirectory;
#endif
				Directory.SetCurrentDirectory (system);
				// e.g. C:dir (no backslash) will return CurrentDirectory + dir
				string dir = system.Substring (0, 2) + "dir";
				AssertEquals ("current dir", Path.Combine (system, "dir"), Path.GetFullPath (dir));
			}
			finally {
				Directory.SetCurrentDirectory (curdir);
			}
		}

		[Test]
#if TARGET_JVM
        [Ignore("Java full (canonical) path always returns windows dir in caps")]
#endif
		public void WindowsDriveC14N_77058 ()
		{
			// check for Unix platforms - see FAQ for more details
			// http://www.mono-project.com/FAQ:_Technical#How_to_detect_the_execution_platform_.3F
			int platform = (int) Environment.OSVersion.Platform;
			if ((platform == 4) || (platform == 128))
				return;

			AssertEquals ("1", @"C:\Windows\dir", Path.GetFullPath (@"C:\Windows\System32\..\dir"));
			AssertEquals ("2", @"C:\dir", Path.GetFullPath (@"C:\Windows\System32\..\..\dir"));
			AssertEquals ("3", @"C:\dir", Path.GetFullPath (@"C:\Windows\System32\..\..\..\dir"));
			AssertEquals ("4", @"C:\dir", Path.GetFullPath (@"C:\Windows\System32\..\..\..\..\dir"));
			AssertEquals ("5", @"C:\dir\", Path.GetFullPath (@"C:\Windows\System32\..\.\..\.\..\dir\"));
		}
#endif

        //NOTE: Test below is version dependent
#if NOT_PFX
        [Test]
        public void InvalidPathChars_Values()
        {
            char[] invalid = Path.InvalidPathChars;
            if (Windows)
            {
#if NET_2_0
				AssertEquals ("Length", 36, invalid.Length);
#else
                AssertEquals("Length", 15, invalid.Length);
#endif
                foreach (char c in invalid)
                {
                    int i = (int)c;
#if NET_2_0
					if (i < 32)
						continue;
#else
                    if ((i == 0) || (i == 8) || ((i > 15) && (i < 19)) || ((i > 19) && (i < 26)))
                        continue;
#endif
                    // in both 1.1 SP1 and 2.0
                    if ((i == 34) || (i == 60) || (i == 62) || (i == 124))
                        continue;
                    Fail(String.Format("'{0}' (#{1}) is invalid", c, i));
                }
            }
            else
            {
                foreach (char c in invalid)
                {
                    int i = (int)c;
                    if (i == 0)
                        continue;
                    Fail(String.Format("'{0}' (#{1}) is invalid", c, i));
                }
            }
        }
#endif

#if NOT_PFX
        [Test]
        public void InvalidPathChars_Modify()
        {
            char[] expected = Path.GetInvalidPathChars();
            char[] invalid = Path.GetInvalidPathChars();
            char original = invalid[0];
            try
            {
                invalid[0] = 'a';
                // kind of scary
                Assert("expected", expected[0] == 'a');
                AssertEquals("readonly", expected[0], Path.GetInvalidPathChars()[0]);
            }
            finally
            {
                invalid[0] = original;
            }
        }
#endif

#if NET_2_0
#if NOT_PFX
	[Test]
		public void GetInvalidFileNameChars_Values ()
		{
			char[] invalid = Path.GetInvalidFileNameChars ();
			if (Windows) {
				AssertEquals (41, invalid.Length);
				foreach (char c in invalid) {
					int i = (int) c;
					if (i < 32)
						continue;
					if ((i == 34) || (i == 60) || (i == 62) || (i == 124))
						continue;
					// ':', '*', '?', '\', '/'
					if ((i == 58) || (i == 42) || (i == 63) || (i == 92) || (i == 47))
						continue;
					Fail (String.Format ("'{0}' (#{1}) is invalid", c, i));
				}
			} else {
				foreach (char c in invalid) {
					int i = (int) c;
					// null or '/'
					if ((i == 0) || (i == 47))
						continue;
					Fail (String.Format ("'{0}' (#{1}) is invalid", c, i));
				}
			}
		}

		[Test]
		public void GetInvalidFileNameChars_Modify ()
		{
			char[] expected = Path.GetInvalidFileNameChars ();
			char[] invalid = Path.GetInvalidFileNameChars ();
			invalid[0] = 'a';
			Assert ("expected", expected[0] != 'a');
			AssertEquals ("readonly", expected[0], Path.GetInvalidFileNameChars ()[0]);
		}
#endif
		[Test]
		public void GetInvalidPathChars_Values ()
		{
			char[] invalid = Path.GetInvalidPathChars ();
			if (Windows) {
				AssertEquals (36, invalid.Length);
				foreach (char c in invalid) {
					int i = (int) c;
					if (i < 32)
						continue;
					if ((i == 34) || (i == 60) || (i == 62) || (i == 124))
						continue;
					Fail (String.Format ("'{0}' (#{1}) is invalid", c, i));
				}
			} else {
				foreach (char c in invalid) {
					int i = (int) c;
					if (i == 0)
						continue;
					Fail (String.Format ("'{0}' (#{1}) is invalid", c, i));
				}
			}
		}

		[Test]
		public void GetInvalidPathChars_Order()
		{
			if (Windows) {
				char [] invalid = Path.GetInvalidPathChars ();
				char [] expected = new char [36] { '\x22', '\x3C', '\x3E', '\x7C', '\x00', '\x01', '\x02',
					'\x03', '\x04', '\x05', '\x06', '\x07', '\x08', '\x09', '\x0A', '\x0B', '\x0C', '\x0D',
					'\x0E', '\x0F', '\x10', '\x11', '\x12', '\x13', '\x14', '\x15', '\x16', '\x17', '\x18',
					'\x19', '\x1A', '\x1B', '\x1C', '\x1D', '\x1E', '\x1F' };
				AssertEquals (expected.Length, invalid.Length);
				for (int i = 0; i < expected.Length; i++ ) {
					AssertEquals( "Character at position " + i,expected [i], invalid [i]);
				}
			}
		}

		[Test]
		public void GetInvalidPathChars_Modify ()
		{
			char[] expected = Path.GetInvalidPathChars ();
			char[] invalid = Path.GetInvalidPathChars ();
			invalid[0] = 'a';
			Assert ("expected", expected[0] != 'a');
			AssertEquals ("readonly", expected[0], Path.GetInvalidPathChars ()[0]);
		}

#if NOT_PFX
[Test]
		public void GetRandomFileName ()
		{
			string s = Path.GetRandomFileName ();
			AssertEquals ("Length", 12, s.Length);
			char[] invalid = Path.GetInvalidFileNameChars ();
			for (int i=0; i < s.Length; i++) {
				if (i == 8)
					AssertEquals ("8", '.', s[i]);
				else
					Assert (i.ToString (), Array.IndexOf (invalid, s[i]) == -1);
			}
		}

		[Test]
		public void GetRandomFileNameIsAlphaNumerical ()
		{
			string [] names = new string [1000];
			for (int i = 0; i < names.Length; i++)
				names [i] = Path.GetRandomFileName ();

			foreach (string name in names) {
				AssertEquals (12, name.Length);
				AssertEquals ('.', name [8]);

				for (int i = 0; i < 12; i++) {
					if (i == 8)
						continue;

					char c = name [i];
					Assert (('a' <= c && c <= 'z') || ('0' <= c && c <= '9'));
				}
			}
		}

		[Test]
		public void TestGetDirectoryName ()
		{
			if (Unix){
				AssertEquals ("GetDirectoryName#1", Path.GetDirectoryName ("/foo//bar/dingus"), "/foo/bar");
				AssertEquals ("GDN#3", Path.GetDirectoryName ("foo/bar/"), "foo/bar");
				AssertEquals ("GDN#6", Path.GetDirectoryName ("/tmp"), "/");
				AssertEquals ("GDN#6", Path.GetDirectoryName ("/"), null);
			} else {
				AssertEquals ("GetDirectoryName#1", Path.GetDirectoryName ("/foo//bar/dingus"), "\\foo\\bar");

				AssertEquals ("GDN#4", Path.GetDirectoryName ("foo/bar/"), "foo\\bar");

				AssertEquals ("GDN#5", Path.GetDirectoryName ("foo/bar\\xxx"), "foo\\bar");
				AssertEquals ("GDN#2", Path.GetDirectoryName ("\\\\host\\dir\\\\dir2\\path"), "\\\\host\\dir\\dir2");
			}
		}
#endif
#endif
    }
}

