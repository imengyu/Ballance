using Ballance2.LuaHelpers;
using Ballance2.Utils;
using SLua;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* SplitViewDragger.cs
* 
* 用途：
* 拖动托块组件
* 
* 通过设置referenceTransform参考视图，拖动方向，
* 设置onValueChanged返回用户拖动value，表示拖动的百分比
*
* 作者：
* mengyu
*
*/

namespace Ballance2.Sys.UI
{
    /// <summary>
    /// 拖动托块组件
    /// </summary>
    [CustomLuaClass]
    [LuaApiDescription("拖动托块组件")]
    [AddComponentMenu("Ballance/UI/Controls/SplitViewDragger")]
    public class SplitViewDragger : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        void Start()
        {
            if (image == null)
                image = GetComponent<Image>();
            rectTransform = GetComponent<RectTransform>();
            direction = _direction;
        }

        private readonly Vector3 vectorz90 = new Vector3(0, 0, 90);

        public RectTransform draggerImage;
        public Image image;
        private RectTransform rectTransform;
        [SerializeField, SetProperty("referenceTransform")]
        private RectTransform _referenceTransform;
        [SerializeField, SetProperty("direction")]
        private SplitViewDirection _direction = SplitViewDirection.LeftRight;

        /// <summary>
        /// 拖动事件（value表示拖动的百分比）
        /// </summary>
        [LuaApiDescription("拖动事件（value表示拖动的百分比）")]
        public Slider.SliderEvent onValueChanged;

        /// <summary>
        /// 拖动参考RectTransform
        /// </summary>
        [LuaApiDescription("拖动参考RectTransform")]
        public RectTransform referenceTransform
        {
            get { return _referenceTransform; }
            set { _referenceTransform = value; }
        }
        /// <summary>
        /// 是否正在拖动
        /// </summary>
        [LuaApiDescription("是否正在拖动")]
        public bool isDrag
        {
            get; private set;
        }
        /// <summary>
        /// 拖动方向
        /// </summary>
        [LuaApiDescription("拖动方向")]
        public SplitViewDirection direction
        {
            get { return _direction; }
            set
            {
                _direction = value;
                if(_direction == SplitViewDirection.LeftRight)
                    draggerImage.eulerAngles = vectorz90;
                else
                    draggerImage.eulerAngles = Vector3.zero;
            }
        }

        [DoNotToLua]
        public void OnDrag(PointerEventData eventData)
        {
            Vector3[] pos = new Vector3[4];
            _referenceTransform.GetWorldCorners(pos);
            float newVal;
            if (_direction == SplitViewDirection.LeftRight)
                newVal = (eventData.position.x - pos[0].x) / (pos[2].x - pos[0].x);
            else
                newVal = ((Screen.height - eventData.position.y) - pos[0].y) / (pos[2].y - pos[0].y);
            onValueChanged.Invoke(newVal);
        }
        [DoNotToLua]
        public void OnPointerDown(PointerEventData eventData)
        {
            isDrag = true;
            image.color = new Color(image.color.r, image.color.g, image.color.b, 0.7f);
        }
        [DoNotToLua]
        public void OnPointerUp(PointerEventData eventData)
        {
            isDrag = false;
            image.color = new Color(image.color.r, image.color.g, image.color.b, 1.0f);
        }
    }
}
