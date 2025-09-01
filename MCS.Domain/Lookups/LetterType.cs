using MCS.Common;

namespace MCS.Domain
{
    public class LetterType : LookupBase
    {
        public LetterListType LetterListType { get; set; }
        public bool IsPopularization { get; set; }
        public bool Notify { get; set; }
        public bool WithExtraField { get; set; }
    }
}
