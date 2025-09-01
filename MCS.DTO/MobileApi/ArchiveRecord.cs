using System;
using MCS.Common;

namespace MobileApi.Domain
{
    public class ArchiveRecord
    {
        public enum enumStatus { added, deleted, modified, unmodified }
        public enumStatus status;
        public long attachRecoredId;
        public int transId;
        public enEditorAttachMethod method;
        public enArchivingType type;
        public int docId;
        public int securtyLevel;
        public string securityLevelDesc;
        public string title;
        public string user;
        public string date;
        //public bool isNotMyRecord;
        public int UserID;
        //public int index;
        public string fileName;
        public string MimeContent;
        public int RowStatus;
        public byte[] DocData;
        public DateTime LastModifiedOn;
        public int IncludedItemId;
        public bool canBeDeleted;
        public string PrivilegeName;
    } 
    public enum enEditorAttachMethod
    {
        WordAttach = 301,
        ScanAttach = 302,
        TextAttach = 303,
        WordAttachFullScreen = 304,
        VoiceAttach = 305,
        HtmlAttach = 306,

    }

    public enum enArchivingType
    {
        TransSourceID = 1,
        IncludedItem = 2,
        Precedent = 3,
        OutboundLetter = 4,
        Letter = 5,
        Explaination = 6,
        Proposition = 7,
        OutboundDraft = 8,
        Coordination = 9,
        Replies = 10,
        Manifest = 11
    }
}