using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;

namespace HKX2E
{
    public class PackFileSerializer
    {
        private int _currentLocalWriteQueue;
        private int _currentSerializationQueue;
        private List<GlobalFixup> _globalFixups = new();

        private Dictionary<IHavokObject, uint> _globalLookup = new(ReferenceEqualityComparer.Instance);
        public HKXHeader _header;

        private List<LocalFixup> _localFixups = new();
        private List<Queue<Action>> _localWriteQueues = new();
        private Dictionary<IHavokObject, List<uint>> _pendingGlobals = new(ReferenceEqualityComparer.Instance);

        private HashSet<IHavokObject> _pendingVirtuals = new(ReferenceEqualityComparer.Instance);
        private List<Queue<IHavokObject>> _serializationQueues = new();

        private HashSet<IHavokObject> _serializedObjects = new(ReferenceEqualityComparer.Instance);
        private List<VirtualFixup> _virtualFixups = new();
        private Dictionary<string, uint> _virtualLookup = new();


        private void PushLocalWriteQueue()
        {
            _currentLocalWriteQueue++;
            if (_currentLocalWriteQueue == _localWriteQueues.Count) _localWriteQueues.Add(new Queue<Action>());
        }

        private void PopLocalWriteQueue()
        {
            _currentLocalWriteQueue--;
        }

        private void PushSerializationQueue()
        {
            _currentSerializationQueue++;
            if (_currentSerializationQueue == _serializationQueues.Count)
                _serializationQueues.Add(new Queue<IHavokObject>());
        }

        private void PopSerializationQueue()
        {
            _currentSerializationQueue--;
        }


        public void Serialize(IHavokObject rootObject, BinaryWriterEx bw, HKXHeader header)
        {
            _header = header;
            bw.BigEndian = _header.Endian == 0;

            _header.Write(bw);
            // Initialize bookkeeping structures
            _localFixups = new List<LocalFixup>();
            _globalFixups = new List<GlobalFixup>();
            _virtualFixups = new List<VirtualFixup>();

            _globalLookup = new Dictionary<IHavokObject, uint>(ReferenceEqualityComparer.Instance);
            _virtualLookup = new Dictionary<string, uint>();

            _localWriteQueues = new List<Queue<Action>>();
            _serializationQueues = new List<Queue<IHavokObject>>();
            _pendingGlobals = new Dictionary<IHavokObject, List<uint>>(ReferenceEqualityComparer.Instance);
            _pendingVirtuals = new HashSet<IHavokObject>(ReferenceEqualityComparer.Instance);

            _serializedObjects = new HashSet<IHavokObject>(ReferenceEqualityComparer.Instance);

            // Memory stream for writing all the class definitions
            var classms = new MemoryStream();
            var classbw = new BinaryWriterEx(
                _header.Endian == 0, _header.PointerSize == 8, classms);

            // Data memory stream for havok objects
            // debugging
            //var datams = new FileStream(".\\dump.hex", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite | FileShare.Delete, 1, FileOptions.WriteThrough);
            var datams = new MemoryStream();
            var databw = new BinaryWriterEx(
                _header.Endian == 0, _header.PointerSize == 8, datams);

            // Populate class names with some stuff havok always has
            var hkClass = new HKXClassName { ClassName = "hkClass", Signature = 0x75585EF6 };
            var hkClassMember = new HKXClassName { ClassName = "hkClassMember", Signature = 0x5C7EA4C2 };
            var hkClassEnum = new HKXClassName { ClassName = "hkClassEnum", Signature = 0x8A3609CF };
            var hkClassEnumItem = new HKXClassName { ClassName = "hkClassEnumItem", Signature = 0xCE6F8A6C };

            hkClass.Write(classbw);
            hkClassMember.Write(classbw);
            hkClassEnum.Write(classbw);
            hkClassEnumItem.Write(classbw);

            _serializationQueues.Add(new Queue<IHavokObject>());
            _serializationQueues[0].Enqueue(rootObject);
            _localWriteQueues.Add(new Queue<Action>());
            _pendingVirtuals.Add(rootObject);

            while (_serializationQueues.Count > 1 || _serializationQueues[0].Count > 0)
            {
                var sq = _serializationQueues.Last();

                while (sq != null && sq.Count == 0 && _serializationQueues.Count > 1)
                {
                    _serializationQueues.RemoveAt(_serializationQueues.Count - 1);
                    sq = _serializationQueues.Last();
                }

                if (sq == null || sq.Count == 0) continue;

                var obj = sq.Dequeue();
                _currentSerializationQueue = _serializationQueues.Count - 1;

                if (_serializedObjects.Contains(obj)) continue;

                // See if we need to add virtual bookkeeping
                if (_pendingVirtuals.Contains(obj))
                {
                    _pendingVirtuals.Remove(obj);
                    var classname = obj.GetType().Name;
                    if (!_virtualLookup.ContainsKey(classname))
                    {
                        // Need to create a new class name entry and record the position
                        var cname = new HKXClassName
                        {
                            ClassName = classname,
                            Signature = obj.Signature
                        };
                        var offset = (uint)classbw.Position;
                        cname.Write(classbw);
                        _virtualLookup.Add(classname, offset + 5);
                    }

                    // Create a new Virtual fixup for this object
                    var vfu = new VirtualFixup
                    {
                        Src = (uint)databw.Position,
                        DstSectionIndex = 0,
                        Dst = _virtualLookup[classname]
                    };
                    _virtualFixups.Add(vfu);

                    // See if we have any pending global references to this object
                    if (_pendingGlobals.ContainsKey(obj))
                    {
                        // If so, create all the needed global fixups
                        foreach (var src in _pendingGlobals[obj])
                        {
                            var gfu = new GlobalFixup
                            {
                                Src = src,
                                DstSectionIndex = 2,
                                Dst = (uint)databw.Position
                            };
                            _globalFixups.Add(gfu);
                        }

                        _pendingGlobals.Remove(obj);
                    }

                    // Add global lookup
                    _globalLookup.Add(obj, (uint)databw.Position);
                }

                obj.Write(this, databw);
                _serializedObjects.Add(obj);
                databw.Pad(16);

                // Write local data (such as array contents and strings)
                while (_localWriteQueues.Count > 1 || _localWriteQueues[0].Count > 0)
                {
                    var q = _localWriteQueues.Last();
                    while (q != null && q.Count == 0 && _localWriteQueues.Count > 1)
                    {
                        if (_localWriteQueues.Count > 1) _localWriteQueues.RemoveAt(_localWriteQueues.Count - 1);

                        q = _localWriteQueues.Last();

                        // Do alignment at the popping of a queue frame
                        databw.Pad(16);
                    }

                    if (q == null || q.Count == 0)
                    {
                        _currentLocalWriteQueue = _localWriteQueues.Count - 1;
                        continue;
                    }

                    var act = q.Dequeue();
                    _currentLocalWriteQueue = _localWriteQueues.Count - 1;
                    act.Invoke();
                }

                databw.Pad(16);
            }

            var classNames = new HKXSection()
            {
                SectionID = 0,
                SectionTag = "__classnames__",
                SectionData = classms.ToArray(),
                ContentsVersionString = _header.ContentsVersionString
            };
            var types = new HKXSection { SectionID = 1, SectionTag = "__types__", SectionData = Array.Empty<byte>(), ContentsVersionString = _header.ContentsVersionString };
            // debugging
            //var ms = new MemoryStream();
            //var tempPosition = datams.Position;
            //datams.Position = 0;
            //datams.CopyTo(ms);
            //datams.Position = tempPosition;

            var data = new HKXSection
            {
                SectionID = 2,
                SectionTag = "__data__",
                SectionData = datams.ToArray(),
                LocalFixups = _localFixups.OrderBy(x => x.Dst).ToList(),
                GlobalFixups = _globalFixups.OrderBy(x => x.Src).ToList(),
                VirtualFixups = _virtualFixups,
                ContentsVersionString = _header.ContentsVersionString
            };

            classNames.WriteHeader(bw);
            types.WriteHeader(bw);
            data.WriteHeader(bw);

            classNames.WriteData(bw);
            types.WriteData(bw);
            data.WriteData(bw);
        }

        #region Write methods

        private void PadToPointerSizeIfPaddingOption(BinaryWriterEx bw)
        {
            if (_header.PaddingOption == 1) bw.Pad(_header.PointerSize);
        }

        public void WriteVoidPointer(BinaryWriterEx bw)
        {
            PadToPointerSizeIfPaddingOption(bw);
            bw.WriteUSize(0);
        }

        public void WriteVoidArray(BinaryWriterEx bw)
        {
            WriteVoidPointer(bw);
            bw.WriteUInt32(0);
            bw.WriteUInt32(0 | ((uint)0x80 << 24));
        }

        private void WriteArrayBase<T>(BinaryWriterEx bw, IList<T> l, Action<T> perElement, bool pad = false)
        {
            PadToPointerSizeIfPaddingOption(bw);

            var src = (uint)bw.Position;
            var size = (uint)l.Count;

            bw.WriteUSize(0);
            bw.WriteUInt32(size);
            bw.WriteUInt32(size | ((uint)0x80 << 24));

            if (size <= 0) return;

            var lfu = new LocalFixup { Src = src };
            _localFixups.Add(lfu);
            _localWriteQueues[_currentLocalWriteQueue].Enqueue(() =>
            {
                bw.Pad(16);
                lfu.Dst = (uint)bw.Position;
                // This ensures any writes the array elements may have are top priority
                PushLocalWriteQueue();
                foreach (var item in l) perElement.Invoke(item);

                PopLocalWriteQueue();
            });
            if (pad) _localWriteQueues[_currentLocalWriteQueue].Enqueue(() => { bw.Pad(16); });
        }

        public void WriteClassArray<T>(BinaryWriterEx bw, IList<T> d) where T : IHavokObject
        {
            WriteArrayBase(bw, d, e => { e.Write(this, bw); }, true);
        }

        public void WriteClassPointer<T>(BinaryWriterEx bw, T? d) where T : IHavokObject
        {
            PadToPointerSizeIfPaddingOption(bw);
            var pos = (uint)bw.Position;
            bw.WriteUSize(0);

            if (d == null) return;

            // If we're referencing an already serialized object, add a global ref
            if (_globalLookup.ContainsKey(d))
            {
                var gfu = new GlobalFixup { Src = pos, DstSectionIndex = 2, Dst = _globalLookup[d] };
                _globalFixups.Add(gfu);
                return;
            }
            // Otherwise need to add a pending reference and mark the object for serialization

            if (!_pendingGlobals.ContainsKey(d))
            {
                _pendingGlobals.Add(d, new List<uint>());
                PushSerializationQueue();
                _serializationQueues[_currentSerializationQueue].Enqueue(d);
                PopSerializationQueue();
                _pendingVirtuals.Add(d);
            }

            _pendingGlobals[d].Add(pos);
        }

        public void WriteClassPointerArray<T>(BinaryWriterEx bw, IList<T> d) where T : IHavokObject
        {
            WriteArrayBase(bw, d, e => WriteClassPointer(bw, e));
        }

        public void WriteStringPointer(BinaryWriterEx bw, string d, int padding = 16)
        {
            PadToPointerSizeIfPaddingOption(bw);
            var src = (uint)bw.Position;
            bw.WriteUSize(0);

            if (d == null) return;

            var lfu = new LocalFixup { Src = src };
            _localFixups.Add(lfu);
            _localWriteQueues[_currentLocalWriteQueue].Enqueue(() =>
            {
                lfu.Dst = (uint)bw.Position;
                bw.WriteASCII(d, true);
                bw.Pad(padding);
            });
        }

        public void WriteCString(BinaryWriterEx bw, string d, int padding = 16)
        {
            // difference with StirngPointer
            // when empty string it didn't create localFixup
            PadToPointerSizeIfPaddingOption(bw);
            var src = (uint)bw.Position;
            bw.WriteUSize(0);

            if (d == null || d == "" || d == "\u2400") return;

            var lfu = new LocalFixup { Src = src };
            _localFixups.Add(lfu);
            _localWriteQueues[_currentLocalWriteQueue].Enqueue(() =>
            {
                lfu.Dst = (uint)bw.Position;
                bw.WriteASCII(d, true);
                bw.Pad(padding);
            });
        }

        public void WriteStringPointerArray(BinaryWriterEx bw, IList<string> d)
        {
            WriteArrayBase(bw, d, e => { WriteStringPointer(bw, e, 2); });
        }

        public void WriteByte(BinaryWriterEx bw, byte d)
        {
            bw.WriteByte(d);
        }

        public void WriteByteArray(BinaryWriterEx bw, IList<byte> d)
        {
            WriteArrayBase(bw, d, e => WriteByte(bw, e));
        }

        public void WriteSByte(BinaryWriterEx bw, sbyte d)
        {
            bw.WriteSByte(d);
        }

        public void WriteSByteArray(BinaryWriterEx bw, IList<sbyte> d)
        {
            WriteArrayBase(bw, d, e => WriteSByte(bw, e));
        }

        public void WriteUInt16(BinaryWriterEx bw, ushort d)
        {
            bw.WriteUInt16(d);
        }

        public void WriteUInt16Array(BinaryWriterEx bw, IList<ushort> d)
        {
            WriteArrayBase(bw, d, e => WriteUInt16(bw, e));
        }

        public void WriteInt16(BinaryWriterEx bw, short d)
        {
            bw.WriteInt16(d);
        }

        public void WriteInt16Array(BinaryWriterEx bw, IList<short> d)
        {
            WriteArrayBase(bw, d, e => WriteInt16(bw, e));
        }

        public void WriteUInt32(BinaryWriterEx bw, uint d)
        {
            bw.WriteUInt32(d);
        }

        public void WriteUInt32Array(BinaryWriterEx bw, IList<uint> d)
        {
            WriteArrayBase(bw, d, e => WriteUInt32(bw, e));
        }

        public void WriteInt32(BinaryWriterEx bw, int d)
        {
            bw.WriteInt32(d);
        }

        public void WriteInt32Array(BinaryWriterEx bw, IList<int> d)
        {
            WriteArrayBase(bw, d, e => WriteInt32(bw, e));
        }

        public void WriteUInt64(BinaryWriterEx bw, ulong d)
        {
            bw.WriteUInt64(d);
        }

        public void WriteUInt64Array(BinaryWriterEx bw, IList<ulong> d)
        {
            WriteArrayBase(bw, d, e => WriteUInt64(bw, e));
        }

        public void WriteInt64(BinaryWriterEx bw, long d)
        {
            bw.WriteInt64(d);
        }

        public void WriteInt64Array(BinaryWriterEx bw, IList<long> d)
        {
            WriteArrayBase(bw, d, e => WriteInt64(bw, e));
        }

        public void WriteHalf(BinaryWriterEx bw, Half d)
        {
            bw.WriteHalf(d);
        }

        public void WriteHalfArray(BinaryWriterEx bw, IList<Half> d)
        {
            WriteArrayBase(bw, d, e => WriteHalf(bw, e));
        }

        public void WriteSingle(BinaryWriterEx bw, float d)
        {
            bw.WriteSingle(d);
        }

        public void WriteSingleArray(BinaryWriterEx bw, IList<float> d)
        {
            WriteArrayBase(bw, d, e => WriteSingle(bw, e));
        }

        public void WriteBoolean(BinaryWriterEx bw, bool d)
        {
            bw.WriteBoolean(d);
        }

        public void WriteBooleanArray(BinaryWriterEx bw, IList<bool> d)
        {
            WriteArrayBase(bw, d, e => WriteBoolean(bw, e));
        }

        public void WriteVector4(BinaryWriterEx bw, Vector4 d)
        {
            bw.WriteVector4(d);
        }

        public void WriteVector4Array(BinaryWriterEx bw, IList<Vector4> d)
        {
            WriteArrayBase(bw, d, e => WriteVector4(bw, e));
        }

        public void WriteMatrix3(BinaryWriterEx bw, Matrix4x4 d)
        {
            bw.WriteSingle(d.M11);
            bw.WriteSingle(d.M12);
            bw.WriteSingle(d.M13);
            bw.WriteSingle(d.M14);
            bw.WriteSingle(d.M21);
            bw.WriteSingle(d.M22);
            bw.WriteSingle(d.M23);
            bw.WriteSingle(d.M24);
            bw.WriteSingle(d.M31);
            bw.WriteSingle(d.M32);
            bw.WriteSingle(d.M33);
            bw.WriteSingle(d.M34);
        }

        public void WriteMatrix3Array(BinaryWriterEx bw, IList<Matrix4x4> d)
        {
            WriteArrayBase(bw, d, e => WriteMatrix3(bw, e));
        }

        public void WriteMatrix4(BinaryWriterEx bw, Matrix4x4 d)
        {
            bw.WriteSingle(d.M11);
            bw.WriteSingle(d.M12);
            bw.WriteSingle(d.M13);
            bw.WriteSingle(d.M14);
            bw.WriteSingle(d.M21);
            bw.WriteSingle(d.M22);
            bw.WriteSingle(d.M23);
            bw.WriteSingle(d.M24);
            bw.WriteSingle(d.M31);
            bw.WriteSingle(d.M32);
            bw.WriteSingle(d.M33);
            bw.WriteSingle(d.M34);
            bw.WriteSingle(d.M41);
            bw.WriteSingle(d.M42);
            bw.WriteSingle(d.M43);
            bw.WriteSingle(d.M44);
        }

        public void WriteMatrix4Array(BinaryWriterEx bw, IList<Matrix4x4> d)
        {
            WriteArrayBase(bw, d, e => WriteMatrix4(bw, e));
        }

        public void WriteTransform(BinaryWriterEx bw, Matrix4x4 d)
        {
            bw.WriteSingle(d.M11);
            bw.WriteSingle(d.M12);
            bw.WriteSingle(d.M13);
            bw.WriteSingle(0.0f); //bw.WriteSingle(d.M14);
            bw.WriteSingle(d.M21);
            bw.WriteSingle(d.M22);
            bw.WriteSingle(d.M23);
            bw.WriteSingle(0.0f); //bw.WriteSingle(d.M24);
            bw.WriteSingle(d.M31);
            bw.WriteSingle(d.M32);
            bw.WriteSingle(d.M33);
            bw.WriteSingle(0.0f); //bw.WriteSingle(d.M34);
            bw.WriteSingle(d.M41);
            bw.WriteSingle(d.M42);
            bw.WriteSingle(d.M43);
            bw.WriteSingle(1.0f); //bw.WriteSingle(d.M44);
        }

        public void WriteTransformArray(BinaryWriterEx bw, IList<Matrix4x4> d)
        {
            WriteArrayBase(bw, d, e => WriteTransform(bw, e));
        }

        public void WriteQSTransform(BinaryWriterEx bw, Matrix4x4 d)
        {
            WriteMatrix3(bw, d);
        }

        public void WriteQSTransformArray(BinaryWriterEx bw, IList<Matrix4x4> d)
        {
            WriteMatrix3Array(bw, d);
        }

        public void WriteQuaternion(BinaryWriterEx bw, Quaternion d)
        {
            bw.WriteSingle(d.X);
            bw.WriteSingle(d.Y);
            bw.WriteSingle(d.Z);
            bw.WriteSingle(d.W);
        }

        public void WriteQuaternionArray(BinaryWriterEx bw, IList<Quaternion> d)
        {
            WriteArrayBase(bw, d, e => WriteQuaternion(bw, e));
        }

        #region C Style Array

        private void WriteCStyleArrayBase<T>(BinaryWriterEx bw, IList<T> content, Action<T> perElement)
        {
            foreach (var item in content)
            {
                perElement.Invoke(item);
            }
        }

        public void WriteBooleanCStyleArray(BinaryWriterEx bw, bool[] d)
        {
            WriteCStyleArrayBase(bw, d, e => WriteBoolean(bw, e));
        }

        public void WriteByteCStyleArray(BinaryWriterEx bw, byte[] d)
        {
            WriteCStyleArrayBase(bw, d, e => WriteByte(bw, e));
        }

        public void WriteSByteCStyleArray(BinaryWriterEx bw, sbyte[] d)
        {
            WriteCStyleArrayBase(bw, d, e => WriteSByte(bw, e));
        }

        public void WriteInt16CStyleArray(BinaryWriterEx bw, short[] d)
        {
            WriteCStyleArrayBase(bw, d, e => WriteInt16(bw, e));
        }

        public void WriteUInt16CStyleArray(BinaryWriterEx bw, ushort[] d)
        {
            WriteCStyleArrayBase(bw, d, e => WriteUInt16(bw, e));
        }

        public void WriteInt32CStyleArray(BinaryWriterEx bw, int[] d)
        {
            WriteCStyleArrayBase(bw, d, e => WriteInt32(bw, e));
        }

        public void WriteUInt32CStyleArray(BinaryWriterEx bw, uint[] d)
        {
            WriteCStyleArrayBase(bw, d, e => WriteUInt32(bw, e));
        }

        public void WriteInt64CStyleArray(BinaryWriterEx bw, long[] d)
        {
            WriteCStyleArrayBase(bw, d, e => WriteInt64(bw, e));
        }

        public void WriteUInt64CStyleArray(BinaryWriterEx bw, ulong[] d)
        {
            WriteCStyleArrayBase(bw, d, e => WriteUInt64(bw, e));
        }

        public void WriteHalfCStyleArray(BinaryWriterEx bw, Half[] d)
        {
            WriteCStyleArrayBase(bw, d, e => WriteHalf(bw, e));
        }

        public void WriteSingleCStyleArray(BinaryWriterEx bw, float[] d)
        {
            WriteCStyleArrayBase(bw, d, e => WriteSingle(bw, e));
        }

        public void WriteVector4CStyleArray(BinaryWriterEx bw, Vector4[] d)
        {
            WriteCStyleArrayBase(bw, d, e => WriteVector4(bw, e));
        }

        public void WriteQuaternionCStyleArray(BinaryWriterEx bw, Quaternion[] d)
        {
            WriteCStyleArrayBase(bw, d, e => WriteQuaternion(bw, e));
        }

        public void WriteMatrix3CStyleArray(BinaryWriterEx bw, Matrix4x4[] d)
        {
            WriteCStyleArrayBase(bw, d, e => WriteMatrix3(bw, e));
        }

        public void WriteRotationCStyleArray(BinaryWriterEx bw, Matrix4x4[] d)
        {
            WriteCStyleArrayBase(bw, d, e => WriteMatrix3(bw, e));
        }

        public void WriteQSTransformCStyleArray(BinaryWriterEx bw, Matrix4x4[] d)
        {
            WriteCStyleArrayBase(bw, d, e => WriteQSTransform(bw, e));
        }

        public void WriteMatrix4CStyleArray(BinaryWriterEx bw, Matrix4x4[] d)
        {
            WriteCStyleArrayBase(bw, d, e => WriteMatrix4(bw, e));
        }

        public void WriteTransformCStyleArray(BinaryWriterEx bw, Matrix4x4[] d)
        {
            WriteCStyleArrayBase(bw, d, e => WriteTransform(bw, e));
        }

        public void WriteClassPointerCStyleArray<T>(BinaryWriterEx bw, T?[] d) where T : IHavokObject, new()
        {
            WriteCStyleArrayBase(bw, d, e => WriteClassPointer(bw, e));
        }

        public void WriteEmptyPointerCStyleArray(BinaryWriterEx bw, short length)
        {
            for (int i = 0; i < length; i++)
            {
                WriteVoidPointer(bw);
            }
        }

        public void WriteStructCStyleArray<T>(BinaryWriterEx bw, T[] d) where T : IHavokObject
        {
            foreach (var item in d)
            {
                item.Write(this, bw);
            }
        }

        #endregion

        #endregion
    }
}