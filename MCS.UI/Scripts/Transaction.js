function UpdateMenuFileInformation(url)
{
    $.ajax({
        url: url,
        type: 'Get',
        cache: false,
        success: function (data)
        {
            if (data.MessageType != 2)
            {
                $(".main-menu-content").html("");
                $(".main-menu-content").html(data.FileMenuHtml);
            }
            else
            {
                ShowErrorMessage(data.MessageText);
            }
        },
        error: function (a)
        {
        }
    });

    return true;
}

function EditURL(url, id)
{
    url = url + '?id=' + EncryptionKey(id);
    window.location.href = url;
}

function UpdateIsBCC(obj)
{
    debugger;
    var objId = obj.id;
    var hdnId = objId.replace("ch_", "hdn_");
    if (obj.checked)
    {
        $('#' + hdnId).val(true);
    } else
    {
        $('#' + hdnId).val(false);
    }
}
function GetMode()
{
    return EncryptionKey(archiveMode).toString();
}
function ShowTextData(id, documentId, key)
{
    debugger;
    $.ajax({
        type: 'get',
        cache: false,
        url: 'GetExplanationByDocumentId',
        data: { id: documentId, hdnExplanationDocumentSessionKey: key },
        success: function (data)
        {
            debugger;

            if (data.Type == 3)
            {
                debugger;
                $("#archivetxtDescription").val(data.Content);
                document.getElementById('explinationDateTime').innerText = data.Date;
                document.getElementById('explinationFromUser').innerText = data.FromUser;
                $('#dvexplanationsText').show();
                $('.doconutContainerForArchiving').hide();
                $('#dvScanningArchiving').hide();
            }
        }
    });
}

function ShowArchiviableData(id, documentId, key)
{
    var token = $('form:first').find("input[name='__RequestVerificationToken']").val();

    var sessionInput = { isEditMode: GetMode(), __RequestVerificationToken: token };
    $.ajax({
        type: "POST",
        url: urlSession,
        cache: false,
        data: sessionInput,
        success: function (sessionData)
        {
            if (sessionData.MessageType != 2)
            {
                var AttachmentSource = $('#dvArchivingGrid').find('#AttachmentSource_' + key).val();
                var data = { key: id, documentId: documentId, __RequestVerificationToken: token };
                $.ajax({
                    type: "POST",
                    url: archiveURL,
                    cache: false,
                    data: data,
                    success: function (data)
                    {


                        if (data.MessageType !== 2)
                        {
                            $('#dvexplanationsText').hide();
                            //if (AttachmentSource == @((int)AttachmentSource.Scanned)) {
                            $('#dvScanningArchiving').show();
                            $('.doconutContainerForArchiving').hide();
                            document.getElementById('frameViewerAttachmentForArchiving').contentWindow.location.reload();
                            //}
                        }
                    }
                });
            }
        }
    });




}

function MoveToAssignmenTransaction()
{
    $(document).trigger("MoveToAssignmenPopupClicked", '');
}

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


function PreviewTransactionDocumentForLink(transactionId)
{
    $.ajax({
        type: 'get',
        url: perviweURL,
        data: { transactionId: transactionId, __RequestVerificationToken: token },
        success: function (data)
        {
            debugger
            if (data.MessageType != 2)
            {

                var documentId = EncryptionKey(data.DocumentDTOResult.Id).toString();
                ShowArchiviableData(transactionId, documentId, "");


            }
            else
            {
                RedirectToTargetPage();
            }
        }
    });

}


function ViewGridRow(id, documentId, key)
{
    debugger;
    var url = showDocumentViewerUrl + EncryptionKey(documentId) + "&documentSessionKey=" + id;
    ShowDialog(url);

}



function AppendArchives()
{
    var linkCount = 0;
    var frameViewerAttachmentUrl = $('#frameViewerAttachment').prop('src');

    $("#frameViewerAttachmentForArchiving").attr("src", frameViewerAttachmentUrl);

    $("input[id^='IdArchiving_']").each(function (count)
    {
        let text = $(this).attr('id');
        debugger;
        count++;
        const myArray = text.split("_");
        var IdArchiving = myArray[1];
        var documentId = myArray[2];
        var key = myArray[3]
        var headerText = myArray[5];
        var IsArchivalbe = myArray[4]
        if ((IsArchivalbe == "True" || IsArchivalbe == "true") && documentId > 0)
        {
            $("#haveArchiving").addClass("Archivebadget");
            $("#haveArchiving").text(count);
            $("#dvUserTransactionEditorArchiving").append('<div onclick="return ViewGridRow(' + IdArchiving + ',' + documentId + ',' + key + ')" class="dv"><h5>' + headerText + ' </h5></div>');
        }
        count++;
    });

    $("input[id^='IdLinkedForMain_']").each(function (count)
    {
        debugger;
        count++;
        let text = $(this).attr('id');
        const myArray = text.split("_");
        var TransId = myArray[2];
        var TransNumber = myArray[3];
        var hasPermission = myArray[4];
        var headerTextLink = " ربط برقم" + TransNumber;
        if (hasPermission == "True")
        {
            $("#haveLink").addClass("Archivebadget");
            $("#haveLink").text(count);
            $("#dvUserTransactionEditorLink").append('<div onclick="return PreviewTransactionDocumentForLink(' + TransId + ')" class="dv"><h5>' + headerTextLink + ' </h5></div>');

        } else
        {
            $("#IncludeArchivingSection").append('<div  class="dv"><h5>' + headerTextLink + ' </h5></div>');

        }
        count++;
        linkCount = count;
    });
    if (linkCount > 0)
    {
        $('#dvGetLinks').attr('onclick', 'ShowTransactionEditorLink()');
    }
}
$(document).on("click", "#UserTransactionEditorSelectScannerForArchiving", function ()
{
    $(".doconutContainerForArchiving").show();
    $('#dvScanningArchiving').hide();
});


function ShowTransactionEditorLink()
{
    debugger
    $("#dvUserTransactionEditorArchiving").removeClass('d-flex');
    /*    $("#dvUserTransactionEditorLink").addClass('d-flex');*/
    $("#dvUserTransactionEditorExplain").removeClass('d-flex');
    $("#DivtxtControls").hide();

    var transactionId = $("#hdnTransactionId").val();
    var trx = EncryptionKey(transactionId);
    var url = viewLinkUrl + '?transactionId=' + trx;
    ShowDialog(url, 'dialogbox col-md-12 ');


}

function OpenCloseEditModeDoconut(isOpen)
{
    debugger;

    var iframe = document.getElementById("frameViewer");
    var elmnt = iframe.contentWindow.document.getElementById("docViewerAnnotations");
    if (elmnt == null) {
        return true;
    }
    if ((elmnt.style.display == "none" && isOpen == true) || (elmnt.style.display != "none" && isOpen == false))
    {
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