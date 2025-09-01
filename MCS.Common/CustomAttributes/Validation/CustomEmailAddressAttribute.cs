using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;
using MCS.Framework.Localization;

namespace MCS.Common.CustomAttributes
{
    public class CustomEmailAddressAttribute : ValidationAttribute, IClientValidatable
    {
        #region Attributes

        private string _validationGroup;

        #endregion Attributes

        #region Methods

        public CustomEmailAddressAttribute(string messageResourceKey)
        {
            ErrorMessage = messageResourceKey;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null && HttpContext.Current.Request.Form["__validationGroup"] == _validationGroup)
            {
                if (!new EmailAddressAttribute().IsValid(value))
                {
                    return new ValidationResult(validationContext.DisplayName);
                }
            }

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

            var modelClientValidationRule = new ModelClientValidationRule
            {
                ValidationType = "customemail",
                ErrorMessage = FormatErrorMessage(metadata.DisplayName)
            };

            modelClientValidationRule.ValidationParameters.Add("validationgroup", _validationGroup);

            yield return modelClientValidationRule;
        }

        #endregion IClientValidatable Members
    }
}