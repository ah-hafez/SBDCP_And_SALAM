using System.Web.Mvc;
namespace MCS.Tenants.Service.Binders
{
    public class TrimModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var shouldPerformRequestValidation = controllerContext.Controller.ValidateRequest && bindingContext.ModelMetadata.RequestValidationEnabled;

            var valueResult = bindingContext.GetValueFromValueProvider(shouldPerformRequestValidation);
            return valueResult?.AttemptedValue == null ? null : (valueResult.AttemptedValue == string.Empty ? string.Empty : valueResult.AttemptedValue.Trim());
        }
    }
    public static class ExtensionHelpers
    {
        public static ValueProviderResult GetValueFromValueProvider(this ModelBindingContext bindingContext, bool performRequestValidation)
        {
            var unvalidatedValueProvider = bindingContext.ValueProvider as IUnvalidatedValueProvider;
            return (unvalidatedValueProvider != null)
              ? unvalidatedValueProvider.GetValue(bindingContext.ModelName, !performRequestValidation)
              : bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
        }
    }
}