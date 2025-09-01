namespace MCS.UI.Areas.Admin.Models.Lookups
{
    public class LetterTypeViewModel
    {
        public LetterTypeVM LetterType { get; set; }
        public LetterTypeAddVM AddLetterType { get; set; }
        public LetterTypeEditVM EditLetterType { get; set; }

        public LetterTypeViewModel()
        {
            LetterType = new LetterTypeVM();
            AddLetterType = new LetterTypeAddVM();
            EditLetterType = new LetterTypeEditVM();
        }
    }
}