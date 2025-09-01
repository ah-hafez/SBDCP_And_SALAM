namespace MCS.DocRepository.DataDef
{
    public class DocumentLocation
    {
        public DocumentLocation()
        {
        }

        public DocumentLocation(int CabinetId)
        {
            this.Cabinet_ID = CabinetId;
        }

        private int Cabinet_Id;
        private string FolderName;
        private string DocumentLibName;
        private string RelativePath;

        public int Cabinet_ID
        {
            get { return this.Cabinet_Id; }
            set { this.Cabinet_Id = value; }
        }

        public string Folder_Name
        {
            get { return this.FolderName; }
            set { this.FolderName = value; }
        }
        public string DocumentLib_Name
        {
            get { return this.DocumentLibName; }
            set { this.DocumentLibName = value; }
        }

        public string Relative_Path
        {
            get { return this.RelativePath; }
            set { this.RelativePath = value; }
        }
    }
}