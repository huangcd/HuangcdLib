using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace HuangcdLib.DataStructure
{
    public class Heap<TElem>
        where TElem : IComparable<TElem>
    {
        private const int MIN_CAPACITY = 8;
        private TElem[] elems;
        private int capacity;
        private int size;

        public int Count
        {
            get { return size; }
        }

        public Heap()
        {
            elems = new TElem[MIN_CAPACITY];
        }

        public Heap(int capacity)
        {
            elems = new TElem[EusurePower(capacity)];
        }

        public Heap(IEnumerable<TElem> iter)
            : this()
        {
            foreach (var item in iter)
            {
                Add(item);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void Swap<TElem>(ref TElem i, ref TElem j)
        {
            TElem temp = i;
            i = j;
            j = temp;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Swap(int i, int j)
        {
            TElem temp = elems[i];
            elems[i] = elems[j];
            elems[j] = temp;
        }

        public void Add(TElem elem)
        {
            elems[size] = elem;
            int childIndex = size;
            int fatherIndex = GetFatherIndex(childIndex);
            while (childIndex > 0 && elems[size].CompareTo(elems[fatherIndex]) < 0)
            {
                Swap(ref elems[childIndex], ref elems[fatherIndex]);
                childIndex = fatherIndex;
                fatherIndex = GetFatherIndex(childIndex);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int GetFatherIndex(int index)
        {
            return (index - 1) >> 1;
        }

        public TElem Remove()
        {
            return default(TElem);
        }

        private int EusurePower(int capacity)
        {
            if (capacity <= MIN_CAPACITY)
                return MIN_CAPACITY;
            int count = 0;
            while (capacity != 1)
            {
                count++;
                capacity >>= 1;
            }
            while (count != 0)
            {
                count--;
                capacity <<= 1;
            }
            return capacity;
        }
    }
}