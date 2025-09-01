using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCS.IntegrationServices.Models.Sharepoints
{
    public class GetTransactionResponse : GetAllBaseResponse
    {
        [JsonProperty("transactions")]
        public List<TransactionModel> Transactions { get; set; }

    }

    public class TransactionModel
    {
        [JsonProperty("transactionUrl")]
        public string TransactionUrl { get; set; }
        //[JsonProperty("subject")]
        //public string Subject { get; set; }
        //[JsonProperty("transactionDate")]
        //public DateTime TransactionDate { get; set; }
        [JsonProperty("transactionDate")]
        public string TransactionDate { get; set; }
        [JsonProperty("confidentiality")]
        public string Confidentiality { get; set; }
        //[JsonProperty("transactionType")]
        //public string TransactionType { get; set; }
        [JsonProperty("transactionNumber")]
        public string TransactionNumber { get; set; }



    }
}