using System;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

namespace PhysicsRT
{
  #region 函数定义

  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate IntPtr fnTestAssert();
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate IntPtr fnCreateVec3(float x, float y, float z);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate IntPtr fnCreateVec4(float x, float y, float z, float w);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate void fnCommonDelete(IntPtr ptr);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate IntPtr fnCreateTransform(float px, float py, float pz, float rx, float ry, float rz, float rw, float sx, float sy, float sz);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate void fnDestroyVec4(IntPtr ptr);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate void fnDestroyVec3(IntPtr ptr);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate void fnDestroyTransform(IntPtr ptr);

  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate IntPtr fnCreateRigidBody(
      IntPtr world,
      IntPtr shape, IntPtr position, IntPtr rot,
      int motionType, int qualityType, float friction, float restitution, float mass, int active, int layer, int isTiggerVolume, int addContactListener,
      float gravityFactor, float linearDamping, float angularDamping, IntPtr centerOfMass, IntPtr inertiaTensor,
      IntPtr linearVelocity, IntPtr angularVelocity, float maxLinearVelocity, float maxAngularVelocity, IntPtr massProperties);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate void fnActiveRigidBody(IntPtr body);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate void fnDeactiveRigidBody(IntPtr body);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate void fnSetRigidBodyMass(IntPtr body, float mass);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate void fnSetRigidBodyFriction(IntPtr body, float friction);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate void fnSetRigidBodyRestitution(IntPtr body, float restitution);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate void fnSetRigidBodyAngularDamping(IntPtr body, float angularDamping);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate void fnSetRigidBodyLinearDampin(IntPtr body, float linearDamping);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate void fnSetRigidBodyCenterOfMass(IntPtr body, IntPtr centerOfMass);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate void fnSetRigidBodyPosition(IntPtr body, IntPtr pos);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate void fnSetRigidBodyPositionAndRotation(IntPtr body, IntPtr pos, IntPtr roate);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate void fnSetRigidBodyMotionType(IntPtr body, int newState);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate void fnDestroyRigidBody(IntPtr body);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate void fnSetRigidBodyGravityFactor(IntPtr body, float gravityFactor);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate void fnGetConvexHullResultTriangles(IntPtr result, IntPtr trianglesBuffer, int count);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate void fnGetConvexHullResultVertices(IntPtr result, IntPtr pointsBuffer, int numPoints);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate IntPtr fnBuild3DPointsConvexHull(IntPtr points, int numPoints);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate IntPtr fnBuild3DFromPlaneConvexHull(IntPtr panels, int numPanels);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate IntPtr fnComputeShapeVolumeMassProperties(IntPtr shape, float mass);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate IntPtr fnComputeBoxSurfaceMassProperties(IntPtr halfExtents, float mass, float surfaceThickness);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate IntPtr fnComputeBoxVolumeMassProperties(IntPtr halfExtents, float mass);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate IntPtr fnComputeCapsuleVolumeMassProperties(IntPtr startAxis, IntPtr endAxis, float radius, float mass);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate IntPtr fnComputeCylinderVolumeMassProperties(IntPtr startAxis, IntPtr endAxis, float radius, float mass);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate IntPtr fnComputeSphereVolumeMassProperties(float radius, float sphereMass);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate IntPtr fnComputeSphereSurfaceMassProperties(float radius, float mass, float surfaceThickness);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate IntPtr fnComputeTriangleSurfaceMassProperties(IntPtr v0, IntPtr v1, IntPtr v2, float mass, float surfaceThickness);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate IntPtr fnCreateBoxShape(IntPtr boxSize, float radius);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate IntPtr fnCreateSphereShape(float radius);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate IntPtr fnCreateCapsuleShape(IntPtr start, IntPtr end, float radius);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate IntPtr fnCreateCylindeShape(IntPtr start, IntPtr end, float radius, float paddingRadius);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate IntPtr fnCreateTriangleShape(IntPtr v0, IntPtr v1, IntPtr v2);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate IntPtr fnCreateConvexVerticesShape(IntPtr vertices, int numVertices, float convexRadius);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate IntPtr fnCreateConvexVerticesShapeByConvexHullResult(IntPtr result, float convexRadius);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate IntPtr fnCreateConvexTranslateShape(IntPtr child, IntPtr translation);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate IntPtr fnCreateConvexTransformShape(IntPtr child, IntPtr transform);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate IntPtr fnCreateListShape(IntPtr childs, int childCount);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate IntPtr fnCreateStaticCompoundShape(IntPtr childs, IntPtr transforms, int childCount);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate void fnStaticCompoundShapeSetInstanceEnabled(IntPtr pStaticCompoundShape, int id, int enabled);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate int fnStaticCompoundShapeIsInstanceEnabled(IntPtr pStaticCompoundShape, int id);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate void fnStaticCompoundShapeEnableAllInstancesAndShapeKeys(IntPtr pStaticCompoundShape);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate void fnDestroyShape(IntPtr s);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate IntPtr fnCreatePhysicsWorld(IntPtr gravity,
      int solverIterationCount, float broadPhaseWorldSize, float collisionTolerance,
      bool bContinuous, bool bVisualDebugger, uint layerMask, IntPtr layerToMask, int stableSolverOn,
      IntPtr onConstraintBreakingCallback, IntPtr onBodyTriggerEventCallback, IntPtr onBodyContactEventCallback);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate void fnDestroyPhysicsWorld(IntPtr world);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate void fnStepPhysicsWorld(IntPtr world, float timestep);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate void fnSetPhysicsWorldGravity(IntPtr world, IntPtr gravity);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate int fnReadPhysicsWorldBodys(IntPtr world, IntPtr buffer, int count);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate void fnSetPhysicsWorldCollisionLayerMasks(IntPtr world, uint layerId, uint toMask, int enable, int forceUpdate);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate void fnSetRigidBodyLayer(IntPtr body, int layer);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate IntPtr fnCreateMatrix4(IntPtr r, int isMatrix4);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate void fnDestroyMatrix4(IntPtr r);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate void fnSetRigidBodyLinearVelocity(IntPtr body, IntPtr v);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate void fnSetRigidBodyAngularVelocity(IntPtr body, IntPtr v);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate void fnGetRigidBodyPosition(IntPtr body, IntPtr outPos);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate void fnGetRigidBodyRotation(IntPtr body, IntPtr outRot);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate void fnGetRigidBodyAngularVelocity(IntPtr body, IntPtr outPos);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate void fnGetRigidBodyLinearVelocity(IntPtr body, IntPtr outPos);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate void fnGetRigidBodyCenterOfMassInWorld(IntPtr body, IntPtr outPos);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate void fnGetRigidBodyPointVelocity(IntPtr body, IntPtr pt, IntPtr outPos);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate void fnRigidBodyResetCenterOfMass(IntPtr body);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate void fnRigidBodyResetInertiaTensor(IntPtr body);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate void fnSetRigidBodyInertiaTensor(IntPtr body, IntPtr inertiaTensor);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate void fnSetRigidBodyMaxLinearVelocity(IntPtr body, float v);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate void fnSetRigidBodyMaxAngularVelocity(IntPtr body, float v);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate void fnDestoryConstraints(IntPtr constraints);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate IntPtr fnCreateBallAndSocketConstraint(IntPtr body, IntPtr otherBody, IntPtr povit, IntPtr breakable);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate IntPtr fnCreateFixedConstraint(IntPtr body, IntPtr otherBody, IntPtr povit, IntPtr breakable);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate IntPtr fnCreateStiffSpringConstraint(IntPtr body, IntPtr otherBody, IntPtr povitAW, IntPtr povitBW, float springMin, float springMax, IntPtr breakable);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate IntPtr fnCreateHingeConstraint(IntPtr body, IntPtr otherBody, IntPtr povit, IntPtr axis, IntPtr breakable);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate IntPtr fnCreateLimitedHingeConstraint(IntPtr body, IntPtr otherBody, IntPtr povit, IntPtr axis, float agularLimitMin, float agularLimitMax, IntPtr breakable, IntPtr motorData);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate IntPtr fnCreateWheelConstraint(IntPtr wheelRigidBody, IntPtr chassis, IntPtr povit, IntPtr axle, IntPtr suspension, IntPtr steering, float suspensionLimitMin, float suspensionLimitMax, float suspensionStrength, float suspensionDamping, IntPtr breakable);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate IntPtr fnCreatePulleyConstraint(IntPtr body, IntPtr otherBody, IntPtr bodyPivot0, IntPtr bodyPivots1, IntPtr worldPivots0, IntPtr worldPivots1, float leverageRatio, IntPtr breakable);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate IntPtr fnCreatePrismaticConstraint(IntPtr body, IntPtr otherBody, IntPtr povit, IntPtr axis, IntPtr breakable, IntPtr motorData);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate IntPtr fnCreateCogWheelConstraint(IntPtr body, IntPtr otherBody, IntPtr rotationPivotA, IntPtr rotationAxisA, float radiusA, IntPtr rotationPivotB, IntPtr rotationAxisB, float radiusB, IntPtr breakable);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate void fnRigidBodyApplyForce(IntPtr body, float delteTime, IntPtr force);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate void fnRigidBodyApplyForceAtPoint(IntPtr body, float delteTime, IntPtr force, IntPtr point);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate void fnRigidBodyApplyTorque(IntPtr body, float delteTime, IntPtr torque);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate void fnRigidBodyApplyAngularImpulse(IntPtr body, IntPtr imp);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate void fnRigidBodyApplyLinearImpulse(IntPtr body, IntPtr imp);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate void fnRigidBodyApplyPointImpulse(IntPtr body, IntPtr imp, IntPtr point);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate int fnIsConstraintBroken(IntPtr constraint);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate void fnSetConstraintBroken(IntPtr constraint, int broken, float force);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate int fnGetRigidBodyId(IntPtr body);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate int fnGetConstraintId(IntPtr body);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate void fnSetConstraintEnable(IntPtr constraint, int enable);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate int fnPhysicsWorldRayCastBody(IntPtr world, IntPtr from, IntPtr to, int rayLayer, IntPtr outResult);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate int fnPhysicsWorldRayCastHit(IntPtr world, IntPtr from, IntPtr to, int rayLayer, int castAll, IntPtr outResult);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate int fnGetVersion();
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate IntPtr fnCreateSimpleMeshShape(IntPtr vertices, int numVertices, IntPtr triangles, int numTriangles, float convexRadius);
    
  /// Return Type: void
  ///constraint: sPhysicsConstraints*
  ///forceMagnitude: float
  ///removed: int
  [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl)]
  public delegate void fnOnConstraintBreakingCallback(IntPtr constraint, int id, float forceMagnitude, int removed);
  [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl)]
  public delegate void fnOnBodyTriggerEventCallback(IntPtr body, IntPtr bodyOther, int id, int otherId, int type);
  [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl)]
  public delegate void fnOnBodyContactEventCallback(IntPtr body, IntPtr bodyOther, int id, int otherId, IntPtr data);

  #endregion

  #region 异常

  public class ApiNotFoundException : Exception
  {
    public ApiNotFoundException(string name) : base(name + " api not found") { }
  }
  public class ApiException : Exception
  {
    public ApiException(string msg) : base(msg) { }
  }

  #endregion

  public class ApiStruct
  {
    public const int Version = 2301;
    public bool InitSuccess { get; private set; } = false;

    //获取所有函数指针
    internal void initAll(IntPtr apiArrayPtr, int len)
    {
      int i = 0;
      IntPtr[] apiArray = new IntPtr[len];
      Marshal.Copy(apiArrayPtr, apiArray, 0, len);

      _GetVersion = Marshal.GetDelegateForFunctionPointer<fnGetVersion>(apiArray[254]);

      var v = _GetVersion();
      if(v != Version)
        throw new ApiException("Native lib version is not compatible with this (" + v + " !=" + Version + ")");

      _CommonDelete = Marshal.GetDelegateForFunctionPointer<fnCommonDelete>(apiArray[i++]);
      _CreateVec3 = Marshal.GetDelegateForFunctionPointer<fnCreateVec3>(apiArray[i++]);
      _CreateTransform = Marshal.GetDelegateForFunctionPointer<fnCreateTransform>(apiArray[i++]);
      _CreateVec4 = Marshal.GetDelegateForFunctionPointer<fnCreateVec4>(apiArray[i++]);
      _DestroyVec4 = Marshal.GetDelegateForFunctionPointer<fnDestroyVec4>(apiArray[i++]);
      _DestroyVec3 = Marshal.GetDelegateForFunctionPointer<fnDestroyVec3>(apiArray[i++]);
      _DestroyTransform = Marshal.GetDelegateForFunctionPointer<fnDestroyTransform>(apiArray[i++]);
      _CreatePhysicsWorld = Marshal.GetDelegateForFunctionPointer<fnCreatePhysicsWorld>(apiArray[i++]);
      _DestroyPhysicsWorld = Marshal.GetDelegateForFunctionPointer<fnDestroyPhysicsWorld>(apiArray[i++]);
      _StepPhysicsWorld = Marshal.GetDelegateForFunctionPointer<fnStepPhysicsWorld>(apiArray[i++]);
      _SetPhysicsWorldGravity = Marshal.GetDelegateForFunctionPointer<fnSetPhysicsWorldGravity>(apiArray[i++]);
      _ReadPhysicsWorldBodys = Marshal.GetDelegateForFunctionPointer<fnReadPhysicsWorldBodys>(apiArray[i++]);
      _CreateRigidBody = Marshal.GetDelegateForFunctionPointer<fnCreateRigidBody>(apiArray[i++]);
      _ActiveRigidBody = Marshal.GetDelegateForFunctionPointer<fnActiveRigidBody>(apiArray[i++]);
      _DeactiveRigidBody = Marshal.GetDelegateForFunctionPointer<fnDeactiveRigidBody>(apiArray[i++]);
      _SetRigidBodyMass = Marshal.GetDelegateForFunctionPointer<fnSetRigidBodyMass>(apiArray[i++]);
      _SetRigidBodyFriction = Marshal.GetDelegateForFunctionPointer<fnSetRigidBodyFriction>(apiArray[i++]);
      _SetRigidBodyRestitution = Marshal.GetDelegateForFunctionPointer<fnSetRigidBodyRestitution>(apiArray[i++]);
      _SetRigidBodyCenterOfMass = Marshal.GetDelegateForFunctionPointer<fnSetRigidBodyCenterOfMass>(apiArray[i++]);
      _SetRigidBodyPosition = Marshal.GetDelegateForFunctionPointer<fnSetRigidBodyPosition>(apiArray[i++]);
      _SetRigidBodyPositionAndRotation = Marshal.GetDelegateForFunctionPointer<fnSetRigidBodyPositionAndRotation>(apiArray[i++]);
      _SetRigidBodyAngularDamping = Marshal.GetDelegateForFunctionPointer<fnSetRigidBodyAngularDamping>(apiArray[i++]);
      _SetRigidBodyLinearDampin = Marshal.GetDelegateForFunctionPointer<fnSetRigidBodyLinearDampin>(apiArray[i++]);
      _SetRigidBodyMotionType = Marshal.GetDelegateForFunctionPointer<fnSetRigidBodyMotionType>(apiArray[i++]);
      _SetRigidBodyGravityFactor = Marshal.GetDelegateForFunctionPointer<fnSetRigidBodyGravityFactor>(apiArray[i++]);
      _GetConvexHullResultTriangles = Marshal.GetDelegateForFunctionPointer<fnGetConvexHullResultTriangles>(apiArray[i++]);
      _GetConvexHullResultVertices = Marshal.GetDelegateForFunctionPointer<fnGetConvexHullResultVertices>(apiArray[i++]);
      _Build3DPointsConvexHull = Marshal.GetDelegateForFunctionPointer<fnBuild3DPointsConvexHull>(apiArray[i++]);
      _Build3DFromPlaneConvexHull = Marshal.GetDelegateForFunctionPointer<fnBuild3DFromPlaneConvexHull>(apiArray[i++]);
      _DestroyRigidBody = Marshal.GetDelegateForFunctionPointer<fnDestroyRigidBody>(apiArray[i++]);
      _ComputeShapeVolumeMassProperties = Marshal.GetDelegateForFunctionPointer<fnComputeShapeVolumeMassProperties>(apiArray[i++]);
      _ComputeBoxSurfaceMassProperties = Marshal.GetDelegateForFunctionPointer<fnComputeBoxSurfaceMassProperties>(apiArray[i++]);
      _ComputeBoxVolumeMassProperties = Marshal.GetDelegateForFunctionPointer<fnComputeBoxVolumeMassProperties>(apiArray[i++]);
      _ComputeCapsuleVolumeMassProperties = Marshal.GetDelegateForFunctionPointer<fnComputeCapsuleVolumeMassProperties>(apiArray[i++]);
      _ComputeCylinderVolumeMassProperties = Marshal.GetDelegateForFunctionPointer<fnComputeCylinderVolumeMassProperties>(apiArray[i++]);
      _ComputeSphereVolumeMassProperties = Marshal.GetDelegateForFunctionPointer<fnComputeSphereVolumeMassProperties>(apiArray[i++]);
      _ComputeSphereSurfaceMassProperties = Marshal.GetDelegateForFunctionPointer<fnComputeSphereSurfaceMassProperties>(apiArray[i++]);
      _ComputeTriangleSurfaceMassProperties = Marshal.GetDelegateForFunctionPointer<fnComputeTriangleSurfaceMassProperties>(apiArray[i++]);
      _CreateBoxShape = Marshal.GetDelegateForFunctionPointer<fnCreateBoxShape>(apiArray[i++]);
      _CreateSphereShape = Marshal.GetDelegateForFunctionPointer<fnCreateSphereShape>(apiArray[i++]);
      _CreateCapsuleShape = Marshal.GetDelegateForFunctionPointer<fnCreateCapsuleShape>(apiArray[i++]);
      _CreateCylindeShape = Marshal.GetDelegateForFunctionPointer<fnCreateCylindeShape>(apiArray[i++]);
      _CreateTriangleShape = Marshal.GetDelegateForFunctionPointer<fnCreateTriangleShape>(apiArray[i++]);
      _CreateConvexVerticesShape = Marshal.GetDelegateForFunctionPointer<fnCreateConvexVerticesShape>(apiArray[i++]);
      _CreateConvexVerticesShapeByConvexHullResult = Marshal.GetDelegateForFunctionPointer<fnCreateConvexVerticesShapeByConvexHullResult>(apiArray[i++]);
      _CreateSimpleMeshShape = Marshal.GetDelegateForFunctionPointer<fnCreateSimpleMeshShape>(apiArray[i++]);
      _CreateConvexTranslateShape = Marshal.GetDelegateForFunctionPointer<fnCreateConvexTranslateShape>(apiArray[i++]);
      _CreateConvexTransformShape = Marshal.GetDelegateForFunctionPointer<fnCreateConvexTransformShape>(apiArray[i++]);
      _CreateListShape = Marshal.GetDelegateForFunctionPointer<fnCreateListShape>(apiArray[i++]);
      _CreateStaticCompoundShape = Marshal.GetDelegateForFunctionPointer<fnCreateStaticCompoundShape>(apiArray[i++]);
      _StaticCompoundShapeSetInstanceEnabled = Marshal.GetDelegateForFunctionPointer<fnStaticCompoundShapeSetInstanceEnabled>(apiArray[i++]);
      _StaticCompoundShapeIsInstanceEnabled = Marshal.GetDelegateForFunctionPointer<fnStaticCompoundShapeIsInstanceEnabled>(apiArray[i++]);
      _StaticCompoundShapeEnableAllInstancesAndShapeKeys = Marshal.GetDelegateForFunctionPointer<fnStaticCompoundShapeEnableAllInstancesAndShapeKeys>(apiArray[i++]);
      _DestroyShape = Marshal.GetDelegateForFunctionPointer<fnDestroyShape>(apiArray[i++]);
	    _TestAssert = Marshal.GetDelegateForFunctionPointer<fnTestAssert>(apiArray[i++]);
	    _SetRigidBodyLayer = Marshal.GetDelegateForFunctionPointer<fnSetRigidBodyLayer>(apiArray[i++]);
	    _SetPhysicsWorldCollisionLayerMasks = Marshal.GetDelegateForFunctionPointer<fnSetPhysicsWorldCollisionLayerMasks>(apiArray[i++]);
      _CreateMatrix4 = Marshal.GetDelegateForFunctionPointer<fnCreateMatrix4>(apiArray[i++]);
      _DestroyMatrix4 = Marshal.GetDelegateForFunctionPointer<fnDestroyMatrix4>(apiArray[i++]);
      _SetRigidBodyLinearVelocity = Marshal.GetDelegateForFunctionPointer<fnSetRigidBodyLinearVelocity>(apiArray[i++]);
      _SetRigidBodyAngularVelocity = Marshal.GetDelegateForFunctionPointer<fnSetRigidBodyAngularVelocity>(apiArray[i++]);
      _SetRigidBodyInertiaTensor = Marshal.GetDelegateForFunctionPointer<fnSetRigidBodyInertiaTensor>(apiArray[i++]);
      _GetRigidBodyPosition = Marshal.GetDelegateForFunctionPointer<fnGetRigidBodyPosition>(apiArray[i++]);
      _GetRigidBodyRotation = Marshal.GetDelegateForFunctionPointer<fnGetRigidBodyRotation>(apiArray[i++]);
      _GetRigidBodyAngularVelocity = Marshal.GetDelegateForFunctionPointer<fnGetRigidBodyAngularVelocity>(apiArray[i++]);
      _GetRigidBodyLinearVelocity = Marshal.GetDelegateForFunctionPointer<fnGetRigidBodyLinearVelocity>(apiArray[i++]);
      _GetRigidBodyCenterOfMassInWorld = Marshal.GetDelegateForFunctionPointer<fnGetRigidBodyCenterOfMassInWorld>(apiArray[i++]);
      _GetRigidBodyPointVelocity = Marshal.GetDelegateForFunctionPointer<fnGetRigidBodyPointVelocity>(apiArray[i++]);
      _RigidBodyResetCenterOfMass = Marshal.GetDelegateForFunctionPointer<fnRigidBodyResetCenterOfMass>(apiArray[i++]);
      _RigidBodyResetInertiaTensor = Marshal.GetDelegateForFunctionPointer<fnRigidBodyResetInertiaTensor>(apiArray[i++]);
      _SetRigidBodyMaxLinearVelocity = Marshal.GetDelegateForFunctionPointer<fnSetRigidBodyMaxLinearVelocity>(apiArray[i++]);
      _SetRigidBodyMaxAngularVelocity = Marshal.GetDelegateForFunctionPointer<fnSetRigidBodyMaxAngularVelocity>(apiArray[i++]);
      _DestoryConstraints = Marshal.GetDelegateForFunctionPointer<fnDestoryConstraints>(apiArray[i++]);
      _CreateBallAndSocketConstraint = Marshal.GetDelegateForFunctionPointer<fnCreateBallAndSocketConstraint>(apiArray[i++]);
      _CreateFixedConstraint = Marshal.GetDelegateForFunctionPointer<fnCreateFixedConstraint>(apiArray[i++]);
      _CreateStiffSpringConstraint = Marshal.GetDelegateForFunctionPointer<fnCreateStiffSpringConstraint>(apiArray[i++]);
      _CreateHingeConstraint = Marshal.GetDelegateForFunctionPointer<fnCreateHingeConstraint>(apiArray[i++]);
      _CreateLimitedHingeConstraint = Marshal.GetDelegateForFunctionPointer<fnCreateLimitedHingeConstraint>(apiArray[i++]);
      _CreateWheelConstraint = Marshal.GetDelegateForFunctionPointer<fnCreateWheelConstraint>(apiArray[i++]);
      _CreatePulleyConstraint = Marshal.GetDelegateForFunctionPointer<fnCreatePulleyConstraint>(apiArray[i++]);
      _CreatePrismaticConstraint = Marshal.GetDelegateForFunctionPointer<fnCreatePrismaticConstraint>(apiArray[i++]);
      _CreateCogWheelConstraint = Marshal.GetDelegateForFunctionPointer<fnCreateCogWheelConstraint>(apiArray[i++]);
      _RigidBodyApplyForce = Marshal.GetDelegateForFunctionPointer<fnRigidBodyApplyForce>(apiArray[i++]);
      _RigidBodyApplyForceAtPoint = Marshal.GetDelegateForFunctionPointer<fnRigidBodyApplyForceAtPoint>(apiArray[i++]);
      _RigidBodyApplyTorque = Marshal.GetDelegateForFunctionPointer<fnRigidBodyApplyTorque>(apiArray[i++]);
      _RigidBodyApplyAngularImpulse = Marshal.GetDelegateForFunctionPointer<fnRigidBodyApplyAngularImpulse>(apiArray[i++]);
      _RigidBodyApplyLinearImpulse = Marshal.GetDelegateForFunctionPointer<fnRigidBodyApplyLinearImpulse>(apiArray[i++]);
      _RigidBodyApplyPointImpulse = Marshal.GetDelegateForFunctionPointer<fnRigidBodyApplyPointImpulse>(apiArray[i++]);
      _IsConstraintBroken = Marshal.GetDelegateForFunctionPointer<fnIsConstraintBroken>(apiArray[i++]);
      _SetConstraintBroken = Marshal.GetDelegateForFunctionPointer<fnSetConstraintBroken>(apiArray[i++]);
      _GetRigidBodyId = Marshal.GetDelegateForFunctionPointer<fnGetRigidBodyId>(apiArray[i++]);
      _GetConstraintId = Marshal.GetDelegateForFunctionPointer<fnGetConstraintId>(apiArray[i++]);
      _SetConstraintEnable = Marshal.GetDelegateForFunctionPointer<fnSetConstraintEnable>(apiArray[i++]);
      _PhysicsWorldRayCastBody = Marshal.GetDelegateForFunctionPointer<fnPhysicsWorldRayCastBody>(apiArray[i++]);
      _PhysicsWorldRayCastHit = Marshal.GetDelegateForFunctionPointer<fnPhysicsWorldRayCastHit>(apiArray[i++]);

      InitSuccess = true;
    }

    private fnTestAssert _TestAssert;
    private fnCommonDelete _CommonDelete;
    private fnCreateVec3 _CreateVec3;
    private fnCreateTransform _CreateTransform;
    private fnCreateVec4 _CreateVec4;
    private fnDestroyVec4 _DestroyVec4;
    private fnDestroyVec3 _DestroyVec3;
    private fnDestroyTransform _DestroyTransform;
    private fnCreatePhysicsWorld _CreatePhysicsWorld;
    private fnDestroyPhysicsWorld _DestroyPhysicsWorld;
    private fnStepPhysicsWorld _StepPhysicsWorld;
    private fnSetPhysicsWorldGravity _SetPhysicsWorldGravity;
    private fnReadPhysicsWorldBodys _ReadPhysicsWorldBodys;
    private fnCreateRigidBody _CreateRigidBody;
    private fnActiveRigidBody _ActiveRigidBody;
    private fnDeactiveRigidBody _DeactiveRigidBody;
    private fnSetRigidBodyMass _SetRigidBodyMass;
    private fnSetRigidBodyFriction _SetRigidBodyFriction;
    private fnSetRigidBodyRestitution _SetRigidBodyRestitution;
    private fnSetRigidBodyCenterOfMass _SetRigidBodyCenterOfMass;
    private fnSetRigidBodyAngularDamping _SetRigidBodyAngularDamping;
    private fnSetRigidBodyLinearDampin _SetRigidBodyLinearDampin;
    private fnSetRigidBodyPosition _SetRigidBodyPosition;
    private fnSetRigidBodyPositionAndRotation _SetRigidBodyPositionAndRotation;
    private fnSetRigidBodyMotionType _SetRigidBodyMotionType;
    private fnSetRigidBodyGravityFactor _SetRigidBodyGravityFactor;
    private fnGetConvexHullResultTriangles _GetConvexHullResultTriangles;
    private fnGetConvexHullResultVertices _GetConvexHullResultVertices;
    private fnBuild3DPointsConvexHull _Build3DPointsConvexHull;
    private fnBuild3DFromPlaneConvexHull _Build3DFromPlaneConvexHull;
    private fnDestroyRigidBody _DestroyRigidBody;
    private fnComputeShapeVolumeMassProperties _ComputeShapeVolumeMassProperties;
    private fnComputeBoxSurfaceMassProperties _ComputeBoxSurfaceMassProperties;
    private fnComputeBoxVolumeMassProperties _ComputeBoxVolumeMassProperties;
    private fnComputeCapsuleVolumeMassProperties _ComputeCapsuleVolumeMassProperties;
    private fnComputeCylinderVolumeMassProperties _ComputeCylinderVolumeMassProperties;
    private fnComputeSphereVolumeMassProperties _ComputeSphereVolumeMassProperties;
    private fnComputeSphereSurfaceMassProperties _ComputeSphereSurfaceMassProperties;
    private fnComputeTriangleSurfaceMassProperties _ComputeTriangleSurfaceMassProperties;
    private fnCreateBoxShape _CreateBoxShape;
    private fnCreateSphereShape _CreateSphereShape;
    private fnCreateCapsuleShape _CreateCapsuleShape;
    private fnCreateCylindeShape _CreateCylindeShape;
    private fnCreateTriangleShape _CreateTriangleShape;
    private fnCreateConvexVerticesShape _CreateConvexVerticesShape;
    private fnCreateConvexVerticesShapeByConvexHullResult _CreateConvexVerticesShapeByConvexHullResult;
    private fnCreateSimpleMeshShape _CreateSimpleMeshShape;
    private fnCreateConvexTranslateShape _CreateConvexTranslateShape;
    private fnCreateConvexTransformShape _CreateConvexTransformShape;
    private fnCreateListShape _CreateListShape;
    private fnCreateStaticCompoundShape _CreateStaticCompoundShape;
    private fnStaticCompoundShapeSetInstanceEnabled _StaticCompoundShapeSetInstanceEnabled;
    private fnStaticCompoundShapeIsInstanceEnabled _StaticCompoundShapeIsInstanceEnabled;
    private fnStaticCompoundShapeEnableAllInstancesAndShapeKeys _StaticCompoundShapeEnableAllInstancesAndShapeKeys;
    private fnDestroyShape _DestroyShape;
    private fnSetPhysicsWorldCollisionLayerMasks _SetPhysicsWorldCollisionLayerMasks;
    private fnSetRigidBodyLayer _SetRigidBodyLayer;
    private fnCreateMatrix4 _CreateMatrix4;
    private fnDestroyMatrix4 _DestroyMatrix4;
    private fnSetRigidBodyLinearVelocity _SetRigidBodyLinearVelocity;
    private fnSetRigidBodyAngularVelocity _SetRigidBodyAngularVelocity;
    private fnGetRigidBodyPosition _GetRigidBodyPosition;
    private fnGetRigidBodyRotation _GetRigidBodyRotation;
    private fnGetRigidBodyAngularVelocity _GetRigidBodyAngularVelocity;
    private fnGetRigidBodyLinearVelocity _GetRigidBodyLinearVelocity;
    private fnGetRigidBodyCenterOfMassInWorld _GetRigidBodyCenterOfMassInWorld;
    private fnGetRigidBodyPointVelocity _GetRigidBodyPointVelocity;
    private fnRigidBodyResetCenterOfMass _RigidBodyResetCenterOfMass;
    private fnRigidBodyResetInertiaTensor _RigidBodyResetInertiaTensor;
    private fnSetRigidBodyInertiaTensor _SetRigidBodyInertiaTensor;
    private fnSetRigidBodyMaxLinearVelocity _SetRigidBodyMaxLinearVelocity;
    private fnSetRigidBodyMaxAngularVelocity _SetRigidBodyMaxAngularVelocity;
    private fnDestoryConstraints _DestoryConstraints;
    private fnCreateBallAndSocketConstraint _CreateBallAndSocketConstraint;
    private fnCreateFixedConstraint _CreateFixedConstraint;
    private fnCreateStiffSpringConstraint _CreateStiffSpringConstraint;
    private fnCreateHingeConstraint _CreateHingeConstraint;
    private fnCreateLimitedHingeConstraint _CreateLimitedHingeConstraint;
    private fnCreateWheelConstraint _CreateWheelConstraint;
    private fnCreatePulleyConstraint _CreatePulleyConstraint;
    private fnCreatePrismaticConstraint _CreatePrismaticConstraint;
    private fnCreateCogWheelConstraint _CreateCogWheelConstraint;
    private fnRigidBodyApplyForce _RigidBodyApplyForce;
    private fnRigidBodyApplyForceAtPoint _RigidBodyApplyForceAtPoint;
    private fnRigidBodyApplyTorque _RigidBodyApplyTorque;
    private fnRigidBodyApplyAngularImpulse _RigidBodyApplyAngularImpulse;
    private fnRigidBodyApplyLinearImpulse _RigidBodyApplyLinearImpulse;
    private fnRigidBodyApplyPointImpulse _RigidBodyApplyPointImpulse;
    private fnIsConstraintBroken _IsConstraintBroken;
    private fnSetConstraintBroken _SetConstraintBroken;
    private fnGetRigidBodyId _GetRigidBodyId;
    private fnGetConstraintId _GetConstraintId;
    private fnSetConstraintEnable _SetConstraintEnable;
    private fnPhysicsWorldRayCastBody _PhysicsWorldRayCastBody;
    private fnPhysicsWorldRayCastHit _PhysicsWorldRayCastHit;
    private fnGetVersion _GetVersion;

    public int GetVersion() { return _GetVersion(); }
    public int BoolToInt(bool a) {
      return a ? 1 : 0;
    }   
    private IntPtr Matrix4x4ToNative9(Matrix4x4 a) {

      IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf<float>() * 9);
      float [] ar = new float[9];
      int i = 0;
      ar[i++] = a[0,0];
      ar[i++] = a[0,1];
      ar[i++] = a[0,2];
      ar[i++] = a[1,0];
      ar[i++] = a[1,1];
      ar[i++] = a[1,2];
      ar[i++] = a[2,0];
      ar[i++] = a[2,1];
      ar[i++] = a[2,2];

      IntPtr mptr = _CreateMatrix4(ptr, 0);
      Marshal.FreeHGlobal(ptr);
      return mptr;
    }
    private IntPtr Matrix4x4ToNative16(Matrix4x4 a) {
       IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf<float>() * 16);
      float [] ar = new float[16];
      int i = 0;
      IntPtr mptr = _CreateMatrix4(ptr, 1);
      
      ar[i++] = a[0,0];
      ar[i++] = a[0,1];
      ar[i++] = a[0,2];
      ar[i++] = a[0,3];
      ar[i++] = a[1,0];
      ar[i++] = a[1,1];
      ar[i++] = a[1,2];
      ar[i++] = a[1,3];
      ar[i++] = a[2,0];
      ar[i++] = a[2,1];
      ar[i++] = a[2,2];
      ar[i++] = a[2,3];
      ar[i++] = a[3,0];
      ar[i++] = a[3,1];
      ar[i++] = a[3,2];
      ar[i++] = a[3,3];

      Marshal.FreeHGlobal(ptr);
      return mptr;
    }
    private IntPtr Vector3ToNative3(Vector3 a) {
      return _CreateVec3(a.x, a.y, a.z);
    }
    private IntPtr Vector4ToNative4(Vector4 a) {
      return _CreateVec4(a.x, a.y, a.z, a.w);
    }
    private IntPtr QuaternionToNative4(Quaternion a) {
      return _CreateVec4(a.x, a.y, a.z, a.w);
    }
    private IntPtr TransformToNative(Vector3 pos, Quaternion rot, Vector3 scale) {
      return _CreateTransform(pos.x, pos.y, pos.z, rot.x, rot.y, rot.z, rot.w, scale.x, scale.y, rot.z);
    }
    private void FreeNativeMatrix4x4(IntPtr a) {
      _DestroyMatrix4(a);
    }
    private void FreeNativeVector3(IntPtr a) {
      _DestroyVec3(a);
    }
    private void FreeNativeVector4(IntPtr a) {
      _DestroyVec4(a);
    }
    private void FreeNativeTransform(IntPtr a) {
      _DestroyTransform(a);
    }

    private void ApiExceptionCheck() {
      var exp = PhysicsApi.checkException();
      if (exp != null)
        throw new ApiException(exp);
    }

    public int PhysicsWorldRayCastBody(IntPtr world, Vector3 from, Vector3 to, int rayLayer, out sRayCastResult outResult) {
      if (_PhysicsWorldRayCastBody == null)
        throw new ApiNotFoundException("PhysicsWorldRayCastBody");

      var outPtr = Marshal.AllocHGlobal(Marshal.SizeOf<IntPtr>());
      var fromPtr = Vector3ToNative3(from);
      var toPtr = Vector3ToNative3(to);
      var rs = _PhysicsWorldRayCastBody(world, fromPtr, toPtr, rayLayer, outPtr);
      FreeNativeVector3(fromPtr);
      FreeNativeVector3(toPtr);

      if(rs > 0) {
        var rsPtr = Marshal.ReadIntPtr(outPtr);
        outResult = Marshal.PtrToStructure<sRayCastResult>(rsPtr);
        _CommonDelete(rsPtr);
      } else {
        outResult = default(sRayCastResult);
      }
      Marshal.FreeHGlobal(outPtr);

      ApiExceptionCheck();

      return rs;
    }
    public int PhysicsWorldRayCastHit(IntPtr world, Vector3 from, Vector3 to, int rayLayer, bool castAll, out sRayCastResult[] outResult) {
      if (_PhysicsWorldRayCastHit == null)
        throw new ApiNotFoundException("PhysicsWorldRayCastHit");

      var outPtr = Marshal.AllocHGlobal(Marshal.SizeOf<IntPtr>());
      var fromPtr = Vector3ToNative3(from);
      var toPtr = Vector3ToNative3(to);
      var rs = _PhysicsWorldRayCastHit(world, fromPtr, toPtr, rayLayer, BoolToInt(castAll), outPtr);
      FreeNativeVector3(fromPtr);
      FreeNativeVector3(toPtr);

      if(rs > 0) {

        if(castAll) {
          var rsPtr = Marshal.ReadIntPtr(outPtr);
          var rsPtrArr = new IntPtr[rs];
          Marshal.Copy(rsPtr, rsPtrArr, 0, rs);
          outResult = new sRayCastResult[rs];
          for(int i = 0; i < rs; i++)
            outResult[i] = Marshal.PtrToStructure<sRayCastResult>(rsPtrArr[i]);
          _CommonDelete(rsPtr);
        } else {
          var rsPtr = Marshal.ReadIntPtr(outPtr);
          outResult = new sRayCastResult[1];
          outResult[0] = Marshal.PtrToStructure<sRayCastResult>(rsPtr);
          _CommonDelete(rsPtr);
        }
      } else {
        outResult = null;
      }
      Marshal.FreeHGlobal(outPtr);

      ApiExceptionCheck();

      return rs;
    }

    private IntPtr ConstraintBreakDataToNative(sConstraintBreakData data) {
      if(data.breakable) {
        IntPtr rs = Marshal.AllocHGlobal(Marshal.SizeOf(data));
        Marshal.StructureToPtr(data, rs, false);
        return rs;
      }
      return IntPtr.Zero;
    }
    private IntPtr ConstraintMotorDataToNative(sConstraintMotorData data) {
      if(data.enable) {
        IntPtr rs = Marshal.AllocHGlobal(Marshal.SizeOf(data));
        Marshal.StructureToPtr(data, rs, false);
        return rs;
      }
      return IntPtr.Zero;
    }

    public IntPtr CreateBallAndSocketConstraint(IntPtr body, IntPtr otherBody, Vector3 povit, sConstraintBreakData breakable) {
      if (_CreateBallAndSocketConstraint == null)
        throw new ApiNotFoundException("CreateBallAndSocketConstraint");

      var povitPtr = Vector3ToNative3(povit);
      var breakablePtr = ConstraintBreakDataToNative(breakable);
      var rs = _CreateBallAndSocketConstraint(body, otherBody, povitPtr, breakablePtr);
      FreeNativeVector3(povitPtr);
      Marshal.FreeHGlobal(breakablePtr);

      ApiExceptionCheck();

      return rs;
    }
    public IntPtr CreateFixedConstraint(IntPtr body, IntPtr otherBody, Vector3 povit, sConstraintBreakData breakable) {
      if (_CreateFixedConstraint == null)
        throw new ApiNotFoundException("CreateFixedConstraint");

      var povitPtr = Vector3ToNative3(povit);
      var breakablePtr = ConstraintBreakDataToNative(breakable);
      var rs = _CreateFixedConstraint(body, otherBody, povitPtr, breakablePtr);
      FreeNativeVector3(povitPtr);
      Marshal.FreeHGlobal(breakablePtr);

      ApiExceptionCheck();

      return rs;
    }
    public IntPtr CreateStiffSpringConstraint(IntPtr body, IntPtr otherBody, Vector3 povitAW, Vector3 povitBW, float springMin, float springMax, sConstraintBreakData breakable) {
      if (_CreateStiffSpringConstraint == null)
        throw new ApiNotFoundException("CreateStiffSpringConstraint");

      var povitAWPtr = Vector3ToNative3(povitAW);
      var povitBWPtr = Vector3ToNative3(povitBW);
      var breakablePtr = ConstraintBreakDataToNative(breakable);
      var rs = _CreateStiffSpringConstraint(body, otherBody, povitAWPtr, povitBWPtr, springMin, springMax, breakablePtr);
      FreeNativeVector3(povitAWPtr);
      FreeNativeVector3(povitBWPtr);
      Marshal.FreeHGlobal(breakablePtr);

      ApiExceptionCheck();

      return rs;
    }
    public IntPtr CreateHingeConstraint(IntPtr body, IntPtr otherBody, Vector3 povit, Vector3 axis, sConstraintBreakData breakable) {
      if (_CreateHingeConstraint == null)
        throw new ApiNotFoundException("CreateHingeConstraint");

      var povitPtr = Vector3ToNative3(povit);
      var axisPtr = Vector3ToNative3(axis);
      var breakablePtr = ConstraintBreakDataToNative(breakable);
      var rs = _CreateHingeConstraint(body, otherBody, povitPtr, axisPtr, breakablePtr);
      FreeNativeVector3(povitPtr);
      FreeNativeVector3(axisPtr);
      Marshal.FreeHGlobal(breakablePtr);

      ApiExceptionCheck();

      return rs;
    }
    public IntPtr CreateLimitedHingeConstraint(IntPtr body, IntPtr otherBody, Vector3 povit, Vector3 axis, float agularLimitMin, float agularLimitMax, sConstraintBreakData breakable, sConstraintMotorData motorData) {
      if (_CreateLimitedHingeConstraint == null)
        throw new ApiNotFoundException("CreateLimitedHingeConstraint");

      var povitPtr = Vector3ToNative3(povit);
      var axisPtr = Vector3ToNative3(axis);
      var breakablePtr = ConstraintBreakDataToNative(breakable);
      var motoDataPtr = ConstraintMotorDataToNative(motorData);
      var rs = _CreateLimitedHingeConstraint(body, otherBody, povitPtr, axisPtr, agularLimitMin, agularLimitMax, breakablePtr, motoDataPtr);
      FreeNativeVector3(povitPtr);
      FreeNativeVector3(axisPtr);
      Marshal.FreeHGlobal(breakablePtr);
      Marshal.FreeHGlobal(motoDataPtr);

      ApiExceptionCheck();

      return rs;
    }
    public IntPtr CreateWheelConstraint(IntPtr wheelRigidBody, IntPtr chassis, Vector3 povit, Vector3 axle, Vector3 suspension, Vector3 steering, float suspensionLimitMin, float suspensionLimitMax, float suspensionStrength, float suspensionDamping, sConstraintBreakData breakable) {
      if (_CreateWheelConstraint == null)
        throw new ApiNotFoundException("CreateWheelConstraint");

      var povitPtr = Vector3ToNative3(povit);
      var axlePtr = Vector3ToNative3(axle);
      var suspensionPtr = Vector3ToNative3(suspension);
      var steeringPtr = Vector3ToNative3(steering);
      var breakablePtr = ConstraintBreakDataToNative(breakable);
      var rs = _CreateWheelConstraint(wheelRigidBody, chassis, povitPtr, axlePtr, suspensionPtr, steeringPtr, suspensionLimitMin, suspensionLimitMax, suspensionStrength, suspensionDamping, breakablePtr);
      FreeNativeVector3(axlePtr); 
      FreeNativeVector3(povitPtr);
      FreeNativeVector3(suspensionPtr);
      FreeNativeVector3(steeringPtr);
      Marshal.FreeHGlobal(breakablePtr);

      ApiExceptionCheck();

      return rs;
    }
    public IntPtr CreatePulleyConstraint(IntPtr body, IntPtr otherBody, Vector3 bodyPivot0, Vector3 bodyPivots1, Vector3 worldPivots0, Vector3 worldPivots1, float leverageRatio, sConstraintBreakData breakable) {
      if (_CreatePulleyConstraint == null)
        throw new ApiNotFoundException("CreatePulleyConstraint");

      var bodyPivot0Ptr = Vector3ToNative3(bodyPivot0);
      var bodyPivots1Ptr = Vector3ToNative3(bodyPivots1);
      var worldPivots0Ptr = Vector3ToNative3(worldPivots0);
      var worldPivots1Ptr = Vector3ToNative3(worldPivots1);
      var breakablePtr = ConstraintBreakDataToNative(breakable);
      var rs = _CreatePulleyConstraint(body, otherBody, bodyPivot0Ptr, bodyPivots1Ptr, worldPivots0Ptr, worldPivots1Ptr, leverageRatio, breakablePtr);
      FreeNativeVector3(bodyPivot0Ptr);
      FreeNativeVector3(bodyPivots1Ptr);
      FreeNativeVector3(worldPivots0Ptr);
      FreeNativeVector3(worldPivots1Ptr);
      Marshal.FreeHGlobal(breakablePtr);

      ApiExceptionCheck();

      return rs;
    }
    public IntPtr CreatePrismaticConstraint(IntPtr body, IntPtr otherBody, Vector3 povit, Vector3 axis, sConstraintBreakData breakable, sConstraintMotorData motorData) {
      if (_CreatePrismaticConstraint == null)
        throw new ApiNotFoundException("CreatePrismaticConstraint");

      var povitPtr = Vector3ToNative3(povit);
      var axisPtr = Vector3ToNative3(axis);
      
      var breakablePtr = ConstraintBreakDataToNative(breakable);
      var motoDataPtr = ConstraintMotorDataToNative(motorData);
      var rs = _CreatePrismaticConstraint(body, otherBody, povitPtr, axisPtr, breakablePtr, motoDataPtr);
      FreeNativeVector3(povitPtr);
      FreeNativeVector3(axisPtr);
      Marshal.FreeHGlobal(breakablePtr);
      Marshal.FreeHGlobal(motoDataPtr);

      ApiExceptionCheck();

      return rs;
    }
    public IntPtr CreateCogWheelConstraint(IntPtr body, IntPtr otherBody, Vector3 rotationPivotA, Vector3 rotationAxisA, float radiusA, Vector3 rotationPivotB, Vector3 rotationAxisB, float radiusB, sConstraintBreakData breakable) {
      if (_CreateCogWheelConstraint == null)
        throw new ApiNotFoundException("CreateCogWheelConstraint");

      var rotationPivotAPtr = Vector3ToNative3(rotationPivotA);
      var rotationAxisAPtr = Vector3ToNative3(rotationAxisA);
      var rotationPivotBPtr = Vector3ToNative3(rotationPivotB);
      var rotationAxisBPtr = Vector3ToNative3(rotationAxisB);
      var breakablePtr = ConstraintBreakDataToNative(breakable);
      var rs = _CreateCogWheelConstraint(body, otherBody, rotationPivotAPtr, rotationAxisAPtr, radiusA, rotationPivotBPtr, rotationAxisBPtr, radiusB, breakablePtr);
      FreeNativeVector3(rotationPivotAPtr);
      FreeNativeVector3(rotationAxisAPtr);
      FreeNativeVector3(rotationPivotBPtr);
      FreeNativeVector3(rotationAxisBPtr);
      Marshal.FreeHGlobal(breakablePtr);



      return rs;
    }

    public int GetRigidBodyId(IntPtr body) {
      if (_GetRigidBodyId == null)
        throw new ApiNotFoundException("GetRigidBodyId");

      var rs = _GetRigidBodyId(body);

      ApiExceptionCheck();

      return rs;
    }
    public int GetConstraintId(IntPtr body) {
      if (_GetConstraintId == null)
        throw new ApiNotFoundException("GetConstraintId");

      var rs = _GetConstraintId(body);

      ApiExceptionCheck();

      return rs;
    }
    public void SetConstraintBroken(IntPtr constraint, bool broken, float force) {
      if (_SetConstraintBroken == null)
        throw new ApiNotFoundException("_SetConstraintBroken");

      _SetConstraintBroken(constraint, BoolToInt(broken), force);

      ApiExceptionCheck();
    }
    public void SetConstraintEnable(IntPtr constraint, bool enable) {
      if (_SetConstraintEnable == null)
        throw new ApiNotFoundException("SetConstraintEnable");

      _SetConstraintEnable(constraint, BoolToInt(enable));

      ApiExceptionCheck();
    }
    public void DestoryConstraints(IntPtr constraint) {
      if (_DestoryConstraints == null)
        throw new ApiNotFoundException("DestoryConstraints");

      _DestoryConstraints(constraint);

      ApiExceptionCheck();
    }
    public bool IsConstraintBroken(IntPtr constraint) {
      if (_IsConstraintBroken == null)
        throw new ApiNotFoundException("IsConstraintBroken");

      var rs = _IsConstraintBroken(constraint) > 0;

      ApiExceptionCheck();

      return rs;
    }
    public void SetRigidBodyInertiaTensor(IntPtr body, Matrix4x4 inertiaTensor) {
      if (_SetRigidBodyInertiaTensor == null)
        throw new ApiNotFoundException("SetRigidBodyInertiaTensor");

      IntPtr nPtr = Matrix4x4ToNative9(inertiaTensor);
      _SetRigidBodyInertiaTensor(body, nPtr);
      FreeNativeMatrix4x4(nPtr);

      ApiExceptionCheck();
    }
    public void SetRigidBodyLinearVelocity(IntPtr body, Vector3 velocity) {
      if (_SetRigidBodyLinearVelocity == null)
        throw new ApiNotFoundException("_SetRigidBodyLinearVelocity");

      IntPtr nVelocityPtr = Vector3ToNative3(velocity);
      _SetRigidBodyLinearVelocity(body, nVelocityPtr);
      FreeNativeVector3(nVelocityPtr);

      ApiExceptionCheck();
    }
    public void SetRigidBodyAngularVelocity(IntPtr body, Vector3 velocity) {
      if (_SetRigidBodyAngularVelocity == null)
        throw new ApiNotFoundException("_SetRigidBodyAngularVelocity");

      IntPtr nVelocityPtr = Vector3ToNative3(velocity);
      _SetRigidBodyAngularVelocity(body, nVelocityPtr);
      FreeNativeVector3(nVelocityPtr);

      ApiExceptionCheck();
    }
    public void RigidBodyApplyForce(IntPtr body, float delteTime, Vector3 force) {
      if (_RigidBodyApplyForce == null)
        throw new ApiNotFoundException("RigidBodyApplyForce");

      IntPtr forcePtr = Vector3ToNative3(force);
      _RigidBodyApplyForce(body, delteTime, forcePtr);
      FreeNativeVector3(forcePtr);

      ApiExceptionCheck();
    }
    public void RigidBodyApplyForceAtPoint(IntPtr body, float delteTime, Vector3 force, Vector3 point) {
      if (_RigidBodyApplyForceAtPoint == null)
        throw new ApiNotFoundException("RigidBodyApplyForceAtPoint");

      IntPtr forcePtr = Vector3ToNative3(force);
      IntPtr pointPtr = Vector3ToNative3(point);
      _RigidBodyApplyForceAtPoint(body, delteTime, forcePtr, pointPtr);
      FreeNativeVector3(forcePtr);
      FreeNativeVector3(pointPtr);

      ApiExceptionCheck();
    }
    public void RigidBodyApplyTorque(IntPtr body, float delteTime, Vector3 torque) {
      if (_RigidBodyApplyTorque == null)
        throw new ApiNotFoundException("RigidBodyApplyTorque");

      IntPtr forcePtr = Vector3ToNative3(torque);
      _RigidBodyApplyTorque(body, delteTime, forcePtr);
      FreeNativeVector3(forcePtr);

      ApiExceptionCheck();
    }
    public void RigidBodyApplyAngularImpulse(IntPtr body, Vector3 imp) {
      if (_RigidBodyApplyAngularImpulse == null)
        throw new ApiNotFoundException("RigidBodyApplyAngularImpulse");

      IntPtr forcePtr = Vector3ToNative3(imp);
      _RigidBodyApplyAngularImpulse(body, forcePtr);
      FreeNativeVector3(forcePtr);

      ApiExceptionCheck();
    }
    public void RigidBodyApplyLinearImpulse(IntPtr body, Vector3 imp) {
      if (_RigidBodyApplyForce == null)
        throw new ApiNotFoundException("RigidBodyApplyLinearImpulse");

      IntPtr forcePtr = Vector3ToNative3(imp);
      _RigidBodyApplyLinearImpulse(body, forcePtr);
      FreeNativeVector3(forcePtr);

      ApiExceptionCheck();
    }
    public void RigidBodyApplyPointImpulse(IntPtr body, Vector3 imp, Vector3 point) {
      if (_RigidBodyApplyPointImpulse == null)
        throw new ApiNotFoundException("RigidBodyApplyPointImpulse");

      IntPtr impPtr = Vector3ToNative3(imp);
      IntPtr pointPtr = Vector3ToNative3(point);
      _RigidBodyApplyPointImpulse(body, impPtr, pointPtr);
      FreeNativeVector3(impPtr);
      FreeNativeVector3(pointPtr);

      ApiExceptionCheck();
    }
    public void GetRigidBodyPosition(IntPtr body, out Vector3 outPos) {
      if (_GetRigidBodyPosition == null)
        throw new ApiNotFoundException("GetRigidBodyPosition");
      
      IntPtr ptr = Vector3ToNative3(Vector3.zero);
      _GetRigidBodyPosition(body, ptr);
      outPos = sVec3.FromNativeToVector3(ptr);
      FreeNativeVector3(ptr);

      ApiExceptionCheck();
    }
    public void GetRigidBodyRotation(IntPtr body, out Vector4 outRot) {
      if (_GetRigidBodyRotation == null)
        throw new ApiNotFoundException("GetRigidBodyRotation");
      
      IntPtr ptr = Vector4ToNative4(Vector4.zero);
      _GetRigidBodyRotation(body, ptr);
      outRot = sVec4.FromNativeToVector4(ptr);
      FreeNativeVector4(ptr);

      ApiExceptionCheck();
    }
    public void GetRigidBodyAngularVelocity(IntPtr body, out Vector3 outVelocity) {
      if (_GetRigidBodyAngularVelocity == null)
        throw new ApiNotFoundException("GetRigidBodyAngularVelocity");
      
      IntPtr ptr = Vector3ToNative3(Vector3.zero);
      _GetRigidBodyAngularVelocity(body, ptr);
      outVelocity = sVec3.FromNativeToVector3(ptr);
      FreeNativeVector3(ptr);

      ApiExceptionCheck();
    }
    public void GetRigidBodyLinearVelocity(IntPtr body, out Vector3 outVelocity) {
      if (_GetRigidBodyLinearVelocity == null)
        throw new ApiNotFoundException("GetRigidBodyLinearVelocity");
      
      IntPtr ptr = Vector3ToNative3(Vector3.zero);
      _GetRigidBodyLinearVelocity(body, ptr);
      outVelocity = sVec3.FromNativeToVector3(ptr);
      FreeNativeVector3(ptr);

      ApiExceptionCheck();
    }
    public void GetRigidBodyCenterOfMassInWorld(IntPtr body, out Vector3 outCenterOfMassInWorld) {
      if (_GetRigidBodyCenterOfMassInWorld == null)
        throw new ApiNotFoundException("GetRigidBodyCenterOfMassInWorld");
      
      IntPtr ptr = Vector3ToNative3(Vector3.zero);
      _GetRigidBodyCenterOfMassInWorld(body, ptr);
      outCenterOfMassInWorld = sVec3.FromNativeToVector3(ptr);
      FreeNativeVector3(ptr);

      ApiExceptionCheck();
    }
    public void GetRigidBodyPointVelocity(IntPtr body, Vector3 pt, out Vector3 ououtVelocity) {
      if (_GetRigidBodyPointVelocity == null)
        throw new ApiNotFoundException("GetRigidBodyPointVelocity");
      
      IntPtr nPtPtr = Vector3ToNative3(pt);
      IntPtr ptr = Vector3ToNative3(Vector3.zero);
      _GetRigidBodyPointVelocity(body, nPtPtr, ptr);
      ououtVelocity = sVec3.FromNativeToVector3(ptr);
      FreeNativeVector3(ptr);
      FreeNativeVector3(nPtPtr);

      ApiExceptionCheck();
    }
    public void RigidBodyResetCenterOfMass(IntPtr body) {
      if (_RigidBodyResetCenterOfMass == null)
        throw new ApiNotFoundException("RigidBodyResetCenterOfMass");

      _RigidBodyResetCenterOfMass(body);

      ApiExceptionCheck();
    }
    public void RigidBodyResetInertiaTensor(IntPtr body) {
      if (_RigidBodyResetInertiaTensor == null)
        throw new ApiNotFoundException("RigidBodyResetInertiaTensor");

      _RigidBodyResetInertiaTensor(body);

      ApiExceptionCheck();
    }

    public void CommonDelete(IntPtr ptr)
    {
      if (_CommonDelete == null)
        throw new ApiNotFoundException("CommonDelete");
      if (ptr == IntPtr.Zero)
        throw new ApiException("ptr is nullptr");

      _CommonDelete(ptr);
    }
    public IntPtr CreateTransform(float px, float py, float pz, float rx, float ry, float rz, float rw, float sx, float sy, float sz) {
      if (_CreateTransform == null)
        throw new ApiNotFoundException("CreateTransform");
      return _CreateTransform( px, py, pz, rx, ry, rz, rw, sx, sy, sz);
    }
    public void DestroyTransform(IntPtr ptr)
    {
      if (_DestroyTransform == null)
        throw new ApiNotFoundException("DestroyTransform");
      _DestroyTransform(ptr);
    }

    public void ActiveRigidBody(IntPtr ptr)
    {
      if (_ActiveRigidBody== null)
        throw new ApiNotFoundException("ActiveRigidBody");
      _ActiveRigidBody(ptr);
    }
    public void DeactiveRigidBody(IntPtr ptr)
    {
      if (_DeactiveRigidBody == null)
        throw new ApiNotFoundException("DeactiveRigidBody");
      _DeactiveRigidBody(ptr);
    }
    public IntPtr CreateRigidBody(IntPtr world, IntPtr shape, Vector3 position, Quaternion rot, int motionType, int qualityType, float friction, 
      float restitution, float mass, int active, int layer, bool isTiggerVolume, bool addContactListener, float gravityFactor, float linearDamping, float angularDamping, 
      Vector3 centerOfMass, Matrix4x4 inertiaTensor, Vector3 linearVelocity, Vector3 angularVelocity, float maxLinearVelocity, float maxAngularVelocity, IntPtr massProperties)
    {
      if (_CreateRigidBody == null)
        throw new ApiNotFoundException("CreateRigidBody");

      var nPtrPosition = Vector3ToNative3(position);
      var nPtrRot = QuaternionToNative4(rot);
      var nPtrInertiaTensor = inertiaTensor == Matrix4x4.identity ? IntPtr.Zero : Matrix4x4ToNative9(inertiaTensor);
      var nPtrCenterOfMass = Vector3ToNative3(centerOfMass);
      var nPtrLinearVelocity = Vector3ToNative3(linearVelocity);
      var nPtrAngularVelocity = Vector3ToNative3(angularVelocity);

      var rs = _CreateRigidBody( 
        world, shape, nPtrPosition, nPtrRot, motionType, qualityType,
        friction, restitution, mass, active, layer, BoolToInt(isTiggerVolume), BoolToInt(addContactListener), gravityFactor, linearDamping, 
        angularDamping, nPtrCenterOfMass, nPtrInertiaTensor, nPtrLinearVelocity, nPtrAngularVelocity, 
        maxLinearVelocity, maxAngularVelocity,
        massProperties);
      
      if(nPtrInertiaTensor != IntPtr.Zero)
        FreeNativeMatrix4x4(nPtrInertiaTensor);
      FreeNativeVector3(nPtrPosition);
      FreeNativeVector4(nPtrRot);
      FreeNativeVector3(nPtrCenterOfMass);
      FreeNativeVector3(nPtrLinearVelocity);
      FreeNativeVector3(nPtrAngularVelocity);
      
      ApiExceptionCheck();

      return rs;
    }
    public void SetRigidBodyMass(IntPtr body, float mass)
    {
      if (_SetRigidBodyMass == null)
        throw new ApiNotFoundException("SetRigidBodyMass");

      _SetRigidBodyMass(body, mass);

      ApiExceptionCheck();
    }
    public void SetRigidBodyFriction(IntPtr body, float friction)
    {
      if (_SetRigidBodyFriction == null)
        throw new ApiNotFoundException("SetRigidBodyFriction");

      _SetRigidBodyFriction(body, friction);

      ApiExceptionCheck();
    }
    public void SetRigidBodyRestitution(IntPtr body, float restitution)
    {
      if (_SetRigidBodyFriction == null)
        throw new ApiNotFoundException("SetRigidBodyFriction");

      _SetRigidBodyFriction(body, restitution);

      ApiExceptionCheck();
    }
    public void SetRigidBodyAngularDamping(IntPtr body, float angularDamping)
    {
      if (_SetRigidBodyAngularDamping == null)
        throw new ApiNotFoundException("SetRigidBodyAngularDamping");

      _SetRigidBodyAngularDamping(body, angularDamping);

      ApiExceptionCheck();
    }
    public void SetRigidBodyLinearDampin(IntPtr body, float linearDamping)
    {
      if (_SetRigidBodyLinearDampin == null)
        throw new ApiNotFoundException("SetRigidBodyLinearDampin");

      _SetRigidBodyLinearDampin(body, linearDamping);

      ApiExceptionCheck();
    }
    public void SetRigidBodyCenterOfMass(IntPtr body, Vector3 centerOfMass)
    {
      if (_SetRigidBodyCenterOfMass == null)
        throw new ApiNotFoundException("SetRigidBodyCenterOfMass");

      var nPtrCenterOfMass = Vector3ToNative3(centerOfMass);
      _SetRigidBodyCenterOfMass(body, nPtrCenterOfMass);
      FreeNativeVector3(nPtrCenterOfMass);

      ApiExceptionCheck();
    }
    public void SetRigidBodyPosition(IntPtr body, Vector3 pos)
    {
      if (_SetRigidBodyPosition == null)
        throw new ApiNotFoundException("SetRigidBodyPosition");

      var nPtrPos = Vector3ToNative3(pos);
      _SetRigidBodyPosition(body, nPtrPos);
      FreeNativeVector3(nPtrPos);

      ApiExceptionCheck();
    }
    public void SetRigidBodyPositionAndRotation(IntPtr body, Vector3 pos, Quaternion roate)
    {
      if (_SetRigidBodyPositionAndRotation == null)
        throw new ApiNotFoundException("SetRigidBodyPositionAndRotation");

      var nPtrPos = Vector3ToNative3(pos);
      var nPtrRot = QuaternionToNative4(roate);
      _SetRigidBodyPositionAndRotation(body, nPtrPos, nPtrRot);
      FreeNativeVector3(nPtrPos);
      FreeNativeVector4(nPtrRot);

      ApiExceptionCheck();
    }
    public void SetRigidBodyMotionType(IntPtr body, int newState)
    {
      if (_SetRigidBodyMotionType == null)
        throw new ApiNotFoundException("SetRigidBodyMotionType");

      _SetRigidBodyMotionType(body, newState);

      ApiExceptionCheck();
    }
    public void SetRigidBodyLayer(IntPtr body, int layer) 
    {
      if (_SetRigidBodyLayer == null)
        throw new ApiNotFoundException("SetRigidBodyLayer");

      _SetRigidBodyLayer(body, layer);

      ApiExceptionCheck();
    }
    public void SetRigidBodyMaxLinearVelocity(IntPtr body, float v)
    {
      if (_SetRigidBodyMaxLinearVelocity == null)
        throw new ApiNotFoundException("SetRigidBodyMaxLinearVelocity");

      _SetRigidBodyMaxLinearVelocity(body, v);

      ApiExceptionCheck();
    }
    public void SetRigidBodyMaxAngularVelocity(IntPtr body, float v)
    {
      if (_SetRigidBodyMaxAngularVelocity == null)
        throw new ApiNotFoundException("SetRigidBodyMaxAngularVelocity");

      _SetRigidBodyMaxAngularVelocity(body, v);

      ApiExceptionCheck();
    }   
    public void DestroyRigidBody(IntPtr body)
    {
      if (_DestroyRigidBody == null)
        throw new ApiNotFoundException("DestroyRigidBody");

      _DestroyRigidBody(body);

      ApiExceptionCheck();
    }
    public void SetRigidBodyGravityFactor(IntPtr body, float gravityFactor)
    {
      if (_SetRigidBodyGravityFactor == null)
        throw new ApiNotFoundException("SetRigidBodyGravityFactor");

      _SetRigidBodyGravityFactor(body, gravityFactor);

      ApiExceptionCheck();
    }
    public void GetConvexHullResultTriangles(IntPtr result, IntPtr trianglesBuffer, int count)
    {
      if (_GetConvexHullResultTriangles == null)
        throw new ApiNotFoundException("GetConvexHullResultTriangles");

      _GetConvexHullResultTriangles(result, trianglesBuffer, count);

      ApiExceptionCheck();
    }
    public void GetConvexHullResultVertices(IntPtr result, IntPtr pointsBuffer, int numPoints)
    {
      if (_GetConvexHullResultVertices == null)
        throw new ApiNotFoundException("GetConvexHullResultVertices");

      _GetConvexHullResultVertices(result, pointsBuffer, numPoints);

      ApiExceptionCheck();
    }
    public IntPtr Build3DPointsConvexHull(Vector3[] points)
    {
      if (_Build3DPointsConvexHull == null)
        throw new ApiNotFoundException("Build3DPointsConvexHull");
      
      float[] verticesArr = new float[points.Length * 3];
      for (int i = 0; i < points.Length; i++)
      {
          verticesArr[i * 3 + 0] = points[i].x;
          verticesArr[i * 3 + 1] = points[i].y;
          verticesArr[i * 3 + 2] = points[i].z;
      }
      
      int bufferSize = Marshal.SizeOf<float>() * verticesArr.Length;
      IntPtr verticesBuffer = Marshal.AllocHGlobal(bufferSize);
      Marshal.Copy(verticesArr, 0, verticesBuffer, verticesArr.Length);

      var rs = _Build3DPointsConvexHull(verticesBuffer, points.Length);
      
      Marshal.FreeHGlobal(verticesBuffer);

      ApiExceptionCheck();

      return rs;
    }
    public IntPtr Build3DFromPlaneConvexHull(IntPtr panels, int numPanels)
    {
      if (_Build3DFromPlaneConvexHull == null)
        throw new ApiNotFoundException("Build3DFromPlaneConvexHull");

      var rs = _Build3DFromPlaneConvexHull(panels, numPanels);
      ApiExceptionCheck();

      return rs;
    }
    public IntPtr ComputeShapeVolumeMassProperties(IntPtr shape, float mass)
    {
      if (_ComputeShapeVolumeMassProperties == null)
        throw new ApiNotFoundException("ComputeShapeVolumeMassProperties");

      var rs = _ComputeShapeVolumeMassProperties(shape, mass);
      ApiExceptionCheck();

      return rs;
    }
    public IntPtr ComputeBoxSurfaceMassProperties(Vector3 halfExtents, float mass, float surfaceThickness)
    {
      if (_ComputeBoxSurfaceMassProperties == null)
        throw new ApiNotFoundException("ComputeBoxSurfaceMassProperties");

      var p0 = Vector3ToNative3(halfExtents);
      var rs = _ComputeBoxSurfaceMassProperties(p0, mass, surfaceThickness);
      FreeNativeVector3(p0);

      ApiExceptionCheck();

      return rs;
    }
    public IntPtr ComputeBoxVolumeMassProperties(Vector3 halfExtents, float mass)
    {
      if (_ComputeBoxVolumeMassProperties == null)
        throw new ApiNotFoundException("ComputeBoxVolumeMassProperties");

      var p0 = Vector3ToNative3(halfExtents);
      var rs = _ComputeBoxVolumeMassProperties(p0, mass);
      FreeNativeVector3(p0);
      ApiExceptionCheck();

      return rs;
    }
    public IntPtr ComputeCapsuleVolumeMassProperties(Vector3 startAxis, Vector3 endAxis, float radius, float mass)
    {
      if (_ComputeCapsuleVolumeMassProperties == null)
        throw new ApiNotFoundException("ComputeCapsuleVolumeMassProperties");

      var pStartAxis = Vector3ToNative3(startAxis);
      var pEndAxis = Vector3ToNative3(endAxis);

      var rs = _ComputeCapsuleVolumeMassProperties(pStartAxis, pEndAxis, radius, mass);

      FreeNativeVector3(pStartAxis);
      FreeNativeVector3(pEndAxis);

      ApiExceptionCheck();

      return rs;
    }
    public IntPtr ComputeCylinderVolumeMassProperties(Vector3 startAxis, Vector3 endAxis, float radius, float mass)
    {
      if (_ComputeCylinderVolumeMassProperties == null)
        throw new ApiNotFoundException("ComputeCylinderVolumeMassProperties");

      var pStartAxis = Vector3ToNative3(startAxis);
      var pEndAxis = Vector3ToNative3(endAxis);

      var rs = _ComputeCylinderVolumeMassProperties(pStartAxis, pEndAxis, radius, mass);

      FreeNativeVector3(pStartAxis);
      FreeNativeVector3(pEndAxis);

      ApiExceptionCheck();

      return rs;
    }
    public IntPtr ComputeSphereVolumeMassProperties(float radius, float sphereMass)
    {
      if (_ComputeSphereVolumeMassProperties == null)
        throw new ApiNotFoundException("ComputeSphereVolumeMassProperties");

      var rs = _ComputeSphereVolumeMassProperties(radius, sphereMass);
      ApiExceptionCheck();

      return rs;
    }
    public IntPtr ComputeSphereSurfaceMassProperties(float radius, float mass, float surfaceThickness)
    {
      if (_ComputeSphereSurfaceMassProperties == null)
        throw new ApiNotFoundException("ComputeSphereSurfaceMassProperties");

      var rs = _ComputeSphereSurfaceMassProperties(radius, mass, surfaceThickness);
      ApiExceptionCheck();

      return rs;
    }
    public IntPtr ComputeTriangleSurfaceMassProperties(Vector3 v0, Vector3 v1, Vector3 v2, float mass, float surfaceThickness)
    {
      if (_ComputeTriangleSurfaceMassProperties == null)
        throw new ApiNotFoundException("ComputeTriangleSurfaceMassProperties");

      var p0 = Vector3ToNative3(v0);
      var p1 = Vector3ToNative3(v1);
      var p2 = Vector3ToNative3(v2);

      var rs = _ComputeTriangleSurfaceMassProperties(p0, p1, p2, mass, surfaceThickness);

      FreeNativeVector3(p0);
      FreeNativeVector3(p1);
      FreeNativeVector3(p2);

      ApiExceptionCheck();

      return rs;
    }
    public IntPtr CreateBoxShape(Vector3 boxSize, float radius)
    {
      if (_CreateBoxShape == null)
        throw new ApiNotFoundException("CreateBoxShape");

      var p0 = Vector3ToNative3(boxSize);

      var rs = _CreateBoxShape(p0, radius);

      FreeNativeVector3(p0);

      ApiExceptionCheck();

      return rs;
    }
    public IntPtr CreateSphereShape(float radius)
    {
      if (_CreateSphereShape == null)
        throw new ApiNotFoundException("CreateSphereShape");

      var rs = _CreateSphereShape(radius);
      ApiExceptionCheck();

      return rs;
    }
    public IntPtr CreateCapsuleShape(Vector3 start, Vector3 end, float radius)
    {
      if (_CreateCapsuleShape == null)
        throw new ApiNotFoundException("CreateCapsuleShape");

      var pStart = Vector3ToNative3(start);
      var pEnd = Vector3ToNative3(end);

      var rs = _CreateCapsuleShape(pStart, pEnd, radius);

      FreeNativeVector3(pStart);
      FreeNativeVector3(pEnd);

      ApiExceptionCheck();

      return rs;
    }
    public IntPtr CreateCylindeShape(Vector3 start, Vector3 end, float radius, float paddingRadius)
    {
      if (_CreateCylindeShape == null)
        throw new ApiNotFoundException("CreateCylindeShape");

      var pStart = Vector3ToNative3(start);
      var pEnd = Vector3ToNative3(end);

      var rs = _CreateCylindeShape(pStart, pEnd, radius, paddingRadius);

      FreeNativeVector3(pStart);
      FreeNativeVector3(pEnd);

      ApiExceptionCheck();

      return rs;
    }
    public IntPtr CreateTriangleShape(Vector3 v0, Vector3 v1, Vector3 v2)
    {
      if (_CreateTriangleShape == null)
        throw new ApiNotFoundException("CreateTriangleShape");
      
      var p0 = Vector3ToNative3(v0);
      var p1 = Vector3ToNative3(v1);
      var p2 = Vector3ToNative3(v2);

      var rs = _CreateTriangleShape(p0, p1, p2);

      FreeNativeVector3(p0);
      FreeNativeVector3(p1);
      FreeNativeVector3(p2);

      ApiExceptionCheck();

      return rs;
    }
    public IntPtr CreateConvexVerticesShape(Vector3[] vertices, float convexRadius)
    {
      if (_CreateConvexVerticesShape == null)
        throw new ApiNotFoundException("CreateConvexVerticesShape");

      float[] verticesArr = new float[vertices.Length * 3];
      for (int i = 0; i < vertices.Length; i++)
      {
          verticesArr[i * 3 + 0] = vertices[i].x;
          verticesArr[i * 3 + 1] = vertices[i].y;
          verticesArr[i * 3 + 2] = vertices[i].z;
      }
      
      int bufferSize = Marshal.SizeOf<float>() * verticesArr.Length;
      IntPtr verticesBuffer = Marshal.AllocHGlobal(bufferSize);
      Marshal.Copy(verticesArr, 0, verticesBuffer, verticesArr.Length);

      var rs = _CreateConvexVerticesShape(verticesBuffer, verticesArr.Length, convexRadius);

      Marshal.FreeHGlobal(verticesBuffer);

      ApiExceptionCheck();

      return rs;
    }
    public IntPtr CreateConvexVerticesShapeByConvexHullResult(IntPtr result, float convexRadius)
    {
      if (_CreateConvexVerticesShapeByConvexHullResult == null)
        throw new ApiNotFoundException("CreateConvexVerticesShapeByConvexHullResult");

      var rs = _CreateConvexVerticesShapeByConvexHullResult(result, convexRadius);
      ApiExceptionCheck();

      return rs;
    }
    public IntPtr CreateSimpleMeshShape(Vector3[] vertices, int[] triangles, float convexRadius) {
      if (_CreateSimpleMeshShape == null)
        throw new ApiNotFoundException("CreateSimpleMeshShape");
      
      float[] verticesArr = new float[vertices.Length * 3];
      for (int i = 0; i < vertices.Length; i++)
      {
          verticesArr[i * 3 + 0] = vertices[i].x;
          verticesArr[i * 3 + 1] = vertices[i].y;
          verticesArr[i * 3 + 2] = vertices[i].z;
      }
      
      int bufferSize = Marshal.SizeOf<float>() * verticesArr.Length;
      IntPtr verticesBuffer = Marshal.AllocHGlobal(bufferSize);
      Marshal.Copy(verticesArr, 0, verticesBuffer, verticesArr.Length);

      bufferSize = Marshal.SizeOf<int>() * triangles.Length;
      IntPtr trianglesBuffer = Marshal.AllocHGlobal(bufferSize);
      Marshal.Copy(triangles, 0, trianglesBuffer, triangles.Length);

      var rs = _CreateSimpleMeshShape(verticesBuffer, verticesArr.Length, trianglesBuffer, triangles.Length, convexRadius);

      Marshal.FreeHGlobal(trianglesBuffer);
      Marshal.FreeHGlobal(verticesBuffer);

      ApiExceptionCheck();
      return rs;
    }
    public IntPtr CreateConvexTranslateShape(IntPtr child, Vector3 translation)
    {
      if (_CreateConvexTranslateShape == null)
        throw new ApiNotFoundException("CreateConvexTranslateShape");

      var ptr = Vector3ToNative3(translation);

      var rs = _CreateConvexTranslateShape(child, ptr);

      FreeNativeVector3(ptr);

      ApiExceptionCheck();

      return rs;
    }
    public IntPtr CreateConvexTransformShape(IntPtr child, IntPtr transform)
    {
      if (_CreateConvexTransformShape == null)
        throw new ApiNotFoundException("CreateConvexTransformShape");

      var rs = _CreateConvexTransformShape(child, transform);
      ApiExceptionCheck();

      return rs;
    }
    public IntPtr CreateListShape(IntPtr childs, int childCount)
    {
      if (_CreateListShape == null)
        throw new ApiNotFoundException("CreateListShape");

      var rs = _CreateListShape(childs, childCount);
      ApiExceptionCheck();

      return rs;
    }
    public IntPtr CreateStaticCompoundShape(IntPtr childs, IntPtr transforms, int childCount)
    {
      if (_CreateStaticCompoundShape == null)
        throw new ApiNotFoundException("CreateStaticCompoundShape");

      var rs = _CreateStaticCompoundShape(childs, transforms, childCount);
      ApiExceptionCheck();

      return rs;
    }
    public void StaticCompoundShapeSetInstanceEnabled(IntPtr pStaticCompoundShape, int id, int enabled)
    {
      if (_StaticCompoundShapeSetInstanceEnabled == null)
        throw new ApiNotFoundException("StaticCompoundShapeSetInstanceEnabled");

      _StaticCompoundShapeSetInstanceEnabled(pStaticCompoundShape, id, enabled);

      ApiExceptionCheck();
    }
    public int StaticCompoundShapeIsInstanceEnabled(IntPtr pStaticCompoundShape, int id)
    {
      if (_StaticCompoundShapeIsInstanceEnabled == null)
        throw new ApiNotFoundException("StaticCompoundShapeIsInstanceEnabled");

      var rs = _StaticCompoundShapeIsInstanceEnabled(pStaticCompoundShape, id);
      ApiExceptionCheck();

      return rs;
    }
    public void StaticCompoundShapeEnableAllInstancesAndShapeKeys(IntPtr pStaticCompoundShape)
    {
      if (_StaticCompoundShapeEnableAllInstancesAndShapeKeys == null)
        throw new ApiNotFoundException("StaticCompoundShapeEnableAllInstancesAndShapeKeys");

      _StaticCompoundShapeEnableAllInstancesAndShapeKeys(pStaticCompoundShape);

      ApiExceptionCheck();
    }
    public void DestroyShape(IntPtr s)
    {
      if (_DestroyShape == null)
        throw new ApiNotFoundException("DestroyShape");

      _DestroyShape(s);

      ApiExceptionCheck();
    }
    public IntPtr CreatePhysicsWorld(Vector3 gravity, int solverIterationCount, float broadPhaseWorldSize, float collisionTolerance,
      bool bContinuous, bool bVisualDebugger, uint layerMask, uint[] layerToMask, bool stableSolverOn,
      fnOnConstraintBreakingCallback onConstraintBreakingCallback, fnOnBodyTriggerEventCallback onBodyTriggerEventCallback, fnOnBodyContactEventCallback onBodyContactEventCallback)
    {
      if (_CreatePhysicsWorld == null)
        throw new ApiNotFoundException("CreatePhysicsWorld");

      //uint[] to native
      IntPtr layerToMaskPtr = Marshal.AllocHGlobal(Marshal.SizeOf<int>() * 32);
      IntPtr layerToMaskPtr2 = new IntPtr(layerToMaskPtr.ToInt64());
      for(int i = 0; i < layerToMask.Length && i < 32; i++){
        Marshal.WriteInt32(layerToMaskPtr, (int)layerToMask[i]);
        layerToMaskPtr2 = new IntPtr(layerToMaskPtr2.ToInt64() + i);
      }

      var pGravity = Vector3ToNative3(gravity);

      var onConstraintBreakingCallbackPtr = Marshal.GetFunctionPointerForDelegate(onConstraintBreakingCallback);
      var onBodyTriggerEnterCallbackPtr = Marshal.GetFunctionPointerForDelegate(onBodyTriggerEventCallback);
      var onBodyTriggerLeaveCallbackPtr = Marshal.GetFunctionPointerForDelegate(onBodyContactEventCallback);

      var rs = _CreatePhysicsWorld(pGravity, solverIterationCount, broadPhaseWorldSize, collisionTolerance, bContinuous, bVisualDebugger, layerMask, layerToMaskPtr, BoolToInt(stableSolverOn),
        onConstraintBreakingCallbackPtr, onBodyTriggerEnterCallbackPtr, onBodyTriggerLeaveCallbackPtr);

      FreeNativeVector4(pGravity);
      Marshal.FreeHGlobal(layerToMaskPtr);

      ApiExceptionCheck();

      return rs;
    }
    public void DestroyPhysicsWorld(IntPtr world)
    {
      if (_DestroyPhysicsWorld == null)
        throw new ApiNotFoundException("DestroyPhysicsWorld");

      _DestroyPhysicsWorld(world);

      ApiExceptionCheck();
    }
    public void StepPhysicsWorld(IntPtr world, float timestep)
    {
      if (_StepPhysicsWorld == null)
        throw new ApiNotFoundException("StepPhysicsWorld");
      _StepPhysicsWorld(world, timestep);

      ApiExceptionCheck();
    }
    public void SetPhysicsWorldGravity(IntPtr world, Vector3 gravity)
    {
      if (_SetPhysicsWorldGravity == null)
        throw new ApiNotFoundException("SetPhysicsWorldGravity");

      var pGravity = Vector3ToNative3(gravity);

      _SetPhysicsWorldGravity(world, pGravity);

      FreeNativeVector4(pGravity);

      ApiExceptionCheck();
    }
    public void SetPhysicsWorldCollisionLayerMasks(IntPtr world, uint layerId, uint toMask, int enable, int forceUpdate) {
      if (_SetPhysicsWorldCollisionLayerMasks == null)
        throw new ApiNotFoundException("SetPhysicsWorldCollisionLayerMasks");

      _SetPhysicsWorldCollisionLayerMasks(world, layerId, toMask, enable, forceUpdate);

      ApiExceptionCheck();
    }
    public int ReadPhysicsWorldBodys(IntPtr world, IntPtr buffer, int count)
    {
      if (_ReadPhysicsWorldBodys == null)
        throw new ApiNotFoundException("ReadPhysicsWorldBodys");

      var rs = _ReadPhysicsWorldBodys(world, buffer, count);

      ApiExceptionCheck();

      return rs;
    }
    public void TestAssert()
    {
      if (_TestAssert == null)
        throw new ApiNotFoundException("TestAssert");

      _TestAssert();

      ApiExceptionCheck();
    }   
  };
}