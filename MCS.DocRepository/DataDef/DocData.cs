using System;

namespace MCS.DocRepository.DataDef

{
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:my-enterprise:Basics")]
    public class DocData
    {
        public DocData()
        {
        }

        #region Members signatures
        private string DocumentId;
        private byte[] DocumentData;
        private string DocumentName;
        private int DocumentSize;
        private string DocumentMimeContent;
        private string DocumentApplication;
        private string User_Id;
        private bool specifiedExplanation;
        private int? entityId;
        private int? personId;
        private string ecmId;
        #endregion
        #region Property signatures
        public string DocID
        {
            get
            {
                return this.DocumentId;
            }
            set
            {
                this.DocumentId = value;
            }
        }
        public byte[] Data
        {
            get
            {
                return this.DocumentData;
            }
            set
            {
                this.DocumentData = value;
            }
        }
        public string DocName
        {
            get
            {
                return this.DocumentName;
            }
            set
            {
                this.DocumentName = value;
            }
        }
        public int DataSize
        {
            get
            {
                return this.DocumentSize;
            }
            set
            {
                this.DocumentSize = value;
            }

        }
        public string MimeContent
        {
            get
            {
                return this.DocumentMimeContent;
            }
            set
            {
                this.DocumentMimeContent = value;
            }
        }
        public string Application
        {
            get
            {
                return this.DocumentApplication;
            }
            set
            {
                this.DocumentApplication = value;
            }

        }
        public string User_ID
        {
            get
            {
                return this.User_Id;
            }
            set
            {
                this.User_Id = value;
            }

        }
        public bool SpecifiedExplanation
        {
            get
            {
                return this.specifiedExplanation;
            }
            set
            {
                this.specifiedExplanation = value;
            }
        }
        public int? EntityId
        {
            get
            {
                return this.entityId;
            }
            set
            {
                this.entityId = value;
            }
        }
        public int? PersonId
        {
            get
            {
                return this.personId;
            }
            set
            {
                this.personId = value;
            }
        }
        public string ECMID
        {
            get
            {
                return this.ecmId;
            }
            set
            {
                this.ecmId = value;
            }
        }
        public int TransactionId { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionDateHijri { get; set; }

        #endregion
        #region Static Data
        public static string[] ColsArray = { "DOCID", "DMSDOCDATA", "DOCNAME", "DATASIZE", "MIMECONTENT", "APPLICATION", "USERID", "NEWDOCID", "LASERFICHE_MIGRATED", "LASERFICHE_ID" };
        public static string ClmDOCID { get { return ColsArray[0]; } }
        public static string ClmDMSDOCDATA { get { return ColsArray[1]; } }
        public static string ClmDOCNAME { get { return ColsArray[2]; } }
        public static string ClmDATASIZE { get { return ColsArray[3]; } }
        public static string ClmMIMECONTENT { get { return ColsArray[4]; } }
        public static string ClmAPPLICATION { get { return ColsArray[5]; } }
        public static string ClmUSERID { get { return ColsArray[6]; } }
        public static string ClmNEWDOCID { get { return ColsArray[7]; } }
        public static string ClmLASERFICHMIGRATED { get { return ColsArray[8]; } }
        public static string ClmLASERFICHID { get { return ColsArray[9]; } }

        #endregion
    }
}
