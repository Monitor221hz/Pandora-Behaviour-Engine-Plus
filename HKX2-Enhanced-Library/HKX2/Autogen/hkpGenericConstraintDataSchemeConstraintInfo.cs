using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpGenericConstraintDataSchemeConstraintInfo Signatire: 0xd6421f19 size: 16 flags: FLAGS_NONE

    // maxSizeOfSchema class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // sizeOfSchemas class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 4 flags: FLAGS_NONE enum: 
    // numSolverResults class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 8 flags: FLAGS_NONE enum: 
    // numSolverElemTemps class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 12 flags: FLAGS_NONE enum: 
    public partial class hkpGenericConstraintDataSchemeConstraintInfo : IHavokObject, IEquatable<hkpGenericConstraintDataSchemeConstraintInfo?>
    {
        public int maxSizeOfSchema { set; get; }
        public int sizeOfSchemas { set; get; }
        public int numSolverResults { set; get; }
        public int numSolverElemTemps { set; get; }

        public virtual uint Signature { set; get; } = 0xd6421f19;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            maxSizeOfSchema = br.ReadInt32();
            sizeOfSchemas = br.ReadInt32();
            numSolverResults = br.ReadInt32();
            numSolverElemTemps = br.ReadInt32();
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            bw.WriteInt32(maxSizeOfSchema);
            bw.WriteInt32(sizeOfSchemas);
            bw.WriteInt32(numSolverResults);
            bw.WriteInt32(numSolverElemTemps);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            maxSizeOfSchema = xd.ReadInt32(xe, nameof(maxSizeOfSchema));
            sizeOfSchemas = xd.ReadInt32(xe, nameof(sizeOfSchemas));
            numSolverResults = xd.ReadInt32(xe, nameof(numSolverResults));
            numSolverElemTemps = xd.ReadInt32(xe, nameof(numSolverElemTemps));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteNumber(xe, nameof(maxSizeOfSchema), maxSizeOfSchema);
            xs.WriteNumber(xe, nameof(sizeOfSchemas), sizeOfSchemas);
            xs.WriteNumber(xe, nameof(numSolverResults), numSolverResults);
            xs.WriteNumber(xe, nameof(numSolverElemTemps), numSolverElemTemps);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpGenericConstraintDataSchemeConstraintInfo);
        }

        public bool Equals(hkpGenericConstraintDataSchemeConstraintInfo? other)
        {
            return other is not null &&
                   maxSizeOfSchema.Equals(other.maxSizeOfSchema) &&
                   sizeOfSchemas.Equals(other.sizeOfSchemas) &&
                   numSolverResults.Equals(other.numSolverResults) &&
                   numSolverElemTemps.Equals(other.numSolverElemTemps) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(maxSizeOfSchema);
            hashcode.Add(sizeOfSchemas);
            hashcode.Add(numSolverResults);
            hashcode.Add(numSolverElemTemps);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

