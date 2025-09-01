
function CustomappendModelPrefix(value, prefix) {
    if (value.indexOf("*.") === 0) {
        value = value.replace("*.", prefix);
    }
    return value;
}

function CustomescapeAttributeValue(value) {
    // As mentioned on http://api.jquery.com/category/selectors/
    return value.replace(/([!"#$%&'()*+,./:;<=>?@\[\\\]^`{|}~])/g, "\\$1");
}


//dateTime Validation 
(function ($) {
    $.validator.addMethod('date',
    function (value, element) {
        if (this.optional(element)) {
            return true;
        }
        var valid = true;
        try {
            $.datepicker.parseDate('dd/mm/yy', value);
        }
        catch (err) {
            valid = false;
        }
        return valid;
    });
    $(".datetype").datepicker({ dateFormat: 'dd/mm/yy' });
})(jQuery);

$.validator.unobtrusive.adapters.add('customcompare', ['propertyname', 'operation', 'validationgroup'], function (options) {

    var customprefix = options.element.name.substr(0, options.element.name.lastIndexOf(".") + 1);
    var customother = options.params.propertyname;
    var customfullOtherName = CustomappendModelPrefix(customother, customprefix);
    var costomelement = $(options.form).find(":input").filter("[name='" + CustomescapeAttributeValue(customfullOtherName) + "']")[0];


    options.rules['customcompare'] = {
        propertyname: costomelement,// options.params['clientvalidationrules'].split(','),
        operation: options.params['operation'],
        validationgroup: options.params['validationgroup'],
        errormessages: options.message
    };
});
$.validator.setDefaults({ ignore: null });

(function ($) {
    $.validator.addMethod("customcompare", function (value, element, params) {

        var parts = element.name.split(".");
        var isvalid = false;
        var prefix = "";
        if (parts.length > 1)
            prefix = parts[0] + ".";

        var startDateValue = $(params.propertyname).val();

        if (startDateValue == null || startDateValue == '' || value == null || value == '')
            return true;

        $.validator.messages.customcompare = params.errormessages;
        var date1 = convertdate(startDateValue);
        var date2 = convertdate(value);

        if (ClickedSubmit === params['validationgroup']) {
            if (!value || !startDateValue)
                return true;
            if (params.operation == "GreaterThanOrEqual")
                isvalid = date1 <= date2;

            if (params.operation == "GreaterThan")
                isvalid = date1 < date2;

            if (params.operation == "Equal")
                isvalid = date1 == date2;

            if (params.operation == "LessThanOrEqual")
                isvalid = date1 >= date2;

            if (params.operation == "LessThan")
                //isvalid = Date.parse(startDateValue) > Date.parse(value);
                isvalid = date1 > date2;

            return isvalid;
        }
        return true;
    });
})(jQuery);

function convertdate(date) {
    var splitDate = date.split("/");

    var jd = $.calendars.instance("gregorian").newDate(
        parseInt(splitDate[2], 10),
        parseInt(splitDate[1], 10),
        parseInt(splitDate[0], 10)).toJD();
    return jd;
}

function converttime(time) {
    var splitTime = time.split(":");
    var str = "";
    $.each(splitTime, function (i, item) {
        str += item;
    });

    return parseInt(str);
}


$.validator.unobtrusive.adapters.add('customcomparetime', ['propertyname', 'operation', 'validationgroup'], function (options) {
    options.rules['customcomparetime'] = {
        propertyname: options.params['propertyname'],// options.params['clientvalidationrules'].split(','),
        operation: options.params['operation'],
        validationgroup: options.params['validationgroup'],
        errormessages: options.message
    };
});

$.validator.addMethod("customcomparetime", function (value, element, params) {
    var parts = element.name.split(".");
    var isvalid = false;
    var prefix = "";
    if (parts.length > 1)
        prefix = parts[0] + ".";

    $.validator.messages.customcomparetime = params.errormessages;

    var startTimeValue = $('input[name="' + prefix + params.propertyname + '"]').val();
    var time1 = converttime(startTimeValue);
    var time2 = converttime(value);

    if (ClickedSubmit === params['validationgroup']) {
        if (!value || !startTimeValue)
            return true;
        if (params.operation == "GreaterThanOrEqual")
            isvalid = time1 <= time2;

        if (params.operation == "GreaterThan")
            isvalid = time1 < time2;

        if (params.operation == "Equal")
            isvalid = time1 == time2;

        if (params.operation == "LessThanOrEqual")
            isvalid = time1 >= time2;

        if (params.operation == "LessThan")
            //isvalid = Date.parse(startDateValue) > Date.parse(value);
            isvalid = time1 > time2;

        return isvalid;
    }
    return true;
});


$.validator.unobtrusive.adapters.add('customcomparenumber', ['propertyname', 'operation', 'validationgroup'], function (options) {

    options.rules['customcomparenumber'] = {
        propertyname: options.params['propertyname'],// options.params['clientvalidationrules'].split(','),
        operation: options.params['operation'],
        validationgroup: options.params['validationgroup'],
        errormessages: options.message
    };
});

$.validator.addMethod("customcomparenumber", function (value, element, params) {

    var parts = element.name.split(".");
    var isvalid = false;
    var prefix = "";
    if (parts.length > 1)
        prefix = parts[0] + ".";

    $.validator.messages.customcomparenumber = params.errormessages;

    var startNumberValue = $('input[name="' + prefix + params.propertyname + '"]').val();
    var number1 = converttime(startNumberValue);
    var number2 = converttime(value);

    if (ClickedSubmit === params['validationgroup']) {
        if (!value || !startNumberValue)
            return true;
        if (params.operation == "GreaterThanOrEqual")
            isvalid = number1 <= number2;

        if (params.operation == "GreaterThan")
            isvalid = number1 < number2;

        if (params.operation == "Equal")
            isvalid = number1 == number2;

        if (params.operation == "LessThanOrEqual")
            isvalid = number1 >= number2;

        if (params.operation == "LessThan")
            //isvalid = Date.parse(startDateValue) > Date.parse(value);
            isvalid = number1 > number2;

        return isvalid;
    }
    return true;
});




//customrequired Validation 

var ClickedSubmit;
$.validator.unobtrusive.adapters.add(
                'customrequired',
                ['validationgroup'],
                function (options) {
                    options.rules['customrequired'] = {
                        validationgroup: options.params['validationgroup'],
                        errormessages: options.params['errormessages']
                    };
                    options.messages['customrequired'] = options.message;
                });

$.validator.addMethod('customrequired',
    function (value, element, parameters) {

        if (ClickedSubmit === parameters['validationgroup']) {

            return $.validator.methods.required.call(
              this, value, element, parameters);
        }


        if (ClickedSubmit === parameters['errormessages']) {
            return $.validator.methods.required.call(
              this, value, element, parameters);
        }

        return true;
    }
);
//adapters.add("customcomparestring", ["other"], function (options) {
//    var prefix = getModelPrefix(options.element.name),
//        other = options.params.other,
//        fullOtherName = appendModelPrefix(other, prefix),
//        element = $(options.form).find(":input").filter("[name='" + escapeAttributeValue(fullOtherName) + "']")[0];

//    setValidationValues(options, "equalTo", element);
//});


$.validator.unobtrusive.adapters.add(
                'customcomparestring',
                ['validationgroup', 'other'],
                function (options) {                    
                    var customprefix = options.element.name.substr(0, options.element.name.lastIndexOf(".") + 1);
                    var customother = options.params.other;
                    var customfullOtherName = CustomappendModelPrefix(customother, customprefix);
                    var costomelement = $(options.form).find(":input").filter("[name='" + CustomescapeAttributeValue(customfullOtherName) + "']")[0];

                    options.rules['customcomparestring'] = {
                        validationgroup: options.params['validationgroup'],
                        errormessages: options.params['errormessages'],
                        other: costomelement
                    };
                    options.messages['customcomparestring'] = options.message;
                });

$.validator.addMethod('customcomparestring',
    function (value, element, parameters) {

        if (ClickedSubmit === parameters['validationgroup']) {

            return $.validator.methods.equalTo.call(
              this, value, element, parameters['other']);
        }


        if (ClickedSubmit === parameters['errormessages']) {
            return $.validator.methods.equalTo.call(
              this, value, element, parameters['other']);
        }

        return true;
    }
);


//Custom Regular Expression validation

$.validator.unobtrusive.adapters.add(
                'customregularexpression',
                ['validationgroup', 'pattern'],
                function (options) {
                    options.rules['customregularexpression'] = {
                        validationgroup: options.params['validationgroup'],
                        pattern: options.params['pattern']
                    };
                    options.messages['customregularexpression'] = options.message;
                });

$.validator.addMethod('customregularexpression', function (value, element, parameters) {
    if (value != "") {
        var pattern = new RegExp(parameters['pattern']);
        if (ClickedSubmit === parameters['validationgroup']) {
            return pattern.test(value);
        }
    }
    return true;
});

//Custom Email Validation

$.validator.unobtrusive.adapters.add(
                'customemail',
                ['validationgroup'],
                function (options) {
                    options.rules['customemail'] = {
                        validationgroup: options.params['validationgroup']
                    };
                    options.messages['customemail'] = options.message;
                });

$.validator.addMethod('customemail',
    function (value, element, parameters) {

        if (value != "") {

            if (ClickedSubmit === parameters['validationgroup']) {

                return $.validator.methods.email.call(
                  this, value, element, parameters);
            }
        }
        return true;
    }
);

//Custom range Validation

$.validator.unobtrusive.adapters.add(
                'customrange',
                ['validationgroup', 'minimum', 'maximum'],
                function (options) {
                    options.rules['customrange'] = {
                        validationgroup: options.params['validationgroup'],
                        minimum: options.params['minimum'],
                        maximum: options.params['maximum']

                    };
                    options.messages['customrange'] = options.message;
                });

$.validator.addMethod('customrange',
    function (value, element, parameters) {
        if (value != "") {
            if (ClickedSubmit === parameters['validationgroup']) {
                return $.validator.methods.range.call(
                    this, parseInt(value), element, [parseInt(parameters["minimum"]), parseInt(parameters["maximum"])]);
            }
        }
        return true;
    }
);

//Custom string length Validation

$.validator.unobtrusive.adapters.add(
                'customstringlength',
                ['validationgroup', 'maximum', 'minimum'],
                function (options) {
                    options.rules['customstringlength'] = {
                        validationgroup: options.params['validationgroup'],
                        maximum: options.params['maximum'],
                        minimum: options.params['minimum']

                    };
                    options.messages['customstringlength'] = options.message;
                });

$.validator.addMethod('customstringlength',
    function (value, element, parameters) {

        if (value != "") {
            if (ClickedSubmit === parameters['validationgroup']) {

                return $.validator.methods.rangelength.call(
                  this, value, element, [parameters["minimum"], parameters["maximum"]]);
            }
        }
        return true;
    }
);




(function ($) {

    //re-set all client validation given a jQuery selected form or child
    $.fn.resetValidation = function () {

        var $form = this.closest('form');

        //reset jQuery Validate's internals
        $form.validate().resetForm();

        //reset unobtrusive validation summary, if it exists
        $form.find("[data-valmsg-summary=true]")
            .removeClass("validation-summary-errors")
            .addClass("validation-summary-valid")
            .find("ul").empty();

        //reset unobtrusive field level, if it exists
        $form.find("[data-valmsg-replace]")
            .removeClass("field-validation-error")
            .addClass("validation-error")
            .append('<i class="glyphicon glyphicon-exclamation-sign"></i>');

        return $form;
    };

    //reset a form given a jQuery selected form or a child
    //by default validation is also reset
    $.fn.formReset = function (resetValidation) {
        var $form = this.closest('form');


        $form[0].reset();

        $form.find('.invalid-input').each(function () {
            $(this).removeClass('invalid-input');
        })

        if (resetValidation == undefined || resetValidation) {
            $form.resetValidation();
        }

        return $form;
    }
})(jQuery);
