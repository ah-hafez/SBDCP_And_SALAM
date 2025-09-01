using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using MCS.Framework.Localization;

namespace MCS.Common.CustomAttributes
{
    public class CustomClientValidationAttribute : ValidationAttribute, IClientValidatable
    {
        #region Attributes

        private string _clientFunctionName;
        private string _validationGroup;

        #endregion Attributes

        #region Methods

        public CustomClientValidationAttribute(string messageErrorKey, string clientFunctionName)
        {
            ErrorMessage = messageErrorKey;
            _clientFunctionName = clientFunctionName;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return ValidationResult.Success;
        }
        public override string FormatErrorMessage(string name)
        {
            return DbRes.TValidation(ErrorMessage);
        }

        #endregion Methods

        #region IClientValidatable Members

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            _validationGroup = context.Controller.ViewBag.validationgroup;

            var rule = new ModelClientValidationRule
            {
                ValidationType = _clientFunctionName.ToLower(),
                ErrorMessage = FormatErrorMessage(metadata.DisplayName)
            };

            rule.ValidationParameters.Add("validationgroup", _validationGroup);

            yield return rule;
        }

        #endregion IClientValidatable Members
    }
}
