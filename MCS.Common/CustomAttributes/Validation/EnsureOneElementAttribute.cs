using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using MCS.Framework.Localization;

namespace MCS.Common.CustomAttributes
{
    public class EnsureOneElementAttribute : ValidationAttribute, IClientValidatable
    {
        #region Attributes

        private string _propertyName = string.Empty;

        #endregion Attributes

        public EnsureOneElementAttribute(string messageResourceKey, string propertyName)
        {
            ErrorMessage = messageResourceKey;
            _propertyName = propertyName;
        }

        #region Methods

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            IList list = value as IList;

            bool valid = false;

            foreach (var item in list)
            {
                var property = Convert.ToBoolean(item.GetType().GetProperty(_propertyName).GetValue(item));

                if (property)
                {
                    valid = true;
                    break;
                }
            }

            if (valid)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(validationContext.DisplayName);
        }

        #endregion Methods

        #region IClientValidatable Members

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule()
            {
                ErrorMessage = FormatErrorMessage(metadata.GetDisplayName()),
                ValidationType = "require"
            };

            rule.ValidationParameters.Add("propertyname", _propertyName);

            yield return rule;
        }

        public override string FormatErrorMessage(string name)
        {
            return DbRes.TValidation(name);
        }

        #endregion IClientValidatable Members
    }
}

