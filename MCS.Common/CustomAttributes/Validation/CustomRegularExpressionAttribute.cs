using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;
using MCS.Framework.Localization;

namespace MCS.Common.CustomAttributes
{
    public class CustomRegularExpressionAttribute : RegularExpressionAttribute, IClientValidatable
    {
        #region Attributes

        private string _pattern;
        private string _validationGroup;

        #endregion Attributes

        public CustomRegularExpressionAttribute(string pattern, string messageResourceKey)
            : base(pattern)
        {
            _pattern = pattern;
            ErrorMessage = messageResourceKey;
        }

        #region Methods

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null && HttpContext.Current.Request.Form["__validationGroup"] == _validationGroup)
            {
                if (!new RegularExpressionAttribute(_pattern).IsValid(value))
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
            
            var rule = new ModelClientValidationRule() 
            {
                ErrorMessage = FormatErrorMessage(metadata.GetDisplayName()),
                ValidationType = "customregularexpression" 
            };

            rule.ValidationParameters.Add("validationgroup", _validationGroup);
            rule.ValidationParameters.Add("pattern", _pattern);

            yield return rule;
        }

        #endregion IClientValidatable Members
    }
}
