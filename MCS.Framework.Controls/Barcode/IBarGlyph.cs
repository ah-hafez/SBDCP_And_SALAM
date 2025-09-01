using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Framework.Controls.Barcode
{
    /// <summary>
    /// <c>IBarGlyph</c> extends <see cref="T:MCS.Framework.Controls.Barcode.IGlyph"/> by 
    /// specifying a bit encoding for a given character. 
    /// The bits indicate where bars are drawn.
    /// </summary>
    public interface IBarGlyph : IGlyph
    {
        /// <summary>
        /// Gets the bit encoding.
        /// </summary>
        /// <value>The bit encoding.</value>
        short BitEncoding
        {
            get;
        }
    }
}
