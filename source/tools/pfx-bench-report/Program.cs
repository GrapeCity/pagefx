using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using DataDynamics;
using DataDynamics.PageFX;


namespace pfx_bench_report
{
    class Program
    {
        const string ReportsDir = @"c:\pfxdata\bench";
        
        //private static readonly string OutputDir = "c:\\QA\\PageFX\\bench\\";


        private static string ScriptDir
        {
            get
            {
                var s = "c:\\QA\\PageFX\\bench\\script\\";
                if (!Directory.Exists(s))
                    Directory.CreateDirectory(s);
                return s;
            }
        }

        private static string ImageName = "perf.jpeg";

        private static string ImagePath
        {
            get { return Path.Combine(ScriptDir, ImageName).Replace("\\", "\\\\"); }
        }

        private static string TempReport 
        { 
            get
            {
                return Path.Combine(ScriptDir, "report.temp");
            }
        }

        private static readonly SortedDictionary<int,int> stat = new SortedDictionary<int, int>();

        private static readonly SortedDictionary<int, int> summary = new SortedDictionary<int, int>();

        static void GenerateReport(string dir)
        {
            string[] fileList = Directory.GetFiles(dir);
            
            foreach (var file in fileList)
            {
                var sr = new StreamReader(file);
                var s = Path.GetFileNameWithoutExtension(file);
                try
                {
                    string[] bs = s.Split('.');
                    int key = Convert.ToInt32(bs[2]);
                    var val = Convert.ToInt32(sr.ReadToEnd());

                    try
                    {
                        stat.Add(key, val);
                    }
                    catch (Exception exc)
                    {
                        Console.WriteLine("key:{0}, val:{1}", key, val);
                        Console.WriteLine(exc);
                    }
                    
                    
                    if (summary.ContainsKey(key))
                    {
                        summary[key] += val;
                    }
                    else
                    {
                        summary.Add(key, val);
                    }
                }
                catch (Exception exc)
                {
                    Console.WriteLine(exc);
                    Console.WriteLine(file);
                    Console.WriteLine(s);
                }
                //sw.WriteLine(s + "\t" + sr.ReadToEnd());
                sr.Close();
            }

            WriteReportFile(stat);
        }
        
        static void WriteReportFile(SortedDictionary<int, int> dict)
        {
            using (var sw = new StreamWriter(Path.Combine(ScriptDir, TempReport)))
            {
                foreach (var key in dict.Keys)
                {
                    sw.WriteLine(key + "\t" + dict[key]);
                }
            }
        }

        #region Utils and helpers
        //private static string GetXtics()
        //{
        //    var xtics = "set xtics (";
        //    int[] a = stat.Values;

        //    for (int i = 0; i < a.Length - 1; i++)
        //    {
        //        xtics += a[i] + ", ";
        //    }
        //    xtics += a[a.Length - 1];
        //    return xtics + ")";
        //}

        #region Max, Min 
        private static int GetMax<T>(IEnumerable<T> a)
        {
            IEnumerator<T> en = a.GetEnumerator();
            en.MoveNext();
            var str = en.Current.ToString();
            int indexFrom = str.IndexOf("\t");
            int max = 0;
            if (indexFrom > -1)
                max = Convert.ToInt32(str.Substring(indexFrom, str.Length - indexFrom));
            else
                max = Convert.ToInt32(str);
            int t = 0;
            foreach (var s in a)
            {
                indexFrom = s.ToString().IndexOf("\t");
                var ss = s.ToString();
                if (indexFrom > -1)
                {
                    t = Convert.ToInt32(ss.Substring(indexFrom, ss.Length - indexFrom));
                }
                else
                {
                    t = Convert.ToInt32(ss);
                }
                if (t > max)
                    max = t;
            }
            return max;
        }

        private static int GetMin<T>(IEnumerable<T> a)
        {
            IEnumerator<T> en = a.GetEnumerator();
            en.MoveNext();
            var str = en.Current.ToString();
            int indexFrom = str.IndexOf("\t");
            int min = 0;
            if (indexFrom > -1)
                min = Convert.ToInt32(str.Substring(indexFrom, str.Length - indexFrom));
            else
                min = Convert.ToInt32(str);
            int t = 0;
            foreach (var s in a)
            {
                indexFrom = s.ToString().IndexOf("\t");
                var ss = s.ToString();
                if (indexFrom > -1)
                {
                    t = Convert.ToInt32(ss.Substring(indexFrom, ss.Length - indexFrom));
                }
                else
                {
                    t = Convert.ToInt32(ss);
                }
                if (t < min)
                    min = t;
            }
            return min;
        }
        #endregion

        private static string GetXrange()
        {
            //var min = GetMin(builds);
            //var max = GetMax(builds);
            return "set xrange [" + 0 + ":" + 100 + "]";
        }
        private static string GetXticsB(SortedDictionary<int, int> dict)
        {
            var xtics = "set xtics (";
            int step = 100 / dict.Count;
            int i = 0;
            int lasKey = -1;
            foreach (var key in dict.Keys)
            {
                if ( i != (dict.Count - 1) )
                {
                    xtics += " \"" + key + "\" " + ((i + 1) * step) + ", ";
                }
                else
                {
                    lasKey = key;
                }
                i++;
            }
            
            xtics += " \"" + lasKey + "\" " + ((dict.Count) * step);
            return xtics + ")\n";
        }

        private static double GetY(Double val, int max, int min)
        {
            return ((val - min) / (max - min));
        }
        

        private static string TempScriptName = "tmp.plt";
        #endregion


        #region Gerenate gnuplot script to get trends

        private static string GenerateGnuplotScriptLines(string name)
        {
            string path = name;
            if (name != "summary")
            {
                try
                {
                    path = name.Remove(0, ReportsDir.Length + 1);
                }
                catch (Exception exc)
                {
                    Console.WriteLine(exc);
                    Console.WriteLine("name:{0}, path:{1}", name, path);
                }
            }
            
            
            ImageName = path + "_lines.jpeg";

            var s = "set terminal jpeg \n"
                    + "set output \""
                    + ImagePath
                    + "\" \n"
                    + "set title 'Performance trends for "
                    + path + "(less is better)' \n"
                    + "set grid \n"
                    //+ GetXticsB() + "\n"
                    //+ GetXrange() + "\n"
                    + "plot "
                    + "'" + TempReport.Replace("\\", "\\\\") + "'"
                    + " notitle with linespoints \n";

            var sw = new StreamWriter(Path.Combine(ScriptDir, TempScriptName));
            sw.Write(s);
            sw.Close();
            return TempScriptName;
        }
        #endregion

        #region Generate gnuplot script to get boxes
        

        private static string GenerateGnuplotScriptBoxes(string name)
        {
            string path = name;
            SortedDictionary<int, int> dict = null;

            if (name != "summary")
            {
                dict = stat;
                try
                {
                    path = name.Remove(0, ReportsDir.Length + 1);
                }
                catch (Exception exc)
                {
                    Console.WriteLine(exc);
                    Console.WriteLine("name:{0}, path:{1}", name, path);
                }    
            }
            else
            {
                dict = summary;
            }
            ImageName = path + "_boxes.jpeg";
            var box = "set terminal jpeg \n"
                      + "set output \""
                      + ImagePath
                      + "\" \n"
                      + "set title 'Performance boxes for "
                      + path + "(less is better) '\n"
                      + "set grid \n"
                      //+ "set xrange [" + GetMin(builds) + ":" + GetMax(builds) + "] \n"
                      + GetXticsB(dict)
                      //+ "set yrange [" + GetMin(ls) + ":" + GetMax(ls) + "] \n";
                      //+ "set yrange [" + GetMin(dict.Values) + ":" + GetMax(dict.Values) + "] \n";
                      + "set yrange [0:100] \n";
                      //+ "set yrange [0:" + GetMax(dict.Values) + "] \n";
            int i = 0;
            double step = 1.0/(dict.Count + 2);
            double x = 0.01;

            foreach (var key in dict.Keys)
            {
                var val = dict[key];
                var y = GetY(val, GetMax(dict.Values), GetMin(dict.Values));

                box += "set object "
                            + (i + 1)
                            + " rect from graph "
                            + (x + 0.04).ToString("0.##")
                            + ", 0.01" 
                            + ", 0 "
                            + "to graph " 
                            + (x + step).ToString("0.##") + ", "
                            + y
                            +", 0 back lw 1.0 fc  linestyle "
                            + i 
                            +" fillstyle  solid 1.00 border -1 \n";
                box += "set label "
                       + "\"" + key + "\""
                       + " at graph "
                       + (x + step/2.0 + 0.01).ToString("0.##")
                       + ", "
                       + (y/2)
                       + " center font \"Symbol,24\" \n";
                x = x + step;
                i++;
            }
            box += "plot 0 notitle \n";

            var sw = new StreamWriter(Path.Combine(ScriptDir, TempScriptName));
            sw.Write(box);
            sw.Close();
            return TempScriptName;
        }
        #endregion

        private delegate string GenerateGnuplotScript(string dir);

        private static void GenerateImages(string dir, GenerateGnuplotScript generator)
        {

            var cmd = Path.Combine(GlobalSettings.ToolsDirectory, "gnuplot\\bin\\pgnuplot.exe");
            int exitCode = 0;
            var gpScript = Path.Combine(ScriptDir, generator(dir));
            string err = null;
            try
            {
                err = CommandPromt.Run(cmd, gpScript, out exitCode);
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc);
                Console.WriteLine("-----------------------------------");
                Console.WriteLine("cmd:{0}\tgpScript:{1}\texitCode:{2}", cmd, gpScript, exitCode);
            }
            
            if (!string.IsNullOrEmpty(err))
            {
                throw new InvalidOperationException("Unable to generate images");
            }
        }


        #region Generate main html
        private static readonly string MainHtmlName = "index.html";

        private static void GenerateMainHTMLHead()
        {
            var path = Path.Combine(ScriptDir, MainHtmlName);
            var sw = new StreamWriter(path);
            string performanceHTML = "<html> \n"
                                   + "<body> \n"
                                   + "<FRAME name=\"toolbar\" scrolling=\"no\" bordercolor=red>  ";
            sw.Write(performanceHTML);
            sw.Close();
        }

        private const string DefaultPage = "summary.html";

        private static void GenerateMainHTMLEnd()
        {
            var path = Path.Combine(ScriptDir, MainHtmlName);
            var sw = new StreamWriter(path, true);
            string performanceHTML = "</FRAME><br><br><IFRAME name=\"report\" align=\"top\" width=95% height=95% frameborder=0 src=\"" + DefaultPage + "\"></IFRAME>\n"
                                     + "</body>\n</html>";
            sw.Write(performanceHTML);
            sw.Close();
        }

        private static void GenerateMainHTMLBody(string str)
        {
            var path = Path.Combine(ScriptDir, MainHtmlName);
            var sw = new StreamWriter(path, true);
            string performanceHTML = "<a href=\"" + str + ".html" + "\" target=\"report\" >" + str + "</a> \n";
            sw.Write(performanceHTML);
            sw.Close();
        }
        #endregion


        #region Generate html file for each benchmark

        private static string ImageNameHelper()
        {
            string name = Path.GetFileNameWithoutExtension(ImageName);
            int sep = name.IndexOf("_");
            try
            {
                name = name.Remove(sep, name.Length - sep);
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc);
                Console.WriteLine("sep:{0}, name:{1}", sep, name);
            }
            return name;
        }

        private static void GenerateHTMLHead()
        {
            // Prepare name
            string name = ImageNameHelper();
            var path = Path.Combine(ScriptDir, name + ".html");
            var sw = new StreamWriter(path);
            string performanceHTML = "<html>\n<body>\n";
            sw.Write(performanceHTML);
            sw.Close();
        }

        private static void GenerateHTMLEnd()
        {
            // Prepare name
            string name = ImageNameHelper();
            var path = Path.Combine(ScriptDir, name + ".html");
            var sw = new StreamWriter(path, true);
            string performanceHTML = "\n</body>\n</html>";
            sw.Write(performanceHTML);
            sw.Close();
        }


        private static void GenerateHTMLBody()
        {
            // Prepare name
            string name = ImageNameHelper();
            var path = Path.Combine(ScriptDir, name + ".html");
            bool append = File.Exists(path);
            var sw = new StreamWriter(path, true);
            string performanceHTML = "<img src=\"" + ImageName + "\" width=50% heigh=50%>";
            sw.Write(performanceHTML);
            sw.Close();
        }
        #endregion

        static void GenerateSummary()
        {
            WriteReportFile(summary);
            GenerateImages("summary",GenerateGnuplotScriptLines);
            GenerateHTMLHead();
            GenerateHTMLBody();
            GenerateImages("summary", GenerateGnuplotScriptBoxes);
            GenerateHTMLBody();
            GenerateHTMLEnd();
            GenerateMainHTMLBody(ImageNameHelper());
        }

        static void Generate()
        {
            string[] dirlist = Directory.GetDirectories(ReportsDir);
            GenerateMainHTMLHead();
            foreach (var dir in dirlist)
            {
                
                GenerateReport(dir);
                GenerateImages(dir, GenerateGnuplotScriptLines);
                GenerateHTMLHead();
                GenerateHTMLBody();
                GenerateImages(dir, GenerateGnuplotScriptBoxes);
                GenerateHTMLBody();
                GenerateHTMLEnd();

                //free local resources
                stat.Clear();
                GenerateMainHTMLBody(ImageNameHelper());
            }
            GenerateSummary();
            GenerateMainHTMLEnd();
        }
        
        static void Main()
        {
            Generate();
        }

    }
}
