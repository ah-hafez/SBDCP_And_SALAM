namespace MobileApi.Domain
{
    public class TransactionPartyDirection : Lookup
    {
    }

    public enum TransPartyDirection
    {
        Uknown = -1,
        Main = 314,
        Copy = 315,
        InternalDistribution = 316,
        Coordination = 509
    }
}
