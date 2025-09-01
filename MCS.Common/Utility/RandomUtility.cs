using System;
using System.Security.Cryptography;

namespace MCS.Common.Utility
{
    public class RandomUtility
    {
        // The random number provider.
        private static RNGCryptoServiceProvider _rand = new RNGCryptoServiceProvider();

        // Return a random integer between a min and max value.
        public static int RandomInteger(int min, int max)
        {
            uint scale = uint.MaxValue;
            while (scale == uint.MaxValue)
            {
                // Get four random bytes.
                byte[] four_bytes = new byte[4];
                _rand.GetBytes(four_bytes);

                // Convert that into an uint.
                scale = BitConverter.ToUInt32(four_bytes, 0);
            }

            // Add min to the scaled difference between max and min.
            return (int)(min + (max - min) *
                (scale / (double)uint.MaxValue));
        }
    }
}
