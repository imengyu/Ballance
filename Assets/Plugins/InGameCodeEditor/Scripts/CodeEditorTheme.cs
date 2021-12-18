using System;
using UnityEngine;

namespace InGameCodeEditor
{
    /// <summary>
    /// A theme that can be assigned to a code editor to change its visual appearance.
    /// </summary>
    [Serializable]
    [CreateAssetMenu(fileName = "Code Editor Theme", menuName = "InGame Code Editor/Code Editor Theme")]
    public class CodeEditorTheme : ScriptableObject
    {
        // Public
        /// <summary>
        /// The color of the caret for the input field.
        /// </summary>
        public Color caretColor = new Color32(51, 51, 51, 255);
        /// <summary>
        /// The main color of the text for the input field.
        /// </summary>
        public Color textColor = new Color32(51, 51, 51, 255);
        /// <summary>
        /// The main background color for the input field.
        /// </summary>
        public Color backgroundColor = new Color32(255, 255, 254, 255);
        /// <summary>
        /// The line highlight overlay color used to show the current line.
        /// </summary>
        public Color lineHighlightColor = new Color32(226, 225, 225, 255);
        /// <summary>
        /// The background color of the line numbers column.
        /// </summary>
        public Color lineNumberBackgroundColor = new Color32(255, 255, 254, 255);
        /// <summary>
        /// The text color of the line numbers column.
        /// </summary>
        public Color lineNumberTextColor = new Color32(43, 145, 175, 255);
        /// <summary>
        /// The color of the scrollbar.
        /// </summary>
        public Color scrollbarColor = new Color32(193, 193, 192, 255);
        /// <summary>
        /// Is syntax highlighting of the input field text allowed by this theme.
        /// </summary>
        public bool allowSyntaxHighlighting = true;

        // Properties
        /// <summary>
        /// Get the default code editor theme.
        /// </summary>
        public static CodeEditorTheme DefaultTheme
        {
            get { return CreateInstance<CodeEditorTheme>(); }
        }
    }
}
