using System.Text;


namespace MCS.UI.Controls
{
    public static class AutoComplete
    {
        public static string RenderAutoComplete(string autoCompleteControlid, string hdnIdToSaveValue, string items, bool matchAnywhere, string content = "", string inputClassName = "", string ulClassName = "", string buttonId = "", string hdnExtraParametersId = "", bool selectFirstIndex = false, string validationClass = "", string waterMarkText = "", int maxlengthText = 40, string onChangeCallback = "", string onKeyUpCallback = "")
        {
            StringBuilder controlStream = new StringBuilder();

            if (inputClassName != string.Empty)
            {
                controlStream.AppendFormat("<div class='AutPosition'>");
                if (onKeyUpCallback != string.Empty)
                {
                    controlStream.AppendFormat("<input id='{0}' type='text' maxlength='{3}' class='{1}'  placeholder='{2}' onkeyup='{4}'/>", autoCompleteControlid, inputClassName, waterMarkText, maxlengthText, onKeyUpCallback);
                }
                else
                {
                    controlStream.AppendFormat("<input id='{0}' type='text' maxlength='{3}' class='{1}'  placeholder='{2}' />", autoCompleteControlid, inputClassName, waterMarkText, maxlengthText);
                }
                controlStream.AppendFormat("<span class='ArrowCustom select2 - selection__arrow' role='presentation'><b role='presentation' style='display: none; '></b><i class='uicon icon_arrow_down'></i></span>");
                controlStream.AppendFormat("</div>");
            }
            else
            {
                controlStream.AppendFormat("<div class='AutPosition'>");
                if (onKeyUpCallback != string.Empty)
                {
                    controlStream.AppendFormat("<input id='{0}' maxlength='{2}' type='text' placeholder='{1}' onkeyup='{3}'/>", autoCompleteControlid, waterMarkText, maxlengthText, onKeyUpCallback);
                }
                else
                {
                    controlStream.AppendFormat("<input id='{0}' maxlength='{2}' type='text' placeholder='{1}' />", autoCompleteControlid, waterMarkText, maxlengthText);
                }
                controlStream.AppendFormat("<span class='ArrowCustom select2 - selection__arrow' role='presentation'><b role='presentation' style='display: none; '></b><i class='uicon icon_arrow_down'></i></span>");
                controlStream.AppendFormat("</div>");
            }

            controlStream.AppendFormat("<script type='text/javascript'> AutoComplete('{0}','{1}','{2}','{3}','{4}','{5}','{6}', '{7}','{8}','{9}'); </script>", autoCompleteControlid, hdnIdToSaveValue, items, content, matchAnywhere.ToString().ToLower(), hdnExtraParametersId, selectFirstIndex.ToString().ToLower(), validationClass, onChangeCallback, onKeyUpCallback);

            return controlStream.ToString();
        }

        public static string RenderResetAutoComplete(string autoCompleteControlid)
        {
            StringBuilder controlStream = new StringBuilder();

            return "";
        }

        public static string AutoCompleteChangeList(string autoCompleteControlid, string newList)
        {
            StringBuilder controlStream = new StringBuilder();

            controlStream.AppendFormat("<script type='text/javascript'> AutoCompleteChangeList('{0}','{1}'); </script>", autoCompleteControlid, newList);

            return controlStream.ToString();
        }
    }
}