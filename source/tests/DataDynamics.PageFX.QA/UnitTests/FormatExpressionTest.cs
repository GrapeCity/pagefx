using System;
using NUnit.Framework;

namespace DataDynamics.Tests
{
    [TestFixture]
    public class FormatExpressionTest
    {
		private class DummyEvaluator : FormatExpression.IEvaluator
		{
			public string Evaluate(string expression)
			{
				return expression;
			}
		}

        static string Eval(string exp)
        {
            return FormatExpression.Eval(new DummyEvaluator(), exp);
        }

        [Test]
        public void Null()
        {
            string s = Eval(null);
            Assert.IsNull(s);
        }

        [Test]
        public void Empty()
        {
            string s = Eval("");
            Assert.AreEqual("", s);
        }

        [Test]
        public void EscapeBraces()
        {
            {
                string s = Eval("A{{A");
                Assert.AreEqual("A{A", s, "#1");
            }
            {
                string s = Eval("A}}A");
                Assert.AreEqual("A}A", s, "#2");
            }
        }

        [Test]
        public void Valid()
        {
            {
                string s = Eval("A{}A");
                Assert.AreEqual("AA", s, "#1");
            }
            {
                string s = Eval("A{A}A");
                Assert.AreEqual("AAA", s, "#2");
            }
        }

        [Test]
        public void Invalid()
        {
            InvalidCore("{");
            InvalidCore("}");
            InvalidCore("A{A");
            InvalidCore("A}A");
            InvalidCore("A{{}A");
            InvalidCore("A{ { }A");
            InvalidCore("A{ } }A");
            InvalidCore("A{ {{ }A");
            InvalidCore("A{ }} }A");
            InvalidCore("A{ {} }A");
        }

        static void InvalidCore(string exp)
        {
            try
            {
                Eval(exp);
                Assert.Fail(string.Format("No expected FormatException. Expression: {0}", exp));
            }
            catch (Exception e)
            {
                Assert.AreEqual(e.GetType(), typeof(FormatException), exp);
            }
        }
    }
}