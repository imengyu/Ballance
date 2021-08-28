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