function AutoComplete(autoCompleteControlid, hdnIdToSaveValue, items, content, matchAnywhere, hdnExtraParametersId, selectFirstIndex, validationClass, onChangeCallback) {
    if (items == null || items == '')
        items = '[]';

    items = $.parseJSON(items);

    var initialSource = items;

    var source;
    if (matchAnywhere == "false") {
        source = function (request, response) {

            var matcher = new RegExp("^" + $.ui.autocomplete.escapeRegex(request.term), "i");

            response($.grep(initialSource, function (item) {
                return matcher.test(item.label);
            }));
        }
    }
    else {

        source = initialSource;
    }

    $(document).ready(function () {

        var contentValue = "";
        var contentlabel = "";
        var contentParameters = "";

        if (selectFirstIndex == "true" && initialSource.length > 0) {

            contentlabel = initialSource[0].label;
            contentValue = initialSource[0].value;
            contentParameters = initialSource[0].parameters;

            $("#" + hdnIdToSaveValue).val(contentValue);
            $('#' + autoCompleteControlid).val(contentlabel);
            $('#' + hdnExtraParametersId).val(contentParameters);
        }
        else {
            if (content != null && content != "") {

                for (var i = 0; i < initialSource.length; i++) {
                    if (initialSource[i].value == content) {
                        contentlabel = initialSource[i].label;
                        contentValue = content;
                        contentParameters = initialSource[i].parameters;
                    }
                }

                $("#" + hdnIdToSaveValue).val(contentValue);
                $('#' + autoCompleteControlid).val(contentlabel);
                $('#' + hdnExtraParametersId).val(contentParameters);
            }
        }

        $('#' + autoCompleteControlid).autocomplete({
            create: function () {

                $(".ui-autocomplete").addClass("autocomplete_scroll");
                $(".ui-autocomplete").addClass("scroll");
                $(".ui-helper-hidden-accessible").remove();
            },
            source: source,
            focus: function (event, ui) {

                event.preventDefault();
                $('#' + autoCompleteControlid).css('color', autoCompleteColor);
                //$(this).val(ui.item.label);
            },
            select: function (event, ui) {

                event.preventDefault();
                $(this).val(ui.item.label);
                $("#" + hdnIdToSaveValue).val(ui.item.value);
                $('#' + hdnExtraParametersId).val(ui.item.parameters);

                if ($('#' + hdnIdToSaveValue).val() == '') {

                    //$('#' + autoCompleteControlid).css('color', 'red');
                    $('#' + autoCompleteControlid).val("");
                }
                else {

                    $('#' + autoCompleteControlid).css('color', autoCompleteColor);
                }

                $("#" + hdnIdToSaveValue).valid();

                if ($('#' + hdnIdToSaveValue).hasClass(validationClass)) {

                    $('#' + autoCompleteControlid).addClass(validationClass);
                }

                $('#' + autoCompleteControlid).trigger('change');
            },
            minLength: 0,
            open: function () {
                $(".jconfirm-box").draggable({ disabled: true });
            },
            close: function () {
                $(".jconfirm-box").draggable({ disabled: false });
            }
        }).click(function () {

            var __hideList = false;

            if ($(".ui-autocomplete").length != 0) {

                $(".ui-autocomplete").each(function () {

                    if ($('#' + this.id).css('display') == 'block') {

                        __hideList = true;
                    }
                });

                if (!__hideList) {

                    $('#' + autoCompleteControlid).autocomplete("search", "");
                }
                else {

                    $(".ui-autocomplete").hide();
                }
            }
            else {

                $('#' + autoCompleteControlid).autocomplete("search", "");
            }
        });
        //var autoCompleteColor = $('#' + autoCompleteControlid).css("color");
        var autoCompleteColor = $('#' + hdnIdToSaveValue).css("color");

        $('#' + autoCompleteControlid).keydown(function () {

            $('#' + autoCompleteControlid).css('color', autoCompleteColor);
        });

        $('#' + autoCompleteControlid).focusout(function () {

            var label = $('#' + autoCompleteControlid).val();
            var value = $('#' + hdnIdToSaveValue).val();
            var param = $('#' + hdnExtraParametersId).val();

            initialSource = $('#' + autoCompleteControlid).autocomplete("option", "source");

            var append = false;
            var change = true;

            for (var i = 0; i < initialSource.length; i++) {

                if (value == initialSource[i].value && initialSource[i].label == label) {

                    change = false;

                    break;
                }
            }

            if (change) {

                for (var i = 0; i < initialSource.length; i++) {

                    if (initialSource[i].label == label) {
                        append = true;
                        if (value != initialSource[i].value) {
                            $('#' + hdnIdToSaveValue).val(initialSource[i].value);
                            $('#' + hdnExtraParametersId).val(initialSource[i].parameters);
                        }
                    }
                }

                if (append == false) {
                    $('#' + hdnIdToSaveValue).val("");
                    $('#' + hdnExtraParametersId).val("");
                }

                if ($('#' + hdnIdToSaveValue).val() == '' && $('#' + autoCompleteControlid).val() != '') {
                    //$('#' + autoCompleteControlid).css('color', 'red');
                    $('#' + autoCompleteControlid).val("");
                }
                else {
                    $('#' + autoCompleteControlid).css('color', autoCompleteColor);
                }

                $('#' + autoCompleteControlid).trigger('change');
            }
        });

        $('#' + hdnIdToSaveValue).change(function () {
          
            var label = $('#' + autoCompleteControlid).val();
            var value = $('#' + hdnIdToSaveValue).val();            

            var append = false;

            initialSource = $('#' + autoCompleteControlid).autocomplete("option", "source");

            for (var i = 0; i < initialSource.length; i++) {
                if (initialSource[i].value == value) {
                    append = true;
                    $('#' + autoCompleteControlid).val(initialSource[i].label);
                    $('#' + hdnExtraParametersId).val(initialSource[i].parameters);

                }
            }
            if (append == false) {
                $('#' + autoCompleteControlid).val("");
                $('#' + hdnExtraParametersId).val("");

            }

            if ($('#' + hdnIdToSaveValue).val() == '' && $('#' + autoCompleteControlid).val() != '') {

                $('#' + autoCompleteControlid).css('color', 'red');
            }
            else {

                $('#' + autoCompleteControlid).css('color', autoCompleteColor);
            }
        });

        $('#' + autoCompleteControlid).change(function () {
            $("#" + hdnIdToSaveValue).valid();

            if ($('#' + hdnIdToSaveValue).hasClass(validationClass)) {

                $('#' + autoCompleteControlid).addClass(validationClass);
            }

            var value = $('#' + hdnIdToSaveValue).val();
            if (value) {
                if (onChangeCallback != "" && onChangeCallback != null)
                    window[onChangeCallback](this, value);
                console.log(value);
            }
        });

        $('input[type=button], input[type=submit], button').click(function () {

            if ($('#' + hdnIdToSaveValue).hasClass(validationClass)) {

                $('#' + autoCompleteControlid).addClass(validationClass);
            }
        });
    });
}

function AutoCompleteChangeList(autoCompleteControlid, newList) {
    $('#' + autoCompleteControlid).autocomplete("option", "source", $.parseJSON(newList));
}

function SetAutoCompleteValue(Id, newValue) {
    var dataDDL = $('#' + Id).autocomplete("option", "source");
    for (var i = 0; i < dataDDL.length; i++) {
        if (dataDDL[i].value == newValue) {
            append = true;
            $('#' + Id).val(dataDDL[i].label);
        }
    }
}

function DeleteAutocompleteItem(Id, newValue) {
    var dataDDL = $('#' + Id).autocomplete("option", "source");
    for (var i = 0; i < dataDDL.length; i++) {
        if (dataDDL[i].value == newValue) {
            dataDDL.pop(dataDDL[i]);
            $('#' + Id).autocomplete("option", "source", dataDDL);
        }
    }
}