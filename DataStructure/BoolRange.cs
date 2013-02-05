using System;
using System.Collections.Generic;
using System.Collections;

namespace HuangcdLib.DataStructure
{
    public class BoolRange : IEnumerable<BoolRange>
    {
        private int __from;
        private int __to;
        private BoolRange left;
        private BoolRange right;
        private bool split;
        private bool value;

        public BoolRange(int size, bool value = false)
            : this(0, size, value)
        {
        }

        public BoolRange(int from, int to, bool value = false)
        {
            this.__from = from;
            this.__to = to;
            this.value = value;
            this.split = false;
        }

        public IEnumerator<BoolRange> GetEnumerator()
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

        public void Flip(int from, int to)
        {
            if (this.__to < from || this.__from > to)
            {
                return;
            }
            if (split)
            {
                left.Flip(from, to);
                right.Flip(from, to);

                // Merge
                if (!left.split && !right.split && left.value == right.value)
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
                value = !value;
                return;
            }

            // Split
            int leftEmpty = from - __from;
            int rightEmpty = __to - to;
            if (leftEmpty < rightEmpty)
            {
                left = new BoolRange(__from, to, value);
                right = new BoolRange(to + 1, __to, value);
                left.Flip(from, to);
            }
            else
            {
                left = new BoolRange(__from, from - 1, value);
                right = new BoolRange(from, __to, value);
                right.Flip(from, to);
            }
            split = true;
        }

        public int Query(int from, int to)
        {
            if (this.__to < from || this.__from > to)
            {
                return 0;
            }
            if (split)
            {
                return left.Query(from, to) + right.Query(from, to);
            }
            from = Math.Max(from, this.__from);
            to = Math.Min(to, this.__to);
            if (!value)
            {
                return 0;
            }
            return to + 1 - from;
        }
    }
}