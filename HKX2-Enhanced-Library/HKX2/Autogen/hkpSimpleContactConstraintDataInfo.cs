using System;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpSimpleContactConstraintDataInfo Signatire: 0xb59d1734 size: 32 flags: FLAGS_NONE

    // flags class:  Type.TYPE_UINT16 Type.TYPE_VOID arrSize: 0 offset: 0 flags: ALIGN_16|FLAGS_NONE enum: 
    // index class:  Type.TYPE_UINT16 Type.TYPE_VOID arrSize: 0 offset: 2 flags: FLAGS_NONE enum: 
    // internalData0 class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 4 flags: FLAGS_NONE enum: 
    // rollingFrictionMultiplier class:  Type.TYPE_HALF Type.TYPE_VOID arrSize: 0 offset: 8 flags: FLAGS_NONE enum: 
    // internalData1 class:  Type.TYPE_HALF Type.TYPE_VOID arrSize: 0 offset: 10 flags: FLAGS_NONE enum: 
    // data class:  Type.TYPE_UINT32 Type.TYPE_VOID arrSize: 5 offset: 12 flags: FLAGS_NONE enum: 
    public partial class hkpSimpleContactConstraintDataInfo : IHavokObject, IEquatable<hkpSimpleContactConstraintDataInfo?>
    {
        public ushort flags { set; get; }
        public ushort index { set; get; }
        public float internalData0 { set; get; }
        public Half rollingFrictionMultiplier { set; get; }
        public Half internalData1 { set; get; }
        public uint[] data = new uint[5];

        public virtual uint Signature { set; get; } = 0xb59d1734;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            flags = br.ReadUInt16();
            index = br.ReadUInt16();
            internalData0 = br.ReadSingle();
            rollingFrictionMultiplier = br.ReadHalf();
            internalData1 = br.ReadHalf();
            data = des.ReadUInt32CStyleArray(br, 5);
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            bw.WriteUInt16(flags);
            bw.WriteUInt16(index);
            bw.WriteSingle(internalData0);
            bw.WriteHalf(rollingFrictionMultiplier);
            bw.WriteHalf(internalData1);
            s.WriteUInt32CStyleArray(bw, data);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            flags = xd.ReadUInt16(xe, nameof(flags));
            index = xd.ReadUInt16(xe, nameof(index));
            internalData0 = xd.ReadSingle(xe, nameof(internalData0));
            rollingFrictionMultiplier = xd.ReadHalf(xe, nameof(rollingFrictionMultiplier));
            internalData1 = xd.ReadHalf(xe, nameof(internalData1));
            data = xd.ReadUInt32CStyleArray(xe, nameof(data), 5);
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteNumber(xe, nameof(flags), flags);
            xs.WriteNumber(xe, nameof(index), index);
            xs.WriteFloat(xe, nameof(internalData0), internalData0);
            xs.WriteFloat(xe, nameof(rollingFrictionMultiplier), rollingFrictionMultiplier);
            xs.WriteFloat(xe, nameof(internalData1), internalData1);
            xs.WriteNumberArray(xe, nameof(data), data);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpSimpleContactConstraintDataInfo);
        }

        public bool Equals(hkpSimpleContactConstraintDataInfo? other)
        {
            return other is not null &&
                   flags.Equals(other.flags) &&
                   index.Equals(other.index) &&
                   internalData0.Equals(other.internalData0) &&
                   rollingFrictionMultiplier.Equals(other.rollingFrictionMultiplier) &&
                   internalData1.Equals(other.internalData1) &&
                   data.SequenceEqual(other.data) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(flags);
            hashcode.Add(index);
            hashcode.Add(internalData0);
            hashcode.Add(rollingFrictionMultiplier);
            hashcode.Add(internalData1);
            hashcode.Add(data.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

