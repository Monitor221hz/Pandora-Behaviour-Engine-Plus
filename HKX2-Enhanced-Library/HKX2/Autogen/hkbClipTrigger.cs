using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbClipTrigger Signatire: 0x7eb45cea size: 32 flags: FLAGS_NONE

    // localTime class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // @eventclass: hkbEventProperty Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 8 flags: FLAGS_NONE enum: 
    // relativeToEndOfClip class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 24 flags: FLAGS_NONE enum: 
    // acyclic class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 25 flags: FLAGS_NONE enum: 
    // isAnnotation class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 26 flags: FLAGS_NONE enum: 
    public partial class hkbClipTrigger : IHavokObject, IEquatable<hkbClipTrigger?>
    {
        public float localTime { set; get; }
        public hkbEventProperty @event{ set; get; } = new();
        public bool relativeToEndOfClip { set; get; }
        public bool acyclic { set; get; }
        public bool isAnnotation { set; get; }

        public virtual uint Signature { set; get; } = 0x7eb45cea;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            localTime = br.ReadSingle();
            br.Position += 4;
            @event.Read(des, br);
            relativeToEndOfClip = br.ReadBoolean();
            acyclic = br.ReadBoolean();
            isAnnotation = br.ReadBoolean();
            br.Position += 5;
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            bw.WriteSingle(localTime);
            bw.Position += 4;
            @event.Write(s, bw);
            bw.WriteBoolean(relativeToEndOfClip);
            bw.WriteBoolean(acyclic);
            bw.WriteBoolean(isAnnotation);
            bw.Position += 5;
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            localTime = xd.ReadSingle(xe, nameof(localTime));
            @event= xd.ReadClass<hkbEventProperty>(xe, nameof(@event));
            relativeToEndOfClip = xd.ReadBoolean(xe, nameof(relativeToEndOfClip));
            acyclic = xd.ReadBoolean(xe, nameof(acyclic));
            isAnnotation = xd.ReadBoolean(xe, nameof(isAnnotation));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteFloat(xe, nameof(localTime), localTime);
            xs.WriteClass<hkbEventProperty>(xe, nameof(@event), @event);
            xs.WriteBoolean(xe, nameof(relativeToEndOfClip), relativeToEndOfClip);
            xs.WriteBoolean(xe, nameof(acyclic), acyclic);
            xs.WriteBoolean(xe, nameof(isAnnotation), isAnnotation);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbClipTrigger);
        }

        public bool Equals(hkbClipTrigger? other)
        {
            return other is not null &&
                   localTime.Equals(other.localTime) &&
                   ((@event is null && other.@event is null) || (@event is not null && other.@event is not null && @event.Equals((IHavokObject)other.@event))) &&
                   relativeToEndOfClip.Equals(other.relativeToEndOfClip) &&
                   acyclic.Equals(other.acyclic) &&
                   isAnnotation.Equals(other.isAnnotation) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(localTime);
            hashcode.Add(@event);
            hashcode.Add(relativeToEndOfClip);
            hashcode.Add(acyclic);
            hashcode.Add(isAnnotation);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

