using HKX2E.Utils;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace HKX2E
{
    public class PackFileDeserializer
    {
        private HKXClassNames _classnames;
        private HKXSection _classSection;
        private HKXSection _dataSection;

        private Dictionary<uint, IHavokObject> _deserializedObjects;
        public HKXHeader _header;
        private HKXSection _typeSection;

        private bool _ignoreNonFatalError;

        private IHavokObject ConstructVirtualClass(BinaryReaderEx br, uint offset)
        {
            if (_deserializedObjects.ContainsKey(offset)) return _deserializedObjects[offset];

            var fixup = _dataSection._virtualMap[offset];
            var hkClassName = _classnames.OffsetClassNamesMap[fixup.Dst].ClassName;

            var hkClass = System.Type.GetType($@"HKX2E.{hkClassName}");
            if (hkClass is null) throw new Exception($@"Havok class type '{hkClassName}' not found!");

            var ret = (IHavokObject)Activator.CreateInstance(hkClass)!;
            if (ret is null) throw new Exception($@"Failed to Activator.CreateInstance({hkClass})");

            br.StepIn(offset);
            ret.Read(this, br);
            br.StepOut();

            _deserializedObjects.Add(offset, ret);
            return ret;
        }

        public void DeserializePartially(BinaryReaderEx br)
        {
            br.StepIn(0x11);
            br.BigEndian = br.ReadByte() == 0x0;
            br.StepOut();

            _header = new HKXHeader(br);

            // Read the 3 sections in the file
            if (_header.SectionOffset == -1)
            {
                br.Position = 0x40;
            }
            else
            {
                br.Position = _header.SectionOffset + 0x40;
            }

            _classSection = new HKXSection(br, _header.ContentsVersionString) { SectionID = 0 };
            _typeSection = new HKXSection(br, _header.ContentsVersionString) { SectionID = 1 };
            _dataSection = new HKXSection(br, _header.ContentsVersionString) { SectionID = 2 };

            // Process the class names
            _classnames = _classSection.ReadClassnames(br);
        }

        public IHavokObject Deserialize(BinaryReaderEx br, bool ignoreNonFatalError = false)
        {
            _ignoreNonFatalError = ignoreNonFatalError;

            DeserializePartially(br);

            // Deserialize the objects
            _deserializedObjects = new Dictionary<uint, IHavokObject>();
            var br2 = new BinaryReaderEx(_header.Endian == 0, _header.PointerSize == 8, _dataSection.SectionData);
            return ConstructVirtualClass(br2, 0);
        }

        #region Read methods

        private void PadToPointerSizeIfPaddingOption(BinaryReaderEx br)
        {
            if (_header.PaddingOption == 1) br.Pad(_header.PointerSize);
        }

        public void ReadEmptyPointer(BinaryReaderEx br)
        {
            PadToPointerSizeIfPaddingOption(br);
            br.AssertUSize(0);
        }

        public void ReadEmptyArray(BinaryReaderEx br)
        {
            ReadEmptyPointer(br);
            var size = br.ReadUInt32();
            br.AssertUInt32(size | ((uint)0x80 << 24));
        }

        private T ReadPointerBase<T, F>(Func<BinaryReaderEx, F, T> func, BinaryReaderEx br) where F : Fixup, new()
        {
            Dictionary<uint, F> map;
            if (typeof(F) == typeof(LocalFixup))
                map = (Dictionary<uint, F>)(object)_dataSection._localMap;
            else if (typeof(F) == typeof(GlobalFixup))
                map = (Dictionary<uint, F>)(object)_dataSection._localMap;
            else
                throw new Exception();

            PadToPointerSizeIfPaddingOption(br);

            var key = (uint)br.Position;

            br.AssertUSize(0);

            if (!map.ContainsKey(key))
            {
                // If the read type is an array, continue like normal
                var oType = typeof(T);
                if (oType.IsGenericType && oType.GetGenericTypeDefinition() == typeof(List<>)) return func(br, new F());

                return default!;
            }

            var f = map[key];
            return func(br, f);
        }

        private List<T> ReadArrayBase<T>(Func<BinaryReaderEx, T> func, BinaryReaderEx br)
        {
            return ReadPointerBase((BinaryReaderEx _br, LocalFixup f) =>
            {
                var size = _br.ReadUInt32();
                _br.AssertUInt32(size | ((uint)0x80 << 24)); // Capacity and flags

                var res = new List<T>();

                if (size <= 0) return res;

                _br.StepIn(f.Dst);
                for (var i = 0; i < size; i++) res.Add(func(_br));

                _br.StepOut();

                return res;
            }, br);
        }

        public List<T> ReadClassArray<T>(BinaryReaderEx br) where T : IHavokObject, new()
        {
            return ReadArrayBase(_br =>
            {
                var cls = new T();
                cls.Read(this, _br);
                return cls;
            }, br);
        }

        public T ReadClassPointer<T>(BinaryReaderEx br) where T : IHavokObject
        {
            PadToPointerSizeIfPaddingOption(br);

            var key = (uint)br.Position;

            // Consume pointer
            br.AssertUSize(0);

            // Do a global fixup lookup
            if (!_dataSection._globalMap.ContainsKey(key)) return default!;

            var f = _dataSection._globalMap[key];
            var klass = ConstructVirtualClass(br, f.Dst);

            if (klass.GetType().IsAssignableTo(typeof(T)))
                return (T)klass;

            if (!_ignoreNonFatalError)
                throw new Exception($@"Could not convert '{typeof(T)}' to '{klass.GetType()}'. Is source malformed?");

            // TODO: this assume klass was read on correct block
            return hkDummyBuilder.CreateDummy(klass, typeof(T));
        }

        public List<T> ReadClassPointerArray<T>(BinaryReaderEx br) where T : IHavokObject
        {
            return ReadArrayBase(ReadClassPointer<T>, br);
        }

        public string ReadStringPointer(BinaryReaderEx br)
        {
            PadToPointerSizeIfPaddingOption(br);

            var key = (uint)br.Position;

            // Consume pointer
            br.AssertUSize(0);

            // Do a local fixup lookup
            if (!_dataSection._localMap.ContainsKey(key)) return string.Empty;

            var f = _dataSection._localMap[key];
            br.StepIn(f.Dst);
            var ret = br.ReadASCII();
            br.StepOut();
            return ret.Trim();
        }

        public List<string> ReadStringPointerArray(BinaryReaderEx br)
        {
            return ReadArrayBase(ReadStringPointer, br);
        }

        public string ReadCString(BinaryReaderEx br)
        {
            PadToPointerSizeIfPaddingOption(br);

            var key = (uint)br.Position;

            // Consume pointer
            br.AssertUSize(0);

            // Do a local fixup lookup
            if (!_dataSection._localMap.ContainsKey(key)) return null;

            var f = _dataSection._localMap[key];
            br.StepIn(f.Dst);
            var ret = br.ReadASCII();
            br.StepOut();
            if (ret == "")
                return null;
            return ret;
        }

        public List<string> ReadCStringArray(BinaryReaderEx br)
        {
            return ReadArrayBase(ReadCString, br);
        }

        public byte ReadByte(BinaryReaderEx br)
        {
            return br.ReadByte();
        }

        public List<byte> ReadByteArray(BinaryReaderEx br)
        {
            return ReadArrayBase(ReadByte, br);
        }

        public sbyte ReadSByte(BinaryReaderEx br)
        {
            return br.ReadSByte();
        }

        public List<sbyte> ReadSByteArray(BinaryReaderEx br)
        {
            return ReadArrayBase(ReadSByte, br);
        }

        public ushort ReadUInt16(BinaryReaderEx br)
        {
            return br.ReadUInt16();
        }

        public List<ushort> ReadUInt16Array(BinaryReaderEx br)
        {
            return ReadArrayBase(ReadUInt16, br);
        }


        public short ReadInt16(BinaryReaderEx br)
        {
            return br.ReadInt16();
        }

        public List<short> ReadInt16Array(BinaryReaderEx br)
        {
            return ReadArrayBase(ReadInt16, br);
        }

        public uint ReadUInt32(BinaryReaderEx br)
        {
            return br.ReadUInt32();
        }

        public List<uint> ReadUInt32Array(BinaryReaderEx br)
        {
            return ReadArrayBase(ReadUInt32, br);
        }

        public int ReadInt32(BinaryReaderEx br)
        {
            return br.ReadInt32();
        }

        public List<int> ReadInt32Array(BinaryReaderEx br)
        {
            return ReadArrayBase(ReadInt32, br);
        }

        public ulong ReadUInt64(BinaryReaderEx br)
        {
            return br.ReadUInt64();
        }

        public List<ulong> ReadUInt64Array(BinaryReaderEx br)
        {
            return ReadArrayBase(ReadUInt64, br);
        }

        public long ReadInt64(BinaryReaderEx br)
        {
            return br.ReadInt64();
        }

        public List<long> ReadInt64Array(BinaryReaderEx br)
        {
            return ReadArrayBase(ReadInt64, br);
        }

        public Half ReadHalf(BinaryReaderEx br)
        {
            return br.ReadHalf();
        }

        public List<Half> ReadHalfArray(BinaryReaderEx br)
        {
            return ReadArrayBase(ReadHalf, br);
        }

        public float ReadSingle(BinaryReaderEx br)
        {
            return br.ReadSingle();
        }

        public List<float> ReadSingleArray(BinaryReaderEx br)
        {
            return ReadArrayBase(ReadSingle, br);
        }

        public bool ReadBoolean(BinaryReaderEx br)
        {
            return br.ReadBoolean();
        }

        public List<bool> ReadBooleanArray(BinaryReaderEx br)
        {
            return ReadArrayBase(ReadBoolean, br);
        }

        public Vector4 ReadVector4(BinaryReaderEx br)
        {
            return br.ReadVector4();
        }

        public List<Vector4> ReadVector4Array(BinaryReaderEx br)
        {
            return ReadArrayBase(ReadVector4, br);
        }

        public Matrix4x4 ReadMatrix3(BinaryReaderEx br)
        {
            var mat3 = new Matrix4x4(
                br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle(),
                br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle(),
                br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle(),
                0, 0, 0, 0)
            {
                M14 = 0,
                M24 = 0,
                M34 = 0
            };
            return mat3;
        }

        public List<Matrix4x4> ReadMatrix3Array(BinaryReaderEx br)
        {
            return ReadArrayBase(ReadMatrix3, br);
        }

        public Matrix4x4 ReadMatrix4(BinaryReaderEx br)
        {
            return new Matrix4x4(
                br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle(),
                br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle(),
                br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle(),
                br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
        }

        public List<Matrix4x4> ReadMatrix4Array(BinaryReaderEx br)
        {
            return ReadArrayBase(ReadMatrix4, br);
        }

        public Matrix4x4 ReadTransform(BinaryReaderEx br)
        {
            var transform = new Matrix4x4(
                br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle(),
                br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle(),
                br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle(),
                br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle())
            {
                M14 = 0,
                M24 = 0,
                M34 = 0,
                M44 = 1
            };

            return transform;
        }

        public List<Matrix4x4> ReadTransformArray(BinaryReaderEx br)
        {
            return ReadArrayBase(ReadTransform, br);
        }

        public Matrix4x4 ReadQSTransform(BinaryReaderEx br)
        {
            var qsTransform = new Matrix4x4(
                br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle(),
                br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle(),
                br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle(),
                0, 0, 0, 0)
            {
                M14 = 0,
                M34 = 0,
            };

            return qsTransform;
        }

        public List<Matrix4x4> ReadQSTransformArray(BinaryReaderEx br)
        {
            return ReadArrayBase(ReadQSTransform, br);
        }

        public Quaternion ReadQuaternion(BinaryReaderEx br)
        {
            return new Quaternion(br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
        }

        public List<Quaternion> ReadQuaternionArray(BinaryReaderEx br)
        {
            return ReadArrayBase(ReadQuaternion, br);
        }


        #region C Style Array
        private T[] ReadCStyleArrayBase<T>(Func<BinaryReaderEx, T> func, BinaryReaderEx br, short length)
        {
            var res = new T[length];
            for (int i = 0; i < length; i++)
            {
                res[i] = (func(br));
            }
            return res;
        }

        public bool[] ReadBooleanCStyleArray(BinaryReaderEx br, short length)
        {
            return ReadCStyleArrayBase(ReadBoolean, br, length);
        }

        public byte[] ReadByteCStyleArray(BinaryReaderEx br, short length)
        {
            return ReadCStyleArrayBase(ReadByte, br, length);
        }

        public sbyte[] ReadSByteCStyleArray(BinaryReaderEx br, short length)
        {
            return ReadCStyleArrayBase(ReadSByte, br, length);
        }

        public short[] ReadInt16CStyleArray(BinaryReaderEx br, short length)
        {
            return ReadCStyleArrayBase(ReadInt16, br, length);
        }

        public ushort[] ReadUInt16CStyleArray(BinaryReaderEx br, short length)
        {
            return ReadCStyleArrayBase(ReadUInt16, br, length);
        }
        public int[] ReadInt32CStyleArray(BinaryReaderEx br, short length)
        {
            return ReadCStyleArrayBase(ReadInt32, br, length);
        }
        public uint[] ReadUInt32CStyleArray(BinaryReaderEx br, short length)
        {
            return ReadCStyleArrayBase(ReadUInt32, br, length);
        }

        public long[] ReadInt64CStyleArray(BinaryReaderEx br, short length)
        {
            return ReadCStyleArrayBase(ReadInt64, br, length);
        }

        public ulong[] ReadUInt64CStyleArray(BinaryReaderEx br, short length)
        {
            return ReadCStyleArrayBase(ReadUInt64, br, length);
        }

        public Half[] ReadHalfCStyleArray(BinaryReaderEx br, short length)
        {
            return ReadCStyleArrayBase(ReadHalf, br, length);
        }

        public float[] ReadSingleCStyleArray(BinaryReaderEx br, short length)
        {
            return ReadCStyleArrayBase(ReadSingle, br, length);
        }

        public Vector4[] ReadVector4CStyleArray(BinaryReaderEx br, short length)
        {
            return ReadCStyleArrayBase(ReadVector4, br, length);
        }

        public Quaternion[] ReadQuaternionCStyleArray(BinaryReaderEx br, short length)
        {
            return ReadCStyleArrayBase(ReadQuaternion, br, length);
        }
        public Matrix4x4[] ReadMatrix3CStyleArray(BinaryReaderEx br, short length)
        {
            return ReadCStyleArrayBase(ReadMatrix3, br, length);
        }

        public Matrix4x4[] ReadRotationCStyleArray(BinaryReaderEx br, short length)
        {
            return ReadCStyleArrayBase(ReadMatrix3, br, length);
        }

        public Matrix4x4[] ReadQSTransformCStyleArray(BinaryReaderEx br, short length)
        {
            return ReadCStyleArrayBase(ReadQSTransform, br, length);
        }

        public Matrix4x4[] ReadMatrix4CStyleArray(BinaryReaderEx br, short length)
        {
            return ReadCStyleArrayBase(ReadMatrix4, br, length);
        }

        public Matrix4x4[] ReadTransformCStyleArray(BinaryReaderEx br, short length)
        {
            return ReadCStyleArrayBase(ReadTransform, br, length);
        }

        public T[] ReadClassPointerCStyleArray<T>(BinaryReaderEx br, short length) where T : IHavokObject, new()
        {
            return ReadCStyleArrayBase(ReadClassPointer<T>, br, length);
        }

        public void ReadEmptyPointerCStyleArray(BinaryReaderEx br, short length)
        {
            for (int i = 0; i < length; i++)
            {
                ReadEmptyPointer(br);
            }
        }

        public T[] ReadStructCStyleArray<T>(BinaryReaderEx br, short length) where T : IHavokObject, new()
        {
            var res = new T[length];
            for (int i = 0; i < length; i++)
            {
                var s = new T();
                s.Read(this, br);
                res[i] = s;
            }
            return res;
        }

        #endregion

        #endregion
    }
}