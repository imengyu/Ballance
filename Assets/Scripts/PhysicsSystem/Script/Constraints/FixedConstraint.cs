using System;
using UnityEngine;

namespace PhysicsRT {

    [AddComponentMenu("PhysicsRT/Constraints/FixedConstraint")]
    [SLua.CustomLuaClass]
    public class FixedConstraint : PhysicsConstraint {

        public GameObject PovitRef;

        public override void Create() {
            var ptr = CreatePre();
            if(ptr == IntPtr.Zero)
                throw new Exception("This body hasn't been created yet");
            var otherPtr = IntPtr.Zero; 
            if(ConnectedBody != null) {
                otherPtr = ConnectedBody.GetPtr();
                if(otherPtr == IntPtr.Zero)
                    throw new Exception("ConnectedBody hasn't been created yet");
            }
            CreateLastStep(PhysicsApi.API.CreateFixedConstraint(ptr, otherPtr, PovitRef.transform.position, GetConstraintBreakData()));
        }
    }
}