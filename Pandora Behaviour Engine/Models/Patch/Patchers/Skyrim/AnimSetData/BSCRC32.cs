using System;
using System.Text;
using static Nito.HashAlgorithms.CRC32;
namespace Pandora.Patch.Patchers.Skyrim.AnimSetData
{
	public static class BSCRC32
	{
		public static Definition BSDefinition = new Definition
		{
			Initializer = 0,
			TruncatedPolynomial = 0x04C11DB7,
			FinalXorValue = 0,
			ReverseResultBeforeFinalXor = true,
			ReverseDataBytes = true
		};

		public static byte[] GetValue(byte[] bytes)
		{

			using (var alg = new Nito.HashAlgorithms.CRC32(BSDefinition))
			{
				byte[] crc32 = alg.ComputeHash(bytes);
				return crc32;
			}
		}

		public static UInt32 GetValueUInt32(string str) => BitConverter.ToUInt32(GetValue(Encoding.ASCII.GetBytes(str)));
		public static string GetValueString(string str) => GetValueUInt32(str).ToString();
	}
}
