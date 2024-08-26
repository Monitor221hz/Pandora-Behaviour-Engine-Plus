using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpWorld Signatire: 0xaadcec37 size: 1072 flags: FLAGS_NOT_SERIALIZABLE

    // simulation class: hkpSimulation Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // gravity class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // fixedIsland class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 48 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // fixedRigidBody class: hkpRigidBody Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 56 flags: FLAGS_NONE enum: 
    // activeSimulationIslands class:  Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 64 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // inactiveSimulationIslands class:  Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 80 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // dirtySimulationIslands class:  Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 96 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // maintenanceMgr class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 112 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // memoryWatchDog class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 120 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // assertOnRunningOutOfSolverMemory class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 128 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // broadPhase class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 136 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // kdTreeManager class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 144 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // autoUpdateTree class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 152 flags: FLAGS_NONE enum: 
    // broadPhaseDispatcher class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 160 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // phantomBroadPhaseListener class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 168 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // entityEntityBroadPhaseListener class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 176 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // broadPhaseBorderListener class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 184 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // multithreadedSimulationJobData class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 192 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // collisionInput class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 200 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // collisionFilter class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 208 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // collisionDispatcher class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 216 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // convexListFilter class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 224 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // pendingOperations class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 232 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // pendingOperationsCount class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 240 flags: FLAGS_NONE enum: 
    // pendingBodyOperationsCount class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 244 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // criticalOperationsLockCount class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 248 flags: FLAGS_NONE enum: 
    // criticalOperationsLockCountForPhantoms class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 252 flags: FLAGS_NONE enum: 
    // blockExecutingPendingOperations class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 256 flags: FLAGS_NONE enum: 
    // criticalOperationsAllowed class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 257 flags: FLAGS_NONE enum: 
    // pendingOperationQueues class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 264 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // pendingOperationQueueCount class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 272 flags: FLAGS_NONE enum: 
    // multiThreadCheck class: hkMultiThreadCheck Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 276 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // processActionsInSingleThread class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 288 flags: FLAGS_NONE enum: 
    // allowIntegrationOfIslandsWithoutConstraintsInASeparateJob class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 289 flags: FLAGS_NONE enum: 
    // minDesiredIslandSize class:  Type.TYPE_UINT32 Type.TYPE_VOID arrSize: 0 offset: 292 flags: FLAGS_NONE enum: 
    // modifyConstraintCriticalSection class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 296 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // isLocked class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 304 flags: FLAGS_NONE enum: 
    // islandDirtyListCriticalSection class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 312 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // propertyMasterLock class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 320 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // wantSimulationIslands class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 328 flags: FLAGS_NONE enum: 
    // useHybridBroadphase class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 329 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // snapCollisionToConvexEdgeThreshold class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 332 flags: FLAGS_NONE enum: 
    // snapCollisionToConcaveEdgeThreshold class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 336 flags: FLAGS_NONE enum: 
    // enableToiWeldRejection class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 340 flags: FLAGS_NONE enum: 
    // wantDeactivation class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 341 flags: FLAGS_NONE enum: 
    // shouldActivateOnRigidBodyTransformChange class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 342 flags: FLAGS_NONE enum: 
    // deactivationReferenceDistance class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 344 flags: FLAGS_NONE enum: 
    // toiCollisionResponseRotateNormal class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 348 flags: FLAGS_NONE enum: 
    // maxSectorsPerMidphaseCollideTask class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 352 flags: FLAGS_NONE enum: 
    // maxSectorsPerNarrowphaseCollideTask class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 356 flags: FLAGS_NONE enum: 
    // processToisMultithreaded class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 360 flags: FLAGS_NONE enum: 
    // maxEntriesPerToiMidphaseCollideTask class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 364 flags: FLAGS_NONE enum: 
    // maxEntriesPerToiNarrowphaseCollideTask class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 368 flags: FLAGS_NONE enum: 
    // maxNumToiCollisionPairsSinglethreaded class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 372 flags: FLAGS_NONE enum: 
    // simulationType class:  Type.TYPE_ENUM Type.TYPE_INT32 arrSize: 0 offset: 376 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // numToisTillAllowedPenetrationSimplifiedToi class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 380 flags: FLAGS_NONE enum: 
    // numToisTillAllowedPenetrationToi class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 384 flags: FLAGS_NONE enum: 
    // numToisTillAllowedPenetrationToiHigher class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 388 flags: FLAGS_NONE enum: 
    // numToisTillAllowedPenetrationToiForced class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 392 flags: FLAGS_NONE enum: 
    // lastEntityUid class:  Type.TYPE_UINT32 Type.TYPE_VOID arrSize: 0 offset: 396 flags: FLAGS_NONE enum: 
    // lastIslandUid class:  Type.TYPE_UINT32 Type.TYPE_VOID arrSize: 0 offset: 400 flags: FLAGS_NONE enum: 
    // lastConstraintUid class:  Type.TYPE_UINT32 Type.TYPE_VOID arrSize: 0 offset: 404 flags: FLAGS_NONE enum: 
    // phantoms class: hkpPhantom Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 408 flags: FLAGS_NONE enum: 
    // actionListeners class:  Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 424 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // entityListeners class:  Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 440 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // phantomListeners class:  Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 456 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // constraintListeners class:  Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 472 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // worldDeletionListeners class:  Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 488 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // islandActivationListeners class:  Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 504 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // worldPostSimulationListeners class:  Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 520 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // worldPostIntegrateListeners class:  Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 536 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // worldPostCollideListeners class:  Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 552 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // islandPostIntegrateListeners class:  Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 568 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // islandPostCollideListeners class:  Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 584 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // contactListeners class:  Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 600 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // contactImpulseLimitBreachedListeners class:  Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 616 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // worldExtensions class:  Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 632 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // violatedConstraintArray class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 648 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // broadPhaseBorder class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 656 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // destructionWorld class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 664 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // npWorld class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 672 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // broadPhaseExtents class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 2 offset: 1008 flags: FLAGS_NONE enum: 
    // broadPhaseNumMarkers class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 1040 flags: FLAGS_NONE enum: 
    // sizeOfToiEventQueue class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 1044 flags: FLAGS_NONE enum: 
    // broadPhaseQuerySize class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 1048 flags: FLAGS_NONE enum: 
    // broadPhaseUpdateSize class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 1052 flags: FLAGS_NONE enum: 
    // contactPointGeneration class:  Type.TYPE_ENUM Type.TYPE_INT8 arrSize: 0 offset: 1056 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    public partial class hkpWorld : hkReferencedObject, IEquatable<hkpWorld?>
    {
        public hkpSimulation? simulation { set; get; }
        public Vector4 gravity { set; get; }
        private object? fixedIsland { set; get; }
        public hkpRigidBody? fixedRigidBody { set; get; }
        public IList<object> activeSimulationIslands { set; get; } = Array.Empty<object>();
        public IList<object> inactiveSimulationIslands { set; get; } = Array.Empty<object>();
        public IList<object> dirtySimulationIslands { set; get; } = Array.Empty<object>();
        private object? maintenanceMgr { set; get; }
        private object? memoryWatchDog { set; get; }
        private bool assertOnRunningOutOfSolverMemory { set; get; }
        private object? broadPhase { set; get; }
        private object? kdTreeManager { set; get; }
        public bool autoUpdateTree { set; get; }
        private object? broadPhaseDispatcher { set; get; }
        private object? phantomBroadPhaseListener { set; get; }
        private object? entityEntityBroadPhaseListener { set; get; }
        private object? broadPhaseBorderListener { set; get; }
        private object? multithreadedSimulationJobData { set; get; }
        private object? collisionInput { set; get; }
        private object? collisionFilter { set; get; }
        private object? collisionDispatcher { set; get; }
        private object? convexListFilter { set; get; }
        private object? pendingOperations { set; get; }
        public int pendingOperationsCount { set; get; }
        private int pendingBodyOperationsCount { set; get; }
        public int criticalOperationsLockCount { set; get; }
        public int criticalOperationsLockCountForPhantoms { set; get; }
        public bool blockExecutingPendingOperations { set; get; }
        public bool criticalOperationsAllowed { set; get; }
        private object? pendingOperationQueues { set; get; }
        public int pendingOperationQueueCount { set; get; }
        public hkMultiThreadCheck multiThreadCheck { set; get; } = new();
        public bool processActionsInSingleThread { set; get; }
        public bool allowIntegrationOfIslandsWithoutConstraintsInASeparateJob { set; get; }
        public uint minDesiredIslandSize { set; get; }
        private object? modifyConstraintCriticalSection { set; get; }
        public int isLocked { set; get; }
        private object? islandDirtyListCriticalSection { set; get; }
        private object? propertyMasterLock { set; get; }
        public bool wantSimulationIslands { set; get; }
        private bool useHybridBroadphase { set; get; }
        public float snapCollisionToConvexEdgeThreshold { set; get; }
        public float snapCollisionToConcaveEdgeThreshold { set; get; }
        public bool enableToiWeldRejection { set; get; }
        public bool wantDeactivation { set; get; }
        public bool shouldActivateOnRigidBodyTransformChange { set; get; }
        public float deactivationReferenceDistance { set; get; }
        public float toiCollisionResponseRotateNormal { set; get; }
        public int maxSectorsPerMidphaseCollideTask { set; get; }
        public int maxSectorsPerNarrowphaseCollideTask { set; get; }
        public bool processToisMultithreaded { set; get; }
        public int maxEntriesPerToiMidphaseCollideTask { set; get; }
        public int maxEntriesPerToiNarrowphaseCollideTask { set; get; }
        public int maxNumToiCollisionPairsSinglethreaded { set; get; }
        private int simulationType { set; get; }
        public float numToisTillAllowedPenetrationSimplifiedToi { set; get; }
        public float numToisTillAllowedPenetrationToi { set; get; }
        public float numToisTillAllowedPenetrationToiHigher { set; get; }
        public float numToisTillAllowedPenetrationToiForced { set; get; }
        public uint lastEntityUid { set; get; }
        public uint lastIslandUid { set; get; }
        public uint lastConstraintUid { set; get; }
        public IList<hkpPhantom> phantoms { set; get; } = Array.Empty<hkpPhantom>();
        public IList<object> actionListeners { set; get; } = Array.Empty<object>();
        public IList<object> entityListeners { set; get; } = Array.Empty<object>();
        public IList<object> phantomListeners { set; get; } = Array.Empty<object>();
        public IList<object> constraintListeners { set; get; } = Array.Empty<object>();
        public IList<object> worldDeletionListeners { set; get; } = Array.Empty<object>();
        public IList<object> islandActivationListeners { set; get; } = Array.Empty<object>();
        public IList<object> worldPostSimulationListeners { set; get; } = Array.Empty<object>();
        public IList<object> worldPostIntegrateListeners { set; get; } = Array.Empty<object>();
        public IList<object> worldPostCollideListeners { set; get; } = Array.Empty<object>();
        public IList<object> islandPostIntegrateListeners { set; get; } = Array.Empty<object>();
        public IList<object> islandPostCollideListeners { set; get; } = Array.Empty<object>();
        public IList<object> contactListeners { set; get; } = Array.Empty<object>();
        public IList<object> contactImpulseLimitBreachedListeners { set; get; } = Array.Empty<object>();
        public IList<object> worldExtensions { set; get; } = Array.Empty<object>();
        private object? violatedConstraintArray { set; get; }
        private object? broadPhaseBorder { set; get; }
        private object? destructionWorld { set; get; }
        private object? npWorld { set; get; }
        public Vector4[] broadPhaseExtents = new Vector4[2];
        public int broadPhaseNumMarkers { set; get; }
        public int sizeOfToiEventQueue { set; get; }
        public int broadPhaseQuerySize { set; get; }
        public int broadPhaseUpdateSize { set; get; }
        private sbyte contactPointGeneration { set; get; }

        public override uint Signature { set; get; } = 0xaadcec37;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            simulation = des.ReadClassPointer<hkpSimulation>(br);
            br.Position += 8;
            gravity = br.ReadVector4();
            des.ReadEmptyPointer(br);
            fixedRigidBody = des.ReadClassPointer<hkpRigidBody>(br);
            des.ReadEmptyArray(br);
            des.ReadEmptyArray(br);
            des.ReadEmptyArray(br);
            des.ReadEmptyPointer(br);
            des.ReadEmptyPointer(br);
            assertOnRunningOutOfSolverMemory = br.ReadBoolean();
            br.Position += 7;
            des.ReadEmptyPointer(br);
            des.ReadEmptyPointer(br);
            autoUpdateTree = br.ReadBoolean();
            br.Position += 7;
            des.ReadEmptyPointer(br);
            des.ReadEmptyPointer(br);
            des.ReadEmptyPointer(br);
            des.ReadEmptyPointer(br);
            des.ReadEmptyPointer(br);
            des.ReadEmptyPointer(br);
            des.ReadEmptyPointer(br);
            des.ReadEmptyPointer(br);
            des.ReadEmptyPointer(br);
            des.ReadEmptyPointer(br);
            pendingOperationsCount = br.ReadInt32();
            pendingBodyOperationsCount = br.ReadInt32();
            criticalOperationsLockCount = br.ReadInt32();
            criticalOperationsLockCountForPhantoms = br.ReadInt32();
            blockExecutingPendingOperations = br.ReadBoolean();
            criticalOperationsAllowed = br.ReadBoolean();
            br.Position += 6;
            des.ReadEmptyPointer(br);
            pendingOperationQueueCount = br.ReadInt32();
            multiThreadCheck.Read(des, br);
            processActionsInSingleThread = br.ReadBoolean();
            allowIntegrationOfIslandsWithoutConstraintsInASeparateJob = br.ReadBoolean();
            br.Position += 2;
            minDesiredIslandSize = br.ReadUInt32();
            des.ReadEmptyPointer(br);
            isLocked = br.ReadInt32();
            br.Position += 4;
            des.ReadEmptyPointer(br);
            des.ReadEmptyPointer(br);
            wantSimulationIslands = br.ReadBoolean();
            useHybridBroadphase = br.ReadBoolean();
            br.Position += 2;
            snapCollisionToConvexEdgeThreshold = br.ReadSingle();
            snapCollisionToConcaveEdgeThreshold = br.ReadSingle();
            enableToiWeldRejection = br.ReadBoolean();
            wantDeactivation = br.ReadBoolean();
            shouldActivateOnRigidBodyTransformChange = br.ReadBoolean();
            br.Position += 1;
            deactivationReferenceDistance = br.ReadSingle();
            toiCollisionResponseRotateNormal = br.ReadSingle();
            maxSectorsPerMidphaseCollideTask = br.ReadInt32();
            maxSectorsPerNarrowphaseCollideTask = br.ReadInt32();
            processToisMultithreaded = br.ReadBoolean();
            br.Position += 3;
            maxEntriesPerToiMidphaseCollideTask = br.ReadInt32();
            maxEntriesPerToiNarrowphaseCollideTask = br.ReadInt32();
            maxNumToiCollisionPairsSinglethreaded = br.ReadInt32();
            simulationType = br.ReadInt32();
            numToisTillAllowedPenetrationSimplifiedToi = br.ReadSingle();
            numToisTillAllowedPenetrationToi = br.ReadSingle();
            numToisTillAllowedPenetrationToiHigher = br.ReadSingle();
            numToisTillAllowedPenetrationToiForced = br.ReadSingle();
            lastEntityUid = br.ReadUInt32();
            lastIslandUid = br.ReadUInt32();
            lastConstraintUid = br.ReadUInt32();
            phantoms = des.ReadClassPointerArray<hkpPhantom>(br);
            des.ReadEmptyArray(br);
            des.ReadEmptyArray(br);
            des.ReadEmptyArray(br);
            des.ReadEmptyArray(br);
            des.ReadEmptyArray(br);
            des.ReadEmptyArray(br);
            des.ReadEmptyArray(br);
            des.ReadEmptyArray(br);
            des.ReadEmptyArray(br);
            des.ReadEmptyArray(br);
            des.ReadEmptyArray(br);
            des.ReadEmptyArray(br);
            des.ReadEmptyArray(br);
            des.ReadEmptyArray(br);
            des.ReadEmptyPointer(br);
            des.ReadEmptyPointer(br);
            des.ReadEmptyPointer(br);
            des.ReadEmptyPointer(br);
            br.Position += 328;
            broadPhaseExtents = des.ReadVector4CStyleArray(br, 2);
            broadPhaseNumMarkers = br.ReadInt32();
            sizeOfToiEventQueue = br.ReadInt32();
            broadPhaseQuerySize = br.ReadInt32();
            broadPhaseUpdateSize = br.ReadInt32();
            contactPointGeneration = br.ReadSByte();
            br.Position += 15;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteClassPointer(bw, simulation);
            bw.Position += 8;
            bw.WriteVector4(gravity);
            s.WriteVoidPointer(bw);
            s.WriteClassPointer(bw, fixedRigidBody);
            s.WriteVoidArray(bw);
            s.WriteVoidArray(bw);
            s.WriteVoidArray(bw);
            s.WriteVoidPointer(bw);
            s.WriteVoidPointer(bw);
            bw.WriteBoolean(assertOnRunningOutOfSolverMemory);
            bw.Position += 7;
            s.WriteVoidPointer(bw);
            s.WriteVoidPointer(bw);
            bw.WriteBoolean(autoUpdateTree);
            bw.Position += 7;
            s.WriteVoidPointer(bw);
            s.WriteVoidPointer(bw);
            s.WriteVoidPointer(bw);
            s.WriteVoidPointer(bw);
            s.WriteVoidPointer(bw);
            s.WriteVoidPointer(bw);
            s.WriteVoidPointer(bw);
            s.WriteVoidPointer(bw);
            s.WriteVoidPointer(bw);
            s.WriteVoidPointer(bw);
            bw.WriteInt32(pendingOperationsCount);
            bw.WriteInt32(pendingBodyOperationsCount);
            bw.WriteInt32(criticalOperationsLockCount);
            bw.WriteInt32(criticalOperationsLockCountForPhantoms);
            bw.WriteBoolean(blockExecutingPendingOperations);
            bw.WriteBoolean(criticalOperationsAllowed);
            bw.Position += 6;
            s.WriteVoidPointer(bw);
            bw.WriteInt32(pendingOperationQueueCount);
            multiThreadCheck.Write(s, bw);
            bw.WriteBoolean(processActionsInSingleThread);
            bw.WriteBoolean(allowIntegrationOfIslandsWithoutConstraintsInASeparateJob);
            bw.Position += 2;
            bw.WriteUInt32(minDesiredIslandSize);
            s.WriteVoidPointer(bw);
            bw.WriteInt32(isLocked);
            bw.Position += 4;
            s.WriteVoidPointer(bw);
            s.WriteVoidPointer(bw);
            bw.WriteBoolean(wantSimulationIslands);
            bw.WriteBoolean(useHybridBroadphase);
            bw.Position += 2;
            bw.WriteSingle(snapCollisionToConvexEdgeThreshold);
            bw.WriteSingle(snapCollisionToConcaveEdgeThreshold);
            bw.WriteBoolean(enableToiWeldRejection);
            bw.WriteBoolean(wantDeactivation);
            bw.WriteBoolean(shouldActivateOnRigidBodyTransformChange);
            bw.Position += 1;
            bw.WriteSingle(deactivationReferenceDistance);
            bw.WriteSingle(toiCollisionResponseRotateNormal);
            bw.WriteInt32(maxSectorsPerMidphaseCollideTask);
            bw.WriteInt32(maxSectorsPerNarrowphaseCollideTask);
            bw.WriteBoolean(processToisMultithreaded);
            bw.Position += 3;
            bw.WriteInt32(maxEntriesPerToiMidphaseCollideTask);
            bw.WriteInt32(maxEntriesPerToiNarrowphaseCollideTask);
            bw.WriteInt32(maxNumToiCollisionPairsSinglethreaded);
            bw.WriteInt32(simulationType);
            bw.WriteSingle(numToisTillAllowedPenetrationSimplifiedToi);
            bw.WriteSingle(numToisTillAllowedPenetrationToi);
            bw.WriteSingle(numToisTillAllowedPenetrationToiHigher);
            bw.WriteSingle(numToisTillAllowedPenetrationToiForced);
            bw.WriteUInt32(lastEntityUid);
            bw.WriteUInt32(lastIslandUid);
            bw.WriteUInt32(lastConstraintUid);
            s.WriteClassPointerArray(bw, phantoms);
            s.WriteVoidArray(bw);
            s.WriteVoidArray(bw);
            s.WriteVoidArray(bw);
            s.WriteVoidArray(bw);
            s.WriteVoidArray(bw);
            s.WriteVoidArray(bw);
            s.WriteVoidArray(bw);
            s.WriteVoidArray(bw);
            s.WriteVoidArray(bw);
            s.WriteVoidArray(bw);
            s.WriteVoidArray(bw);
            s.WriteVoidArray(bw);
            s.WriteVoidArray(bw);
            s.WriteVoidArray(bw);
            s.WriteVoidPointer(bw);
            s.WriteVoidPointer(bw);
            s.WriteVoidPointer(bw);
            s.WriteVoidPointer(bw);
            bw.Position += 328;
            s.WriteVector4CStyleArray(bw, broadPhaseExtents);
            bw.WriteInt32(broadPhaseNumMarkers);
            bw.WriteInt32(sizeOfToiEventQueue);
            bw.WriteInt32(broadPhaseQuerySize);
            bw.WriteInt32(broadPhaseUpdateSize);
            bw.WriteSByte(contactPointGeneration);
            bw.Position += 15;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            simulation = xd.ReadClassPointer<hkpSimulation>(this, xe, nameof(simulation));
            gravity = xd.ReadVector4(xe, nameof(gravity));
            fixedRigidBody = xd.ReadClassPointer<hkpRigidBody>(this, xe, nameof(fixedRigidBody));
            autoUpdateTree = xd.ReadBoolean(xe, nameof(autoUpdateTree));
            pendingOperationsCount = xd.ReadInt32(xe, nameof(pendingOperationsCount));
            criticalOperationsLockCount = xd.ReadInt32(xe, nameof(criticalOperationsLockCount));
            criticalOperationsLockCountForPhantoms = xd.ReadInt32(xe, nameof(criticalOperationsLockCountForPhantoms));
            blockExecutingPendingOperations = xd.ReadBoolean(xe, nameof(blockExecutingPendingOperations));
            criticalOperationsAllowed = xd.ReadBoolean(xe, nameof(criticalOperationsAllowed));
            pendingOperationQueueCount = xd.ReadInt32(xe, nameof(pendingOperationQueueCount));
            processActionsInSingleThread = xd.ReadBoolean(xe, nameof(processActionsInSingleThread));
            allowIntegrationOfIslandsWithoutConstraintsInASeparateJob = xd.ReadBoolean(xe, nameof(allowIntegrationOfIslandsWithoutConstraintsInASeparateJob));
            minDesiredIslandSize = xd.ReadUInt32(xe, nameof(minDesiredIslandSize));
            isLocked = xd.ReadInt32(xe, nameof(isLocked));
            wantSimulationIslands = xd.ReadBoolean(xe, nameof(wantSimulationIslands));
            snapCollisionToConvexEdgeThreshold = xd.ReadSingle(xe, nameof(snapCollisionToConvexEdgeThreshold));
            snapCollisionToConcaveEdgeThreshold = xd.ReadSingle(xe, nameof(snapCollisionToConcaveEdgeThreshold));
            enableToiWeldRejection = xd.ReadBoolean(xe, nameof(enableToiWeldRejection));
            wantDeactivation = xd.ReadBoolean(xe, nameof(wantDeactivation));
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
            lastEntityUid = xd.ReadUInt32(xe, nameof(lastEntityUid));
            lastIslandUid = xd.ReadUInt32(xe, nameof(lastIslandUid));
            lastConstraintUid = xd.ReadUInt32(xe, nameof(lastConstraintUid));
            phantoms = xd.ReadClassPointerArray<hkpPhantom>(this, xe, nameof(phantoms));
            broadPhaseExtents = xd.ReadVector4CStyleArray(xe, nameof(broadPhaseExtents), 2);
            broadPhaseNumMarkers = xd.ReadInt32(xe, nameof(broadPhaseNumMarkers));
            sizeOfToiEventQueue = xd.ReadInt32(xe, nameof(sizeOfToiEventQueue));
            broadPhaseQuerySize = xd.ReadInt32(xe, nameof(broadPhaseQuerySize));
            broadPhaseUpdateSize = xd.ReadInt32(xe, nameof(broadPhaseUpdateSize));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClassPointer(xe, nameof(simulation), simulation);
            xs.WriteVector4(xe, nameof(gravity), gravity);
            xs.WriteSerializeIgnored(xe, nameof(fixedIsland));
            xs.WriteClassPointer(xe, nameof(fixedRigidBody), fixedRigidBody);
            xs.WriteSerializeIgnored(xe, nameof(activeSimulationIslands));
            xs.WriteSerializeIgnored(xe, nameof(inactiveSimulationIslands));
            xs.WriteSerializeIgnored(xe, nameof(dirtySimulationIslands));
            xs.WriteSerializeIgnored(xe, nameof(maintenanceMgr));
            xs.WriteSerializeIgnored(xe, nameof(memoryWatchDog));
            xs.WriteSerializeIgnored(xe, nameof(assertOnRunningOutOfSolverMemory));
            xs.WriteSerializeIgnored(xe, nameof(broadPhase));
            xs.WriteSerializeIgnored(xe, nameof(kdTreeManager));
            xs.WriteBoolean(xe, nameof(autoUpdateTree), autoUpdateTree);
            xs.WriteSerializeIgnored(xe, nameof(broadPhaseDispatcher));
            xs.WriteSerializeIgnored(xe, nameof(phantomBroadPhaseListener));
            xs.WriteSerializeIgnored(xe, nameof(entityEntityBroadPhaseListener));
            xs.WriteSerializeIgnored(xe, nameof(broadPhaseBorderListener));
            xs.WriteSerializeIgnored(xe, nameof(multithreadedSimulationJobData));
            xs.WriteSerializeIgnored(xe, nameof(collisionInput));
            xs.WriteSerializeIgnored(xe, nameof(collisionFilter));
            xs.WriteSerializeIgnored(xe, nameof(collisionDispatcher));
            xs.WriteSerializeIgnored(xe, nameof(convexListFilter));
            xs.WriteSerializeIgnored(xe, nameof(pendingOperations));
            xs.WriteNumber(xe, nameof(pendingOperationsCount), pendingOperationsCount);
            xs.WriteSerializeIgnored(xe, nameof(pendingBodyOperationsCount));
            xs.WriteNumber(xe, nameof(criticalOperationsLockCount), criticalOperationsLockCount);
            xs.WriteNumber(xe, nameof(criticalOperationsLockCountForPhantoms), criticalOperationsLockCountForPhantoms);
            xs.WriteBoolean(xe, nameof(blockExecutingPendingOperations), blockExecutingPendingOperations);
            xs.WriteBoolean(xe, nameof(criticalOperationsAllowed), criticalOperationsAllowed);
            xs.WriteSerializeIgnored(xe, nameof(pendingOperationQueues));
            xs.WriteNumber(xe, nameof(pendingOperationQueueCount), pendingOperationQueueCount);
            xs.WriteSerializeIgnored(xe, nameof(multiThreadCheck));
            xs.WriteBoolean(xe, nameof(processActionsInSingleThread), processActionsInSingleThread);
            xs.WriteBoolean(xe, nameof(allowIntegrationOfIslandsWithoutConstraintsInASeparateJob), allowIntegrationOfIslandsWithoutConstraintsInASeparateJob);
            xs.WriteNumber(xe, nameof(minDesiredIslandSize), minDesiredIslandSize);
            xs.WriteSerializeIgnored(xe, nameof(modifyConstraintCriticalSection));
            xs.WriteNumber(xe, nameof(isLocked), isLocked);
            xs.WriteSerializeIgnored(xe, nameof(islandDirtyListCriticalSection));
            xs.WriteSerializeIgnored(xe, nameof(propertyMasterLock));
            xs.WriteBoolean(xe, nameof(wantSimulationIslands), wantSimulationIslands);
            xs.WriteSerializeIgnored(xe, nameof(useHybridBroadphase));
            xs.WriteFloat(xe, nameof(snapCollisionToConvexEdgeThreshold), snapCollisionToConvexEdgeThreshold);
            xs.WriteFloat(xe, nameof(snapCollisionToConcaveEdgeThreshold), snapCollisionToConcaveEdgeThreshold);
            xs.WriteBoolean(xe, nameof(enableToiWeldRejection), enableToiWeldRejection);
            xs.WriteBoolean(xe, nameof(wantDeactivation), wantDeactivation);
            xs.WriteBoolean(xe, nameof(shouldActivateOnRigidBodyTransformChange), shouldActivateOnRigidBodyTransformChange);
            xs.WriteFloat(xe, nameof(deactivationReferenceDistance), deactivationReferenceDistance);
            xs.WriteFloat(xe, nameof(toiCollisionResponseRotateNormal), toiCollisionResponseRotateNormal);
            xs.WriteNumber(xe, nameof(maxSectorsPerMidphaseCollideTask), maxSectorsPerMidphaseCollideTask);
            xs.WriteNumber(xe, nameof(maxSectorsPerNarrowphaseCollideTask), maxSectorsPerNarrowphaseCollideTask);
            xs.WriteBoolean(xe, nameof(processToisMultithreaded), processToisMultithreaded);
            xs.WriteNumber(xe, nameof(maxEntriesPerToiMidphaseCollideTask), maxEntriesPerToiMidphaseCollideTask);
            xs.WriteNumber(xe, nameof(maxEntriesPerToiNarrowphaseCollideTask), maxEntriesPerToiNarrowphaseCollideTask);
            xs.WriteNumber(xe, nameof(maxNumToiCollisionPairsSinglethreaded), maxNumToiCollisionPairsSinglethreaded);
            xs.WriteSerializeIgnored(xe, nameof(simulationType));
            xs.WriteFloat(xe, nameof(numToisTillAllowedPenetrationSimplifiedToi), numToisTillAllowedPenetrationSimplifiedToi);
            xs.WriteFloat(xe, nameof(numToisTillAllowedPenetrationToi), numToisTillAllowedPenetrationToi);
            xs.WriteFloat(xe, nameof(numToisTillAllowedPenetrationToiHigher), numToisTillAllowedPenetrationToiHigher);
            xs.WriteFloat(xe, nameof(numToisTillAllowedPenetrationToiForced), numToisTillAllowedPenetrationToiForced);
            xs.WriteNumber(xe, nameof(lastEntityUid), lastEntityUid);
            xs.WriteNumber(xe, nameof(lastIslandUid), lastIslandUid);
            xs.WriteNumber(xe, nameof(lastConstraintUid), lastConstraintUid);
            xs.WriteClassPointerArray(xe, nameof(phantoms), phantoms!);
            xs.WriteSerializeIgnored(xe, nameof(actionListeners));
            xs.WriteSerializeIgnored(xe, nameof(entityListeners));
            xs.WriteSerializeIgnored(xe, nameof(phantomListeners));
            xs.WriteSerializeIgnored(xe, nameof(constraintListeners));
            xs.WriteSerializeIgnored(xe, nameof(worldDeletionListeners));
            xs.WriteSerializeIgnored(xe, nameof(islandActivationListeners));
            xs.WriteSerializeIgnored(xe, nameof(worldPostSimulationListeners));
            xs.WriteSerializeIgnored(xe, nameof(worldPostIntegrateListeners));
            xs.WriteSerializeIgnored(xe, nameof(worldPostCollideListeners));
            xs.WriteSerializeIgnored(xe, nameof(islandPostIntegrateListeners));
            xs.WriteSerializeIgnored(xe, nameof(islandPostCollideListeners));
            xs.WriteSerializeIgnored(xe, nameof(contactListeners));
            xs.WriteSerializeIgnored(xe, nameof(contactImpulseLimitBreachedListeners));
            xs.WriteSerializeIgnored(xe, nameof(worldExtensions));
            xs.WriteSerializeIgnored(xe, nameof(violatedConstraintArray));
            xs.WriteSerializeIgnored(xe, nameof(broadPhaseBorder));
            xs.WriteSerializeIgnored(xe, nameof(destructionWorld));
            xs.WriteSerializeIgnored(xe, nameof(npWorld));
            xs.WriteVector4Array(xe, nameof(broadPhaseExtents), broadPhaseExtents);
            xs.WriteNumber(xe, nameof(broadPhaseNumMarkers), broadPhaseNumMarkers);
            xs.WriteNumber(xe, nameof(sizeOfToiEventQueue), sizeOfToiEventQueue);
            xs.WriteNumber(xe, nameof(broadPhaseQuerySize), broadPhaseQuerySize);
            xs.WriteNumber(xe, nameof(broadPhaseUpdateSize), broadPhaseUpdateSize);
            xs.WriteSerializeIgnored(xe, nameof(contactPointGeneration));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpWorld);
        }

        public bool Equals(hkpWorld? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   ((simulation is null && other.simulation is null) || (simulation is not null && other.simulation is not null && simulation.Equals((IHavokObject)other.simulation))) &&
                   gravity.Equals(other.gravity) &&
                   ((fixedRigidBody is null && other.fixedRigidBody is null) || (fixedRigidBody is not null && other.fixedRigidBody is not null && fixedRigidBody.Equals((IHavokObject)other.fixedRigidBody))) &&
                   autoUpdateTree.Equals(other.autoUpdateTree) &&
                   pendingOperationsCount.Equals(other.pendingOperationsCount) &&
                   criticalOperationsLockCount.Equals(other.criticalOperationsLockCount) &&
                   criticalOperationsLockCountForPhantoms.Equals(other.criticalOperationsLockCountForPhantoms) &&
                   blockExecutingPendingOperations.Equals(other.blockExecutingPendingOperations) &&
                   criticalOperationsAllowed.Equals(other.criticalOperationsAllowed) &&
                   pendingOperationQueueCount.Equals(other.pendingOperationQueueCount) &&
                   processActionsInSingleThread.Equals(other.processActionsInSingleThread) &&
                   allowIntegrationOfIslandsWithoutConstraintsInASeparateJob.Equals(other.allowIntegrationOfIslandsWithoutConstraintsInASeparateJob) &&
                   minDesiredIslandSize.Equals(other.minDesiredIslandSize) &&
                   isLocked.Equals(other.isLocked) &&
                   wantSimulationIslands.Equals(other.wantSimulationIslands) &&
                   snapCollisionToConvexEdgeThreshold.Equals(other.snapCollisionToConvexEdgeThreshold) &&
                   snapCollisionToConcaveEdgeThreshold.Equals(other.snapCollisionToConcaveEdgeThreshold) &&
                   enableToiWeldRejection.Equals(other.enableToiWeldRejection) &&
                   wantDeactivation.Equals(other.wantDeactivation) &&
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
                   lastEntityUid.Equals(other.lastEntityUid) &&
                   lastIslandUid.Equals(other.lastIslandUid) &&
                   lastConstraintUid.Equals(other.lastConstraintUid) &&
                   phantoms.SequenceEqual(other.phantoms) &&
                   broadPhaseExtents.SequenceEqual(other.broadPhaseExtents) &&
                   broadPhaseNumMarkers.Equals(other.broadPhaseNumMarkers) &&
                   sizeOfToiEventQueue.Equals(other.sizeOfToiEventQueue) &&
                   broadPhaseQuerySize.Equals(other.broadPhaseQuerySize) &&
                   broadPhaseUpdateSize.Equals(other.broadPhaseUpdateSize) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(simulation);
            hashcode.Add(gravity);
            hashcode.Add(fixedRigidBody);
            hashcode.Add(autoUpdateTree);
            hashcode.Add(pendingOperationsCount);
            hashcode.Add(criticalOperationsLockCount);
            hashcode.Add(criticalOperationsLockCountForPhantoms);
            hashcode.Add(blockExecutingPendingOperations);
            hashcode.Add(criticalOperationsAllowed);
            hashcode.Add(pendingOperationQueueCount);
            hashcode.Add(processActionsInSingleThread);
            hashcode.Add(allowIntegrationOfIslandsWithoutConstraintsInASeparateJob);
            hashcode.Add(minDesiredIslandSize);
            hashcode.Add(isLocked);
            hashcode.Add(wantSimulationIslands);
            hashcode.Add(snapCollisionToConvexEdgeThreshold);
            hashcode.Add(snapCollisionToConcaveEdgeThreshold);
            hashcode.Add(enableToiWeldRejection);
            hashcode.Add(wantDeactivation);
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
            hashcode.Add(lastEntityUid);
            hashcode.Add(lastIslandUid);
            hashcode.Add(lastConstraintUid);
            hashcode.Add(phantoms.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(broadPhaseExtents.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(broadPhaseNumMarkers);
            hashcode.Add(sizeOfToiEventQueue);
            hashcode.Add(broadPhaseQuerySize);
            hashcode.Add(broadPhaseUpdateSize);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

