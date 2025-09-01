using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using MCS.Framework.Localization;

namespace MCS.Common.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ValueExistsInListAttribute : ValidationAttribute, IClientValidatable
    {
        public string AssociatedListFieldName { get; set; }
        public string FieldNameToCompareValueWith { get; set; }
        public string FieldNameToExcludeFromList { get; set; }
        public string FieldNameToCompareWithExclude { get; set; }
        public int ValueMinCount { get; set; }
        public int ValueMaxCount { get; set; }
        public string LErrorMessage
        {
            get
            {
                return DbRes.TValidation(ErrorMessageResourceName);
            }
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            object FieldValueToCompareWithExclude = null;
            if (!string.IsNullOrWhiteSpace(FieldNameToCompareWithExclude))
            {
                FieldValueToCompareWithExclude = validationContext.ObjectInstance.GetPropertyValueByPath(FieldNameToCompareWithExclude);
            }

            object associatedList = validationContext.ObjectType.GetProperty(AssociatedListFieldName).GetValue(validationContext.ObjectInstance, null);

            int valueCount = associatedList == null
                                ? 0
                                : (associatedList as IEnumerable<object>).Count(item =>
                                                                               object.Equals(value, item.GetPropertyValueByPath(FieldNameToCompareValueWith))
                                                                               &&
                                                                               (
                                                                                 object.Equals(FieldValueToCompareWithExclude, null)
                                                                                 ||
                                                                                 object.Equals(FieldNameToExcludeFromList, null)
                                                                                 ||
                                                                                 !object.Equals(FieldValueToCompareWithExclude, item.GetPropertyValueByPath(FieldNameToExcludeFromList))
                                                                               ));

            if (
                ((associatedList == null || (associatedList as IList).Count == 0) && ValueMinCount > 0) ||
                (associatedList != null && (associatedList as IList).Count > 0 && valueCount < ValueMinCount) ||
                (associatedList != null && (associatedList as IList).Count > 0 && valueCount > ValueMaxCount)
               )
            {
                return new ValidationResult(LErrorMessage);
            }

            return ValidationResult.Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = LErrorMessage,
                ValidationType = "valueexistsinlist"
            };

            rule.ValidationParameters["associatedlistfieldname"] = AssociatedListFieldName;
            rule.ValidationParameters["fieldnametocomparevaluewith"] = FieldNameToCompareValueWith;
            rule.ValidationParameters["fieldnametoexcludefromlist"] = FieldNameToExcludeFromList;
            rule.ValidationParameters["fieldnametocomparewithexclude"] = context.Controller.ViewData.TemplateInfo.GetFullHtmlFieldName(FieldNameToCompareWithExclude);
            rule.ValidationParameters["valuemincount"] = ValueMinCount;
            rule.ValidationParameters["valuemaxcount"] = ValueMaxCount;

            yield return rule;
        }
    }
}