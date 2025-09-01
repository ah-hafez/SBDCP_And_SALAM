using System.Collections.Generic;

namespace MCS.UI.Areas.Admin.Models.Lookups
{
    public class SubjectClassificationViewModel
    {
        public SubjectClassificationVM SubjectClassification { get; set; }
        public SubjectClassificationAddVM SubjectClassificationAddVM { get; set; }
        public SubjectClassificationEditVM SubjectClassificationEditVM { get; set; }

        public SubjectClassificationViewModel()
        {
            SubjectClassification = new SubjectClassificationVM();
            SubjectClassificationAddVM = new SubjectClassificationAddVM();
            SubjectClassificationEditVM = new SubjectClassificationEditVM();
        }
    }
}