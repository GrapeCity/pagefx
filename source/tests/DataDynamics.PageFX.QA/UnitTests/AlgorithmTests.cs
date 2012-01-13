using NUnit.Framework;

namespace DataDynamics
{
    [TestFixture]
    public class AlgorithmTests
    {
        private static void TestShuffleN(int n)
        {
            var arr = Algorithms.Shuffle(n);
            Assert.IsTrue(arr != null);
            Assert.AreEqual(n, arr.Length);
            for (int i = 0; i < n; ++i)
            {
                Assert.IsTrue(Algorithms.Contains(arr, i));
            }
        }

        [Test]
        public void TestShuffleN()
        {
            TestShuffleN(0);
            TestShuffleN(1);
            TestShuffleN(2);
            TestShuffleN(10);
            TestShuffleN(100);
        }
    }
}