using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;
using MCS.Framework.Localization;

namespace MCS.Common.CustomAttributes
{
    public class CustomRangeAttribute : RangeAttribute, IClientValidatable
    {
        #region Attributes

        private string _validationGroup;
        private double _min;
        private double _max;

        #endregion Attributes

        public CustomRangeAttribute(string messageResourceKey, int minValue, int maxValue)
            : base(minValue, maxValue)
        {
            _min = minValue;
            _max = maxValue;
            ErrorMessage = messageResourceKey;
        }

        #region Methods

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null && HttpContext.Current.Request.Form["__validationGroup"] == _validationGroup)
            {
                if (!new RangeAttribute(_min, _max).IsValid(value))
                {
                    return new ValidationResult(this.ErrorMessage, new[] { validationContext.MemberName });
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
                ValidationType = "customrange" 
            };

            rule.ValidationParameters.Add("validationgroup", _validationGroup);
            rule.ValidationParameters.Add("minimum", _min);
            rule.ValidationParameters.Add("maximum", _max);

            yield return rule;
        }

        #endregion IClientValidatable Members
    }
}
