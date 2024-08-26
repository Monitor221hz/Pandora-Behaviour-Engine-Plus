using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbClipGeneratorInternalState Signatire: 0x26ce5bf3 size: 112 flags: FLAGS_NONE

    // extractedMotion class:  Type.TYPE_QSTRANSFORM Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // echos class: hkbClipGeneratorEcho Type.TYPE_ARRAY Type.TYPE_STRUCT arrSize: 0 offset: 64 flags: FLAGS_NONE enum: 
    // localTime class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    // time class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 84 flags: FLAGS_NONE enum: 
    // previousUserControlledTimeFraction class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 88 flags: FLAGS_NONE enum: 
    // bufferSize class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 92 flags: FLAGS_NONE enum: 
    // echoBufferSize class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 96 flags: FLAGS_NONE enum: 
    // atEnd class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 100 flags: FLAGS_NONE enum: 
    // ignoreStartTime class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 101 flags: FLAGS_NONE enum: 
    // pingPongBackward class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 102 flags: FLAGS_NONE enum: 
    public partial class hkbClipGeneratorInternalState : hkReferencedObject, IEquatable<hkbClipGeneratorInternalState?>
    {
        public Matrix4x4 extractedMotion { set; get; }
        public IList<hkbClipGeneratorEcho> echos { set; get; } = Array.Empty<hkbClipGeneratorEcho>();
        public float localTime { set; get; }
        public float time { set; get; }
        public float previousUserControlledTimeFraction { set; get; }
        public int bufferSize { set; get; }
        public int echoBufferSize { set; get; }
        public bool atEnd { set; get; }
        public bool ignoreStartTime { set; get; }
        public bool pingPongBackward { set; get; }

        public override uint Signature { set; get; } = 0x26ce5bf3;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            extractedMotion = des.ReadQSTransform(br);
            echos = des.ReadClassArray<hkbClipGeneratorEcho>(br);
            localTime = br.ReadSingle();
            time = br.ReadSingle();
            previousUserControlledTimeFraction = br.ReadSingle();
            bufferSize = br.ReadInt32();
            echoBufferSize = br.ReadInt32();
            atEnd = br.ReadBoolean();
            ignoreStartTime = br.ReadBoolean();
            pingPongBackward = br.ReadBoolean();
            br.Position += 9;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteQSTransform(bw, extractedMotion);
            s.WriteClassArray(bw, echos);
            bw.WriteSingle(localTime);
            bw.WriteSingle(time);
            bw.WriteSingle(previousUserControlledTimeFraction);
            bw.WriteInt32(bufferSize);
            bw.WriteInt32(echoBufferSize);
            bw.WriteBoolean(atEnd);
            bw.WriteBoolean(ignoreStartTime);
            bw.WriteBoolean(pingPongBackward);
            bw.Position += 9;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            extractedMotion = xd.ReadQSTransform(xe, nameof(extractedMotion));
            echos = xd.ReadClassArray<hkbClipGeneratorEcho>(xe, nameof(echos));
            localTime = xd.ReadSingle(xe, nameof(localTime));
            time = xd.ReadSingle(xe, nameof(time));
            previousUserControlledTimeFraction = xd.ReadSingle(xe, nameof(previousUserControlledTimeFraction));
            bufferSize = xd.ReadInt32(xe, nameof(bufferSize));
            echoBufferSize = xd.ReadInt32(xe, nameof(echoBufferSize));
            atEnd = xd.ReadBoolean(xe, nameof(atEnd));
            ignoreStartTime = xd.ReadBoolean(xe, nameof(ignoreStartTime));
            pingPongBackward = xd.ReadBoolean(xe, nameof(pingPongBackward));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteQSTransform(xe, nameof(extractedMotion), extractedMotion);
            xs.WriteClassArray(xe, nameof(echos), echos);
            xs.WriteFloat(xe, nameof(localTime), localTime);
            xs.WriteFloat(xe, nameof(time), time);
            xs.WriteFloat(xe, nameof(previousUserControlledTimeFraction), previousUserControlledTimeFraction);
            xs.WriteNumber(xe, nameof(bufferSize), bufferSize);
            xs.WriteNumber(xe, nameof(echoBufferSize), echoBufferSize);
            xs.WriteBoolean(xe, nameof(atEnd), atEnd);
            xs.WriteBoolean(xe, nameof(ignoreStartTime), ignoreStartTime);
            xs.WriteBoolean(xe, nameof(pingPongBackward), pingPongBackward);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbClipGeneratorInternalState);
        }

        public bool Equals(hkbClipGeneratorInternalState? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   extractedMotion.Equals(other.extractedMotion) &&
                   echos.SequenceEqual(other.echos) &&
                   localTime.Equals(other.localTime) &&
                   time.Equals(other.time) &&
                   previousUserControlledTimeFraction.Equals(other.previousUserControlledTimeFraction) &&
                   bufferSize.Equals(other.bufferSize) &&
                   echoBufferSize.Equals(other.echoBufferSize) &&
                   atEnd.Equals(other.atEnd) &&
                   ignoreStartTime.Equals(other.ignoreStartTime) &&
                   pingPongBackward.Equals(other.pingPongBackward) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(extractedMotion);
            hashcode.Add(echos.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(localTime);
            hashcode.Add(time);
            hashcode.Add(previousUserControlledTimeFraction);
            hashcode.Add(bufferSize);
            hashcode.Add(echoBufferSize);
            hashcode.Add(atEnd);
            hashcode.Add(ignoreStartTime);
            hashcode.Add(pingPongBackward);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

