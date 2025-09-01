namespace MCS.UI.Areas.Admin.Models.Lookups
{
    public class LinkViewModel
    {
        public LinkVM Link { get; set; }
        public LinkAddVM AddLink { get; set; }
        public LinkEditVM EditLink { get; set; }

        public LinkViewModel()
        {
            Link = new LinkVM();
            AddLink = new LinkAddVM();
            EditLink = new LinkEditVM();
        }
    }
}