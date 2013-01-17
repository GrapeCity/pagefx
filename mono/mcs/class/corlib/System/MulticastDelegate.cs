//CHANGED
//
// System.MultiCastDelegate.cs
//
// Authors:
//   Miguel de Icaza (miguel@ximian.com)
//   Daniel Stodden (stodden@in.tum.de)
//
// (C) Ximian, Inc.  http://www.ximian.com
//

//
// Copyright (C) 2004 Novell, Inc (http://www.novell.com)
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

using System.Collections;

namespace System
{
    public abstract class MulticastDelegate : Delegate
    {
        internal MulticastDelegate m_prev;
        internal MulticastDelegate m_next;

        public override void AddEventListeners(object dispatcher, string eventName)
        {
            if (m_prev != null)
                m_prev.AddEventListeners(dispatcher, eventName);
            base.AddEventListeners(dispatcher, eventName);
        }

        public override void RemoveEventListeners(object dispatcher, string eventName)
        {
            if (m_prev != null)
                m_prev.RemoveEventListeners(dispatcher, eventName);
            base.RemoveEventListeners(dispatcher, eventName);
        }

        public override int GetInvocationCount()
        {
            int n = 0;
            if (m_prev != null)
                n += m_prev.GetInvocationCount();
            n += base.GetInvocationCount();
            return n;
        }

        protected override sealed object DynamicInvokeImpl(object[] args)
        {
            if (m_prev != null)
                m_prev.DynamicInvokeImpl(args);
            return base.DynamicInvokeImpl(args);
        }

        // <remarks>
        //   Equals: two multicast delegates are equal if their base is equal
        //   and their invocations list is equal.
        // </remarks>
        public override sealed bool Equals(object o)
        {
            if (!base.Equals(o))
                return false;

            MulticastDelegate d = (MulticastDelegate)o;

            if (m_prev == null)
            {
                return d.m_prev == null;
            }

            return m_prev.Equals(d.m_prev);
        }

        // <summary>
        //   Return, in order of invocation, the invocation list
        //   of a MulticastDelegate
        // </summary>
        public override sealed Delegate[] GetInvocationList()
        {
            MulticastDelegate d;
            d = (MulticastDelegate)Clone();
            for (d.m_next = null; d.m_prev != null; d = d.m_prev)
                d.m_prev.m_next = d;

            if (d.m_next == null)
            {
                MulticastDelegate other = (MulticastDelegate)d.Clone();
                other.m_prev = null;
                other.m_next = null;
                return new Delegate[1] {other};
            }

            ArrayList list = new ArrayList();
            for (; d != null; d = d.m_next)
            {
                MulticastDelegate other = (MulticastDelegate)d.Clone();
                other.m_prev = null;
                other.m_next = null;
                list.Add(other);
            }

            return (Delegate[])list.ToArray(typeof(Delegate));
        }

        // <summary>
        //   Combines this MulticastDelegate with the (Multicast)Delegate `follow'.
        //   This does _not_ combine with Delegates. ECMA states the whole delegate
        //   thing should have better been a simple System.Delegate class.
        //   Compiler generated delegates are always MulticastDelegates.
        // </summary>
        protected override sealed Delegate CombineImpl(Delegate follow)
        {
            MulticastDelegate combined, orig, clone;

            if (GetType() != follow.GetType())
                throw new ArgumentException(Locale.GetText("Incompatible Delegate Types."));

            combined = (MulticastDelegate)follow.Clone();
            //combined.SetMulticastInvoke();

            for (clone = combined, orig = ((MulticastDelegate)follow).m_prev; orig != null; orig = orig.m_prev)
            {
                clone.m_prev = (MulticastDelegate)orig.Clone();
                clone = clone.m_prev;
            }

            clone.m_prev = (MulticastDelegate)Clone();

            for (clone = clone.m_prev, orig = m_prev; orig != null; orig = orig.m_prev)
            {
                clone.m_prev = (MulticastDelegate)orig.Clone();
                clone = clone.m_prev;
            }

            return combined;
        }

        private bool BaseEquals(MulticastDelegate value)
        {
            return base.Equals(value);
        }

        /* 
         * Perform a slightly crippled version of
         * Knuth-Pratt-Morris over MulticastDelegate chains.
         * Border values are set as pointers in kpm_next;
         * Generally, KPM border arrays are length n+1 for
         * strings of n. This one works with length n at the
         * expense of a few additional comparisions.
         */

        private static MulticastDelegate KPM(MulticastDelegate needle, MulticastDelegate haystack,
                                             out MulticastDelegate tail)
        {
            MulticastDelegate nx, hx;

            // preprocess
            hx = needle;
            nx = needle.m_next = null;
            do
            {
                while ((nx != null) && (!nx.BaseEquals(hx)))
                    nx = nx.m_next;

                hx = hx.m_prev;
                if (hx == null)
                    break;

                nx = nx == null ? needle : nx.m_prev;
                if (hx.BaseEquals(nx))
                    hx.m_next = nx.m_next;
                else
                    hx.m_next = nx;
            } while (true);

            // match
            MulticastDelegate match = haystack;
            nx = needle;
            hx = haystack;
            do
            {
                while (nx != null && !nx.BaseEquals(hx))
                {
                    nx = nx.m_next;
                    match = match.m_prev;
                }

                nx = nx == null ? needle : nx.m_prev;
                if (nx == null)
                {
                    // bingo
                    tail = hx.m_prev;
                    return match;
                }

                hx = hx.m_prev;
            } while (hx != null);

            tail = null;
            return null;
        }

        protected override sealed Delegate RemoveImpl(Delegate value)
        {
            if (value == null)
                return this;

            // match this with value
            MulticastDelegate head, tail;
            head = KPM((MulticastDelegate)value, this, out tail);
            if (head == null)
                return this;

            // duplicate chain without head..tail
            MulticastDelegate prev = null, retval = null, orig;
            for (orig = this; (object)orig != (object)head; orig = orig.m_prev)
            {
                MulticastDelegate clone = (MulticastDelegate)orig.Clone();
                if (prev != null)
                    prev.m_prev = clone;
                else
                    retval = clone;
                prev = clone;
            }
            for (orig = tail; (object)orig != null; orig = orig.m_prev)
            {
                MulticastDelegate clone = (MulticastDelegate)orig.Clone();
                if (prev != null)
                    prev.m_prev = clone;
                else
                    retval = clone;
                prev = clone;
            }
            if (prev != null)
                prev.m_prev = null;

            return retval;
        }

        public static bool operator ==(MulticastDelegate a, MulticastDelegate b)
        {
            if (a == null)
                return b == null;

            return a.Equals(b);
        }

        public static bool operator !=(MulticastDelegate a, MulticastDelegate b)
        {
            if (a == null)
                return b != null;

            return !a.Equals(b);
        }

        public override object Clone()
        {
            MulticastDelegate d = (MulticastDelegate)CreateInstance();
            d.m_target = m_target;
            d.m_function = m_function;
            d.m_prev = m_prev;
            d.m_next = m_next;
            return d;
        }
    }
}