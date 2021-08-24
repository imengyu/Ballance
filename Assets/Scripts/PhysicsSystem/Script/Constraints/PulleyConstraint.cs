using System;
using UnityEngine;

namespace PhysicsRT {

    [AddComponentMenu("PhysicsRT/Constraints/PulleyConstraint")]
    [SLua.CustomLuaClass]
    public class PulleyConstraint : PhysicsConstraint {

        public GameObject pivotAWRef;
        public GameObject pivotBWRef;
        public GameObject pulleyPivotAWRef;
        public GameObject pulleyPivotBWRef;
        public float leverageRatio = 0;

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
            CreateLastStep(PhysicsApi.API.CreatePulleyConstraint(ptr, otherPtr, 
                pivotAWRef.transform.position, pivotBWRef.transform.position,
                pulleyPivotAWRef.transform.position, pulleyPivotBWRef.transform.position, 
                leverageRatio, GetConstraintBreakData()));
        }
    }
}