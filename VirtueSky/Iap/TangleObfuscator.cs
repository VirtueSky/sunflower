#if UNITY_EDITOR
using System;
using System.Linq;

namespace VirtueSky.Iap
{
    public static class TangleObfuscator
    {
        /// <summary>
        /// An Exception thrown when the tangle order array provided is invalid or shorter than the number of data slices made.
        /// </summary>
        public class InvalidOrderArray : Exception
        {
        }

        /// <summary>
        /// Generates the obfucscation tangle data.
        /// </summary>
        /// <param name="data"> The Apple or GooglePlay public key data to be obfuscated. </param>
        /// <param name="order"> The array, passed by reference, of order of the data slices used to obfuscate the data with. </param>
        /// <param name="rkey"> Outputs the encryption key to deobfuscate the tangled data at runtime </param>
        /// <returns>The obfucated public key</returns>
        public static byte[] Obfuscate(byte[] data, int[] order, out int rkey)
        {
            var rnd = new System.Random();
            var key = rnd.Next(2, 255);
            var res = new byte[data.Length];
            var slices = data.Length / 20 + 1;

            if (order == null || order.Length < slices)
            {
                throw new InvalidOrderArray();
            }

            Array.Copy(data, res, data.Length);
            for (var i = 0; i < slices - 1; i++)
            {
                var j = rnd.Next(i, slices - 1);
                order[i] = j;
                var sliceSize = 20; // prob should be configurable
                var tmp = res.Skip(i * 20).Take(sliceSize).ToArray(); // tmp = res[i*20 .. slice]
                Array.Copy(res,
                    j * 20,
                    res,
                    i * 20,
                    sliceSize); // res[i] = res[j*20 .. slice]
                Array.Copy(tmp,
                    0,
                    res,
                    j * 20,
                    sliceSize); // res[j] = tmp
            }

            order[slices - 1] = slices - 1;

            rkey = key;
            return res.Select(x => (byte)(x ^ key)).ToArray();
        }
    }
}
#endif