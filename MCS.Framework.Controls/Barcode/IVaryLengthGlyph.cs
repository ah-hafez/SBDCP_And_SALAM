using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Framework.Controls.Barcode
{
    /// <summary>
    /// <c>IVaryLengthGlyph</c> identifies a glyph object as supporting a 
    /// custom bit encoding width
    /// </summary>
    public interface IVaryLengthGlyph : IBarGlyph
    {
        /// <summary>
        /// Gets the number of bits used for the bit encoding.
        /// </summary>
        /// <value>The width encoding.</value>
        short BitEncodingWidth
        {
            get;
        }
    }
}
