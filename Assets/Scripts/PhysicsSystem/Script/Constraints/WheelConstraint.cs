using System;
using UnityEngine;

namespace PhysicsRT {

    [AddComponentMenu("PhysicsRT/Constraints/WheelConstraint")]
    [SLua.CustomLuaClass]
    public class WheelConstraint : PhysicsConstraint {

        public Vector3 Povit;
        public Vector3 Axle;
        public Vector3 Suspension;
        public Vector3 Steering;
        public float SuspensionLimitMin;
        public float SuspensionLimitMax; 
        public float SuspensionStrength;
        public float SuspensionDamping;

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
            CreateLastStep(PhysicsApi.API.CreateWheelConstraint(ptr, otherPtr, (Povit), Axle, Suspension,
                Steering, SuspensionLimitMin, SuspensionLimitMax, SuspensionStrength, SuspensionDamping, GetConstraintBreakData(), Priority));
        }
    }
}