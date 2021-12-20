using System;
using System.Collections;

namespace AdventOfCode2021
{
    public static class Extensions
    {
        /// <summary>
        ///     Convert BitArray value to Int32
        /// </summary>
        /// <param name="ar"></param>
        /// <returns></returns>
        public static int ToInt(this BitArray ar)
        {
            int len = Math.Min(32, ar.Count);
            int result = 0;

            for (int i = 0; i < len; i++)
                if (ar.Get(i))
                    result |= 1 << i;

            return result;
        }

        /// <summary>
        ///     Reverse a BitArray (e.g. 1100 -> 0011)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static BitArray Reverse(this BitArray input)
        {
            BitArray result = new(input.Length);

            int i;
            int x;

            for (i = input.Length - 1, x = 0; i >= 0; i--, x++)
                result[x] = input[i];

            return result;
        }

        /// <summary>
        ///     Calculate median from sequence of integers.
        /// </summary>
        /// <param name="input"></param>
        /// <returns>Median value</returns>
        public static int Median(this int[] input)
        {
            int[] arr = new int[input.Length];

            Array.Copy(input, 0, arr, 0, input.Length);
            Array.Sort(arr);

            if (arr.Length % 2 == 0)
                return (arr[arr.Length / 2] + arr[(arr.Length + 1) / 2]) / 2;

            return arr[(arr.Length + 1) / 2];
        }
    }
}