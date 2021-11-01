using Ballance2;
using UnityEngine;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* DebugCamera.cs
* 
* 用途：
* 调试控制器相关脚本。
*
* 作者：
* mengyu
*/

namespace Ballance2.Sys.Debug
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