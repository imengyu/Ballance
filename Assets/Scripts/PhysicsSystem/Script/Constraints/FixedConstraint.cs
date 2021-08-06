using System;
using UnityEngine;

namespace PhysicsRT {

    [AddComponentMenu("PhysicsRT/Constraints/FixedConstraint")]
    [SLua.CustomLuaClass]
    public class FixedConstraint : PhysicsConstraint {

        public Vector3 Povit;

        public override void Create() {
            if(ConnectedBody == null) 
                throw new Exception("ConnectedBody is null");
            var ptr = CreatePre();
            var otherPtr = ConnectedBody.GetPtr();
            if(ptr == IntPtr.Zero)
                throw new Exception("This body hasn't been created yet");
            if(otherPtr == IntPtr.Zero)
                throw new Exception("ConnectedBody hasn't been created yet");
            CreateLastStep(PhysicsApi.API.CreateFixedConstraint(ptr, otherPtr, transform.TransformPoint(Povit), GetConstraintBreakData()));
        }
    }
}