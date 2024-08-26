using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbTestStateChooser Signatire: 0xc0fcc436 size: 32 flags: FLAGS_NONE

    // @int class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // real class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 20 flags: FLAGS_NONE enum: 
    // @string class:  Type.TYPE_STRINGPTR Type.TYPE_VOID arrSize: 0 offset: 24 flags: FLAGS_NONE enum: 
    public partial class hkbTestStateChooser : hkbStateChooser, IEquatable<hkbTestStateChooser?>
    {
        public int @int { set; get; }
        public float real { set; get; }
        public string @string { set; get; } = "";

        public override uint Signature { set; get; } = 0xc0fcc436;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            @int = br.ReadInt32();
            real = br.ReadSingle();
            @string = des.ReadStringPointer(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteInt32(@int);
            bw.WriteSingle(real);
            s.WriteStringPointer(bw, @string);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            @int = xd.ReadInt32(xe, nameof(@int));
            real = xd.ReadSingle(xe, nameof(real));
            @string = xd.ReadString(xe, nameof(@string));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteNumber(xe, nameof(@int), @int);
            xs.WriteFloat(xe, nameof(real), real);
            xs.WriteString(xe, nameof(@string), @string);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbTestStateChooser);
        }

        public bool Equals(hkbTestStateChooser? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   @int.Equals(other.@int) &&
                   real.Equals(other.real) &&
                   (@string is null && other.@string is null || @string == other.@string || @string is null && other.@string == "" || @string == "" && other.@string is null) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(@int);
            hashcode.Add(real);
            hashcode.Add(@string);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

