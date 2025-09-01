function CalenderIsDate(e) {
    var evt = (e) ? e : window.event; var charCode = (evt.keyCode) ? evt.keyCode : evt.which; if (charCode == 47) { return !0 }
    if (charCode > 31 && (charCode < 48 || charCode > 57)) { return !1 }
    return !0
}; function Calender(CalendarControlId, hdnIdToSaveGregorianDate, hdnIdToSaveUmmalquraDate, calendarType, languageShortName, defaultDate) {
    var dateFormat = "dd/mm/yyyy"; var toGregorian; var toUmmalqura; var changeCalendar; if (languageShortName == 'en') { toGregorian = "Gregorian"; toUmmalqura = "Ummalqura"; $.calendarsPicker.setDefaults($.calendarsPicker.regionalOptions['']) }
    else { toGregorian = "ميلادي"; toUmmalqura = "هجري"; $.calendarsPicker.setDefaults($.calendarsPicker.regionalOptions.ar) }
    if (calendarType == "ummalqura") { changeCalendar = toGregorian }
    else { changeCalendar = toUmmalqura }
    var theCalendar = calendarType; $('#' + CalendarControlId).calendarsPicker({
        calendar: $.calendars.instance(calendarType, languageShortName), changeText: changeCalendar, showSpeed: 'fast', showOptions: 'none', dateFormat: dateFormat, onSelect: function (date) {
            var type = $('#' + CalendarControlId)[0].name; if (type == "gregorian") { oldDate = $('#' + hdnIdToSaveGregorianDate).val() }
            else { oldDate = $('#' + hdnIdToSaveUmmalquraDate).val() }
            if (typeof date[0] != "undefined") {
                if ($.calendars.instance(type)._validateLevel === 0 && !$.calendars.instance(type).isValid(date[0].formatYear(), date[0].month(), date[0].day())) { $("#" + CalendarControlId).val(''); $('#' + hdnIdToSaveUmmalquraDate).val(''); $('#' + hdnIdToSaveGregorianDate).val('') }
                else {
                    var jd = $.calendars.instance(type).newDate(parseInt(date[0].formatYear(), 10), parseInt(date[0].month(), 10), parseInt(date[0].day(), 10)).toJD(); if (type == "gregorian") {
                        var newCal = $.calendars.instance("ummalqura")
                        var newDate = newCal.fromJD(jd); $('#' + hdnIdToSaveGregorianDate).val(date[0].day() + '/' + date[0].month() + '/' + date[0].formatYear()); $('#' + hdnIdToSaveUmmalquraDate).val(newCal.formatDate(dateFormat, newDate))
                    }
                    else {
                        var newCal = $.calendars.instance("gregorian")
                        var newDate = newCal.fromJD(jd); $('#' + hdnIdToSaveUmmalquraDate).val(date[0].day() + '/' + date[0].month() + '/' + date[0].formatYear()); $('#' + hdnIdToSaveGregorianDate).val(newCal.formatDate(dateFormat, newDate))
                    }
                }
            }
            else { $('#' + hdnIdToSaveUmmalquraDate).val(""); $('#' + hdnIdToSaveGregorianDate).val("") }
            if (oldDate != date[0].day() + '/' + date[0].month() + '/' + date[0].formatYear())
                $('#' + CalendarControlId).trigger('change')
        }
    }); if (defaultDate != null && defaultDate != "") {
        $('#' + CalendarControlId).val(defaultDate); if (calendarType == "gregorian") { $('#' + hdnIdToSaveGregorianDate).val(defaultDate); DateConverter(defaultDate, 'gregorian', 'ummalqura', hdnIdToSaveUmmalquraDate) }
        else { $('#' + hdnIdToSaveUmmalquraDate).val(defaultDate); DateConverter(defaultDate, 'ummalqura', 'gregorian', hdnIdToSaveGregorianDate) }
    }
    else { $('#' + hdnIdToSaveGregorianDate).val(''); $('#' + hdnIdToSaveUmmalquraDate).val('') }
    $(document).on("focusout", "#" + CalendarControlId, function () {
        var type = $('#' + CalendarControlId)[0].name; var date = $("#" + CalendarControlId).val(); var oldDate; if (type == "gregorian") { oldDate = $('#' + hdnIdToSaveGregorianDate).val() }
        else { oldDate = $('#' + hdnIdToSaveUmmalquraDate).val() }
        if (date != "") {
            var splitDate = date.split("/"); if ($.calendars.instance(type)._validateLevel === 0 && !$.calendars.instance(type).isValid(splitDate[2], splitDate[1], splitDate[0])) { date = ''; $("#" + CalendarControlId).val(''); $('#' + hdnIdToSaveUmmalquraDate).val(''); $('#' + hdnIdToSaveGregorianDate).val('') }
            else {
                var jd = $.calendars.instance(type).newDate(parseInt(splitDate[2], 10), parseInt(splitDate[1], 10), parseInt(splitDate[0], 10)).toJD(); if (type == "gregorian") {
                    var newCal = $.calendars.instance("ummalqura")
                    var newDate = newCal.fromJD(jd); $('#' + hdnIdToSaveGregorianDate).val(splitDate[0] + '/' + splitDate[1] + '/' + splitDate[2]); $('#' + hdnIdToSaveUmmalquraDate).val(newCal.formatDate(dateFormat, newDate))
                }
                else {
                    var newCal = $.calendars.instance("gregorian")
                    var newDate = newCal.fromJD(jd); $('#' + hdnIdToSaveUmmalquraDate).val(splitDate[0] + '/' + splitDate[1] + '/' + splitDate[2]); $('#' + hdnIdToSaveGregorianDate).val(newCal.formatDate(dateFormat, newDate))
                }
            }
        }
        else { $('#' + hdnIdToSaveUmmalquraDate).val(""); $('#' + hdnIdToSaveGregorianDate).val("") }
        if (oldDate != date)
            $('#' + CalendarControlId).trigger('change')
    })
}
function __CalendarChangeType(inst) {
    var type = $('#' + inst.elem[0].id)[0].name; var date = $('#' + inst.elem[0].id).val(); var toGregorian = "Gregorian"; var toUmmalqura = "Ummalqura"; var languageShortName = 'en'; var english = /^[A-Za-z0-9]*$/; var splitDate = date.split("/"); if (!english.test(inst.options.changeText)) { toGregorian = "ميلادي"; toUmmalqura = "هجري"; languageShortName = 'ar' }
    if ($.calendars.instance(type)._validateLevel === 0 && !$.calendars.instance(type).isValid(splitDate[2], splitDate[1], splitDate[0])) { date = ''; $("#" + inst.elem[0].id).val('') }
    switch (type) {
        case "ummalqura": {
            if (date != '')
                DateConverter(date, 'ummalqura', 'gregorian', inst.elem[0].id); $('#' + inst.elem[0].id).calendarsPicker('option', { calendar: $.calendars.instance('gregorian', languageShortName), changeText: toUmmalqura }); $("#" + inst.elem[0].id).attr('name', 'gregorian')
        }
            break; case "gregorian": {
                if (date != '')
                    DateConverter(date, 'gregorian', 'ummalqura', inst.elem[0].id); $('#' + inst.elem[0].id).calendarsPicker('option', { calendar: $.calendars.instance('ummalqura', languageShortName), changeText: toGregorian }); $("#" + inst.elem[0].id).attr('name', 'ummalqura')
            }
            break
    }
}
function SetDate(calendarControlId, hdnIdToSaveGregorianDate, hdnIdToSaveUmmalquraDate, newDate) {
    $('#' + hdnIdToSaveGregorianDate).val(newDate); DateConverter(newDate, "gregorian", "ummalqura", hdnIdToSaveUmmalquraDate); if ($('#' + calendarControlId)[0].name != "gregorian") { newDate = $('#' + hdnIdToSaveUmmalquraDate).val() }
    $('#' + calendarControlId).val(newDate); $('#' + calendarControlId).calendarsPicker('option', { defaultDate: newDate })
}
function SetStartEndDate(year, calendarFromId, calendarToId, type) {
    var fromType = $('#' + calendarFromId)[0].name; var toType = $('#' + calendarToId)[0].name; var fromDate = '01/01/' + year; var toDate; var to; var today = $.calendars.instance(type).formatDate('dd/mm/yyyy', $.calendars.instance(type).today()); var splitToday = today.split("/"); switch (type) {
        case "ummalqura": {
            if ($.calendars.instance(type)._validateLevel === 0 && !$.calendars.instance(type).isValid(year, 12, 30)) { toDate = '29/12/' + year }
            else { toDate = '30/12/' + year }
            to = "gregorian"
        }
            break; case "gregorian": { toDate = '31/12/' + year; to = "ummalqura" }
            break
    }
    if (splitToday[2] == year) { toDate = today }
    if (fromType == type) { $('#' + calendarFromId).val(fromDate) }
    else { DateConverter(fromDate, type, to, calendarFromId) }
    if (toType == type) { $('#' + calendarToId).val(toDate) }
    else { DateConverter(toDate, type, to, calendarToId) }
    $('#' + calendarFromId).trigger('focusout'); $('#' + calendarToId).trigger('focusout')
}
function DateConverter(date, from, to, hdnIdToStoreTheConvertedDate) { var splitDate = date.split("/"); var jd = $.calendars.instance(from).newDate(parseInt(splitDate[2], 10), parseInt(splitDate[1], 10), parseInt(splitDate[0], 10)).toJD(); var newCal = $.calendars.instance(to); var newDate = newCal.fromJD(jd); $('#' + hdnIdToStoreTheConvertedDate).val(newCal.formatDate("dd/mm/yyyy", newDate)) }