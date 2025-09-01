using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using MCS.Framework.Controls.Mvc;
using MCS.Framework.Localization;
using MCS.Common;
using MCS.Common.CustomAttributes;

namespace MCS.UI.TenantsAdmin
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




        public static MvcHtmlString AutoCompleteBuilder(this HtmlHelper html, string autoCompleteControlid,
           string hdnIdToSaveValue, string items, string selectedId = "", bool matchAnywhere = true,
           string inputClassName = "", string hdnExtraParametersId = "", string ulClassName = "", string buttonId = "", bool selectFirstIndex = false, string validationGroup = "")
        {
            string inputClass = String.Concat(inputClassName, " fx_control");
            string validationClass = "input-validation-error";

            html.ViewContext.Controller.ViewBag.validationGroup = validationGroup;

            return AutoCompleteExtensions.AutoComplete(html, autoCompleteControlid, hdnIdToSaveValue, items, matchAnywhere, selectedId, inputClassName, ulClassName, buttonId, hdnExtraParametersId, selectFirstIndex, validationClass);
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
                var Attributes = helper.MergeHtmlAttributes(htmlAttributes, new { onkeypress = "return IsNumeric(event);" });
                sb.Append(helper.TextBoxFor(expression, Attributes));
            }

            sb.Append(helper.ValidationMessageFor(expression));

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
                        sb.Append("<div class='col-md-1'>");
                        sb.Append("<a class='node' >");
                        sb.Append("<span id='span_" + node.Id + "' class='fa fa-plus'></span>");
                        sb.Append("</a>");
                        sb.Append("</div>");
                        sb.Append("<div data-value='" + node.Id + "' class='col-md-8  col_text' id='" + key + "'>" + node.Name + "</div>");
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
                        sb.Append("<div class='col-md-1'>");
                        sb.Append("</div>");
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
            sb.Append(helper.LabelFor(expression, new { @class = "col-md-2 control-label" }));
            sb.Append("<div class='col-md-10'>");
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


            sb.Append("<div id='divAcc__" + container + "' class='accordion'>");
            sb.Append("<div class='panel-body panel-style' id='divContainer__" + container + "'>");
            sb.Append("<div id='divDir__" + container + "' class='directory'>");
            sb.Append("<div class='row'>");
            sb.Append("<div class='form-group'>");
            sb.Append("<label class='LredColor col-md-2 control-label'>" + searchText + "</label>");
            sb.Append("<div class='col-md-10'>");
            sb.Append("<div><input class='form-control txtPermissionSearch' data-container='" + container + "' maxlength='40'  type='text'></div>");
            sb.Append("</div></div>");
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
                        sb.Append("<li data-li-id='" + node.Id + "' id='li_" + node.Id + "'>");
                        sb.Append("<div class='row row_directory " + selectableClass + " " + isSelectedClass + " selected_bottom' id='divspan_" + node.Id + container + "'>");
                        sb.Append("<div class='col-md-1'>");
                        sb.Append("<a class='node' >");
                        sb.Append("<span id='span_" + node.Id + container + "' class='fa fa-plus'></span>");
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
                        sb.Append("<li id='li_" + node.Id + "'>");
                        sb.Append("<div class='row row_directory last " + selectableClass + " " + isSelectedClass + "'>");
                        sb.Append("<div class='col-md-1'>");
                        sb.Append("</div>");
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
            sb.Append(helper.LabelFor(expression, new { @class = "col-md-2 control-label" }));
            sb.Append("<div class='col-md-10'>");
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


            sb.Append("<div id='divAcc__" + container + "' class='accordion'>");
            sb.Append("<div class='panel-body panel-style' id='divContainer__" + container + "'>");
            sb.Append("<div id='divDir__" + container + "' class='directory'>");
            sb.Append("<div class='row'>");
            sb.Append("<div class='form-group'>");
            sb.Append("<label class='LredColor col-md-2 control-label'>" + searchText + "</label>");
            sb.Append("<div class='col-md-10'>");
            sb.Append("<div><input class='form-control txtPermissionSearch' data-container='" + container + "' maxlength='40'  type='text'></div>");
            sb.Append("</div></div>");
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
                        sb.Append("<span id='span_" + node.Id + container + "' class='fa fa-plus'></span>");
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
                        sb.Append("<div class='col-md-1'>");
                        sb.Append("</div>");
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


        public static MvcHtmlString DepartmentsTree<TModel, TProperty>(this HtmlHelper<TModel> helper, TreeViewModel treeViewModel, Expression<Func<TModel, TProperty>> expression, TreeMode treeMode, string treeId, bool isAjaxCall, string onClickfunction = "", string validationGroup = "", string divContainerId = "")
        {
            if (treeMode == TreeMode.Multiple)
            {
                return HtmlHelperExt.DepartmentsMultipleTree(helper, treeViewModel, expression, treeMode, treeId, isAjaxCall, onClickfunction, validationGroup);
            }
            else
            {
                return HtmlHelperExt.DepartmentsSingleTree(helper, treeViewModel, expression, treeMode, treeId, isAjaxCall, onClickfunction, validationGroup, divContainerId);
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

            sb.Append("<div class='form-group' id='divForm__" + treeId + "'>");
            sb.Append(helper.LabelFor(expression, new { @class = "col-md-4 control-label" }));
            sb.Append("<div class='col-md-8'>");
            sb.Append("<div class='row'>");
            sb.Append("<div class='col-md-4 none_left'>");
            sb.Append("<input type='text' data-func='" + onClickfunction + "' data-container='" + treeId + "' onkeypress ='return IsNumeric(event);' class='form-control txtDepartmentNumber' maxlength='20' />");
            sb.Append("</div>");
            sb.Append("<div class='col-md-8 none_right'>");
            sb.Append("<div class='input-group'>");
            sb.Append("<input type='text' data-func='" + onClickfunction + "' data-container='" + treeId + "' class='form-control txtDepartmentName' maxlength='40' />");
            sb.Append("<span class='input-group-addon link'>");
            sb.Append("<div class='panel-title'>");
            sb.Append("<a onclick=ShowHideTree('#divContainer__" + treeId + "') title=" + DbRes.TResource("Global.Tree.Show") + "><span class='fa fa-sort-amount-desc'> </span> </a>");
            sb.Append("</div></span>");
            //sb.Append(helper.HiddenFor(expression, new { @class = "hdnDepartmentId", @Value = "" }));
            //sb.Append("<input type='hidden'  data-container='" + treeId + "' name='" + propertyName + "' class='hdnDepartmentId'/>");
            sb.Append("</div></div>");
            sb.Append(helper.HiddenGroupFor(expression, attrs, validationGroup));
            sb.Append("</div></div></div>");

            sb.Append("<div class='accordion' id='divAcc__" + treeId + "'>");
            sb.Append("<div class='panel-body panel-style' style='display: none;' id='divContainer__" + treeId + "' data-func='" + onClickfunction + "' data-Name='" + propertyName + "'>");
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

            //helper.ViewContext.Controller.ViewBag.validationGroup = validationGroup;

            return MvcHtmlString.Create(sb.ToString());
        }

        public static MvcHtmlString DepartmentsTreeOnDemand<TModel, TProperty>(this HtmlHelper<TModel> helper, TreeViewModel treeViewModel, Expression<Func<TModel, TProperty>> expression, TreeMode treeMode, string treeId, bool isAjaxCall, string onClickfunction = "", string validationGroup = "", string divContainerId = "", bool isRefreshEnabled = false)
        {
            string propertyName = ExpressionHelper.GetExpressionText(expression);
            StringBuilder sb = new StringBuilder();
            var attrs = new Dictionary<string, object>();
            attrs.Add("class", "hdnDepartmentId");
            attrs.Add("data-container", treeId);
            attrs.Add("data-func", onClickfunction);

            sb.Append("<div class='form-group' id='divForm__" + treeId + "'>");
            sb.Append(helper.LabelFor(expression, new { @class = "col-md-4 control-label" }));
            sb.Append("<div class='col-md-8'>");
            sb.Append("<div class='row'>");
            sb.Append("<div class='col-md-4 none_left'>");
            sb.Append("<input type='text' data-func='" + onClickfunction + "' data-container='" + treeId + "' onkeypress ='return IsNumeric(event);' class='form-control txtDepartmentNumber' maxlength='20' />");
            sb.Append("</div>");
            sb.Append("<div class='col-md-8 none_right'>");
            sb.Append("<div class='input-group'>");
            sb.Append("<input type='text' data-func='" + onClickfunction + "' data-container='" + treeId + "' class='form-control txtDepartmentName' maxlength='40' />");
            sb.Append("<span class='input-group-addon link'>");
            sb.Append("<div class='panel-title'>");
            sb.Append("<a onclick=ShowHideTree('#divContainer__" + treeId + "') title=" + DbRes.TResource("Global.Tree.Show") + "><span class='fa fa-sort-amount-desc'> </span> </a>");
            sb.Append("</div></span>");
            sb.Append("</div></div>");
            sb.Append(helper.HiddenGroupFor(expression, attrs, validationGroup));
            sb.Append("</div></div></div>");

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
            sb.Append(helper.LabelFor(expression, new { @class = "col-md-2 control-label" }));
            sb.Append("<div class='col-md-10'>");
            sb.Append("<div class='input-group rtl_btn'>");
            sb.Append("<span class='tags'><div class='list-tags' id='divTags' data-name=" + propertyName + ">");
            sb.Append("</div></span>");
            sb.Append("<span class='input-group-addon link'>");
            sb.Append("<div class='panel-title'>");
            sb.Append("<a onclick=ShowHideTree('#divContainer__" + container + "') title=" + DbRes.TResource("Global.Tree.Show") + "><span class='fa fa-sort-amount-desc'> </span> </a>");
            sb.Append("</div></span>");
            sb.Append("</div></div></div></div>");
            //sb.Append(helper.ValidationMessageFor(expression));
            if (modelMetaData.IsRequired && !isAjaxCall)
            {
                sb.Append(helper.HiddenGroupFor(expression, new { @class = "__hdnDepartment" }, validationGroup));
            }

            sb.Append("<div class='accordion' id='divAcc__" + container + "'>");
            sb.Append("<div class='panel-body panel-style' id='divContainer__" + container + "'>");
            sb.Append("<div id='divDir__" + container + "' class='directory'>");
            sb.Append("<div class='row'>");
            sb.Append("<div class='form-group'>");
            sb.Append("<label class='LredColor col-md-2 control-label'>" + searchText + "</label>");
            sb.Append("<div class='col-md-10'>");
            sb.Append("<div><input class='form-control txtDepartmentSearch' data-container='" + container + "'  type='text' maxlength='40'></div>");
            sb.Append("</div></div>");
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

                        if (node.Childs.Count > 0)
                        {
                            sb.Append("<li id='li_" + node.Id + "'>");
                            sb.Append("<div class='row row_directory " + selectableClass + " " + isSelectedClass + " selected_bottom' id='divspan_" + node.Id + container + "'>");
                            sb.Append("<div class='col-md-1'>");
                            sb.Append("<a class='node'  >");
                            sb.Append("<span id='span_" + node.Id + container + "' class='fa fa-plus'></span>");
                            sb.Append("</a>");
                            sb.Append("</div>");
                            sb.Append("<input type='hidden' value='" + node.DepartmentNumber + "'  class='hdnDepartmentNumber __hdnSticky'/>");
                            sb.Append("<div data-container='" + container + "' data-Name='" + propertyName + "' data-value='" + node.Id + "' data-func='" + onClickfunction + "' class='col-md-8 " + selectMode + "' id='" + key + "'>" + node.Name + "</div>");
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
                            sb.Append("<li id='li_" + node.Id + "'>");
                            sb.Append("<div class='row row_directory last " + selectableClass + " " + isSelectedClass + "'>");
                            sb.Append("<div class='col-md-1'>");
                            sb.Append("</div>");
                            sb.Append("<input type='hidden' value='" + node.DepartmentNumber + "'  class='hdnDepartmentNumber __hdnSticky'/>");
                            sb.Append("<div data-container='" + container + "' data-Name='" + propertyName + "' data-value='" + node.Id + "' data-func='" + onClickfunction + "' class='col-md-8 " + selectMode + "' id='" + key + "'>" + node.Name + "</div>");
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
                            sb.Append("<div class='col-md-1' >");
                            sb.Append("<a class='node'  >");
                            sb.Append("<span id='span_" + node.Id + container + "' class='fa fa-plus LoadedData' data-isRefreshEnabled='" + isRefreshEnabled + "' data-node-id='" + node.Id + "'></span>");
                            sb.Append("</a>");
                            sb.Append("</div>");
                            sb.Append("<input type='hidden' value='" + node.DepartmentNumber + "'  class='hdnDepartmentNumber __hdnSticky'/>");
                            sb.Append("<div data-container='" + container + "' data-Name='" + propertyName + "' data-value='" + node.Id + "' data-func='" + onClickfunction + "' class='col-md-8 " + selectMode + "' id='" + key + "'>" + node.Name + "</div>");
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
                            sb.Append("<div class='col-md-1'>");
                            sb.Append("</div>");
                            sb.Append("<input type='hidden' value='" + node.DepartmentNumber + "'  class='hdnDepartmentNumber __hdnSticky'/>");
                            sb.Append("<div data-container='" + container + "' data-Name='" + propertyName + "' data-value='" + node.Id + "' data-func='" + onClickfunction + "' class='col-md-8 " + selectMode + "' id='" + key + "'>" + node.Name + "</div>");
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
                            sb.Append("<div class='col-md-1' >");
                            sb.Append("<a class='node'  >");
                            sb.Append("<span id='span_" + node.Id + container + "' class='fa fa-plus LoadedData'  data-isRefreshEnabled='" + isRefreshEnabled + "' data-node-id='" + node.Id + "'></span>");
                            sb.Append("</a>");
                            sb.Append("</div>");
                            sb.Append("<input type='hidden' value='" + node.DepartmentNumber + "'  class='hdnDepartmentNumber __hdnSticky'/>");
                            sb.Append("<div data-container='" + container + "' data-Name='" + propertyName + "' data-value='" + node.Id + "' data-func='" + onClickfunction + "' class='col-md-8 " + selectMode + "' id='" + key + "'>" + node.Name + "</div>");
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
                            sb.Append("<div class='col-md-1'>");
                            sb.Append("</div>");
                            sb.Append("<input type='hidden' value='" + node.DepartmentNumber + "'  class='hdnDepartmentNumber __hdnSticky'/>");
                            sb.Append("<div data-container='" + container + "' data-Name='" + propertyName + "' data-value='" + node.Id + "' data-func='" + onClickfunction + "' class='col-md-8 " + selectMode + "' id='" + key + "'>" + node.Name + "</div>");
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
                    if (node.Childs.Count > 0)
                    {
                        stringBuilder.Append("<li class='jstree-closed root node_Native " + isSelectedClass + "'  node='" + node.Id + "'>");

                        stringBuilder.Append(node.Name);

                        HtmlHelperExt.NativeTreeNode(node, ref stringBuilder);
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

        public static MvcHtmlString MenuFileLink(this HtmlHelper htmlHelper, string itemText, int transactionsCount, string actionName, string controllerName, string areaName, string permissionCode = "")
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

            linkBuilder.InnerHtml = itemText + "<i>" + transactionsCount + "</i>";
            if (controllerName == currentController && actionName == currentAction)
            {
                linkBuilder.AddCssClass("active_ok");
            }

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

            html = String.Format("<a><span>{0}</span><em class='user_name'> <div class='appear' onclick='ComfirmBindingChat({1})' data-online-username='{2}' data-online-userid='{3}'></div> {4} </em> </a>", userResource, transactionId, userName, userId, userName);

            if (userId == SessionInfo.CurrentUser.Id)
            {
                html = String.Format("<a><span>{0}</span> {1}</a>", userResource, userName);
            }

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

            return MvcHtmlString.Create(Framework.Controls.TextEditor.RenderTextEditor(textControlId, hdnIdToSaveContent, readOnly,
                languageShortName, content, javascriptFunName, stampBase64Image, signatureBase64Image, isContentEncoded));
        }
      
    }
}