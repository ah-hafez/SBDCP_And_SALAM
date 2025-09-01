using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;
using MCS.Framework.Localization;

namespace MCS.Common.CustomAttributes
{
    public class CustomStringLengthAttribute : StringLengthAttribute, IClientValidatable
    {
        #region Attributes

        private string _validationgroup;
        private int _max;

        #endregion Attributes

        public CustomStringLengthAttribute(string messageResourceKey, int maximumLength, int minimumLength = 0)
            : base(maximumLength)
        {
            _max = maximumLength;
            MinimumLength = minimumLength;
            ErrorMessage = messageResourceKey;
        }

        #region Methods

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null && HttpContext.Current.Request.Form["__validationGroup"] == _validationgroup)
            {
                if (!new StringLengthAttribute(_max).IsValid(value.ToString()))
                {
                    return new ValidationResult(this.ErrorMessage, new[] { validationContext.MemberName });
                }
            }

            return ValidationResult.Success;
        }

        #endregion Methods

        #region IClientValidatable Members

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            _validationgroup = context.Controller.ViewBag.validationgroup;

            var rule = new ModelClientValidationRule()
            {
                ErrorMessage = FormatErrorMessage(metadata.GetDisplayName()),
                ValidationType = "customstringlength"
            };

            rule.ValidationParameters.Add("validationgroup", _validationgroup);
            rule.ValidationParameters.Add("maximum", _max);
            rule.ValidationParameters.Add("minimum", MinimumLength);

            yield return rule;
        }

        public override string FormatErrorMessage(string name)
        {
            return DbRes.TValidation(ErrorMessage);
        }

        #endregion IClientValidatable Members
    }
}
