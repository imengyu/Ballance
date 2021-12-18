using System;
using UnityEngine;

namespace InGameCodeEditor.Lexer
{
    /// <summary>
    /// A matcher that checks for any numbers that appear in the lexer stream.
    /// </summary>
    [Serializable]
    public sealed class NumberGroupMatch : MatchLexer
    {
        // Private
        [NonSerialized]
        private string htmlColor = null;

        // Public
        /// <summary>
        /// Should number highlighting be used.
        /// When false, numbers will appear in the default text color as defined by the current editor theme.
        /// </summary>
        [Tooltip("Should numbers be highlighted")]
        public bool highlightNumbers = true;
        /// <summary>
        /// The color that any matched numbers will be highlighted.
        /// </summary>
        [Tooltip("The color that all numbers will be highlighted with")]
        public Color highlightColor = Color.black;

        // Properties
        /// <summary>
        /// Get a value indicating whether keyword highlighting is enabled.
        /// </summary>
        public bool HasNumberHighlighting
        {
            get { return highlightNumbers; }
        }

        /// <summary>
        /// Get the html formatted color tag that any matched numbers will be highlighted with.
        /// </summary>
        public override string HTMLColor
        {
            get
            {
                if (htmlColor == null)
                    htmlColor = "<#" + ColorUtility.ToHtmlStringRGB(highlightColor) + ">";

                return htmlColor;
            }
        }

        // Methods
        /// <summary>
        /// Causes any cached data to be reloaded.
        /// </summary>
        public override void Invalidate()
        {
            this.htmlColor = null;
        }

        /// <summary>
        /// Check whether the specified lexer has a valid number sequence at its current position.
        /// </summary>
        /// <param name="lexer">The input lexer to check</param>
        /// <returns>True if the stream has a number sequence or false if not</returns>
        public override bool IsImplicitMatch(ILexer lexer)
        {
            // Skip highlighting
            if (highlightNumbers == false)
                return false;

            // Require whitespace or symbols before numbers
            if (char.IsWhiteSpace(lexer.Previous) == false &&
                lexer.IsSpecialSymbol(lexer.Previous, SpecialCharacterPosition.End) == false)
            {
                // There is some other character before the potential number
                return false;
            }

            bool matchedNumber = false;

            // Consume the number characters
            while (lexer.EndOfStream == false) 
            {
                // Check for valid numerical character
                if (IsNumberOrDecimalPoint(lexer, lexer.ReadNext()) == true)
                {
                    // We have found a number or decimal
                    matchedNumber = true;
                    lexer.Commit();
                }
                else
                {
                    lexer.Rollback();
                    break;
                }
            }

            return matchedNumber;
        }

        private bool IsNumberOrDecimalPoint(ILexer lexer, char character)
        {
            if(char.IsNumber(character) == true ||
                character == '.')
            {
                // Found a valid number or decimal
                return true;
            }
            return false;
        }
    }
}
