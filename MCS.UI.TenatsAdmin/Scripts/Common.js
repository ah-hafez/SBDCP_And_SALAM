
function ShowMessage(messageType, messageText) {

    alert(messageText + '.........' + messageType);
}

function ClearInputs(containerId) {
  
    $('#' + containerId).find(':input')
        .not('input:submit').not('input:button').not('input:radio')
        .not('input:checkbox').not('input.__hdnSticky').each(function () {

            $(this).val('');
        })

    $('#' + containerId).find('input:checkbox').each(function () {

        $(this).prop('checked', false);
    });

    $('#' + containerId).find(".removeTag").trigger("click");
    $('#' + containerId).find(".removeDepartmentTag").trigger("click");
    $('#' + containerId).find(".hdnDepartmentId").trigger("change");
}


function ClearFormInputs() {

    var form = $(event.target).parents("form:first");
    $(form).find(':input')
        .not('input:submit').not('input:button').not('input:radio')
        .not('input:checkbox').not('input.__hdnSticky').each(function () {

            $(this).val('');
        })

    $(form).find('input:checkbox').each(function () {
        $(this).prop('checked', false);
    })

    $(".removeTag").trigger("click");
    $(".removeDepartmentTag").trigger("click");
    $(".hdnDepartmentId").trigger("change");
}

var __dialog;

function ShowDialog(url, dialogClass) {

    dialogClass = dialogClass || 'dialogbox col-md-8 col-md-offset-2 col-xs-8 col-xs-offset-2';

    __dialog = $.confirm({
        title: false,
        content: 'url:' + url,
        icon: 'fa fa-info',
        animation: 'scale',
        columnClass: dialogClass,
        backgroundDismiss: false,
        cancelButton: false,
        confirmButton: false,
        closeIcon: true,
        onOpen: function () {
            jQuery(function () {
                jQuery('.jconfirm-box').draggable({
                    revert: false,
                });
            });

            $("#Loader").hide();
        },

    });
}

function ShowDialogAjax(url, data, onAjaxSuccess, dialogClass) {
   
    dialogClass = dialogClass || 'dialogbox col-md-8 col-md-offset-2 col-xs-8 col-xs-offset-2';

    __dialog = $.confirm({
        title: false,
        content: function ($obj) {
            return $.ajax({
                url: url,
                type: 'POST',
                data: data,
                cache: false,
                success: function (data) {

                    if (onAjaxSuccess != '') {
                        window[onAjaxSuccess](data, $obj);
                    }
                }
            });
        },
        onOpen: function () {
            jQuery(function () {
                jQuery('.jconfirm-box').draggable({
                    revert: false,
                });
            });

            $("#Loader").hide();
        },


        confirmButton: false,
        icon: 'fa fa-info',
        animation: 'scale',
        columnClass: dialogClass,
        backgroundDismiss: false,
        closeIcon: true,
        cancelButton: false
    });
}

function ShowInformationMessage(messageText) {
    $.confirm({
        title: false,
        content: '<div class="mb-title"><span class="fa fa-info"></span> </div><p>' + messageText + '</p>',

        cancelButton: __dialogCloseText,
        cancelButtonClass: 'btn-w btn-close-margin',

        icon: 'fa fa-info',
        animation: 'scale',
        columnClass: 'alert_information alert_new col-md-6 col-md-offset-3 col-xs-6 col-xs-offset-3',
        backgroundDismiss: false,
        closeIcon: false,
        confirmButton: false,
        onOpen: function () {
            jQuery(function () {
                jQuery('.jconfirm-box').draggable({
                    revert: false
                });
            });
        },

    });
}

function ShowSuccesMessage(messageText, OnCloseFunction) {
    $.confirm({
        title: false,
        content: '<div class="mb-title"><span class="fa fa-check"></span></div><p>' + messageText + '</p>',

        cancelButton: __dialogCloseText,
        cancelButtonClass: 'btn-w btn-close-margin',

        icon: 'fa fa-info',
        animation: 'scale',
        columnClass: 'alert_success alert_new col-md-6 col-md-offset-3 col-xs-6 col-xs-offset-3',
        backgroundDismiss: false,
        closeIcon: false,
        confirmButton: false,
        cancel: function () {
            if (OnCloseFunction != '' && OnCloseFunction != undefined) {
                window[OnCloseFunction]();
            }
        },
        onOpen: function () {
            jQuery(function () {
                jQuery('.jconfirm-box').draggable({
                    revert: false
                });
            });
        },
    });
}

function ShowErrorMessage(messageText, OnCloseFunction) {
    $.confirm({
        title: false,
        content: '<div><div class="mb-title"><span class="fa fa-times"></span></div><p>' + messageText + '</p></div>',

        cancelButton: __dialogCloseText,
        cancelButtonClass: 'btn-w btn-close-margin',

        icon: 'fa fa-info',
        animation: 'scale',
        columnClass: 'alert_danger alert_new col-md-6 col-md-offset-3 col-xs-6 col-xs-offset-3',
        backgroundDismiss: false,
        closeIcon: false,
        confirmButton: false,
         cancel: function () {
            if (OnCloseFunction != '' && OnCloseFunction != undefined) {
                window[OnCloseFunction]();
            }
         },
         onOpen: function () {
             jQuery(function () {
                 jQuery('.jconfirm-box').draggable({
                     revert: false
                 });
             });
         },
    });
}

function ShowWarningMessage(messageText) {
    $.confirm({
        title: false,
        content: '<div><div class="mb-title"><span class="fa fa fa-warning"></span> تحذير</div><p>' + messageText + '</p></div>',

        cancelButton: __dialogCloseText,
        cancelButtonClass: 'btn-w btn-close-margin',

        icon: 'fa fa-info',
        animation: 'scale',
        columnClass: 'alert_warning alert_new col-md-6 col-md-offset-3 col-xs-6 col-xs-offset-3',
        backgroundDismiss: false,
        closeIcon: false,
        confirmButton: false,
        onOpen: function () {
            jQuery(function () {
                jQuery('.jconfirm-box').draggable({
                    revert: false,
                });
            });
        },
    });
}

function ShowConfirmMessage(messageText,onConfirmFunction)
{
    $.confirm({
        title: '',
        content: '<div class="mb-title"><span class="fa fa-question"></span></div><p>' + messageText + '</p>',
        cancelButton: __dialogNoText,
        cancelButtonClass: 'btn-default',
        confirmButton: __dialogYesText,
        confirmButtonClass: 'btn-primary',

        animation: 'scale',
        columnClass: 'alert_normal alert_new col-md-6 col-md-offset-3 col-xs-6 col-xs-offset-3',
        backgroundDismiss: false,
        closeIcon: false,
        confirm: function () {
            if (onConfirmFunction != '' && onConfirmFunction != undefined) {
                window[onConfirmFunction]();
            }
        },
        cancel: function () {
            $("#Loader").hide();
        },
        onOpen: function () {
            jQuery(function () {
                jQuery('.jconfirm-box').draggable({
                    revert: false,
                });
            });
        },

    });
}
