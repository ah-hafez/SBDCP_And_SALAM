namespace MobileApi.Common
{
    public class ResourceText
    {
        public const string LoginSuccessfully = "LoginSuccessfully";
        public const string InvalidUserName = "InvalidUserName";
        public const string UnauthenticatedUser = "UnauthenticatedUser";
        public const string UnauthenticatedUserOniPad = "UnauthenticatedUserOniPad";
        public const string UserConnectedToAnotheriPad = "UserConnectedToAnotheriPad";
        public const string SessionTokenTimedOut = "SessionTokenTimedOut";
        public const string LogoutUnsuccessfully = "LogoutUnsuccessfully";
        public const string CorrespondenceIdNotValid = "CorrespondenceIdNotValid";
        public const string CorrespondenceGetUnsuccessful = "CorrespondenceGetUnsuccessful";
        public const string Inbound = "Inbound";
        public const string InboundCopies = "InobundCopies";
        public const string Outbound = "Outbound";
        public const string OutboundCopies = "OutboundCopies";
        public const string Confidentiality = "Confidentiality";
        public const string Priority = "Priority";
        public const string PriorityDate = "PriorityDate";
        public const string Subject = "Subject";
        public const string Type = "Type";
        public const string DeptReception = "DeptReception";
        public const string DocumentDataInvalid = "DocumentDataInvalid";
        public const string DataNotReturned = "DataNotReturned";
        public const string CorrespondenceUpdatedUnsuccessfully = "CorrespondenceUpdatedUnsuccessfully";
        public const string YouAreNotLoggedIn = "YouAreNotLoggedIn";
        public const string MyTransactionsDashboardLabel = "MyTransactionsDashboardLabel";
        public const string DelayedTransactionsDashboardLabel = "DelayedTransactionsDashboardLabel";
        public const string WithAppointmentTransactionsDashboardLabel = "WithAppointmentTransactionsDashboardLabel";
        public const string DecisionTransactionsDashboardLabel = "DecisionTransactionsDashboardLabel";
        public const string CopiesDashboardLabel = "CopiesDashboardLabel";
        public const string ContactUs = "ContactUs";
        public const string AboutMorasalat = "AboutMorasalat";
        public const string TermsAndConditions = "TermsAndConditions";
        public const string IncorrectSignaturePassword = "IncorrectSignaturePassword";
        public const string NoSignaturePasswordExist = "NoSignaturePasswordExist";
        public const string HijriSymbol = "HijriSymbol";
        public const string FaildToSetDefaultEntity = "FaildToSetDefaultEntity";

    }

    public class MessageResources
    {
        public static string GetResourceText(string resourceText, string languageAbbreviation)
        {
            string text = string.Empty;

            switch (languageAbbreviation)
            {
                case "ar":
                    text = Resources.Messages.ResourceManager.GetString(resourceText + "_AR");
                    break;
                case "en":
                    text = Resources.Messages.ResourceManager.GetString(resourceText + "_EN");
                    break;
                default:
                    text = Resources.Messages.ResourceManager.GetString(resourceText + "_AR");
                    break;
            }

            return text;
        }
    }

    public class LabelResources
    {
        public static string GetResourceText(string resourceText, string languageAbbreviation)
        {
            string text = string.Empty;

            switch (languageAbbreviation)
            {
                case "ar":
                    text = Resources.Labels.ResourceManager.GetString(resourceText + "_AR");
                    break;
                case "en":
                    text = Resources.Labels.ResourceManager.GetString(resourceText + "_EN");
                    break;
                default:
                    text = Resources.Labels.ResourceManager.GetString(resourceText + "_AR");
                    break;
            }

            return text;
        }
    }
}