var RenderTextEditorSettings = { textControlId: "", readOnly: "", languageShortName: "", stampBase64Image: "", signatureBase64Image: "", content: "", javascriptFunName: "", isContentEncoded: "", hdnIdToSaveContent: "", defaultPlugins: [], defaultToolbar: "" };
var javascriptContent = "";
var isCopyPrevious;

function RenderTextEditor(textControlId, readOnly, languageShortName, stampBase64Image, signatureBase64Image, content, javascriptFunName, isContentEncoded, hdnIdToSaveContent, BarCodeBase64Image, WaterMarkBase64Image) {
    var javascriptContent = "";

    if (javascriptFunName != null && javascriptFunName != "") {

        javascriptContent = javascriptFunName;
    }
    else if (content != null && content != "") {

        javascriptContent = content;
    }

    if (readOnly == "false") {
        RenderTextEditorSettings.readOnly = false;
        readOnly = false;
    }
    else {
        readOnly = true;
        RenderTextEditorSettings.readOnly = true;
    }

    var defaultPlugins = [
        "advlist directionality autolink lists link charmap hr anchor pagebreak",
        "searchreplace wordcount visualblocks visualchars code fullscreen",
        "media nonbreaking table ",
        "textcolor colorpicker textpattern",
        "print paste"
    ];
    var defaultToolbar = "print | undo redo | styleselect | ltr rtl bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link fullscreen | media | forecolor backcolor | fontselect | fontsizeselect | directionality ";

    if (BarCodeBase64Image != null && BarCodeBase64Image != "") {
        defaultPlugins.push("InsertBarCode");
        defaultToolbar = defaultToolbar + "| InsertBarCode "
    }

    //Share Letter
    defaultPlugins.push("ShareLetter");
    RenderTextEditorSettings.defaultPlugins.push("ShareLetter");
    defaultToolbar = defaultToolbar + "| ShareLetter ";
    RenderTextEditorSettings.defaultToolbar = defaultToolbar + "| ShareLetter ";

    if (stampBase64Image != null && stampBase64Image != "") {
        defaultPlugins.push("InsertStamp");
        defaultToolbar = defaultToolbar + "| InsertStamp ";
        RenderTextEditorSettings.defaultPlugins.push("InsertStamp");
        RenderTextEditorSettings.defaultToolbar = defaultToolbar + "| InsertStamp ";
    }

    if (signatureBase64Image != null && signatureBase64Image != "") {
        defaultPlugins.push("InsertSignature");
        defaultToolbar = defaultToolbar + "| InsertSignature ";
        RenderTextEditorSettings.defaultPlugins.push("InsertSignature");
        RenderTextEditorSettings.defaultToolbar = defaultToolbar + "| InsertSignature ";
    }

    RenderTextEditorSettings.textControlId = textControlId;
    RenderTextEditorSettings.languageShortName = languageShortName;
    RenderTextEditorSettings.stampBase64Image = stampBase64Image;
    RenderTextEditorSettings.signatureBase64Image = signatureBase64Image;
    RenderTextEditorSettings.content = content;
    RenderTextEditorSettings.javascriptFunName = javascriptFunName;
    RenderTextEditorSettings.isContentEncoded = isContentEncoded;
    RenderTextEditorSettings.hdnIdToSaveContent = hdnIdToSaveContent;
    RenderTextEditorSettings.javascriptContent = javascriptContent;

    $(document).ready(function () {
        if (isCopyPrevious !== undefined && isCopyPrevious) {
            InitTinymce();
        } else {
            if (tinymce.get(textControlId)) {
                tinymce.remove('#' + textControlId);
            }
            
            tinymce.init({
                selector: '#' + textControlId,
                paste_data_images: true,
                readonly: readOnly,
                language: languageShortName,
                plugins: defaultPlugins,
                toolbar: defaultToolbar,
                directionality: "rtl",
                content_css: [window.location.origin + "/" + window.location.pathname.split('/')[1] + "/Content/User/lib/fonts/tajawal/tajawal.css"],
                font_formats: 'Tajawal =Tajawal Regular=andale mono,times;Arial=arial,helvetica,sans-serif;Adobe Arabic = Adobe Arabic;Arial Black=arial black,avant garde;Book Antiqua=book antiqua,palatino;Bahij Palatino Sans Arabic = Bahij Palatino Sans Arabic;Comic Sans MS=comic sans ms,sans-serif;Courier New=courier new,courier;Georgia=georgia,palatino;Helvetica=helvetica;Impact=impact,chicago;Lalezar = Lalezar;Longdoosi = Longdoosi Personal Use;Microsoft Uighur=Microsoft Uighur;Symbol=symbol;Tahoma=tahoma,arial,helvetica,sans-serif;Terminal=terminal,monaco;Times New Roman=times new roman,times;Trebuchet MS=trebuchet ms,geneva;Verdana=verdana,geneva;Webdings=webdings;Wingdings=wingdings,zapf dingbats',
                height: 500,
                removed_menuitems: 'cut, copy, paste',
                setup: function (ed) {
                    ed.addButton('InsertStamp', {
                        text: 'Insert Stamp',
                        icon: 'stamp',
                        onclick: function () {
                            tinymce.activeEditor.execCommand('mceInsertContent', false, '<img src=data:image/png;base64,' + stampBase64Image + '></img>');
                        }
                    });

                    ed.addMenuItem('InsertStamp', {
                        text: 'Insert Stamp',
                        icon: 'stamp',
                        context: 'insert',
                        onclick: function () {
                            tinymce.activeEditor.execCommand('mceInsertContent', false, '<img src=data:image/png;base64,' + stampBase64Image + '></img>');
                        }
                    });
                    ed.addButton('InsertSignature', {
                        text: 'Insert Signature',
                        icon: 'signature',
                        onclick: function () {
                            tinymce.activeEditor.execCommand('mceInsertContent', false, '<img class="imgSign" src=data:image/png;base64,' + signatureBase64Image + '></img>');
                        }
                    });

                    ed.addMenuItem('InsertSignature', {
                        text: 'Insert Signature',
                        icon: 'signature',
                        context: 'insert',
                        onclick: function () {
                            tinymce.activeEditor.execCommand('mceInsertContent', false, '<img class="imgSign" src=data:image/png;base64,' + signatureBase64Image + '></img>');
                        }
                    });

                    ed.addButton('InsertBarCode', {
                        text: 'Insert BarCode',
                        icon: 'signature',
                        onclick: function () {
                            content = tinymce.activeEditor.getContent();
                            if (content.indexOf("[barcode]") > -1 || content.indexOf("[باركود]") > -1) {
                                content = content.replace("[barcode]", '<img class="imgSign tinySignature" src=data:image/png;base64,' + BarCodeBase64Image + '></img>');
                                content = content.replace("[باركود]", '<img class="imgSign tinySignature" src=data:image/png;base64,' + BarCodeBase64Image + '></img>');
                                tinymce.activeEditor.setContent(content);
                            } else {
                                tinymce.activeEditor.execCommand('mceInsertContent', false, '<img class="imgSign tinySignature" src=data:image/png;base64,' + BarCodeBase64Image + '></img>');
                            }
                        }
                    });

                    ed.addMenuItem('InsertBarCode', {
                        text: 'Insert BarCode',
                        icon: 'signature',
                        context: 'insert',
                        onclick: function () {
                            content = tinymce.activeEditor.getContent();
                            if (content.indexOf("[barcode]") > -1 || content.indexOf("[باركود]") > -1) {
                                content = content.replace("[barcode]", '<img class="imgSign tinySignature" src=data:image/png;base64,' + BarCodeBase64Image + '></img>');
                                content = content.replace("[باركود]", '<img class="imgSign tinySignature" src=data:image/png;base64,' + BarCodeBase64Image + '></img>');
                                tinymce.activeEditor.setContent(content);
                            } else {
                                tinymce.activeEditor.execCommand('mceInsertContent', false, '<img class="imgSign tinySignature" src=data:image/png;base64,' + BarCodeBase64Image + '></img>');
                            }
                        }
                    });

                    //ed.addButton('ShareLetter', {
                    //    text: '',
                    //    icon: 'share',
                    //    onclick: function () {
                    //        OpenShareModal();
                    //    }
                    //});
                    ed.on("init",
                        function (ed) {
                            if (javascriptContent != null && javascriptContent != "") {
                                if (isContentEncoded == "false") {
                                    tinyMCE.get(textControlId).setContent(javascriptContent);
                                    $('#' + hdnIdToSaveContent).val($('<div/>').text(tinymce.get(textControlId).getContent()).html());
                                }
                                else {
                                    tinyMCE.get(textControlId).setContent($('<div/>').html(javascriptContent).text());
                                    $('#' + hdnIdToSaveContent).val($('<div/>').text(tinymce.get(textControlId).getContent()).html());
                                }
                            }
                            if (window.location.href.includes('Edit') && window.textEditorSingature) {
                                jQuery.each($('.mce-tinymce iframe'), function (indx, val) {
                                    $('body', $(val).contents()).css({ 'background-image': "url('data:image/png;base64," + textEditorSingature + "')" })
                                });
                            }
                        }
                    );
                    ed.on("change",
                        function (ed) {
                            $('#' + hdnIdToSaveContent).val($('<div/>').text(tinymce.get(textControlId).getContent()).html());
                        }
                    );
                    ed.on("postProcess",
                        function (ed) {                         
                            //var editorContent = tinymce.get(textControlId).getContent();
                            if (typeof ShareLetterContent === "function") {
                                console.log(ed.content);
                                setTimeout(function () {
                                    ShareLetterContent(ed.content);
                                }, 2000);
                            }
                        }
                    );
                    ed.on("keyup",
                        function (ed) {
                            var editorContent = tinymce.get(textControlId).getContent();
                            //console.log(editorContent);
                            if (typeof ShareLetterContent === "function") {
                                setTimeout(function () {
                                    ShareLetterContent(editorContent);
                                }, 2000);
                            }
                        }
                    );
                }
            });
        }
    });
}

function ChangeTextEditorContent(textControlId, hdnIdToSaveContent, newData, isContentEncoded) {
    RenderTextEditorSettings.isContentEncoded = isContentEncoded;
    RenderTextEditorSettings.textControlId = textControlId;
    RenderTextEditorSettings.hdnIdToSaveContent = hdnIdToSaveContent;
    if (isCopyPrevious !== undefined && isCopyPrevious) {
        if ((RenderTextEditorSettings.isContentEncoded != undefined && RenderTextEditorSettings.isContentEncoded != "") || RenderTextEditorSettings.isContentEncoded == false) {
            tinyMCE.get(RenderTextEditorSettings.textControlId).setContent(newData);
            $('#' + RenderTextEditorSettings.hdnIdToSaveContent).val($('<div/>').text(tinymce.get(RenderTextEditorSettings.textControlId).getContent()).html());
        }
        else {
            tinyMCE.get(RenderTextEditorSettings.textControlId).setContent($('<div/>').html(newData).text());
            $('#' + RenderTextEditorSettings.hdnIdToSaveContent).val($('<div/>').text(tinymce.get(RenderTextEditorSettings.textControlId).getContent()).html());
        }
    } else {
        if ((isContentEncoded != undefined && isContentEncoded != "") || isContentEncoded == false) {
            tinyMCE.get(textControlId).setContent(newData);
            $('#' + hdnIdToSaveContent).val($('<div/>').text(tinymce.get(textControlId).getContent()).html());
        }
        else {
            tinyMCE.get(textControlId).setContent($('<div/>').html(newData).text());
            $('#' + hdnIdToSaveContent).val($('<div/>').text(tinymce.get(textControlId).getContent()).html());
        }
    }
}

function InitTinymce() {
    
    tinymce.init({
        selector: '#' + RenderTextEditorSettings.textControlId,
        paste_data_images: true,
        readonly: RenderTextEditorSettings.readOnly,
        language: RenderTextEditorSettings.languageShortName,
        plugins: RenderTextEditorSettings.defaultPlugins,
        toolbar: RenderTextEditorSettings.defaultToolbar,
        directionality: "rtl",
        height: 500,
        removed_menuitems: 'cut, copy, paste',
        setup: function (ed) {
            ed.addButton('ShareLetter', {
                text: 'Share Letter',
                icon: 'stamp',
                onclick: function () {
                    alert('share letter');
                }
            });

            ed.addButton('InsertStamp', {
                text: 'Insert Stamp',
                icon: 'stamp',
                onclick: function () {
                    tinymce.activeEditor.execCommand('mceInsertContent', false, '<img src=data:image/png;base64,' + RenderTextEditorSettings.stampBase64Image + '></img>');
                }
            });

            ed.addMenuItem('InsertStamp', {
                text: 'Insert Stamp',
                icon: 'stamp',
                context: 'insert',
                onclick: function () {
                    tinymce.activeEditor.execCommand('mceInsertContent', false, '<img src=data:image/png;base64,' + RenderTextEditorSettings.stampBase64Image + '></img>');
                }
            });

            ed.addButton('InsertSignature', {
                text: 'Insert Signature',
                icon: 'signature',
                onclick: function () {
                    tinymce.activeEditor.execCommand('mceInsertContent', false, '<img class="imgSign" src=data:image/png;base64,' + RenderTextEditorSettings.signatureBase64Image + '></img>');
                }
            });

            ed.addMenuItem('InsertSignature', {
                text: 'Insert Signature',
                icon: 'signature',
                context: 'insert',
                onclick: function () {
                    tinymce.activeEditor.execCommand('mceInsertContent', false, '<img class="imgSign" src=data:image/png;base64,' + RenderTextEditorSettings.signatureBase64Image + '></img>');
                }
            });

            ed.addButton('InsertBarCode', {
                text: 'Insert BarCode',
                icon: 'signature',
                onclick: function () {
                    content = tinymce.activeEditor.getContent();
                    if (content.indexOf("[barcode]") > -1 || content.indexOf("[باركود]") > -1) {
                        content = content.replace("[barcode]", '<img class="imgSign tinySignature" src=data:image/png;base64,' + BarCodeBase64Image + '></img>');
                        content = content.replace("[باركود]", '<img class="imgSign tinySignature" src=data:image/png;base64,' + BarCodeBase64Image + '></img>');
                        tinymce.activeEditor.setContent(content);
                    } else {
                        tinymce.activeEditor.execCommand('mceInsertContent', false, '<img class="imgSign tinySignature" src=data:image/png;base64,' + BarCodeBase64Image + '></img>');
                    }
                }
            });

            ed.addMenuItem('InsertBarCode', {
                text: 'Insert BarCode',
                icon: 'signature',
                context: 'insert',
                onclick: function () {
                    content = tinymce.activeEditor.getContent();
                    if (content.indexOf("[barcode]") > -1 || content.indexOf("[باركود]") > -1) {
                        content = content.replace("[barcode]", '<img class="imgSign tinySignature" src=data:image/png;base64,' + BarCodeBase64Image + '></img>');
                        content = content.replace("[باركود]", '<img class="imgSign tinySignature" src=data:image/png;base64,' + BarCodeBase64Image + '></img>');
                        tinymce.activeEditor.setContent(content);
                    } else {
                        tinymce.activeEditor.execCommand('mceInsertContent', false, '<img class="imgSign tinySignature" src=data:image/png;base64,' + BarCodeBase64Image + '></img>');
                    }
                }
            });

            ed.addButton('ShareLetter', {
                text: '',
                icon: 'share',
                onclick: function () {
                    OpenShareModal();
                }
            });

            ed.on("init",
                function (ed) {
                    if (RenderTextEditorSettings.javascriptContent != null && RenderTextEditorSettings.javascriptContent != "") {
                        if (RenderTextEditorSettings.isContentEncoded == "false") {
                            tinyMCE.get(RenderTextEditorSettings.textControlId).setContent(RenderTextEditorSettings.javascriptContent);
                            $('#' + RenderTextEditorSettings.hdnIdToSaveContent).val($('<div/>').text(tinymce.get(RenderTextEditorSettings.textControlId).getContent()).html());
                        }
                        else {
                            tinyMCE.get(RenderTextEditorSettings.textControlId).setContent($('<div/>').html(RenderTextEditorSettings.javascriptContent).text());
                            $('#' + RenderTextEditorSettings.hdnIdToSaveContent).val($('<div/>').text(tinymce.get(RenderTextEditorSettings.textControlId).getContent()).html());
                        }
                    }
                }
            );
            ed.on("change",
                function (ed) {
                    $('#' + RenderTextEditorSettings.hdnIdToSaveContent).val($('<div/>').text(tinymce.get(RenderTextEditorSettings.textControlId).getContent()).html());
                }
            );
            ed.on("postProcess",
                function (ed) {
                    //var editorContent = tinymce.get(textControlId).getContent();
                    //console.log(ed.content);
                    if (typeof ShareLetterContent === "function") {
                        setTimeout(function () {
                            ShareLetterContent(ed.content);
                        }, 2000);
                    }
                }
            );
            ed.on("keyup",
                function (ed) {
                    var editorContent = tinymce.get(textControlId).getContent();
                    //console.log(editorContent);
                    if (typeof ShareLetterContent === "function") {
                        setTimeout(function () {
                            ShareLetterContent(editorContent);
                        }, 2000);
                    }
                }
            );
        }
    });
}
