using System;

namespace MobileApi.Common
{
    [Serializable]
    public enum MessageCode
    {
        Success = 0,
        InvalidUserNameOrPassword = -1,
        UnauthenticatedUser = -2,
        UnauthenticatedUserOnMobile = -3,
        UserConnectedToAnotherMobile = -4,
        SessionTokenTimedOut = -5,
        LogoutUnsuccessfully = -6,
        CorrespondenceIdNotValid = -7,
        CorrespondenceGetUnsuccessful = -8,
        DocumentDataInvalid = -9,
        DataNotReturned = -10,
        CorrespondenceUpdatedUnsuccessfully = -11,
        YouAreNotLoggedIn = -12,
        IncorrectSignaturePassword = -13,
        NoSignaturePasswordExist = -14
    }

    //[Serializable]
    //public enum LanguageName
    //{
    //    ar,
    //    en
    //}

    public enum ReportType
    {
        Pie,
        Bar
    }

    public enum ReportPeriodType
    {
        Year,
        Month,
        Week
    }

    [Flags]
    public enum enUpdateFlags
    {
        None = 0,
        SettingsUpdated = 0x01,
        RevocationNeeded = 0x02,
        OrgChartUpdated = 0x04,
        ResourcesUpdated = 0x08,
    }
}