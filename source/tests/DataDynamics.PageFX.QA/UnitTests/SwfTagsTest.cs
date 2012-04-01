using System.IO;
using DataDynamics.PageFX.FLI.SWF;
using NUnit.Framework;

namespace DataDynamics.PageFX.FLI.Tests
{
    [TestFixture]
    public class SwfTagsTest
    {
        private delegate void ResHandler(string resname);

        private static readonly string[] swfs = { "mx.swf", "mixins.swf", "mx.assets.swf" };
        //private static readonly string[] swfs = { "mx.assets.swf" };

        private static void Run(ResHandler handler)
        {
            foreach (var swf in swfs)
                handler(swf);
        }

        private static void TagEquals(SwfTag tag1, SwfTag tag2, string msg)
        {
            var data1 = tag1.GetData();
            var data2 = tag2.GetData();
            Assert.AreEqual(data1, data2, msg);
        }

        [Test]
        public void TestClone()
        {
            Run(TestClone);
        }

        private static void TestClone(string resname)
        {
            var rs = typeof(SwfIOTest).GetResourceStream(resname);
            if (rs == null) return;
            var swf = new SwfMovie(rs);
            int n = swf.Tags.Count;
            for (int i = 0; i < n; ++i)
            {
                var tag1 = swf.Tags[i];
                tag1.Clone();
            }
        }

        [Test]
        public void TestReadWriteShapes()
        {
            Run(TestReadWriteShapes);
        }

        private static void TestReadWriteShapes(string resname)
        {
            var rs = typeof(SwfIOTest).GetResourceStream(resname);
            if (rs == null) return;

            var ms = rs.ToMemoryStream();

            var swf1 = new SwfMovie(ms, SwfTagDecodeOptions.DonotDecodeTags);

            ms.Position = 0;

            var swf2 = new SwfMovie(ms);

            Assert.AreEqual(swf1.Tags.Count, swf2.Tags.Count, resname + "#count");
            int n = swf1.Tags.Count;
            for (int i = 0; i < n; ++i)
            {
                var tag1 = swf1.Tags[i];
                var tag2 = swf2.Tags[i];
                if (SwfTag.IsShape(tag1.TagCode))
                {
                    string msg;
                    var c = tag2 as ISwfCharacter;
                    if (c != null)
                    {
                        msg = string.Format("{0}, #{1}, {2}, {3} - {4}",
                                            resname, i, tag1.TagCode, c.CharacterID, c.Name);
                    }
                    else
                    {
                        msg = string.Format("{0}, #{1}, {2}",
                                            resname, i, tag1.TagCode);
                    }

                    TagEquals(tag1, tag2, msg);
                }
            }
        }
    } 
}