using MCS.GridMvc.Columns;
using System.Web;

namespace MCS.GridMvc
{
    /// <summary>
    ///     Renderer of the header
    /// </summary>
    public interface IGridColumnHeaderRenderer
    {
        /// <summary>
        ///     Render grid header
        /// </summary>
        /// <param name="column">Column</param>
        /// <returns>HTML</returns>
        IHtmlString Render(IGridColumn column);
    }
}