using System;
using UnityEngine;

namespace PhysicsRT {

    [AddComponentMenu("PhysicsRT/Constraints/PrismaticConstraint")]
    [SLua.CustomLuaClass]
    public class PrismaticConstraint : MotorConstraint {
 
        public GameObject PovitRef;
        public GameObject AxisRef;
        public bool AllowRotationAroundAxis = false;
        public float MaxLinearLimit = 100;
        public float MinLinearLimit = 0;
        public float MaxFrictionForce = 1000;

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
            CreateLastStep(PhysicsApi.API.CreatePrismaticConstraint(ptr, otherPtr, PovitRef.transform.position, AxisRef.transform.forward.normalized, 
                AllowRotationAroundAxis, MaxLinearLimit, MinLinearLimit, MaxFrictionForce,
                GetConstraintBreakData(), GetConstraintMotorData()));
        }
    }
}