function DeleteButton(actionUrl, formId, gridName, contentType, onAjaxSuccess, onAjaxFaild, actionType, id, columnIndex, divUpdatData) {

    var input = '';

    input = $("#" + id).parents('tr').find("td").eq(columnIndex).html();

    //actionUrl = actionUrl.replace(".", "\\");
    //$(document).ajaxStart(function () {
    //    $(".spinner").show();

    //});
    //$(document).ajaxComplete(function () {
    //    $(".spinner").hide();

    //});

    jQuery.support.cors = true;

    $.ajax({
        cache: false,
        type: actionType,
        url: actionUrl,
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        data: $("#" + formId).serialize() + '&' + $.param({ 'ids': input }),//"{id:1}",
        success:
            function (data) {

                if (data.MessageType != 2) {

                    if (data.MessageType == undefined) {
                        window.location.href = __loginPageUrl;
                    }

                    $("#" + divUpdatData).html("");
                    $("#" + divUpdatData).html(data.Html);

                    if (onAjaxSuccess != '') {
                        window[onAjaxSuccess](data, divUpdatData, input);
                        //window[onAjaxSuccess](data, divUpdatData);
                    }
                }
                else {
                    ShowErrorMessage(data.MessageText);
                }
            },
        error: function (data) {
            if (onAjaxFaild != '') {
                window[onAjaxFaild](data, divUpdatData);
            }
        }


    });

}

function DeleteActionButton(actionUrl, formId, gridName, contentType, onAjaxSuccess, onAjaxFaild, actionType, id, columnIndex, divUpdatData, confirmMsg, confirmResource, cancelResource, validFunctionCall) {

    var input = '';
    var dialogConfirm = "dialog-confirm";

    input = $("#" + id).parents('tr').find("td").eq(columnIndex).html();

    //actionUrl = actionUrl.replace(".", "\\");
    //$(document).ajaxStart(function () {
    //    $(".spinner").show();

    //});
    //$(document).ajaxComplete(function () {
    //    $(".spinner").hide();
    //});

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

            if (validFunctionCall != "" && validFunctionCall != null) {

                var valid = window[validFunctionCall](input);

                if (!valid) {

                    return false;
                }
            }
            var dataAction = "";
            if ($("#" + formId).serialize() != "") {
                dataAction = $("#" + formId).serialize() + '&' + $.param({ 'ids': input });
            }
            else {
                dataAction = $.param({ 'ids': input });
            }

            $.ajax({
                cache: false,
                type: actionType,
                url: actionUrl,
                contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
                data: dataAction,//"{id:1}",
                success:
                    function (data) {
                        if (data.MessageType != 2) {

                            if (data.MessageType == undefined) {
                                window.location.href = __loginPageUrl;
                            }

                            $("#" + divUpdatData).html("");
                            $("#" + divUpdatData).html(data.Html);

                            if (onAjaxSuccess != '') {
                                window[onAjaxSuccess](data, divUpdatData, input);
                            }
                            else {
                                ShowSuccesMessage(data.MessageText);
                            }
                        }
                        else {
                            ShowErrorMessage(data.MessageText);
                        }
                    },
                error: function (data) {

                    if (onAjaxFaild != '') {
                        window[onAjaxFaild](data, divUpdatData);
                    }
                }
            });
        },
        cancel: function () {
        }
    });
    return false;
}

function DeleteItems(tableId, actionUrl, columnIndex, gridName, formId, divUpdatData, onAjaxSuccess, validFunctionCall, confirmMsg) {

    var deletedList = '';
    $(tableId).find("#spanCheckBox :checkbox:checked").each(function () {
        deletedList += parseInt($(this).parents('tr').find("td").eq(columnIndex).html()) + ',';
    });
    deletedList = deletedList.substring(0, deletedList.length - 1);

    jQuery.support.cors = true

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

            if (validFunctionCall != "" && validFunctionCall != null) {

                var valid = window[validFunctionCall](deletedList);

                if (!valid) {

                    return false;
                }
            }

            $.ajax({
                cache: false,
                type: 'post',
                url: actionUrl,
                data: $("#" + formId).serialize() + '&' + $.param({ 'ids': deletedList }),//$(input).serializeArray(),//"{id:1}",
                success:
                    function (data) {

                        if (data.MessageType != 2) {

                            if (data.MessageType == undefined) {
                                window.location.href = __loginPageUrl;
                            }

                            $("#" + divUpdatData).html("");
                            $("#" + divUpdatData).html(data.Html);

                            if (onAjaxSuccess != '') {
                                window[onAjaxSuccess](data, divUpdatData, deletedList);
                            }
                            else {
                                ShowSuccesMessage(data.MessageText);
                            }
                        }
                        else {
                            ShowErrorMessage(data.MessageText);
                        }

                    },
                error: function (data) {
                }

            });
        },
        cancel: function () {
        }
    });
}

function UpdateButton(actionUrl, formId, gridName, contentType, onAjaxSuccess, onAjaxFaild, actionType, id, columnIndex, divUpdatData) {

    var input;

    if (!$.isNumeric($("#" + id).parents('tr').find("td").eq(columnIndex).html())) {
        input = $("#" + id).parents('tr').find("td").eq(columnIndex).html();
    }
    else {
        input = parseInt($("#" + id).parents('tr').find("td").eq(columnIndex).html());
    }
    //actionUrl = actionUrl.replace(".", "\\");
    //$(document).ajaxStart(function () {
    //    $(".spinner").show();

    //});
    //$(document).ajaxComplete(function () {
    //    $(".spinner").hide();

    //});
    jQuery.support.cors = true
    $.ajax({
        cache: false,
        type: actionType,
        url: actionUrl,
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        data: $("#" + formId).serialize() + '&' + $.param({ 'id': input }),
        success:
            function (data) {
                if (data.MessageType != 2) {

                    if (data.MessageType == undefined) {
                        window.location.href = __loginPageUrl;
                    }

                    $("#" + divUpdatData).html("");
                    $("#" + divUpdatData).html(data.Html);
                    jQuery.validator.unobtrusive.parse('#' + divUpdatData);
                }
                else {
                    ShowErrorMessage(data.MessageText);
                }

                if (onAjaxSuccess != '') {
                    window[onAjaxSuccess](data, divUpdatData);
                }
            },
        error: function (data) {

            if (onAjaxFaild != '') {
                window[onAjaxFaild](data, divUpdatData);
            }
        }


    });
}

