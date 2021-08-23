using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace PhysicsRT
{  
    [AddComponentMenu("PhysicsRT/Physics Force")]
    [DefaultExecutionOrder(210)]
    [DisallowMultipleComponent]
    [SLua.CustomLuaClass]
    public class PhysicsForce : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("指定恒力的方向和大小")]
        public Vector3 Force;
        [SerializeField]
        [Tooltip("指定是否启用恒力")]
        public bool Enable;

        private PhysicsBody body;

        private void Start() {
            body = GetComponent<PhysicsBody>();
        }
        private void FixedUpdate() {
            if(Enable)
                body.ApplyForce(Force);
        }
    }

}
