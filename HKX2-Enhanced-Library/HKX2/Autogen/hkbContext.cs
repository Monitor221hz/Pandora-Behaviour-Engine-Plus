using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbContext Signatire: 0xe0c4d4a7 size: 80 flags: FLAGS_NONE

    // character class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 0 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // behavior class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 8 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // nodeToIndexMap class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 16 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // eventQueue class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 24 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // sharedEventQueue class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 32 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // generatorOutputListener class: hkbGeneratorOutputListener Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 40 flags: FLAGS_NONE enum: 
    // eventTriggeredTransition class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 48 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // world class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 56 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // attachmentManager class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 64 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // animationCache class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 72 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    public partial class hkbContext : IHavokObject, IEquatable<hkbContext?>
    {
        private object? character { set; get; }
        private object? behavior { set; get; }
        private object? nodeToIndexMap { set; get; }
        private object? eventQueue { set; get; }
        private object? sharedEventQueue { set; get; }
        public hkbGeneratorOutputListener? generatorOutputListener { set; get; }
        private bool eventTriggeredTransition { set; get; }
        private object? world { set; get; }
        private object? attachmentManager { set; get; }
        private object? animationCache { set; get; }

        public virtual uint Signature { set; get; } = 0xe0c4d4a7;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            des.ReadEmptyPointer(br);
            des.ReadEmptyPointer(br);
            des.ReadEmptyPointer(br);
            des.ReadEmptyPointer(br);
            des.ReadEmptyPointer(br);
            generatorOutputListener = des.ReadClassPointer<hkbGeneratorOutputListener>(br);
            eventTriggeredTransition = br.ReadBoolean();
            br.Position += 7;
            des.ReadEmptyPointer(br);
            des.ReadEmptyPointer(br);
            des.ReadEmptyPointer(br);
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            s.WriteVoidPointer(bw);
            s.WriteVoidPointer(bw);
            s.WriteVoidPointer(bw);
            s.WriteVoidPointer(bw);
            s.WriteVoidPointer(bw);
            s.WriteClassPointer(bw, generatorOutputListener);
            bw.WriteBoolean(eventTriggeredTransition);
            bw.Position += 7;
            s.WriteVoidPointer(bw);
            s.WriteVoidPointer(bw);
            s.WriteVoidPointer(bw);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            generatorOutputListener = xd.ReadClassPointer<hkbGeneratorOutputListener>(this, xe, nameof(generatorOutputListener));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteSerializeIgnored(xe, nameof(character));
            xs.WriteSerializeIgnored(xe, nameof(behavior));
            xs.WriteSerializeIgnored(xe, nameof(nodeToIndexMap));
            xs.WriteSerializeIgnored(xe, nameof(eventQueue));
            xs.WriteSerializeIgnored(xe, nameof(sharedEventQueue));
            xs.WriteClassPointer(xe, nameof(generatorOutputListener), generatorOutputListener);
            xs.WriteSerializeIgnored(xe, nameof(eventTriggeredTransition));
            xs.WriteSerializeIgnored(xe, nameof(world));
            xs.WriteSerializeIgnored(xe, nameof(attachmentManager));
            xs.WriteSerializeIgnored(xe, nameof(animationCache));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbContext);
        }

        public bool Equals(hkbContext? other)
        {
            return other is not null &&
                   ((generatorOutputListener is null && other.generatorOutputListener is null) || (generatorOutputListener is not null && other.generatorOutputListener is not null && generatorOutputListener.Equals((IHavokObject)other.generatorOutputListener))) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(generatorOutputListener);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

