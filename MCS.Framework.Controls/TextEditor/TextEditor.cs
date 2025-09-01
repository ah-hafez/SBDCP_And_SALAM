using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Handlers;
using System.Web.Optimization;

#region [ Resources ]

[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.TextEditor.JS.tinymce.langs.ar.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.advlist.plugin.min.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.anchor.plugin.min.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.autolink.plugin.min.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.autoresize.plugin.min.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.autosave.plugin.min.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.bbcode.plugin.min.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.charmap.plugin.min.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.code.plugin.min.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.colorpicker.plugin.min.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.contextmenu.plugin.min.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.directionality.plugin.min.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.emoticons.img.smiley-cool.gif", "img/gif")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.emoticons.img.smiley-cry.gif", "img/gif")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.emoticons.img.smiley-embarassed.gif", "img/gif")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.emoticons.img.smiley-foot-in-mouth.gif", "img/gif")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.emoticons.img.smiley-frown.gif", "img/gif")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.emoticons.img.smiley-innocent.gif", "img/gif")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.emoticons.img.smiley-kiss.gif", "img/gif")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.emoticons.img.smiley-laughing.gif", "img/gif")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.emoticons.img.smiley-money-mouth.gif", "img/gif")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.emoticons.img.smiley-sealed.gif", "img/gif")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.emoticons.img.smiley-smile.gif", "img/gif")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.emoticons.img.smiley-surprised.gif", "img/gif")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.emoticons.img.smiley-tongue-out.gif", "img/gif")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.emoticons.img.smiley-undecided.gif", "img/gif")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.emoticons.img.smiley-wink.gif", "img/gif")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.emoticons.img.smiley-yell.gif", "img/gif")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.emoticons.plugin.min.js", "text/javascript", PerformSubstitution = true)]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.example.dialog.html", "text/html")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.example.plugin.min.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.example_dependency.plugin.min.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.fullpage.plugin.min.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.fullscreen.plugin.min.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.hr.plugin.min.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.image.plugin.min.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.importcss.plugin.min.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.insertdatetime.plugin.min.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.layer.plugin.min.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.legacyoutput.plugin.min.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.link.plugin.min.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.lists.plugin.min.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.media.moxieplayer.swf", "application/x-shockwave-flash")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.media.plugin.min.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.nonbreaking.plugin.min.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.noneditable.plugin.min.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.pagebreak.plugin.min.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.paste.plugin.min.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.preview.plugin.min.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.print.plugin.min.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.save.plugin.min.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.searchreplace.plugin.min.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.spellchecker.plugin.min.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.tabfocus.plugin.min.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.table.plugin.min.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.template.plugin.min.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.textcolor.plugin.min.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.textpattern.plugin.min.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.visualblocks.css.visualblocks.css", "text/css")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.visualblocks.plugin.min.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.visualchars.plugin.min.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.wordcount.plugin.min.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.TextEditor.JS.tinymce.skins.lightgray.fonts.tinymce-small.eot", "application/vnd.ms-fontobject")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.TextEditor.JS.tinymce.skins.lightgray.fonts.stinymce-small.svg", "img/svg")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.TextEditor.JS.tinymce.skins.lightgray.fonts.tinymce-small.ttf", "application/x-font-ttf")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.TextEditor.JS.tinymce.skins.lightgray.fonts.tinymce-small.woff", "application/font-woff")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.TextEditor.JS.tinymce.skins.lightgray.fonts.tinymce.eot", "application/vnd.ms-fontobject")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.TextEditor.JS.tinymce.skins.lightgray.fonts.tinymce.svg", "img/svg")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.TextEditor.JS.tinymce.skins.lightgray.fonts.tinymce.ttf", "application/x-font-ttf")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.TextEditor.JS.tinymce.skins.lightgray.fonts.tinymce.woff", "application/font-woff")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.TextEditor.JS.tinymce.skins.lightgray.img.anchor.gif", "img/gif")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.TextEditor.JS.tinymce.skins.lightgray.img.loader.gif", "img/gif")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.TextEditor.JS.tinymce.skins.lightgray.img.object.gif", "img/gif")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.TextEditor.JS.tinymce.skins.lightgray.img.trans.gif", "img/gif")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.TextEditor.JS.tinymce.skins.lightgray.img.pencil.png", "img/png")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.TextEditor.JS.tinymce.skins.lightgray.content.inline.min.css", "text/css", PerformSubstitution = true)]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.TextEditor.JS.tinymce.skins.lightgray.content.min.css", "text/css", PerformSubstitution = true)]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.TextEditor.JS.tinymce.skins.lightgray.skin.ie7.min.css", "text/css", PerformSubstitution = true)]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.TextEditor.JS.tinymce.skins.lightgray.skin.min.css", "text/css", PerformSubstitution = true)]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.TextEditor.JS.tinymce.themes.modern.theme.min.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.TextEditor.JS.tinymce.license.txt", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.TextEditor.JS.tinymce.tinymce.min.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("MCS.Framework.Controls.TextEditor.JS.TextEditorScript.js", "text/javascript")]

#endregion [ Resources ]

namespace MCS.Framework.Controls
{
    public class TextEditor
    {
        public static string RenderTextEditor(string textControlId, string hdnIdToSaveContent, bool readOnly, string languageShortName, string content = "", 
            string javascriptFunName = "", string stampBase64Image = "", string signatureBase64Image = "", bool isContentEncoded = true, string BarCodeBase64Image = "", string WaterMarkBase64Image = "")
        {
            StringBuilder controlStream = new StringBuilder();

            if (languageShortName != "en")
            {
                string langsWebResourceUrl = WebResourceUrl("MCS.Framework.Controls.TextEditor.JS.tinymce.langs." + languageShortName + ".js");
                controlStream.AppendFormat("<script type='text/javascript'>var langsWebResourceUrl='{0}';</script>", langsWebResourceUrl);
            }

            controlStream.AppendFormat("<textarea id='{0}'></textarea>", textControlId);

            if (javascriptFunName != null && javascriptFunName != "")
            {
                controlStream.AppendFormat("<script type='text/javascript'> RenderTextEditor('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', {6}, '{7}', '{8}', '{9}', '{10}'); </script>",
                textControlId, readOnly.ToString().ToLower(), languageShortName, stampBase64Image, signatureBase64Image, content, javascriptFunName, isContentEncoded.ToString().ToLower(), hdnIdToSaveContent, BarCodeBase64Image, WaterMarkBase64Image);
            }
            else
            {
                javascriptFunName="";
                controlStream.AppendFormat("<script type='text/javascript'> RenderTextEditor('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}'); </script>",
                textControlId, readOnly.ToString().ToLower(), languageShortName, stampBase64Image, signatureBase64Image, content, javascriptFunName, isContentEncoded.ToString().ToLower(), hdnIdToSaveContent, BarCodeBase64Image, WaterMarkBase64Image);
            }

            return controlStream.ToString();
        }

        public static string RenderTextEditorResources()
        {
            string jsWebResourceUrl = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.TextEditor.JS.tinymce.tinymce.min.js", typeof(TextEditor));
            string jsTextEditorScriptUrl = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.TextEditor.JS.TextEditorScript.js", typeof(TextEditor));
            string themesWebResourceUrl = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.TextEditor.JS.tinymce.themes.modern.theme.min.js", typeof(TextEditor));

            string advlistWebResourceUrl = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.advlist.plugin.min.js", typeof(TextEditor));
            string autolinkWebResourceUrl = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.autolink.plugin.min.js", typeof(TextEditor));
            string listWebResourceUrl = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.lists.plugin.min.js", typeof(TextEditor));
            string linkWebResourceUrl = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.link.plugin.min.js", typeof(TextEditor));
            string charmapWebResourceUrl = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.charmap.plugin.min.js", typeof(TextEditor));
            string printWebResourceUrl = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.print.plugin.min.js", typeof(TextEditor));
            string previewWebResourceUrl = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.preview.plugin.min.js", typeof(TextEditor));
            string anchorWebResourceUrl = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.anchor.plugin.min.js", typeof(TextEditor));
            string searchreplaceWebResourceUrl = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.searchreplace.plugin.min.js", typeof(TextEditor));
            string visualblocksWebResourceUrl = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.visualblocks.plugin.min.js", typeof(TextEditor));
            string fullscreenWebResourceUrl = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.fullscreen.plugin.min.js", typeof(TextEditor));
            string insertdatetimeWebResourceUrl = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.insertdatetime.plugin.min.js", typeof(TextEditor));
            string tableWebResourceUrl = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.table.plugin.min.js", typeof(TextEditor));
            string pasteWebResourceUrl = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.paste.plugin.min.js", typeof(TextEditor));
            string emoticonsWebResourceUrl = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.emoticons.plugin.min.js", typeof(TextEditor));
            string autoresizeWebResourceUrl = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.autoresize.plugin.min.js", typeof(TextEditor));
            string autosaveWebResourceUrl = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.autosave.plugin.min.js", typeof(TextEditor));
            string bbcodeWebResourceUrl = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.bbcode.plugin.min.js", typeof(TextEditor));
            string codeWebResourceUrl = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.code.plugin.min.js", typeof(TextEditor));
            string colorpickerWebResourceUrl = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.colorpicker.plugin.min.js", typeof(TextEditor));
            string contextmenutWebResourceUrl = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.contextmenu.plugin.min.js", typeof(TextEditor));
            string directionalityWebResourceUrl = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.directionality.plugin.min.js", typeof(TextEditor));
            string example_dependencyWebResourceUrl = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.example_dependency.plugin.min.js", typeof(TextEditor));
            string fullpageWebResourceUrl = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.fullpage.plugin.min.js", typeof(TextEditor));
            string hrWebResourceUrl = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.hr.plugin.min.js", typeof(TextEditor));
            string imageWebResourceUrl = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.image.plugin.min.js", typeof(TextEditor));
            string importcssWebResourceUrl = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.importcss.plugin.min.js", typeof(TextEditor));
            string layerWebResourceUrl = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.layer.plugin.min.js", typeof(TextEditor));
            string legacyoutputtWebResourceUrl = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.legacyoutput.plugin.min.js", typeof(TextEditor));
            string nonbreakingWebResourceUrl = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.nonbreaking.plugin.min.js", typeof(TextEditor));
            string noneditableWebResourceUrl = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.noneditable.plugin.min.js", typeof(TextEditor));
            string pagebreakWebResourceUrl = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.pagebreak.plugin.min.js", typeof(TextEditor));
            string saveWebResourceUrl = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.save.plugin.min.js", typeof(TextEditor));
            string spellcheckerWebResourceUrl = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.spellchecker.plugin.min.js", typeof(TextEditor));
            string tabfocusWebResourceUrl = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.tabfocus.plugin.min.js", typeof(TextEditor));
            string templateWebResourceUrl = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.template.plugin.min.js", typeof(TextEditor));
            string textcolorWebResourceUrl = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.textcolor.plugin.min.js", typeof(TextEditor));
            string textpatternWebResourceUrl = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.textpattern.plugin.min.js", typeof(TextEditor));
            string visualcharsWebResourceUrl = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.visualchars.plugin.min.js", typeof(TextEditor));
            string wordcountWebResourceUrl = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.wordcount.plugin.min.js", typeof(TextEditor));
            string exampleWebResourceUrl = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.example.plugin.min.js", typeof(TextEditor));
            string exampleHtmlWebResourceUrl = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.example.dialog.html", typeof(TextEditor));
            string mediaWebResourceUrl = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.media.plugin.min.js", typeof(TextEditor));

            string imgcoolWebResourceUrl = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.emoticons.img.smiley-cool.gif", typeof(TextEditor));
            string imgcryWebResourceUrl = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.emoticons.img.smiley-cry.gif", typeof(TextEditor));
            string imgembarassedWebResourceUrl = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.emoticons.img.smiley-embarassed.gif", typeof(TextEditor));
            string imgfootinmouthWebResourceUrl = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.emoticons.img.smiley-foot-in-mouth.gif", typeof(TextEditor));
            string imgfrownWebResourceUrl = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.emoticons.img.smiley-frown.gif", typeof(TextEditor));
            string imgkissWebResourceUrl = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.emoticons.img.smiley-kiss.gif", typeof(TextEditor));
            string imglaughingWebResourceUrl = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.emoticons.img.smiley-laughing.gif", typeof(TextEditor));
            string imgmoneymouthWebResourceUrl = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.emoticons.img.smiley-money-mouth.gif", typeof(TextEditor));
            string imgsealedWebResourceUrl = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.emoticons.img.smiley-sealed.gif", typeof(TextEditor));
            string imgsmileWebResourceUrl = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.emoticons.img.smiley-smile.gif", typeof(TextEditor));
            string imgsurprisedWebResourceUrl = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.emoticons.img.smiley-surprised.gif", typeof(TextEditor));
            string imgundecidedWebResourceUrl = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.emoticons.img.smiley-undecided.gif", typeof(TextEditor));
            string imgwinkWebResourceUrl = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.emoticons.img.smiley-wink.gif", typeof(TextEditor));
            string imgyellWebResourceUrl = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.emoticons.img.smiley-yell.gif", typeof(TextEditor));
            string imgtongueoutWebResourceUrl = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.emoticons.img.smiley-tongue-out.gif", typeof(TextEditor));
            string imginnocentWebResourceUrl = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.emoticons.img.smiley-innocent.gif", typeof(TextEditor));

            string skinsLightgrayContentInlineMincssWebResourceUrl = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.TextEditor.JS.tinymce.skins.lightgray.content.inline.min.css", typeof(TextEditor));
            string skinsLightgrayContentMincssWebResourceUrl = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.TextEditor.JS.tinymce.skins.lightgray.content.min.css", typeof(TextEditor));
            string skinsLightgraySkinie7MincssWebResourceUrl = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.TextEditor.JS.tinymce.skins.lightgray.skin.ie7.min.css", typeof(TextEditor));
            string skinsLightgraySkinMincssWebResourceUrl = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.TextEditor.JS.tinymce.skins.lightgray.skin.min.css", typeof(TextEditor));
            string cssVisualblocksWebResourceUrl = EmbeddedResourcesHelper.WebResourceUrl("MCS.Framework.Controls.TextEditor.JS.tinymce.plugins.visualblocks.css.visualblocks.css", typeof(TextEditor));

            StringBuilder controlStream = new StringBuilder();

            controlStream.AppendFormat("<script type='text/javascript'> var advlistWebResourceUrl='{0}'; var autolinkWebResourceUrl='{1}'; var listWebResourceUrl='{2}'; var linkWebResourceUrl='{3}'; var charmapWebResourceUrl='{4}'; var printWebResourceUrl='{5}'; var previewWebResourceUrl='{6}'; var anchorWebResourceUrl='{7}'; var themesWebResourceUrl='{8}'; var searchreplaceWebResourceUrl='{9}'; var visualblocksWebResourceUrl='{10}'; var fullscreenWebResourceUrl='{11}'; var insertdatetimeWebResourceUrl='{12}'; var tableWebResourceUrl='{13}'; var pasteWebResourceUrl='{14}'; var skinsLightgrayContentInlineMincssWebResourceUrl='{15}'; var skinsLightgrayContentMincssWebResourceUrl='{16}'; var skinsLightgraySkinie7MincssWebResourceUrl='{17}'; var skinsLightgraySkinMincssWebResourceUrl='{18}'; var cssVisualblocksWebResourceUrl='{19}'; var emoticonsWebResourceUrl='{20}'; var imgcoolWebResourceUrl='{21}'; var imgembarassedWebResourceUrl='{22}'; var imgfootinmouthWebResourceUrl='{23}'; var imgfrownWebResourceUrl='{24}'; var imgkissWebResourceUrl='{25}'; var imglaughingWebResourceUrl='{26}'; var imgmoneymouthWebResourceUrl='{27}'; var imgsealedWebResourceUrl='{28}'; var imgsmileWebResourceUrl='{29}'; var imgsurprisedWebResourceUrl='{30}'; var imgundecidedWebResourceUrl='{31}'; var imgwinkWebResourceUrl='{32}'; var imgyellWebResourceUrl='{33}'; var imgtongueoutWebResourceUrl='{34}'; var imginnocentWebResourceUrl='{35}'; var imgcryWebResourceUrl='{36}';</script>", advlistWebResourceUrl, autolinkWebResourceUrl, tableWebResourceUrl, linkWebResourceUrl, charmapWebResourceUrl, printWebResourceUrl, previewWebResourceUrl, anchorWebResourceUrl, themesWebResourceUrl, searchreplaceWebResourceUrl, visualblocksWebResourceUrl, fullscreenWebResourceUrl, insertdatetimeWebResourceUrl, tableWebResourceUrl, pasteWebResourceUrl, skinsLightgrayContentInlineMincssWebResourceUrl, skinsLightgrayContentMincssWebResourceUrl, skinsLightgraySkinie7MincssWebResourceUrl, skinsLightgraySkinMincssWebResourceUrl, cssVisualblocksWebResourceUrl, emoticonsWebResourceUrl, imgcoolWebResourceUrl, imgembarassedWebResourceUrl, imgfootinmouthWebResourceUrl, imgfrownWebResourceUrl, imgkissWebResourceUrl, imglaughingWebResourceUrl, imgmoneymouthWebResourceUrl, imgsealedWebResourceUrl, imgsmileWebResourceUrl, imgsurprisedWebResourceUrl, imgundecidedWebResourceUrl, imgwinkWebResourceUrl, imgyellWebResourceUrl, imgtongueoutWebResourceUrl, imginnocentWebResourceUrl, imgcryWebResourceUrl);
            controlStream.AppendFormat("<script type='text/javascript'> var autoresizeWebResourceUrl='{0}'; var autosaveWebResourceUrl='{1}'; var bbcodeWebResourceUrl='{2}'; var codeWebResourceUrl='{3}'; var colorpickerWebResourceUrl='{4}'; var contextmenutWebResourceUrl='{5}'; var directionalityWebResourceUrl='{6}'; var example_dependencyWebResourceUrl='{7}'; var fullpageWebResourceUrl='{8}'; var hrWebResourceUrl='{9}'; var imageWebResourceUrl='{10}'; var importcssWebResourceUrl='{11}'; var layerWebResourceUrl='{12}'; var legacyoutputtWebResourceUrl='{13}'; var nonbreakingWebResourceUrl='{14}'; var noneditableWebResourceUrl='{15}'; var pagebreakWebResourceUrl='{16}'; var saveWebResourceUrl='{17}'; var spellcheckerWebResourceUrl='{18}'; var tabfocusWebResourceUrl='{19}'; var templateWebResourceUrl='{20}'; var textcolorWebResourceUrl='{21}'; var textpatternWebResourceUrl='{22}'; var visualcharsWebResourceUrl='{23}'; var wordcountWebResourceUrl='{24}'; var exampleWebResourceUrl='{25}'; var mediaWebResourceUrl='{26}'; var exampleHtmlWebResourceUrl='{27}'; </script>", autoresizeWebResourceUrl, autosaveWebResourceUrl, bbcodeWebResourceUrl, codeWebResourceUrl, colorpickerWebResourceUrl, contextmenutWebResourceUrl, directionalityWebResourceUrl, example_dependencyWebResourceUrl, fullpageWebResourceUrl, hrWebResourceUrl, imageWebResourceUrl, importcssWebResourceUrl, layerWebResourceUrl, legacyoutputtWebResourceUrl, nonbreakingWebResourceUrl, noneditableWebResourceUrl, pagebreakWebResourceUrl, saveWebResourceUrl, spellcheckerWebResourceUrl, tabfocusWebResourceUrl, templateWebResourceUrl, textcolorWebResourceUrl, textpatternWebResourceUrl, visualcharsWebResourceUrl, wordcountWebResourceUrl, exampleWebResourceUrl, mediaWebResourceUrl, exampleHtmlWebResourceUrl);

            string js = Scripts.Render("~/MCS.Framework.Controls/JSTextEditor").ToString();

            return controlStream.ToString() + js;
        }

        private static string WebResourceUrl(string resourceName)
        {
            string resourceUrl = string.Empty;

            List<MemberInfo> methodCandidates =
                typeof(AssemblyResourceLoader).GetMember("GetWebResourceUrlInternal",
                BindingFlags.NonPublic | BindingFlags.Static).ToList();

            foreach (var methodCandidate in methodCandidates)
            {
                var method = methodCandidate as MethodInfo;

                if (method == null || method.GetParameters().Length != 5)
                    continue;

                resourceUrl = string.Format("{0}", method.Invoke
                (
                    null,
                    new object[] { Assembly.GetAssembly(typeof(TextEditor)), resourceName, false, false, null })
                );
                break;
            }
            return HttpContext.Current.Request.Url.Host + resourceUrl;
        }       
    }
}
