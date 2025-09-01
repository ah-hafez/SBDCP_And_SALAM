// GLOBAL

var annotationsSaved = false;
var annotationsChanged = false;
var isLocal = false;
var rotateValue = 0;
if (!window.location.href.includes('localhost') && !window.location.href.toLowerCase().includes('/mcs'))
{
    basePath = '';
}

if (window.location.href.includes('localhost'))
{
    isLocal = true;
}


function StartAnnotation()
{
    var annotationsDivId = $(objctlDoc)[0].id;
    if (!annMode)
    {
        OpenAnnotation();
    }
    else
    {
        //$('#' + annotationsDivId + ' .btnAnnSave').click();
        $('#' + annotationsDivId + ' .btnAnnCancel').click();
    }

    $('.docThumb').click(function ()
    {
        if (annMode)
        {
            $('button.Edit').click()
            objctlDoc.ShowPage($(this).val());
            $('button.Edit').click();
        }
    })
}

$('.AnnotationButton').bind('click', function ()
{
    var annotationButton = $(this);
    var annotationTitle = annotationButton.data('title');
    var titleLength = annotationTitle.length;
    if (titleLength > 0 && annMode)
    {
        SelectAnnType(annotationTitle);
    }
});

function SelectAnnType(type)
{
    if (null != objctlDoc.AnnotationController())
    {
        objctlDoc.AnnotationController().annotationType = type;
    }
}

function OpenAnnotation()
{
    annMode = true;
    if (globalToken === "")
    {
        alert("Please open a document first. Then click Annotate.");
        return;
    }

    var zoom = objctlDoc.CurrentZoom();
    objctlDoc.ShowAnnotations(parseInt(zoom), true);

    var annotationsDivId = $(objctlDoc)[0].id;
    $('#' + annotationsDivId + '_divAnn').addClass('fix');

    $("#navDefault").hide();
    $("#navAnnotation").show();
    EditModeToggle(true);
}

function CloseAnnotation()
{
    var returnClose = false;

    if (annotationsChanged === true && annotationsSaved === false)
    {
        var doExit = confirm('Exit without saving?');
        if (doExit === true)
        {
            objctlDoc.CloseAnnotations(false);
            returnClose = true;
        }
    }
    else
    {
        objctlDoc.CloseAnnotations(annotationsSaved);
        returnClose = true;
    }

    return returnClose;
}
function RotateDocument()
{
    if (rotateValue > 3)
    {
        rotateValue = 0;
    } else
    {
        rotateValue = rotateValue + 1;
    }
    objctlDoc.Rotate(objctlDoc.CurrentPage(), rotateValue);
}

function EditModeToggle(display)
{
    if (display)
    {
        $('#docViewerAnnotations').show();
        $('#btnAddSign').show();
        $('#btnAddStamp').show();
        $('#btnAddBarcode').show();
        $('#btnDeleteAnnotation').show();
        $('#btnClearAnnotaions').show();
        $('#btnTextNote').show();
    }
    else
    {
        $('#docViewerAnnotations').hide();
        $('#btnAddSign').hide();
        $('#btnAddStamp').hide();
        $('#btnAddBarcode').hide();
        $('#btnDeleteAnnotation').hide();
        $('#btnClearAnnotaions').hide();
        $('#btnTextNote').hide();
    }
}

function ExportAnnotation()
{

    loader.show();

    $.ajax({
        type: "POST",
        cache: false,
        async: true,
        url: "/" + basePath + "/Doconut/ExportAnnotations?token=" + globalToken,
        success: function (data)
        {
            if (data.indexOf("error") > -1)
            {
                alert("Error exporting, " + data);
            } else
            {
                window.open("/" + basePath + "/files/" + data);
            }

            loader.hide();
        },
        error: function (textStatus, errorThrown, data)
        {
            alert("Error exporting to pdf");

            loader.hide();
        }
    });

}

function DeleteAnnotations()
{
    if (null != objctlDoc.AnnotationController())
    {
        objctlDoc.AnnotationController().DeleteAnnotation(objctlDoc.AnnotationController().GetSelectedAnnotation());
    }
}



function ClearAnnotations()
{
    if (null != objctlDoc.AnnotationController())
    {
        objctlDoc.AnnotationController().ClearAnnotations();
    }
}

function ClearAnnotationData()
{
    $('#annlineVertical').val("");
    $('#annarrowDirection').val("");

    $('#annId').val("");
    $('#annType').val("");
    $('#annLeft').val("");
    $('#annTop').val("");
    $('#annWidth').val("");
    $('#annHeight').val("");

    $('#annborderColor').val("");
    $('#annbackColor').val("");
    $('#annborderWidth').val("");

    $('#annOpactiy').val("");
    $('#annborderWidth').val("");
    $('#annshowBorder').val("");

    $('#annRotate').val("");
    $('#annCanRotate').val("");

    $('#annTitle').val("");
    $('#annshowTitle').val("");

    $('#anntitleColor').val("");
    $('#anntitleFontSize').val("");

    $('#annNote').val("");
    $('#annshowNote').val("");

    $('#annBurn').val("");
    $('#annLocked').val("");

    $('#annzindex').val("");
    $('#annBase64').val("");

    $('#anntextAlign').val("");
    $('#annAuthor').val("");
}

function SetDirty(isDirty)
{
    if (isDirty)
    {
        annotationsSaved = !isDirty;
    }
    annotationsChanged = isDirty;
}

function ExportXml()
{

    loader.show();

    $.ajax({
        type: "POST",
        cache: false,
        async: true,
        url: "/" + basePath + "/Doconut/ExportXml?token=" + globalToken,
        success: function (data)
        {
            if (data.indexOf("error") > -1)
            {
                alert("Error exporting, " + data);
            } else
            {
                window.open("/" + basePath + "/files/" + data);
            }

            loader.hide();
        },
        error: function (textStatus, errorThrown, data)
        {
            alert("Error exporting to xml");

            loader.hide();
        }
    });

}

function PrintDocument()
{
    var isSuccess = false;
    var transactionId = 0;
    var isWithoutWatermark = true;
    $.ajax({
        type: "GET",
        url: "/" + basePath + "/Doconut/GetTransactionId",
        success: function (data)
        {
            if (data != null)
            {
                transactionId = data.transactionId;
                isWithoutWatermark = data.printWithoutWatermark
            }
            if (isWithoutWatermark)
            {
                if (transactionId > 0)
                {
                    var baseProPath = "/" + basePath + "/User/Shared/LogPrintWithoutWatermark?transactionId=" + transactionId;
                    var logUrl = urlScheme + "://" + host + "/" + basePath + "/User/Shared/LogPrintWithoutWatermark?transactionId=" + transactionId;
                    if (!isLocal)
                    {
                        logUrl = baseProPath;
                    }

                    $.ajax({
                        type: "GET",
                        url: logUrl,
                        success: function (result)
                        {

                            if (result.MessageType == 0)
                            {
                                $.ajax({
                                    type: "GET",
                                    url: "/" + basePath + "/Doconut/AddWatermark?token=" + globalToken,
                                    success: function (token)
                                    {

                                        var link = "/" + basePath + "/Doconut/Print?token=" + token + "&basePath=" + basePath + "&printtotal=" + objctlDoc.TotalPages() + "&KeepThis=false&TB_iframe=true&height=150&width=300";
                                        window.open(link);


                                    },
                                    error: function (textStatus, errorThrown, data)
                                    {
                                        alert("Error exporting to pdf");

                                        loader.hide();
                                    }
                                });
                            } else
                            {
                                alert("Error exporting to pdf");

                                loader.hide();
                            }

                        },
                        error: function (textStatus, errorThrown, data)
                        {
                            alert("Error exporting to pdf");

                            loader.hide();
                        }
                    });
                }


            } else
            {
                $.ajax({
                    type: "GET",
                    url: "/" + basePath + "/Doconut/AddWatermark?token=" + globalToken,
                    success: function (token)
                    {

                        var link = "/" + basePath + "/Doconut/Print?token=" + token + "&basePath=" + basePath + "&printtotal=" + objctlDoc.TotalPages() + "&KeepThis=false&TB_iframe=true&height=150&width=300";
                        window.open(link);


                    },
                    error: function (textStatus, errorThrown, data)
                    {
                        alert("Error exporting to pdf");

                        loader.hide();
                    }
                });

            }




        }

    });



}

function DownloadDocument(token)
{
    var link = "/" + basePath + "/Doconut/DownloadFile?token=" + token;
    window.open(link);
}

function ctlDoc_AnnClosed()
{
    $("#navDefault").show();
    $("#navAnnotation").hide();
    EditModeToggle(false);

    annMode = false;
    SetDirty(false);
}

function ctlDoc_Deleted()
{
    ClearAnnotationData();
    SetDirty(true);
    objctlDoc.SaveAnnotations();
}

function ctlDoc_Changed()
{
    objctlDoc.SaveAnnotations();
    SetDirty(true);
}

function ctlDoc_AnnLoaded()
{
    SetDirty(false);
}

// Do something when any annotation is double clicked
function ctlDoc_Properties()
{

    //LoadAnnotationData();

    //$('#annModal').modal('show');
}

function ctlDoc_OnThumbnailClicked(t)
{
    selectedPageIndex = t;
    if (ppp)
    {
        if (CloseAnnotation())
        {

            objctlDoc.GotoPage(parseInt(t));
            OpenAnnotation();

            annotationsSaved = false;
            SetDirty(false);
        }
    }

    return true;
}

function CallNext(goNext)
{

    if (CloseAnnotation())
    {

        objctlDoc.Next(goNext);
        OpenAnnotation();

        annotationsSaved = false;
        SetDirty(false);
    }
}

function CallSave()
{
    var didSave = objctlDoc.SaveAnnotations();
    if (didSave)
    {
        SetDirty(false);
        annotationsSaved = true;
        CloseAnnotation();
    }
}

function ctlDoc_Created()
{
    // Do something when any annotation is created
    var annObj = objctlDoc.AnnotationController().GetAnnotationById(objctlDoc.AnnotationController().GetSelectedAnnotation());

    annObj.SetAuthor('Admin');

    // you can change color and other properties as element is created

    //annObj.SetBackColor('orange');
    //annObj.Paint();

    SetDirty(true);
    objctlDoc.SaveAnnotations();

    //Save Custom Annotation
    //CallSave();
    //annMode = false;
    //CloseAnnotation();
}

/* Custom Annotations */

function AddOrangeRect()
{
    if (null != objctlDoc.AnnotationController())
    {
        var objRect = new RectangleAnnotation({ left: 10, top: 10, width: 300, height: 100, backColor: 'orange', opacity: 70 });
        objctlDoc.AnnotationController().AddAnnotation(null, objRect, null);
    }
}

function AddGoogle()
{
    if (null != objctlDoc.AnnotationController())
    {
        var objImage = new ImageAnnotation({ left: 100, top: 100, width: 250, height: 100 });
        objctlDoc.AnnotationController().AddAnnotation(null, objImage, null);

        objImage.SetNote('https://www.google.com/images/srpr/logo11w.png');
        objImage.Paint();
    }
}

function AddStamp()
{
    if (null != objctlDoc.AnnotationController())
    {
        var objStamp = new StampAnnotation({ left: 350, top: 300, width: 300, height: 100, rotate: -15, title: 'Hello World !', borderWidth: 4, borderColor: 'red' });
        objctlDoc.AnnotationController().AddAnnotation(null, objStamp, null);
    }
}

function ImageSignature()
{
    if (null != objctlDoc.AnnotationController())
    {
        debugger;
        if (signatureUri != "")
        {
            $.post(urlScheme + "://" + rootPath + signTokenPath, {
                binaryData: signatureUri
            }).done(handleSign);
        }
        else
        {
            window.parent.ShowInformationMessage("لا يوجد توقيع الرجاء اضافة توقيع من تفضيلات المستخدم");
        }
    }
}

function ImageStamp()
{
    if (null != objctlDoc.AnnotationController())
    {
        if (stampUri != "")
        {
            $.post(urlScheme + "://" + rootPath + signTokenPath, {
                binaryData: stampUri
            }).done(handleMark);
        }
        else
        {
            window.parent.ShowInformationMessage("لا يوجد تأشير الرجاء اضافة تأشير من تفضيلات المستخدم");
        }
    }
}
function ImageMessage() {
    if (null != objctlDoc.AnnotationController()) {
        if (messageUri != "") {
            $.post(urlScheme + "://" + rootPath + signTokenPath, {
                binaryData: messageUri
            }).done(handleSign);
        }
        else {
            window.parent.ShowInformationMessage("لا يوجد ختم الرجاء اضافة ختم من تفضيلات المستخدم");
        }
    }
}

function AddTextNote()
{
    if (null != objctlDoc.AnnotationController())
    {
        handleTextNote();
    } else
    {
        window.parent.ShowInformationMessage("لا يمكن اضافة الملاحظة");
    }
}

handleSign = function (data)
{
    data = data.trim();
    if (data.length == 36)
    {
        signToken = data.trim();
        var objSignImage = new ImageAnnotation({ left: 400, top: 500, width: 160, height: 120 });
        objctlDoc.AnnotationController().AddAnnotation(null, objSignImage, null);
        objSignImage.SetNote(urlScheme + "://" + rootPath + signPath + '?token=' + signToken);
        objSignImage.Paint();
        objSignImage.note = objSignImage.note;
        objctlDoc.SaveAnnotations();
        SetDirty(true);
    }
}

handleMark = function (data)
{
    data = data.trim();
    if (data.length == 36)
    {
        signToken = data.trim();
        var objSignImage = new ImageAnnotation({ left: 400, top: 400, width: 60, height: 40 });
        objctlDoc.AnnotationController().AddAnnotation(null, objSignImage, null);
        objSignImage.SetNote(urlScheme + "://" + rootPath + signPath + '?token=' + signToken);
        objSignImage.Paint();
        objSignImage.note = objSignImage.note;
        objctlDoc.SaveAnnotations();
        SetDirty(true);
    }
}

handleTextNote = function (data)
{
    var objNote = new NoteAnnotation({ left: 200, top: 200, width: 260, height: 55 });
    objctlDoc.AnnotationController().AddAnnotation(null, objNote, null);
    //objNote.SetNote(urlScheme + "://" + rootPath + signPath + '?token=' + signToken);
    objNote.Paint();
    var text = prompt("الرجاء إدخال النص", "");
    if (text != null && text != "")
    {
        //objNote.note = text;

        objNote.SetNote(text);
        objNote.SetTitleFontSize(25);
        objNote.SetTitleColor('black');
        objNote.SetBorderWidth(0);
        objNote.showBorder = false;

        objctlDoc.SaveAnnotations();
        SetDirty(true);
        $("div[id*='_note_']").css('font-size', '25px')
        $("div[id*='_note_']").css('color', '#000000')
    }
}

function ImageBarcode()
{
    debugger;
    if (null != objctlDoc.AnnotationController())
    {

        if (parent.document.getElementById('ImgBarcodeCanvas') != null && parent.document.getElementById('ImgBarcodeCanvas') != undefined && parent.document.getElementById('ImgBarcodeCanvas').src != null && parent.document.getElementById('ImgBarcodeCanvas').src != undefined)
        {
            $.post(urlScheme + "://" + rootPath + barcodeTokenPath, {
                binaryData: parent.document.getElementById('ImgBarcodeCanvas').src.replace("data:image/png;base64,", '')
            }).done(handleBarcode);
        }
        else
        {
            window.parent.ShowInformationMessage("لا يوجد باركود الرجاء اضافة باركود ");
        }
    }


}

handleBarcode = function (data)
{
    data = data.trim();
    if (data.length == 36)
    {
        debugger;
        barcodeToken = data.trim();
        var objBarcodeImage = new ImageAnnotation({ left: 10, top: 10, width: 250, height: 150 });
        objctlDoc.AnnotationController().AddAnnotation(null, objBarcodeImage, null);
        objBarcodeImage.SetNote(urlScheme + "://" + rootPath + barcodePath + '?token=' + barcodeToken);
        objBarcodeImage.Paint();
        objBarcodeImage.note = objBarcodeImage.note;
        objctlDoc.SaveAnnotations();
        SetDirty(true);
    }
}

function DeleteImageFile(imageName)
{
    $.ajax({
        type: 'post',
        url: DeleteImageFileUrl,
        data: { imageName: imageName },
        success: function (Success)
        {
            alert(Success);
        }
    });
}

/************************/

function LoadAnnotationData()
{
    var ann = objctlDoc.AnnotationController().GetAnnotationById(objctlDoc.AnnotationController().GetSelectedAnnotation());

    if (null != ann)
    {
        $('#annlineVertical').val("false");
        $('#annarrowDirection').val("");

        $('#annId').val(ann.annId);
        $('#annType').val(ann.annType);

        $('#annWidth').val(ann.GetWidth());
        $('#annHeight').val(ann.GetHeight());

        $('#annborderColor').val(ann.GetBorderColor());
        $('#annbackColor').val(ann.GetBackColor());

        $('#color1_color_picker').css("background-color", ann.GetBackColor());
        $('#color2_color_picker').css("background-color", ann.GetBorderColor());

        $('#color1').val(ann.GetBackColor());
        $('#color2').val(ann.GetBorderColor());

        $('#annborderWidth').val(ann.GetBorderWidth());

        $('#annOpactiy').val(ann.GetOpacity());
        $('#annborderWidth').val(ann.GetBorderWidth());
        $('#annshowBorder').val(ann.GetShowBorder());

        $('#annRotate').val(ann.GetRotate());
        $('#annCanRotate').val(ann.GetCanRotate());

        $('#annTitle').val(ann.GetTitle());
        $('#annshowTitle').val(ann.GetShowTitle());

        $('#anntitleColor').val(ann.GetTitleColor());
        $('#anntitleFontSize').val(ann.GetTitleFontSize());

        $('#annNote').val(ann.GetNote());
        $('#annshowNote').val(ann.GetShowNote());

        $('#annBurn').val(ann.GetBurn());
        $('#annLocked').val(ann.GetLocked());

        $('#annzindex').val(ann.GetzIndex());
        // $('#annBase64').val(Base64.encode(ann.toString()));

        $('#anntextAlign').val(ann.GetTextAlign());

        $('#annAuthor').val(ann.GetAuthor());

        switch (ann.annType)
        {

            case "line":
                $('#annlineVertical').val(ann.GetLineVertical());
                break;
            case "arrow":
                $('#annarrowDirection').val(ann.GetArrowDirection());
                break;
        }
    }

}

function SaveAnnotationData()
{


    var ann = objctlDoc.AnnotationController()
        .GetAnnotationById(objctlDoc.AnnotationController().GetSelectedAnnotation());

    if (null != ann)
    {
        ann.SetBorderColor($('#annborderColor').val());
        ann.SetBackColor($('#annbackColor').val());
        ann.SetBorderWidth($('#annborderWidth').val());

        ann.SetOpacity($('#annOpactiy').val());
        ann.SetShowBorder($('#annshowBorder').val().toString().toLowerCase() === 'true');

        ann.SetCanRotate($('#annCanRotate').val().toString().toLowerCase() === 'true');
        ann.SetRotate($('#annRotate').val());

        ann.SetShowTitle($('#annshowTitle').val().toString().toLowerCase() === 'true');
        ann.SetTitle($('#annTitle').val());

        ann.SetTitleColor($('#anntitleColor').val());
        ann.SetTitleFontSize($('#anntitleFontSize').val());

        ann.SetNote($('#annNote').val());
        ann.SetShowNote($('#annshowNote').val().toString().toLowerCase() === 'true');

        ann.SetBurn($('#annBurn').val().toString().toLowerCase() === 'true');
        ann.SetLocked($('#annLocked').val().toString().toLowerCase() === 'true');
        ann.SetzIndex(parseInt($('#annzindex').val()));
        ann.SetAuthor($('#annAuthor').val());

        switch (ann.annType)
        {
            case "line":
                ann.SetLineVertical($('#annlineVertical').val().toString().toLowerCase() === 'true');
                break;
            case "arrow":
                ann.SetArrowDirection($('#annarrowDirection').val());
                break;
            case "note":
                ann.SetTextAlign($('#anntextAlign').val());
                break;
        }

        ann.Paint();
    }


    SetDirty(true);
}

function AddWatermark()
{
    //loader.Show();
    $.ajax({
        type: "GET",
        url: "/" + basePath + "/Doconut/AddWatermark?token=" + globalToken,
        success: function (token)
        {
            OpenDocument(token);
            //loader.hide();
        },
        error: function (textStatus, errorThrown, data)
        {
            alert("Error exporting to pdf");

            loader.hide();
        }
    });
}

function DeletePage()
{
    if (selectedPageIndex < 0)
    {
        window.parent.ShowInformationMessage("الرجاء اختيار صفحة لحذفها");
        return;
    }

    $.ajax({
        type: "POST",
        cache: false,
        async: true,
        url: "/" + basePath + "/Doconut/DeletePage?token=" + globalToken + "&sCurrPageIndex=" + selectedPageIndex,
        success: function (token)
        {
            OpenDocument(token);
            selectedPageIndex = 1;
        },
        error: function (textStatus, errorThrown, data)
        {
            alert("Error exporting to pdf");

            loader.hide();
        }
    });
}

function ChangePageOrder(orderType)
{
    //up  true
    //down false

    if (selectedPageIndex == 1 && orderType == true)
    {
        window.parent.ShowInformationMessage("لا يمكن ترتيب الصفحة الأولة الى الاعلى");
        return;
    }

    if (selectedPageIndex < 0)
    {
        window.parent.ShowInformationMessage("الرجاء اختيار صفحة");
        return;
    }

    $.ajax({
        type: "POST",
        cache: false,
        async: true,
        url: "/" + basePath + "/Doconut/ChangePageOrder?token=" + globalToken + "&sCurrPageIndex=" + selectedPageIndex + "&orderType=" + orderType,
        success: function (token)
        {
            OpenDocument(token);

        },
        error: function (textStatus, errorThrown, data)
        {
            alert("Error exporting to pdf");

            loader.hide();
        }
    });
}

function DeleteDocumentTemp()
{
    $.ajax({
        type: "POST",
        cache: false,
        async: true,
        url: "/" + basePath + "/Doconut/DeleteDocumentTemp",
        success: function (token)
        {
            OpenDocument(token);
            selectedPageIndex = 1;
        },
        error: function (textStatus, errorThrown, data)
        {
            alert("Error exporting to pdf");
            loader.hide();
        }
    });
}

//window.onload = function () {
//    Dynamsoft.WebTwainEnv.Unload();
//    Dynamsoft.WebTwainEnv.Load();
//};
let DWObject = null;
let twaitInterval = null;
function AcquireImage(pixelType)
{
    debugger;
    twaitInterval = setInterval(function ()
    {
        setTwaitInstance(pixelType);
    }, 1500);
}

function DoAcquireImage(pixelType)
{
    debugger;
    $('#dwtcontrolContainer').show();

    var isHttps = location.href.indexOf("https") > -1;
    var strHTTPServer = location.hostname;
    DWObject.IfSSL = isHttps
    DWObject.HTTPPort = isHttps ? 443 : 80;
    var pageIndex = (selectedPageIndex != undefined && selectedPageIndex > 0) ? selectedPageIndex - 1 : 0;
    var CurrentPathName = unescape(location.pathname);
    var CurrentPath = CurrentPathName.substring(0, CurrentPathName.lastIndexOf("/") + 1);
    var strActionPage = CurrentPath + "UploadFile?documentToken=" + globalToken + "&modeValue=" + $('#ddlUploadMode').val() + "&pageIndex=" + pageIndex;
    var uploadfilename = "TestImage.pdf";

    //DWObject = Dynamsoft.WebTwainEnv.GetWebTwain('dwtcontrolContainer');
    DWObject.IfShowUI = false;
    //*Use the property
    DWObject.IfAutoDiscardBlankpages = true;

    DWObject.SelectSource(function ()
    {
        var OnAcquireImageSuccess, OnAcquireImageFailure;
        OnAcquireImageFailure = OnAcquireImageSuccess = function ()
        {
            DWObject.HTTPUploadAllThroughPostAsPDF(
                strHTTPServer,
                strActionPage,
                uploadfilename,
                OnHttpUploadSuccess,
                OnHttpUploadFailure
            );
            $('#dwtcontrolContainer').hide();

            DWObject.CloseSource();
        };
        OnHttpUploadSuccess = function ()
        {
            if (showwatermark == 'true')
            {
                globalToken = token;
                AddWatermark();
            } else
            {
                OpenDocument(token);
            }

            DWObject.CloseSource();
            DWObject = null;
        };
        OnHttpUploadFailure = function (a, b, token)
        {
            if (token)
            {
                if (showwatermark == 'true')
                {
                    globalToken = token;
                    AddWatermark();
                } else
                {
                    OpenDocument(token);
                }

                DWObject.CloseSource();
                DWObject = null;
            }
        };
        DWObject.OpenSource();

        DWObject.PixelType = pixelType;
        DWObject.AcquireImage(OnAcquireImageSuccess, OnAcquireImageFailure);
    }, function () { console.log("Failed to Select A Source!"); });
}


function setTwaitInstance(pixelType)
{
    debugger;
    if (!DWObject)
    {
        Dynamsoft.WebTwainEnv.DeleteDWTObject("dwtcontrolContainer2");
        //DWObject = Dynamsoft.WebTwainEnv.GetWebTwain('dwtcontrolContainer');
        Dynamsoft.WebTwainEnv.CreateDWTObject(
            "dwtcontrolContainer2",
            function (newDWObject) { DWObject = newDWObject; },
            function (errorString) { alert(errorString); }
        );
    }
    else
    {
        clearInterval(twaitInterval);
        DoAcquireImage(pixelType);

    }
}
