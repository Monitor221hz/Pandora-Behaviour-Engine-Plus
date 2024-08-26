using System;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpWorldCinfo Signatire: 0xa5255445 size: 256 flags: FLAGS_NONE

    // gravity class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // broadPhaseQuerySize class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // contactRestingVelocity class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 36 flags: FLAGS_NONE enum: 
    // broadPhaseBorderBehaviour class:  Type.TYPE_ENUM Type.TYPE_INT8 arrSize: 0 offset: 40 flags: FLAGS_NONE enum: BroadPhaseBorderBehaviour
    // mtPostponeAndSortBroadPhaseBorderCallbacks class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 41 flags: FLAGS_NONE enum: 
    // broadPhaseWorldAabb class: hkAabb Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    // useKdTree class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    // useMultipleTree class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 81 flags: FLAGS_NONE enum: 
    // treeUpdateType class:  Type.TYPE_ENUM Type.TYPE_INT8 arrSize: 0 offset: 82 flags: FLAGS_NONE enum: TreeUpdateType
    // autoUpdateKdTree class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 83 flags: FLAGS_NONE enum: 
    // collisionTolerance class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 84 flags: FLAGS_NONE enum: 
    // collisionFilter class: hkpCollisionFilter Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 88 flags: FLAGS_NONE enum: 
    // convexListFilter class: hkpConvexListFilter Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 96 flags: FLAGS_NONE enum: 
    // expectedMaxLinearVelocity class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 104 flags: FLAGS_NONE enum: 
    // sizeOfToiEventQueue class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 108 flags: FLAGS_NONE enum: 
    // expectedMinPsiDeltaTime class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 112 flags: FLAGS_NONE enum: 
    // memoryWatchDog class: hkWorldMemoryAvailableWatchDog Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 120 flags: FLAGS_NONE enum: 
    // broadPhaseNumMarkers class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 128 flags: FLAGS_NONE enum: 
    // contactPointGeneration class:  Type.TYPE_ENUM Type.TYPE_INT8 arrSize: 0 offset: 132 flags: FLAGS_NONE enum: ContactPointGeneration
    // allowToSkipConfirmedCallbacks class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 133 flags: FLAGS_NONE enum: 
    // useHybridBroadphase class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 134 flags: FLAGS_NONE enum: 
    // solverTau class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 136 flags: FLAGS_NONE enum: 
    // solverDamp class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 140 flags: FLAGS_NONE enum: 
    // solverIterations class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 144 flags: FLAGS_NONE enum: 
    // solverMicrosteps class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 148 flags: FLAGS_NONE enum: 
    // maxConstraintViolation class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 152 flags: FLAGS_NONE enum: 
    // forceCoherentConstraintOrderingInSolver class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 156 flags: FLAGS_NONE enum: 
    // snapCollisionToConvexEdgeThreshold class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 160 flags: FLAGS_NONE enum: 
    // snapCollisionToConcaveEdgeThreshold class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 164 flags: FLAGS_NONE enum: 
    // enableToiWeldRejection class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 168 flags: FLAGS_NONE enum: 
    // enableDeprecatedWelding class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 169 flags: FLAGS_NONE enum: 
    // iterativeLinearCastEarlyOutDistance class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 172 flags: FLAGS_NONE enum: 
    // iterativeLinearCastMaxIterations class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 176 flags: FLAGS_NONE enum: 
    // deactivationNumInactiveFramesSelectFlag0 class:  Type.TYPE_UINT8 Type.TYPE_VOID arrSize: 0 offset: 180 flags: FLAGS_NONE enum: 
    // deactivationNumInactiveFramesSelectFlag1 class:  Type.TYPE_UINT8 Type.TYPE_VOID arrSize: 0 offset: 181 flags: FLAGS_NONE enum: 
    // deactivationIntegrateCounter class:  Type.TYPE_UINT8 Type.TYPE_VOID arrSize: 0 offset: 182 flags: FLAGS_NONE enum: 
    // shouldActivateOnRigidBodyTransformChange class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 183 flags: FLAGS_NONE enum: 
    // deactivationReferenceDistance class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 184 flags: FLAGS_NONE enum: 
    // toiCollisionResponseRotateNormal class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 188 flags: FLAGS_NONE enum: 
    // maxSectorsPerMidphaseCollideTask class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 192 flags: FLAGS_NONE enum: 
    // maxSectorsPerNarrowphaseCollideTask class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 196 flags: FLAGS_NONE enum: 
    // processToisMultithreaded class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 200 flags: FLAGS_NONE enum: 
    // maxEntriesPerToiMidphaseCollideTask class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 204 flags: FLAGS_NONE enum: 
    // maxEntriesPerToiNarrowphaseCollideTask class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 208 flags: FLAGS_NONE enum: 
    // maxNumToiCollisionPairsSinglethreaded class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 212 flags: FLAGS_NONE enum: 
    // numToisTillAllowedPenetrationSimplifiedToi class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 216 flags: FLAGS_NONE enum: 
    // numToisTillAllowedPenetrationToi class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 220 flags: FLAGS_NONE enum: 
    // numToisTillAllowedPenetrationToiHigher class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 224 flags: FLAGS_NONE enum: 
    // numToisTillAllowedPenetrationToiForced class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 228 flags: FLAGS_NONE enum: 
    // enableDeactivation class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 232 flags: FLAGS_NONE enum: 
    // simulationType class:  Type.TYPE_ENUM Type.TYPE_INT8 arrSize: 0 offset: 233 flags: FLAGS_NONE enum: SimulationType
    // enableSimulationIslands class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 234 flags: FLAGS_NONE enum: 
    // minDesiredIslandSize class:  Type.TYPE_UINT32 Type.TYPE_VOID arrSize: 0 offset: 236 flags: FLAGS_NONE enum: 
    // processActionsInSingleThread class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 240 flags: FLAGS_NONE enum: 
    // allowIntegrationOfIslandsWithoutConstraintsInASeparateJob class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 241 flags: FLAGS_NONE enum: 
    // frameMarkerPsiSnap class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 244 flags: FLAGS_NONE enum: 
    // fireCollisionCallbacks class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 248 flags: FLAGS_NONE enum: 
    public partial class hkpWorldCinfo : hkReferencedObject, IEquatable<hkpWorldCinfo?>
    {
        public Vector4 gravity { set; get; }
        public int broadPhaseQuerySize { set; get; }
        public float contactRestingVelocity { set; get; }
        public sbyte broadPhaseBorderBehaviour { set; get; }
        public bool mtPostponeAndSortBroadPhaseBorderCallbacks { set; get; }
        public hkAabb broadPhaseWorldAabb { set; get; } = new();
        public bool useKdTree { set; get; }
        public bool useMultipleTree { set; get; }
        public sbyte treeUpdateType { set; get; }
        public bool autoUpdateKdTree { set; get; }
        public float collisionTolerance { set; get; }
        public hkpCollisionFilter? collisionFilter { set; get; }
        public hkpConvexListFilter? convexListFilter { set; get; }
        public float expectedMaxLinearVelocity { set; get; }
        public int sizeOfToiEventQueue { set; get; }
        public float expectedMinPsiDeltaTime { set; get; }
        public hkWorldMemoryAvailableWatchDog? memoryWatchDog { set; get; }
        public int broadPhaseNumMarkers { set; get; }
        public sbyte contactPointGeneration { set; get; }
        public bool allowToSkipConfirmedCallbacks { set; get; }
        public bool useHybridBroadphase { set; get; }
        public float solverTau { set; get; }
        public float solverDamp { set; get; }
        public int solverIterations { set; get; }
        public int solverMicrosteps { set; get; }
        public float maxConstraintViolation { set; get; }
        public bool forceCoherentConstraintOrderingInSolver { set; get; }
        public float snapCollisionToConvexEdgeThreshold { set; get; }
        public float snapCollisionToConcaveEdgeThreshold { set; get; }
        public bool enableToiWeldRejection { set; get; }
        public bool enableDeprecatedWelding { set; get; }
        public float iterativeLinearCastEarlyOutDistance { set; get; }
        public int iterativeLinearCastMaxIterations { set; get; }
        public byte deactivationNumInactiveFramesSelectFlag0 { set; get; }
        public byte deactivationNumInactiveFramesSelectFlag1 { set; get; }
        public byte deactivationIntegrateCounter { set; get; }
        public bool shouldActivateOnRigidBodyTransformChange { set; get; }
        public float deactivationReferenceDistance { set; get; }
        public float toiCollisionResponseRotateNormal { set; get; }
        public int maxSectorsPerMidphaseCollideTask { set; get; }
        public int maxSectorsPerNarrowphaseCollideTask { set; get; }
        public bool processToisMultithreaded { set; get; }
        public int maxEntriesPerToiMidphaseCollideTask { set; get; }
        public int maxEntriesPerToiNarrowphaseCollideTask { set; get; }
        public int maxNumToiCollisionPairsSinglethreaded { set; get; }
        public float numToisTillAllowedPenetrationSimplifiedToi { set; get; }
        public float numToisTillAllowedPenetrationToi { set; get; }
        public float numToisTillAllowedPenetrationToiHigher { set; get; }
        public float numToisTillAllowedPenetrationToiForced { set; get; }
        public bool enableDeactivation { set; get; }
        public sbyte simulationType { set; get; }
        public bool enableSimulationIslands { set; get; }
        public uint minDesiredIslandSize { set; get; }
        public bool processActionsInSingleThread { set; get; }
        public bool allowIntegrationOfIslandsWithoutConstraintsInASeparateJob { set; get; }
        public float frameMarkerPsiSnap { set; get; }
        public bool fireCollisionCallbacks { set; get; }

        public override uint Signature { set; get; } = 0xa5255445;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            gravity = br.ReadVector4();
            broadPhaseQuerySize = br.ReadInt32();
            contactRestingVelocity = br.ReadSingle();
            broadPhaseBorderBehaviour = br.ReadSByte();
            mtPostponeAndSortBroadPhaseBorderCallbacks = br.ReadBoolean();
            br.Position += 6;
            broadPhaseWorldAabb.Read(des, br);
            useKdTree = br.ReadBoolean();
            useMultipleTree = br.ReadBoolean();
            treeUpdateType = br.ReadSByte();
            autoUpdateKdTree = br.ReadBoolean();
            collisionTolerance = br.ReadSingle();
            collisionFilter = des.ReadClassPointer<hkpCollisionFilter>(br);
            convexListFilter = des.ReadClassPointer<hkpConvexListFilter>(br);
            expectedMaxLinearVelocity = br.ReadSingle();
            sizeOfToiEventQueue = br.ReadInt32();
            expectedMinPsiDeltaTime = br.ReadSingle();
            br.Position += 4;
            memoryWatchDog = des.ReadClassPointer<hkWorldMemoryAvailableWatchDog>(br);
            broadPhaseNumMarkers = br.ReadInt32();
            contactPointGeneration = br.ReadSByte();
            allowToSkipConfirmedCallbacks = br.ReadBoolean();
            useHybridBroadphase = br.ReadBoolean();
            br.Position += 1;
            solverTau = br.ReadSingle();
            solverDamp = br.ReadSingle();
            solverIterations = br.ReadInt32();
            solverMicrosteps = br.ReadInt32();
            maxConstraintViolation = br.ReadSingle();
            forceCoherentConstraintOrderingInSolver = br.ReadBoolean();
            br.Position += 3;
            snapCollisionToConvexEdgeThreshold = br.ReadSingle();
            snapCollisionToConcaveEdgeThreshold = br.ReadSingle();
            enableToiWeldRejection = br.ReadBoolean();
            enableDeprecatedWelding = br.ReadBoolean();
            br.Position += 2;
            iterativeLinearCastEarlyOutDistance = br.ReadSingle();
            iterativeLinearCastMaxIterations = br.ReadInt32();
            deactivationNumInactiveFramesSelectFlag0 = br.ReadByte();
            deactivationNumInactiveFramesSelectFlag1 = br.ReadByte();
            deactivationIntegrateCounter = br.ReadByte();
            shouldActivateOnRigidBodyTransformChange = br.ReadBoolean();
            deactivationReferenceDistance = br.ReadSingle();
            toiCollisionResponseRotateNormal = br.ReadSingle();
            maxSectorsPerMidphaseCollideTask = br.ReadInt32();
            maxSectorsPerNarrowphaseCollideTask = br.ReadInt32();
            processToisMultithreaded = br.ReadBoolean();
            br.Position += 3;
            maxEntriesPerToiMidphaseCollideTask = br.ReadInt32();
            maxEntriesPerToiNarrowphaseCollideTask = br.ReadInt32();
            maxNumToiCollisionPairsSinglethreaded = br.ReadInt32();
            numToisTillAllowedPenetrationSimplifiedToi = br.ReadSingle();
            numToisTillAllowedPenetrationToi = br.ReadSingle();
            numToisTillAllowedPenetrationToiHigher = br.ReadSingle();
            numToisTillAllowedPenetrationToiForced = br.ReadSingle();
            enableDeactivation = br.ReadBoolean();
            simulationType = br.ReadSByte();
            enableSimulationIslands = br.ReadBoolean();
            br.Position += 1;
            minDesiredIslandSize = br.ReadUInt32();
            processActionsInSingleThread = br.ReadBoolean();
            allowIntegrationOfIslandsWithoutConstraintsInASeparateJob = br.ReadBoolean();
            br.Position += 2;
            frameMarkerPsiSnap = br.ReadSingle();
            fireCollisionCallbacks = br.ReadBoolean();
            br.Position += 7;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteVector4(gravity);
            bw.WriteInt32(broadPhaseQuerySize);
            bw.WriteSingle(contactRestingVelocity);
            bw.WriteSByte(broadPhaseBorderBehaviour);
            bw.WriteBoolean(mtPostponeAndSortBroadPhaseBorderCallbacks);
            bw.Position += 6;
            broadPhaseWorldAabb.Write(s, bw);
            bw.WriteBoolean(useKdTree);
            bw.WriteBoolean(useMultipleTree);
            bw.WriteSByte(treeUpdateType);
            bw.WriteBoolean(autoUpdateKdTree);
            bw.WriteSingle(collisionTolerance);
            s.WriteClassPointer(bw, collisionFilter);
            s.WriteClassPointer(bw, convexListFilter);
            bw.WriteSingle(expectedMaxLinearVelocity);
            bw.WriteInt32(sizeOfToiEventQueue);
            bw.WriteSingle(expectedMinPsiDeltaTime);
            bw.Position += 4;
            s.WriteClassPointer(bw, memoryWatchDog);
            bw.WriteInt32(broadPhaseNumMarkers);
            bw.WriteSByte(contactPointGeneration);
            bw.WriteBoolean(allowToSkipConfirmedCallbacks);
            bw.WriteBoolean(useHybridBroadphase);
            bw.Position += 1;
            bw.WriteSingle(solverTau);
            bw.WriteSingle(solverDamp);
            bw.WriteInt32(solverIterations);
            bw.WriteInt32(solverMicrosteps);
            bw.WriteSingle(maxConstraintViolation);
            bw.WriteBoolean(forceCoherentConstraintOrderingInSolver);
            bw.Position += 3;
            bw.WriteSingle(snapCollisionToConvexEdgeThreshold);
            bw.WriteSingle(snapCollisionToConcaveEdgeThreshold);
            bw.WriteBoolean(enableToiWeldRejection);
            bw.WriteBoolean(enableDeprecatedWelding);
            bw.Position += 2;
            bw.WriteSingle(iterativeLinearCastEarlyOutDistance);
            bw.WriteInt32(iterativeLinearCastMaxIterations);
            bw.WriteByte(deactivationNumInactiveFramesSelectFlag0);
            bw.WriteByte(deactivationNumInactiveFramesSelectFlag1);
            bw.WriteByte(deactivationIntegrateCounter);
            bw.WriteBoolean(shouldActivateOnRigidBodyTransformChange);
            bw.WriteSingle(deactivationReferenceDistance);
            bw.WriteSingle(toiCollisionResponseRotateNormal);
            bw.WriteInt32(maxSectorsPerMidphaseCollideTask);
            bw.WriteInt32(maxSectorsPerNarrowphaseCollideTask);
            bw.WriteBoolean(processToisMultithreaded);
            bw.Position += 3;
            bw.WriteInt32(maxEntriesPerToiMidphaseCollideTask);
            bw.WriteInt32(maxEntriesPerToiNarrowphaseCollideTask);
            bw.WriteInt32(maxNumToiCollisionPairsSinglethreaded);
            bw.WriteSingle(numToisTillAllowedPenetrationSimplifiedToi);
            bw.WriteSingle(numToisTillAllowedPenetrationToi);
            bw.WriteSingle(numToisTillAllowedPenetrationToiHigher);
            bw.WriteSingle(numToisTillAllowedPenetrationToiForced);
            bw.WriteBoolean(enableDeactivation);
            bw.WriteSByte(simulationType);
            bw.WriteBoolean(enableSimulationIslands);
            bw.Position += 1;
            bw.WriteUInt32(minDesiredIslandSize);
            bw.WriteBoolean(processActionsInSingleThread);
            bw.WriteBoolean(allowIntegrationOfIslandsWithoutConstraintsInASeparateJob);
            bw.Position += 2;
            bw.WriteSingle(frameMarkerPsiSnap);
            bw.WriteBoolean(fireCollisionCallbacks);
            bw.Position += 7;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            gravity = xd.ReadVector4(xe, nameof(gravity));
            broadPhaseQuerySize = xd.ReadInt32(xe, nameof(broadPhaseQuerySize));
            contactRestingVelocity = xd.ReadSingle(xe, nameof(contactRestingVelocity));
            broadPhaseBorderBehaviour = xd.ReadFlag<BroadPhaseBorderBehaviour, sbyte>(xe, nameof(broadPhaseBorderBehaviour));
            mtPostponeAndSortBroadPhaseBorderCallbacks = xd.ReadBoolean(xe, nameof(mtPostponeAndSortBroadPhaseBorderCallbacks));
            broadPhaseWorldAabb = xd.ReadClass<hkAabb>(xe, nameof(broadPhaseWorldAabb));
            useKdTree = xd.ReadBoolean(xe, nameof(useKdTree));
            useMultipleTree = xd.ReadBoolean(xe, nameof(useMultipleTree));
            treeUpdateType = xd.ReadFlag<TreeUpdateType, sbyte>(xe, nameof(treeUpdateType));
            autoUpdateKdTree = xd.ReadBoolean(xe, nameof(autoUpdateKdTree));
            collisionTolerance = xd.ReadSingle(xe, nameof(collisionTolerance));
            collisionFilter = xd.ReadClassPointer<hkpCollisionFilter>(this, xe, nameof(collisionFilter));
            convexListFilter = xd.ReadClassPointer<hkpConvexListFilter>(this, xe, nameof(convexListFilter));
            expectedMaxLinearVelocity = xd.ReadSingle(xe, nameof(expectedMaxLinearVelocity));
            sizeOfToiEventQueue = xd.ReadInt32(xe, nameof(sizeOfToiEventQueue));
            expectedMinPsiDeltaTime = xd.ReadSingle(xe, nameof(expectedMinPsiDeltaTime));
            memoryWatchDog = xd.ReadClassPointer<hkWorldMemoryAvailableWatchDog>(this, xe, nameof(memoryWatchDog));
            broadPhaseNumMarkers = xd.ReadInt32(xe, nameof(broadPhaseNumMarkers));
            contactPointGeneration = xd.ReadFlag<ContactPointGeneration, sbyte>(xe, nameof(contactPointGeneration));
            allowToSkipConfirmedCallbacks = xd.ReadBoolean(xe, nameof(allowToSkipConfirmedCallbacks));
            useHybridBroadphase = xd.ReadBoolean(xe, nameof(useHybridBroadphase));
            solverTau = xd.ReadSingle(xe, nameof(solverTau));
            solverDamp = xd.ReadSingle(xe, nameof(solverDamp));
            solverIterations = xd.ReadInt32(xe, nameof(solverIterations));
            solverMicrosteps = xd.ReadInt32(xe, nameof(solverMicrosteps));
            maxConstraintViolation = xd.ReadSingle(xe, nameof(maxConstraintViolation));
            forceCoherentConstraintOrderingInSolver = xd.ReadBoolean(xe, nameof(forceCoherentConstraintOrderingInSolver));
            snapCollisionToConvexEdgeThreshold = xd.ReadSingle(xe, nameof(snapCollisionToConvexEdgeThreshold));
            snapCollisionToConcaveEdgeThreshold = xd.ReadSingle(xe, nameof(snapCollisionToConcaveEdgeThreshold));
            enableToiWeldRejection = xd.ReadBoolean(xe, nameof(enableToiWeldRejection));
            enableDeprecatedWelding = xd.ReadBoolean(xe, nameof(enableDeprecatedWelding));
            iterativeLinearCastEarlyOutDistance = xd.ReadSingle(xe, nameof(iterativeLinearCastEarlyOutDistance));
            iterativeLinearCastMaxIterations = xd.ReadInt32(xe, nameof(iterativeLinearCastMaxIterations));
            deactivationNumInactiveFramesSelectFlag0 = xd.ReadByte(xe, nameof(deactivationNumInactiveFramesSelectFlag0));
            deactivationNumInactiveFramesSelectFlag1 = xd.ReadByte(xe, nameof(deactivationNumInactiveFramesSelectFlag1));
            deactivationIntegrateCounter = xd.ReadByte(xe, nameof(deactivationIntegrateCounter));
            shouldActivateOnRigidBodyTransformChange = xd.ReadBoolean(xe, nameof(shouldActivateOnRigidBodyTransformChange));
            deactivationReferenceDistance = xd.ReadSingle(xe, nameof(deactivationReferenceDistance));
            toiCollisionResponseRotateNormal = xd.ReadSingle(xe, nameof(toiCollisionResponseRotateNormal));
            maxSectorsPerMidphaseCollideTask = xd.ReadInt32(xe, nameof(maxSectorsPerMidphaseCollideTask));
            maxSectorsPerNarrowphaseCollideTask = xd.ReadInt32(xe, nameof(maxSectorsPerNarrowphaseCollideTask));
            processToisMultithreaded = xd.ReadBoolean(xe, nameof(processToisMultithreaded));
            maxEntriesPerToiMidphaseCollideTask = xd.ReadInt32(xe, nameof(maxEntriesPerToiMidphaseCollideTask));
            maxEntriesPerToiNarrowphaseCollideTask = xd.ReadInt32(xe, nameof(maxEntriesPerToiNarrowphaseCollideTask));
            maxNumToiCollisionPairsSinglethreaded = xd.ReadInt32(xe, nameof(maxNumToiCollisionPairsSinglethreaded));
            numToisTillAllowedPenetrationSimplifiedToi = xd.ReadSingle(xe, nameof(numToisTillAllowedPenetrationSimplifiedToi));
            numToisTillAllowedPenetrationToi = xd.ReadSingle(xe, nameof(numToisTillAllowedPenetrationToi));
            numToisTillAllowedPenetrationToiHigher = xd.ReadSingle(xe, nameof(numToisTillAllowedPenetrationToiHigher));
            numToisTillAllowedPenetrationToiForced = xd.ReadSingle(xe, nameof(numToisTillAllowedPenetrationToiForced));
            enableDeactivation = xd.ReadBoolean(xe, nameof(enableDeactivation));
            simulationType = xd.ReadFlag<SimulationType, sbyte>(xe, nameof(simulationType));
            enableSimulationIslands = xd.ReadBoolean(xe, nameof(enableSimulationIslands));
            minDesiredIslandSize = xd.ReadUInt32(xe, nameof(minDesiredIslandSize));
            processActionsInSingleThread = xd.ReadBoolean(xe, nameof(processActionsInSingleThread));
            allowIntegrationOfIslandsWithoutConstraintsInASeparateJob = xd.ReadBoolean(xe, nameof(allowIntegrationOfIslandsWithoutConstraintsInASeparateJob));
            frameMarkerPsiSnap = xd.ReadSingle(xe, nameof(frameMarkerPsiSnap));
            fireCollisionCallbacks = xd.ReadBoolean(xe, nameof(fireCollisionCallbacks));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteVector4(xe, nameof(gravity), gravity);
            xs.WriteNumber(xe, nameof(broadPhaseQuerySize), broadPhaseQuerySize);
            xs.WriteFloat(xe, nameof(contactRestingVelocity), contactRestingVelocity);
            xs.WriteEnum<BroadPhaseBorderBehaviour, sbyte>(xe, nameof(broadPhaseBorderBehaviour), broadPhaseBorderBehaviour);
            xs.WriteBoolean(xe, nameof(mtPostponeAndSortBroadPhaseBorderCallbacks), mtPostponeAndSortBroadPhaseBorderCallbacks);
            xs.WriteClass<hkAabb>(xe, nameof(broadPhaseWorldAabb), broadPhaseWorldAabb);
            xs.WriteBoolean(xe, nameof(useKdTree), useKdTree);
            xs.WriteBoolean(xe, nameof(useMultipleTree), useMultipleTree);
            xs.WriteEnum<TreeUpdateType, sbyte>(xe, nameof(treeUpdateType), treeUpdateType);
            xs.WriteBoolean(xe, nameof(autoUpdateKdTree), autoUpdateKdTree);
            xs.WriteFloat(xe, nameof(collisionTolerance), collisionTolerance);
            xs.WriteClassPointer(xe, nameof(collisionFilter), collisionFilter);
            xs.WriteClassPointer(xe, nameof(convexListFilter), convexListFilter);
            xs.WriteFloat(xe, nameof(expectedMaxLinearVelocity), expectedMaxLinearVelocity);
            xs.WriteNumber(xe, nameof(sizeOfToiEventQueue), sizeOfToiEventQueue);
            xs.WriteFloat(xe, nameof(expectedMinPsiDeltaTime), expectedMinPsiDeltaTime);
            xs.WriteClassPointer(xe, nameof(memoryWatchDog), memoryWatchDog);
            xs.WriteNumber(xe, nameof(broadPhaseNumMarkers), broadPhaseNumMarkers);
            xs.WriteEnum<ContactPointGeneration, sbyte>(xe, nameof(contactPointGeneration), contactPointGeneration);
            xs.WriteBoolean(xe, nameof(allowToSkipConfirmedCallbacks), allowToSkipConfirmedCallbacks);
            xs.WriteBoolean(xe, nameof(useHybridBroadphase), useHybridBroadphase);
            xs.WriteFloat(xe, nameof(solverTau), solverTau);
            xs.WriteFloat(xe, nameof(solverDamp), solverDamp);
            xs.WriteNumber(xe, nameof(solverIterations), solverIterations);
            xs.WriteNumber(xe, nameof(solverMicrosteps), solverMicrosteps);
            xs.WriteFloat(xe, nameof(maxConstraintViolation), maxConstraintViolation);
            xs.WriteBoolean(xe, nameof(forceCoherentConstraintOrderingInSolver), forceCoherentConstraintOrderingInSolver);
            xs.WriteFloat(xe, nameof(snapCollisionToConvexEdgeThreshold), snapCollisionToConvexEdgeThreshold);
            xs.WriteFloat(xe, nameof(snapCollisionToConcaveEdgeThreshold), snapCollisionToConcaveEdgeThreshold);
            xs.WriteBoolean(xe, nameof(enableToiWeldRejection), enableToiWeldRejection);
            xs.WriteBoolean(xe, nameof(enableDeprecatedWelding), enableDeprecatedWelding);
            xs.WriteFloat(xe, nameof(iterativeLinearCastEarlyOutDistance), iterativeLinearCastEarlyOutDistance);
            xs.WriteNumber(xe, nameof(iterativeLinearCastMaxIterations), iterativeLinearCastMaxIterations);
            xs.WriteNumber(xe, nameof(deactivationNumInactiveFramesSelectFlag0), deactivationNumInactiveFramesSelectFlag0);
            xs.WriteNumber(xe, nameof(deactivationNumInactiveFramesSelectFlag1), deactivationNumInactiveFramesSelectFlag1);
            xs.WriteNumber(xe, nameof(deactivationIntegrateCounter), deactivationIntegrateCounter);
            xs.WriteBoolean(xe, nameof(shouldActivateOnRigidBodyTransformChange), shouldActivateOnRigidBodyTransformChange);
            xs.WriteFloat(xe, nameof(deactivationReferenceDistance), deactivationReferenceDistance);
            xs.WriteFloat(xe, nameof(toiCollisionResponseRotateNormal), toiCollisionResponseRotateNormal);
            xs.WriteNumber(xe, nameof(maxSectorsPerMidphaseCollideTask), maxSectorsPerMidphaseCollideTask);
            xs.WriteNumber(xe, nameof(maxSectorsPerNarrowphaseCollideTask), maxSectorsPerNarrowphaseCollideTask);
            xs.WriteBoolean(xe, nameof(processToisMultithreaded), processToisMultithreaded);
            xs.WriteNumber(xe, nameof(maxEntriesPerToiMidphaseCollideTask), maxEntriesPerToiMidphaseCollideTask);
            xs.WriteNumber(xe, nameof(maxEntriesPerToiNarrowphaseCollideTask), maxEntriesPerToiNarrowphaseCollideTask);
            xs.WriteNumber(xe, nameof(maxNumToiCollisionPairsSinglethreaded), maxNumToiCollisionPairsSinglethreaded);
            xs.WriteFloat(xe, nameof(numToisTillAllowedPenetrationSimplifiedToi), numToisTillAllowedPenetrationSimplifiedToi);
            xs.WriteFloat(xe, nameof(numToisTillAllowedPenetrationToi), numToisTillAllowedPenetrationToi);
            xs.WriteFloat(xe, nameof(numToisTillAllowedPenetrationToiHigher), numToisTillAllowedPenetrationToiHigher);
            xs.WriteFloat(xe, nameof(numToisTillAllowedPenetrationToiForced), numToisTillAllowedPenetrationToiForced);
            xs.WriteBoolean(xe, nameof(enableDeactivation), enableDeactivation);
            xs.WriteEnum<SimulationType, sbyte>(xe, nameof(simulationType), simulationType);
            xs.WriteBoolean(xe, nameof(enableSimulationIslands), enableSimulationIslands);
            xs.WriteNumber(xe, nameof(minDesiredIslandSize), minDesiredIslandSize);
            xs.WriteBoolean(xe, nameof(processActionsInSingleThread), processActionsInSingleThread);
            xs.WriteBoolean(xe, nameof(allowIntegrationOfIslandsWithoutConstraintsInASeparateJob), allowIntegrationOfIslandsWithoutConstraintsInASeparateJob);
            xs.WriteFloat(xe, nameof(frameMarkerPsiSnap), frameMarkerPsiSnap);
            xs.WriteBoolean(xe, nameof(fireCollisionCallbacks), fireCollisionCallbacks);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpWorldCinfo);
        }

        public bool Equals(hkpWorldCinfo? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   gravity.Equals(other.gravity) &&
                   broadPhaseQuerySize.Equals(other.broadPhaseQuerySize) &&
                   contactRestingVelocity.Equals(other.contactRestingVelocity) &&
                   broadPhaseBorderBehaviour.Equals(other.broadPhaseBorderBehaviour) &&
                   mtPostponeAndSortBroadPhaseBorderCallbacks.Equals(other.mtPostponeAndSortBroadPhaseBorderCallbacks) &&
                   ((broadPhaseWorldAabb is null && other.broadPhaseWorldAabb is null) || (broadPhaseWorldAabb is not null && other.broadPhaseWorldAabb is not null && broadPhaseWorldAabb.Equals((IHavokObject)other.broadPhaseWorldAabb))) &&
                   useKdTree.Equals(other.useKdTree) &&
                   useMultipleTree.Equals(other.useMultipleTree) &&
                   treeUpdateType.Equals(other.treeUpdateType) &&
                   autoUpdateKdTree.Equals(other.autoUpdateKdTree) &&
                   collisionTolerance.Equals(other.collisionTolerance) &&
                   ((collisionFilter is null && other.collisionFilter is null) || (collisionFilter is not null && other.collisionFilter is not null && collisionFilter.Equals((IHavokObject)other.collisionFilter))) &&
                   ((convexListFilter is null && other.convexListFilter is null) || (convexListFilter is not null && other.convexListFilter is not null && convexListFilter.Equals((IHavokObject)other.convexListFilter))) &&
                   expectedMaxLinearVelocity.Equals(other.expectedMaxLinearVelocity) &&
                   sizeOfToiEventQueue.Equals(other.sizeOfToiEventQueue) &&
                   expectedMinPsiDeltaTime.Equals(other.expectedMinPsiDeltaTime) &&
                   ((memoryWatchDog is null && other.memoryWatchDog is null) || (memoryWatchDog is not null && other.memoryWatchDog is not null && memoryWatchDog.Equals((IHavokObject)other.memoryWatchDog))) &&
                   broadPhaseNumMarkers.Equals(other.broadPhaseNumMarkers) &&
                   contactPointGeneration.Equals(other.contactPointGeneration) &&
                   allowToSkipConfirmedCallbacks.Equals(other.allowToSkipConfirmedCallbacks) &&
                   useHybridBroadphase.Equals(other.useHybridBroadphase) &&
                   solverTau.Equals(other.solverTau) &&
                   solverDamp.Equals(other.solverDamp) &&
                   solverIterations.Equals(other.solverIterations) &&
                   solverMicrosteps.Equals(other.solverMicrosteps) &&
                   maxConstraintViolation.Equals(other.maxConstraintViolation) &&
                   forceCoherentConstraintOrderingInSolver.Equals(other.forceCoherentConstraintOrderingInSolver) &&
                   snapCollisionToConvexEdgeThreshold.Equals(other.snapCollisionToConvexEdgeThreshold) &&
                   snapCollisionToConcaveEdgeThreshold.Equals(other.snapCollisionToConcaveEdgeThreshold) &&
                   enableToiWeldRejection.Equals(other.enableToiWeldRejection) &&
                   enableDeprecatedWelding.Equals(other.enableDeprecatedWelding) &&
                   iterativeLinearCastEarlyOutDistance.Equals(other.iterativeLinearCastEarlyOutDistance) &&
                   iterativeLinearCastMaxIterations.Equals(other.iterativeLinearCastMaxIterations) &&
                   deactivationNumInactiveFramesSelectFlag0.Equals(other.deactivationNumInactiveFramesSelectFlag0) &&
                   deactivationNumInactiveFramesSelectFlag1.Equals(other.deactivationNumInactiveFramesSelectFlag1) &&
                   deactivationIntegrateCounter.Equals(other.deactivationIntegrateCounter) &&
                   shouldActivateOnRigidBodyTransformChange.Equals(other.shouldActivateOnRigidBodyTransformChange) &&
                   deactivationReferenceDistance.Equals(other.deactivationReferenceDistance) &&
                   toiCollisionResponseRotateNormal.Equals(other.toiCollisionResponseRotateNormal) &&
                   maxSectorsPerMidphaseCollideTask.Equals(other.maxSectorsPerMidphaseCollideTask) &&
                   maxSectorsPerNarrowphaseCollideTask.Equals(other.maxSectorsPerNarrowphaseCollideTask) &&
                   processToisMultithreaded.Equals(other.processToisMultithreaded) &&
                   maxEntriesPerToiMidphaseCollideTask.Equals(other.maxEntriesPerToiMidphaseCollideTask) &&
                   maxEntriesPerToiNarrowphaseCollideTask.Equals(other.maxEntriesPerToiNarrowphaseCollideTask) &&
                   maxNumToiCollisionPairsSinglethreaded.Equals(other.maxNumToiCollisionPairsSinglethreaded) &&
                   numToisTillAllowedPenetrationSimplifiedToi.Equals(other.numToisTillAllowedPenetrationSimplifiedToi) &&
                   numToisTillAllowedPenetrationToi.Equals(other.numToisTillAllowedPenetrationToi) &&
                   numToisTillAllowedPenetrationToiHigher.Equals(other.numToisTillAllowedPenetrationToiHigher) &&
                   numToisTillAllowedPenetrationToiForced.Equals(other.numToisTillAllowedPenetrationToiForced) &&
                   enableDeactivation.Equals(other.enableDeactivation) &&
                   simulationType.Equals(other.simulationType) &&
                   enableSimulationIslands.Equals(other.enableSimulationIslands) &&
                   minDesiredIslandSize.Equals(other.minDesiredIslandSize) &&
                   processActionsInSingleThread.Equals(other.processActionsInSingleThread) &&
                   allowIntegrationOfIslandsWithoutConstraintsInASeparateJob.Equals(other.allowIntegrationOfIslandsWithoutConstraintsInASeparateJob) &&
                   frameMarkerPsiSnap.Equals(other.frameMarkerPsiSnap) &&
                   fireCollisionCallbacks.Equals(other.fireCollisionCallbacks) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(gravity);
            hashcode.Add(broadPhaseQuerySize);
            hashcode.Add(contactRestingVelocity);
            hashcode.Add(broadPhaseBorderBehaviour);
            hashcode.Add(mtPostponeAndSortBroadPhaseBorderCallbacks);
            hashcode.Add(broadPhaseWorldAabb);
            hashcode.Add(useKdTree);
            hashcode.Add(useMultipleTree);
            hashcode.Add(treeUpdateType);
            hashcode.Add(autoUpdateKdTree);
            hashcode.Add(collisionTolerance);
            hashcode.Add(collisionFilter);
            hashcode.Add(convexListFilter);
            hashcode.Add(expectedMaxLinearVelocity);
            hashcode.Add(sizeOfToiEventQueue);
            hashcode.Add(expectedMinPsiDeltaTime);
            hashcode.Add(memoryWatchDog);
            hashcode.Add(broadPhaseNumMarkers);
            hashcode.Add(contactPointGeneration);
            hashcode.Add(allowToSkipConfirmedCallbacks);
            hashcode.Add(useHybridBroadphase);
            hashcode.Add(solverTau);
            hashcode.Add(solverDamp);
            hashcode.Add(solverIterations);
            hashcode.Add(solverMicrosteps);
            hashcode.Add(maxConstraintViolation);
            hashcode.Add(forceCoherentConstraintOrderingInSolver);
            hashcode.Add(snapCollisionToConvexEdgeThreshold);
            hashcode.Add(snapCollisionToConcaveEdgeThreshold);
            hashcode.Add(enableToiWeldRejection);
            hashcode.Add(enableDeprecatedWelding);
            hashcode.Add(iterativeLinearCastEarlyOutDistance);
            hashcode.Add(iterativeLinearCastMaxIterations);
            hashcode.Add(deactivationNumInactiveFramesSelectFlag0);
            hashcode.Add(deactivationNumInactiveFramesSelectFlag1);
            hashcode.Add(deactivationIntegrateCounter);
            hashcode.Add(shouldActivateOnRigidBodyTransformChange);
            hashcode.Add(deactivationReferenceDistance);
            hashcode.Add(toiCollisionResponseRotateNormal);
            hashcode.Add(maxSectorsPerMidphaseCollideTask);
            hashcode.Add(maxSectorsPerNarrowphaseCollideTask);
            hashcode.Add(processToisMultithreaded);
            hashcode.Add(maxEntriesPerToiMidphaseCollideTask);
            hashcode.Add(maxEntriesPerToiNarrowphaseCollideTask);
            hashcode.Add(maxNumToiCollisionPairsSinglethreaded);
            hashcode.Add(numToisTillAllowedPenetrationSimplifiedToi);
            hashcode.Add(numToisTillAllowedPenetrationToi);
            hashcode.Add(numToisTillAllowedPenetrationToiHigher);
            hashcode.Add(numToisTillAllowedPenetrationToiForced);
            hashcode.Add(enableDeactivation);
            hashcode.Add(simulationType);
            hashcode.Add(enableSimulationIslands);
            hashcode.Add(minDesiredIslandSize);
            hashcode.Add(processActionsInSingleThread);
            hashcode.Add(allowIntegrationOfIslandsWithoutConstraintsInASeparateJob);
            hashcode.Add(frameMarkerPsiSnap);
            hashcode.Add(fireCollisionCallbacks);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

