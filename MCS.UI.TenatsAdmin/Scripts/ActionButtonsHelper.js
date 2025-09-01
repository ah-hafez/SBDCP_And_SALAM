/// <reference path="Common.js" />
function ConfirmAction(actionUrl, divUpdateContent, paramObj, contentType, onAjaxSuccess, onAjaxFaild, confirmMsg, actionType, formId, buttonId, confirmResource, cancelResource, clientFunctionToCall, notValidFunctionCall) {

    var continueWork = true;

    if (clientFunctionToCall != '' && clientFunctionToCall != null)
        continueWork = window[clientFunctionToCall]();

    if (!continueWork) {
        return false;
    }

    ClickedSubmit = $("#" + buttonId).attr("name");

    if (!$('#' + formId).valid()) {

        if (notValidFunctionCall != "" && notValidFunctionCall != null)
            window[notValidFunctionCall]();

        return false;
    }

    $('#' + formId).append('<input type="hidden" id="__validationGroup" value=' + ClickedSubmit + ' name="__validationGroup">');
    //actionUrl = actionUrl.replace(".", "\\");

    if ($('div.grid-mvc table').attr('id') != null) {
        eval("pageGrids." + $('div.grid-mvc table').attr('id') + ".clearGridFiltersAndSorting()");
    }

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

    $.confirm({
        title: '',
        content: '<div class="mb-title"><span class="fa fa-question"></span></div><p>' + confirmMsg + '</p>',
        cancelButton: __dialogNoText,
        cancelButtonClass: 'btn-default',
        confirmButton: __dialogYesText,
        confirmButtonClass: 'btn-primary',

        animation: 'scale',
        columnClass: 'alert_normal alert_new col-md-6 col-md-offset-3 col-xs-6 col-xs-offset-3',
        backgroundDismiss: false,
        closeIcon: false,
        confirm: function () {

            var data = $('#' + formId).serializeArray();

            if (paramObj != "" && paramObj != null) {
                data.push({ name: 'param', value: eval(paramObj) });
            }

            $.ajax({
                type: actionType,
                url: actionUrl,
                cache: false,
                //contentType: 'application/json; charset=utf-8',
                data: data,//paramObj,//"{id:1}",
                success:
                    function (data) {
                        if (typeof data.errorOccurred != "undefined") {
                            if (data.errorOccurred == true && data.url != null) {
                                window.location.href = data.url;
                                return;
                            }
                        }
                        $('#' + formId).find("input[name=__validationGroup]:hidden").remove();

                        if (onAjaxSuccess != '') {
                            window[onAjaxSuccess](data, divUpdateContent);
                        }
                    },
                error: function (data) {

                    if (onAjaxFaild != '') {
                        window[onAjaxFaild](data, divUpdateContent);
                    }
                },

            });
        },
        cancel: function () {
            $("#Loader").hide();
        }
    });
    return false;
}

function onSuccessResult(data, divUpdateContent) {
    $("#" + divUpdateContent).html(data);
}

function onErrorResult(result, divUpdateContent) {
    $("#" + divUpdateContent).html(result);
}

function ConfirmCustomAction(dialogConfirm, confirmMsg, confirmResource, cancelResource) {

    $(document).ajaxStart(function () {
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

function ConfirmPartialAction(dialogConfirm, confirmMsg, confirmResource, cancelResource) {

    $(document).ajaxStart(function () {
        $("#progress").css("display", "block");
    });
    $(document).ajaxComplete(function () { $("#progress").css("display", "none"); });
    jQuery.support.cors = true

    $("#" + dialogConfirm).dialog({
    });
}

function Action(actionUrl, onAjaxSuccess, formId) {

    $.ajax({
        type: 'post',
        url: actionUrl,
        cache: false,
        data: $('#' + formId).serializeArray(),
        success:
            function (data, textStatus, request) {
                if (typeof data.errorOccurred != "undefined") {
                    if (data.errorOccurred == true && data.url != null) {
                        window.location.href = data.url;
                        return;
                    }
                }

                if (onAjaxSuccess != '') {
                    window[onAjaxSuccess](data);
                }
            }
    });
}

function ActionButton(actionUrl, divUpdateContent, paramObj, contentType, onAjaxSuccess, onAjaxFaild, actionType, buttonId, formId, clientFunctionToCall, notValidFunctionCall, validFunctionCall) {
    var continueWorkResult = true;
    var array = clientFunctionToCall.split(",");
    for (var i = 0; i < array.length; i++) {
        if (array[i] != '' && array[i] != null) {
            continueWorkResult &= window[array[i]]();
        }
    }

    if (!continueWorkResult) {
        return false;
    }

    if (actionType == "post") {
        ClickedSubmit = $("#" + buttonId).attr("name");
        if (!$('#' + formId).valid()) {

            if (notValidFunctionCall != "" && notValidFunctionCall != null)
                window[notValidFunctionCall]();

            return false;
        }
        else {
            if (validFunctionCall != "" && validFunctionCall != null)
                window[validFunctionCall]();
        }
    }
    //actionUrl = actionUrl.replace(".", "\\");

    if ($('div.grid-mvc table').attr('id') != null) {
        eval("pageGrids." + $('div.grid-mvc table').attr('id') + ".clearGridFiltersAndSorting()");
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

    if ($('#' + formId) != null && ('#' + formId).length > 0) {

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
            function (data, textStatus, request) {
                if (typeof data.errorOccurred != "undefined") {
                    if (data.errorOccurred == true && data.url != null) {
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

                if (onAjaxSuccess != '') {
                    window[onAjaxSuccess](data, divUpdateContent);
                }
            },
        error: function (data) {
            //   eval(onAjaxSuccess)(data);
            if (onAjaxFaild != '') {
                window[onAjaxFaild](data, divUpdateContent);
            }
        }

    });
}

function ClientActionButton(actionUrl, divUpdateContent, onAjaxSuccess, onAjaxFaild, buttonId, divId, paramObj, clientFunctionToCall) {

    var continueWork = true;

    if (clientFunctionToCall != '' && clientFunctionToCall != null)
        continueWork = window[clientFunctionToCall]();

    if (!continueWork) {
        return false;
    }

    ClickedSubmit = $("#" + buttonId).attr("name");
    var form = $('#' + divId).parents("form");
    if (!$(form).valid()) {
        return false;
    }
    //actionUrl = actionUrl.replace(".", "\\");

    if ($('div.grid-mvc table').attr('id') != null) {
        eval("pageGrids." + $('div.grid-mvc table').attr('id') + ".clearGridFiltersAndSorting()");
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

    $.ajax({
        type: 'post',
        cache: false,
        url: actionUrl,
        data: data,// + '&' + $.param({ 'param': eval(paramObj) }),//paramObj,//"{id:1}",
        success:
            function (data, textStatus, request) {
                if (typeof data.errorOccurred != "undefined") {
                    if (data.errorOccurred == true && data.url != null) {
                        window.location.href = data.url;
                        return;
                    }
                }

                if (onAjaxSuccess != '') {
                    window[onAjaxSuccess](data, divUpdateContent);
                }
            },
        error: function (data) {
            //   eval(onAjaxSuccess)(data);
            if (onAjaxFaild != '') {
                window[onAjaxFaild](data, divUpdateContent);
            }
        }

    });
}

//function PrintButton(actionUrl) {

//    var printWindow = window.open(actionUrl, '_BLANK', "location=1,status=1,scrollbars=1,width=1000,height=600");

//    printWindow.print();

//}


function PrintButton(actionUrl, callbackFunc) {
    $.ajax({
        type: "Get",
        url: actionUrl,
        cache: false,
        success:
            function (data, textStatus, request) {

                if (typeof data.errorOccurred != "undefined") {
                    if (data.errorOccurred == true && data.url != null) {
                        window.location.href = data.url;
                        return;
                    }
                }

                if (data.MessageType != 2) {

                    if (callbackFunc != "" & callbackFunc != undefined) {
                        callbackFunc();
                    }

                    var BarcodeWindow = window.open('', '_blank', 'width=500,height=500,noopener,noreferrer');

                    BarcodeWindow.document.write(data.Html);
                    BarcodeWindow.document.close();
                    BarcodeWindow.focus();
                    setTimeout(function () { BarcodeWindow.print(); BarcodeWindow.close(); }, 2000);
                }
                else {
                    ShowErrorMessage(data.MessageText);
                }

            },
        error: function (data) {

        }

    });

}