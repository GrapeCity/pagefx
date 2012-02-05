using System.Collections.Generic;
using NUnit.Framework;

namespace DataDynamics.Collections.Tests
{
    [TestFixture]
    public class TreeIteratorsTest
    {
        private class Node
        {
            public readonly int Value;
            public readonly List<Node> Kids = new List<Node>();

            public Node(int value, params Node[] next)
            {
                Value = value;
                Kids.AddRange(next);
            }

            public override string ToString()
            {
                return Value.ToString();
            }
        }

        [Test]
        public void TestDeepEnumerator()
        {
            var root = new Node(0, 
                new Node(1, new Node(11), new Node(12), new Node(13)),
                new Node(2, new Node(21), new Node(22), new Node(23)),
                new Node(3, new Node(31), new Node(32), new Node(33)));

            int[] arr1 = {0, 1, 2, 3, 11, 12, 13, 21, 22, 23, 31, 32, 33};
            int i = 0;
            foreach (var node in new DeepEnumerator<Node>(root, delegate(Node n) { return n.Kids; }, true, true))
            {
                Assert.AreEqual(arr1[i], node.Value, "#1:" + i);
                ++i;
            }

            int[] arr2 = { 11, 12, 13, 1, 21, 22, 23, 2, 31, 32, 33, 3, 0 };
            i = 0;
            foreach (var node in new DeepEnumerator<Node>(root, delegate(Node n) { return n.Kids; }, true, false))
            {
                Assert.AreEqual(arr2[i], node.Value, "#2:" + i);
                ++i;
            }
        }

        [Test]
        public void TestTopDown()
        {
            var root = new Node(0,
                new Node(1, new Node(11), new Node(12), new Node(13)),
                new Node(2, new Node(21), new Node(22), new Node(23)),
                new Node(3, new Node(31), new Node(32), new Node(33)));

            int[] arr1 = { 0, 1, 11, 12, 13, 2, 21, 22, 23, 3, 31, 32, 33 };
            int i = 0;
            foreach (var node in Algorithms.IterateTreeTopDown(root, delegate(Node n) { return n.Kids; }))
            {
                Assert.AreEqual(arr1[i], node.Value, "#" + i);
                ++i;
            }
        }

        [Test]
        public void TestBottomUp()
        {
            var root = new Node(0,
                           new Node(1, new Node(11), new Node(12), new Node(13)),
                           new Node(2, new Node(21), new Node(22), new Node(23)),
                           new Node(3, new Node(31), new Node(32), new Node(33)));

            int[] arr = { 11, 12, 13, 1, 21, 22, 23, 2, 31, 32, 33, 3, 0 };
            int i = 0;
            foreach (var node in Algorithms.IterateTreeBottomUp(root, delegate(Node n) { return n.Kids; }))
            {
                Assert.AreEqual(arr[i], node.Value, "#" + i);
                ++i;
            }
        }
    }
}