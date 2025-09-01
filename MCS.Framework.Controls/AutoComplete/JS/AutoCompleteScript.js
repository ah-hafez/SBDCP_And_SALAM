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

            $("input[id ='" + hdnIdToSaveValue + "']").val(contentValue);
            $("input[id ='" + autoCompleteControlid + "']").val(contentlabel);
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

                $("input[id ='" + hdnIdToSaveValue + "']").val(contentValue);
                $("input[id ='" + autoCompleteControlid + "']").val(contentlabel);
                $('#' + hdnExtraParametersId).val(contentParameters);
            }
        }

        $("input[id ='" + autoCompleteControlid + "']").autocomplete({
            create: function () {

                $(".ui-autocomplete").addClass("autocomplete_scroll");
                $(".ui-autocomplete").addClass("scroll");
                $(".ui-helper-hidden-accessible").remove();
            },
            source: source,
            focus: function (event, ui) {

                event.preventDefault();
                $("input[id ='" + autoCompleteControlid + "']").css('color', autoCompleteColor);
                //$(this).val(ui.item.label);
            },
            select: function (event, ui) {

                event.preventDefault();
                $(this).val(ui.item.label);
                $("input[id ='" + hdnIdToSaveValue + "']").val(ui.item.value);
                $('#' + hdnExtraParametersId).val(ui.item.parameters);

                if ($("input[id ='" + hdnIdToSaveValue + "']").val() == '') {

                    //$("input[id ='" + autoCompleteControlid + "']").css('color', 'red');
                    $("input[id ='" + autoCompleteControlid + "']").val("");
                }
                else {

                    $("input[id ='" + autoCompleteControlid + "']").css('color', autoCompleteColor);
                }

                $("input[id ='" + hdnIdToSaveValue + "']").valid();

                if ($("input[id ='" + hdnIdToSaveValue + "']").hasClass(validationClass)) {

                    $("input[id ='" + autoCompleteControlid + "']").addClass(validationClass);
                }

                $("input[id ='" + autoCompleteControlid + "']").trigger('change');
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

                    if ($('input[id =' + this.id + ']').css('display') == 'block') {

                        __hideList = true;
                    }
                });

                if (!__hideList) {

                    $("input[id ='" + autoCompleteControlid + "']").autocomplete("search", "");
                }
                else {

                    $(".ui-autocomplete").hide();
                }
            }
            else {

                $("input[id ='" + autoCompleteControlid + "']").autocomplete("search", "");
            }
        });
        //var autoCompleteColor = $("input[id ='" + autoCompleteControlid + "']").css("color");
        var autoCompleteColor = $("input[id ='" + hdnIdToSaveValue + "']").css("color");

        $("input[id ='" + autoCompleteControlid + "']").keydown(function () {

            $("input[id ='" + autoCompleteControlid + "']").css('color', autoCompleteColor);
        });

        $("input[id ='" + autoCompleteControlid + "']").focusout(function () {

            var label = $("input[id ='" + autoCompleteControlid + "']").val();
            var value = $("input[id ='" + hdnIdToSaveValue + "']").val();
            var param = $('#' + hdnExtraParametersId).val();

            initialSource = $("input[id ='" + autoCompleteControlid + "']").autocomplete("option", "source");

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
                            $("input[id ='" + hdnIdToSaveValue + "']").val(initialSource[i].value);
                            $('#' + hdnExtraParametersId).val(initialSource[i].parameters);
                        }
                    }
                }

                if (append == false) {
                    $("input[id ='" + hdnIdToSaveValue + "']").val("");
                    $('#' + hdnExtraParametersId).val("");
                }

                if ($("input[id ='" + hdnIdToSaveValue + "']").val() == '' && $("input[id ='" + autoCompleteControlid + "']").val() != '') {
                    //$("input[id ='" + autoCompleteControlid + "']").css('color', 'red');
                    $("input[id ='" + autoCompleteControlid + "']").val("");
                }
                else {
                    $("input[id ='" + autoCompleteControlid + "']").css('color', autoCompleteColor);
                }

                $("input[id ='" + autoCompleteControlid + "']").trigger('change');
            }
        });

        $("input[id ='" + hdnIdToSaveValue + "']").change(function () {
          
            var label = $("input[id ='" + autoCompleteControlid + "']").val();
            var value = $("input[id ='" + hdnIdToSaveValue + "']").val();            

            var append = false;

            initialSource = $("input[id ='" + autoCompleteControlid + "']").autocomplete("option", "source");

            for (var i = 0; i < initialSource.length; i++) {
                if (initialSource[i].value == value) {
                    append = true;
                    $("input[id ='" + autoCompleteControlid + "']").val(initialSource[i].label);
                    $('#' + hdnExtraParametersId).val(initialSource[i].parameters);

                }
            }
            if (append == false) {
                $("input[id ='" + autoCompleteControlid + "']").val("");
                $('#' + hdnExtraParametersId).val("");

            }

            if ($("input[id ='" + hdnIdToSaveValue + "']").val() == '' && $("input[id ='" + autoCompleteControlid + "']").val() != '') {

                $("input[id ='" + autoCompleteControlid + "']").css('color', 'red');
            }
            else {

                $("input[id ='" + autoCompleteControlid + "']").css('color', autoCompleteColor);
            }
        });

        $("input[id ='" + autoCompleteControlid + "']").change(function () {
            $("input[id ='" + hdnIdToSaveValue + "']").valid();

            if ($("input[id ='" + hdnIdToSaveValue + "']").hasClass(validationClass)) {

                $("input[id ='" + autoCompleteControlid + "']").addClass(validationClass);
            }

            var value = $("input[id ='" + hdnIdToSaveValue + "']").val();
            if (value) {
                if (onChangeCallback != "" && onChangeCallback != null)
                    window[onChangeCallback](this, value);
                console.log(value);
            }
        });

        $('input[type=button], input[type=submit], button').click(function () {

            if ($("input[id ='" + hdnIdToSaveValue + "']").hasClass(validationClass)) {

                $("input[id ='" + autoCompleteControlid + "']").addClass(validationClass);
            }
        });
    });
}

function AutoCompleteChangeList(autoCompleteControlid, newList) {
    $("input[id ='" + autoCompleteControlid + "']").autocomplete("option", "source", $.parseJSON(newList));
}

