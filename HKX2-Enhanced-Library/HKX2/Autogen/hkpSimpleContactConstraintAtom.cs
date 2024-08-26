using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpSimpleContactConstraintAtom Signatire: 0x920df11a size: 48 flags: FLAGS_NONE

    // sizeOfAllAtoms class:  Type.TYPE_UINT16 Type.TYPE_VOID arrSize: 0 offset: 2 flags: FLAGS_NONE enum: 
    // numContactPoints class:  Type.TYPE_UINT16 Type.TYPE_VOID arrSize: 0 offset: 4 flags: FLAGS_NONE enum: 
    // numReservedContactPoints class:  Type.TYPE_UINT16 Type.TYPE_VOID arrSize: 0 offset: 6 flags: FLAGS_NONE enum: 
    // numUserDatasForBodyA class:  Type.TYPE_UINT8 Type.TYPE_VOID arrSize: 0 offset: 8 flags: FLAGS_NONE enum: 
    // numUserDatasForBodyB class:  Type.TYPE_UINT8 Type.TYPE_VOID arrSize: 0 offset: 9 flags: FLAGS_NONE enum: 
    // contactPointPropertiesStriding class:  Type.TYPE_UINT8 Type.TYPE_VOID arrSize: 0 offset: 10 flags: FLAGS_NONE enum: 
    // maxNumContactPoints class:  Type.TYPE_UINT16 Type.TYPE_VOID arrSize: 0 offset: 12 flags: FLAGS_NONE enum: 
    // info class: hkpSimpleContactConstraintDataInfo Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 16 flags: ALIGN_16|FLAGS_NONE enum: 
    public partial class hkpSimpleContactConstraintAtom : hkpConstraintAtom, IEquatable<hkpSimpleContactConstraintAtom?>
    {
        public ushort sizeOfAllAtoms { set; get; }
        public ushort numContactPoints { set; get; }
        public ushort numReservedContactPoints { set; get; }
        public byte numUserDatasForBodyA { set; get; }
        public byte numUserDatasForBodyB { set; get; }
        public byte contactPointPropertiesStriding { set; get; }
        public ushort maxNumContactPoints { set; get; }
        public hkpSimpleContactConstraintDataInfo info { set; get; } = new();

        public override uint Signature { set; get; } = 0x920df11a;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            sizeOfAllAtoms = br.ReadUInt16();
            numContactPoints = br.ReadUInt16();
            numReservedContactPoints = br.ReadUInt16();
            numUserDatasForBodyA = br.ReadByte();
            numUserDatasForBodyB = br.ReadByte();
            contactPointPropertiesStriding = br.ReadByte();
            br.Position += 1;
            maxNumContactPoints = br.ReadUInt16();
            br.Position += 2;
            info.Read(des, br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteUInt16(sizeOfAllAtoms);
            bw.WriteUInt16(numContactPoints);
            bw.WriteUInt16(numReservedContactPoints);
            bw.WriteByte(numUserDatasForBodyA);
            bw.WriteByte(numUserDatasForBodyB);
            bw.WriteByte(contactPointPropertiesStriding);
            bw.Position += 1;
            bw.WriteUInt16(maxNumContactPoints);
            bw.Position += 2;
            info.Write(s, bw);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            sizeOfAllAtoms = xd.ReadUInt16(xe, nameof(sizeOfAllAtoms));
            numContactPoints = xd.ReadUInt16(xe, nameof(numContactPoints));
            numReservedContactPoints = xd.ReadUInt16(xe, nameof(numReservedContactPoints));
            numUserDatasForBodyA = xd.ReadByte(xe, nameof(numUserDatasForBodyA));
            numUserDatasForBodyB = xd.ReadByte(xe, nameof(numUserDatasForBodyB));
            contactPointPropertiesStriding = xd.ReadByte(xe, nameof(contactPointPropertiesStriding));
            maxNumContactPoints = xd.ReadUInt16(xe, nameof(maxNumContactPoints));
            info = xd.ReadClass<hkpSimpleContactConstraintDataInfo>(xe, nameof(info));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteNumber(xe, nameof(sizeOfAllAtoms), sizeOfAllAtoms);
            xs.WriteNumber(xe, nameof(numContactPoints), numContactPoints);
            xs.WriteNumber(xe, nameof(numReservedContactPoints), numReservedContactPoints);
            xs.WriteNumber(xe, nameof(numUserDatasForBodyA), numUserDatasForBodyA);
            xs.WriteNumber(xe, nameof(numUserDatasForBodyB), numUserDatasForBodyB);
            xs.WriteNumber(xe, nameof(contactPointPropertiesStriding), contactPointPropertiesStriding);
            xs.WriteNumber(xe, nameof(maxNumContactPoints), maxNumContactPoints);
            xs.WriteClass<hkpSimpleContactConstraintDataInfo>(xe, nameof(info), info);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpSimpleContactConstraintAtom);
        }

        public bool Equals(hkpSimpleContactConstraintAtom? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   sizeOfAllAtoms.Equals(other.sizeOfAllAtoms) &&
                   numContactPoints.Equals(other.numContactPoints) &&
                   numReservedContactPoints.Equals(other.numReservedContactPoints) &&
                   numUserDatasForBodyA.Equals(other.numUserDatasForBodyA) &&
                   numUserDatasForBodyB.Equals(other.numUserDatasForBodyB) &&
                   contactPointPropertiesStriding.Equals(other.contactPointPropertiesStriding) &&
                   maxNumContactPoints.Equals(other.maxNumContactPoints) &&
                   ((info is null && other.info is null) || (info is not null && other.info is not null && info.Equals((IHavokObject)other.info))) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(sizeOfAllAtoms);
            hashcode.Add(numContactPoints);
            hashcode.Add(numReservedContactPoints);
            hashcode.Add(numUserDatasForBodyA);
            hashcode.Add(numUserDatasForBodyB);
            hashcode.Add(contactPointPropertiesStriding);
            hashcode.Add(maxNumContactPoints);
            hashcode.Add(info);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

