using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbNodeInternalStateInfo Signatire: 0x7db9971d size: 120 flags: FLAGS_NONE

    // syncInfo class: hkbGeneratorSyncInfo Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // name class:  Type.TYPE_STRINGPTR Type.TYPE_VOID arrSize: 0 offset: 96 flags: FLAGS_NONE enum: 
    // internalState class: hkReferencedObject Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 104 flags: FLAGS_NONE enum: 
    // nodeId class:  Type.TYPE_INT16 Type.TYPE_VOID arrSize: 0 offset: 112 flags: FLAGS_NONE enum: 
    // hasActivateBeenCalled class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 114 flags: FLAGS_NONE enum: 
    public partial class hkbNodeInternalStateInfo : hkReferencedObject, IEquatable<hkbNodeInternalStateInfo?>
    {
        public hkbGeneratorSyncInfo syncInfo { set; get; } = new();
        public string name { set; get; } = "";
        public hkReferencedObject? internalState { set; get; }
        public short nodeId { set; get; }
        public bool hasActivateBeenCalled { set; get; }

        public override uint Signature { set; get; } = 0x7db9971d;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            syncInfo.Read(des, br);
            name = des.ReadStringPointer(br);
            internalState = des.ReadClassPointer<hkReferencedObject>(br);
            nodeId = br.ReadInt16();
            hasActivateBeenCalled = br.ReadBoolean();
            br.Position += 5;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            syncInfo.Write(s, bw);
            s.WriteStringPointer(bw, name);
            s.WriteClassPointer(bw, internalState);
            bw.WriteInt16(nodeId);
            bw.WriteBoolean(hasActivateBeenCalled);
            bw.Position += 5;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            syncInfo = xd.ReadClass<hkbGeneratorSyncInfo>(xe, nameof(syncInfo));
            name = xd.ReadString(xe, nameof(name));
            internalState = xd.ReadClassPointer<hkReferencedObject>(this, xe, nameof(internalState));
            nodeId = xd.ReadInt16(xe, nameof(nodeId));
            hasActivateBeenCalled = xd.ReadBoolean(xe, nameof(hasActivateBeenCalled));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClass<hkbGeneratorSyncInfo>(xe, nameof(syncInfo), syncInfo);
            xs.WriteString(xe, nameof(name), name);
            xs.WriteClassPointer(xe, nameof(internalState), internalState);
            xs.WriteNumber(xe, nameof(nodeId), nodeId);
            xs.WriteBoolean(xe, nameof(hasActivateBeenCalled), hasActivateBeenCalled);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbNodeInternalStateInfo);
        }

        public bool Equals(hkbNodeInternalStateInfo? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   ((syncInfo is null && other.syncInfo is null) || (syncInfo is not null && other.syncInfo is not null && syncInfo.Equals((IHavokObject)other.syncInfo))) &&
                   (name is null && other.name is null || name == other.name || name is null && other.name == "" || name == "" && other.name is null) &&
                   ((internalState is null && other.internalState is null) || (internalState is not null && other.internalState is not null && internalState.Equals((IHavokObject)other.internalState))) &&
                   nodeId.Equals(other.nodeId) &&
                   hasActivateBeenCalled.Equals(other.hasActivateBeenCalled) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(syncInfo);
            hashcode.Add(name);
            hashcode.Add(internalState);
            hashcode.Add(nodeId);
            hashcode.Add(hasActivateBeenCalled);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

