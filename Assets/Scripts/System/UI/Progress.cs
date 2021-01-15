using Ballance2.System.UI.Utils;
using Ballance2.Utils;
using UnityEngine;

namespace Ballance2.System.UI
{
    /// <summary>
    /// 进度条组件
    /// </summary>
    [ExecuteInEditMode]
    public class Progress : MonoBehaviour
    {
        void Start()
        {

        }
        private void Update()
        {
            if (val != currentVal)
                updateVal();
        }

        [SerializeField, SetProperty("value")]
        private float val = 0;
        private float currentVal = 0;

        public RectTransform fillArea;
        public RectTransform fillRect;

        private void updateVal()
        {
            currentVal = val;
            UIAnchorPosUtils.SetUIRightTop(fillRect, ((1.0f - val) * fillArea.rect.size.x), 0);
        }

        public float value
        {
            get { return val; }
            set
            {
                val = value;
                updateVal();
            }
        }
    }
}
