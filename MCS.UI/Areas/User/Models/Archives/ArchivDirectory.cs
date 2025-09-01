using System;

namespace MCS.UI.Areas.User.Models.Archives
{
    public class ArchivDirectory
    {
        public int DirectoryNum { get; set; }
        public string Description { get; set; }
        public int? Code { get; set; }

        public int ClassificationId { get; set; }
        public int DirectedToId { get; set; }




    }
}