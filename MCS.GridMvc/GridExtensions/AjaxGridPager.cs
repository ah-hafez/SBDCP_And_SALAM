using MCS.GridMvc.Pagination;
using System.Linq;
namespace MCS.GridMvc.Ajax.GridExtensions
{
    public class AjaxGridPager : IGridPager, IAjaxGridPager
    {
        private readonly IGrid _grid;
        public int PagePartitionSize { get; set; }
        public AjaxGridPager(IGrid grid)
        {
            _grid = grid;
        }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public string TemplateName
        {
            get
            {
                return "_AjaxGridNewPager";
            }
        }
        public bool HasPages
        {
            get
            {
                return _grid.ItemsCount > PageSize;
            }
        }
        public int Pages { get; protected set; }
        public void Initialize(int itemsCount)
        {
            int count = _grid.ItemsCount;
            Pages = count / PageSize;
            if (count % PageSize > 0)
                Pages++;
        }
        public string GridName
        {
            get
            {
                return _grid.RenderOptions.GridName;
            }
        }
    }
}