using System;
using UnityEngine;

namespace PhysicsRT {

    [AddComponentMenu("PhysicsRT/Constraints/CogWheelConstraint")]
    [SLua.CustomLuaClass]
    public class CogWheelConstraint : PhysicsConstraint {

        public GameObject rotationPivotARef;
        public GameObject rotationAxisARef;
        public float radiusA;
        public GameObject rotationPivotBRef;
        public GameObject rotationAxisBRef;
        public float radiusB;

        public override void Create() {
            if(ConnectedBody == null) 
                throw new Exception("ConnectedBody is null");
            var ptr = CreatePre();
            var otherPtr = ConnectedBody.GetPtr();
            if(ptr == IntPtr.Zero)
                throw new Exception("This body hasn't been created yet");
            if(otherPtr == IntPtr.Zero)
                throw new Exception("ConnectedBody hasn't been created yet");
            CreateLastStep(PhysicsApi.API.CreateCogWheelConstraint(ptr, otherPtr, 
                rotationPivotARef.transform.position, rotationAxisARef.transform.forward.normalized, radiusA,
                rotationPivotBRef.transform.position, rotationAxisBRef.transform.forward.normalized, radiusB,
                GetConstraintBreakData()));
        }
    }
}