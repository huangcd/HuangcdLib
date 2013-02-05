using System;
using System.Collections;

namespace HuangcdLib.Algorithm.Security
{
    public class MD5Encrypt
    {
        public void Encrypt(String str)
        {
            long[] r = new long[] {
                7, 12, 17, 22,  7, 12, 17, 22,  7, 12, 17, 22,  7, 12, 17, 22,
                5,  9, 14, 20,  5,  9, 14, 20,  5,  9, 14, 20,  5,  9, 14, 20,
                4, 11, 16, 23,  4, 11, 16, 23,  4, 11, 16, 23,  4, 11, 16, 23,
                6, 10, 15, 21,  6, 10, 15, 21,  6, 10, 15, 21,  6, 10, 15, 21,
            };
            long[] k = new long[64];
            for (int i = 0; i < 64; i++)
            {
                k[i] = (long)Math.Floor(Math.Abs(Math.Sin(i + 1) * Math.Pow(2, 32)));
            }
            long h0 = 0x67452301L;
            long h1 = 0xEFCDAB89L;
            long h2 = 0x98BADCFEL;
            long h3 = 0x10325476L;

            byte[] bytes = System.Text.UTF8Encoding.Default.GetBytes(str);
            BitArray bits = new BitArray(bytes);
            int len = bits.Length;
        }

        private void AjustLength(BitArray array)
        {
            int length = array.Length;
            int newLength = array.Length + 1;
            if ((newLength & 512) > 448)
            {
                newLength += 64;
            }
            newLength = ((newLength >> 9) << 9) | 448;
            array.Length = newLength;
            array.Set(length, true);
            for (int i = length + 1; i < newLength; i++)
            {
                array.Set(length, false);
            }
        }
    }
}