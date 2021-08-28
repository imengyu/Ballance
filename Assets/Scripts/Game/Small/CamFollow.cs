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
        
        public Transform Target {
            get { return _Target; }
            set {
                _Target = value;
                if(_Target != null) {
                    LookSmoothTarget.transform.position = _Target.position;
                    transform.position = _Target.position;
                }
            }
        }
        public Transform LookSmoothTarget = null;
        [LuaApiDescription("指定当前跟踪的目标")]
        [Tooltip("指定当前跟踪的目标")]
        public Transform CameraTransform = null;
        [LuaApiDescription("摄像机平滑移动的时间")]
        [Tooltip("摄像机平滑移动的时间")]
        public float SmoothTime = 0.2f;
        [LuaApiDescription("摄像机平滑移动的时间")]
        [Tooltip("摄像机平滑移动的时间")]
        public float SmoothTimeY = 0.5f;

        public float SmoothToTargetTime = 0.1f;

        [Tooltip("指定当前跟踪的目标")]
        private Transform _Target = null;
        private float smoothyVelocity = 0;
        private Vector3 smoothTargetVelocity = Vector3.zero;
        private Vector3 cameraVelocity = Vector3.zero;

        private void FixedUpdate() {
            if(Target != null) 
            {
                if(Look) {
                    LookSmoothTarget.transform.position = Vector3.SmoothDamp(transform.position, Target.position, ref smoothTargetVelocity, SmoothToTargetTime);
                    
                    if(CameraTransform != null)
                        CameraTransform.LookAt(LookSmoothTarget);
                }
                if(Follow) {
                    var v = Vector3.SmoothDamp(transform.position, Target.position, ref cameraVelocity, SmoothTime);
                    transform.position = new Vector3(
                        v.x, 
                        Mathf.SmoothDamp(transform.position.y, Target.position.y, ref smoothyVelocity, SmoothTimeY), 
                        v.z);
                }
            }
        }
    }
}