var selectedPageIndex = -1;

function ctlDoc_OnViewerBusy() {
    loader.show();
}

function ctlDoc_OnViewerReady() {
    loader.hide();
}

function Resize(orientation) {
  
    if (resizing) { return; }

    resizing = true;

    w = document.documentElement.clientWidth;
    h = document.documentElement.clientHeight;


    var xdec = 30;
    var ydec = 70;

    if (isMobile) {
        xdec = 30;
        ydec = 70;

        if (typeof orientation !== 'undefined') {
            if (orientation === "landscape") {
                w = document.documentElement.clientHeight;
                h = document.documentElement.clientWidth;
            }
        }
    }

    docViewerDiv.width(w - xdec);
    docViewerDiv.height(h - ydec);

    SetThumbs();
    resizing = false;
}


function SetThumbs() {
    if (annMode)
        return;

    try {
        objctlDoc.HideThumbs(true);
        objctlDoc.HideThumbs(false);
    } catch (exception) {

    }
}


function OpenDocument(token) {
    if (token != "") {
        loader.show();
        /* [Important]
         * globalToken in this place enables access to the file before adding the watermark; in case
         * that they required "undo adding watermark".
        */
        objctlDoc.View(token); // use global object to view any document
        globalToken = token; // data is actuall a Token (unique to the document being viewed)
        $('#_hdnToken').val(token);
    }
}

function GoFS() {

    if (
        document.fullscreenElement ||
        document.webkitFullscreenElement ||
        document.mozFullScreenElement ||
        document.msFullscreenElement
    ) {
        if (document.exitFullscreen) {
            document.exitFullscreen();
        } else if (document.mozCancelFullScreen) {
            document.mozCancelFullScreen();
        } else if (document.webkitExitFullscreen) {
            document.webkitExitFullscreen();
        } else if (document.msExitFullscreen) {
            document.msExitFullscreen();
        }
    } else {
        var element = $('#divDocViewer').get(0);

        if (element.requestFullscreen) {
            element.requestFullscreen();
        } else if (element.mozRequestFullScreen) {
            element.mozRequestFullScreen();
        } else if (element.webkitRequestFullscreen) {
            element.webkitRequestFullscreen(Element.ALLOW_KEYBOARD_INPUT);
        } else if (element.msRequestFullscreen) {
            element.msRequestFullscreen();
        }
    }
}

function OpenUpload() {
    $('#myModal').modal('show');
}


$(document).ready(function () {
    var mutipleFiles = false;
    $("#dropZoneForm").dropzone({
        url: "/" + basePath + "/Doconut/UploadFile",
        maxFiles: 1,
        paramName: "file",
        uploadMultiple: false,
        //maxFilesize: 31,
        acceptedFiles:
            ".doc,.docx,.docm,.odt,.xls,.xlsx,.xlsm,.ods,.csv,.ppt,.pptx,.odp,.vsd,.vsdx,.mpp,.mppx,.pdf,.tif,.tiff,.dwg,.dxf,.dgn,.xps,.psd,.jpg,.jpeg,.jpe,.png,.bmp,.gif,.eml,.msg,.txt,.rtf,.xml,.epub,.svg,.html,.htm,.mht,.dcn,.dcm",
        addRemoveLinks: false,
        init: function () {
            var th = this;
            this.on("success",
                function (file, response) {
                    if (response != "" && response != undefined) {
                        //if (file.size >= 31457280) { //30 MB = 31457280 Bytes
                        //    window.parent.ShowErrorMessage('حجم الملف يجب ان لا يتجاوز 30 ميجابايت');
                        //    return;
                        //}

                        if (mutipleFiles == false) {
                            OpenDocument(response); // Response is the Token itself.
                            selectedPageIndex = 1;
                        } else {
                            mutipleFiles = false;
                        }
                        $('#myModal').modal('hide');
                        window.top.UploadDoconutFileSuccess()
                    }
                    else {
                        window.parent.ShowErrorMessage('حدث خطأ ما, يرجى التاكد من تحميل ملف واحد مطابق للمعايير في المرة الواحدة');
                    }

                }),
                this.on("error",
                    function (file, errorMessage, c) {
                        window.parent.ShowErrorMessage('حدث خطأ ما, يرجى التاكد من تحميل ملف واحد مطابق للمعايير في المرة الواحدة');
                        mutipleFiles = true;
                        return;
                    }),
                this.on("queuecomplete",
                    function () {
                        setTimeout(function () {
                            th.removeAllFiles();
                        },
                            3000);
                    }),
                this.on("sending",
                    function (file, xhr, formData) {
                        var pageIndex = (selectedPageIndex != undefined && selectedPageIndex > 0)? selectedPageIndex - 1 : 0;
                        formData.append('documentToken', globalToken);
                        formData.append('modeValue', $('#ddlUploadMode').val());
                        formData.append('pageIndex', pageIndex);
                    });
        }
    });
});

