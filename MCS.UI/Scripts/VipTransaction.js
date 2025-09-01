
$(document).ready(function ()
{
    $('html, body').animate({
        scrollTop: $('#ArchivingPartial').offset().top
    }, 'slow');
    GetCertificateAssignments();
});

function CollapseDiv(divName)
{
    if ($('#' + divName).css('display') == 'block')
    {
        $('#' + divName).css("display", "none");
        $('#' + divName + 'PlusIcon').css("display", "inline-block");
        $('#' + divName + 'DashIcon').css("display", "none");
    }
    else
    {
        $('#' + divName).css("display", "block");
        $('#' + divName + 'PlusIcon').css("display", "none");
        $('#' + divName + 'DashIcon').css("display", "inline-block");
    }
}

function checkDocument()
{
    return true;
}
$("div.anchor a").click(function ()
{
    $("div.anchor a").removeClass("active");
    $("div.anchor a").addClass("disabled");
    $(this).addClass("active");
    var id = $(this).data("id");
    $("#" + id).addClass("active");
    $(".tabscontent").addClass("hidden_");

    var divId = $(this).data("divid");
    $("#" + divId).removeClass("hidden_");

    if (divId == "ten-tab" || divId == "second-tab" || divId == "twelve-tab")
    {
        $('#dvButtonFooter').hide();
    }
    else
    {
        $('#dvButtonFooter').show();
    }

    if ($(this).data("id") == 'outboundDraft')
    {
        GetOutboundDraft();
    }
});

function GetMode()
{
    return EncryptionKey(archiveMode).toString();
}



function ViewGridRow(id, documentId, key)
{
    debugger;
    var url = showDocumentViewerUrl + EncryptionKey(documentId) + "&documentSessionKey=" + id;
    ShowDialog(url);

}


function AssignBack()
{

    var note = $("#ctl00_MainPlaceHolder_txtData").val();
    var token = $("input[name='__RequestVerificationToken']").val();

    $.ajax({
        type: 'post',
        cache: false,
        url: '@Url.Action("AssignItBack", "VipInbound")',
        data: {
            TransId: $("#InboundId").val(), Notes: note, __RequestVerificationToken: token
        },
        success: function (data)
        {
            OnSuccess(data);
        }
    });
}


$("ul li a").click(function ()
{
    var tabId = $(this).attr("aria-controls");

    if (tabId != undefined && tabId.trim() != '')
    {
        $('ul li').removeClass('active');
        $(this).parent().addClass("active");
        $(".tab-pane").hide();
        $("#" + tabId).show();
        if (tabId == "ten-tab" || tabId == "EditorAssignmentForm" || tabId == "twelve-tab")
        {
            $('#dvButtonFooter').hide();
        }
        else
        {
            $('#dvButtonFooter').show();
        }
    }
});
function checkboxSelect(control)
{

    var lastNumber = $(control).attr("id").toString().split("_")[1];

    if (control.checked)
    {
        $('#AssignmentPaperMainSource_' + lastNumber).prop('checked', false);
        $("#IsCopy_" + lastNumber).val(true);
        $("#IsAssigned_" + lastNumber).val(false);
    }
}
function DisplaySpecificTab(element, isAnchor)
{
    return;
    var tabId = $(element).find('a').attr('aria-controls');

    if (isAnchor)
    {
        tabId = $(element).attr('aria-controls');
        $('.flex-fill').removeClass('active');
    }
    if (tabId != undefined && tabId != '')
    {
        $('.tab-pane').hide();
        $('#' + tabId).show();

        if (tabId == "ten-tab" || tabId == "EditorAssignmentForm" || tabId == "twelve-tab")
        {
            $('#dvButtonFooter').hide();
        }
        else
        {
            $('#dvButtonFooter').show();
        }
    }
}
function radioSelect(control)
{



    $(".repRadio").each(function ()
    {
        var lastNumber = $(this).attr("id").toString().split("_")[1];
        $("#IsAssigned_" + lastNumber).val(false);
        $(this).prop('checked', false);

    });

    $('#' + control.id).prop('checked', true);

    var lastNumber = $(control).attr("id").toString().split("_")[1];

    if (control.checked)
    {
        $('#MainPlaceHolderAssignmentPaperCopy_' + lastNumber).prop('checked', false);

        $("#IsAssigned_" + lastNumber).val(true);
        $("#IsCopy_" + lastNumber).val(false);
    }
}
function DirectionTypeChange(value)
{

    var vAgree = $('#ctl00_MainPlaceHolder_chkAgree')[0];
    var vNotAgree = $('#ctl00_MainPlaceHolder_chkNotAgree')[0];
    var vViewed = $('#ctl00_MainPlaceHolder_chkAcknowledge')[0];


    var vAgreeValue = $("label[for='ctl00_MainPlaceHolder_chkAgree']")[0].innerHTML;
    var vNotAgreeValue = $("label[for='ctl00_MainPlaceHolder_chkNotAgree']")[0].innerHTML;
    var vViewedValue = $("label[for='ctl00_MainPlaceHolder_chkAcknowledge']")[0].innerHTML;


    var vData = $('#ctl00_MainPlaceHolder_txtData');

    switch (value)
    {
        case 1:
            if (vAgree.checked)
            {
                var dataToRemove = vData.val();
                dataToRemove = dataToRemove.replace(vNotAgreeValue, "");
                vData.val(dataToRemove);
                var vDataValue = vData.val();
                vDataValue = vDataValue + ' ' + vAgreeValue;
                vData.val(vDataValue);
                vNotAgree.checked = false;
            } else
            {
                var dataToRemove = vData.val();
                dataToRemove = dataToRemove.replace(vAgreeValue, "");
                vData.val(dataToRemove);
            }
            break;
        case 2:
            if (vNotAgree.checked)
            {
                var dataToRemove = vData.val();
                dataToRemove = dataToRemove.replace(vAgreeValue, "");
                vData.val(dataToRemove);
                var vDataValue = vData.val();
                vDataValue = vDataValue + ' ' + vNotAgreeValue;
                vData.val(vDataValue);
                vAgree.checked = false;
            }
            else
            {
                var dataToRemove = vData.val();
                dataToRemove = dataToRemove.replace(vNotAgreeValue, "");
                vData.val(dataToRemove);
            }
            break;
        case 3:

            if (vViewed.checked)
            {
                var vDataValue = vData.val();
                vDataValue = vDataValue + ' ' + vViewedValue;
                vData.val(vDataValue);

            }
            else
            {
                var dataToRemove = vData.val();
                dataToRemove = dataToRemove.replace(vViewedValue, "");
                vData.val(dataToRemove);
            }
            break;


    }


}


function NotValid()
{
    $('#dvBasicInfo input').each(function ()
    {

        if ($('#dvBasicInfo input').hasClass('invalid-input'))
        {

            $('div, a').removeClass('selected');

            if (!$('#first').hasClass('selected'))
                $('#first').addClass('active');

            $(".tabscontent").addClass("hidden_");
            $("#first-tab").removeClass("hidden_");

            $(".boardered-circle-transparent").removeClass("active");
            $('#first').addClass('active');

            $(window).scrollTop(0);
        }
    });
}

function redirect()
{
    setTimeout(function ()
    {
        window.location.href = '@Url.Action("MyTransactions", "File", new { Area = "user" })';
    }, 2000);
}

function OnSuccess(data)
{


    switch (data.MessageType)
    {
        case 0:
            goToNext(data);
            break;
        case 1:
            return ShowConfirmMessage(data.MessageText, "ContinueAssigneTransaction");
            break;

        default:
            return ShowErrorMessage(data.MessageText);

    }
}

function ContinueAssigneTransaction()
{
    $("#isConfirmed").val(true);
    $("#SaveBtn").click();

}

function ValidateSendForm()
{
    var valid = false;

    // checkIfHasMainDocument(true);

    $(".repRadio").each(function ()
    {
        var lastNumber = $(this).attr("id").toString().split("_")[1];
        if ($("#IsAssigned_" + lastNumber).val() == "true")
        {
            valid = true;
            return false;
        }
    });
    var isValidAgree = true;


    return (valid && isValidAgree && documentValidationResult && priorityValidationResult);

}

$(document).on("RedirectToTrayButtonPopupClicked", function (event, param)
{
    window.location.href = '@Url.Action("MyTransactions", "File")';
});
$(document).on("AssignButtonPopupClicked", function (event, param)
{
    assignTransAjax();
});



function ReturnToMyTransactionTray()
{
    window.location.href = '@Url.Action("MyTransactions", "File")';
}

function GetCertificateAssignments()
{


    var id = $("#Id").val();
    $.ajax({
        type: 'Get',
        cache: false,
        url: assignmentHistoryUrl,
        data: { transactionId: id },
        success:
            function (data)
            {

                $("#dvContentAssignmement").html("");
                $("#dvContentAssignmement").html(data);
            }
    });
}

function OpenCloseEditModeDoconut(isOpen) {
    debugger;

    var iframe = document.getElementById("frameViewer");
    var elmnt = iframe.contentWindow.document.getElementById("docViewerAnnotations");
    if (elmnt == null) {
        return true;
    }
    if ((elmnt.style.display == "none" && isOpen == true) || (elmnt.style.display != "none" && isOpen == false)) {
        document.getElementById('frameViewer').contentWindow.StartAnnotation();

    }



    return true;

}
function DownloadFile(id)
{
    var url = downloadDocumentUrl + EncryptionKey(id);
    window.open(url, '_blank');
    $("#Loader").hide();

}