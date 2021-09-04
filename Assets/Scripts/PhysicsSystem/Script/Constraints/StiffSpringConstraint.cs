using System;
using UnityEngine;

namespace PhysicsRT {

    [AddComponentMenu("PhysicsRT/Constraints/StiffSpringConstraint")]
    [SLua.CustomLuaClass]
    public class StiffSpringConstraint : PhysicsConstraint {

        public GameObject PovitAWRef;
        public GameObject PovitBWRef;
        public float SpringMin;
        public float SpringMax;
        public bool Stabilized;

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
            CreateLastStep(PhysicsApi.API.CreateStiffSpringConstraint(ptr, otherPtr, PovitAWRef.transform.position, PovitBWRef.transform.position, 
                SpringMin, SpringMax, GetConstraintBreakData(), Priority, Stabilized));
        }
    }
}