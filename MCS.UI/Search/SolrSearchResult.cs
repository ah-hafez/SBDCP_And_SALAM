using System;
using MCS.Common;
using MCS.UI;

namespace MCS.UI
{
    public class SolrSearchResult : ISearchResult
    {
        public int DocId { get; set; }
        public int TypeId { get; set; }
        public long Number { get; set; }
        public string Barcode { get; set; }
        public string Subject { get; set; }
        public string DateH { get; set; }
        public DateTime Date { get; set; }
        public string PermissionCode { get; set; }
        public int PriorityId { get; set; }
        public int? PartyId { get; set; }
        public int OrgUnitId { get; set; }
        public int SignedByUserId { get; set; }
        public int? DirectedToUserId { get; set; }
        public int StatusId { get; set; }
        public int LetterTypeId { get; set; }
        public string OrgUnitNameAr { get; set; }
        public string OrgUnitNameEn { get; set; }
        public string TypeNameAr { get; set; }
        public string TypeNameEn { get; set; }
        public string PartyNameAr { get; set; }
        public string PartyNameEn { get; set; }
        public string SignedByNameAr { get; set; }
        public string SignedByNameEn { get; set; }
        public string ConfidentialityNameAr { get; set; }
        public string ConfidentialityNameEn { get; set; }
        public string PriorityNameAr { get; set; }
        public string PriorityNameEn { get; set; }
        public string StatusNameAr { get; set; }
        public string StatusNameEn { get; set; }
        public bool WithArchiving { get; set; }
        public int ColorCode { get; set; }
        public string TransactionTypeNameAr { get; set; }
        public string TransactionTypeNameEn { get; set; }
        public int TransactionCategoryId { get; set; }
        public string Type
        {
            get
            {
                if (SessionInfo.CultureShortName == Constants.Languages.Arabic)
                {
                    return TypeNameAr;
                }

                return TypeNameEn;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string ConfidentialityName
        {
            get
            {
                if (SessionInfo.CultureShortName == Constants.Languages.Arabic)
                {
                    return ConfidentialityNameAr;
                }

                return ConfidentialityNameEn;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string PriorityName
        {
            get
            {
                if (SessionInfo.CultureShortName == Constants.Languages.Arabic)
                {
                    return PriorityNameAr;
                }

                return PriorityNameEn;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string PartyName
        {
            get
            {
                if (SessionInfo.CultureShortName == Constants.Languages.Arabic)
                {
                    return PartyNameAr;
                }

                return PartyNameEn;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string OrgUnitName
        {
            get
            {
                if (SessionInfo.CultureShortName == Constants.Languages.Arabic)
                {
                    return OrgUnitNameAr;
                }

                return OrgUnitNameEn;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string SignedByUserName
        {
            get
            {
                if (SessionInfo.CultureShortName == Constants.Languages.Arabic)
                {
                    return SignedByNameAr;
                }

                return SignedByNameEn;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string StatusName
        {
            get
            {
                if (SessionInfo.CultureShortName == Constants.Languages.Arabic)
                {
                    return StatusNameAr;
                }

                return StatusNameEn;
            }
            set
            {
                throw new NotImplementedException();
            }
        }
        public string TransactionTypeName
        {
            get
            {
                if (SessionInfo.CultureShortName == Constants.Languages.Arabic)
                {
                    return TransactionTypeNameAr;
                }

                return TransactionTypeNameEn;
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
