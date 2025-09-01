using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;
using MCS.Framework.Localization;

namespace MCS.Common.CustomAttributes
{
    [Serializable]
    public class CustomCompareAttribute : System.ComponentModel.DataAnnotations.CompareAttribute, IClientValidatable
    {
        #region Attributes

        private string _validationGroup;
        private string _otherProperty;
        private RequiredAttribute _innerAttribute = new RequiredAttribute();

        #endregion Attributes

        #region Methods

        public CustomCompareAttribute(string otherProperty, string messageResourceKey)
            : base(otherProperty)
        {
            _otherProperty = otherProperty;
            ErrorMessage = messageResourceKey;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null && HttpContext.Current.Request.Form["__validationGroup"] == _validationGroup)
            {
                if (_innerAttribute.IsValid(value))
                {
                    if (!new System.ComponentModel.DataAnnotations.CompareAttribute(_otherProperty).IsValid(value))
                    {
                        return new ValidationResult(this.ErrorMessage, new[] { validationContext.MemberName });
                    }
                }
            }

            return ValidationResult.Success;
        }

        #endregion Methods

        #region IClientValidatable Members

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            _validationGroup = context.Controller.ViewBag.validationgroup;

            string _errorMessage = context.Controller.ViewBag.ErrorMessage;

            if (string.IsNullOrEmpty(context.Controller.ViewBag.ErrorMessage))
            {
                _errorMessage = FormatErrorMessage(metadata.GetDisplayName());
            }

            var rule = new ModelClientValidationRule()
            {
                ErrorMessage = _errorMessage,
                ValidationType = "customcomparestring"
            };

            rule.ValidationParameters.Add("validationgroup", _validationGroup);
            rule.ValidationParameters.Add("other", "*." + _otherProperty);

            yield return rule;
        }

        public override string FormatErrorMessage(string name)
        {
            return DbRes.TValidation(ErrorMessage);
        }

        #endregion IClientValidatable Members
    }
}
