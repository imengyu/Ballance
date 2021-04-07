namespace Ballance2.Sys.UI.UISystem
{
    /// <summary>
    /// 布局参数
    /// </summary>
    [SLua.CustomLuaClass]
    public class LayoutParams
    {
        /// <summary>
        /// 填充父元素
        /// </summary>
        public const float MATCH_PARENT = -1;
        /// <summary>
        /// 依据自身内容调整
        /// </summary>
        public const float WRAP_CONTENT = -2;

        public float height = WRAP_CONTENT;
        public float width = WRAP_CONTENT;

        public float layoutMargin = 0;
        public float bottomMargin = 0;
        public float endMargin = 0;
        public float leftMergin = 0;
        public float rightMargin = 0;
        public float startMargin = 0;
        public float topMargin = 0;

        /// <summary>
        /// TODO: [未完成]
        /// </summary>
        public LayoutDirection layoutDirection = LayoutDirection.LTR;
    }

    /// <summary>
    /// 布局方向（用于不同文字书写方向）TODO: [未完成]
    /// </summary>
    [SLua.CustomLuaClass]
    public enum LayoutDirection {
        LTR,
        RTL
    }
}
