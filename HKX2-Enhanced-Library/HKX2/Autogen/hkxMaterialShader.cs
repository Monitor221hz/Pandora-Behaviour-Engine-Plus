using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkxMaterialShader Signatire: 0x28515eff size: 72 flags: FLAGS_NONE

    // name class:  Type.TYPE_STRINGPTR Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // type class:  Type.TYPE_ENUM Type.TYPE_UINT8 arrSize: 0 offset: 24 flags: FLAGS_NONE enum: ShaderType
    // vertexEntryName class:  Type.TYPE_STRINGPTR Type.TYPE_VOID arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // geomEntryName class:  Type.TYPE_STRINGPTR Type.TYPE_VOID arrSize: 0 offset: 40 flags: FLAGS_NONE enum: 
    // pixelEntryName class:  Type.TYPE_STRINGPTR Type.TYPE_VOID arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    // data class:  Type.TYPE_ARRAY Type.TYPE_UINT8 arrSize: 0 offset: 56 flags: FLAGS_NONE enum: 
    public partial class hkxMaterialShader : hkReferencedObject, IEquatable<hkxMaterialShader?>
    {
        public string name { set; get; } = "";
        public byte type { set; get; }
        public string vertexEntryName { set; get; } = "";
        public string geomEntryName { set; get; } = "";
        public string pixelEntryName { set; get; } = "";
        public IList<byte> data { set; get; } = Array.Empty<byte>();

        public override uint Signature { set; get; } = 0x28515eff;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            name = des.ReadStringPointer(br);
            type = br.ReadByte();
            br.Position += 7;
            vertexEntryName = des.ReadStringPointer(br);
            geomEntryName = des.ReadStringPointer(br);
            pixelEntryName = des.ReadStringPointer(br);
            data = des.ReadByteArray(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteStringPointer(bw, name);
            bw.WriteByte(type);
            bw.Position += 7;
            s.WriteStringPointer(bw, vertexEntryName);
            s.WriteStringPointer(bw, geomEntryName);
            s.WriteStringPointer(bw, pixelEntryName);
            s.WriteByteArray(bw, data);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            name = xd.ReadString(xe, nameof(name));
            type = xd.ReadFlag<ShaderType, byte>(xe, nameof(type));
            vertexEntryName = xd.ReadString(xe, nameof(vertexEntryName));
            geomEntryName = xd.ReadString(xe, nameof(geomEntryName));
            pixelEntryName = xd.ReadString(xe, nameof(pixelEntryName));
            data = xd.ReadByteArray(xe, nameof(data));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteString(xe, nameof(name), name);
            xs.WriteEnum<ShaderType, byte>(xe, nameof(type), type);
            xs.WriteString(xe, nameof(vertexEntryName), vertexEntryName);
            xs.WriteString(xe, nameof(geomEntryName), geomEntryName);
            xs.WriteString(xe, nameof(pixelEntryName), pixelEntryName);
            xs.WriteNumberArray(xe, nameof(data), data);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkxMaterialShader);
        }

        public bool Equals(hkxMaterialShader? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   (name is null && other.name is null || name == other.name || name is null && other.name == "" || name == "" && other.name is null) &&
                   type.Equals(other.type) &&
                   (vertexEntryName is null && other.vertexEntryName is null || vertexEntryName == other.vertexEntryName || vertexEntryName is null && other.vertexEntryName == "" || vertexEntryName == "" && other.vertexEntryName is null) &&
                   (geomEntryName is null && other.geomEntryName is null || geomEntryName == other.geomEntryName || geomEntryName is null && other.geomEntryName == "" || geomEntryName == "" && other.geomEntryName is null) &&
                   (pixelEntryName is null && other.pixelEntryName is null || pixelEntryName == other.pixelEntryName || pixelEntryName is null && other.pixelEntryName == "" || pixelEntryName == "" && other.pixelEntryName is null) &&
                   data.SequenceEqual(other.data) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(name);
            hashcode.Add(type);
            hashcode.Add(vertexEntryName);
            hashcode.Add(geomEntryName);
            hashcode.Add(pixelEntryName);
            hashcode.Add(data.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

