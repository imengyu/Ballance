using System;
using UnityEngine;

namespace PhysicsRT {

    [AddComponentMenu("PhysicsRT/Constraints/CogWheelConstraint")]
    [SLua.CustomLuaClass]
    public class CogWheelConstraint : PhysicsConstraint {

        public Vector3 rotationPivotA;
        public Vector3 rotationAxisA = Vector3.forward;
        public float radiusA;
        public Vector3 rotationPivotB;
        public Vector3 rotationAxisB = Vector3.forward;
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
                rotationPivotA, rotationAxisA, radiusA,
                rotationPivotB, rotationAxisB, radiusB,
                GetConstraintBreakData()));
        }
    }
}