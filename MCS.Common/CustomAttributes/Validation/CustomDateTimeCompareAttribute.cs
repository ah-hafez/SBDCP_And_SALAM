using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;
using MCS.Framework.Localization;

namespace MCS.Common.CustomAttributes
{
    public enum Operation
    {
        GreaterThan,
        LessThan,
        Equal,
        GreaterThanOrEqual,
        LessThanOrEqual
    }

    public class CustomDateTimeCompareAttribute : ValidationAttribute, IClientValidatable
    {
        #region Attributes

        private readonly string _propertyName;
        private readonly string _errorMessage;
        private readonly Operation _operation;
        private string _validationGroup;

        #endregion Attributes

        #region Methods

        public CustomDateTimeCompareAttribute(string propertyName, Operation operation, string errorMessage = null)
        {
            _propertyName = propertyName;
            _operation = operation;
            _errorMessage = errorMessage;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (HttpContext.Current.Request.Form["__validationGroup"] == _validationGroup)
            {
                var propertyInfo = validationContext.ObjectType.GetProperty(_propertyName);

                if (propertyInfo == null)
                {
                    return new ValidationResult(string.Format("Unknown Property {0}", _propertyName));
                }

                if (value == null || !(value is DateTime))
                {
                    return ValidationResult.Success;
                }

                var propertyValue = propertyInfo.GetValue(validationContext.ObjectInstance, null);

                if (propertyValue == null || !(propertyValue is DateTime))
                {
                    return ValidationResult.Success;
                }

                // Compare values
                switch (_operation)
                {
                    case Operation.GreaterThan:

                        if ((DateTime)value > (DateTime)propertyValue)
                        {
                            return ValidationResult.Success;
                        }
                        break;

                    case Operation.LessThan:

                        if ((DateTime)value < (DateTime)propertyValue)
                        {
                            return ValidationResult.Success;
                        }
                        break;

                    case Operation.Equal:

                        if ((DateTime)value == (DateTime)propertyValue)
                        {
                            return ValidationResult.Success;
                        }
                        break;

                    case Operation.GreaterThanOrEqual:

                        if ((DateTime)value >= (DateTime)propertyValue)
                        {
                            return ValidationResult.Success;
                        }
                        break;

                    case Operation.LessThanOrEqual:

                        if ((DateTime)value <= (DateTime)propertyValue)
                        {
                            return ValidationResult.Success;
                        }
                        break;
                }

                return new ValidationResult(FormatErrorMessage(_errorMessage));
            }

            return ValidationResult.Success;
        }

        public override string FormatErrorMessage(string name)
        {
            return DbRes.TValidation(name);
        }

        #endregion Methods

        #region IClientValidatable Members

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            _validationGroup = context.Controller.ViewBag.validationgroup;

            var rule = new ModelClientValidationRule
            {
                ErrorMessage = DbRes.TValidation(_errorMessage),
                ValidationType = "customcompare"
            };

            rule.ValidationParameters.Add("validationgroup", _validationGroup);
            rule.ValidationParameters["propertyname"] = "*." + _propertyName;
            rule.ValidationParameters["operation"] = _operation;

            yield return rule;
        }

        #endregion IClientValidatable Members
    }

    public class CustomTimeSpanCompareAttribute : ValidationAttribute, IClientValidatable
    {
        #region Attributes

        private readonly string _propertyName;
        private readonly string _errorMessage;
        private readonly Operation _operation;
        private string _validationGroup;

        #endregion Attributes

        #region Methods

        public CustomTimeSpanCompareAttribute(string propertyName, Operation operation, string errorMessage = null)
        {
            _propertyName = propertyName;
            _operation = operation;
            _errorMessage = errorMessage;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (HttpContext.Current.Request.Form["__validationGroup"] == _validationGroup)
            {
                var propertyInfo = validationContext.ObjectType.GetProperty(_propertyName);

                if (propertyInfo == null)
                {
                    return new ValidationResult(string.Format("Unknown Property {0}", _propertyName));
                }

                if (value == null || !(value is TimeSpan))
                {
                    return ValidationResult.Success;
                }

                var propertyValue = propertyInfo.GetValue(validationContext.ObjectInstance, null);

                if (propertyValue == null || !(propertyValue is TimeSpan))
                {
                    return ValidationResult.Success;
                }

                // Compare values
                switch (_operation)
                {
                    case Operation.GreaterThan:

                        if ((TimeSpan)value > (TimeSpan)propertyValue)
                        {
                            return ValidationResult.Success;
                        }
                        break;

                    case Operation.LessThan:

                        if ((TimeSpan)value < (TimeSpan)propertyValue)
                        {
                            return ValidationResult.Success;
                        }
                        break;

                    case Operation.Equal:

                        if ((TimeSpan)value == (TimeSpan)propertyValue)
                        {
                            return ValidationResult.Success;
                        }
                        break;

                    case Operation.GreaterThanOrEqual:

                        if ((TimeSpan)value >= (TimeSpan)propertyValue)
                        {
                            return ValidationResult.Success;
                        }
                        break;

                    case Operation.LessThanOrEqual:

                        if ((TimeSpan)value <= (TimeSpan)propertyValue)
                        {
                            return ValidationResult.Success;
                        }
                        break;
                }

                return new ValidationResult(FormatErrorMessage(_errorMessage));
            }

            return ValidationResult.Success;
        }

        public override string FormatErrorMessage(string name)
        {
            return DbRes.TValidation(name);
        }

        #endregion Methods

        #region IClientValidatable Members

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            _validationGroup = context.Controller.ViewBag.validationgroup;

            var rule = new ModelClientValidationRule
            {
                ErrorMessage = DbRes.TValidation(_errorMessage),
                ValidationType = "customcomparetime"
            };

            rule.ValidationParameters.Add("validationgroup", _validationGroup);
            rule.ValidationParameters["propertyname"] = _propertyName;
            rule.ValidationParameters["operation"] = _operation;

            yield return rule;
        }

        #endregion IClientValidatable Members
    }


    public class CustomNumberCompareAttribute : ValidationAttribute, IClientValidatable
    {
        #region Attributes

        private readonly string _propertyName;
        private readonly string _errorMessage;
        private readonly Operation _operation;
        private string _validationGroup;

        #endregion Attributes

        #region Methods

        public CustomNumberCompareAttribute(string propertyName, Operation operation, string errorMessage = null)
        {
            _propertyName = propertyName;
            _operation = operation;
            _errorMessage = errorMessage;
        }

        protected override ValidationResult IsValid(object numberValue, ValidationContext validationContext)
        {
            int? value = Convert.ToInt32(numberValue);

            if (HttpContext.Current.Request.Form["__validationGroup"] == _validationGroup)
            {
                var propertyInfo = validationContext.ObjectType.GetProperty(_propertyName);

                if (propertyInfo == null)
                {
                    return new ValidationResult(string.Format("Unknown Property {0}", _propertyName));
                }

                if (value == null)
                {
                    return ValidationResult.Success;
                }

                int? propertyValue = Convert.ToInt32(propertyInfo.GetValue(validationContext.ObjectInstance));

                if (propertyValue == null)
                {
                    return ValidationResult.Success;
                }

                // Compare values
                switch (_operation)
                {
                    case Operation.GreaterThan:

                        if (value > propertyValue)
                        {
                            return ValidationResult.Success;
                        }
                        break;

                    case Operation.LessThan:

                        if (value < propertyValue)
                        {
                            return ValidationResult.Success;
                        }
                        break;

                    case Operation.Equal:

                        if (value == propertyValue)
                        {
                            return ValidationResult.Success;
                        }
                        break;

                    case Operation.GreaterThanOrEqual:

                        if (value >= propertyValue)
                        {
                            return ValidationResult.Success;
                        }
                        break;

                    case Operation.LessThanOrEqual:

                        if (value <= propertyValue)
                        {
                            return ValidationResult.Success;
                        }
                        break;
                }

                return new ValidationResult(FormatErrorMessage(_errorMessage));
            }

            return ValidationResult.Success;
        }

        public override string FormatErrorMessage(string name)
        {
            return DbRes.TValidation(name);
        }

        #endregion Methods

        #region IClientValidatable Members

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            _validationGroup = context.Controller.ViewBag.validationgroup;

            var rule = new ModelClientValidationRule
            {
                ErrorMessage = DbRes.TValidation(_errorMessage),
                ValidationType = "customcomparenumber"
            };

            rule.ValidationParameters.Add("validationgroup", _validationGroup);
            rule.ValidationParameters["propertyname"] = _propertyName;
            rule.ValidationParameters["operation"] = _operation;

            yield return rule;
        }

        #endregion IClientValidatable Members
    }
}
