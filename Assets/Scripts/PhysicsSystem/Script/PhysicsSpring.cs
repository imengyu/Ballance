using System;
using System.Collections;
using UnityEngine;

namespace PhysicsRT
{
    [AddComponentMenu("PhysicsRT/Physics Spring")]
    [DefaultExecutionOrder(250)]
    [DisallowMultipleComponent]
    [SLua.CustomLuaClass]
    public class PhysicsSpring : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("获取弹簧锚点A (世界坐标)")]
        private Transform m_PovitA;
        [SerializeField]
        [Tooltip("获取弹簧锚点B (世界坐标)")]
        private Transform m_PovitB;
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
        public Transform PovitA { get => m_PovitA; set => m_PovitA = value; }
        /// <summary>
        /// 获取弹簧锚点B (世界坐标)
        /// </summary>
        /// <value></value>
        public Transform PovitB { get => m_PovitB; set => m_PovitB = value; }

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

        internal bool TryCreate()
        {
          if (!enabled)
            return false;
          if (ptr == IntPtr.Zero)
          {
            var thisBody = GetComponent<PhysicsBody>();
            if (thisBody.GetPtr() != IntPtr.Zero && (ConnectedBody == null || ConnectedBody.GetPtr() != IntPtr.Zero))
            {
              Create();
              return true;
            }
            else if (ConnectedBody != null && ConnectedBody.GetPtr() == IntPtr.Zero)
              ConnectedBody.AddPendingCreateSpring(this);
            else if (thisBody.GetPtr() == IntPtr.Zero)
              thisBody.AddPendingCreateSpring(this);
          }
          return false;
        }

        public void Create() {
            if(ptr != IntPtr.Zero) {
                return;
            }   
            if(CurrentPhysicsWorld == null) 
              CurrentPhysicsWorld = PhysicsWorld.GetCurrentScensePhysicsWorld();
            if(CurrentPhysicsWorld == null) {
                Debug.LogWarning("Not found PhysicsWorld in this scense, please add it before use PhysicsBody.");
                return;
            }

            ptr = PhysicsApi.API.CreateSpringAction(
                CurrentPhysicsWorld.GetPtr(),
                GetComponent<PhysicsBody>().GetPtr(),
                ConnectedBody != null ? ConnectedBody.GetPtr() : IntPtr.Zero,
                m_PovitA.position,
                m_PovitB.position,
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