using System;
using System.Collections;
using UnityEngine;

namespace PhysicsRT
{
    [AddComponentMenu("PhysicsRT/Physics Spring")]
    [DefaultExecutionOrder(250)]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(PhysicsBody))]
    [SLua.CustomLuaClass]
    public class PhysicsSpring : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("获取弹簧锚点A (世界坐标)")]
        private Vector3 m_PovitA = new Vector3(0.0f,-0.5f,0.0f);
        [SerializeField]
        [Tooltip("获取弹簧锚点B (世界坐标)")]
        private Vector3 m_PovitB = new Vector3(0.0f,0.5f,0.0f);
        [Tooltip("在 Awake 时不自动创建，设置为 false 后您需要手动调用 ForceReCreate 来创建")]
        [SerializeField]
        private bool m_DoNotAutoCreateAtAwake = false;
        private IntPtr ptr = IntPtr.Zero;

        public float springConstant = 1000.0f;
        public float springDamping = 0.1f;
        public float springRestLength = 0.0f;
        [SerializeField]
        public PhysicsBody ConnectedBody;

        /// <summary>
        /// 在 Awake 时不自动创建，设置为 false 后您需要手动调用 ForceReCreate 来创建
        /// </summary>
        public bool DoNotAutoCreateAtAwake { get => m_DoNotAutoCreateAtAwake; set { m_DoNotAutoCreateAtAwake = value; } }
        
        /// <summary>
        /// 获取弹簧锚点A (世界坐标)
        /// </summary>
        /// <value></value>
        public Vector3 PovitA { get => m_PovitA; set => m_PovitA = value; }
        /// <summary>
        /// 获取弹簧锚点B (世界坐标)
        /// </summary>
        /// <value></value>
        public Vector3 PovitB { get => m_PovitB; set => m_PovitB = value; }

        /// <summary>
        /// 重新创建弹簧
        /// </summary>
        public void ForceReCreate()
        {
            Destroy();
            Create();
        }

        private void OnDestroy() {
            Destroy();
        }
        
        private PhysicsWorld CurrentPhysicsWorld = null;

        public IntPtr GetPtr() { return ptr; }

        public void Create() {
            if(ptr != IntPtr.Zero) {
                return;
            }
            if(CurrentPhysicsWorld == null) {
                Debug.LogWarning("Not found PhysicsWorld in this scense, please add it before use PhysicsBody.");
                return;
            }

            ptr = PhysicsApi.API.CreateSpringAction(
                CurrentPhysicsWorld.GetPtr(),
                GetComponent<PhysicsBody>().GetPtr(),
                ConnectedBody != null ? ConnectedBody.GetPtr() : IntPtr.Zero,
                m_PovitA,
                m_PovitB,
                springConstant,
                springDamping,
                springRestLength);
        }
        public void Destroy() {
            if(CurrentPhysicsWorld == null || ptr == IntPtr.Zero)
                return;
            PhysicsApi.API.DestroySpringAction(CurrentPhysicsWorld.GetPtr(), ptr); 
            ptr = IntPtr.Zero;
        }
    }
}