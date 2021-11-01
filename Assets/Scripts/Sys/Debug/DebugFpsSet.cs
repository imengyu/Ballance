/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* DebugFpsSet.cs
* 
* 用途：
* 用于调试时设置FPS的脚本。
*
* 作者：
* mengyu
*/

namespace Ballance2.Sys.Debug
{
    using UnityEngine;
    
    public class DebugFpsSet : MonoBehaviour {
        public bool SetFPS = true;
        public int FPS = 60;

        private void Start() {
            Application.targetFrameRate = FPS;
        }
    }
}