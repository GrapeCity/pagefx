#if NUNIT
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace DataDynamics.PageFX.Tests
{
	[TestFixture]
	public class MemoizationTests
	{
		private int _iterations;

		[SetUp]
		public void SetUp()
		{
			_iterations = 0;
		}

		[Test]
		public void CheckFullIteration()
		{
			var seq = FiveNumbers().Memoize();

			Assert.That(seq, Is.EquivalentTo(new[] {1, 2, 3, 4, 5}));
			Assert.AreEqual(5, _iterations);

			Assert.That(seq, Is.EquivalentTo(new[] {1, 2, 3, 4, 5}));
			Assert.AreEqual(5, _iterations);
		}

		[Test]
		public void CheckPartialIteration()
		{
			var seq = FiveNumbers().Memoize();

			Assert.That(seq.Take(3), Is.EquivalentTo(new[] {1, 2, 3}));
			Assert.AreEqual(3, _iterations);

			// no change, allready in cache
			Assert.That(seq.Take(3), Is.EquivalentTo(new[] {1, 2, 3}));
			Assert.AreEqual(3, _iterations);

			Assert.That(seq, Is.EquivalentTo(new[] {1, 2, 3, 4, 5}));
			Assert.AreEqual(5, _iterations);

			Assert.That(seq, Is.EquivalentTo(new[] { 1, 2, 3, 4, 5 }));
			Assert.AreEqual(5, _iterations);
		}

		[Test]
		public void CheckMemoization()
		{
			var seq = FiveObjects().Memoize();
			var list = new List<object>(seq);

			int index = 0;
			foreach (var item in seq)
			{
				Assert.AreSame(list[index++], item);
			}
		}

		[Test]
		public void CheckIndexer()
		{
			var seq = FiveNumbers().Memoize();
			for (int i = 5; i >= 1; i--)
			{
				Assert.AreEqual(i, seq[i - 1]);
			}
			for (int i = 1; i <= 5; i++)
			{
				Assert.AreEqual(i, seq[i - 1]);
			}
		}

		[Test]
		public void CheckCount()
		{
			var seq = FiveNumbers().Memoize();
			Assert.AreEqual(5, seq.Count);
		}

		private IEnumerable<int> FiveNumbers()
		{
			for (int i = 1; i <= 5; i++)
			{
				_iterations++;
				yield return i;
			}
		}

		private IEnumerable<object> FiveObjects()
		{
			for (int i = 1; i <= 5; i++)
			{
				yield return new object();
			}
		}
	}
}
#endif