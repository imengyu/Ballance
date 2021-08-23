using System;
using UnityEngine;

namespace PhysicsRT {

    [AddComponentMenu("PhysicsRT/Constraints/StiffSpringConstraint")]
    [SLua.CustomLuaClass]
    public class StiffSpringConstraint : PhysicsConstraint {

        public Vector3 PovitAW;
        public Vector3 PovitBW;
        public float SpringMin;
        public float SpringMax;

        public override void Create() {
            if(ConnectedBody == null) 
                throw new Exception("ConnectedBody is null");
            var ptr = CreatePre();
            var otherPtr = ConnectedBody.GetPtr();
            if(ptr == IntPtr.Zero)
                throw new Exception("This body hasn't been created yet");
            if(otherPtr == IntPtr.Zero)
                throw new Exception("ConnectedBody hasn't been created yet");
            CreateLastStep(PhysicsApi.API.CreateStiffSpringConstraint(ptr, otherPtr, (PovitAW), (PovitBW), SpringMin, SpringMax, GetConstraintBreakData()));
        }
    }
}