function PreviewTransactionDocument(transactionId)
{

    var token = $("input[name='__RequestVerificationToken']").val();
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
                var url = showDocumentURL + '?documentId=' + documentId + "&documentSessionKey=";
                ShowDialog(url);
            }
            else
            {
                RedirectToTargetPage();
                //ShowErrorMessage(data.MessageText);
            }
        }
    });

}
function ShowTransactionEditorLink()
{
    debugger
    $("#dvUserTransactionEditorArchiving").removeClass('d-flex');
    /*    $("#dvUserTransactionEditorLink").addClass('d-flex');*/
    $("#dvUserTransactionEditorExplain").removeClass('d-flex');
    $("#DivtxtControls").hide();

    var transactionId = $("#Id").val();
    var trx = EncryptionKey(transactionId);
    var url = viewLinkUrl + '?transactionId=' + trx;
    ShowDialog(url, 'dialogbox col-md-12 ');


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
                            //if (AttachmentSource == @((int)AttachmentSource.Scanned)) {
                            $('#dvexplanationsText').hide();
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