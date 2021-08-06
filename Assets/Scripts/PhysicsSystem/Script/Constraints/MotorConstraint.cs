using System;
using UnityEngine;

namespace PhysicsRT {

    public class MotorConstraint : PhysicsConstraint {

        [HideInInspector]
        public bool motorEnable = false;
        [HideInInspector]
        public bool motorSpring = false;
        [HideInInspector]
        public float motorTau = 0.2f;
        [HideInInspector]
        public float motorDamping = 0.2f;
        [HideInInspector]
        public float motorProportionalRecoveryVelocity = 0;
        [HideInInspector]
        public float motorConstantRecoveryVelocity = 0;
        [HideInInspector]
        public float motorMinForce = -1e6f;
        [HideInInspector]
        public float motorMaxForce = 1e6f;
        [HideInInspector]
        public float motorSpringConstant = 0;
        [HideInInspector]
        public float motorSpringDamping = 0.3f;

        protected sConstraintMotorData GetConstraintMotorData() {
            sConstraintMotorData data = new sConstraintMotorData();
            data.enable = motorEnable;
            data.spring = motorSpring ? 1 : 0;
            data.m_tau = motorTau;
            data.m_damping = motorDamping;
            data.m_proportionalRecoveryVelocity = motorProportionalRecoveryVelocity;
            data.m_constantRecoveryVelocity = motorConstantRecoveryVelocity;
            data.m_minForce = motorMinForce;
            data.m_maxForce = motorMaxForce;
            data.m_springConstant = motorSpringConstant;
            data.m_springDamping = motorSpringDamping;
            return data;
        }
    
    }
}