using MCS.Business;
using MCS.DTO;

namespace MCS.Service.Mappers
{
    public class TransactionTicketMapper
    {
        public static TransactionTicketDTO Map(TransactionTicket transactionTicket)
        {
            if (transactionTicket != null)
            {
                TransactionTicketDTO transactionTicketDTO = new TransactionTicketDTO()
                {
                    BarcodeValue = transactionTicket.barcode.Value,
                    Number = transactionTicket.Number,
                    SequenceNumber = transactionTicket.SequenceNumber,
                    Date = transactionTicket.Date,
                };

                return transactionTicketDTO; 
            }
            return null;
        }
    }
}