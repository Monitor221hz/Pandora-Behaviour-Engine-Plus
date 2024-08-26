using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkaSkeletonLocalFrameOnBone Signatire: 0x52e8043 size: 16 flags: FLAGS_NONE

    // localFrame class: hkLocalFrame Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // boneIndex class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 8 flags: FLAGS_NONE enum: 
    public partial class hkaSkeletonLocalFrameOnBone : IHavokObject, IEquatable<hkaSkeletonLocalFrameOnBone?>
    {
        public hkLocalFrame? localFrame { set; get; }
        public int boneIndex { set; get; }

        public virtual uint Signature { set; get; } = 0x52e8043;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            localFrame = des.ReadClassPointer<hkLocalFrame>(br);
            boneIndex = br.ReadInt32();
            br.Position += 4;
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            s.WriteClassPointer(bw, localFrame);
            bw.WriteInt32(boneIndex);
            bw.Position += 4;
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            localFrame = xd.ReadClassPointer<hkLocalFrame>(this, xe, nameof(localFrame));
            boneIndex = xd.ReadInt32(xe, nameof(boneIndex));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteClassPointer(xe, nameof(localFrame), localFrame);
            xs.WriteNumber(xe, nameof(boneIndex), boneIndex);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkaSkeletonLocalFrameOnBone);
        }

        public bool Equals(hkaSkeletonLocalFrameOnBone? other)
        {
            return other is not null &&
                   ((localFrame is null && other.localFrame is null) || (localFrame is not null && other.localFrame is not null && localFrame.Equals((IHavokObject)other.localFrame))) &&
                   boneIndex.Equals(other.boneIndex) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(localFrame);
            hashcode.Add(boneIndex);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

