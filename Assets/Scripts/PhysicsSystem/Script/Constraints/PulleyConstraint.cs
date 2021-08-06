using System;
using UnityEngine;

namespace PhysicsRT {

    [AddComponentMenu("PhysicsRT/Constraints/PulleyConstraint")]
    [SLua.CustomLuaClass]
    public class PulleyConstraint : PhysicsConstraint {

        public Vector3 pivotAW;
        public Vector3 pivotBW;
        public Vector3 pulleyPivotAW;
        public Vector3 pulleyPivotBW;
        public float leverageRatio = 0;

        public override void Create() {
            if(ConnectedBody == null) 
                throw new Exception("ConnectedBody is null");
            var ptr = CreatePre();
            var otherPtr = ConnectedBody.GetPtr();
            if(ptr == IntPtr.Zero)
                throw new Exception("This body hasn't been created yet");
            if(otherPtr == IntPtr.Zero)
                throw new Exception("ConnectedBody hasn't been created yet");
            CreateLastStep(PhysicsApi.API.CreatePulleyConstraint(ptr, otherPtr, 
                transform.TransformPoint(pivotAW), transform.TransformPoint(pivotBW),
                transform.TransformPoint(pulleyPivotAW), transform.TransformPoint(pulleyPivotBW), 
                leverageRatio, GetConstraintBreakData()));
        }
    }
}