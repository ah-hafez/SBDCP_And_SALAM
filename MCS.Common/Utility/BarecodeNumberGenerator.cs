using System;
using System.Collections;
using MCS.Common.Utility;

namespace MCS.Common
{
    public static class BarecodeNumberGenerator
    {
        private const int PART1 = 4;
        private const int PART2 = 4;
        private const int PART3 = 4;

        public static string Generate(int itemId, TransactionCategory transactionType)
        {
            string identity = itemId.ToString("0000000000");
            string barcode = EncodeDecodeBarcode(Convert.ToInt64(identity)).ToString("0000000000");
            string type = Convert.ToInt32(transactionType).ToString("00");

            Random random = new Random();
            string rndNo = RandomUtility.RandomInteger(0, 10000).ToString("0000");

            barcode = barcode.Insert(0, rndNo.Substring(0, 2));
            barcode = barcode.Insert(PART1, type.Substring(0, 1));
            barcode = barcode.Insert(PART1 + PART2, rndNo.Substring(1, 2));
            barcode = barcode.Insert(PART1 + PART2 + PART3, type.Substring(1, 1));

            return barcode;
        }

        public static string GenerateForAttachment(int OrgUnitId, long transactionNumber, int attachmentId, int year)
        {
            return string.Format("{0}{1}{2}{3}", OrgUnitId, transactionNumber, attachmentId, year);
        }

        private static long EncodeDecodeBarcode(long value)
        {
            bool bt1;
            byte xorMask = 0x9;

            byte[] binary = BitConverter.GetBytes(value);

            // Mask the bit values
            for (int i = 0; i < binary.Length; i++)
            {
                if (binary[i] != 0)
                    binary[i] = (byte)(binary[i] ^ xorMask);
            }

            // Bit Swapping               
            BitArray bitArray = new BitArray(binary);

            for (int i = 0; i < bitArray.Length - 1; i += 2)
            {
                bt1 = bitArray[i];

                bitArray[i] = bitArray[i + 1];
                bitArray[i + 1] = bt1;
            }

            // return back to long
            return BitConverter.ToInt64(binary, 0);
        }
    }
}
