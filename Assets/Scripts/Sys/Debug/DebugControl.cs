using Ballance2;
using UnityEngine;

namespace Ballance.Sys.Debug
{
    public class DebugControl : MonoBehaviour 
    {
        private void Update()
		{   
            DebugCamera.Instance.UpdateEnableDebugZ();
        }
        public void PrepareWindow() {
            var d = DebugCamera.Instance;
            d.GameDebugInspectorWindow.onShow += (w) => { d.EnableDebug = true; };
            d.GameDebugInspectorWindow.onHide += (w) => { d.EnableDebug = false; };
            d.GameDebugInspectorWindow.onClose += (w) => { d.EnableDebug = false; };
        }
    
        public static Camera LastCamera;

        private void OnPostRender() {
            LastCamera = Camera.current;
        }
    }
}