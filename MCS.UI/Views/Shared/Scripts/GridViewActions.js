function DeleteButton(actionUrl, formId, gridName, contentType, onAjaxSuccess, onAjaxFaild, actionType, id, columnIndex, divUpdatData) {

    var input = '';

    input = $("#" + id).parents('tr').find("td").eq(columnIndex).html();

    var data = $("#" + formId).serialize();
    if (actionType != "" && actionType.toLowerCase() == "post" && /__RequestVerificationToken/i.test(data) == false) {
        var token = $("form:first").find("input[name='__RequestVerificationToken']").val();
        if (token != "") {
            data += "&__RequestVerificationToken=" + token;
        }
    }

    jQuery.support.cors = true;

    $.ajax({
        cache: false,
        type: actionType,
        url: actionUrl,
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        data: data + '&' + $.param({ 'ids': input }),//"{id:1}",
        success:
            function (data) {

                if (data.MessageType != 2) {

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

    jQuery.support.cors = true;

    $.confirm({
        title: '',
        content: '<p>' + confirmMsg + '</p>',
        template: '<div class="jconfirm"><div class="jconfirm-bg"></div><div class="modal-dialog"><div class="modal-content"><div class="modal-body" style="padding:15px"> <h4 class="site-color title4 text-center"><div class="content" style="margin:0px"></div></h2></div><div class="modal-footer actions-buttons buttons"></div></div></div>',
        cancelButton: __dialogNoText,
        cancelButtonClass: 'btn-site',
        confirmButton: __dialogYesText,
        confirmButtonClass: 'btn-site',

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

            var data = null;
            if (formId) {
                data = $("#" + formId).serialize();
            }

            if (actionType != "" && actionType.toLowerCase() == "post" && /__RequestVerificationToken/i.test(data) == false) {
                var token = $("form:first").find("input[name='__RequestVerificationToken']").val();
                if (token != "") {
                    data += "&__RequestVerificationToken=" + token;
                }
            }

            $.ajax({
                cache: false,
                type: actionType,
                url: actionUrl,
                contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
                data: data + '&' + $.param({ 'ids': input }),//"{id:1}",
                success:
                    function (data) {
                        if (data.MessageType != 2) {

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
        content: '<p>' + confirmMsg + '</p>',
        template: '<div class="jconfirm"><div class="jconfirm-bg"></div><div class="modal-dialog"><div class="modal-content"><div class="modal-body" style="padding:15px"> <h4 class="site-color title4 text-center"><div class="content" style="margin:0px"></div></h2></div><div class="modal-footer actions-buttons buttons"></div></div></div>',
        cancelButton: __dialogNoText,
        cancelButtonClass: 'btn-site',
        confirmButton: __dialogYesText,
        confirmButtonClass: 'btn-site',

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

    var data = null;

    if (formId) {
        data = $("#" + formId).serialize() + '&' + $.param({ 'id': input });
    }
    else {
        data = $.param({ 'id': input });
    }

    jQuery.support.cors = true;
    $.ajax({
        cache: false,
        type: actionType,
        url: actionUrl,
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        data: data,
        success:
            function (data) {
                if (data.MessageType != 2) {

                    $("#" + divUpdatData).html("");
                    $("#" + divUpdatData).html(data.Html);
                 /*   jQuery.validator.unobtrusive.parse('#' + divUpdatData);*/
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

function DeleteActionGrid(gridName, columnIndex, confirmMsg, ClearFormId, callbackFunction, value, callbackFunctionAfterClearForm) {
    
    var deleteRow = $("#grid-table-" + gridName).find("tr > td").find("#Key_" + columnIndex);
    jQuery.support.cors = true;
    $.confirm({
        title: '',
        content: '<p>' + confirmMsg + '</p>',
        template: '<div class="jconfirm"><div class="jconfirm-bg"></div><div class="modal-dialog"><div class="modal-content"><div class="modal-body" style="padding:15px"> <h4 class="site-color title4 text-center"><div class="content" style="margin:0px"></div></h2></div><div class="modal-footer actions-buttons buttons"></div></div></div>',
        cancelButton: __dialogNoText,
        cancelButtonClass: 'btn-site ',
        confirmButton: __dialogYesText,
        confirmButtonClass: 'confirmDelete btn-site',
        animation: 'scale',
        columnClass: 'alert_normal alert_new col-md-6 col-md-offset-3 col-xs-6 col-xs-offset-3',
        backgroundDismiss: false,
        closeIcon: false,
        confirm: function () {
            if (deleteRow) {
                deleteRow.closest('tr').remove();
            }
            deleteRow = undefined;
            var gridTd = $("#grid-table-" + gridName).find("tr > td");
            if (gridTd.length === 0) {
                $("#grid-table-" + gridName + "> tbody:last").append("<tr class='grid-empty-text'><td colspan = '18'>لا يوجد بيانات للعرض</td></tr>");
            }
            if (callbackFunction && callbackFunction !== '') {
                window[callbackFunction](value);
            }
            if (ClearFormId !== '') {
                ClearInputs(ClearFormId);
            }
            if (callbackFunctionAfterClearForm && callbackFunctionAfterClearForm !== '') {
                window[callbackFunctionAfterClearForm](value);
            }
        },
        onOpen: function () {
            if ($('.confirmDelete')) {
                $('.confirmDelete').click(function (e) {
                    $(document).trigger("confirmDeleteClicked", '');
                });
            }
        }
    });
    return false;
}

function DeleteActionGridWithValidation(gridName, columnIndex, confirmMsg, ClearFormId, callbackFunction, value, clientFunctionToCall, extraParam) {
    var deleteRow = $("#grid-table-" + gridName).find("tr > td").find("#Key_" + columnIndex);
    jQuery.support.cors = true;
    $.confirm({
        title: '',
        content: '<p>' + confirmMsg + '</p>',
        template: '<div class="jconfirm"><div class="jconfirm-bg"></div><div class="modal-dialog"><div class="modal-content"><div class="modal-body" style="padding:15px"> <h4 class="site-color title4 text-center"><div class="content" style="margin:0px"></div></h2></div><div class="modal-footer actions-buttons buttons"></div></div></div>',
        cancelButton: __dialogNoText,
        cancelButtonClass: 'btn-site',
        confirmButton: __dialogYesText,
        confirmButtonClass: 'btn-site',
        animation: 'scale',
        columnClass: 'alert_normal alert_new col-md-6 col-md-offset-3 col-xs-6 col-xs-offset-3',
        backgroundDismiss: false,
        closeIcon: false,
        confirm: function () {
            if (clientFunctionToCall && clientFunctionToCall !== '') {
                if (window[clientFunctionToCall](value, extraParam)) {
                    if (deleteRow) {
                        deleteRow.closest('tr').remove();
                    }
                    deleteRow = undefined;
                    var gridTd = $("#grid-table-" + gridName).find("tr > td");
                    if (gridTd.length === 0) {
                        $("#grid-table-" + gridName + "> tbody:last").append("<tr class='grid-empty-text'><td colspan = '18'>لا يوجد بيانات للعرض</td></tr>");
                    }
                    if (callbackFunction && callbackFunction !== '') {
                        window[callbackFunction](value);
                    }
                    if (ClearFormId !== '') {
                        ClearInputs(ClearFormId);
                    }
                }
                else {

                }
            }
            else {
                if (deleteRow) {
                    deleteRow.closest('tr').remove();
                }
                deleteRow = undefined;
                var gridTd = $("#grid-table-" + gridName).find("tr > td");
                if (gridTd.length === 0) {
                    $("#grid-table-" + gridName + "> tbody:last").append("<tr class='grid-empty-text'><td colspan = '18'>لا يوجد بيانات للعرض</td></tr>");
                }
                if (callbackFunction && callbackFunction !== '') {
                    window[callbackFunction](value);
                }
                if (ClearFormId !== '') {
                    ClearInputs(ClearFormId);
                }
            }
        }
    });
    return false;
}