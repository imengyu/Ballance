using System;
using System.Text;

namespace InGameCodeEditor
{
    /// <summary>
    /// Auto indent options used to define indentation rules.
    /// </summary>
    [Serializable]
    public class AutoIndent
    {
        // Enum
        public enum IndentMode
        {
            /// <summary>
            /// Auto indenting will not be used.
            /// </summary>
            None,
            /// <summary>
            /// Basic auto indenting where moving to the next line will cause the caret to be indented based upon the previous line.
            /// </summary>
            AutoTab,
            /// <summary>
            /// Contextual auto indenting based on language opening/closing blocks.
            /// </summary>
            AutoTabContextual,
        }

        // Private
        private static StringBuilder indentBuilder = new StringBuilder();

        [NonSerialized]
        private string indentDecreaseString = null;

        // Public
        public IndentMode autoIndentMode = IndentMode.AutoTab;

        /// <summary>
        /// Should auto indent be used for this language.
        /// </summary>
        public bool allowAutoIndent = true;
        /// <summary>
        /// The character that causes the indent level to increase.
        /// </summary>
        public char indentIncreaseCharacter;
        /// <summary>
        /// The character that causes the indent level to decrease.
        /// </summary>
        public char indentDecreaseCharacter;

        // Properties
        /// <summary>
        /// Get the string representation of the indent character.
        /// </summary>
        public string IndentDecreaseString
        {
            get
            {
                if (indentDecreaseString == null)
                {
                    indentDecreaseString = new string(indentDecreaseCharacter, 1);
                }
                return indentDecreaseString;
            }
        }

        // Methods
        public string GetAutoIndentedFormattedString(string indentSection, int currentIndent, out int caretPosition)
        {
            // Add indent level
            int indent = currentIndent + 1;
                       
            // Append characters
            for(int i = 0; i < indentSection.Length; i++)
            {
                if(indentSection[i] == '\n')
                {
                    indentBuilder.Append('\n');
                    AppendIndentString(indent);
                }
                else if(indentSection[i] == '\t')
                {
                    // We will add tabs manually
                    continue;
                }
                else if(indentSection[i] == indentIncreaseCharacter)
                {
                    indentBuilder.Append(indentIncreaseCharacter);
                    indent++;
                }
                else if(indentSection[i] == indentDecreaseCharacter)
                {
                    indentBuilder.Append(indentDecreaseCharacter);
                    indent--;
                }
                else
                {
                    indentBuilder.Append(indentSection[i]);
                }
            }

            // Build the string
            string formattedSection = indentBuilder.ToString();
            indentBuilder.Length = 0;

            // Default caret position
            caretPosition = formattedSection.Length - 1;

            // Find the caret position
            for (int i = formattedSection.Length - 1; i >= 0; i--)
            {
                if (formattedSection[i] == '\n')
                    continue;

                caretPosition = i;
                break;
            }

            return formattedSection;
        }

        public int GetAutoIndentLevel(string inputString, int startIndex, int endIndex)
        {
            int indent = 0;

            for(int i = startIndex; i < endIndex; i++)
            {
                if (inputString[i] == '\t')
                    indent++;

                // Check for end line or other characters
                if (inputString[i] == '\n' || inputString[i] != ' ')
                    break;
            }

            return indent;
        }

        private void AppendIndentString(int amount)
        {
            for (int i = 0; i < amount; i++)
                indentBuilder.Append("\t");
        }
    }
}
