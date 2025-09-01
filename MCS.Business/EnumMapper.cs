using MCS.Common;

namespace MCS.Business
{
    public class EnumMapper
    {
        public static TransactionCategories GetTransactionCategory(TransactionCategory transactionCategory)
        {
            switch (transactionCategory)
            {
                case TransactionCategory.Inbound:
                    return TransactionCategories.Inbound;
                case TransactionCategory.DraftOutbound:
                case TransactionCategory.ExternalOutbound:
                    return TransactionCategories.Outbound;
                case TransactionCategory.InternalOutbound:
                    return TransactionCategories.InternalOutbound;
            }

            return TransactionCategories.None;
        }
        public static TransactionCategory GetTransactionCategory(TransactionCategories transactionCategories)
        {
            switch (transactionCategories)
            {
                case TransactionCategories.Inbound:
                    return TransactionCategory.Inbound;
                case TransactionCategories.Outbound:
                    return TransactionCategory.ExternalOutbound;
                case TransactionCategories.InternalOutbound:
                    return TransactionCategory.InternalOutbound;
                case TransactionCategories.DraftOutbound:
                    return TransactionCategory.DraftOutbound;
            }
            return TransactionCategory.None;
        }
        public static NotificationSubscription GetNotificationSubscription(NotificationSubscriptions notificationSubscriptions)
        {
            switch (notificationSubscriptions)
            {
                case NotificationSubscriptions.MyTransactions:
                    return NotificationSubscription.MyTransactions;
                case NotificationSubscriptions.OutboundDraft:
                    return NotificationSubscription.OutboundDraft;
                case NotificationSubscriptions.Tasks:
                    return NotificationSubscription.Tasks;
                case NotificationSubscriptions.ElectronicCopies:
                    return NotificationSubscription.ElectronicCopies;
                case NotificationSubscriptions.Followup:
                    return NotificationSubscription.Followup;
                case NotificationSubscriptions.Explanation:
                    return NotificationSubscription.Explanation;
                case NotificationSubscriptions.ReceiveReport:
                    return NotificationSubscription.ReceiveReport;
                case NotificationSubscriptions.Delegation:
                    return NotificationSubscription.Delegation;
                case NotificationSubscriptions.OrgUnit:
                    return NotificationSubscription.OrgUnit;
                case NotificationSubscriptions.VerificationCode:
                    return NotificationSubscription.VerificationCode;
            }
            return NotificationSubscription.None;
        }
    }
}
