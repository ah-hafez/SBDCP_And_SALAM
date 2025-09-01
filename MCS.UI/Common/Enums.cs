namespace MCS.UI
{
    public enum MessageType
    {
        Information,
        Warning,
        Error
    }

    public enum ResourceSet
    {
        StatusCode,
        Message
    }

    public enum UsersAndTraysReportType
    {
        User,
        Permission,
        Tray
    }
    public enum VerificationType
    {
        None,
        NeedEmail,
        NeedCode
    }
    
    public enum CategoryTypes
    {
        Confedentiality,
        TransactionSourceType,
        BasicDeliveryMethod,
        PriorityLevel,
        InboundDocumentType
    }
}