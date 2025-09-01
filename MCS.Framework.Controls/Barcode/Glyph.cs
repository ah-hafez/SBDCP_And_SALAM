using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Framework.Controls.Barcode
{
    public class Glyph : IGlyph
    {
        private char _character;

        /// <summary>
        /// Initialises a new instance of the <see cref="T:MCS.Framework.Controls.Barcode.Glyph"/>
        /// class with the specified bit encoding.
        /// </summary>
        /// <param name="character">Character represented by glyph.</param>
        public Glyph(char character)
        {
            _character = character;
        }

        /// <summary>
        /// Gets the <see cref="T:System.Char"/> character associated with this glyph.
        /// </summary>
        /// <value>The character.</value>
        public char Character
        {
            get
            {
                return _character;
            }
        }
    }
}
