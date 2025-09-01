namespace MCS.Framework.Controls.Mvc
{
    /// <summary>
    ///     Renders the hiiden cells of the hidden columns
    /// </summary>
    internal class GridHiddenHeaderRenderer : GridHeaderRenderer
    {
        public GridHiddenHeaderRenderer()
        {
            AddCssStyle("display:none;");
        }
    }
}