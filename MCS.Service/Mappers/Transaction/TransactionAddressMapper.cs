using MCS.Domain;
using MCS.DTO;
using System.Linq;

namespace MCS.Service.Mappers
{
    public static class TransactionAddressMapper
    {
        public static TransactionAddressDTO Map(Transaction transaction, string CultureName)
        {
            if (transaction == null)
            {
                return new TransactionAddressDTO();
            }

            TransactionAddressDTO transactionAddressDTO = new TransactionAddressDTO
            {
                DirectedTo = transaction.ExternalPartyManager != null ? transaction.ExternalPartyManager.Name.ToString() : "",
                DirectedToOrgUnit = transaction.ExternalParty.Name.Localizations.Where(l => l.Culture.ShortName == CultureName).FirstOrDefault().Text,
                DocumentType = transaction.TransactionType.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == CultureName).FirstOrDefault().Text,
                TransactionDate = transaction.Date.ToString(),
                Transactionnumber = transaction.Number.ToString(),
                ShipmentNumber = transaction.DeliveryNumber
            };

            return transactionAddressDTO;
        }
    }
}