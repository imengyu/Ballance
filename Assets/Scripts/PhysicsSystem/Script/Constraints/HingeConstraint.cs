using System;
using UnityEngine;

namespace PhysicsRT {

    [AddComponentMenu("PhysicsRT/Constraints/HingeConstraint")]
    [SLua.CustomLuaClass]
    public class HingeConstraint : PhysicsConstraint {

        public GameObject PovitRef;
        public GameObject AxisRef;
        public bool Stabilized;

        public override void Create() {
            var ptr = CreatePre();
            var otherPtr = IntPtr.Zero; 
            if(ConnectedBody != null) {
                otherPtr = ConnectedBody.GetPtr();
                if(otherPtr == IntPtr.Zero)
                    throw new Exception("ConnectedBody hasn't been created yet");
            }
            if(ptr == IntPtr.Zero)
                throw new Exception("This body hasn't been created yet");
            CreateLastStep(PhysicsApi.API.CreateHingeConstraint(ptr, otherPtr, PovitRef.transform.position, 
              AxisRef.transform.forward.normalized, GetConstraintBreakData(), Priority, Stabilized));
        }
    }
}