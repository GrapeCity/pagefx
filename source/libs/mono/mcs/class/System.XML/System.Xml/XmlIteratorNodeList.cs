//
// System.Xml.XmlIteratorNodeList
//
// Author:
//	Atsushi Enomoto <atsushi@ximian.com>
//
// (C) 2006 Novell Inc.
//

//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using System.Collections;

using System.Xml.XPath;

namespace System.Xml
{
	internal class XmlIteratorNodeList : XmlNodeList
	{
		XPathNodeIterator source;
		XPathNodeIterator iterator;
		XmlNode previous;
		ArrayList list;
		bool finished;

		#region Constructors

		public XmlIteratorNodeList (XPathNodeIterator iter)
		{
			source = iter;
			iterator = iter.Clone ();
			list = new ArrayList ();
		}

		#endregion

		#region Properties

		public override int Count {
			get {
/*
				// The performance on Count depends on the 
				// iterator which is actually used. In some
				// iterators, getting iterator.Count is much
				// faster.

				// With (current) implementation in general,
				// those iterators that requires sorting is
				// likely to have already-computed arrays, so
				// for them getting Count does not impact on
				// performance.
				
				// But by default, getting iterator.Count means
				// it internally iterates all the nodes. That
				// might result in duplicate iteration (so
				// ineffective). So here I decided that it
				// just collects all the nodes to the list.

				if (!finished) {
					BaseIterator iter = iterator as BaseIterator;
					if (iter != null && iter.ReverseAxis || iter is SlashIterator)
						return iter.Count;

					while (iterator.MoveNext ())
						list.Add (((IHasXmlNode) iterator.Current).GetNode ());
					finished = true;
				}
				return list.Count;
*/
				// anyways such code that uses
				// XmlNodeList.Count already gives up 
				// performance. Also, storing things in the
				// list causes extra memory consumption.
				return iterator.Count;
			}
		}

		#endregion

		#region Methods

		public override IEnumerator GetEnumerator ()
		{
			if (finished)
				return list.GetEnumerator ();
			else
				return new XPathNodeIteratorNodeListIterator (source);
//				return new XPathNodeIteratorNodeListIterator2 (this);
		}

		public override XmlNode Item (int index)
		{
			if (index < 0)
				return null;
			if (index < list.Count)
				return (XmlNode) list [index];
			index++;
			while (iterator.CurrentPosition < index) {
				if (!iterator.MoveNext ()) {
					finished = true;
					return null;
				}
				list.Add (((IHasXmlNode) iterator.Current).GetNode ());
			}
			return (XmlNode) list [index - 1];
		}

		#endregion

		class XPathNodeIteratorNodeListIterator : IEnumerator
		{
			XPathNodeIterator iter;
			XPathNodeIterator source;
			public XPathNodeIteratorNodeListIterator (XPathNodeIterator source)
			{
				this.source = source;
				Reset ();
			}

			public bool MoveNext ()
			{
				return iter.MoveNext ();
			}

			public object Current {
				get { return ((IHasXmlNode) iter.Current).GetNode (); }
			}

			public void Reset ()
			{
				iter = source.Clone ();
			}
		}

		/*
		class XPathNodeIteratorNodeListIterator2 : IEnumerator
		{
			int current = -1;
			XmlIteratorNodeList source;

			public XPathNodeIteratorNodeListIterator2 (XmlIteratorNodeList source)
			{
				this.source = source;
			}

			public bool MoveNext ()
			{
				return source [++current] != null;
			}

			public object Current {
				get { return source [current]; }
			}

			public void Reset ()
			{
				current = -1;
			}
		}
		*/
	}
}
