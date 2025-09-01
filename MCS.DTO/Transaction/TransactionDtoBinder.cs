using Newtonsoft.Json.Linq;
using MCS.Common;

namespace MCS.DTO
{
    public class TransactionDTOBinder : System.Web.Http.ModelBinding.IModelBinder
    {
        public bool BindModel(System.Web.Http.Controllers.HttpActionContext actionContext, System.Web.Http.ModelBinding.ModelBindingContext bindingContext)
        {
            //TODO : To Recheck It With Ahmad 
            string ct = actionContext.Request.Content.ReadAsStringAsync().Result;

            JObject jObject = Newtonsoft.Json.JsonConvert.DeserializeObject<JObject>(ct);

            int transactionID = jObject.GetValue("Id").Value<int>();

            TransactionCategory transactionCategory = (TransactionCategory)jObject.GetValue("TransactionCategory").Value<int>();

            switch (transactionCategory)
            {
                case TransactionCategory.Inbound:
                    {
                        if (transactionID == 0)
                        {
                            AddInboundDTO inboundAddDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<AddInboundDTO>(ct);

                            bindingContext.Model = inboundAddDTO;

                            return true;
                        }

                        EditInboundDTO inboundEditDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<EditInboundDTO>(ct);

                        bindingContext.Model = inboundEditDTO;

                        return true;
                    }
                case TransactionCategory.ExternalOutbound:
                    {
                        if (transactionID == 0)
                        {
                            AddOutboundExternalDTO outboundExternalAddDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<AddOutboundExternalDTO>(ct);

                            bindingContext.Model = outboundExternalAddDTO;

                            return true;
                        }

                        EditOutboundExternalDTO outboundExternalEditDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<EditOutboundExternalDTO>(ct);

                        bindingContext.Model = outboundExternalEditDTO;

                        return true;
                    }
                case TransactionCategory.InternalOutbound:
                    {
                        if (transactionID == 0)
                        {
                            AddOutboundInternalDTO outboundInternalAddDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<AddOutboundInternalDTO>(ct);

                            bindingContext.Model = outboundInternalAddDTO;

                            return true;
                        }

                        EditOutboundInternalDTO outboundInternalEditDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<EditOutboundInternalDTO>(ct);

                        bindingContext.Model = outboundInternalEditDTO;

                        return true;
                    }
                case TransactionCategory.DraftOutbound:
                    {
                        if (transactionID == 0)
                        {
                            AddOutboundDraftDTO outboundDraftAddDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<AddOutboundDraftDTO>(ct);

                            bindingContext.Model = outboundDraftAddDTO;

                            return true;
                        }

                        EditOutboundDraftDTO outboundDraftEditDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<EditOutboundDraftDTO>(ct);

                        bindingContext.Model = outboundDraftEditDTO;

                        return true;
                    }
            }

            return true;
        }

    }
}
