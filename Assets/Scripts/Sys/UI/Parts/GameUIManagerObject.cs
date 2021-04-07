using UnityEngine;

namespace Ballance2.Sys.UI.Parts
{
    class GameUIManagerObject : MonoBehaviour
    {
        public delegate void UpdateDelegate();

        public UpdateDelegate GameUIManagerUpdateDelegate;

        private void Update()
        {
            GameUIManagerUpdateDelegate?.Invoke();
        }
    }
}
