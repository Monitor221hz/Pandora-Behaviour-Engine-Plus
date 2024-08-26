using System;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbFootIkControlsModifierLeg Signatire: 0x9e17091a size: 48 flags: FLAGS_NONE

    // groundPosition class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // ungroundedEvent class: hkbEventProperty Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // verticalError class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // hitSomething class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 36 flags: FLAGS_NONE enum: 
    // isPlantedMS class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 37 flags: FLAGS_NONE enum: 
    public partial class hkbFootIkControlsModifierLeg : IHavokObject, IEquatable<hkbFootIkControlsModifierLeg?>
    {
        public Vector4 groundPosition { set; get; }
        public hkbEventProperty ungroundedEvent { set; get; } = new();
        public float verticalError { set; get; }
        public bool hitSomething { set; get; }
        public bool isPlantedMS { set; get; }

        public virtual uint Signature { set; get; } = 0x9e17091a;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            groundPosition = br.ReadVector4();
            ungroundedEvent.Read(des, br);
            verticalError = br.ReadSingle();
            hitSomething = br.ReadBoolean();
            isPlantedMS = br.ReadBoolean();
            br.Position += 10;
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            bw.WriteVector4(groundPosition);
            ungroundedEvent.Write(s, bw);
            bw.WriteSingle(verticalError);
            bw.WriteBoolean(hitSomething);
            bw.WriteBoolean(isPlantedMS);
            bw.Position += 10;
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            groundPosition = xd.ReadVector4(xe, nameof(groundPosition));
            ungroundedEvent = xd.ReadClass<hkbEventProperty>(xe, nameof(ungroundedEvent));
            verticalError = xd.ReadSingle(xe, nameof(verticalError));
            hitSomething = xd.ReadBoolean(xe, nameof(hitSomething));
            isPlantedMS = xd.ReadBoolean(xe, nameof(isPlantedMS));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteVector4(xe, nameof(groundPosition), groundPosition);
            xs.WriteClass<hkbEventProperty>(xe, nameof(ungroundedEvent), ungroundedEvent);
            xs.WriteFloat(xe, nameof(verticalError), verticalError);
            xs.WriteBoolean(xe, nameof(hitSomething), hitSomething);
            xs.WriteBoolean(xe, nameof(isPlantedMS), isPlantedMS);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbFootIkControlsModifierLeg);
        }

        public bool Equals(hkbFootIkControlsModifierLeg? other)
        {
            return other is not null &&
                   groundPosition.Equals(other.groundPosition) &&
                   ((ungroundedEvent is null && other.ungroundedEvent is null) || (ungroundedEvent is not null && other.ungroundedEvent is not null && ungroundedEvent.Equals((IHavokObject)other.ungroundedEvent))) &&
                   verticalError.Equals(other.verticalError) &&
                   hitSomething.Equals(other.hitSomething) &&
                   isPlantedMS.Equals(other.isPlantedMS) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(groundPosition);
            hashcode.Add(ungroundedEvent);
            hashcode.Add(verticalError);
            hashcode.Add(hitSomething);
            hashcode.Add(isPlantedMS);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

