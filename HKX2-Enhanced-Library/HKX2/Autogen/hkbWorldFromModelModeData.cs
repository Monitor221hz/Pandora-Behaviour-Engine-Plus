using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbWorldFromModelModeData Signatire: 0xa3af8783 size: 8 flags: FLAGS_NONE

    // poseMatchingBone0 class:  Type.TYPE_INT16 Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // poseMatchingBone1 class:  Type.TYPE_INT16 Type.TYPE_VOID arrSize: 0 offset: 2 flags: FLAGS_NONE enum: 
    // poseMatchingBone2 class:  Type.TYPE_INT16 Type.TYPE_VOID arrSize: 0 offset: 4 flags: FLAGS_NONE enum: 
    // mode class:  Type.TYPE_ENUM Type.TYPE_INT8 arrSize: 0 offset: 6 flags: FLAGS_NONE enum: WorldFromModelMode
    public partial class hkbWorldFromModelModeData : IHavokObject, IEquatable<hkbWorldFromModelModeData?>
    {
        public short poseMatchingBone0 { set; get; }
        public short poseMatchingBone1 { set; get; }
        public short poseMatchingBone2 { set; get; }
        public sbyte mode { set; get; }

        public virtual uint Signature { set; get; } = 0xa3af8783;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            poseMatchingBone0 = br.ReadInt16();
            poseMatchingBone1 = br.ReadInt16();
            poseMatchingBone2 = br.ReadInt16();
            mode = br.ReadSByte();
            br.Position += 1;
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            bw.WriteInt16(poseMatchingBone0);
            bw.WriteInt16(poseMatchingBone1);
            bw.WriteInt16(poseMatchingBone2);
            bw.WriteSByte(mode);
            bw.Position += 1;
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            poseMatchingBone0 = xd.ReadInt16(xe, nameof(poseMatchingBone0));
            poseMatchingBone1 = xd.ReadInt16(xe, nameof(poseMatchingBone1));
            poseMatchingBone2 = xd.ReadInt16(xe, nameof(poseMatchingBone2));
            mode = xd.ReadFlag<WorldFromModelMode, sbyte>(xe, nameof(mode));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteNumber(xe, nameof(poseMatchingBone0), poseMatchingBone0);
            xs.WriteNumber(xe, nameof(poseMatchingBone1), poseMatchingBone1);
            xs.WriteNumber(xe, nameof(poseMatchingBone2), poseMatchingBone2);
            xs.WriteEnum<WorldFromModelMode, sbyte>(xe, nameof(mode), mode);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbWorldFromModelModeData);
        }

        public bool Equals(hkbWorldFromModelModeData? other)
        {
            return other is not null &&
                   poseMatchingBone0.Equals(other.poseMatchingBone0) &&
                   poseMatchingBone1.Equals(other.poseMatchingBone1) &&
                   poseMatchingBone2.Equals(other.poseMatchingBone2) &&
                   mode.Equals(other.mode) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(poseMatchingBone0);
            hashcode.Add(poseMatchingBone1);
            hashcode.Add(poseMatchingBone2);
            hashcode.Add(mode);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

