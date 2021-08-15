using Ballance2.LuaHelpers;
using UnityEngine;

namespace Ballance2.Game
{
    [LuaApiDescription("摄像机跟随脚本")]
    [SLua.CustomLuaClass]
    public class CamFollow : MonoBehaviour {
        [LuaApiDescription("指定摄像机跟随球是否开启")]
        [Tooltip("指定摄像机跟随球是否开启")]
        public bool Follow = false;
        [LuaApiDescription("指定摄像机看着球是否开启")]
        [Tooltip("指定摄像机看着球是否开启")]
        public bool Look = false;
        [LuaApiDescription("指定当前跟踪的目标")]
        [Tooltip("指定当前跟踪的目标")]
        public Transform Target = null;
        [LuaApiDescription("指定当前跟踪的目标")]
        [Tooltip("指定当前跟踪的目标")]
        public Transform CameraTransform = null;
        [LuaApiDescription("摄像机平滑移动的时间")]
        [Tooltip("摄像机平滑移动的时间")]
        public float SmoothTime = 0.1f;

        private Vector3 cameraVelocity = Vector3.zero;

        private void FixedUpdate() {
            if(Target != null) 
            {
                if(Look && CameraTransform != null)
                    CameraTransform.LookAt(Target);
                if(Follow) 
                    transform.position = Vector3.SmoothDamp(transform.position, Target.position, ref cameraVelocity, SmoothTime);
            }
        }
    }
}