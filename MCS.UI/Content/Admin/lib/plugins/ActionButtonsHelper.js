/// <reference path="Common.js" />

function ConfirmAction(actionUrl, divUpdateContent, paramObj, contentType, onAjaxSuccess, onAjaxFaild, confirmMsg, actionType, formId, buttonId, confirmResource, cancelResource, clientFunctionToCall, notValidFunctionCall)
{
    debugger;
    var continueWork = true;

    if (clientFunctionToCall != '' && clientFunctionToCall != null && window[clientFunctionToCall] !== 'undefined')
        continueWork = window[clientFunctionToCall]();

    if (!continueWork)
    {
        return false;
    }

    ClickedSubmit = $("#" + buttonId).attr("name");

    if (!$('#' + formId).valid())
    {

        if (notValidFunctionCall != "" && notValidFunctionCall != null)
            window[notValidFunctionCall]();

        return false;
    }

    $('#' + formId).append('<input type="hidden" id="__validationGroup" value=' + ClickedSubmit + ' name="__validationGroup">');
    //actionUrl = actionUrl.replace(".", "\\");

    if ($('div.grid-mvc table').attr('id') != null)
    {
        //eval("pageGrids." + $('div.grid-mvc table').attr('id') + ".clearGridFiltersAndSorting()");
    }
    jQuery.support.cors = true;

    $.confirm({
        title: '<div class="modal-header"></div>',
        content: '<p>' + confirmMsg + '</p>',
        template: '<div class="jconfirm"><div class="jconfirm-bg"></div><div class="modal-dialog"><div class="modal-content"><div class="modal-body" style="padding:15px"> <h4 class="site-color title4 text-center"><div class="content" style="margin:0px"></div></h2></div><div class="modal-footer actions-buttons buttons"></div></div></div>',
        //template: '<div class="jconfirm-scrollpane"><div class="container"><div class="row"><div class="jconfirm-box-container span6 offset3"><div class="jconfirm-box"><div class="closeIcon"><span class="glyphicon glyphicon-remove"></span></div><div class="title"></div><div class="modal-body"><h4 class="site-color title4 text-center"><div class="content"></div></h2></div><div class="modal-footer actions-buttons"><div class="buttons"></div></div><div class="jquery-clear"></div></div></div></div></div></div></div>',

        cancelButton: __dialogCancelText,
        cancelButtonClass: 'btn-site',
        confirmButton: __dialogYesText,
        confirmButtonClass: 'btn-site confirmed-btn',

        animation: 'scale',
        columnClass: 'alert_normal alert_new col-md-6 col-md-offset-3 col-xs-6 col-xs-offset-3',
        backgroundDismiss: false,
        closeIcon: false,
        confirm: function ()
        {
            debugger;
            $("#Loader").show();
            event.preventDefault();
            event.stopPropagation();
            var data = $('#' + formId).serializeArray();

            if (paramObj != "" && paramObj != null)
            {
                data.push({ name: 'param', value: eval(paramObj) });
            }

            $.ajax({
                type: actionType,
                url: actionUrl,
                cache: false,
                //contentType: 'application/json; charset=utf-8',
                data: data,//paramObj,//"{id:1}",
                success:
                    function (data)
                    {
                        if (typeof data.errorOccurred != "undefined")
                        {
                            if (data.errorOccurred == true && data.url != null)
                            {
                                window.location.href = data.url;
                                return;
                            }
                        }
                        $('#' + formId).find("input[name=__validationGroup]:hidden").remove();

                        if (onAjaxSuccess != '')
                        {
                            window[onAjaxSuccess](data, divUpdateContent);
                        }
                    },
                error: function (data)
                {

                    if (onAjaxFaild != '')
                    {
                        window[onAjaxFaild](data, divUpdateContent);
                    }
                },

            });
        },
        cancel: function ()
        {
            $("#Loader").hide();
        }
    });
    return false;
}

function ConfirmExplanationAction(actionUrl, divUpdateContent, paramObj, contentType, onAjaxSuccess, onAjaxFaild, confirmMsg, actionType, formId, buttonId, confirmResource, cancelResource, clientFunctionToCall, notValidFunctionCall)
{
    var continueWork = true;

    if (clientFunctionToCall != '' && clientFunctionToCall != null)
        continueWork = window[clientFunctionToCall]();

    if (!continueWork)
    {
        return false;
    }

    var docoNutFrameContent = $('#' + paramObj).contents();
    var docoNutToken = $('#_hdnToken', docoNutFrameContent)[0].value;

    ClickedSubmit = $("#" + buttonId).attr("name");

    if (!$('#' + formId).valid())
    {

        if (notValidFunctionCall != "" && notValidFunctionCall != null)
            window[notValidFunctionCall]();

        return false;
    }

    $('#' + formId).append('<input type="hidden" id="__validationGroup" value=' + ClickedSubmit + ' name="__validationGroup">');

    //if ($('div.grid-mvc table').attr('id') != null) {
    //    eval("pageGrids." + $('div.grid-mvc table').attr('id') + ".clearGridFiltersAndSorting()");
    //}

    jQuery.support.cors = true;

    $.confirm({
        title: '<div class="modal-header"></div>',
        content: '<p>' + confirmMsg + '</p>',
        template: '<div class="jconfirm"><div class="jconfirm-bg"></div><div class="modal-dialog"><div class="modal-content" style="margin:0px"><div class="modal-body" style="padding:15px"> <h4 class="site-color title4 text-center"><div class="content" style="margin:0px"></div></h2></div><div class="modal-footer actions-buttons buttons"></div></div></div>',
        cancelButton: __dialogCancelText,
        cancelButtonClass: 'btn-site',
        confirmButton: __dialogYesText,
        confirmButtonClass: 'btn-site',

        animation: 'scale',
        columnClass: 'alert_normal alert_new col-md-6 col-md-offset-3 col-xs-6 col-xs-offset-3',
        backgroundDismiss: false,
        closeIcon: false,
        confirm: function ()
        {

            var data = $('#' + formId).serializeArray();

            if (paramObj != "" && paramObj != null)
            {
                data.push({ name: 'param', value: docoNutToken });
            }

            $.ajax({
                type: actionType,
                url: actionUrl,
                cache: false,
                data: data,
                success:
                    function (data)
                    {
                        if (typeof data.errorOccurred != "undefined")
                        {
                            if (data.errorOccurred == true && data.url != null)
                            {
                                window.location.href = data.url;
                                return;
                            }
                        }
                        $('#' + formId).find("input[name=__validationGroup]:hidden").remove();

                        if (onAjaxSuccess != '')
                        {
                            window[onAjaxSuccess](data, divUpdateContent);
                        }
                    },
                error: function (data)
                {

                    if (onAjaxFaild != '')
                    {
                        window[onAjaxFaild](data, divUpdateContent);
                    }
                },

            });
        },
        cancel: function ()
        {
            $("#Loader").hide();
        }
    });
    return false;
}

function onSuccessResult(data, divUpdateContent)
{
    $("#" + divUpdateContent).html(data);
}

function onErrorResult(result, divUpdateContent)
{
    $("#" + divUpdateContent).html(result);
}

function ConfirmCustomAction(dialogConfirm, confirmMsg, confirmResource, cancelResource)
{

    $(document).ajaxStart(function ()
    {
        $("#progress").css("display", "block");
        //    $("#resultData").html("");
        //  $("#" + divUpdateContent).html("");
    });
    $(document).ajaxComplete(function () { $("#progress").css("display", "none"); });
    jQuery.support.cors = true

    $("#" + dialogConfirm);//.html(confirmMsg);
    $("#" + dialogConfirm).dialog({
    });
}

function ConfirmPartialAction(dialogConfirm, confirmMsg, confirmResource, cancelResource)
{

    $(document).ajaxStart(function ()
    {
        $("#progress").css("display", "block");
    });
    $(document).ajaxComplete(function () { $("#progress").css("display", "none"); });
    jQuery.support.cors = true

    $("#" + dialogConfirm).dialog({
    });
}

function Action(actionUrl, onAjaxSuccess, formId)
{

    $.ajax({
        type: 'post',
        url: actionUrl,
        cache: false,
        data: $('#' + formId).serializeArray(),
        success:
            function (data, textStatus, request)
            {
                if (typeof data.errorOccurred != "undefined")
                {
                    if (data.errorOccurred == true && data.url != null)
                    {
                        window.location.href = data.url;
                        return;
                    }
                }

                if (onAjaxSuccess != '')
                {
                    window[onAjaxSuccess](data);
                }
            }
    });
}

function ActionButton(actionUrl, divUpdateContent, paramObj, contentType, onAjaxSuccess, onAjaxFaild, actionType, buttonId, formId, clientFunctionToCall, notValidFunctionCall, validFunctionCall)
{

    var continueWorkResult = true;

    if (clientFunctionToCall != '' && clientFunctionToCall != null)
        continueWorkResult = window[clientFunctionToCall]();

    if (!continueWorkResult)
    {
        return false;
    }

    if (actionType == "post")
    {

        ClickedSubmit = $("#" + buttonId).attr("name");
        if (!$('#' + formId).valid())
        {

            if (notValidFunctionCall != "" && notValidFunctionCall != null)
                window[notValidFunctionCall]();
            $(window).scrollTop(0);
            return false;
        }
        else
        {

            if (validFunctionCall != "" && validFunctionCall != null)
                window[validFunctionCall]();
        }
    }
    //actionUrl = actionUrl.replace(".", "\\");

    if ($('div.grid-mvc table').attr('id') != null)
    {
        //eval("pageGrids." + $('div.grid-mvc table').attr('id') + ".clearGridFiltersAndSorting()");
    }

    $('#' + formId).append('<input type="hidden" id="__validationGroup" value=' + ClickedSubmit + ' name="__validationGroup">');



    //$(document).ajaxStart(function () {
    //    $("#Loader").show();
    //    //    $("#resultData").html("");
    //    //$("#" + divUpdateContent).html("");
    //});

    //$(document).ajaxSuccess(function (event, request, settings) {
    //});

    //$(document).ajaxError(function (event, request, settings) {
    //});

    //$(document).ajaxComplete(function () { $("#Loader").hide(); });
    jQuery.support.cors = true;

    var formData;

    if ($('#' + formId) != null && ('#' + formId).length > 0)
    {

        formData = new FormData($('#' + formId)[0]);
    }

    $.ajax({
        type: actionType,
        url: actionUrl,
        //async: false,
        //contentType: contentType,// 'application/json; charset=utf-8',
        data: formData,//$('#' + formId).serializeArray(),//paramObj,//"{id:1}",
        cache: false,
        contentType: false,
        processData: false,
        success:
            function (data, textStatus, request)
            {
                if (typeof data.errorOccurred != "undefined")
                {
                    if (data.errorOccurred == true && data.url != null)
                    {
                        window.location.href = data.url;
                        return;
                    }
                }

                //if (data.MessageType != 2) {

                //    $("#" + divUpdateContent).html("");
                //    $("#" + divUpdateContent).html(data.Html);
                //    jQuery.validator.unobtrusive.parse('#' + divUpdateContent);
                //    ShowSuccesMessage(data.MessageText);
                //}
                //else {
                //    ShowErrorMessage(data.MessageText);
                //}

                $('#' + formId).find("input[name=__validationGroup]:hidden").remove();

                if (onAjaxSuccess != '')
                {
                    window[onAjaxSuccess](data, divUpdateContent);
                }
            },
        error: function (data)
        {
            //   eval(onAjaxSuccess)(data);
            if (onAjaxFaild != '')
            {
                window[onAjaxFaild](data, divUpdateContent);
            }
        }

    });
}

function ClientActionButton(actionUrl, divUpdateContent, onAjaxSuccess, onAjaxFaild, buttonId, divId, paramObj, clientFunctionToCall)
{
    var continueWork = true;

    if (clientFunctionToCall != '' && clientFunctionToCall != null)
        continueWork = window[clientFunctionToCall]();

    if (!continueWork)
    {
        return false;
    }

    ClickedSubmit = $("#" + buttonId).attr("name");
    var form = $('#' + divId).parents("form");
    if (!$(form).valid())
    {
        return false;
    }
    //actionUrl = actionUrl.replace(".", "\\");

    if ($('div.grid-mvc table').attr('id') != null)
    {
        //eval("pageGrids." + $('div.grid-mvc table').attr('id') + ".clearGridFiltersAndSorting()");
    }

    //$(document).ajaxStart(function () {
    //    $("#Loader").show();
    //    setTimeout(2000);

    //});

    $.ajaxSetup({ cache: false });

    //$(document).ajaxComplete(function (resp) {
    //    $("#Loader").hide();
    //});

    var data = $('#' + divId + " :input").serializeArray();
    data.push({ name: 'param', value: eval(paramObj) });

    var previouslyAddedToken = data.filter(function (item) { return item != null && item.name === '__RequestVerificationToken'; });
    if (previouslyAddedToken == null || previouslyAddedToken == undefined || previouslyAddedToken.length == 0)
    {
        data.push({ name: '__RequestVerificationToken', value: form.find("input[name='__RequestVerificationToken']").val() });
    }
    $.ajax({
        type: 'post',
        cache: false,
        url: actionUrl,
        data: data,// + '&' + $.param({ 'param': eval(paramObj) }),//paramObj,//"{id:1}",
        success:
            function (data, textStatus, request)
            {
                if (typeof data.errorOccurred != "undefined")
                {
                    if (data.errorOccurred == true && data.url != null)
                    {
                        window.location.href = data.url;
                        return;
                    }
                }

                if (onAjaxSuccess != '')
                {
                    window[onAjaxSuccess](data, divUpdateContent);
                }
            },
        error: function (data)
        {
            //   eval(onAjaxSuccess)(data);
            if (onAjaxFaild != '')
            {
                window[onAjaxFaild](data, divUpdateContent);
            }
        }

    });
}

function AddToViewerClientActionButton(actionUrl, divUpdateContent, onAjaxSuccess, onAjaxFaild, buttonId, divId, clientFunctionToCall, frameViewerId, uploadFileDivId)
{
    var continueWork = true;
    //TODO: Doconut
    var docoNutFrameContent = $('#' + frameViewerId).contents();
    var docoNutToken = $('#_hdnToken', docoNutFrameContent)[0].value;

    if (clientFunctionToCall != '' && clientFunctionToCall != null)
        continueWork = window[clientFunctionToCall]();

    if (!continueWork)
    {
        return false;
    }

    ClickedSubmit = $("#" + buttonId).attr("name");
    var form = $('#' + divId).parents("form");
    var token = form.find("input[name='__RequestVerificationToken']").val();
    if (!$(form).valid())
    {
        return false;
    }
    //actionUrl = actionUrl.replace(".", "\\");

    if ($('div.grid-mvc table').attr('id') != null)
    {
        //eval("pageGrids." + $('div.grid-mvc table').attr('id') + ".clearGridFiltersAndSorting()");
    }

    //$(document).ajaxStart(function () {
    //    $("#Loader").show();
    //    setTimeout(2000);

    //});

    $.ajaxSetup({ cache: false });

    //$(document).ajaxComplete(function (resp) {
    //    $("#Loader").hide();
    //});

    var data = $('#' + divId + " :input").serializeArray();
    data.push({ name: 'param', value: docoNutToken });
    var previouslyAddedToken = data.filter(function (item) { return item != null && item.name === '__RequestVerificationToken'; });
    if (previouslyAddedToken == null || previouslyAddedToken == undefined || previouslyAddedToken.length == 0)
    {
        data.push({ name: '__RequestVerificationToken', value: token });
    }

    if (uploadFileDivId != "")
    {
        var files = $('#' + uploadFileDivId).find($("input:file")).get(0).files;
        if (files.length > 0)
        {
            var FormattedFileName = "_" + files[0].name;
            data.push({ name: "file", value: FormattedFileName });
        } else
        {
            data.push({ name: "file", value: "" });
        }
    }

    $.ajax({
        type: 'post',
        cache: false,
        url: actionUrl,
        data: data,// + '&' + $.param({ 'param': eval(paramObj) }),//paramObj,//"{id:1}",
        success:
            function (data, textStatus, request)
            {
                if (typeof data.errorOccurred != "undefined")
                {
                    if (data.errorOccurred == true && data.url != null)
                    {
                        window.location.href = data.url;
                        return;
                    }
                }

                if (onAjaxSuccess != '')
                {
                    window[onAjaxSuccess](data, divUpdateContent);
                }
            },
        error: function (data)
        {
            //   eval(onAjaxSuccess)(data);
            if (onAjaxFaild != '')
            {
                window[onAjaxFaild](data, divUpdateContent);
            }
        }

    });
}

function PrintButton(actionUrl, callbackFunc)
{
    $.ajax({
        type: "Get",
        url: actionUrl,
        cache: false,
        success:
            function (data, textStatus, request)
            {

                if (typeof data.errorOccurred != "undefined")
                {
                    if (data.errorOccurred == true && data.url != null)
                    {
                        window.location.href = data.url;
                        return;
                    }
                }

                if (data.MessageType != 2)
                {

                    if (callbackFunc != "" & callbackFunc != undefined)
                    {
                        callbackFunc();
                    }

                    var BarcodeWindow = window.open('', '_blank', 'width=500,height=500');
                    BarcodeWindow.opener = null;
                    BarcodeWindow.document.write(data.Html);
                    BarcodeWindow.document.close();
                    BarcodeWindow.focus();
                    setTimeout(function () { BarcodeWindow.print(); BarcodeWindow.close(); }, 2000);
                }
                else
                {
                    ShowErrorMessage(data.MessageText);
                }

            },
        error: function (data)
        {

        }

    });

}

function PrintReport(actionUrl, callbackFunc, callBackAfterPrintFunc)
{
    $.ajax({
        type: "Get",
        url: actionUrl,
        cache: false,
        success:
            function (data, textStatus, request)
            {

                if (typeof data.errorOccurred != "undefined")
                {
                    if (data.errorOccurred == true && data.url != null)
                    {
                        window.location.href = data.url;
                        return;
                    }
                }

                if (data.MessageType != 2)
                {

                    if (callbackFunc != "" & callbackFunc != undefined)
                    {
                        callbackFunc();
                    }

                    var BarcodeWindow = window.open('', '_blank', 'width=900,height=500');

                    BarcodeWindow.document.write(data.Html);
                    BarcodeWindow.document.close();
                    BarcodeWindow.focus();

                    if (callBackAfterPrintFunc != "" & callBackAfterPrintFunc != undefined)
                    {
                        BarcodeWindow.onafterprint = callBackAfterPrintFunc();
                    }

                    setTimeout(function ()
                    {
                        BarcodeWindow.print();
                        BarcodeWindow.close();
                    }, 2000);
                }
                else
                {
                    ShowErrorMessage(data.MessageText);
                }

            },
        error: function (data)
        {

        }

    });

}

function ShowVerifyTransactionNumberOrBarcode(url, tranId, type)
{
    __dialogVerifyTransaction = $.confirm({
        title: false,
        content: function ($obj)
        {
            return $.ajax({
                url: url,
                type: 'get',
                data: { id: tranId, transactionCategoryId: type },
                success: function (data)
                {
                    $obj.setContent(data.Html);
                    jQuery.validator.unobtrusive.parse('#frmCheckTransaction');
                }
            });
        },
        onOpen: function ()
        {
            jQuery(function ()
            {
                jQuery('.jconfirm-box').draggable({
                    revert: false,
                });
            });
            $("#Loader").hide();
        },
        confirmButton: false,
        icon: 'fa fa-info',
        animation: 'scale',
        columnClass: "dialogbox col-md-8 col-md-offset-2 col-xs-8 col-xs-offset-2",
        backgroundDismiss: false,
        closeIcon: true,
        cancelButton: false
    });
}