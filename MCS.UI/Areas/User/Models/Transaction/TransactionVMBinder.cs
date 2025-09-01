using Newtonsoft.Json.Linq;
using MCS.Common;
using MCS.UI.Areas.User.Models.Transaction.Inbound;
using MCS.UI.Areas.User.Models.Transaction.Outbound.Draft;
using MCS.UI.Areas.User.Models.Transaction.Outbound.External;
using MCS.UI.Areas.User.Models.Transaction.Outbound.Internal;

namespace MCS.UI.Areas.User.Models.Transaction
{
    public class TransactionVMBinder : System.Web.Http.ModelBinding.IModelBinder
    {
        public bool BindModel(System.Web.Http.Controllers.HttpActionContext actionContext, System.Web.Http.ModelBinding.ModelBindingContext bindingContext)
        {
            //TODO : To Recheck It With Ahmad 
            string ct = actionContext.Request.Content.ReadAsStringAsync().Result;

            JObject jObject = Newtonsoft.Json.JsonConvert.DeserializeObject<JObject>(ct);

            int transactionID = jObject.GetValue("Id").Value<int>();

            TransactionCategory transactionType = (TransactionCategory)jObject.GetValue("Type").Value<int>().LookupInternalID(LookupCategory.TransactionStatus, string.Empty);

            switch (transactionType)
            {
                case TransactionCategory.Inbound:
                    {
                        if (transactionID == 0)
                        {
                            AddInboundVM inboundAddVM = Newtonsoft.Json.JsonConvert.DeserializeObject<AddInboundVM>(ct);

                            bindingContext.Model = inboundAddVM;

                            return true;
                        }

                        EditInboundVM inboundEditVM = Newtonsoft.Json.JsonConvert.DeserializeObject<EditInboundVM>(ct);

                        bindingContext.Model = inboundEditVM;

                        return true;
                    }
                case TransactionCategory.ExternalOutbound:
                    {
                        if (transactionID == 0)
                        {
                            AddOutboundExternalVM outboundExternalAddVM = Newtonsoft.Json.JsonConvert.DeserializeObject<AddOutboundExternalVM>(ct);

                            bindingContext.Model = outboundExternalAddVM;

                            return true;
                        }

                        EditOutboundExternalVM outboundExternalEditVM = Newtonsoft.Json.JsonConvert.DeserializeObject<EditOutboundExternalVM>(ct);

                        bindingContext.Model = outboundExternalEditVM;

                        return true;
                    }
                case TransactionCategory.InternalOutbound:
                    {
                        if (transactionID == 0)
                        {
                            AddOutboundInternalVM outboundInternalAddVM = Newtonsoft.Json.JsonConvert.DeserializeObject<AddOutboundInternalVM>(ct);

                            bindingContext.Model = outboundInternalAddVM;

                            return true;
                        }

                        EditOutboundInternalVM outboundInternalEditVM = Newtonsoft.Json.JsonConvert.DeserializeObject<EditOutboundInternalVM>(ct);

                        bindingContext.Model = outboundInternalEditVM;

                        return true;
                    }
                case TransactionCategory.DraftOutbound:
                    {
                        if (transactionID == 0)
                        {
                            AddOutboundDraftVM outboundDraftAddVM = Newtonsoft.Json.JsonConvert.DeserializeObject<AddOutboundDraftVM>(ct);

                            bindingContext.Model = outboundDraftAddVM;

                            return true;
                        }

                        EditOutboundDraftVM outboundDraftEditVM = Newtonsoft.Json.JsonConvert.DeserializeObject<EditOutboundDraftVM>(ct);

                        bindingContext.Model = outboundDraftEditVM;

                        
                        return true;
                    }
            }

            return true;
        }
    }
   
}