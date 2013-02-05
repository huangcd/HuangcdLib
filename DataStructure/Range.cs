using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HuangcdLib.DataStructure
{
    public class Range<T> : IEnumerable<Range<T>>
    {
        private int __from;
        private int __to;
        private Range<T> left;
        private Range<T> right;
        private bool split;
        private T value;

        public Range(int size, T value = default(T))
            : this(0, size, value)
        {
        }

        public Range(int from, int to, T value = default(T))
        {
            this.__from = from;
            this.__to = to;
            this.value = value;
            this.split = false;
        }

        public virtual IEnumerator<Range<T>> GetEnumerator()
        {
            if (split)
            {
                yield return this;
            }
            else
            {
                foreach (var item in left)
                {
                    yield return item;
                }
                foreach (var item in right)
                {
                    yield return item;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public int CountLeavesNode()
        {
            return split ? left.CountLeavesNode() + right.CountLeavesNode() : 1;
        }

        public virtual void Set(int from, int to, T newValue)
        {
            if (this.__to < from || this.__from > to)
            {
                return;
            }
            if (split)
            {
                left.Set(from, to, newValue);
                right.Set(from, to, newValue);

                // Merge
                if (!left.split && !right.split && left.value.Equals(right.value))
                {
                    split = false;
                    value = left.value;
                }
                return;
            }
            from = Math.Max(from, this.__from);
            to = Math.Min(to, this.__to);
            if (from == this.__from && to == this.__to)
            {
                value = newValue;
                return;
            }

            // Split
            int leftEmpty = from - __from;
            int rightEmpty = __to - to;
            if (leftEmpty < rightEmpty)
            {
                left = new Range<T>(__from, to, value);
                right = new Range<T>(to + 1, __to, value);
                left.Set(from, to, newValue);
            }
            else
            {
                left = new Range<T>(__from, from - 1, value);
                right = new Range<T>(from, __to, value);
                right.Set(from, to, newValue);
            }
            split = true;
        }

        public virtual int Query(int from, int to, T queryValue)
        {
            if (this.__to < from || this.__from > to)
            {
                return 0;
            }
            if (split)
            {
                return left.Query(from, to, queryValue) + right.Query(from, to, queryValue);
            }
            from = Math.Max(from, this.__from);
            to = Math.Min(to, this.__to);
            if (!value.Equals(queryValue))
            {
                return 0;
            }
            return to + 1 - from;
        }
    }
}
