using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using System.Web.Script.Serialization;
using MCS.Framework.Controls;
using MCS.Framework.Controls.Mvc;
using MCS.Framework.Encryption;
using MCS.Framework.Localization;
using MCS.Common;
using MCS.Common.ApiControllerResults;
using MCS.Common.CustomAttributes;
using MCS.UI.Areas.Admin.Models.Shared;
using MCS.UI.Areas.User.Models.Lookups;
namespace MCS.UI
{
    public static class HtmlHelperExt
    {
        private static string GetVirtualPath(HtmlHelper htmlhelper)
        {
            WebFormView view = htmlhelper.ViewContext.View as WebFormView;
            if (view != null)
            {
                return view.ViewPath;
            }

            return null;
        }
        public static IDisposable BeginCollectionItem(this HtmlHelper html, string collectionName)
        {
            string itemIndex = Guid.NewGuid().ToString();
            html.ViewContext.Writer.WriteLine(string.Format("<input type=\"hidden\" name=\"{0}.index\" autocomplete=\"off\" value=\"{1}\" />", collectionName, html.Encode(itemIndex)));
            return BeginHtmlFieldPrefixScope(html, string.Format("{0}[{1}]", collectionName, itemIndex));
        }
        public static IDisposable BeginChildItem(this HtmlHelper html, string itemNameInParent)
        {
            return BeginHtmlFieldPrefixScope(html, itemNameInParent);
        }
        public static IDisposable BeginHtmlFieldPrefixScope(this HtmlHelper html, string htmlFieldPrefix)
        {
            return new HtmlFieldPrefixScope(html.ViewData.TemplateInfo, htmlFieldPrefix);
        }
        public static MvcHtmlString FixIDAndName(this MvcHtmlString htmlString, string IdPartToRemove, string NamePartToRemove)
        {
            return MvcHtmlString.Create(htmlString.ToHtmlString().Replace(IdPartToRemove, "").Replace(NamePartToRemove, ""));
        }
        public static MvcHtmlString AutoCompleteBuilder(this HtmlHelper html, string autoCompleteControlid,
            string hdnIdToSaveValue, LookupCategory lookupCategory, string selectedId = "", bool matchAnywhere = true,
            string inputClassName = "", string ulClassName = "",
            string buttonId = "", bool selectFirstIndex = false, string validationGroup = "", string waterMarkText = "",
            int maxLengthText = 40, string onChangeCallback = "")
        {
            string inputClass = String.Concat(inputClassName, " fx_control");
            string validationClass = "invalid-input";
            string items = "";
            string selectedValue = selectedId;
            // call lookups manager
            html.ViewContext.Controller.ViewBag.validationGroup = validationGroup;
            GetResult<IList<LookupVM>> lookups = LookupsHelper.GetLookupItems(lookupCategory, SessionInfo.CultureShortName);
            if (lookupCategory == LookupCategory.Year)
            {
                string date = DateTimeUtility.ConvertToUmAlQuraCalendar(DateTime.Now);
                int year = Convert.ToInt32(date.Split('/')[0]);
                lookups.Result = lookups.Result.ToList().Where(l => Convert.ToInt32(l.Text) <= year && l.Sort > 0).OrderByDescending(a => a.Text).ToList();
            }
            IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();
            if (lookups.Result != null)
            {
                foreach (LookupVM lookupVM in lookups.Result)
                {
                    if (lookupVM.Text == selectedId)
                    {
                        selectedValue = lookupVM.Id.ToString();
                    }
                    dataSource.Add(new AutoCompleteDataSource()
                    {
                        Value = lookupVM.Id.ToString(),
                        Label = lookupVM.Text
                    });
                }
            }
            items = JsonConvert.SerializeObject(dataSource);
            return AutoCompleteExtensions.AutoComplete(html, autoCompleteControlid, hdnIdToSaveValue, items, matchAnywhere, selectedValue, inputClass, ulClassName, buttonId, null, selectFirstIndex, validationClass, waterMarkText, maxLengthText, onChangeCallback);
        }
        public static MvcHtmlString CalendarCustom(this HtmlHelper html, string CalendarControlId, string hdnIdToSaveGregorianDate, string hdnIdToSaveUmmalquraDate, CalenderType calendarName, string languageShortName, string defaultDate = "", string className = "")
        {
            SettingVM DateType = SessionInfo.GetObjectFromSession(Constants.SettingDate) as SettingVM;
            var adminDates = GetDateLookups(LookupCategory.DateType);
            var DateValue = adminDates.FirstOrDefault().Value;
            var settingDateType = DateType != null ? DateType.Value : DateValue;
            if (settingDateType != null)
            {
                if (settingDateType == DateValue)
                {
                    calendarName = CalenderType.Ummalqura;
                }
                else
                {
                    calendarName = CalenderType.Gregorian;
                }
            }
            return MvcHtmlString.Create(Framework.Controls.Calendar.RenderCalendar(CalendarControlId, hdnIdToSaveGregorianDate, hdnIdToSaveUmmalquraDate, calendarName, languageShortName, defaultDate, className));
        }
        public static List<AutoCompleteDataSource> GetDateLookups(LookupCategory lookupCategory)
        {
            try
            {

                var dataSource = new List<AutoCompleteDataSource>();
                GetResult<IList<LookupVM>> lookups = LookupsHelper.GetLookupItems(lookupCategory, SessionInfo.CultureShortName);
                if (lookups.Result != null)
                {
                    foreach (var item in lookups.Result)
                    {
                        dataSource.Add(new AutoCompleteDataSource()
                        {
                            Value = item.Id.ToString(),
                            Label = item.Text
                        });
                    }
                }
                return dataSource.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static MvcHtmlString AutoCompleteBuilder(this HtmlHelper html, string autoCompleteControlid,
           string hdnIdToSaveValue, string items, string selectedId = "", bool matchAnywhere = true,
           string inputClassName = "", string hdnExtraParametersId = "", string ulClassName = "", string buttonId = "", bool selectFirstIndex = false, string validationGroup = "", string onChangeCallback = "")
        {
            string inputClass = String.Concat(inputClassName, " fx_control");
            string validationClass = "invalid-input";
            html.ViewContext.Controller.ViewBag.validationGroup = validationGroup;
            return AutoCompleteExtensions.AutoComplete(html, autoCompleteControlid, hdnIdToSaveValue, items, matchAnywhere, selectedId, inputClassName, ulClassName, buttonId, hdnExtraParametersId, selectFirstIndex, validationClass, onChangeCallback: onChangeCallback);
        }
        public static MvcHtmlString AutoCompleteBuilderWithKeyup(this HtmlHelper html, string autoCompleteControlid,
           string hdnIdToSaveValue, string items, string selectedId = "", bool matchAnywhere = true,
           string inputClassName = "", string hdnExtraParametersId = "", string ulClassName = "", string buttonId = "", bool selectFirstIndex = false, string validationGroup = "", string onChangeCallback = "", string onKeyUpCallback = "")
        {
            string inputClass = String.Concat(inputClassName, " fx_control");
            string validationClass = "invalid-input";
            html.ViewContext.Controller.ViewBag.validationGroup = validationGroup;
            return AutoCompleteExtensions.AutoComplete(html, autoCompleteControlid, hdnIdToSaveValue, items, matchAnywhere, selectedId, inputClassName, ulClassName, buttonId, hdnExtraParametersId, selectFirstIndex, validationClass, onChangeCallback: onChangeCallback, onKeyUpCallback: onKeyUpCallback);
        }
        public static MvcHtmlString NumericTextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null, string validationGroup = "", string errorMessage = "")
        {
            StringBuilder sb = new StringBuilder();
            helper.ViewContext.Controller.ViewBag.validationGroup = validationGroup;
            helper.ViewContext.Controller.ViewBag.ErrorMessage = errorMessage;
            if (errorMessage != null && errorMessage != "")
            {
                helper.ViewContext.Controller.ViewBag.ErrorMessage = errorMessage;
            }
            var memberExpression = expression.Body as MemberExpression;
            var customStringLength = memberExpression.Member.GetCustomAttributes(typeof(CustomStringLengthAttribute), true);
            if (customStringLength.Length == 1)
            {
                int maxlength = ((StringLengthAttribute)(customStringLength[0])).MaximumLength;
                var Attributes = helper.MergeHtmlAttributes(htmlAttributes, new { maxlength = maxlength, onkeypress = "return IsNumeric(event);" });
                sb.Append(helper.TextBoxFor(expression, Attributes));
            }
            else
            {
                var Attributes = helper.MergeHtmlAttributes(htmlAttributes, new { maxlength = 100, onkeypress = "return IsNumeric(event);" });
                sb.Append(helper.TextBoxFor(expression, Attributes));
            }
            sb.Append(helper.ValidationMessageFor(expression));
            return MvcHtmlString.Create(sb.ToString());
        }

        public static MvcHtmlString CustomRadioButtonFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, object value, object htmlAttributes = null)
        {
            StringBuilder sb = new StringBuilder();
            var memberExpression = expression.Body as MemberExpression;
            sb.Append(helper.RadioButtonFor(expression, value, htmlAttributes));
            sb.Append("<span class=\"cr\"><i class=\"cr-icon\"></i></span>");
            return MvcHtmlString.Create(sb.ToString());
        }

        public static MvcHtmlString TextAreaGroupFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null, string validationGroup = "")
        {
            StringBuilder sb = new StringBuilder();
            var memberExpression = expression.Body as MemberExpression;
            var customStringLength = memberExpression.Member.GetCustomAttributes(typeof(CustomStringLengthAttribute), true);
            helper.ViewContext.Controller.ViewBag.validationGroup = validationGroup;
            if (customStringLength.Length == 1)
            {
                int maxlength = ((StringLengthAttribute)(customStringLength[0])).MaximumLength;
                IDictionary<string, object> attributes = helper.MergeHtmlAttributes(htmlAttributes, new { @maxlength = maxlength });
                sb.Append(helper.TextAreaFor(expression, attributes));
            }
            else
            {
                IDictionary<string, object> attributes = helper.MergeHtmlAttributes(htmlAttributes, new { @maxlength = 500 });
                sb.Append(helper.TextAreaFor(expression, htmlAttributes));
            }
            sb.Append(helper.ValidationMessageFor(expression));
            return MvcHtmlString.Create(sb.ToString());
        }
        public static MvcHtmlString ClientCheckBox(this HtmlHelper helper,
            string checkBoxId, string cssClass = null, string permissionCode = "")
        {
            if (!string.IsNullOrEmpty(permissionCode))
            {
                if (!SessionInfo.CurrentUser.Claims.Contains(permissionCode))
                {
                    return new MvcHtmlString(string.Empty);
                }
            }
            TagBuilder input = new TagBuilder("input");
            input.Attributes.Add("type", "checkbox");
            input.Attributes.Add("name", checkBoxId);
            input.Attributes.Add("id", checkBoxId);
            input.Attributes.Add("class", cssClass);
            return MvcHtmlString.Create(input.ToString(TagRenderMode.Normal));
        }
        public static MvcHtmlString CustomCheckBox(this HtmlHelper helper,
            string checkBoxName, object htmlAttributes = null, string permissionCode = "")
        {
            if (!string.IsNullOrEmpty(permissionCode))
            {
                if (!SessionInfo.CurrentUser.Claims.Contains(permissionCode))
                {
                    return new MvcHtmlString(string.Empty);
                }
            }
            return MvcHtmlString.Create(helper.CheckBox(checkBoxName, htmlAttributes).ToString());
        }
        public static MvcHtmlString CustomCheckBoxFor<TModel>(this HtmlHelper<TModel> helper, Expression<Func<TModel, bool>> expression, object htmlAttributes, string text, string permissionCode = "")
        {
            if (!string.IsNullOrEmpty(permissionCode))
            {
                if (!SessionInfo.CurrentUser.Claims.Contains(permissionCode))
                {
                    return new MvcHtmlString(string.Empty);
                }
            }
            StringBuilder sb = new StringBuilder();
            sb.Append(helper.CheckBoxFor(expression, htmlAttributes));
            TagBuilder label = new TagBuilder("label");
            label.InnerHtml = text;
            sb.Append(label.ToString(TagRenderMode.Normal));
            return MvcHtmlString.Create(sb.ToString());
        }
        public static MvcHtmlString HiddenGroupFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes, string validationGroup = "", string errorMessage = "")
        {
            StringBuilder sb = new StringBuilder();
            helper.ViewContext.Controller.ViewBag.validationGroup = validationGroup;
            helper.ViewContext.Controller.ViewBag.ErrorMessage = errorMessage;
            sb.Append(helper.HiddenFor(expression, htmlAttributes));
            sb.Append(helper.ValidationMessageFor(expression));
            return MvcHtmlString.Create(sb.ToString());
        }
        public static MvcHtmlString HiddenGroupFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object> htmlAttributes, string validationGroup = "", string errorMessage = "")
        {
            StringBuilder sb = new StringBuilder();
            helper.ViewContext.Controller.ViewBag.validationGroup = validationGroup;
            helper.ViewContext.Controller.ViewBag.ErrorMessage = errorMessage;
            sb.Append(helper.HiddenFor(expression, htmlAttributes));
            sb.Append(helper.ValidationMessageFor(expression));
            return MvcHtmlString.Create(sb.ToString());
        }
        public static MvcHtmlString HiddenTimeSpanFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, string hourTextId, string minuteTextId, string validationGroup = "")
        {
            string propertyName = ExpressionHelper.GetExpressionText(expression);
            StringBuilder sb = new StringBuilder();
            helper.ViewContext.Controller.ViewBag.validationGroup = validationGroup;
            sb.Append("<script>$(document).on('focusout','#" + hourTextId + "',function(){AppendHours('hdnTimeSpan" + propertyName + "','" + hourTextId + "' )})</script>");
            sb.Append("<script>$(document).on('focusout','#" + minuteTextId + "',function(){AppendMinutes('hdnTimeSpan" + propertyName + "','" + minuteTextId + "' )})</script>");
            sb.Append(helper.HiddenFor(expression, new { id = "hdnTimeSpan" + propertyName }));
            sb.Append(helper.ValidationMessageFor(expression));
            return MvcHtmlString.Create(sb.ToString());
        }
        //public static MvcHtmlString HiddenGroup<TModel, TProperty>(this HtmlHelper<TModel> helper, string hdnName, IDictionary<string, object> htmlAttributes, string validationGroup = "")
        //{
        //    StringBuilder sb = new StringBuilder();
        //    helper.ViewContext.Controller.ViewBag.validationGroup = validationGroup;
        //    sb.Append(helper.Hidden(hdnName, htmlAttributes));
        //    sb.Append(helper.ValidationMessageFor(expression));
        //    return MvcHtmlString.Create(sb.ToString());
        //}
        public static MvcHtmlString StickyHiddenFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null)
        {
            var Attributes = helper.MergeHtmlAttributes(htmlAttributes, new { @class = "__hdnSticky" });
            return helper.HiddenFor(expression, Attributes);
        }
        public static MvcHtmlString StickyHidden(this HtmlHelper helper, string name, string value = "", object htmlAttributes = null)
        {
            string prefix = helper.ViewData.TemplateInfo.HtmlFieldPrefix;
            helper.ViewData.TemplateInfo.HtmlFieldPrefix = null;
            var Attributes = helper.MergeHtmlAttributes(htmlAttributes, new { @class = "__hdnSticky" });
            var hidden = helper.Hidden(name, value, Attributes);
            helper.ViewData.TemplateInfo.HtmlFieldPrefix = prefix;
            return hidden;
        }
        public static MvcHtmlString EncryptedStickyHidden(this HtmlHelper helper, string name, string value = "", object htmlAttributes = null)
        {
            string prefix = helper.ViewData.TemplateInfo.HtmlFieldPrefix;
            helper.ViewData.TemplateInfo.HtmlFieldPrefix = null;
            var Attributes = helper.MergeHtmlAttributes(htmlAttributes, new { @class = "__hdnSticky" });
            value = AESEncrytDecry.Base64Encode(value);
            var hidden = helper.Hidden(name, value, Attributes);
            helper.ViewData.TemplateInfo.HtmlFieldPrefix = prefix;
            return hidden;
        }

        public static MvcHtmlString TextBoxGroupFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, string validationGroup = "", object htmlAttributes = null, string errorMessage = "")
        {
            StringBuilder sb = new StringBuilder();
            var memberExpression = expression.Body as MemberExpression;
            var customStringLength = memberExpression.Member.GetCustomAttributes(typeof(CustomStringLengthAttribute), true);
            helper.ViewContext.Controller.ViewBag.validationGroup = validationGroup;
            helper.ViewContext.Controller.ViewBag.ErrorMessage = errorMessage;
            if (customStringLength.Length == 1)
            {
                int maxlength = ((StringLengthAttribute)(customStringLength[0])).MaximumLength;
                IDictionary<string, object> attributes = helper.MergeHtmlAttributes(htmlAttributes, new { @maxlength = maxlength });
                sb.Append(helper.TextBoxFor(expression, attributes));
            }
            else
            {
                IDictionary<string, object> attributes = helper.MergeHtmlAttributes(htmlAttributes, new { @maxlength = 100 });
                sb.Append(helper.TextBoxFor(expression, htmlAttributes));
            }
            sb.Append(helper.ValidationMessageFor(expression));
            return MvcHtmlString.Create(sb.ToString());
        }
        public static MvcHtmlString PasswordGroupFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, string validationGroup = "", object htmlAttributes = null, string errorMessage = "")
        {
            StringBuilder sb = new StringBuilder();
            helper.ViewContext.Controller.ViewBag.validationGroup = validationGroup;
            helper.ViewContext.Controller.ViewBag.ErrorMessage = errorMessage;
            sb.Append(helper.PasswordFor(expression, htmlAttributes));
            sb.Append(helper.ValidationMessageFor(expression));
            return MvcHtmlString.Create(sb.ToString());
        }
        public static MvcHtmlString CurrencyTextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes, string validationGroup = "")
        {
            StringBuilder sb = new StringBuilder();
            var memberExpression = expression.Body as MemberExpression;
            var customStringLength = memberExpression.Member.GetCustomAttributes(typeof(CustomStringLengthAttribute), true);
            if (customStringLength.Length == 1)
            {
                int maxlength = ((StringLengthAttribute)(customStringLength[0])).MaximumLength;
                var Attributes = helper.MergeHtmlAttributes(htmlAttributes, new { maxlength = maxlength, onkeypress = "return isDecimal(event);" });
                sb.Append(helper.TextBoxFor(expression, Attributes));
            }
            else
            {
                var Attributes = helper.MergeHtmlAttributes(htmlAttributes, new { onkeypress = "return isDecimal(event);" });
                sb.Append(helper.TextBoxFor(expression, Attributes));
            }
            sb.Append(helper.ValidationMessageFor(expression));
            helper.ViewContext.Controller.ViewBag.validationGroup = validationGroup;
            return MvcHtmlString.Create(sb.ToString());
        }
        public static MvcHtmlString TextBoxGroup(this HtmlHelper helper, string name, string validationGroup = "")
        {
            helper.ViewContext.Controller.ViewBag.validationGroup = validationGroup;
            return helper.TextBox(name);
        }
        public static MvcHtmlString MenuCaller<TModel, TProperty>(this HtmlHelper<TModel> helper, string path, Expression<Func<TModel, TProperty>> expression, string validationGroup = "")
        {
            var sb = new StringBuilder();
            sb.Append("<div class='col3'><div class='col3'>");
            var text = helper.TextBoxFor(expression, new { @class = "txtDepartmentId", @Value = "" });
            sb.Append(text);
            sb.Append("</div> <div class='col8'><input class='txtDepartmentName' readonly='readonly' type='text'></div></div>");
            sb.Append("<div id='menuCol' class='btnCol expandPopupBtn'><a href='#' class='btnCol' title='استعرض'><div class='btnTools btnToolsBlue'> <img src='../Images/edit.png' width='24' height='24' style='margin:1px -3px 0 0;'> </div></a></div>");
            sb.Append(helper.Partial(path));
            helper.ViewContext.Controller.ViewBag.validationGroup = validationGroup;
            return MvcHtmlString.Create(sb.ToString());
        }
        #region Tree
        #region Tree
        public static MvcHtmlString Tree<TModel, TProperty>(this HtmlHelper<TModel> helper, TreeViewModel treeViewModel, Expression<Func<TModel, TProperty>> expression, string validationGroup = "")
        {
            var sb = new StringBuilder();
            sb.Append("<div class='tree_parties space'>");
            sb.Append("<ul class='tree' >");
            HtmlHelperExt.TreeNode(helper, treeViewModel.RootNode, expression, ref sb);
            sb.Append("</ul>");
            sb.Append("</div>");
            return MvcHtmlString.Create(sb.ToString());
        }
        private static void TreeNode<TModel, TProperty>(this HtmlHelper<TModel> helper, TreeNode treeNode, Expression<Func<TModel, TProperty>> expression, ref StringBuilder sb)
        {
            foreach (var node in treeNode.Childs)
            {
                string key = Guid.NewGuid().ToString();
                if (node != null)
                {
                    if (node.Childs.Count > 0)
                    {
                        sb.Append("<li id='li_" + node.Id + "'>");
                        sb.Append("<div class='row row_directory selected_bottom' id='divspan_" + node.Id + "'>");

                        sb.Append("<div data-value='" + node.Id + "' class='col-md-8  col_text' id='" + key + "'>" + node.Name + "</div>");
                        sb.Append("<div class='col-md-4'>");
                        sb.Append("<a class='node' >");
                        sb.Append("<span id='span_" + node.Id + "' class='fa fa-plus'></span>");
                        sb.Append("</a>");
                        sb.Append("</div>");
                        sb.Append("<div class='col-md-2'>");
                        sb.Append("<div class='row control_name'>");
                        sb.Append("<div class='col-md-6'>");
                        sb.Append("</div>");
                        sb.Append("<div class='col-md-6'>");
                        sb.Append("</div>");
                        sb.Append("</div>");
                        sb.Append("</div>");
                        sb.Append("</div>");
                        sb.Append("<ul id='ul_" + node.Id + "' style='display:none'>");
                        HtmlHelperExt.TreeNode(helper, node, expression, ref sb);
                        sb.Append("</ul>");
                        sb.Append("</li>");
                    }
                    else
                    {
                        sb.Append("<li id='li_" + node.Id + "'>");
                        sb.Append("<div class='row row_directory last'>");
                        //sb.Append("<div class='col-md-1'>");
                        //sb.Append("</div>");
                        sb.Append("<div data-value='" + node.Id + "' class='col-md-8  col_text' id='" + key + "'>" + node.Name + "</div>");
                        sb.Append("<div class='col-md-2'>");
                        sb.Append("<div class='row control_name'>");
                        sb.Append("<div class='col-md-6'>");
                        sb.Append("<a class='col_cancel'  style='font-size:13px; display:none;'>إلغاء</a>");
                        sb.Append("</div>");
                        sb.Append("<div class='col-md-6'>");
                        sb.Append(helper.HiddenFor(expression, new { id = "__hdnText" }));
                        sb.Append("<a style='display:none;' class='save_text' ><i class='fa fa-save'></i></a>");
                        sb.Append("<a class='edit_text' title='" + DbRes.TResource("Global.Edit") + "' alt='" + DbRes.TResource("Global.Edit") + "'><i class='fa fa-edit'></i></a>");
                        sb.Append("</div>");
                        sb.Append("</div>");
                        sb.Append("</div>");
                        sb.Append("</div>");
                        sb.Append("</li>");
                    }
                }
            }
        }
        public static MvcHtmlString PermissionsTree<TModel, TProperty>(this HtmlHelper<TModel> helper, TreeViewModel treeViewModel, Expression<Func<TModel, TProperty>> expression, TreeMode treeMode, string treeId, bool isAjaxCall, bool enableChildsSelectionByParent = false, string validationGroup = "", string divContainerId = "")
        {
            if (treeMode == TreeMode.Multiple)
            {
                return HtmlHelperExt.PermissionsMultipleTree(helper, treeViewModel, expression, treeMode, treeId, isAjaxCall, validationGroup);
            }
            else
            {
                return HtmlHelperExt.PermissionsSingleTree(helper, treeViewModel, expression, treeMode, treeId, isAjaxCall, validationGroup, divContainerId);
            }
        }
        private static MvcHtmlString PermissionsSingleTree<TModel, TProperty>(this HtmlHelper<TModel> helper, TreeViewModel treeViewModel, Expression<Func<TModel, TProperty>> expression, TreeMode treeMode, string container, bool isAjaxCall, string validationGroup = "", string divContainerId = "")
        {
            string propertyName = ExpressionHelper.GetExpressionText(expression);
            StringBuilder sb = new StringBuilder();
            sb.Append("<div class='form-group' id='divForm__" + container + "'>");
            sb.Append(helper.LabelFor(expression, new { @class = "col-md-4 control-label" }));
            sb.Append("<div class='col-md-8'>");
            sb.Append("<div class='row'>");
            sb.Append("<div class='input-group rtl_btn'>");
            sb.Append("<span class='input-group-addon link'>");
            sb.Append("<div class='panel-title'>");
            sb.Append("<a onclick=ShowPermissionSingleTree('#divContainer__" + container + "') ><span class='fa fa-sort-amount-desc'> </span> </a>");
            sb.Append("</div></span>");
            sb.Append("<input type='text' data-container='" + container + "' maxlength='40' class='form-control txtPermissionName' />");
            sb.Append("</div>");
            sb.Append(helper.ValidationMessageFor(expression));
            sb.Append(helper.HiddenFor(expression, new { @class = "hdnPermissionId", @Value = "" }));
            sb.Append("</div></div></div>");
            sb.Append("<div class='accordion' id='divAcc__" + container + "'>");
            sb.Append("<div class='panel-body panel-style' style='display: none;' id='divContainer__" + container + "'>");
            sb.Append("<div id='divDir__" + container + "' class='directory'>");
            sb.Append("<div class='row'>");
            sb.Append("<div class='tree_parties'>");
            sb.Append("<div class='group_top_directory'>");
            sb.Append("<ul class='tree' >");
            HtmlHelperExt.PermissionsTreeNode(treeViewModel.RootNode, propertyName, treeMode, container, ref sb);
            sb.Append("</ul>");
            sb.Append("</div>");
            sb.Append("<div class='col-md-3'></div>");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("<script>");
            sb.Append("RepiarTreeHtml('" + container + "','" + divContainerId + "')");
            sb.Append("</script>");
            if (isAjaxCall)
            {
                sb.Append("<script>");
                sb.Append("AppendText('" + container + "')");
                sb.Append("</script>");
            }
            helper.ViewContext.Controller.ViewBag.validationGroup = validationGroup;
            return MvcHtmlString.Create(sb.ToString());
        }
        private static MvcHtmlString PermissionsMultipleTree<TModel, TProperty>(this HtmlHelper<TModel> helper, TreeViewModel treeViewModel, Expression<Func<TModel, TProperty>> expression, TreeMode treeMode, string container, bool isAjaxCall, string validationGroup = "")
        {
            string propertyName = ExpressionHelper.GetExpressionText(expression);
            var modelMetaData = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            helper.ViewContext.Controller.ViewBag.ErrorMessage = "";
            var sb = new StringBuilder();
            string required = string.Empty;
            string searchText = DbRes.TResource("Global.Tree.Search");
            if (modelMetaData.IsRequired)
            {
                required = "Required";
            }
            sb.Append("<div id='divForm__" + container + "' class='panel-group accordion " + required + "'>");
            sb.Append("<div class='permissions_area'>");
            sb.Append("<div class='form-group'>");
            sb.Append("<div class='col-md-12'>");
            sb.Append("<div class='input-group rtl_btn'>");
            sb.Append("<span class='tags'><div class='list-tags' id='divTags' data-name=" + propertyName + ">");
            sb.Append("</div></span>");
            sb.Append("<span class='input-group-addon link'>");
            sb.Append("<div class='panel-title'>");
            sb.Append("<a onclick=ShowHideTree('#divContainer__" + container + "') title=" + DbRes.TResource("Global.Tree.Show") + "><span class='fa fa-sort-amount-desc'> </span> </a>");
            sb.Append("</div></span>");
            sb.Append("</div></div></div></div>");
            if (modelMetaData.IsRequired)
            {
                sb.Append(helper.HiddenGroupFor(expression, new { @class = "__hdnPermission", @Value = "" }, validationGroup));
            }
            sb.Append("<div id='divAcc__" + container + "' class='accordion col-lg-12'>");
            sb.Append("<div class='panel-body panel-style' id='divContainer__" + container + "'>");
            sb.Append("<div id='divDir__" + container + "' class='directory'>");

            sb.Append("<div class='tree_parties space'>");
            sb.Append("<div class='group_top_directory'>");
            sb.Append("<ul class='tree' >");
            HtmlHelperExt.PermissionsTreeNode(treeViewModel.RootNode, propertyName, treeMode, container, ref sb);
            sb.Append("</ul>");
            sb.Append("</div>");
            sb.Append("<div class='col-md-3'></div>");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");
            if (isAjaxCall)
            {
                sb.Append("<script>");
                sb.Append("TagsPopulation('" + container + "')");
                sb.Append("</script>");
            }
            helper.ViewContext.Controller.ViewBag.validationGroup = validationGroup;
            return MvcHtmlString.Create(sb.ToString());
        }
        private static void PermissionsTreeNode(TreeNode treeNode, string propertyName, TreeMode treeMode, string container, ref StringBuilder sb)
        {
            string selectableClass = string.Empty;
            string isSelectedClass = string.Empty;
            string selectMode = string.Empty;
            if (treeMode == TreeMode.Multiple)
            {
                selectMode = "multiple_select";
            }
            else
            {
                selectMode = "single_select";
            }
            foreach (var node in treeNode.Childs)
            {
                selectableClass = string.Empty;
                isSelectedClass = string.Empty;
                string key = Guid.NewGuid().ToString();
                if (node != null)
                {
                    //if (node.Selectable)
                    //{
                    selectableClass = "selectable_node";
                    //}
                    if (node.IsSelected)
                    {
                        isSelectedClass = "selected";
                    }
                    if (node.Childs.Count > 0)
                    {
                        sb.Append("<li class='choice_li' data-li-id='" + node.Id + "' id='li_" + node.Id + "'>");
                        sb.Append("<div class='row row_directory " + selectableClass + " " + isSelectedClass + " selected_bottom' id='divspan_" + node.Id + container + "'>");
                        sb.Append("<div class='col-md-1'>");
                        sb.Append("<a class='node' >");
                        sb.Append("<span id='span_" + node.Id + container + "' data-node-container='" + container + "' class='fa fa-plus'></span>");
                        sb.Append("</a>");
                        sb.Append("</div>");
                        sb.Append("<div data-container='" + container + "' data-Name='" + propertyName + "' data-id='" + node.Id + "' class='col-md-8 parent " + selectMode + "' id='" + key + "'>" + node.Name + "</div>");
                        sb.Append("<div class='col-md-1 glyphicon_ok'>");
                        sb.Append("<i class='glyphicon glyphicon-ok'></i>");
                        sb.Append("</div>");
                        sb.Append("</div>");
                        sb.Append("<ul id='ul_" + node.Id + container + "' style='display:none'>");
                        HtmlHelperExt.PermissionsTreeNode(node, propertyName, treeMode, container, ref sb);
                        sb.Append("</ul>");
                        sb.Append("</li>");
                    }
                    else
                    {
                        sb.Append("<li class='choice_li' id='li_" + node.Id + "'>");
                        sb.Append("<div class='row row_directory last " + selectableClass + " " + isSelectedClass + "'>");
                        //sb.Append("<div class='col-md-1'>");
                        //sb.Append("</div>");
                        sb.Append("<div data-container='" + container + "' data-Name='" + propertyName + "' data-parent='" + treeNode.Id + "' data-value='" + node.Id + "' class='col-md-8  leaf " + selectMode + "' id='" + key + "'>" + node.Name + "</div>");
                        sb.Append("<div class='col-md-1 glyphicon_ok'>");
                        sb.Append("<i class='glyphicon glyphicon-ok'></i>");
                        sb.Append("</div>");
                        sb.Append("</div>");
                        sb.Append("</li>");
                    }
                }
            }
        }
        public static MvcHtmlString PermissionsEditableTree<TModel, TProperty>(this HtmlHelper<TModel> helper, TreeViewModel treeViewModel, Expression<Func<TModel, TProperty>> expression, TreeMode treeMode, string treeId, bool isAjaxCall, bool enableChildsSelectionByParent = false, string validationGroup = "", string divContainerId = "")
        {
            if (treeMode == TreeMode.Multiple)
            {
                return HtmlHelperExt.PermissionsMultipleEditableTree(helper, treeViewModel, expression, treeMode, treeId, isAjaxCall, validationGroup);
            }
            else
            {
                return HtmlHelperExt.PermissionsSingleEditableTree(helper, treeViewModel, expression, treeMode, treeId, isAjaxCall, validationGroup, divContainerId);
            }
        }
        private static MvcHtmlString PermissionsSingleEditableTree<TModel, TProperty>(this HtmlHelper<TModel> helper, TreeViewModel treeViewModel, Expression<Func<TModel, TProperty>> expression, TreeMode treeMode, string container, bool isAjaxCall, string validationGroup = "", string divContainerId = "")
        {
            string propertyName = ExpressionHelper.GetExpressionText(expression);
            StringBuilder sb = new StringBuilder();
            sb.Append("<div class='form-group' id='divForm__" + container + "'>");
            sb.Append(helper.LabelFor(expression, new { @class = "col-md-4 control-label" }));
            sb.Append("<div class='col-md-8'>");
            sb.Append("<div class='row'>");
            sb.Append("<div class='input-group rtl_btn'>");
            sb.Append("<span class='input-group-addon link'>");
            sb.Append("<div class='panel-title'>");
            sb.Append("<a onclick=ShowPermissionSingleTree('#divContainer__" + container + "') ><span class='fa fa-sort-amount-desc'> </span> </a>");
            sb.Append("</div></span>");
            sb.Append("<input type='text' data-container='" + container + "' maxlength='40' class='form-control txtPermissionName' />");
            sb.Append("</div>");
            sb.Append(helper.ValidationMessageFor(expression));
            sb.Append(helper.HiddenFor(expression, new { @class = "hdnPermissionId", @Value = "" }));
            sb.Append("</div></div></div>");
            sb.Append("<div class='accordion' id='divAcc__" + container + "'>");
            sb.Append("<div class='panel-body panel-style' style='display: none;' id='divContainer__" + container + "'>");
            sb.Append("<div id='divDir__" + container + "' class='directory'>");
            sb.Append("<div class='row'>");
            sb.Append("<div class='tree_parties'>");
            sb.Append("<div class='group_top_directory'>");
            sb.Append("<ul class='tree' >");
            HtmlHelperExt.PermissionsEditableTreeNode(treeViewModel.RootNode, propertyName, treeMode, container, ref sb);
            sb.Append("</ul>");
            sb.Append("</div>");
            sb.Append("<div class='col-md-3'></div>");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("<script>");
            sb.Append("RepiarTreeHtml('" + container + "','" + divContainerId + "')");
            sb.Append("</script>");
            if (isAjaxCall)
            {
                sb.Append("<script>");
                sb.Append("AppendText('" + container + "')");
                sb.Append("</script>");
            }
            helper.ViewContext.Controller.ViewBag.validationGroup = validationGroup;
            return MvcHtmlString.Create(sb.ToString());
        }
        private static MvcHtmlString PermissionsMultipleEditableTree<TModel, TProperty>(this HtmlHelper<TModel> helper, TreeViewModel treeViewModel, Expression<Func<TModel, TProperty>> expression, TreeMode treeMode, string container, bool isAjaxCall, string validationGroup = "")
        {
            string propertyName = ExpressionHelper.GetExpressionText(expression);
            var modelMetaData = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            helper.ViewContext.Controller.ViewBag.ErrorMessage = "";
            var sb = new StringBuilder();
            string required = string.Empty;
            string searchText = DbRes.TResource("Global.Tree.Search");
            if (modelMetaData.IsRequired)
            {
                required = "Required";
            }
            sb.Append("<div id='divForm__" + container + "' class='panel-group accordion " + required + "'>");
            sb.Append("<div class='permissions_area'>");
            sb.Append("<div class='form-group'>");
            sb.Append("<div class='col-md-12'>");
            sb.Append("<div class='input-group rtl_btn'>");
            sb.Append("<span class='tags '><div class='list-tags' id='divTags' data-name=" + propertyName + ">");
            sb.Append("</div></span>");
            sb.Append("<span class='input-group-addon link'>");
            sb.Append("<div class='panel-title'>");
            sb.Append("<a onclick=ShowHideTree('#divContainer__" + container + "') title=" + DbRes.TResource("Global.Tree.Show") + "><span class='fa fa-sort-amount-desc'> </span> </a>");
            sb.Append("</div></span>");
            sb.Append("</div></div></div></div>");
            if (modelMetaData.IsRequired)
            {
                sb.Append(helper.HiddenGroupFor(expression, new { @class = "__hdnPermission", @Value = "" }, validationGroup));
            }
            sb.Append("<div id='divAcc__" + container + "' class='accordion col-lg-12'>");
            sb.Append("<div class='panel-body panel-style' id='divContainer__" + container + "'>");
            sb.Append("<div id='divDir__" + container + "' class='directory'>");
            sb.Append("<div class='row'>");
            sb.Append("<div class='form-group'>");

            sb.Append("<div class='tree_parties space'>");
            sb.Append("<div class='group_top_directory'>");
            sb.Append("<ul class='tree' >");
            HtmlHelperExt.PermissionsEditableTreeNode(treeViewModel.RootNode, propertyName, treeMode, container, ref sb);
            sb.Append("</ul>");
            sb.Append("</div>");
            sb.Append("<div class='col-md-3'></div>");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");
            if (isAjaxCall)
            {
                sb.Append("<script>");
                sb.Append("TagsPopulation('" + container + "')");
                sb.Append("</script>");
            }
            helper.ViewContext.Controller.ViewBag.validationGroup = validationGroup;
            return MvcHtmlString.Create(sb.ToString());
        }
        private static void PermissionsEditableTreeNode(TreeNode treeNode, string propertyName, TreeMode treeMode, string container, ref StringBuilder sb)
        {
            string selectableClass = string.Empty;
            string isSelectedClass = string.Empty;
            string selectMode = string.Empty;
            if (treeMode == TreeMode.Multiple)
            {
                selectMode = "multiple_select";
            }
            else
            {
                selectMode = "single_select";
            }
            foreach (var node in treeNode.Childs)
            {
                selectableClass = string.Empty;
                isSelectedClass = string.Empty;
                string key = Guid.NewGuid().ToString();
                if (node != null)
                {
                    //if (node.Selectable)
                    //{
                    selectableClass = "selectable_node";
                    //}
                    if (node.IsSelected)
                    {
                        isSelectedClass = "selected";
                    }
                    if (node.Childs.Count > 0)
                    {
                        sb.Append("<li data-li-id='" + node.Id + "' id='li_" + node.Id + "'>");
                        sb.Append("<div class='row row_directory " + selectableClass + " " + isSelectedClass + " selected_bottom' id='divspan_" + node.Id + container + "'>");
                        sb.Append("<div class='col-md-1'>");
                        sb.Append("<a class='node' >");
                        sb.Append("<span id='span_" + node.Id + container + "' data-node-container='" + container + "' class='fa fa-plus'></span>");
                        sb.Append("</a>");
                        sb.Append("</div>");
                        sb.Append("<div data-container='" + container + "' data-Name='" + propertyName + "' data-id='" + node.Id + "' class='col-md-8 parent " + selectMode + "' id='" + key + "'>" + node.Name + "</div>");
                        sb.Append("<div class='col-md-1 glyphicon_ok'>");
                        sb.Append("<i class='glyphicon glyphicon-ok'></i>");
                        sb.Append("</div>");
                        sb.Append("<div class='col-md-2'>");
                        sb.Append("<div class='row control_name'>");
                        sb.Append("<div class='col-md-6'></div>");
                        sb.Append("<div class='col-md-6'>");
                        if (node.Id == (int)PermissionGroupName.ExplanationsConfidentiality || node.Id == (int)PermissionGroupName.InboundTransactionsTypes || node.Id == (int)PermissionGroupName.OutboundTransactionsTypes || node.Id == (int)PermissionGroupName.InternalOutboundTransactionsTypes)
                        {
                            sb.Append("<a class='dia_add_permission' onclick='OpenDialog(" + node.Id + ");' href='#' data-toggle='tooltip' data-placement='top' data-original-title='" + DbRes.TResource("Admin.Permissions.AddPermission") + "' title=''>+<i class='fa fa-level-down'></i></a>");
                        }

                        sb.Append("</div>");
                        sb.Append("</div>");
                        sb.Append("</div>");
                        sb.Append("</div>");
                        sb.Append("<ul id='ul_" + node.Id + container + "' style='display:none'>");
                        HtmlHelperExt.PermissionsEditableTreeNode(node, propertyName, treeMode, container, ref sb);
                        sb.Append("</ul>");
                        sb.Append("</li>");
                    }
                    else
                    {
                        sb.Append("<li id='li_" + node.Id + "'>");
                        sb.Append("<div class='row row_directory last " + selectableClass + " " + isSelectedClass + "'>");
                        //sb.Append("<div class='col-md-1'>");
                        //sb.Append("</div>");
                        sb.Append("<div data-container='" + container + "' data-Name='" + propertyName + "' data-parent='" + treeNode.Id + "' data-value='" + node.Id + "' class='col-md-8  leaf " + selectMode + "' id='" + key + "'>" + node.Name + "</div>");
                        sb.Append("<div class='col-md-1 glyphicon_ok'>");
                        sb.Append("<i class='glyphicon glyphicon-ok'></i>");
                        sb.Append("</div>");
                        sb.Append("<div class='col-md-2'>");
                        sb.Append("<div class='row control_name'>");
                        sb.Append("<div class='col-md-6'></div>");
                        sb.Append("<div class='col-md-6'>");
                        if (node.IsUserDefined)
                        {
                            sb.Append("<a href='#' data-toggle='tooltip' data-placement='top' data-original-title='" + DbRes.TResource("Admin.Permissions.Delete") + "' title='' ><i class='glyphicon glyphicon-trash' onclick='DeletePermission(" + node.Id + ");'></i></a>");
                            sb.Append("<a href='#' data-toggle='tooltip' data-placement='top' data-original-title='" + DbRes.TResource("Admin.Permissions.Edit") + "' title='' onclick='EditPermission(" + node.Id + ");'><i class='fa fa-edit'></i></a>");
                        }
                        sb.Append("</div>");
                        sb.Append("</div>");
                        sb.Append("</div>");
                        sb.Append("</li>");
                    }
                }
            }
        }
        public static MvcHtmlString DepartmentsTree<TModel, TProperty>(this HtmlHelper<TModel> helper, TreeViewModel treeViewModel, Expression<Func<TModel, TProperty>> expression, TreeMode treeMode, string treeId, bool isAjaxCall, string onClickfunction = "", string validationGroup = "", string divContainerId = "", string checkFunction = "", string additionalClass = "")
        {
            switch (treeMode)
            {
                case TreeMode.Multiple:
                    return HtmlHelperExt.DepartmentsMultipleTree(helper, treeViewModel, expression, treeMode, treeId, isAjaxCall, onClickfunction, validationGroup);

                case TreeMode.Single:
                    return HtmlHelperExt.DepartmentsSingleTree(helper, treeViewModel, expression, treeMode, treeId, isAjaxCall, onClickfunction, validationGroup, divContainerId);

                case TreeMode.MultiCheckbox:
                    return HtmlHelperExt.DepartmentsMultiCheckboxTree(helper, treeViewModel, expression, treeMode, treeId, isAjaxCall, onClickfunction, validationGroup, divContainerId, checkFunction, additionalClass);

                default:
                    return HtmlHelperExt.DepartmentsSingleTreeNotMandatory(helper, treeViewModel, expression, treeMode, treeId, isAjaxCall, onClickfunction, validationGroup, divContainerId);

            }

        }
        private static MvcHtmlString DepartmentsSingleTree<TModel, TProperty>(this HtmlHelper<TModel> helper, TreeViewModel treeViewModel, Expression<Func<TModel, TProperty>> expression, TreeMode treeMode, string treeId, bool isAjaxCall, string onClickfunction = "", string validationGroup = "", string divContainerId = "")
        {
            string propertyName = ExpressionHelper.GetExpressionText(expression);
            StringBuilder sb = new StringBuilder();
            var attrs = new Dictionary<string, object>();
            attrs.Add("class", "hdnDepartmentId");
            attrs.Add("data-container", treeId);
            attrs.Add("data-func", onClickfunction);
            sb.Append("<div id='divForm__" + treeId + "'>");
            sb.Append(helper.LabelFor(expression, new { @class = "control-label" }));
            sb.Append(helper.Label("*", new { @class = "stark" }));
            sb.Append("<div class='row'>");
            sb.Append("<div class='col-md-4 none_left'>");
            sb.Append("<input id='right" + treeId + "' type='text' data-func='" + onClickfunction + "' data-container='" + treeId + "' onkeypress ='return IsNumeric(event);' class='form-control txtDepartmentNumber' maxlength='20' />");
            sb.Append("</div>");
            sb.Append("<div class='col-md-8 none_right'>");
            sb.Append("<div class='input-group'>");
            sb.Append("<input id='left" + treeId + "' type='text' data-func='" + onClickfunction + "' data-container='" + treeId + "' class='form-control txtDepartmentName' maxlength='40' />");
            sb.Append("<span class='input-group-prepend'>");
            sb.Append("<div class='input-group-text'>");
            sb.Append("<a onclick=ShowHideTree('#divContainer__" + treeId + "') title=" + DbRes.TResource("Global.Tree.Show") + "><span class='fa fa-sort-amount-desc'> </span> </a>");
            sb.Append("</div></span>");
            sb.Append("</div></div>");
            sb.Append(helper.HiddenGroupFor(expression, attrs, validationGroup));
            sb.Append("</div><span class='d-inline-block'></span><div class='clear'></div></div>");
            sb.Append("<div class='accordion' id='divAcc__" + treeId + "'>");


            sb.Append("<div class='panel-body panel-style single-tree' style='display: none;' id='divContainer__" + treeId + "' data-func='" + onClickfunction + "' data-Name='" + propertyName + "'>");
            sb.Append("<div id='divDir__" + treeId + "' class='directory'>");
            sb.Append("<div class='row'>");
            sb.Append("<div class='tree_parties'>");
            sb.Append("<div class='group_top_directory'>");
            sb.Append("<ul class='tree' >");
            if (treeViewModel != null && treeViewModel.RootNode != null)
            {
                HtmlHelperExt.DepartmentsTreeNode(treeViewModel.RootNode, propertyName, treeMode, treeId, onClickfunction, ref sb);
            }
            sb.Append("</ul>");
            sb.Append("</div>");
            sb.Append("<div class='col-md-3'></div>");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<script>");
            sb.Append("RepiarTreeHtml('" + treeId + "','" + divContainerId + "')");
            sb.Append("</script>");
            if (isAjaxCall)
            {
                sb.Append("<script>");
                sb.Append("AppendDepartmentText('" + treeId + "')");
                sb.Append("</script>");
            }
            if (ModelMetadata.FromLambdaExpression(expression, helper.ViewData).Model != null)
            {
                string expressionValue = ModelMetadata.FromLambdaExpression(expression, helper.ViewData).Model.ToString();
                if (!string.IsNullOrEmpty(expressionValue) && expressionValue != "0")
                {
                    sb.Append("<script>");
                    sb.Append("FillOrgUnitsTreeControls('" + treeId + "','" + expressionValue + "')");
                    sb.Append("</script>");
                }
            }
            return MvcHtmlString.Create(sb.ToString());
        }
        private static MvcHtmlString DepartmentsMultiCheckboxTree<TModel, TProperty>(this HtmlHelper<TModel> helper, TreeViewModel treeViewModel, Expression<Func<TModel, TProperty>> expression, TreeMode treeMode, string treeId, bool isAjaxCall, string onClickfunction = "", string validationGroup = "", string divContainerId = "", string checkboxChange = "", string additionalClass = "")
        {
            string propertyName = ExpressionHelper.GetExpressionText(expression);
            StringBuilder sb = new StringBuilder();
            var attrs = new Dictionary<string, object>();
            attrs.Add("class", "hdnDepartmentId");
            attrs.Add("data-container", treeId);
            attrs.Add("data-func", onClickfunction);
            sb.Append("<div id='divForm__" + treeId + "'>");
            sb.Append(helper.LabelFor(expression, new { @class = "control-label" }));
            sb.Append(helper.Label("*", new { @class = "stark" }));
            sb.Append("<div class='row'>");
            sb.Append("<div class='col-md-4 none_left'>");
            sb.Append("<input id='right" + treeId + "' type='text' data-func='" + onClickfunction + "' data-container='" + treeId + "' onkeypress ='return IsNumeric(event);' class='form-control txtDepartmentNumber' maxlength='20' />");
            sb.Append("</div>");
            sb.Append("<div class='col-md-8 none_right'>");
            sb.Append("<div class='input-group'>");
            sb.Append("<input id='left" + treeId + "' type='text' data-func='" + onClickfunction + "' data-container='" + treeId + "' class='form-control txtDepartmentName' maxlength='40' />");
            sb.Append("<span class='input-group-prepend'>");
            sb.Append("<div class='input-group-text'>");
            sb.Append("<a onclick=ShowHideTree('#divContainer__" + treeId + "') title=" + DbRes.TResource("Global.Tree.Show") + "><span class='fa fa-sort-amount-desc'> </span> </a>");
            sb.Append("</div></span>");
            sb.Append("</div></div>");
            sb.Append(helper.HiddenGroupFor(expression, attrs, validationGroup));
            sb.Append("</div><span class='d-inline-block'></span><div class='clear'></div></div>");
            sb.Append("<div class='accordion' id='divAcc__" + treeId + "'>");


            sb.Append("<div class='panel-body panel-style single-tree' style='display: none;' id='divContainer__" + treeId + "' data-func='" + onClickfunction + "' data-Name='" + propertyName + "'>");
            sb.Append("<div id='divDir__" + treeId + "' class='directory'>");
            sb.Append("<div class='row'>");
            sb.Append("<div class='tree_parties'>");
            sb.Append("<div class='group_top_directory'>");
            sb.Append("<ul class='tree' >");
            if (treeViewModel.RootNode != null)
            {
                HtmlHelperExt.DepartmentsTreeNodeCheckbox(treeViewModel.RootNode, propertyName, treeMode, treeId, onClickfunction, ref sb, checkboxChange, additionalClass);
            }
            sb.Append("</ul>");
            sb.Append("</div>");
            sb.Append("<div class='col-md-3'></div>");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<script>");
            sb.Append("RepiarTreeHtml('" + treeId + "','" + divContainerId + "')");
            sb.Append("</script>");
            if (isAjaxCall)
            {
                sb.Append("<script>");
                sb.Append("AppendDepartmentText('" + treeId + "')");
                sb.Append("</script>");
            }
            if (ModelMetadata.FromLambdaExpression(expression, helper.ViewData).Model != null)
            {
                string expressionValue = ModelMetadata.FromLambdaExpression(expression, helper.ViewData).Model.ToString();
                if (!string.IsNullOrEmpty(expressionValue) && expressionValue != "0")
                {
                    sb.Append("<script>");
                    sb.Append("FillOrgUnitsTreeControls('" + treeId + "','" + expressionValue + "')");
                    sb.Append("</script>");
                }
            }
            return MvcHtmlString.Create(sb.ToString());
        }

        private static MvcHtmlString DepartmentsSingleTreeNotMandatory<TModel, TProperty>(this HtmlHelper<TModel> helper, TreeViewModel treeViewModel, Expression<Func<TModel, TProperty>> expression, TreeMode treeMode, string treeId, bool isAjaxCall, string onClickfunction = "", string validationGroup = "", string divContainerId = "")
        {
            string propertyName = ExpressionHelper.GetExpressionText(expression);
            StringBuilder sb = new StringBuilder();
            var attrs = new Dictionary<string, object>();
            attrs.Add("class", "hdnDepartmentId");
            attrs.Add("data-container", treeId);
            attrs.Add("data-func", onClickfunction);
            sb.Append("<div id='divForm__" + treeId + "'>");
            sb.Append(helper.LabelFor(expression, new { @class = "control-label" }));


            sb.Append("<div class='row'>");
            sb.Append("<div class='col-md-4 none_left'>");
            sb.Append("<input type='text' data-func='" + onClickfunction + "' data-container='" + treeId + "' onkeypress ='return IsNumeric(event);' class='form-control txtDepartmentNumber' maxlength='20' />");
            sb.Append("</div>");
            sb.Append("<div class='col-md-8 none_right'>");
            sb.Append("<div class='input-group'>");
            sb.Append("<input type='text' data-func='" + onClickfunction + "' data-container='" + treeId + "' class='form-control txtDepartmentName' maxlength='40' />");
            sb.Append("<span class='input-group-prepend'>");
            sb.Append("<div class='input-group-text'>");
            sb.Append("<a onclick=ShowHideTree('#divContainer__" + treeId + "') title=" + DbRes.TResource("Global.Tree.Show") + "><span class='fa fa-sort-amount-desc'> </span> </a>");
            sb.Append("</div></span>");
            sb.Append("</div></div>");
            sb.Append(helper.HiddenGroupFor(expression, attrs, validationGroup));
            sb.Append("</div><span class='d-inline-block'></span><div class='clear'></div></div>");
            sb.Append("<div class='accordion' id='divAcc__" + treeId + "'>");
            sb.Append("<div class='panel-body panel-style single-tree' style='display: none;' id='divContainer__" + treeId + "' data-func='" + onClickfunction + "' data-Name='" + propertyName + "'>");
            sb.Append("<div id='divDir__" + treeId + "' class='directory'>");
            sb.Append("<div class='row'>");
            sb.Append("<div class='tree_parties'>");
            sb.Append("<div class='group_top_directory'>");
            sb.Append("<ul class='tree' >");
            if (treeViewModel.RootNode != null)
            {
                HtmlHelperExt.DepartmentsTreeNode(treeViewModel.RootNode, propertyName, treeMode, treeId, onClickfunction, ref sb);
            }
            sb.Append("</ul>");
            sb.Append("</div>");
            sb.Append("<div class='col-md-3'></div>");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");

            sb.Append("<script>");
            sb.Append("RepiarTreeHtml('" + treeId + "','" + divContainerId + "')");
            sb.Append("</script>");
            if (isAjaxCall)
            {
                sb.Append("<script>");
                sb.Append("AppendDepartmentText('" + treeId + "')");
                sb.Append("</script>");
            }
            if (ModelMetadata.FromLambdaExpression(expression, helper.ViewData).Model != null)
            {
                string expressionValue = ModelMetadata.FromLambdaExpression(expression, helper.ViewData).Model.ToString();
                if (!string.IsNullOrEmpty(expressionValue) && expressionValue != "0")
                {
                    sb.Append("<script>");
                    sb.Append("FillOrgUnitsTreeControls('" + treeId + "','" + expressionValue + "')");
                    sb.Append("</script>");
                }
            }
            return MvcHtmlString.Create(sb.ToString());
        }
        public static MvcHtmlString DepartmentsTreeOnDemand<TModel, TProperty>(this HtmlHelper<TModel> helper, TreeViewModel treeViewModel, Expression<Func<TModel, TProperty>> expression, TreeMode treeMode, string treeId, bool isAjaxCall, string onClickfunction = "", string validationGroup = "", string divContainerId = "", bool isRefreshEnabled = false, bool isOptinal = false, bool ShowYesserEntities = false, string checkboxFunction = "", string additionalClass = "" )
        {
            switch (treeMode)
            {
                case TreeMode.MultiCheckbox:
                    return DepartmentsTreeOnDemandMulti(helper, treeViewModel, expression, treeMode, treeId, isAjaxCall, onClickfunction, validationGroup, divContainerId, isRefreshEnabled, isOptinal, ShowYesserEntities, checkboxFunction, additionalClass);
                    break;
                default:
                    return DepartmentsTreeOnDemandDefault(helper, treeViewModel, expression, treeMode, treeId, isAjaxCall, onClickfunction, validationGroup, divContainerId, isRefreshEnabled, isOptinal, ShowYesserEntities);
                    break;
            }
            //helper.ViewContext.Controller.ViewBag.validationGroup = validationGroup;
            return null;
        }

        private static MvcHtmlString DepartmentsTreeOnDemandMulti<TModel, TProperty>(this HtmlHelper<TModel> helper, TreeViewModel treeViewModel, Expression<Func<TModel, TProperty>> expression, TreeMode treeMode, string treeId, bool isAjaxCall, string onClickfunction = "", string validationGroup = "", string divContainerId = "", bool isRefreshEnabled = false, bool isOptinal = false, bool ShowYesserEntities = false, string checkboxChange = "", string additionalClass = "")
        {
            string propertyName = ExpressionHelper.GetExpressionText(expression);
            StringBuilder sb = new StringBuilder();
            var attrs = new Dictionary<string, object>();
            attrs.Add("class", "hdnDepartmentId");
            attrs.Add("data-container", treeId);
            attrs.Add("data-func", onClickfunction);
            sb.Append("<div class='' id='divForm__" + treeId + "'>");
            sb.Append(helper.LabelFor(expression, new { @class = "control-label" }));
            if (isOptinal == false)
            {
                sb.Append(helper.Label("*", new { @class = "stark NotIndividual" }));
            }
            if (ShowYesserEntities)
            {
                string resource = DbRes.TResource("Admin.YesserMapping.YesserMappedEntities");
                sb.Append(" <a id='btnShowYesserEntities' title='" + resource + "' class='fa fa-link'></a>");
            }
            //sb.Append("<div class='col-md-12 Masser'>");
            sb.Append("<div class='row'>");
            sb.Append("<div class='col-md-4 none_left'>");
            sb.Append("<input id='right" + treeId + "' type='text' data-func='" + onClickfunction + "' data-container='" + treeId + "' onkeypress ='return IsNumeric(event);' class='form-control InboundDepNum txtDepartmentNumber' maxlength='20' />");
            sb.Append("</div>");
            sb.Append("<div class='col-md-8 none_right'>");
            sb.Append("<div class='input-group'>");
            sb.Append("<input id='left" + treeId + "' type='text' data-func='" + onClickfunction + "' data-container='" + treeId + "' class='form-control InboundDepName txtDepartmentName' maxlength='40' />");
            sb.Append("<span class='input-group-prepend'>");
            sb.Append("<div class='input-group-text'>");
            sb.Append("<a onclick=ShowHideTree('#divContainer__" + treeId + "') title=" + DbRes.TResource("Global.Tree.Show") + "><span class='fa fa-sort-amount-desc'> </span> </a>");
            sb.Append("</div></span>");
            sb.Append("</div></div>");
            sb.Append(helper.HiddenGroupFor(expression, attrs, validationGroup));
            sb.Append("</div><span class='d-inline-block'></span><div class='clear'></div></div>");
            sb.Append("<div class='accordion' id='divAcc__" + treeId + "'>");
            sb.Append("<div class='panel-body panel-style single-tree' style='display: none;' id='divContainer__" + treeId + "' data-func='" + onClickfunction + "' data-Name='" + propertyName + "'>");
            sb.Append("<div id='divDir__" + treeId + "' class='directory'>");
            sb.Append("<div class='row'>");
            sb.Append("<div class='tree_parties'>");
            sb.Append("<div class='group_top_directory'>");
            sb.Append("<ul class='tree' >");
            if (treeViewModel.RootNode != null)
            {
                HtmlHelperExt.DepartmentsTreeNodeOnDemandCheckbox(treeViewModel.RootNode, propertyName, treeMode, treeId, onClickfunction, ref sb, checkboxChange, false,additionalClass);
            }
            sb.Append("</ul>");
            sb.Append("</div>");
            sb.Append("<div class='col-md-3'></div>");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("<script>");
            sb.Append("RepiarTreeHtml('" + treeId + "','" + divContainerId + "')");
            sb.Append("</script>");
            if (isAjaxCall)
            {
                sb.Append("<script>");
                sb.Append("AppendDepartmentText('" + treeId + "')");
                sb.Append("</script>");
            }
            if (ModelMetadata.FromLambdaExpression(expression, helper.ViewData).Model != null)
            {
                string expressionValue = ModelMetadata.FromLambdaExpression(expression, helper.ViewData).Model.ToString();
                if (!string.IsNullOrEmpty(expressionValue) && expressionValue != "0")
                {
                    sb.Append("<script>");
                    sb.Append("FillDepartmentTreeControls('" + treeId + "','" + expressionValue + "')");
                    sb.Append("</script>");
                }
            }
            //helper.ViewContext.Controller.ViewBag.validationGroup = validationGroup;
            return MvcHtmlString.Create(sb.ToString());
        }
        private static MvcHtmlString DepartmentsTreeOnDemandDefault<TModel, TProperty>(this HtmlHelper<TModel> helper, TreeViewModel treeViewModel, Expression<Func<TModel, TProperty>> expression, TreeMode treeMode, string treeId, bool isAjaxCall, string onClickfunction = "", string validationGroup = "", string divContainerId = "", bool isRefreshEnabled = false, bool isOptinal = false, bool ShowYesserEntities = false)
        {
            string propertyName = ExpressionHelper.GetExpressionText(expression);
            StringBuilder sb = new StringBuilder();
            var attrs = new Dictionary<string, object>();
            attrs.Add("class", "hdnDepartmentId");
            attrs.Add("data-container", treeId);
            attrs.Add("data-func", onClickfunction);
            sb.Append("<div class='' id='divForm__" + treeId + "'>");
            sb.Append(helper.LabelFor(expression, new { @class = "control-label" }));
            if (isOptinal == false)
            {
                sb.Append(helper.Label("*", new { @class = "stark NotIndividual" }));
            }
            if (ShowYesserEntities)
            {
                string resource = DbRes.TResource("Admin.YesserMapping.YesserMappedEntities");
                sb.Append(" <a id='btnShowYesserEntities' title='" + resource + "' class='fa fa-link'></a>");
            }
            //sb.Append("<div class='col-md-12 Masser'>");
            sb.Append("<div class='row'>");
            sb.Append("<div class='col-md-4 none_left'>");
            sb.Append("<input id='right" + treeId + "' type='text' data-func='" + onClickfunction + "' data-container='" + treeId + "' onkeypress ='return IsNumeric(event);' class='form-control InboundDepNum txtDepartmentNumber' maxlength='20' />");
            sb.Append("</div>");
            sb.Append("<div class='col-md-8 none_right'>");
            sb.Append("<div class='input-group'>");
            sb.Append("<input id='left" + treeId + "' type='text' data-func='" + onClickfunction + "' data-container='" + treeId + "' class='form-control InboundDepName txtDepartmentName' maxlength='40' />");
            sb.Append("<span class='input-group-prepend'>");
            sb.Append("<div class='input-group-text'>");
            sb.Append("<a onclick=ShowHideTree('#divContainer__" + treeId + "') title=" + DbRes.TResource("Global.Tree.Show") + "><span class='fa fa-sort-amount-desc'> </span> </a>");
            sb.Append("</div></span>");
            sb.Append("</div></div>");
            sb.Append(helper.HiddenGroupFor(expression, attrs, validationGroup));
            sb.Append("</div><span class='d-inline-block'></span><div class='clear'></div></div>");
            sb.Append("<div class='accordion' id='divAcc__" + treeId + "'>");
            sb.Append("<div class='panel-body panel-style single-tree' style='display: none;' id='divContainer__" + treeId + "' data-func='" + onClickfunction + "' data-Name='" + propertyName + "'>");
            sb.Append("<div id='divDir__" + treeId + "' class='directory'>");
            sb.Append("<div class='row'>");
            sb.Append("<div class='tree_parties'>");
            sb.Append("<div class='group_top_directory'>");
            sb.Append("<ul class='tree' >");
            if (treeViewModel.RootNode != null)
            {
                HtmlHelperExt.DepartmentsTreeNodeOnDemand(treeViewModel.RootNode, propertyName, treeMode, treeId, onClickfunction, ref sb, isRefreshEnabled);
            }
            sb.Append("</ul>");
            sb.Append("</div>");
            sb.Append("<div class='col-md-3'></div>");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("<script>");
            sb.Append("RepiarTreeHtml('" + treeId + "','" + divContainerId + "')");
            sb.Append("</script>");
            if (isAjaxCall)
            {
                sb.Append("<script>");
                sb.Append("AppendDepartmentText('" + treeId + "')");
                sb.Append("</script>");
            }
            if (ModelMetadata.FromLambdaExpression(expression, helper.ViewData).Model != null)
            {
                string expressionValue = ModelMetadata.FromLambdaExpression(expression, helper.ViewData).Model.ToString();
                if (!string.IsNullOrEmpty(expressionValue) && expressionValue != "0")
                {
                    sb.Append("<script>");
                    sb.Append("FillDepartmentTreeControls('" + treeId + "','" + expressionValue + "')");
                    sb.Append("</script>");
                }
            }
            //helper.ViewContext.Controller.ViewBag.validationGroup = validationGroup;
            return MvcHtmlString.Create(sb.ToString());
        }
        private static MvcHtmlString DepartmentsMultipleTree<TModel, TProperty>(this HtmlHelper<TModel> helper, TreeViewModel treeViewModel, Expression<Func<TModel, TProperty>> expression, TreeMode treeMode, string container, bool isAjaxCall, string onClickfunction = "", string validationGroup = "")
        {
            string propertyName = null;
            if (string.IsNullOrEmpty(helper.ViewData.TemplateInfo.HtmlFieldPrefix))
            {
                propertyName = ExpressionHelper.GetExpressionText(expression);
            }
            else
            {
                propertyName = string.Join(".", helper.ViewData.TemplateInfo.HtmlFieldPrefix, ExpressionHelper.GetExpressionText(expression));
            }
            var modelMetaData = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            var sb = new StringBuilder();
            string required = string.Empty;
            string searchText = DbRes.TResource("Global.Tree.Search");
            if (modelMetaData.IsRequired)
            {
                required = "Required";
            }
            sb.Append("<div id='divForm__" + container + "' class='panel-group accordion " + required + "'>");
            sb.Append("<div class='permissions_area'>");
            sb.Append("<div class='form-group'>");
            sb.Append(helper.LabelFor(expression, new { @class = " control-label" }));
            //sb.Append("<div class='col-md-12'>");
            sb.Append("<div class='input-group rtl_btn'>");
            sb.Append("<Input class='tags form-control'><div class='list-tags' id='divTags' data-name=" + propertyName + ">");
            sb.Append("</div></Input>");
            sb.Append("<span class='input-group-prepend'>");
            sb.Append("<div class='input-group-text'>");
            sb.Append("<a onclick=ShowHideTree('#divContainer__" + container + "') title=" + DbRes.TResource("Global.Tree.Show") + "><span class='fa fa-sort-amount-desc'> </span> </a>");
            sb.Append("</div></span>");
            sb.Append("</div></div></div>");
            //sb.Append(helper.ValidationMessageFor(expression));
            if (modelMetaData.IsRequired && !isAjaxCall)
            {
                sb.Append(helper.HiddenGroupFor(expression, new { @class = "__hdnDepartment" }, validationGroup));
            }
            sb.Append("<div class='accordion' id='divAcc__" + container + "'>");
            sb.Append("<div class='panel-body panel-style single-tree' id='divContainer__" + container + "'>");
            sb.Append("<div id='divDir__" + container + "' class='directory'>");
            sb.Append("<div class='row'>");

            sb.Append("<div class='tree_parties space'>");
            sb.Append("<div class='group_top_directory'>");
            sb.Append("<ul class='tree' >");
            HtmlHelperExt.DepartmentsTreeNode(treeViewModel.RootNode, propertyName, treeMode, container, onClickfunction, ref sb);
            sb.Append("</ul>");
            sb.Append("</div>");
            sb.Append("<div class='col-md-3'></div>");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");
            if (isAjaxCall)
            {
                sb.Append("<script>");
                sb.Append("TagsPopulationDept('" + container + "')");
                sb.Append("</script>");
            }
            helper.ViewContext.Controller.ViewBag.validationGroup = validationGroup;
            return MvcHtmlString.Create(sb.ToString());
        }
        private static void DepartmentsTreeNode(TreeNode treeNode, string propertyName, TreeMode treeMode, string container, string onClickfunction, ref StringBuilder sb)
        {
            string selectableClass = string.Empty;
            string isSelectedClass = string.Empty;
            string selectMode = string.Empty;
            if (treeMode == TreeMode.Multiple)
            {
                selectMode = "multiple_select_dept";
            }
            else
            {
                selectMode = "single_select_dept";
            }
            if (treeNode != null && treeNode.Childs != null)
            {
                foreach (var node in treeNode.Childs)
                {
                    selectableClass = string.Empty;
                    isSelectedClass = string.Empty;
                    string key = Guid.NewGuid().ToString();
                    if (node != null)
                    {
                        if (node.Selectable)
                        {
                            selectableClass = "selectable_node";
                        }
                        if (node.IsSelected)
                        {
                            isSelectedClass = "selected";
                        }
                        if (node.HasChilds)
                        {
                            sb.Append("<li id='li_" + node.Id + "' title='" + node.Name + "'>");
                            sb.Append("<div class='row row_directory " + selectableClass + " " + isSelectedClass + " selected_bottom' id='divspan_" + node.Id + container + "'>");

                            sb.Append("<input type='hidden' value='" + node.DepartmentNumber + "'  class='hdnDepartmentNumber __hdnSticky'/>");
                            sb.Append("<div data-container='" + container + "' data-Name='" + propertyName + "' data-value='" + node.Id + "' data-func='" + onClickfunction + "' class='col-md-8 " + selectMode + "' id='" + key + "' title='" + node.Name + "'>" + node.Name + "</div>");
                            sb.Append("<div class='col-md-4'>");
                            sb.Append("<a class='node'  >");
                            sb.Append("<span id='span_" + node.Id + container + "' data-node-container='" + container + "' data-func='" + onClickfunction + "' class='fa fa-plus InternalLoadedData' data-node-id='" + node.Id + "'></span>");
                            sb.Append("</a>");
                            sb.Append("</div>");
                            sb.Append("<div class='col-md-1 glyphicon_ok'>");
                            sb.Append("<i class='glyphicon glyphicon-ok'></i>");
                            sb.Append("</div>");
                            sb.Append("</div>");
                            sb.Append("<ul id='ul_" + node.Id + container + "' style='display:none'>");
                            HtmlHelperExt.DepartmentsTreeNode(node, propertyName, treeMode, container, onClickfunction, ref sb);
                            sb.Append("</ul>");
                            sb.Append("</li>");
                        }
                        else
                        {
                            sb.Append("<li id='li_" + node.Id + "' title='" + node.Name + "'>");
                            sb.Append("<div class='row row_directory last " + selectableClass + " " + isSelectedClass + "'>");
                            //sb.Append("<div class='col-md-1'>");
                            //sb.Append("</div>");
                            sb.Append("<input type='hidden' value='" + node.DepartmentNumber + "'  class='hdnDepartmentNumber __hdnSticky'/>");
                            sb.Append("<div data-container='" + container + "' data-Name='" + propertyName + "' data-value='" + node.Id + "' data-func='" + onClickfunction + "' class='col-md-8 " + selectMode + "' id='" + key + "' title='" + node.Name + "'>" + node.Name + "</div>");
                            sb.Append("<div class='col-md-1 glyphicon_ok'>");
                            sb.Append("<i class='glyphicon glyphicon-ok'></i>");
                            sb.Append("</div>");
                            sb.Append("</div>");
                            sb.Append("</li>");
                        }
                    }
                }
            }
        }

        private static void DepartmentsTreeNodeCheckbox(TreeNode treeNode, string propertyName, TreeMode treeMode, string container, string onClickfunction, ref StringBuilder sb, string checkboxChange, string additionalClass = "")
        {
            string selectableClass = string.Empty;
            string isSelectedClass = string.Empty;
            string selectMode = string.Empty;

            selectMode = "multiple_select_dept";


            if (treeNode != null && treeNode.Childs != null)
            {
                foreach (var node in treeNode.Childs)
                {
                    selectableClass = string.Empty;
                    isSelectedClass = string.Empty;
                    string key = Guid.NewGuid().ToString();
                    if (node != null)
                    {
                        if (node.Selectable)
                        {
                            selectableClass = "selectable_node";
                        }
                        if (node.IsSelected)
                        {
                            isSelectedClass = "selected";
                        }
                        if (node.HasChilds)
                        {
                            sb.Append("<li id='li_" + node.Id + "' title='" + node.Name + "'>");
                            sb.Append("<div class='row row_directory " + selectableClass + " " + isSelectedClass + " selected_bottom' id='divspan_" + node.Id + container + "'>");
                            sb.Append("<div class='d-flex flex-row'>");
                            sb.Append("<div class='col-md-1 checkboxNode'>");
                            sb.Append("<input type='checkbox' onClick='" + checkboxChange + "(this,`" + node.Id + "`,`" + node.DepartmentNumber + "`,`" + node.Name + "`)' />");
                            sb.Append("</div>");
                            sb.Append("<div class='col-lg-3 dir-num'>");
                            sb.Append("<input type='' value='" + node.DepartmentNumber + "'  class='hdnDepartmentNumber __hdnSticky' style='background:none; border:none;'/>");
                            sb.Append("</div>");
                            sb.Append("<div class='flex-grow-1'>");
                            sb.Append("<div data-container='" + container + "' data-Name='" + propertyName + "' data-value='" + node.Id + "' data-func='" + onClickfunction + "' class='" + selectMode + "' id='" + key + "' title='" + node.Name + "'>" + node.Name + "</div>");
                            sb.Append("</div>");
                            sb.Append("<a class='node'  >");
                            sb.Append("<span id='span_" + node.Id + container + "' data-node-container='" + container + "' data-func='" + onClickfunction + "' class='fa fa-plus InternalLoadedData " + additionalClass + "' data-multi-node-id='" + node.Id + "'></span>");
                            sb.Append("</a>");
                            sb.Append("<div class='col-md-1 glyphicon_ok'>");
                            sb.Append("<i class='glyphicon glyphicon-ok'></i>");
                            sb.Append("</div>");
                            sb.Append("</div>");


                            //sb.Append("<input type='hidden' value='" + node.DepartmentNumber + "'  class='hdnDepartmentNumber __hdnSticky'/>");
                            //sb.Append("<div data-container='" + container + "' data-Name='" + propertyName + "' data-value='" + node.Id + "' data-func='" + onClickfunction + "' class='col-md-8 " + selectMode + "' id='" + key + "' title='" + node.Name + "'>" + node.Name + "</div>");
                            sb.Append("<div class='col-md-4'>");
                            //sb.Append("<a class='node'  >");
                            //sb.Append("<span id='span_" + node.Id + container + "' data-node-container='" + container + "' data-func='" + onClickfunction + "' class='fa fa-plus InternalLoadedData' data-node-id='" + node.Id + "'></span>");
                            //sb.Append("</a>");
                            sb.Append("</div>");
                            //sb.Append("<div class='col-md-1 glyphicon_ok'>");
                            //sb.Append("<i class='glyphicon glyphicon-ok'></i>");
                            //sb.Append("</div>");
                            sb.Append("</div>");
                            sb.Append("<ul id='ul_" + node.Id + container + "'style='display:none'>");
                            HtmlHelperExt.DepartmentsTreeNodeCheckbox(node, propertyName, treeMode, container, onClickfunction, ref sb, checkboxChange, additionalClass);
                            sb.Append("</ul>");
                            sb.Append("</li>");
                        }
                        else
                        {
                            sb.Append("<li id='li_" + node.Id + "' title='" + node.Name + "'>");
                            sb.Append("<div class='row row_directory last " + selectableClass + " " + isSelectedClass + "'>");
                            sb.Append("<div class='d-flex flex-row'>");
                            sb.Append("<div class='col-md-1 checkboxNode'>");
                            sb.Append("<input type='checkbox'  onClick='" + checkboxChange + "(this,`" + node.Id + "`,`" + node.DepartmentNumber + "`,`" + node.Name + "`)'  />");
                            sb.Append("</div>");
                            sb.Append("<div class='col-lg-3 dir-num'>");
                            sb.Append("<input type='' value='" + node.DepartmentNumber + "'  class='hdnDepartmentNumber __hdnSticky' style='background:none; border:none;'/>");
                            sb.Append("</div>");
                            sb.Append("<div class='flex-grow-1'>");
                            sb.Append("<div data-container='" + container + "' data-Name='" + propertyName + "' data-value='" + node.Id + "' data-func='" + onClickfunction + "' class='col-md-8 " + selectMode + "' id='" + key + "' title='" + node.Name + "'>" + node.Name + "</div>");
                            sb.Append("</div>");
                            sb.Append("<div class='col-md-1 glyphicon_ok'>");
                            sb.Append("<i class='glyphicon glyphicon-ok'></i>");
                            sb.Append("</div>");
                            sb.Append("</div>");


                            sb.Append("</div>");
                            sb.Append("</li>");
                        }
                    }
                }
            }
        }


        public static void DepartmentsTreeNodeOnDemand(TreeNode treeNode, string propertyName, TreeMode treeMode, string container, string onClickfunction, ref StringBuilder sb, bool isRefreshEnabled = false)
        {
            string selectableClass = string.Empty;
            string isSelectedClass = string.Empty;
            string selectMode = string.Empty;
            if (treeMode == TreeMode.Multiple)
            {
                selectMode = "multiple_select_dept";
            }
            else
            {
                selectMode = "single_select_dept";
            }
            if (treeNode != null && treeNode.Childs != null)
            {
                foreach (var node in treeNode.Childs)
                {
                    selectableClass = string.Empty;
                    isSelectedClass = string.Empty;
                    string key = Guid.NewGuid().ToString();
                    if (node != null)
                    {
                        if (node.Selectable)
                        {
                            selectableClass = "selectable_node";
                        }
                        if (node.IsSelected)
                        {
                            isSelectedClass = "selected";
                        }
                        if (node.HasChilds)
                        {
                            sb.Append("<li id='li_" + node.Id + "'>");
                            sb.Append("<div class='row row_directory " + selectableClass + " " + isSelectedClass + " selected_bottom' id='divspan_" + node.Id + container + "'>");

                            sb.Append("<input type='hidden' value='" + node.DepartmentNumber + "'  class='hdnDepartmentNumber __hdnSticky'/>");
                            if (node.IsYesserRegistered)
                            {
                                sb.Append("<div data-container='" + container + "' data-Name='" + propertyName + "' data-value='" + node.Id + "' data-func='" + onClickfunction + "' class='col-md-8 stark " + selectMode + "' id='" + key + "'>" + node.Name + "</div>"); //yesserregisteredmark
                            }
                            else
                            {
                                sb.Append("<div data-container='" + container + "' data-Name='" + propertyName + "' data-value='" + node.Id + "' data-func='" + onClickfunction + "' class='col-md-8 " + selectMode + "' id='" + key + "'>" + node.Name + "</div>");
                            }
                            sb.Append("<div class='col-md-4' >");
                            sb.Append("<a class='node'  >");
                            sb.Append("<span id='span_" + node.Id + container + "' data-node-container='" + container + "' data-func='" + onClickfunction + "' class='fa fa-plus LoadedData' data-isRefreshEnabled='" + isRefreshEnabled + "' data-node-id='" + node.Id + "'></span>");
                            sb.Append("</a>");
                            sb.Append("</div>");
                            sb.Append("<div class='col-md-1 glyphicon_ok'>");
                            sb.Append("<i class='glyphicon glyphicon-ok'></i>");
                            sb.Append("</div>");
                            sb.Append("</div>");
                            sb.Append("<ul id='ul_" + node.Id + container + "' style='display:none'>");
                            HtmlHelperExt.DepartmentsTreeNodeOnDemand(node, propertyName, treeMode, container, onClickfunction, ref sb);
                            sb.Append("</ul>");
                            sb.Append("</li>");
                        }
                        else
                        {
                            sb.Append("<li id='li_" + node.Id + "'>");
                            sb.Append("<div class='row row_directory last " + selectableClass + " " + isSelectedClass + "'>");
                            //sb.Append("<div class='col-md-1'>");
                            //sb.Append("</div>");
                            sb.Append("<input type='hidden' value='" + node.DepartmentNumber + "'  class='hdnDepartmentNumber __hdnSticky'/>");
                            if (node.IsYesserRegistered)
                            {
                                sb.Append("<div data-container='" + container + "' data-Name='" + propertyName + "' data-value='" + node.Id + "' data-func='" + onClickfunction + "' class='col-md-8 stark " + selectMode + "' id='" + key + "'>" + node.Name + "</div>");
                            }
                            else
                            {
                                sb.Append("<div data-container='" + container + "' data-Name='" + propertyName + "' data-value='" + node.Id + "' data-func='" + onClickfunction + "' class='col-md-8 " + selectMode + "' id='" + key + "'>" + node.Name + "</div>");
                            }
                            sb.Append("<div class='col-md-1 glyphicon_ok'>");
                            sb.Append("<i class='glyphicon glyphicon-ok'></i>");
                            sb.Append("</div>");
                            sb.Append("</div>");
                            sb.Append("</li>");
                        }
                    }
                }
            }
        }

        public static void DepartmentsTreeNodeOnDemandCheckbox(TreeNode treeNode, string propertyName, TreeMode treeMode, string container, string onClickfunction, ref StringBuilder sb, string checkboxChange, bool isRefreshEnabled = false,string additionalClass = "")
        {
            string selectableClass = string.Empty;
            string isSelectedClass = string.Empty;
            string selectMode = string.Empty;

            selectMode = "multiple_select_dept";

            if (treeNode != null && treeNode.Childs != null)
            {
                foreach (var node in treeNode.Childs)
                {
                    selectableClass = string.Empty;
                    isSelectedClass = string.Empty;
                    string key = Guid.NewGuid().ToString();
                    if (node != null)
                    {
                        if (node.Selectable)
                        {
                            selectableClass = "selectable_node";
                        }
                        if (node.IsSelected)
                        {
                            isSelectedClass = "selected";
                        }
                        if (node.HasChilds)
                        {
                            sb.Append("<li id='li_" + node.Id + "' title='" + node.Name + "'>");
                            sb.Append("<div class='row row_directory " + selectableClass + " " + isSelectedClass + " selected_bottom' id='divspan_" + node.Id + container + "'>");
                            sb.Append("<div class='d-flex flex-row'>");
                            sb.Append("<div class='col-md-1 checkboxNode'>");
                            sb.Append("<input type='checkbox' onClick='" + checkboxChange + "(this,`" + node.Id + "`,`" + node.DepartmentNumber + "`,`" + node.Name + "`)' />");
                            sb.Append("</div>");
                            sb.Append("<div class='col-lg-3 dir-num'>");
                            sb.Append("<input type='' value='" + node.DepartmentNumber + "'  class='hdnDepartmentNumber __hdnSticky' style='background:none; border:none;'/>");
                            sb.Append("</div>");
                            sb.Append("<div class='flex-grow-1'>");

                            if (node.IsYesserRegistered)
                            {
                                sb.Append("<div data-container='" + container + "' data-Name='" + propertyName + "' data-value='" + node.Id + "' data-func='" + onClickfunction + "' class=' " + selectMode + "' id='" + key + "'>" + node.Name + "</div>"); //yesserregisteredmark
                            }
                            else
                            {
                                sb.Append("<div data-container='" + container + "' data-Name='" + propertyName + "' data-value='" + node.Id + "' data-func='" + onClickfunction + "' class=' " + selectMode + "' id='" + key + "'>" + node.Name + "</div>");
                            }
                            sb.Append("</div>");
                            sb.Append("<a class='node'  >");
                            sb.Append("<span id='span_" + node.Id + container + "' data-node-container='" + container + "' data-func='" + onClickfunction + "' class='fa fa-plus LoadedData " + additionalClass + "' data-isRefreshEnabled='" + isRefreshEnabled + "' data-multi-node-id='" + node.Id + "'></span>");
                            sb.Append("</a>");
                            sb.Append("<div class='col-md-1 glyphicon_ok'>");
                            sb.Append("<i class='glyphicon glyphicon-ok'></i>");
                            sb.Append("</div>");
                            sb.Append("</div>");
                            sb.Append("</div>");
                            sb.Append("<ul id='ul_" + node.Id + container + "' style='display:none'>");
                            HtmlHelperExt.DepartmentsTreeNodeOnDemandCheckbox(node, propertyName, treeMode, container, onClickfunction, ref sb, checkboxChange,false,additionalClass);
                            sb.Append("</ul>");
                            sb.Append("</li>");
                        }
                        else
                        {
                            sb.Append("<li id='li_" + node.Id + "'>");
                            sb.Append("<div class='row row_directory last " + selectableClass + " " + isSelectedClass + "'>");
                            sb.Append("<div class='d-flex flex-row'>");
                            sb.Append("<div class='col-md-1 checkboxNode'>");
                            sb.Append("<input type='checkbox'  onClick='" + checkboxChange + "(this,`" + node.Id + "`,`" + node.DepartmentNumber + "`,`" + node.Name + "`)'  />");
                            sb.Append("</div>");
                            sb.Append("<div class='col-lg-3 dir-num'>");
                            sb.Append("<input type='' value='" + node.DepartmentNumber + "'  class='hdnDepartmentNumber __hdnSticky' style='background:none; border:none;'/>");
                            sb.Append("</div>");
                            sb.Append("<div class='flex-grow-1'>");
                            if (node.IsYesserRegistered)
                            {
                                sb.Append("<div data-container='" + container + "' data-Name='" + propertyName + "' data-value='" + node.Id + "' data-func='" + onClickfunction + "' class=' " + selectMode + "' id='" + key + "'>" + node.Name + "</div>");
                            }
                            else
                            {
                                sb.Append("<div data-container='" + container + "' data-Name='" + propertyName + "' data-value='" + node.Id + "' data-func='" + onClickfunction + "' class=' " + selectMode + "' id='" + key + "'>" + node.Name + "</div>");
                            }
                            sb.Append("</div>");
                            sb.Append("<div class='col-md-1 glyphicon_ok'>");
                            sb.Append("<i class='glyphicon glyphicon-ok'></i>");
                            sb.Append("</div>");
                            sb.Append("</div>");
                            sb.Append("</div>");
                            sb.Append("</li>");
                        }
                    }
                }
            }
        }

        public static MvcHtmlString DepartmentsTreeNodeOnDemandCheckbox<TModel>(this HtmlHelper<TModel> helper, TreeNode treeNode, string propertyName, TreeMode treeMode, string container, string onClickfunction, string checkboxChange, bool isRefreshEnabled = false,string additionalClass = "")
        {
            string selectableClass = string.Empty;
            string isSelectedClass = string.Empty;
            string selectMode = string.Empty;
            StringBuilder sb = new StringBuilder();

            selectMode = "multiple_select_dept";

            if (treeNode != null && treeNode.Childs != null)
            {
                foreach (var node in treeNode.Childs)
                {
                    selectableClass = string.Empty;
                    isSelectedClass = string.Empty;
                    string key = Guid.NewGuid().ToString();
                    if (node != null)
                    {
                        if (node.Selectable)
                        {
                            selectableClass = "selectable_node";
                        }
                        if (node.IsSelected)
                        {
                            isSelectedClass = "selected";
                        }
                        if (node.HasChilds)
                        {
                            sb.Append("<li id='li_" + node.Id + "'>");
                            sb.Append("<div class='row row_directory " + selectableClass + " " + isSelectedClass + " selected_bottom' id='divspan_" + node.Id + container + "'>");
                            sb.Append("<div class='d-flex flex-row'>");
                            sb.Append("<div class='col-md-1 checkboxNode'>");
                            sb.Append("<input type='checkbox' onClick='" + checkboxChange + "(this,`" + node.Id + "`,`" + node.DepartmentNumber + "`,`" + node.Name + "`)' />");
                            sb.Append("</div>");
                            sb.Append("<div class='col-lg-3 dir-num'>");
                            sb.Append("<input type='' value='" + node.DepartmentNumber + "'  class='hdnDepartmentNumber __hdnSticky' style='background:none; border:none'/>");
                            sb.Append("</div>");
                            sb.Append("<div class='flex-grow-1'>");

                            if (node.IsYesserRegistered)
                            {
                                sb.Append("<div data-container='" + container + "' data-Name='" + propertyName + "' data-value='" + node.Id + "' data-func='" + onClickfunction + "' class=' " + selectMode + "' id='" + key + "'>" + node.Name + "</div>"); //yesserregisteredmark
                            }
                            else
                            {
                                sb.Append("<div data-container='" + container + "' data-Name='" + propertyName + "' data-value='" + node.Id + "' data-func='" + onClickfunction + "' class=' " + selectMode + "' id='" + key + "'>" + node.Name + "</div>");
                            }
                            sb.Append("</div>");
                            sb.Append("<a class='node'  >");
                            sb.Append("<span id='span_" + node.Id + container + "' data-node-container='" + container + "' data-func='" + onClickfunction + "' class='fa fa-plus LoadedData "+ additionalClass +"' data-isRefreshEnabled='" + isRefreshEnabled + "' data-multi-node-id='" + node.Id + "'></span>");
                            sb.Append("</a>");
                            sb.Append("<div class='col-md-1 glyphicon_ok'>");
                            sb.Append("<i class='glyphicon glyphicon-ok'></i>");
                            sb.Append("</div>");
                            sb.Append("</div>");
                            sb.Append("</div>");
                            sb.Append("<ul id='ul_" + node.Id + container + "' style='display:none'>");
                            HtmlHelperExt.DepartmentsTreeNodeOnDemandCheckbox(node, propertyName, treeMode, container, onClickfunction, ref sb, checkboxChange,false,additionalClass);
                            sb.Append("</ul>");
                            sb.Append("</li>");
                        }
                        else
                        {
                            sb.Append("<li id='li_" + node.Id + "'>");
                            sb.Append("<div class='row row_directory last " + selectableClass + " " + isSelectedClass + "'>");
                            sb.Append("<div class='d-flex flex-row'>");
                            sb.Append("<div class='col-md-1 checkboxNode'>");
                            sb.Append("<input type='checkbox' onClick='" + checkboxChange + "(this,`" + node.Id + "`,`" + node.DepartmentNumber + "`,`" + node.Name + "`)' />");
                            sb.Append("</div>");
                            sb.Append("<div class='col-lg-3 dir-num'>");
                            sb.Append("<input type='' value='" + node.DepartmentNumber + "'  class='hdnDepartmentNumber __hdnSticky' style='background:none; border:none;'/>");
                            sb.Append("</div>");
                            sb.Append("<div class='flex-grow-1'>");
                            sb.Append("<div data-container='" + container + "' data-Name='" + propertyName + "' data-value='" + node.Id + "' data-func='" + onClickfunction + "' class=' " + selectMode + "' id='" + key + "' title='" + node.Id + "'>" + node.Name + "</div>");
                            sb.Append("</div>");
                            sb.Append("<div class='col-md-1 glyphicon_ok'>");
                            sb.Append("<i class='glyphicon glyphicon-ok'></i>");
                            sb.Append("</div>");
                            sb.Append("</div>");
                            sb.Append("</div>");
                            sb.Append("</li>");
                        }
                    }
                }
            }
            return MvcHtmlString.Create(sb.ToString());
        }

        public static MvcHtmlString DepartmentsTreeNodeOnDemand<TModel>(this HtmlHelper<TModel> helper, TreeNode treeNode, string propertyName, TreeMode treeMode, string container, string onClickfunction, bool isRefreshEnabled = false)
        {
            string selectableClass = string.Empty;
            string isSelectedClass = string.Empty;
            string selectMode = string.Empty;
            StringBuilder sb = new StringBuilder();
            if (treeMode == TreeMode.Multiple)
            {
                selectMode = "multiple_select_dept";
            }
            else
            {
                selectMode = "single_select_dept";
            }
            if (treeNode != null && treeNode.Childs != null)
            {
                foreach (var node in treeNode.Childs)
                {
                    selectableClass = string.Empty;
                    isSelectedClass = string.Empty;
                    string key = Guid.NewGuid().ToString();
                    if (node != null)
                    {
                        if (node.Selectable)
                        {
                            selectableClass = "selectable_node";
                        }
                        if (node.IsSelected)
                        {
                            isSelectedClass = "selected";
                        }
                        if (node.HasChilds)
                        {
                            sb.Append("<li id='li_" + node.Id + "'>");
                            sb.Append("<div class='row row_directory " + selectableClass + " " + isSelectedClass + " selected_bottom' id='divspan_" + node.Id + container + "'>");

                            sb.Append("<input type='hidden' value='" + node.DepartmentNumber + "'  class='hdnDepartmentNumber __hdnSticky'/>");
                            sb.Append("<div data-container='" + container + "' data-Name='" + propertyName + "' data-value='" + node.Id + "' data-func='" + onClickfunction + "' class='col-md-8 " + selectMode + "' id='" + key + "' title='" + node.Id + "'>" + node.Name + "</div>");
                            sb.Append("<div class='col-md-4' >");
                            sb.Append("<a class='node'  >");
                            sb.Append("<span id='span_" + node.Id + container + "' data-node-container='" + container + "' data-func='" + onClickfunction + "' class='fa fa-plus LoadedData'  data-isRefreshEnabled='" + isRefreshEnabled + "' data-node-id='" + node.Id + "'></span>");
                            sb.Append("</a>");
                            sb.Append("</div>");
                            sb.Append("<div class='col-md-1 glyphicon_ok'>");
                            sb.Append("<i class='glyphicon glyphicon-ok'></i>");
                            sb.Append("</div>");
                            sb.Append("</div>");
                            sb.Append("<ul id='ul_" + node.Id + container + "' style='display:none'>");
                            HtmlHelperExt.DepartmentsTreeNodeOnDemand(node, propertyName, treeMode, container, onClickfunction, ref sb);
                            sb.Append("</ul>");
                            sb.Append("</li>");
                        }
                        else
                        {
                            sb.Append("<li id='li_" + node.Id + "'>");
                            sb.Append("<div class='row row_directory last " + selectableClass + " " + isSelectedClass + "'>");
                            //sb.Append("<div class='col-md-1'>");
                            //sb.Append("</div>");
                            sb.Append("<input type='hidden' value='" + node.DepartmentNumber + "'  class='hdnDepartmentNumber __hdnSticky'/>");
                            sb.Append("<div data-container='" + container + "' data-Name='" + propertyName + "' data-value='" + node.Id + "' data-func='" + onClickfunction + "' class='col-md-8 " + selectMode + "' id='" + key + "' title='" + node.Id + "'>" + node.Name + "</div>");
                            sb.Append("<div class='col-md-1 glyphicon_ok'>");
                            sb.Append("<i class='glyphicon glyphicon-ok'></i>");
                            sb.Append("</div>");
                            sb.Append("</div>");
                            sb.Append("</li>");
                        }
                    }
                }
            }
            return MvcHtmlString.Create(sb.ToString());
        }

        public static MvcHtmlString DepartmentsTreeNodeCheckbox<TModel>(this HtmlHelper<TModel> helper, TreeNode treeNode, string propertyName, TreeMode treeMode, string container, string onClickfunction, string checkboxChange = "CheckboxChange", string additionalClass = "")
        {
            string selectableClass = string.Empty;
            string isSelectedClass = string.Empty;
            string selectMode = string.Empty;
            StringBuilder sb = new StringBuilder();

            selectMode = "multiple_select_dept";

            if (treeNode != null && treeNode.Childs != null)
            {
                foreach (var node in treeNode.Childs)
                {
                    selectableClass = string.Empty;
                    isSelectedClass = string.Empty;
                    string key = Guid.NewGuid().ToString();
                    if (node != null)
                    {
                        if (node.Selectable)
                        {
                            selectableClass = "selectable_node";
                        }
                        if (node.IsSelected)
                        {
                            isSelectedClass = "selected";
                        }
                        if (node.HasChilds)
                        {
                            sb.Append("<li id='li_" + node.Id + "' title='" + node.Name + "'>");
                            sb.Append("<div class='row row_directory " + selectableClass + " " + isSelectedClass + " selected_bottom' id='divspan_" + node.Id + container + "'>");
                            sb.Append("<div class='d-flex flex-row'>");
                            sb.Append("<div class='col-md-1 checkboxNode'>");
                            sb.Append("<input type='checkbox' onClick='" + checkboxChange + "(this,`" + node.Id + "`,`" + node.DepartmentNumber + "`,`" + node.Name + "`)' />");
                            sb.Append("</div>");
                            sb.Append("<div class='col-lg-3 dir-num'>");
                            sb.Append("<input type='' value='" + node.DepartmentNumber + "'  class='hdnDepartmentNumber __hdnSticky' style='background:none; border:none;'/>");
                            sb.Append("</div>");
                            sb.Append("<div class='flex-grow-1'>");
                            sb.Append("<div data-container='" + container + "' data-Name='" + propertyName + "' data-value='" + node.Id + "' data-func='" + onClickfunction + "' class='" + selectMode + "' id='" + key + "' title='" + node.Name + "'>" + node.Name + "</div>");
                            sb.Append("</div>");
                            sb.Append("<a class='node'  >");
                            sb.Append("<span id='span_" + node.Id + container + "' data-node-container='" + container + "' data-func='" + onClickfunction + "' class='fa fa-plus InternalLoadedData " + additionalClass + " ' data-multi-node-id='" + node.Id + "'></span>");
                            sb.Append("</a>");
                            sb.Append("<div class='col-md-1 glyphicon_ok'>");
                            sb.Append("<i class='glyphicon glyphicon-ok'></i>");
                            sb.Append("</div>");
                            sb.Append("</div>");


                            //sb.Append("<input type='hidden' value='" + node.DepartmentNumber + "'  class='hdnDepartmentNumber __hdnSticky'/>");
                            //sb.Append("<div data-container='" + container + "' data-Name='" + propertyName + "' data-value='" + node.Id + "' data-func='" + onClickfunction + "' class='col-md-8 " + selectMode + "' id='" + key + "' title='" + node.Name + "'>" + node.Name + "</div>");
                            sb.Append("<div class='col-md-4'>");
                            //sb.Append("<a class='node'  >");
                            //sb.Append("<span id='span_" + node.Id + container + "' data-node-container='" + container + "' data-func='" + onClickfunction + "' class='fa fa-plus InternalLoadedData' data-node-id='" + node.Id + "'></span>");
                            //sb.Append("</a>");
                            sb.Append("</div>");
                            //sb.Append("<div class='col-md-1 glyphicon_ok'>");
                            //sb.Append("<i class='glyphicon glyphicon-ok'></i>");
                            //sb.Append("</div>");
                            sb.Append("</div>");
                            sb.Append("<ul id='ul_" + node.Id + container + "'style='display:none'>");
                            HtmlHelperExt.DepartmentsTreeNodeCheckbox(node, propertyName, treeMode, container, onClickfunction, ref sb, checkboxChange);
                            sb.Append("</ul>");
                            sb.Append("</li>");
                        }
                        else
                        {
                            sb.Append("<li id='li_" + node.Id + "' title='" + node.Name + "'>");
                            sb.Append("<div class='row row_directory last " + selectableClass + " " + isSelectedClass + "'>");
                            sb.Append("<div class='d-flex flex-row'>");
                            sb.Append("<div class='col-md-1 checkboxNode'>");
                            sb.Append("<input type='checkbox' onClick='" + checkboxChange + "(this,`" + node.Id + "`,`" + node.DepartmentNumber + "`,`" + node.Name + "`)' />");
                            sb.Append("</div>");
                            sb.Append("<div class='col-lg-3 dir-num'>");
                            sb.Append("<input type='' value='" + node.DepartmentNumber + "'  class='hdnDepartmentNumber __hdnSticky' style='background:none; border:none;'/>");
                            sb.Append("</div>");
                            sb.Append("<div class='flex-grow-1'>");
                            sb.Append("<div data-container='" + container + "' data-Name='" + propertyName + "' data-value='" + node.Id + "' data-func='" + onClickfunction + "' class='col-md-8 " + selectMode + "' id='" + key + "' title='" + node.Name + "'>" + node.Name + "</div>");
                            sb.Append("</div>");
                            sb.Append("<div class='col-md-1 glyphicon_ok'>");
                            sb.Append("<i class='glyphicon glyphicon-ok'></i>");
                            sb.Append("</div>");
                            sb.Append("</div>");


                            sb.Append("</div>");
                            sb.Append("</li>");
                        }
                    }
                }
            }
            return MvcHtmlString.Create(sb.ToString());
        }

        public static MvcHtmlString DepartmentsTreeNode<TModel>(this HtmlHelper<TModel> helper, TreeNode treeNode, string propertyName, TreeMode treeMode, string container, string onClickfunction)
        {
            string selectableClass = string.Empty;
            string isSelectedClass = string.Empty;
            string selectMode = string.Empty;
            StringBuilder sb = new StringBuilder();
            if (treeMode == TreeMode.Multiple)
            {
                selectMode = "multiple_select_dept";
            }
            else
            {
                selectMode = "single_select_dept";
            }
            if (treeNode != null && treeNode.Childs != null)
            {
                foreach (var node in treeNode.Childs)
                {
                    selectableClass = string.Empty;
                    isSelectedClass = string.Empty;
                    string key = Guid.NewGuid().ToString();
                    if (node != null)
                    {
                        if (node.Selectable)
                        {
                            selectableClass = "selectable_node";
                        }
                        if (node.IsSelected)
                        {
                            isSelectedClass = "selected";
                        }
                        if (node.HasChilds)
                        {
                            sb.Append("<li id='li_" + node.Id + "' title='" + node.Name + "'>");
                            sb.Append("<div class='row row_directory " + selectableClass + " " + isSelectedClass + " selected_bottom' id='divspan_" + node.Id + container + "'>");

                            sb.Append("<input type='hidden' value='" + node.DepartmentNumber + "'  class='hdnDepartmentNumber __hdnSticky'/>");
                            sb.Append("<div data-container='" + container + "' data-Name='" + propertyName + "' data-value='" + node.Id + "' data-func='" + onClickfunction + "' class='col-md-8 " + selectMode + "' id='" + key + "' title='" + node.Name + "'>" + node.Name + "</div>");
                            sb.Append("<div class='col-md-4'>");
                            sb.Append("<a class='node'  >");
                            sb.Append("<span id='span_" + node.Id + container + "' data-node-container='" + container + "' data-func='" + onClickfunction + "' class='fa fa-plus InternalLoadedData' data-node-id='" + node.Id + "'></span>");
                            sb.Append("</a>");
                            sb.Append("</div>");
                            sb.Append("<div class='col-md-1 glyphicon_ok'>");
                            sb.Append("<i class='glyphicon glyphicon-ok'></i>");
                            sb.Append("</div>");
                            sb.Append("</div>");
                            sb.Append("<ul id='ul_" + node.Id + container + "' style='display:none'>");
                            HtmlHelperExt.DepartmentsTreeNode(node, propertyName, treeMode, container, onClickfunction, ref sb);
                            sb.Append("</ul>");
                            sb.Append("</li>");
                        }
                        else
                        {
                            sb.Append("<li id='li_" + node.Id + "' title='" + node.Name + "'>");
                            sb.Append("<div class='row row_directory last " + selectableClass + " " + isSelectedClass + "'>");
                            //sb.Append("<div class='col-md-1'>");
                            //sb.Append("</div>");
                            sb.Append("<input type='hidden' value='" + node.DepartmentNumber + "'  class='hdnDepartmentNumber __hdnSticky'/>");
                            sb.Append("<div data-container='" + container + "' data-Name='" + propertyName + "' data-value='" + node.Id + "' data-func='" + onClickfunction + "' class='col-md-8 " + selectMode + "' id='" + key + "' title='" + node.Name + "'>" + node.Name + "</div>");
                            sb.Append("<div class='col-md-1 glyphicon_ok'>");
                            sb.Append("<i class='glyphicon glyphicon-ok'></i>");
                            sb.Append("</div>");
                            sb.Append("</div>");
                            sb.Append("</li>");
                        }
                    }
                }
            }
            return MvcHtmlString.Create(sb.ToString());
        }
        #endregion Tree




















        private static void PermissionsEditableTreeNodeArticle(TreeNode treeNode, string propertyName, TreeMode treeMode, string container, ref StringBuilder sb)
        {
            string selectableClass = string.Empty;
            string isSelectedClass = string.Empty;
            string isCheckedProp = string.Empty;
            string selectMode = string.Empty;
            if (treeMode == TreeMode.Multiple)
            {
                selectMode = "multiple_select";
            }
            else
            {
                selectMode = "single_select";
            }
            foreach (var node in treeNode.Childs)
            {
                selectableClass = string.Empty;
                isSelectedClass = string.Empty;
                isCheckedProp = string.Empty;
                string key = Guid.NewGuid().ToString();
                if (node != null)
                {
                    //if (node.Selectable)
                    //{
                    selectableClass = "selectable_node";
                    //}
                    if (node.IsSelected)
                    {
                        isSelectedClass = "selected";
                        isCheckedProp = "checked='checked'";
                    }
                    if (node.Childs.Count > 0)
                    {
                        sb.Append("<div data-li-id='" + node.Id + "' id='li_" + node.Id + "' class='article col-lg-3 col-md-4  col-sm-6 col-xs-12' data-row='1'>");
                        sb.Append("<div class='article-design " + selectableClass + " " + isSelectedClass + " selected_bottom' id='divspan_" + node.Id + container + "'>");
                        sb.Append("<div data-container='" + container + "' data-Name='" + propertyName + "' data-id='" + node.Id + "' class='d-flex flex-row " + selectMode + "'>");
                        sb.Append("<div class='checkbox mt-0 mr-1'>");
                        sb.Append("<label class='px-0'><input type='checkbox' value='' class='chkGroupPermission parent' " + isCheckedProp + " data-container='" + container + "' data-Name='" + propertyName + "' data-id='" + node.Id + "' id='" + key + "'><span class='cr'><i class='cr-icon glyphicon glyphicon-ok'></i></span></label>");
                        sb.Append("</div>");
                        sb.Append("<div class='flex-grow-1 text-over'>");
                        sb.Append("<span class='treate-name'>" + node.Name + "</span>");
                        sb.Append("<span class='perm'>الصلاحيات المضافة" + "<strong class='badge badge-pill mx-2'>" + node.Childs.Count + "/0</strong></span>");
                        sb.Append("</div>");
                        sb.Append("<i class='fas fa-sort-down align-self-end expand'></i>");
                        sb.Append("</div>");
                        sb.Append("<div id='ul_" + node.Id + container + "' class='ip-details'>");
                        sb.Append("<div class='treate d-flex flex-sm-row flex-column'>");
                        sb.Append("<h3 class='white-c-title'>" + node.Name + "</h3>");
                        sb.Append("<div><span class='roundedred ml-sm-3 ml-auto'>" + node.Childs.Count + "</span><span class='pl-2 gray4'>" + DbRes.TResource("Admin.User.Permissions") + "</span></div>");
                        sb.Append("<div class='d-flex ml-sm-auto'>");
                        //sb.Append("<div class='checkbox mt-0'>");
                        //sb.Append("<label for='chkAllGroupPermission" + node.Id + "__" + container + "'>");
                        //sb.Append("<input id='chkAllGroupPermission" + node.Id + "__" + container + "' type='checkbox' value='' data-value='" + node.Id +"'>");
                        //sb.Append("<span class='cr'><i class='cr-icon glyphicon glyphicon-ok'></i></span>");
                        //sb.Append("<span class='black'>" + DbRes.TResource("User.File.SelectAll") + "</span>");
                        //sb.Append("</label>");
                        //sb.Append("</div>");
                        sb.Append("</div>");
                        sb.Append("</div>");
                        sb.Append("<div class='row'>");
                        HtmlHelperExt.PermissionsEditableTreeNodeArticle(node, propertyName, treeMode, container, ref sb);
                        sb.Append("</div>");
                        sb.Append("</div>");
                        sb.Append("</div>");
                        sb.Append("</div>");
                    }
                    else
                    {
                        sb.Append("<div data-li-id='" + node.Id + "' id='li_" + node.Id + "' class='article2 col-lg-3 col-md-4  col-sm-6 col-xs-12' data-row='1'>");
                        sb.Append("<div class='article-design " + selectableClass + " " + isSelectedClass + " selected_bottom' id='divspan_" + node.Id + container + "'>");
                        sb.Append("<div data-container='" + container + "' data-Name='" + propertyName + "' data-id='" + node.Id + "' class='d-flex flex-row " + selectMode + "'>");
                        sb.Append("<div class='checkbox mt-0 mr-1'>");
                        sb.Append("<label class='px-0'><input type='checkbox' value='' data-value='" + node.Id + "' class='chkGroupPermission child' " + isCheckedProp + " data-container='" + container + "' data-Name='" + propertyName + "' data-id='" + node.Id + "' id='" + key + "'><span class='cr'><i class='cr-icon glyphicon glyphicon-ok'></i></span>");
                        if (node.IsSelected)
                        {
                            sb.Append("<span id='" + node.Id + "'>");
                            sb.Append("<input type='hidden' value='" + node.Id + "' name='" + propertyName + "' id='" + node.Id + "'>");
                            sb.Append("</span>");
                        }
                        sb.Append("</label>");
                        sb.Append("</div>");
                        sb.Append("<div class='text-over'>");
                        sb.Append("<span class='treate-name'>" + node.Name + "</span>");
                        sb.Append("</div>");
                        sb.Append("</div>");
                        sb.Append("</div>");
                        sb.Append("</div>");
                    }
                }
            }
        }
        public static MvcHtmlString DepartmentsTreeAdmin<TModel, TProperty>(this HtmlHelper<TModel> helper, TreeViewModel treeViewModel, Expression<Func<TModel, TProperty>> expression, TreeMode treeMode, string treeId, bool isAjaxCall, string onClickfunction = "", string validationGroup = "", string divContainerId = "")
        {
            if (treeMode == TreeMode.Multiple)
            {
                return HtmlHelperExt.DepartmentsMultipleTree(helper, treeViewModel, expression, treeMode, treeId, isAjaxCall, onClickfunction, validationGroup);
            }
            else if (treeMode == TreeMode.Single)
            {
                return HtmlHelperExt.DepartmentsSingleTreeAdmin(helper, treeViewModel, expression, treeMode, treeId, isAjaxCall, onClickfunction, validationGroup, divContainerId);
            }
            else
            {
                return HtmlHelperExt.DepartmentsSingleTreeNotMandatory(helper, treeViewModel, expression, treeMode, treeId, isAjaxCall, onClickfunction, validationGroup, divContainerId);
            }
        }
        private static MvcHtmlString DepartmentsSingleTreeAdmin<TModel, TProperty>(this HtmlHelper<TModel> helper, TreeViewModel treeViewModel, Expression<Func<TModel, TProperty>> expression, TreeMode treeMode, string treeId, bool isAjaxCall, string onClickfunction = "", string validationGroup = "", string divContainerId = "")
        {
            string propertyName = ExpressionHelper.GetExpressionText(expression);
            StringBuilder sb = new StringBuilder();
            var attrs = new Dictionary<string, object>();
            attrs.Add("class", "hdnDepartmentId");
            attrs.Add("data-container", treeId);
            attrs.Add("data-func", onClickfunction);
            sb.Append("<div id='divForm__" + treeId + "'>");
            sb.Append(helper.Label("الادارة", new { @class = "control-label" }));
            sb.Append(helper.Label("*", new { @class = "stark ignore-label" }));

            sb.Append("<div class='row'>");
            sb.Append("<div class='col-xs-4 col-xs-4 none_left'>");
            sb.Append("<input type='text' data-func='" + onClickfunction + "' data-container='" + treeId + "' onkeypress ='return IsNumeric(event);' class='form-control txtDepartmentNumber' maxlength='20' />");
            sb.Append("</div>");
            sb.Append("<div class='col-xs-8 none_right'>");
            sb.Append("<div class='input-group'>");
            sb.Append("<input  type='text' data-func='" + onClickfunction + "' data-container='" + treeId + "' class='form-control txtDepartmentName' maxlength='40' />");
            sb.Append("<span class='input-group-prepend'>");
            sb.Append("<div class='input-group-text'>");
            sb.Append("<a onclick=ShowHideTreeDialog('#divContainer__" + treeId + "') title=" + DbRes.TResource("Global.Tree.Show") + "><span class='fa fa-filter'> </span> </a>");
            sb.Append("</div></span>");
            sb.Append("</div></div>");
            sb.Append(helper.HiddenGroupFor(expression, attrs, validationGroup));
            sb.Append("</div></div>");
            //Tree dialog
            sb.Append("<div class='modal fade in ' id='modal__" + treeId + "' role='dialog'>");
            sb.Append("<div class='modal-dialog medium-dialog'>");
            sb.Append("<div class='modal-content modal-height'>");
            sb.Append("<div class='modal-header'>");
            sb.Append("<button type='button' class='close' data-dismiss='modal'>×</button>");
            sb.Append("<div class='d-sm-flex d-block align-self-center'>");
            sb.Append("<div class=''>");
            sb.Append("<span class='site-color main-title2'></span>");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("<div class='modal-body' id='modalBody__" + treeId + "'>");
            //Tree Start
            sb.Append("<div class='accordion' id='divAcc__" + treeId + "'>");
            sb.Append("<div class='panel-body panel-style panel-popup' style='display: none;' id='divContainer__" + treeId + "' data-func='" + onClickfunction + "' data-Name='" + propertyName + "'>");
            sb.Append("<div id='divDir__" + treeId + "' class='directory'>");
            sb.Append("<div class='row'>");
            sb.Append("<div class='tree_parties'>");
            sb.Append("<div class='group_top_directory'>");
            sb.Append("<ul class='tree' >");
            if (treeViewModel.RootNode != null)
            {
                HtmlHelperExt.DepartmentsTreeNode(treeViewModel.RootNode, propertyName, treeMode, treeId, onClickfunction, ref sb);
            }
            sb.Append("</ul>");
            sb.Append("</div>");
            sb.Append("<div class='col-md-3'></div>");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");
            //End Tree
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");
            //End tree dialog

            //sb.Append("<script>");
            //sb.Append("RepiarTreeHtml('" + treeId + "','" + divContainerId + "')");
            //sb.Append("</script>");
            if (isAjaxCall)
            {
                sb.Append("<script>");
                sb.Append("AppendDepartmentText('" + treeId + "')");
                sb.Append("</script>");
            }
            if (ModelMetadata.FromLambdaExpression(expression, helper.ViewData).Model != null)
            {
                string expressionValue = ModelMetadata.FromLambdaExpression(expression, helper.ViewData).Model.ToString();
                if (!string.IsNullOrEmpty(expressionValue) && expressionValue != "0")
                {
                    sb.Append("<script>");
                    sb.Append("FillOrgUnitsTreeControls('" + treeId + "','" + expressionValue + "')");
                    sb.Append("</script>");
                }
            }
            return MvcHtmlString.Create(sb.ToString());
        }


        public static MvcHtmlString DepartmentsTreeNotMandetory<TModel, TProperty>(this HtmlHelper<TModel> helper, TreeViewModel treeViewModel, Expression<Func<TModel, TProperty>> expression, TreeMode treeMode, string treeId, bool isAjaxCall, string onClickfunction = "", string validationGroup = "", string divContainerId = "", bool isRefreshEnabled = false)
        {
            string propertyName = ExpressionHelper.GetExpressionText(expression);
            StringBuilder sb = new StringBuilder();
            var attrs = new Dictionary<string, object>();
            attrs.Add("class", "hdnDepartmentId");
            attrs.Add("data-container", treeId);
            attrs.Add("data-func", onClickfunction);
            sb.Append("<div class='' id='divForm__" + treeId + "'>");
            sb.Append(helper.LabelFor(expression, new { @class = "control-label" }));
            sb.Append("<div class='row'>");
            sb.Append("<div class='col-xs-4 col-xs-4 none_left'>");
            sb.Append("<input type='text' data-func='" + onClickfunction + "' data-container='" + treeId + "' onkeypress ='return IsNumeric(event);' class='form-control InboundDepNum txtDepartmentNumber' maxlength='20' />");
            sb.Append("</div>");
            sb.Append("<div class='col-xs-8 none_right'>");
            sb.Append("<div class='input-group'>");
            sb.Append("<input type='text' data-func='" + onClickfunction + "' data-container='" + treeId + "' class='form-control InboundDepName txtDepartmentName' maxlength='40' />");
            sb.Append("<span class='input-group-addon link'>");
            sb.Append("<div class='panel-title'>");
            sb.Append("<a onclick=ShowHideTree('#divContainer__" + treeId + "') title=" + DbRes.TResource("Global.Tree.Show") + "><span class='fa fa-filter'> </span> </a>");
            sb.Append("</div></span>");
            sb.Append("</div></div>");
            sb.Append(helper.HiddenGroupFor(expression, attrs, validationGroup));
            sb.Append("</div></div>");
            sb.Append("<div class='accordion' id='divAcc__" + treeId + "'>");
            sb.Append("<div class='panel-body panel-style panel-popup' style='display: none;' id='divContainer__" + treeId + "' data-func='" + onClickfunction + "' data-Name='" + propertyName + "'>");
            sb.Append("<div id='divDir__" + treeId + "' class='directory'>");
            sb.Append("<div class='row'>");
            sb.Append("<div class='tree_parties'>");
            sb.Append("<div class='group_top_directory'>");
            sb.Append("<ul class='tree' >");
            if (treeViewModel.RootNode != null)
            {
                HtmlHelperExt.DepartmentsTreeNodeOnDemand(treeViewModel.RootNode, propertyName, treeMode, treeId, onClickfunction, ref sb, isRefreshEnabled);
            }
            sb.Append("</ul>");
            sb.Append("</div>");
            sb.Append("<div class='col-md-3'></div>");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("<script>");
            sb.Append("RepiarTreeHtml('" + treeId + "','" + divContainerId + "')");
            sb.Append("</script>");
            if (isAjaxCall)
            {
                sb.Append("<script>");
                sb.Append("AppendDepartmentText('" + treeId + "')");
                sb.Append("</script>");
            }
            if (ModelMetadata.FromLambdaExpression(expression, helper.ViewData).Model != null)
            {
                string expressionValue = ModelMetadata.FromLambdaExpression(expression, helper.ViewData).Model.ToString();
                if (!string.IsNullOrEmpty(expressionValue) && expressionValue != "0")
                {
                    sb.Append("<script>");
                    sb.Append("FillDepartmentTreeControlsById('" + treeId + "','" + expressionValue + "')");
                    sb.Append("</script>");
                }
            }
            //helper.ViewContext.Controller.ViewBag.validationGroup = validationGroup;
            return MvcHtmlString.Create(sb.ToString());
        }
        public static MvcHtmlString DepartmentsNotMandatoryTreeOnDemand<TModel, TProperty>(this HtmlHelper<TModel> helper, TreeViewModel treeViewModel, Expression<Func<TModel, TProperty>> expression, TreeMode treeMode, string treeId, bool isAjaxCall, string onClickfunction = "", string validationGroup = "", string divContainerId = "", bool isRefreshEnabled = false)
        {
            string propertyName = ExpressionHelper.GetExpressionText(expression);
            StringBuilder sb = new StringBuilder();
            var attrs = new Dictionary<string, object>();
            attrs.Add("class", "hdnDepartmentId");
            attrs.Add("data-container", treeId);
            attrs.Add("data-func", onClickfunction);
            sb.Append("<div class='' id='divForm__" + treeId + "'>");
            sb.Append(helper.LabelFor(expression, new { @class = "control-label" }));
            sb.Append("<div class='row'>");
            sb.Append("<div class='col-xs-4 col-xs-4 none_left'>");
            sb.Append("<input type='text' data-func='" + onClickfunction + "' data-container='" + treeId + "' onkeypress ='return IsNumeric(event);' class='form-control InboundDepNum txtDepartmentNumber' maxlength='20' />");
            sb.Append("</div>");
            sb.Append("<div class='col-xs-8 none_right'>");
            sb.Append("<div class='input-group'>");
            sb.Append("<input type='text' data-func='" + onClickfunction + "' data-container='" + treeId + "' class='form-control InboundDepName txtDepartmentName' maxlength='40' />");
            sb.Append("<span class='input-group-addon link'>");
            sb.Append("<div class='panel-title'>");
            sb.Append("<a onclick=ShowHideTree('#divContainer__" + treeId + "') title=" + DbRes.TResource("Global.Tree.Show") + "><span class='fa fa-filter'> </span> </a>");
            sb.Append("</div></span>");
            sb.Append("</div></div>");
            sb.Append(helper.HiddenGroupFor(expression, attrs, validationGroup));
            sb.Append("</div></div>");
            sb.Append("<div class='accordion' id='divAcc__" + treeId + "'>");
            sb.Append("<div class='panel-body panel-style' style='display: none;' id='divContainer__" + treeId + "' data-func='" + onClickfunction + "' data-Name='" + propertyName + "'>");
            sb.Append("<div id='divDir__" + treeId + "' class='directory'>");
            sb.Append("<div class='row'>");
            sb.Append("<div class='tree_parties'>");
            sb.Append("<div class='group_top_directory'>");
            sb.Append("<ul class='tree' >");
            if (treeViewModel.RootNode != null)
            {
                HtmlHelperExt.DepartmentsTreeNodeOnDemand(treeViewModel.RootNode, propertyName, treeMode, treeId, onClickfunction, ref sb, isRefreshEnabled);
            }
            sb.Append("</ul>");
            sb.Append("</div>");
            sb.Append("<div class='col-md-3'></div>");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("<script>");
            sb.Append("RepiarTreeHtml('" + treeId + "','" + divContainerId + "')");
            sb.Append("</script>");
            if (isAjaxCall)
            {
                sb.Append("<script>");
                sb.Append("AppendDepartmentText('" + treeId + "')");
                sb.Append("</script>");
            }
            //helper.ViewContext.Controller.ViewBag.validationGroup = validationGroup;
            return MvcHtmlString.Create(sb.ToString());
        }

        #endregion Tree
        public static MvcHtmlString CustomLabelFor<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, object htmlAttributes = null, string labelText = null)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            if (metadata.IsRequired)
            {
                var Attributes = helper.MergeHtmlAttributes(htmlAttributes, new { @class = "LredColor" });
                return helper.LabelFor(expression, labelText, Attributes);
            }
            return helper.LabelFor(expression, labelText, htmlAttributes);
        }
        public static IDictionary<string, object> MergeHtmlAttributes(this HtmlHelper helper, object htmlAttributesObject, object defaultHtmlAttributesObject)
        {
            var concatKeys = new string[] { "class" };
            var htmlAttributesDict = htmlAttributesObject as IDictionary<string, object>;
            var defaultHtmlAttributesDict = defaultHtmlAttributesObject as IDictionary<string, object>;
            RouteValueDictionary htmlAttributes = (htmlAttributesDict != null)
                ? new RouteValueDictionary(htmlAttributesDict)
                : HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributesObject);
            RouteValueDictionary defaultHtmlAttributes = (defaultHtmlAttributesDict != null)
                ? new RouteValueDictionary(defaultHtmlAttributesDict)
                : HtmlHelper.AnonymousObjectToHtmlAttributes(defaultHtmlAttributesObject);
            foreach (var item in htmlAttributes)
            {
                if (concatKeys.Contains(item.Key))
                {
                    defaultHtmlAttributes[item.Key] = (defaultHtmlAttributes[item.Key] != null)
                        ? string.Format("{0} {1}", defaultHtmlAttributes[item.Key], item.Value)
                        : item.Value;
                }
                else
                {
                    defaultHtmlAttributes[item.Key] = item.Value;
                }
            }
            return defaultHtmlAttributes;
        }
        public static MvcHtmlString NativeTree(this HtmlHelper helper, TreeViewModel treeViewModel, string treeId, string validationGroup = "")
        {
            var stringBuilder = new StringBuilder();
            if (treeViewModel.RootNode != null)
            {
                stringBuilder.Append("<ul>");
                stringBuilder.Append($"<li id='node_{treeViewModel.RootNode.Id}' parentId='{treeViewModel.RootNode.ParentId}' hasChild='{treeViewModel.RootNode.HasChilds}' class='root node_Native jstree-closed' node='{treeViewModel.RootNode.Id}' number='{treeViewModel.RootNode.DepartmentNumber}'>");
                stringBuilder.Append(treeViewModel.RootNode.Name);
                NativeTreeNode(treeViewModel.RootNode, ref stringBuilder);
                stringBuilder.Append("</li>");
                stringBuilder.Append("</ul>");
            }
            helper.ViewContext.Controller.ViewBag.validationGroup = validationGroup;
            return MvcHtmlString.Create(stringBuilder.ToString());
        }
        public static MvcHtmlString ExternalPartyNativeTree(this HtmlHelper helper, TreeViewModel treeViewModel, string treeId, string validationGroup = "")
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("<div id='" + treeId + "' class='tree jstree_scroll push-down-20'>");
            if (treeViewModel.RootNode != null)
            {
                stringBuilder.Append("<ul>");
                stringBuilder.Append("<li class='root node_Native jstree-closed' node='" + treeViewModel.RootNode.Id + "'>");
                stringBuilder.Append(treeViewModel.RootNode.Name);
                NativeTreeNode(treeViewModel.RootNode, ref stringBuilder);
                stringBuilder.Append("</li>");
                stringBuilder.Append("</ul>");
            }
            stringBuilder.Append("</div>");
            helper.ViewContext.Controller.ViewBag.validationGroup = validationGroup;
            return MvcHtmlString.Create(stringBuilder.ToString());
        }
        public static void NativeTreeNode(TreeNode treeNode, ref StringBuilder stringBuilder)
        {
            foreach (var node in treeNode.Childs)
            {
                string isSelectedClass = string.Empty;
                if (node.IsSelected)
                {
                    isSelectedClass = "_selected";
                }
                stringBuilder.Append("<ul>");
                if (node != null)
                {
                    if (node.Childs.Count > 0 || node.HasChilds)
                    {
                        stringBuilder.Append($"<li id='node_{node.Id}' parentId='{node.ParentId}' hasChild='{node.HasChilds}' class='jstree-closed LoadedData root node_Native {isSelectedClass}'  node='{node.Id}' number='{node.DepartmentNumber}'>");
                        stringBuilder.Append(node.Name);
                        NativeTreeNode(node, ref stringBuilder);
                    }
                    else
                    {
                        stringBuilder.Append($"<li id='node_{node.Id}' parentId='{node.ParentId}' hasChild='{node.HasChilds}' class='jstree-node  jstree-leaf child node_Native {isSelectedClass}' node='{node.Id}' number='{node.DepartmentNumber}'>");
                        stringBuilder.Append(node.Name);
                    }
                    stringBuilder.Append("</li>");
                }
                stringBuilder.Append("</ul>");
            }
        }
        public static MvcHtmlString ChildsNativeTree(this HtmlHelper helper, List<TreeNode> treeNodes)
        {
            var stringBuilder = new StringBuilder();
            foreach (var node in treeNodes)
            {
                string isSelectedClass = string.Empty;
                if (node.IsSelected)
                {
                    isSelectedClass = "_selected";
                }
                stringBuilder.Append("<ul>");
                if (node != null)
                {
                    if (node.HasChilds)
                    {
                        stringBuilder.Append($"<li id='node_{node.Id}' parentId='{node.ParentId}' hasChild='{node.HasChilds}' class='jstree-closed root node_Native {isSelectedClass}'  node='{node.Id}' number='{node.DepartmentNumber}'>");
                        stringBuilder.Append(node.Name);
                    }
                    else
                    {
                        stringBuilder.Append($"<li id='node_{node.Id}' parentId='{node.ParentId}' hasChild='{node.HasChilds}' class='jstree-node  jstree-leaf child node_Native {isSelectedClass}' node='{node.Id}' number='{node.DepartmentNumber}'>");
                        stringBuilder.Append(node.Name);
                    }
                    stringBuilder.Append("</li>");
                }
                stringBuilder.Append("</ul>");
            }
            return MvcHtmlString.Create(stringBuilder.ToString());
        }
        public static MvcHtmlString MenuLink(this HtmlHelper htmlHelper, string itemText, string actionName, string controllerName, string areaName, string cssClass = "", string permissionCode = "")
        {
            if (!string.IsNullOrEmpty(permissionCode))
            {
                if (!SessionInfo.CurrentUser.Claims.Contains(permissionCode))
                {
                    return new MvcHtmlString(string.Empty);
                }
            }
            var currentAction = htmlHelper.ViewContext.RouteData.GetRequiredString("action");
            var currentController = htmlHelper.ViewContext.RouteData.GetRequiredString("controller");
            string finalHtml;
            var linkBuilder = new TagBuilder("a");
            var urlHelper = UrlHelper.GetBaseUri();
            if (!string.IsNullOrEmpty(controllerName))
            {
                string menuURL = urlHelper.AbsoluteUri + "/" + areaName + "/" + controllerName + "/" + actionName;
                if (SystemConfigurations.EnableSSL)
                {
                    menuURL = menuURL.Replace("http://", "https://");
                }

                linkBuilder.MergeAttribute("href", menuURL);
            }
            if (!string.IsNullOrEmpty(cssClass))
            {
                linkBuilder.InnerHtml = "<span class='" + cssClass + "'></span><span class='xn-text'>" + itemText + "</span>";
            }
            else
            {
                linkBuilder.InnerHtml = itemText;
            }
            if (controllerName == currentController && actionName == currentAction)
            {
                linkBuilder.AddCssClass("active_ok");
            }
            finalHtml = linkBuilder.ToString();
            return new MvcHtmlString(finalHtml);
        }
        public static MvcHtmlString MenuFileLink(this HtmlHelper htmlHelper, string itemText, int? transactionsCount, string actionName, string controllerName, string areaName, string iconImgPath = "", string permissionCode = "")
        {
            if (!string.IsNullOrEmpty(permissionCode))
            {
                if (!SessionInfo.CurrentUser.Claims.Contains(permissionCode))
                {
                    return new MvcHtmlString(string.Empty);
                }
            }
            var currentAction = htmlHelper.ViewContext.RouteData.GetRequiredString("action");
            var currentController = htmlHelper.ViewContext.RouteData.GetRequiredString("controller");
            string finalHtml;
            var linkBuilder = new TagBuilder("a");
            var urlHelper = UrlHelper.GetBaseUri();
            if (!string.IsNullOrEmpty(controllerName))
            {
                string menuURL = urlHelper.AbsoluteUri + "/" + areaName + "/" + controllerName + "/" + actionName;
                if (SystemConfigurations.EnableSSL)
                {
                    menuURL = menuURL.Replace("http://", "https://");
                }
                linkBuilder.MergeAttribute("href", menuURL);
            }

            linkBuilder.InnerHtml = @"<i><img src='" + iconImgPath + "' width='25' height='25' /></i>" +
            "<span class='menu-title'>" + itemText + "" + " </span > ";
            if (transactionsCount.HasValue && transactionsCount.Value > 0)
            {
                linkBuilder.InnerHtml += "<span class='badge badge badge-primary badge-pill float-left ml-2'>" + transactionsCount + "</span>";
            }
            //linkBuilder.InnerHtml = itemText + "<i>" + transactionsCount + "</i>";
            //if (controllerName == currentController && actionName == currentAction)
            //{
            //    linkBuilder.AddCssClass("active_ok");
            //}

            finalHtml = linkBuilder.ToString();
            return new MvcHtmlString(finalHtml);
        }
        public static MvcHtmlString ListPaging(this HtmlHelper htmlHelper, int selectedPageIndex, int itemsCount, int pageSize, string clientFunction)
        {
            StringBuilder strListPaging = new StringBuilder();
            // int pageSize = GridHelper.PageSize;
            int pagesCount = GridHelper.PagePartitionSize;
            double allPagesCount = (double)itemsCount / pageSize;
            int previousPage = selectedPageIndex - 1;
            int nextPage = (selectedPageIndex + pagesCount);
            string previousPageTag = String.Format("<li onclick='{1}({0})'><a href='#'>«</a></li>", previousPage, clientFunction);
            string nextPageTag = String.Format("<li onclick='{1}({0})'><a href='#'>»</a></li>", nextPage, clientFunction);
            if (previousPage < 1)
            {
                previousPageTag = String.Format("<li class='disabled')'><a href='#'>«</a></li>");
            }
            if (nextPage > allPagesCount)
            {
                nextPageTag = String.Format("<li class='disabled'><a href='#'>»</a></li>");
            }
            strListPaging.Append(previousPageTag);
            for (int i = 1; i <= Math.Ceiling(allPagesCount); i++)
            {
                if (selectedPageIndex == i)
                {
                    strListPaging.Append(String.Format("<li class='active' onclick='{1}({0})'><a href='#'> {0}</a></li>", i, clientFunction));
                    continue;
                }
                if (i > previousPage && i < nextPage)
                {
                    strListPaging.Append(String.Format("<li onclick='{1}({0})'><a href='#'>{0}</a></li>", i, clientFunction));
                }
            }
            strListPaging.Append(nextPageTag);
            return new MvcHtmlString(strListPaging.ToString());
        }
        public static MvcHtmlString CheckUserChatStatus(this HtmlHelper htmlHelper, string userResource, string userName, int transactionId, int userId)
        {
            StringBuilder strBuilder = new StringBuilder();
            string html = String.Empty;
            html = String.Format("<a><span class='light-text'>{0}</span><em class='user_name indent-text'> <div class='appear' onclick='ComfirmBindingChat({1})' data-online-username='{2}' data-online-userid='{3}'></div> {4} </em> </a>", userResource, transactionId, userName, userId, userName);
            //if (userId == SessionInfo.CurrentUser.Id)
            //{
            //    html = String.Format("<a><span>{0}</span> {1}</a>", userResource, userName);
            //}
            strBuilder.Append(html);
            return new MvcHtmlString(strBuilder.ToString());
        }
        public static MvcHtmlString CheckTransactionChatStatus(this HtmlHelper htmlHelper, bool hasChat)
        {
            StringBuilder strBuilder = new StringBuilder();
            string html = String.Empty;
            if (hasChat)
            {
                html = String.Format("<span class='chat_group'><a class='inside' href='#'><i class='fa fa-comments-o'></i></a></span>");
            }
            strBuilder.Append(html);
            return new MvcHtmlString(strBuilder.ToString());
        }
        public static MvcHtmlString TextEditor(this HtmlHelper html, string textControlId, string hdnIdToSaveContent, bool readOnly, string languageShortName, string content = "", string javascriptFunName = "",
            string stampBase64Image = "", string signatureBase64Image = "", bool isContentEncoded = true)
        {
            if (SessionInfo.CurrentUser.Signature != null)
            {
                signatureBase64Image = Convert.ToBase64String(SessionInfo.CurrentUser.Signature);
            }

            if (SessionInfo.CurrentUser.Marking != null)
            {
                stampBase64Image = Convert.ToBase64String(SessionInfo.CurrentUser.Marking);
            }

            string BarCodeBase64Image = string.Empty;
            if (SessionInfo.GetObjectFromSession("BarcodeImgByte") != null)
            {
                BarCodeBase64Image = Convert.ToBase64String(SessionInfo.GetObjectFromSession("BarcodeImgByte") as byte[]);
            }

            string WaterMarkBase64Image = string.Empty;
            if (SessionInfo.GetObjectFromSession("WaterMarkImage") != null)
            {
                WaterMarkBase64Image = Convert.ToBase64String(SessionInfo.GetObjectFromSession("WaterMarkImage") as byte[]);
            }


            return MvcHtmlString.Create(Framework.Controls.TextEditor.RenderTextEditor(textControlId, hdnIdToSaveContent, readOnly,
                languageShortName, content, javascriptFunName, stampBase64Image, signatureBase64Image, isContentEncoded, BarCodeBase64Image, WaterMarkBase64Image));
        }
        private class HtmlFieldPrefixScope : IDisposable
        {
            private readonly TemplateInfo templateInfo;
            private readonly string previousHtmlFieldPrefix;
            public HtmlFieldPrefixScope(TemplateInfo templateInfo, string htmlFieldPrefix)
            {
                this.templateInfo = templateInfo;
                previousHtmlFieldPrefix = templateInfo.HtmlFieldPrefix;
                templateInfo.HtmlFieldPrefix = htmlFieldPrefix;
            }
            public void Dispose()
            {
                templateInfo.HtmlFieldPrefix = previousHtmlFieldPrefix;
            }
        }

        public static MvcHtmlString ExternalNativeTree(this HtmlHelper helper, TreeViewModel treeViewModel, string treeId, string validationGroup = "")
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("<div id='" + treeId + "' class='tree jstree_scroll push-down-20'>");
            if (treeViewModel.RootNode != null)
            {
                stringBuilder.Append("<ul>");
                stringBuilder.Append("<li class='root node_Native jstree-closed' node='" + treeViewModel.RootNode.Id + "'>");
                stringBuilder.Append(treeViewModel.RootNode.Name);
                ExternalNativeTreeNode(treeViewModel.RootNode, ref stringBuilder);
                stringBuilder.Append("</li>");
                stringBuilder.Append("</ul>");
            }
            stringBuilder.Append("</div>");
            helper.ViewContext.Controller.ViewBag.validationGroup = validationGroup;
            return MvcHtmlString.Create(stringBuilder.ToString());
        }

        public static void ExternalNativeTreeNode(TreeNode treeNode, ref StringBuilder stringBuilder)
        {
            foreach (var node in treeNode.Childs)
            {
                string isSelectedClass = string.Empty;
                if (node.IsSelected)
                {
                    isSelectedClass = "_selected";
                }
                stringBuilder.Append("<ul>");
                if (node != null)
                {
                    if (node.HasChilds)
                    {
                        stringBuilder.Append("<li class='jstree-closed root node_Native " + isSelectedClass + "'  node='" + node.Id + "'>");
                        stringBuilder.Append(node.Name);
                        //NativeTreeNode(node, ref stringBuilder);
                    }
                    else
                    {
                        stringBuilder.Append("<li class='jstree-node  jstree-leaf child node_Native " + isSelectedClass + "' node='" + node.Id + "'>");
                        stringBuilder.Append(node.Name);
                    }
                    stringBuilder.Append("</li>");
                }
                stringBuilder.Append("</ul>");
            }
        }


        public static MvcHtmlString SettingControlWithValidation<TModel>(this HtmlHelper<TModel> htmlHelper, string name, object value, SettingConfigVM settingConfigVM)
        {
            string finalResult = string.Empty;
            string result = string.Empty;
            IDictionary<string, object> unobtrusiveValidationAttributes = new Dictionary<string, object>();

            #region Unobtrusive Validation Attributes
            if (settingConfigVM.IsRequired)
            {
                //enable validation and add required validation on setting control
                unobtrusiveValidationAttributes = MergeHtmlAttributes(new Dictionary<string, object>(){
                        { "data-val", "true" },
                        { "data-val-customrequired", settingConfigVM.RequiredMessage != null ? settingConfigVM.RequiredMessage : DbRes.TValidation("Admin.RequiredField") },
                        { "data-val-customrequired-validationgroup", string.Empty},
                        { "class", "form-control" },
                }, unobtrusiveValidationAttributes);
            }
            if (string.IsNullOrEmpty(settingConfigVM.Regx) == false)
            {
                unobtrusiveValidationAttributes = MergeHtmlAttributes(new Dictionary<string, object>(){
                        { "data-val-customregularexpression-pattern", settingConfigVM.Regx},
                        { "data-val-customregularexpression", settingConfigVM.RegxMessage},
                        { "data-val-customregularexpression-validationgroup", string.Empty},
                    }, unobtrusiveValidationAttributes);
            }
            if (string.IsNullOrEmpty(settingConfigVM.RangeMessage) == false)
            {
                unobtrusiveValidationAttributes = MergeHtmlAttributes(new Dictionary<string, object>(){
                        { "data-val-customrange-minimum", settingConfigVM.Min},
                        { "data-val-customrange-maximum", settingConfigVM.Max},
                        { "data-val-customrange", settingConfigVM.RangeMessage},
                        { "data-val-customrange-validationgroup", string.Empty},
                    }, unobtrusiveValidationAttributes);
            }

            unobtrusiveValidationAttributes = MergeHtmlAttributes(new Dictionary<string, object>() { { "class", settingConfigVM.ClassName } }, unobtrusiveValidationAttributes);
            htmlHelper.ViewContext.Controller.ViewBag.validationGroup = string.Empty;
            htmlHelper.ViewContext.Controller.ViewBag.ErrorMessage = string.Empty;
            #endregion

            switch (settingConfigVM.ControlType)
            {
                case ControlType.Text:
                    #region Text
                    finalResult = htmlHelper.TextBox(name, value, unobtrusiveValidationAttributes).ToString();
                    unobtrusiveValidationAttributes = MergeHtmlAttributes(new Dictionary<string, object>() { { "MaxLength", settingConfigVM.MaxLength } }, unobtrusiveValidationAttributes);
                    break;
                #endregion
                case ControlType.Password:
                    #region Password
                    finalResult = htmlHelper.Password(name, value, unobtrusiveValidationAttributes).ToString();
                    unobtrusiveValidationAttributes = MergeHtmlAttributes(new Dictionary<string, object>() { { "MaxLength", settingConfigVM.MaxLength } }, unobtrusiveValidationAttributes);
                    break;
                #endregion
                case ControlType.Numeric:
                    #region Numeric
                    unobtrusiveValidationAttributes = MergeHtmlAttributes(new Dictionary<string, object>() { { "onkeypress", "return IsNumeric(event);" } }, unobtrusiveValidationAttributes);
                    finalResult = htmlHelper.NumericTextBoxFor(name, value, unobtrusiveValidationAttributes).ToString();
                    break;
                #endregion
                case ControlType.Dropdown:
                    #region Dropdown
                    unobtrusiveValidationAttributes = MergeHtmlAttributes(new Dictionary<string, object>() { { "id", name } }, unobtrusiveValidationAttributes);
                    finalResult = htmlHelper.AutoCompleteBuilder(autoCompleteControlid: "ddl" + name, hdnIdToSaveValue: name,
                        lookupCategory: settingConfigVM.LookupCategory, selectedId: Convert.ToString(value),
                        inputClassName: "form-control ui-autocomplete-input").ToString();
                    finalResult += htmlHelper.Hidden(name, Convert.ToString(value), unobtrusiveValidationAttributes);
                    break;
                #endregion
                case ControlType.RadioButton:
                    #region RadioButton
                    var items = settingConfigVM.Label.Split(new char[] { ',' }).ToArray();
                    var values = value.ToString().Split(new char[] { ',' }).ToArray();
                    finalResult += htmlHelper.Hidden($"{settingConfigVM.StaticName}.SettingConfigVM.ConnectionProtocolType");
                    for (int i = 0; i < items.Length; i++)
                    {
                        unobtrusiveValidationAttributes = MergeHtmlAttributes(new Dictionary<string, object>() { { "data-ConnectionProtocolType", (int)settingConfigVM.ConnectionProtocolTypes[i] } },
                            unobtrusiveValidationAttributes);
                        finalResult += htmlHelper.CustomRadioButtonFor($"{settingConfigVM.StaticName}.Value", Convert.ToBoolean(values[i]), items[i], unobtrusiveValidationAttributes);
                    }
                    break;
                #endregion
                case ControlType.Checkbox:
                    #region Checkbox
                    finalResult = htmlHelper.CheckBox(name, Convert.ToBoolean(value), unobtrusiveValidationAttributes).ToString();
                    break;
                case ControlType.Textarea:
                    #region Textarea
                    finalResult = htmlHelper.TextArea(name, string.IsNullOrEmpty(Convert.ToString(value)) ? string.Empty : value.ToString(), unobtrusiveValidationAttributes).ToString();
                    break;
                #endregion
                case ControlType.ImageUpload:
                    #region ImageUpload
                    finalResult = htmlHelper.FileFor(name, string.IsNullOrEmpty(Convert.ToString(value)) ? string.Empty : value.ToString(), settingConfigVM, unobtrusiveValidationAttributes).ToString();
                    break;
                    #endregion
                    #endregion
            }

            return new MvcHtmlString(finalResult);
        }
        public static MvcHtmlString FileFor<TModel>(this HtmlHelper<TModel> helper, string name, object value, SettingConfigVM settingConfigVM, IDictionary<string, object> htmlAttributes = null)
        {
            var Filebuilder = new TagBuilder("input");

            Filebuilder.GenerateId("file");
            Filebuilder.MergeAttribute("name", "file");
            Filebuilder.MergeAttribute("type", "file");
            Filebuilder.MergeAttribute("class", "fileinput btn btn-st1 upload_design none_push");
            Filebuilder.MergeAttribute("data-filename-placement", "inside");
            Filebuilder.MergeAttribute("accept", ".jpg,.jpeg,.png");
            Filebuilder.MergeAttribute("onchange", "validateFileType()");
            Filebuilder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            MvcHtmlString fileTag = MvcHtmlString.Create(Filebuilder.ToString(TagRenderMode.SelfClosing));

            var id = name;
            var hdnbuilder = new TagBuilder("input");
            hdnbuilder.GenerateId(id);
            hdnbuilder.MergeAttribute("name", id);
            hdnbuilder.MergeAttribute("type", "hidden");
            hdnbuilder.MergeAttribute("class", "hdnBase64File");
            hdnbuilder.MergeAttribute("value", Convert.ToString(value));
            MvcHtmlString hdnTag = MvcHtmlString.Create(hdnbuilder.ToString(TagRenderMode.SelfClosing));


            var imgBuilder = new TagBuilder("img");
            imgBuilder.GenerateId("img");
            imgBuilder.MergeAttribute("name", "img");
            imgBuilder.MergeAttribute("src", "data:image/png;base64," + Convert.ToString(value));
            if (settingConfigVM != null && settingConfigVM.LogoHeight != null && settingConfigVM.LogoWidth != null)
            {
                imgBuilder.MergeAttribute("Height", settingConfigVM.LogoHeight);
                imgBuilder.MergeAttribute("Width", settingConfigVM.LogoWidth);
            }
            MvcHtmlString imgTag = MvcHtmlString.Create(imgBuilder.ToString(TagRenderMode.SelfClosing));

            StringBuilder sb = new StringBuilder();
            sb.Append(fileTag);
            sb.Append(hdnbuilder);
            sb.Append(imgTag);
            // Render tags
            return MvcHtmlString.Create(sb.ToString());
        }
        public static MvcHtmlString NumericTextBoxFor<TModel>(this HtmlHelper<TModel> helper, string name, object value, IDictionary<string, object> htmlAttributes = null)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(helper.TextBox(name, value, htmlAttributes));
            return MvcHtmlString.Create(sb.ToString());
        }
        public static MvcHtmlString CustomRadioButtonFor<TModel>(this HtmlHelper<TModel> helper, string name, object value, string label, IDictionary<string, object> htmlAttributes = null)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<label class='col-md-5'>");
            sb.Append(helper.RadioButton(name, value, (bool)value, htmlAttributes)).Append($"<span>{label}</span>");
            sb.Append("</label>");
            return MvcHtmlString.Create(sb.ToString());
        }
        public static IDictionary<string, object> MergeHtmlAttributes(IDictionary<string, object> htmlAttributes1, IDictionary<string, object> htmlAttributes2)
        {
            if (htmlAttributes1 == null)
            {
                return htmlAttributes2;
            }
            else if (htmlAttributes2 == null)
            {
                return htmlAttributes1;
            }

            IDictionary<string, object> dictionary1 = new RouteValueDictionary(htmlAttributes1);
            IDictionary<string, object> dictionary2 = new RouteValueDictionary(htmlAttributes2);
            IDictionary<string, object> result = new Dictionary<string, object>();

            foreach (var pair in dictionary1.Concat(dictionary2))
            {
                if (!result.ContainsKey(pair.Key))
                {
                    result.Add(pair);
                }
                else if (result.ContainsKey(pair.Key) && pair.Key.ToLower() == "class")
                {
                    result[pair.Key] = result[pair.Key] + " " + pair.Value;//merge the classes of css
                }
            }
            return result;
        }
    }
}