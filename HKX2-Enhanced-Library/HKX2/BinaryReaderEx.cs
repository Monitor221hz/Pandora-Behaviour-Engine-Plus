using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;

namespace HKX2E
{
    public class BinaryReaderEx
    {
        private readonly BinaryReader br;
        private readonly Stack<long> steps;

        public BinaryReaderEx(byte[] input) : this(false, false, input)
        {
        }

        public BinaryReaderEx(Stream stream) : this(false, false, stream)
        {
        }

        public BinaryReaderEx(bool bigEndian, bool uSizeLong, byte[] input) : this(bigEndian, uSizeLong,
            new MemoryStream(input))
        {
        }

        public BinaryReaderEx(bool bigEndian, bool uSizeLong, Stream stream)
        {
            BigEndian = bigEndian;
            USizeLong = uSizeLong;
            steps = new Stack<long>();
            Stream = stream;
            br = new BinaryReader(stream);
        }

        public bool BigEndian { get; set; }

        public bool USizeLong { get; }

        public Stream Stream { get; }

        public long Position
        {
            get => Stream.Position;
            set => Stream.Position = value;
        }

        public long Length => Stream.Length;


        public byte[] ReadBytes(int count)
        {
            var result = br.ReadBytes(count);
            if (result.Length != count)
                throw new EndOfStreamException("Remaining size of stream was smaller than requested number of bytes.");
            return result;
        }

        private byte[] ReadReversedBytes(int length)
        {
            var bytes = ReadBytes(length);
            Array.Reverse(bytes);
            return bytes;
        }

        private T AssertValue<T>(T value, string typeName, string valueFormat, T[] options) where T : IEquatable<T>
        {
            foreach (var option in options)
                if (value.Equals(option))
                    return value;

            var strValue = string.Format(valueFormat, value);
            var strOptions = string.Join(", ", options.Select(o => string.Format(valueFormat, o)));
            throw new InvalidDataException(
                $"Read {typeName}: {strValue} | Expected: {strOptions} | Ending position: 0x{Position:X}");
        }

        public void StepIn(long offset)
        {
            steps.Push(Stream.Position);
            Stream.Position = offset;
        }

        public void StepOut()
        {
            if (steps.Count == 0)
                throw new InvalidOperationException("Reader is already stepped all the way out.");

            Stream.Position = steps.Pop();
        }

        public void Pad(int align)
        {
            if (Stream.Position % align > 0)
                Stream.Position += align - Stream.Position % align;
        }

        public ulong ReadUSize()
        {
            if (USizeLong)
                return ReadUInt64();
            return ReadUInt32();
        }

        public ulong AssertUSize(params ulong[] options)
        {
            return AssertValue(ReadUSize(), USizeLong ? "USize64" : "USize32", "0x{0:X}", options);
        }

        #region Other

        public Vector4 ReadVector4()
        {
            var x = ReadSingle();
            var y = ReadSingle();
            var z = ReadSingle();
            var w = ReadSingle();
            return new Vector4(x, y, z, w);
        }

        #endregion

        #region Boolean

        public bool ReadBoolean()
        {
            var b = ReadByte();

            return b switch
            {
                0 => false,
                1 => true,
                _ => throw new InvalidDataException($"ReadBoolean encountered non-boolean value: 0x{b:X2}")
            };
        }

        public bool AssertBoolean(bool option)
        {
            return AssertValue(ReadBoolean(), "Boolean", "{0}", new[] { option });
        }

        #endregion

        #region SByte

        public sbyte ReadSByte()
        {
            return (sbyte)ReadByte();
        }

        public sbyte AssertSByte(params sbyte[] options)
        {
            return AssertValue(ReadSByte(), "SByte", "0x{0:X}", options);
        }

        #endregion

        #region Byte

        public byte ReadByte()
        {
            return ReadBytes(1)[0];
        }

        public byte AssertByte(params byte[] options)
        {
            return AssertValue(ReadByte(), "Byte", "0x{0:X}", options);
        }

        #endregion

        #region Int16

        public short ReadInt16()
        {
            if (BigEndian)
                return BitConverter.ToInt16(ReadReversedBytes(2));
            return br.ReadInt16();
        }

        public short AssertInt16(params short[] options)
        {
            return AssertValue(ReadInt16(), "Int16", "0x{0:X}", options);
        }

        #endregion

        #region UInt16

        public ushort ReadUInt16()
        {
            if (BigEndian)
                return BitConverter.ToUInt16(ReadReversedBytes(2), 0);
            return br.ReadUInt16();
        }

        public ushort AssertUInt16(params ushort[] options)
        {
            return AssertValue(ReadUInt16(), "UInt16", "0x{0:X}", options);
        }

        #endregion

        #region Int32

        public int ReadInt32()
        {
            if (BigEndian)
                return BitConverter.ToInt32(ReadReversedBytes(4), 0);
            return br.ReadInt32();
        }

        public int AssertInt32(params int[] options)
        {
            return AssertValue(ReadInt32(), "Int32", "0x{0:X}", options);
        }

        #endregion

        #region UInt32

        public uint ReadUInt32()
        {
            if (BigEndian)
                return BitConverter.ToUInt32(ReadReversedBytes(4), 0);
            return br.ReadUInt32();
        }

        public uint AssertUInt32(params uint[] options)
        {
            return AssertValue(ReadUInt32(), "UInt32", "0x{0:X}", options);
        }

        #endregion

        #region Int64

        public long ReadInt64()
        {
            if (BigEndian)
                return BitConverter.ToInt64(ReadReversedBytes(8), 0);
            return br.ReadInt64();
        }

        public long AssertInt64(params long[] options)
        {
            return AssertValue(ReadInt64(), "Int64", "0x{0:X}", options);
        }

        #endregion

        #region UInt64

        public ulong ReadUInt64()
        {
            if (BigEndian)
                return BitConverter.ToUInt64(ReadReversedBytes(8), 0);
            return br.ReadUInt64();
        }

        public ulong AssertUInt64(params ulong[] options)
        {
            return AssertValue(ReadUInt64(), "UInt64", "0x{0:X}", options);
        }

        #endregion

        #region Half

        public Half ReadHalf()
        {
            if (BigEndian)
                return BitConverter.ToHalf(ReadReversedBytes(2), 0);
            return br.ReadHalf();
        }

        public Half AssertHalf(params Half[] options)
        {
            return AssertValue(ReadHalf(), "Half", "{0}", options);
        }

        #endregion

        #region Single

        private float RoundSignle(float d)
        {
            var s = Math.Round(d - Math.Truncate(d), 6).ToString("F6");
            s = s[(s.IndexOf(".") + 1)..];
            s = $"{Math.Truncate(d):F0}.{s}";
            return float.Parse(s);
        }

        public float ReadSingle()
        {
            // XXX: NaN(0xFFC0000) to 0.
            // XXX: round? to 6 deciaml
            if (BigEndian)
            {
                var revVal = BitConverter.ToSingle(ReadReversedBytes(4), 0);
                return float.IsNaN(revVal) ? 0 : (float)Math.Round(revVal, 6);
            }
            var val = br.ReadSingle();
            return float.IsNaN(val) ? 0 : (float)Math.Round(val, 6);
        }

        public float AssertSingle(params float[] options)
        {
            return AssertValue(ReadSingle(), "Single", "{0}", options);
        }

        #endregion

        #region Double

        public double ReadDouble()
        {
            if (BigEndian)
                return BitConverter.ToDouble(ReadReversedBytes(8), 0);
            return br.ReadDouble();
        }

        public double AssertDouble(params double[] options)
        {
            return AssertValue(ReadDouble(), "Double", "{0}", options);
        }

        #endregion

        #region String

        private string ReadChars(Encoding encoding, int length)
        {
            var bytes = ReadBytes(length);
            return encoding.GetString(bytes);
        }

        private string ReadCharsTerminated(Encoding encoding)
        {
            var bytes = new List<byte>();

            var b = ReadByte();
            while (b != 0)
            {
                bytes.Add(b);
                b = ReadByte();
            }

            return encoding.GetString(bytes.ToArray());
        }

        public string ReadASCII()
        {
            return ReadCharsTerminated(Encoding.ASCII);
        }

        public string ReadASCII(int length)
        {
            return ReadChars(Encoding.ASCII, length);
        }

        public string ReadFixStr(int size)
        {
            var bytes = ReadBytes(size);
            int terminator;
            for (terminator = 0; terminator < size; terminator++)
                if (bytes[terminator] == 0)
                    break;

            return Encoding.ASCII.GetString(bytes, 0, terminator);
        }

        #endregion
    }
}