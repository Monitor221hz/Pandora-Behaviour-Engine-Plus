using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpMoppCode Signatire: 0x924c2661 size: 64 flags: FLAGS_NONE

    // info class: hkpMoppCodeCodeInfo Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // data class:  Type.TYPE_ARRAY Type.TYPE_UINT8 arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // buildType class:  Type.TYPE_ENUM Type.TYPE_INT8 arrSize: 0 offset: 48 flags: FLAGS_NONE enum: BuildType
    public partial class hkpMoppCode : hkReferencedObject, IEquatable<hkpMoppCode?>
    {
        public hkpMoppCodeCodeInfo info { set; get; } = new();
        public IList<byte> data { set; get; } = Array.Empty<byte>();
        public sbyte buildType { set; get; }

        public override uint Signature { set; get; } = 0x924c2661;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            info.Read(des, br);
            data = des.ReadByteArray(br);
            buildType = br.ReadSByte();
            br.Position += 15;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            info.Write(s, bw);
            s.WriteByteArray(bw, data);
            bw.WriteSByte(buildType);
            bw.Position += 15;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            info = xd.ReadClass<hkpMoppCodeCodeInfo>(xe, nameof(info));
            data = xd.ReadByteArray(xe, nameof(data));
            buildType = xd.ReadFlag<BuildType, sbyte>(xe, nameof(buildType));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClass<hkpMoppCodeCodeInfo>(xe, nameof(info), info);
            xs.WriteNumberArray(xe, nameof(data), data);
            xs.WriteEnum<BuildType, sbyte>(xe, nameof(buildType), buildType);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpMoppCode);
        }

        public bool Equals(hkpMoppCode? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   ((info is null && other.info is null) || (info is not null && other.info is not null && info.Equals((IHavokObject)other.info))) &&
                   data.SequenceEqual(other.data) &&
                   buildType.Equals(other.buildType) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(info);
            hashcode.Add(data.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(buildType);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

