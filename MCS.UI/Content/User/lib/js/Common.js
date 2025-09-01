
function ShowMessage(messageType, messageText) {

    alert(messageText + '.........' + messageType);
}

function ClearInputs(containerId) {

    $('#' + containerId).find(':input')
        .not('input:submit').not('input:button').not('input:radio').not("input[name='__RequestVerificationToken']")
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
        .not('input:submit').not('input:button').not('input:radio').not("input[name='__RequestVerificationToken']")
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
    debugger;

    dialogClass = dialogClass || 'dialogbox col-md-12  col-xs-8';

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

            //jQuery(function () {
            //    jQuery('.jconfirm-box').draggable({
            //        revert: false,
            //    });
            //});

            $("#Loader").hide();
        },

    });
}

function ShowDialogAjax(url, data, onAjaxSuccess, dialogClass, AfterClose) {

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
            //jQuery(function () {
            //    jQuery('.jconfirm-box').draggable({
            //        revert: false,
            //    });
            //});
            $("#Loader").hide();
        },
        onClose: function () {
            if (AfterClose != '' && AfterClose != undefined) {
                window[AfterClose]();
            }
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

function ShowDialogAjaxPrintingBarcodes(url, data, onAjaxSuccess, dialogClass, AfterClose) {
    dialogClass = dialogClass || 'dialogbox col-md-8 col-md-offset-2 col-xs-8 col-xs-offset-2';
    __dialog = $.confirm({
        title: false,
        content: function ($obj) {
            return $.ajax({
                url: url,
                type: 'get',
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
            $("#Loader").hide();
        },
        onClose: function () {
            if (AfterClose != '' && AfterClose != undefined) {
                window[AfterClose]();
            }
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
        cancelButtonClass: 'btn-site',

        icon: 'fa fa-info',
        animation: 'scale',
        columnClass: 'alert_danger alert_new col-md-6 col-md-offset-3 col-xs-6 col-xs-offset-3',
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
        cancelButtonClass: 'btn-site',

        icon: 'fa fa-info',
        animation: 'scale',
        columnClass: 'close-btn alert_success alert_new col-md-6 col-md-offset-3 col-xs-6 col-xs-offset-3',
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
        cancelButtonClass: 'btn-site',

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

function BuildReplyTaskModelContent(messageText, ReplyTransactionTxt, replyBtn) {
    var replyBtnHtml = '';
    if (replyBtn && replyBtn != '') {
        replyBtnHtml = '<button class="reply-button-popup btn-site" data-dismiss="modal">' + replyBtn + '</button>';
    }
    var modelHeader = '<div class="modal-header"></div><div class="modal-body">';
    var modelBody = '<h4 class="site-color title4 text-center">' + messageText + '</h4><div class="row"><div class="col-xs-12 col-sm-10 col-sm-push-1"><fieldset><div class="form-group"><label>' + ReplyTransactionTxt + '</label><div><textarea id="replyTxt" class="form-control"></textarea></div></div><i id="CalReplyMessageError" style="color:red" class="glyphicon glyphicon-exclamation-sign hidden_"></i><span id="ReplyErrorMessageValidate" style="color:red;"></span></fieldset></div></div>';
    var modelFileUploader = ' <div class="col-xs-12 col-sm-10 col-sm-push-1" id="dvUploadFiles"><fieldset><div class="form-group"><input type="file" name="Files" multiple="" onchange="SaveTaskAttachements()" id="Files" class="hidden"><a class="btn-site btn-upload" onclick="$(\'#Files\').click();"><i class="uicon icon_attachment_plus"></i>تحميل مرفق جديد</a></div></fieldset></div><div class="col-xs-12 col-sm-10 col-sm-push-1 col-uploadeed-files"><fieldset><div class="form-group"><label class="control-label">المرفقات المضافة </label><div class="uploaded-files "></div></div></fieldset></div></div>';
    var modelFooter = '<div class="modal-footer actions-buttons">' + replyBtnHtml + '<button class="close-button-popup btn-site" >الغاء</button>  </div>';
    return modelHeader + modelBody + modelFileUploader + modelFooter;
}

function BuildLoginModelContent(messageText) {
    var modelHeader = '<div class="modal-header"></div><div class="modal-body">';
    var modelBody = '<div class="row"><div class="col-xs-12 col-sm-10 col-sm-push-1"><fieldset><div class="form-group">' + messageText + '<div></div></div></fieldset></div></div>';
    var modelFooter = '<div class="modal-footer actions-buttons"><button class="close-button-popup btn-site" >الغاء</button>  </div>';
    return modelHeader + modelBody  + modelFooter;
}

function BuildRejectModelContent(messageText, RejectTransactionTxt, DaysToCompleteTxt, rejectBtn) {
    var rejectBtnHtml = '';
    if (rejectBtn && rejectBtn != '') {
        rejectBtnHtml = '<button class="reject-button-popup btn-site" data-dismiss="modal">' + rejectBtn + '</button>';
    }
    var modelHeader = '<div class="modal-header"></div>';
    var modelBody;
    if (DaysToCompleteTxt != '') {

        modelBody = '<div class="modal-body"><h4 class="site-color title4 text-center">' + messageText + '</h4><div class="row"><div class="col-xs-12 col-sm-10 col-sm-push-1">' +
            '<fieldset>' +
            '<div class="form-group"><label>' + RejectTransactionTxt + '</label><div><textarea id="rejectionReason" class="form-control"></textarea></div></div> <i id="CalMessageError" style="color:red" class="glyphicon glyphicon-exclamation-sign hidden_"></i> <span id="errorMessageValidate" style="color:red;"></span>' +
            '</fieldset>' +

            '<fieldset>' +
            '<div class="form-group">' +
            '<label>' + DaysToCompleteTxt + '</label>' +
            '<div><input id="expectedDays" onkeypress="return IsNumeric(event);" class="form-control"></div>' +
            '</div>' +
            '<i id="CalDaysMessageError" style="color:red" class="glyphicon glyphicon-exclamation-sign hidden_"></i>' +
            '<span id="errorDaysMessageValidate" style="color:red;"></span>' +
            '</fieldset >' +
            '</div ></div ></div> ';
    }
    else {
        modelBody = '<div class="modal-body"><h4 class="site-color title4 text-center">' + messageText + '</h4><div class="row"><div class="col-xs-12 col-sm-10 col-sm-push-1">' +
            '<fieldset>' +
            '<div class="form-group"><label>' + RejectTransactionTxt + '</label><div><textarea id="rejectionReason" class="form-control"></textarea></div></div> <i id="CalMessageError" style="color:red" class="glyphicon glyphicon-exclamation-sign hidden_"></i> <span id="errorMessageValidate" style="color:red;"></span>' +
            '</fieldset>' +
            '<fieldset>' +
            '<i id="CalDaysMessageError" style="color:red" class="glyphicon glyphicon-exclamation-sign hidden_"></i>' +
            '<span id="errorDaysMessageValidate" style="color:red;"></span>' +
            '</fieldset >' +
            '</div ></div ></div> ';
    }

    var modelFooter = '<div class="modal-footer actions-buttons">' + rejectBtnHtml + '<button class="close-button-popup btn-site" >الغاء</button>  </div>';
    return modelHeader + modelBody + modelFooter;
}

function ShowReplyTaskPopup(messageText, OnCloseFunction, ReplyTransactionTxt, replyBtn) {
    $.confirm({
        title: false,
        cancelButton: __dialogCloseText,
        cancelButtonClass: 'btn-site',
        content: BuildReplyTaskModelContent(messageText, ReplyTransactionTxt, replyBtn),
        template: '<div class="jconfirm white"><div class="jconfirm-bg"></div><div class="modal-dialog"><div class="modal-content"><div class="content" style="margin:0px"></div></div></div>',
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
            if ($('.reply-button-popup')) {
                $('.reply-button-popup').click(function (e) {
                    $(document).trigger("ReplyTaskButtonPopupClicked", '');
                });
            }
            if ($('.btn-close')) {
                $('.btn-close').click(function (e) {
                    $('.close-button-popup').click();
                });
            }
            //jQuery(function () {
            //    jQuery('.jconfirm-box').draggable({
            //        revert: false
            //    });
            //});
        },
    });
}

function ShowLoginPopup(messageText, OnCloseFunction) {
    $.confirm({
        title: false,
        cancelButton: __dialogCloseText,
        cancelButtonClass: 'btn-site',
        content: BuildLoginModelContent(messageText),
        template: '<div class="jconfirm white"><div class="jconfirm-bg"></div><div class="modal-dialog"><div class="modal-content"><div class="content" style="margin:0px"></div></div></div>',
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
            if ($('.btn-close')) {
                $('.btn-close').click(function (e) {
                    $('.close-button-popup').click();
                });
            }
           
        },
    });
}


function BuildRejectContent(messageText, RejectTransactionTxt, rejectBtn) {
    var rejectBtnHtml = '';
    if (rejectBtn && rejectBtn != '') {
        rejectBtnHtml = '<button class="reject-button-popup btn-site" data-dismiss="modal">' + rejectBtn + '</button>';
    }
    var modelHeader = '<div class="modal-header"></div>';
    var modelBody = '<div class="modal-body"><h4 class="site-color title4 text-center">' + messageText + '</h4><div class="row"><div class="col-xs-12 col-sm-10 col-sm-push-1"><fieldset><div class="form-group"><label>' + RejectTransactionTxt + '</label><div><textarea id="rejectionReason" class="form-control"></textarea></div></div><i id="CalMessageError" style="color:red" class="glyphicon glyphicon-exclamation-sign hidden_"></i><span id="errorMessageValidate" style="color:red;"></span></fieldset></div></div></div>';
    var modelFooter = '<div class="modal-footer actions-buttons">' + rejectBtnHtml + '<button class="close-button-popup btn-site" >الغاء</button>  </div>';
    return modelHeader + modelBody + modelFooter;
}


function ShowRejectTraPopup(messageText, OnCloseFunction, RejectTransactionTxt, rejectBtn) {
    $.confirm({
        title: false,
        cancelButton: __dialogCloseText,
        cancelButtonClass: 'btn-site',
        content: BuildRejectContent(messageText, RejectTransactionTxt, rejectBtn),
        template: '<div class="jconfirm white"><div class="jconfirm-bg"></div><div class="modal-dialog"><div class="modal-content"><div class="content" style="margin:0px"></div></div></div>',
        icon: 'fa fa-info',
        animation: 'scale',
        columnClass: 'alert_success alert_new col-md-6 col-md-offset-3 col-xs-6 col-xs-offset-3',
        backgroundDismiss: false,
        closeIcon: true,
        confirmButton: false,
        cancel: function () {
            if (OnCloseFunction != '' && OnCloseFunction != undefined) {
                window[OnCloseFunction]();
            }
        },
        onOpen: function () {
            if ($('.reject-button-popup')) {
                $('.reject-button-popup').click(function (e) {
                    $(document).trigger("RejectButtonPopupClicked", '');
                });
            }
            if ($('.btn-close')) {
                $('.btn-close').click(function (e) {
                    $('.close-button-popup').click();
                });
            }
            jQuery(function () {
                jQuery('.jconfirm-box').draggable({
                    revert: false
                });
            });
        },
    });
}
function ShowRejectTransactionPopup(messageText, OnCloseFunction, RejectTransactionTxt, DaysToCompleteTxt, rejectBtn) {
    $.confirm({
        title: false,
        cancelButton: __dialogCloseText,
        cancelButtonClass: 'btn-site',
        content: BuildRejectModelContent(messageText, RejectTransactionTxt, DaysToCompleteTxt, rejectBtn),
        template: '<div class="jconfirm white"><div class="jconfirm-bg"></div><div class="modal-dialog"><div class="modal-content"><div class="content" style="margin:0px"></div></div></div>',
        icon: 'fa fa-info',
        animation: 'scale',
        columnClass: 'alert_success alert_new col-md-6 col-md-offset-3 col-xs-6 col-xs-offset-3',
        backgroundDismiss: false,
        closeIcon: true,
        confirmButton: false,
        cancel: function () {
            if (OnCloseFunction != '' && OnCloseFunction != undefined) {
                window[OnCloseFunction]();
            }
        },
        onOpen: function () {
            if ($('.reject-button-popup')) {
                $('.reject-button-popup').click(function (e) {
                    $(document).trigger("RejectButtonPopupClicked", '');
                });
            }
            if ($('.btn-close')) {
                $('.btn-close').click(function (e) {
                    $('.close-button-popup').click();
                });
            }
            jQuery(function () {
                jQuery('.jconfirm-box').draggable({
                    revert: false
                });
            });
        },
    });
}

function continueSendTransaction() {
    
    isArchived = true
    $(document).trigger("MoveToAssignmenPopupClicked", '');

}

function ShowSaveTransSuccesMessage(messageText, OnCloseFunction, transNoTxt, transNoVal, transDateTxt, transDateVal, barcodeTxt, extraBtn1, extraBtn2, oldTransCopy, newTrans, PrintBtn, ReferralBtn, transactionId, ShowFollowUpControls, BackToBox, addArchiveButtonTxt, moveToAssignmentPaperButtonTxt) {

    __dialog = $.confirm({
        title: false,
        content: BuildModelContent(messageText, transNoTxt, transNoVal, transDateTxt, transDateVal, barcodeTxt, extraBtn1, extraBtn2, oldTransCopy, newTrans, PrintBtn, ReferralBtn, transactionId, ShowFollowUpControls, BackToBox, addArchiveButtonTxt, moveToAssignmentPaperButtonTxt),
        template: '<div class="jconfirm"><div class="jconfirm-bg"></div><div class="modal-dialog"><div class="modal-content"><div class="content" style="margin:0px"></div></div></div>',
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
            if ($('.barcode-button-popup')) {
                $('.barcode-button-popup').click(function (e) {
                    $(document).trigger("BarcodeButtonPopupClicked", '');
                });
            }
            if ($('.add-archive-button-popup')) {
                $('.add-archive-button-popup').click(function (e) {

                    $(document).trigger("AddArchiveButtonPopupClicked", '');
                });
            }
            if ($('.move-to-assignment-button-popup')) {
                $('.move-to-assignment-button-popup').click(function (e) {
                   
                    var docoNutFrameContent = $('#frameViewer').contents();
                    var __docViewerToken = $('#_hdnToken', docoNutFrameContent)[0].value;
                    var token = $("#dvArchivingMainDocumentBody").parents("form").find("input[name='__RequestVerificationToken']").val();

                    $.ajax({
                        type: 'post',
                        cache: false,
                        url: window.checkIfHasMainArchiveURL,
                        data: { param: __docViewerToken, __RequestVerificationToken: token },
                        async: false,
                        success: function (data) {
                            if (data.hasArchive.toLowerCase() == "false") {
                                isArchived = false
                                ShowConfirmMessage('المعاملة من غير ارشفة, هل ترغب في المتابعة؟', "continueSendTransaction")
                            } else {
                                isArchived = true
                                continueSendTransaction()
                            }
                        }
                    });
                });
            }
            if ($('.new-button-popup')) {
                $('.new-button-popup').click(function (e) {
                    $(document).trigger("NewButtonPopupClicked", '');
                });
            }
            if ($('.redirect-to-tray')) {
                $('.redirect-to-tray').click(function (e) {
                    $(document).trigger("RedirectToTrayButtonPopupClicked", '');
                });
            }
            if ($('.assign-button-popup')) {
                $('.assign-button-popup').click(function (e) {
                    $(document).trigger("AssignButtonPopupClicked", '');
                });
            }
            if ($('.print-ticket-button-popup')) {
                $('.print-ticket-button-popup').click(function (e) {
                    $(document).trigger("PrintTicketButtonPopupClicked", '');
                });
            }
            if ($('.print-address-button-popup')) {
                $('.print-address-button-popup').click(function (e) {
                    $(document).trigger("AddressButtonPopupClicked", '');
                });
            }
            if ($('.referral-button-popup')) {
                $('.referral-button-popup').click(function (event) {
                    $(document).trigger("ReferralButtonPopupClicked", '');
                });
            }
            if ($('.oldTrans-button-popup')) {
                $('.oldTrans-button-popup').click(function (event) {
                    $(document).trigger("GetPreviousPopupClicked", '');
                });
            }
            jQuery(function () {
                jQuery('.jconfirm-box').draggable({
                    revert: false
                });
            });
        },
    });
}

function BuildModelContent(messageText, transNoTxt, transNoVal, transDateTxt, transDateVal, barcodeTxt, sendBtn, printTicketBtn, oldTransCopy, newTrans, PrintAddressBtn, referralBtn, transactionId, ShowFollowUpControls, BackToBox, addArchiveButtonTxt, moveToAssignmentPaperButtonTxt) {

    var sendBtnHtml = '';
    if (sendBtn && sendBtn != '') {
        sendBtnHtml = '<button class="btn-site assign-button-popup">' + sendBtn + '</button>';
    }
    var printTicketBtnHtml = '';
    if (printTicketBtn && printTicketBtn != '') {
        printTicketBtnHtml = '<button class="btn-site print-ticket-button-popup col-lg-5 btn-act">' + printTicketBtn + '</button>';
    }
    var oldTransCopyHtml = '';
    if (oldTransCopy && oldTransCopy != '') {
        oldTransCopyHtml = '<button class="btn-site  oldTrans-button-popup">' + oldTransCopy + '</button>';
    }
    var PrintAddressBtnHtml = '';
    if (PrintAddressBtn && PrintAddressBtn != '') {
        PrintAddressBtnHtml = '<button class="btn-site print-address-button-popup col-lg-5 btn-act">' + PrintAddressBtn + '</button>';
    }
    var ReferralBtnHtml = '';
    if (referralBtn && referralBtn != '') {
        ReferralBtnHtml = '<button class="btn-site referral-button-popup" data-id="' + transactionId + '">' + referralBtn + '</button>';
    }
    var barcodeButtonHtml = '';
    if (barcodeTxt && barcodeTxt != '') {
        barcodeButtonHtml = '<button class="btn-site barcode-button-popup col-lg-5 btn-act">' + barcodeTxt + '</button>';
    }
    var addArchiveButtonButtonHtml = '';
    if (addArchiveButtonTxt && addArchiveButtonTxt != '') {
        addArchiveButtonButtonHtml = '<button class="btn-site add-archive-button-popup col-lg-5 btn-act">' + addArchiveButtonTxt + '</button>';
    }

    var moveToAssignmentPaperButtonTxtHtml = '';
    if (moveToAssignmentPaperButtonTxt && moveToAssignmentPaperButtonTxt != '') {
        moveToAssignmentPaperButtonTxtHtml = '<button class="btn-site move-to-assignment-button-popup col-lg-5 btn-act">' + moveToAssignmentPaperButtonTxt + '</button>';
    }

    var modelHeader = '<div class="add-archive-button-popup"><i class="fa fa-times mt-3" style="font-size:25px; position:absolute; left:17px; color:#a2a2a2"></i></div><div class=""><i class="la la-check-circle" style="text-align:center;display:grid;font-size:49px;padding-top: 14px;color:#198f19;"></i><h4 class="site-color title4 text-center">' + messageText + '</h2></div>';
    var modelBody = '<div class="modal-body" style="padding:15px"> <div class="row info"><div class="col-xs-4 text-center"><span class="light-text">' + transNoTxt + '</span><span class="indent-text"><b>' + transNoVal + '</b></span></div>';
    modelBody += '<div class="col-xs-8  col-sm-push-1  text-center"><span class="light-text">' + transDateTxt + '</span><span class="indent-text"><b>' + transDateVal + '</b></span></div></div>';
    if (ShowFollowUpControls) {
        modelBody += '<div class="row info"><div class="col-xs-5 text-center"><span class="light-text"><span class="indent-text"><input type="checkbox" class="chkboxFollowUp" id="chkboxFollowUp" /></span>متابعة المعاملة</span></div><div class="col-xs-7 text-right" ></div></div>';
    }
    modelBody += '<div class="row text-center actions-buttons">' + printTicketBtnHtml + barcodeButtonHtml + sendBtnHtml + ReferralBtnHtml + addArchiveButtonButtonHtml + moveToAssignmentPaperButtonTxtHtml + '</div></div>';
    var modelFooter = '<div class="actions-buttons" style="text-align:center"> ' + oldTransCopyHtml + '<button class="btn-site new-button-popup">' + newTrans + '</button><button class="btn-site redirect-to-tray">' + BackToBox + '</button></div></div></div>';

    return modelHeader + modelBody + modelFooter;
}

function ShowWarningMessage(messageText) {
    $.confirm({
        title: false,
        content: '<div><div class="mb-title"><span class="fa fa fa-warning"></span></div><p>' + messageText + '</p></div>',

        cancelButton: __dialogCloseText,
        cancelButtonClass: 'btn-site',

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

function ShowConfirmMessage(messageText, onConfirmFunction) {
    $.confirm({
        title: '',
        //content: '<div class="mb-title"><span class="fa fa-question"></span></div><p>' + messageText + '</p>',

        content: '<p>' + messageText + '</p>',
        template: '<div class="jconfirm"><div class="jconfirm-bg"></div><div class="modal-dialog"><div class="modal-content"><div class="modal-body" style="padding:15px"> <h4 class="site-color title4 text-center"><div class="content" style="margin:0px"></div></h2></div><div class="modal-footer actions-buttons buttons"></div></div></div>',
        cancelButton: __dialogCancelText,
        cancelButtonClass: 'btn-site',
        confirmButton: __dialogYesText,
        confirmButtonClass: 'btn-site',

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

function ShowPasswordValidatorPopup(messageText, OnCloseFunction, ReasonTxt, OkBtn) {
    $.confirm({
        title: false,
        cancelButton: __dialogCloseText,
        cancelButtonClass: 'btn-site',
        content: BuildPasswordModelContent(messageText, ReasonTxt, OkBtn),
        template: '<div class="jconfirm white"><div class="jconfirm-bg"></div><div class="modal-dialog"><div class="modal-content"><div class="content" style="margin:0px"></div></div></div>',
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
            if ($('.Ok-button-popup')) {
                $('.Ok-button-popup').click(function (e) {
                    $(document).trigger("OkButtonPopupClicked", '');
                });
            }
            if ($('.btn-close')) {
                $('.btn-close').click(function (e) {
                    $('.close-button-popup').click();
                });
            }
            jQuery(function () {
                jQuery('.jconfirm-box').draggable({
                    revert: false
                });
            });
        },
    });
}

function BuildPasswordModelContent(messageText, ReasonTxt, OkBtn) {
    var OkBtnHtml = '';
    if (OkBtn && OkBtn != '') {
        OkBtnHtml = '<button class="Ok-button-popup btn-site" data-dismiss="modal">' + OkBtn + '</button>';
    }
    var modelHeader = '<div class="modal-header"></div>';
    var modelBody = '<div class="modal-body"><h4 class="site-color title4 text-center">' + messageText + '</h4><div class="row"><div class="col-xs-12 col-sm-10 col-sm-push-1"><fieldset><div class="form-group"><label>' + ReasonTxt + '</label><div><input type="password" id="passwordTxt" class="form-control"></input></div></div><i id="CalMessageError" style="color:red" class="glyphicon glyphicon-exclamation-sign hidden_"></i><span id="errorMessageValidate" style="color:red;"></span></fieldset></div></div></div>';
    var modelFooter = '<div class="modal-footer actions-buttons">' + OkBtnHtml + '<button class="close-button-popup btn-site" >الغاء</button>  </div>';
    return modelHeader + modelBody + modelFooter;
}

$.fn.serializeObject = function () {
    var o = {};
    var a = this.serializeArray();
    var unique = {};
    var distinct = [];
    for (var i in a) {
        if (typeof (unique[a[i].name]) == "undefined") {
            distinct.push(a[i]);
        }
        unique[a[i].name] = 0;
    }

    $.each(distinct, function () {
        if (o[this.name] !== undefined) {
            if (!o[this.name].push) {
                o[this.name] = [o[this.name]];
            }
            o[this.name].push(this.value || '');
        } else {
            o[this.name] = this.value || '';
        }
    });
    return o;
};

function CheckIfInputHasPotentiallyDangerous() {
    var continueWork = true;
    //$("input.form-control:text,textarea").each(function () {
    //    var txt = $(this).val();
    //    if (txt != '' && (txt.indexOf('<') > -1 || txt.indexOf('>') > -1 || txt.indexOf('&') > -1 || txt.indexOf('\'') > -1 || txt.indexOf('\"') > -1)) {
    //        continueWork = false;
    //        return;
    //    }
    //});
    return continueWork;
}

function ShowPrinttingDivisionPopup(messageText, OnCloseFunction, PrinttingDivisionTxt, PrintBtn) {
    $.confirm({
        title: false,
        cancelButton: __dialogCloseText,
        cancelButtonClass: 'btn-site',
        content: BuildPrinttingDivisionModelContent(messageText, PrinttingDivisionTxt, PrintBtn),
        template: '<div class="jconfirm white"><div class="jconfirm-bg"></div><div class="modal-dialog"><div class="modal-content"><div class="content" style="margin:0px"></div></div></div>',
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
            if ($('.print-button-popup')) {
                $('.print-button-popup').click(function (e) {
                    $(document).trigger("DoPrintButtonPopupClicked", '');
                });
            }
            if ($('.btn-close')) {
                $('.btn-close').click(function (e) {
                    $('.close-button-popup').click();
                });
            }
            jQuery(function () {
                jQuery('.jconfirm-box').draggable({
                    revert: false
                });
            });
        },
    });
}
function BuildPrinttingDivisionModelContent(messageText, PrinttingDivisionTxt, PrintBtn) {
    var printBtnHtml = '';
    if (PrintBtn && PrintBtn != '') {
        printBtnHtml = '<button class="print-button-popup btn-site" data-dismiss="modal">' + PrintBtn + '</button>';
    }
    var modelHeader = '<div class="modal-header"></div>';
    var modelBody = '<div class="modal-body"><h4 class="site-color title4 text-center">' + messageText + '</h4><div class="row"><div class="col-xs-12 col-sm-10 col-sm-push-1"><fieldset><div class="form-group"><label>' + PrinttingDivisionTxt + '</label><div><select name="DivisionsList" id="DivisionsList" class="form-control"><option value="1">1</option><option value="2">2</option><option value="3">3</option><option value="4">4</option></select></div></div><i id="CalMessageError" style="color:red" class="glyphicon glyphicon-exclamation-sign hidden_"></i><span id="errorMessageValidate" style="color:red;"></span></fieldset></div></div></div>';
    var modelFooter = '<div class="modal-footer actions-buttons">' + printBtnHtml + '<button class="close-button-popup btn-site" >الغاء</button>  </div>';
    return modelHeader + modelBody + modelFooter;
}
function ShowFollowUpNote(NoteTxt) {
    $.confirm({
        title: false,
        content: '<textarea class="form-control long-textarea valid" cols="20"  id="txtAreaFollowUpNote" maxlength="255" name="txtAreaFollowUpNote" rows="2" disabled="true">' + NoteTxt + '</textarea>',
        cancelButton: __dialogCloseText,
        cancelButtonClass: 'btn-site',
        icon: 'fa fa-info',
        animation: 'scale',
        columnClass: 'alert_danger alert_new col-md-6 col-md-offset-3 col-xs-6 col-xs-offset-3',
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
function ShowCustomConfirmMessage(messageText, onConfirmFunction, parameter) {
    $.confirm({
        title: '',
        //content: '<div class="mb-title"><span class="fa fa-question"></span></div><p>' + messageText + '</p>',

        content: '<p>' + messageText + '</p>',
        template: '<div class="jconfirm"><div class="jconfirm-bg"></div><div class="modal-dialog"><div class="modal-content"><div class="modal-body" style="padding:15px"> <h4 class="site-color title4 text-center"><div class="content" style="margin:0px"></div></h2></div><div class="modal-footer actions-buttons buttons"></div></div></div>',
        cancelButton: __dialogCancelText,
        cancelButtonClass: 'btn-site',
        confirmButton: __dialogYesText,
        confirmButtonClass: 'btn-site',

        animation: 'scale',
        columnClass: 'alert_normal alert_new col-md-6 col-md-offset-3 col-xs-6 col-xs-offset-3',
        backgroundDismiss: false,
        closeIcon: false,
        confirm: function () {
            if (onConfirmFunction != '' && onConfirmFunction != undefined) {
                window[onConfirmFunction](parameter);
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
function ShowCustomConfirmationMessage(messageText, onConfirmFunction, parameter1, parameter2) {
    $.confirm({
        title: '',
        //content: '<div class="mb-title"><span class="fa fa-question"></span></div><p>' + messageText + '</p>',

        content: '<p>' + messageText + '</p>',
        template: '<div class="jconfirm"><div class="jconfirm-bg"></div><div class="modal-dialog"><div class="modal-content"><div class="modal-body" style="padding:15px"> <h4 class="site-color title4 text-center"><div class="content" style="margin:0px"></div></h2></div><div class="modal-footer actions-buttons buttons"></div></div></div>',
        cancelButton: __dialogCancelText,
        cancelButtonClass: 'btn-site',
        confirmButton: __dialogYesText,
        confirmButtonClass: 'btn-site',

        animation: 'scale',
        columnClass: 'alert_normal alert_new col-md-6 col-md-offset-3 col-xs-6 col-xs-offset-3',
        backgroundDismiss: false,
        closeIcon: false,
        confirm: function () {
            if (onConfirmFunction != '' && onConfirmFunction != undefined) {
                window[onConfirmFunction](parameter1, parameter2);
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
function ShowCustomCloseConfirmMessage(messageText, onConfirmFunction, parameter, onCloseFunction, closeParameter) {
    $.confirm({
        title: '',
        //content: '<div class="mb-title"><span class="fa fa-question"></span></div><p>' + messageText + '</p>',

        content: '<p>' + messageText + '</p>',
        template: '<div class="jconfirm"><div class="jconfirm-bg"></div><div class="modal-dialog"><div class="modal-content"><div class="modal-body" style="padding:15px"> <h4 class="site-color title4 text-center"><div class="content" style="margin:0px"></div></h2></div><div class="modal-footer actions-buttons buttons"></div></div></div>',
        cancelButton: __dialogCancelText,
        cancelButtonClass: 'btn-site',
        confirmButton: __dialogYesText,
        confirmButtonClass: 'btn-site',

        animation: 'scale',
        columnClass: 'alert_normal alert_new col-md-6 col-md-offset-3 col-xs-6 col-xs-offset-3',
        backgroundDismiss: false,
        closeIcon: false,
        confirm: function () {
            if (onConfirmFunction != '' && onConfirmFunction != undefined) {
                window[onConfirmFunction](parameter);
            }
        },
        cancel: function () {
            $("#Loader").hide();
            if (onCloseFunction != '' && onCloseFunction != undefined) {
                window[onCloseFunction](closeParameter);
            }
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
function EscapeSpecialCharacters(str) {
    var specialCharactersFormat = /[!@$%^&*()+\-=\[\]{};':"\\|,.<>\/?]/;
    var escapedStr = '';
    for (var i = 0; i < str.length; i++) {
        if (specialCharactersFormat.test(str.charAt(i))) {
            escapedStr += '\\\\' + str.charAt(i);
        }
        else {
            escapedStr += str.charAt(i);
        }
    }
    return escapedStr;
}
function ShowEncrypttionDialog(data, dialogClass) {

    dialogClass = dialogClass || 'dialogbox col-md-8 col-md-offset-2 col-xs-8 col-xs-offset-2';

    __dialog = $.confirm({
        title: false,
        content: data,
        icon: 'fa fa-info',
        animation: 'scale',
        columnClass: dialogClass,
        backgroundDismiss: false,
        cancelButton: false,
        confirmButton: false,
        closeIcon: true,
        onOpen: function () {
            $("#Loader").hide();
        },
    });
};

function ShowCommitmentDialog(data, dialogClass) {

    dialogClass = dialogClass || 'dialogbox col-md-8 col-md-offset-2 col-xs-8 col-xs-offset-2';

    __dialog = $.confirm({
        title: false,
        content: data,
        icon: 'fa fa-info',
        animation: 'scale',
        columnClass: dialogClass,
        backgroundDismiss: false,
        cancelButton: false,
        confirmButton: false,
        closeIcon: false,
        onOpen: function () {
            $("#Loader").hide();
        },
    });
};
function ShowConfidentialityAcknowledgmentMassage(ConfidentialityAcknowledgmentList, onConfirmFunction, onCancelFunction) {
 
    $.confirm({
        title: '',
        //content: '<div class="mb-title"><span class="fa fa-question"></span></div><p>' + messageText + '</p>',
        //'<p>' + messageText + '</p>'
        content: buildConfidentialityAcknowledgmentContent(ConfidentialityAcknowledgmentList),
        template: '<div class="jconfirm"><div class="jconfirm-bg"></div><div class="modal-dialog"><div class="modal-content" style=width:700px;><div class="modal-body" style="padding:15px"> <h4 class="site-color title4 "><div class="content" style="margin:0px"></div></h2></div><div class="modal-footer actions-buttons buttons"></div></div></div>',
        cancelButton: __dialogCancelText,
        cancelButtonClass: 'btn-site',
        confirmButton: __dialogYesText,
        confirmButtonClass: 'btn-site acknowledged-btn',
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
            if (onCancelFunction != '' && onCancelFunction != undefined) {
                window[onCancelFunction]();
            }
       

        },
        onOpen: function () {
         
            $('.acknowledged-btn').prop('disabled', 'disabled');  
            $(".acknowledged-btn").css('background-color', 'gray');
            jQuery(function () {
                jQuery('.jconfirm-box').draggable({
                    revert: false,
                });
            });
        },

    });
}
function buildConfidentialityAcknowledgmentContent(ConfidentialityAcknowledgmentList) {
  
    var HtmlBody = ''; 

    for (var i = 0; i < ConfidentialityAcknowledgmentList.length; i++) {
        if (ConfidentialityAcknowledgmentList[i].parameters[0] == true) {

            HtmlBody += '<div class="form-group">   ' +
                '<input id=chkTable' + ConfidentialityAcknowledgmentList[i].value + ' type = "checkbox" class="requiredacknowledged" required onclick = "CheckboxAcknowledged();" name = "chkTable' + ConfidentialityAcknowledgmentList[i].value + '" value = ' + ConfidentialityAcknowledgmentList[i].value + ' > ' +
               '<span class="stark">*</span>'+ '<label>' + ConfidentialityAcknowledgmentList[i].label + '</label></div>'
                ;

        } else {

            HtmlBody += '<div class="form-group">   ' +
                '<input id=chkTable' + ConfidentialityAcknowledgmentList[i].value + ' type = "checkbox" class="" onclick = "CheckboxAcknowledged();" name = "chkTable' + ConfidentialityAcknowledgmentList[i].value + '" value = ' + ConfidentialityAcknowledgmentList[i].value + ' > ' +
                '<label>' + ConfidentialityAcknowledgmentList[i].label + '</label></div>'
                ;  
        }  
    }  
    var modelHeader = '<div class="modal-header">الرجاء الاقرار  على المعلومات التالية</div> ';
 
    var modelFooter = '';
    return modelHeader + HtmlBody + modelFooter;




}
 
