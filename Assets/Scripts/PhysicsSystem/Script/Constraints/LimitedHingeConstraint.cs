using System;
using UnityEngine;

namespace PhysicsRT {

    [AddComponentMenu("PhysicsRT/Constraints/LimitedHingeConstraint")]
    [SLua.CustomLuaClass]
    public class LimitedHingeConstraint : MotorConstraint {
 
        public GameObject PovitRef;
        public GameObject AxisRef;
        public float AgularLimitMin = 0;
        public float AgularLimitMax = 360;
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
            CreateLastStep(PhysicsApi.API.CreateLimitedHingeConstraint(ptr, otherPtr,
                 PovitRef.transform.position, AxisRef.transform.forward.normalized, 
                (AgularLimitMin) * Mathf.Deg2Rad, 
                (AgularLimitMax) * Mathf.Deg2Rad, 
                GetConstraintBreakData(), GetConstraintMotorData(), Priority, Stabilized));
        }
    }
}