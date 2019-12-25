$.ajaxSetup({ cache: false });
$calanderDialogRnge = "";


var SelectedDay = 1;
var _dtObject;
var _customSearchObject;
var _customerSearchObject;
var isOpen = 0;

var date;
//$(document).on('click', function () {
//   
//});
$(document).keydown(function (e) {

    e.stopImmediatePropagation();
    if (e.keyCode == 27) { // ESC            
        if ($calanderDialogRnge != "") {
            if (!$(e.target).is('.dialog_calander') && !$(e.target).parents().is('.dialog_calander')) {
                CloseCalanderDialogRange();
            }
        }
    }
});

$(document).mousedown(function (e) {
    e.stopImmediatePropagation();
    var clicked = $(e.target); // get the element clicked  

    if ($calanderDialogRnge != "") {
        if (!clicked.is('.date-range-default') && !clicked.parents().is('.date-range-default')) {
            isOpen = 0;
            CloseCalanderDialogRange();

        }
       
    }
    //if ($calanderDialog != "") {
    //    if (!$(e.target).is('.dialog_calander') && !$(e.target).parents().is('.dialog_calander')) {
    //        CloseCalanderDialog();
    //    }
    //}
});

function CloseCalanderDialogRange() {
    $calanderDialogRnge.parentNode.removeChild($calanderDialogRnge);
    $calanderDialogRnge = "";
}

function ShowCalanderRange(html, left, top, dtype) {

    $calanderDialogRnge = document.createElement("div");
    var title = "Calendar";

    $calanderDialogRnge.setAttribute("id", "dialog_calander");
    $calanderDialogRnge.setAttribute("class", "dialog_calander panel panel-default1 date-range-default");
    $calanderDialogRnge.setAttribute("style", "z-index:1000001;");

    $calanderDialogRnge.setAttribute("style", "left:" + left + "px;top:" + top + "px;width:520px;");
    if (dtype == 1) {
        $calanderDialogRnge.innerHTML = $calanderDialogRnge.innerHTML + "<select class='form-header BsAdRange'> <option value='1'selected>AD</option><option value='2'>BS</option></select>";
    }
    else {
        $calanderDialogRnge.innerHTML = $calanderDialogRnge.innerHTML + "<select class='form-header BsAdRange'> <option value='1'>AD</option><option value='2'selected>BS</option></select>";
    }
    $calanderDialogRnge.innerHTML = $calanderDialogRnge.innerHTML + " <span class='fc-header-title'> <button type='button' class='btn btn-default chCloseReg'><i class='fa fa-close'></i></button></span>";
    $calanderDialogRnge.innerHTML = $calanderDialogRnge.innerHTML + "<hrLineCalendar>";
    $calanderDialogRnge.innerHTML = $calanderDialogRnge.innerHTML + "<div class='calander_container' >" + html + "<" + "/div>"

    document.body.appendChild($calanderDialogRnge);

    $('.date-range-default').hide().fadeIn(500);
}

$(document).on('click', '.chCloseReg', function (e) {
    e.stopImmediatePropagation();
    CloseCalanderDialogRange();
    isOpen = 0;
});

$(document).on('click', '.dpCalBtnRange', function (e) {
  
    //if ($(document).find('.dialog_calander') != undefined)
    //{
    //    CloseCalanderDialogRange();
    //}
    // 

    if (isOpen == 1) {
        CloseCalanderDialogRange();
    }
    isOpen = 1;
    e.stopImmediatePropagation();
    _dtObject = $(this).closest('.chdPickerRange');

    var windowwidth = $(window).width();
    var windowheight = $(window).height();
   
    var DType = _dtObject.find('.dpDFormatRange').find('#DateTypeRange').val();
    var DateFrom = _dtObject.find('.txtDateFrom').val();
    var DateTo = _dtObject.find('.txtDateTo').val();

    date = DateFrom;
    var pos = _dtObject.offset(); var left = 0; var top = 0; var right = 0; var bottom = 0;


    if ((windowwidth - pos.left) < 520) {

        left = windowwidth - 600;
    }
    else {
        left = pos.left;
    }
    if ((windowheight - pos.top) < 275) {
        top = pos.top - 282;
    }
    else {
        top = pos.top + 35;
    }
    bottom = top + 300;
    $.ajax({
        url: "/DatePicker/DatePickerRangeIndex",
        type: "Get",
        data: { fromDate: DateFrom, toDate: DateTo, dateType: DType.toString() },
        dataType: "html",
        success: function (html) {
            ShowCalanderRange(html, left, top, DType)
        },
        error: function (xhr, ajaxOptions, thrownError) {
            ErrorAlert("An error has occured:\n" + xhr.status + "\n" + ajaxOptions + "\n" + thrownError, 5000);

        }
    });
});

$(document).on('click', '.fc-button-prevRange', function (e) {

    e.stopImmediatePropagation();
    var dType = _dtObject.find('.dpDFormatRange').find('#DateTypeRange').val();
    var mn = $(this).parents('.date-range-default').find("#Months").val();
    var yr = $(this).parents('.date-range-default').find("#Years").val();
    var maxdays = $(this).parents('.date-range-default').find("#Maxdate").val();
    var current = $(this);
    if (mn == '1') {
        yr = parseInt(yr) - 1;
        mn = 12;
    } else {
        mn = parseInt(mn) - 1
    }
    var type = 1;
    DateRageForFirst(yr, mn, dType, current, type, maxdays);

});

$(document).on('click', '.fc-button-nextRange', function (e) {
    e.stopImmediatePropagation();
    var dType = _dtObject.find('.dpDFormatRange').find('#DateTypeRange').val();
    var mn = $(this).parents('.dialog_calander').find("#Months").val();
    var yr = $(this).parents('.dialog_calander').find("#Years").val();
    var maxdays = $(this).parents('.dialog_calander').find("#Maxdate").val();
    var current = $(this);
    if (mn == '12') {
        yr = parseInt(yr) + 1;
        mn = 1;
    } else {
        mn = parseInt(mn) + 1
    }
    var type = 1;
    DateRageForFirst(yr, mn, dType, current, type, maxdays);
});

$(document).on('click', '.fc-button-prevRange1', function (e) {

    e.stopImmediatePropagation();
    var dType = _dtObject.find('.dpDFormatRange').find('#DateTypeRange').val();
    var mn = $(this).parents('.date-range-default').find("#MonthTo").val();
    var yr = $(this).parents('.date-range-default').find("#YearTo").val();
    var mindays = $(this).parents('.date-range-default').find("#MinDate").val();

    var current = $(this);
    if (mn == '1') {
        yr = parseInt(yr) - 1;
        mn = 12;
    } else {
        mn = parseInt(mn) - 1
    }
    var type = 2;
    DateRageForFirst(yr, mn, dType, current, type, mindays);

});

$(document).on('click', '.fc-button-nextRange1', function (e) {
    e.stopImmediatePropagation();
    var dType = _dtObject.find('.dpDFormatRange').find('#DateTypeRange').val();
    var mn = $(this).parents('.date-range-default').find("#MonthTo").val();
    var yr = $(this).parents('.date-range-default').find("#YearTo").val();
    var mindays = $(this).parents('.date-range-default').find("#MinDate").val();
    var current = $(this);
    if (mn == '12') {
        yr = parseInt(yr) + 1;
        mn = 1;
    } else {
        mn = parseInt(mn) + 1
    }
    var type = 2;
    DateRageForFirst(yr, mn, dType, current, type, mindays);
});

$(document).on('click', '.calanderDayRange', function (e) {

    e.stopImmediatePropagation();
    // $('.dialog_calander').fadeOut(300);

    if (_dtObject.attr("readonlytag") == "True") {
        InfoAlert("Date Object Is Read Only! Cannot Change Date.", 2000)
        CloseCalanderDialogRange();
        return;
    }

    var newDate;
    var result;

    //var month; var year; var dType;
    SelectedDay = $(this).attr("id");
    var month = $(this).parents('.date-range-default').find("#Months").val();
    var year = $(this).parents('.date-range-default').find("#Years").val()

    var fromDateAd = $(this).parents('.date-range-default').find(".fc-header-title").find(".fromDateAd");
    var fromDateBs = $(this).parents('.date-range-default').find(".fc-header-title").find(".fromDateBs");
    var toDateAd = $(this).parents('.date-range-default').find(".fc-header-title").find(".toDateAd");
    var toDateBs = $(this).parents('.date-range-default').find(".fc-header-title").find(".toDateBs");
    var engDateFrom = $(this).parents('.date-range-default').find(".fc-header-title").find("#EnglishDateFrom");
    var engDateTo = $(this).parents('.date-range-default').find(".fc-header-title").find("#EnglishDateTo");
    var minDate = $(this).parents('.date-range-default').find(".fc-header-title").find("#MinDate");

    var dType = _dtObject.find('.dpDFormatRange').find('#DateTypeRange').val();
    var me = $(this);
    //var ADCntrl = _dtObject.find('.txtDateADRange');
    //var BSCntrl = _dtObject.find('.txtDateBSRange')
    //var DTCntrl = _dtObject.find('.txtDateValueRange')

    $.ajax({
        url: "/DatePicker/GetDate",
        type: "Get",
        data: { year: year, month: month, dateType: dType, day: SelectedDay },
        dataType: "json",
        success: function (data) {

            result = data;
            fromDateAd.val(result.DateAD);
            fromDateBs.val(result.DateBS);
            engDateFrom.val(result.Date);
            var mindate = minDate.val(result.Date);
            //  DateChangedFunctionRange(_dtObject);
            me.parents('.date-range-default').find("#MonthTo").val(month);
            me.parents('.date-range-default').find("#YearTo").val(year);
            DateByYearMonthChangeRange(year, month, dType, SelectedDay, mindate.val(), 1)
            isOpen = 1;
        },
        error: function (xhr, ajaxOptions, thrownError) {
            CloseCalanderDialogRange();
            ErrorAlert("Error No:\n" + xhr.status + "\n, Error Type:" + ajaxOptions + "\n, Error Description:" + thrownError, 5000);
        }
    });


    // CloseCalanderDialogRange();

    if (dType == 1) {
        fromDateAd.focus();
    } else {
        fromDateBs.focus();
    }
});

$(document).on('click', '.calanderDayRange1', function (e) {

    e.stopImmediatePropagation();
    // $('.dialog_calander').fadeOut(300);

    if (_dtObject.attr("readonlytag") == "True") {
        InfoAlert("Date Object Is Read Only! Cannot Change Date.", 2000)
        CloseCalanderDialogRange();
        return;
    }


    var newDate;
    var result;

    var month; var year; var dType;
    SelectedDay = $(this).attr("id");
    var month = $(this).parents('.date-range-default').find("#MonthTo").val();
    var year = $(this).parents('.date-range-default').find("#YearTo").val()

    var fromDateAd = $(this).parents('.date-range-default').find(".fc-header-title").find(".fromDateAd");
    var fromDateBs = $(this).parents('.date-range-default').find(".fc-header-title").find(".fromDateBs");
    var toDateAd = $(this).parents('.date-range-default').find(".fc-header-title").find(".toDateAd");
    var toDateBs = $(this).parents('.date-range-default').find(".fc-header-title").find(".toDateBs");
    var engDateFrom = $(this).parents('.date-range-default').find(".fc-header-title").find("#EnglishDateFrom");
    var engDateTo = $(this).parents('.date-range-default').find(".fc-header-title").find("#EnglishDateTo");
    dType = _dtObject.find('.dpDFormatRange').find('#DateTypeRange').val();
 
    var maxDate = $(this).parents('.date-range-default').find(".fc-header-title").find("#Maxdate");
    var me = $(this);
    //var ADCntrl = _dtObject.find('.txtDateADRange');
    //var BSCntrl = _dtObject.find('.txtDateBSRange')
    //var DTCntrl = _dtObject.find('.txtDateValueRange')

    $.ajax({
        url: "/DatePicker/GetDate",
        type: "Get",
        data: { year: year, month: month, dateType: dType, day: SelectedDay },
        dataType: "json",
        success: function (data) {

            result = data;
            newDate = formatDate(result.Date)
            toDateAd.val(result.DateAD);
            toDateBs.val(result.DateBS);
            engDateTo.val(result.Date);
            maxDate.val(result.Date);
            // DateChangedFunctionRange(_dtObject);
            me.parents('.date-range-default').find("#Months").val(month);
            me.parents('.date-range-default').find("#Years").val(year)
            DateByYearMonthChangeRange(year, month, dType, SelectedDay, maxDate.val(), 2)

        },
        error: function (xhr, ajaxOptions, thrownError) {
            CloseCalanderDialogRange();
            ErrorAlert("Error No:\n" + xhr.status + "\n, Error Type:" + ajaxOptions + "\n, Error Description:" + thrownError, 5000);
        }
    });
    if (dType == 1) {
        toDateAd.focus();
    } else {
        toDateBs.focus();
    }
    isOpen = 1;
});

$(document).on('click', '.Apply', function (e) {

    e.stopImmediatePropagation();
    $('.date-range-default').fadeOut(300);
    var fromDateAd = $(this).parents('.date-range-default').find(".fc-header-title").find(".fromDateAd").val();
    var fromDateBs = $(this).parents('.date-range-default').find(".fc-header-title").find(".fromDateBs").val();
    var toDateAd = $(this).parents('.date-range-default').find(".fc-header-title").find(".toDateAd").val();
    var toDateBs = $(this).parents('.date-range-default').find(".fc-header-title").find(".toDateBs").val();
    var engDateFrom = $(this).parents('.date-range-default').find(".fc-header-title").find("#EnglishDateFrom").val();
    var engDateTo = $(this).parents('.date-range-default').find(".fc-header-title").find("#EnglishDateTo").val();
    _dtObject.find('.txtDateADRange').val(fromDateAd + "|" + toDateAd);
    _dtObject.find('.txtDateBSRange').val(fromDateBs + "|" + toDateBs);
    _dtObject.find('.txtDateFrom').val(engDateFrom);
    _dtObject.find('.txtDateTo').val(engDateTo);
    CloseCalanderDialogRange();
    isOpen = 0;
});

$(document).on('change', '.yearMonthFrom', function (e) {
    e.stopImmediatePropagation();
    var dType = _dtObject.find('.dpDFormatRange').find('#DateTypeRange').val();
    var mn = $(this).parents('.date-range-default').find("#Months").val();
    var yr = $(this).parents('.date-range-default').find("#Years").val();
    var maxdays = $(this).parents('.date-range-default').find("#Maxdate").val();

    var current = $(this);
    var type = 1;
    DateRageForYearMonthChange(yr, mn, dType, current, type, maxdays);
});

$(document).on('change', '.yearMonthTo', function (e) {
    e.stopImmediatePropagation();
    var dType = _dtObject.find('.dpDFormatRange').find('#DateTypeRange').val();
    var mn = $(this).parents('.date-range-default').find("#MonthTo").val();
    var yr = $(this).parents('.date-range-default').find("#YearTo").val();
    var maxdays = $(this).parents('.date-range-default').find("#MinDate").val();
    var current = $(this);
    var type = 2;
    DateRageForYearMonthChange(yr, mn, dType, current, type, maxdays);
});

$('.dpDFormatRange').on("change", "#DateTypeRange", function (e) {
    e.stopImmediatePropagation();
    _dtObject = $(this).closest('.chdPickerRange');
    var selected = $(this).val();
    var allowTypeChange = $(_dtObject).attr("allowdtypechange");
    if (allowTypeChange == "Yes") {
        if (selected == "1") {
            _dtObject.find('.txtDateADRange').show();
            _dtObject.find('.txtDateBSRange').hide();
            _dtObject.find('.txtDateADRange').focus();
            //$(this).html('AD');
            //$(this).attr("id", "1");
        }
        else {
            _dtObject.find('.txtDateADRange').hide();
            _dtObject.find('.txtDateBSRange').show();
            _dtObject.find('.txtDateBSRange').focus();
            //$(this).html('BS');
            //$(this).attr("id", "2");
        }
        DateChangedFunctionRange(_dtObject);
    }
    else {
        InfoAlert("You cannot change date type in this transaction mode!", 5000);
    }
});

$(document).on('change', '.BsAdRange', function (e) {
    e.stopImmediatePropagation();
    var selected = $(this).val();

    var allowTypeChange = $(_dtObject).attr("allowdtypechange");
    if (allowTypeChange == "Yes") {
        if (selected == "1") {
            _dtObject.find('.txtDateADRange').show();
            _dtObject.find('.txtDateBSRange').hide();
            _dtObject.find('.txtDateADRange').focus();
            //_dtObject.find('.calDateTypeRange').html('AD');
            //_dtObject.find('.calDateTypeRange').attr("id", "1");
            _dtObject.find('.dpDFormatRange').find('#DateTypeRange').val(1);
        }
        else {
            _dtObject.find('.txtDateADRange').hide();
            _dtObject.find('.txtDateBSRange').show();
            _dtObject.find('.txtDateBSRange').focus();
            _dtObject.find('.dpDFormatRange').find('#DateTypeRange').val(2);
            //_dtObject.find('.calDateTypeRange').html('BS');
            //_dtObject.find('.calDateTypeRange').attr("id", "2");
        }
        DateChangedFunctionRange(_dtObject);
    }
    else {
        InfoAlert("You cannot change date type in this transaction mode!", 5000);
        return;
    }



    var engDateFrom = $(this).parents('.date-range-default').find(".fc-header-title").find("#EnglishDateFrom").val();
    var engDateTo = $(this).parents('.date-range-default').find(".fc-header-title").find("#EnglishDateTo").val();

    $.ajax({
        url: "/DatePicker/DatePickerRangeIndex",
        type: "Get",
        data: { fromDate: engDateFrom, toDate: engDateTo, dateType: selected.toString() },
        dataType: "html",
        success: function (html) {
            $('.calander_container').empty();
            $('.calander_container').html(html);
        },
        error: function (xhr, ajaxOptions, thrownError) {
            ErrorAlert("An error has occured:\n" + xhr.status + "\n" + ajaxOptions + "\n" + thrownError, 5000);

        }
    });

});

$(document).on('change', '.fromDateAd#FromDateAD', function (e) {
    e.stopImmediatePropagation();
    var _dtObject = $(this);
    SetDateRange(_dtObject, $(this).val(), 1);
});

$(document).on('change', '.toDateAd#ToDateAD', function (e) {
    e.stopImmediatePropagation();
    var _dtObject = $(this);
    SetDateRange(_dtObject, $(this).val(), 2);
});

$(document).on('change', '.fromDateBs#FromDateBS', function (e) {
    e.stopImmediatePropagation();
    var _dtObject = $(this);
    SetDateRange(_dtObject, $(this).val(), 1);
});

$(document).on('change', '.toDateBs#ToDateBS', function (e) {
    e.stopImmediatePropagation();
    var _dtObject = $(this);
    SetDateRange(_dtObject, $(this).val(), 2);
});

$(document).on('click', '.Cancel', function (e) {

    e.stopImmediatePropagation();
    //_dtObject = $(this).closest('.chdPickerRange');
    var DType = _dtObject.find('.dpDFormatRange').find('#DateTypeRange').val();

    var DateFrom = _dtObject.find('.txtDateFrom').val();
    var DateTo = _dtObject.find('.txtDateTo').val();
    $.ajax({
        url: "/DatePicker/DatePickerRangeIndex",
        type: "Get",
        data: { fromDate: DateFrom, toDate: DateTo, dateType: DType.toString() },
        dataType: "html",
        success: function (html) {
            $('.calander_container').html("");
            $('.calander_container').html(html);
        },
        error: function (xhr, ajaxOptions, thrownError) {
            ErrorAlert("An error has occured:\n" + xhr.status + "\n" + ajaxOptions + "\n" + thrownError, 5000);

        }
    });
});

$('.txtDateADRange#DateADRange').on('change', function (e) {

    debugger;
    e.stopImmediatePropagation();
    _dtObject = $(this).closest('.chdPickerRange');
    var date = $(this).val();
    var fromDate = formatDate(_dtObject.find('.txtDateFrom').val());
    var toDate = formatDate(_dtObject.find('.txtDateTo').val());

    var splitDate = date.split("|");
    if (splitDate[0] == undefined) {
        SetDateRengeValue1(_dtObject, fromDate, toDate)
        var errMsg = "Invalid Date!";
        ErrorAlert(errMsg, 5000);
    }
    else if (splitDate[1] == undefined) {
        SetDateRengeValue1(_dtObject, fromDate, toDate)
        var errMsg = "Invalid Date!";
        ErrorAlert(errMsg, 5000);
    } else {
        SetDateRengeValue1(_dtObject, splitDate[0], splitDate[1])
    }
});

$('.txtDateBSRange#DateBSRange').on('change', function (e) {
   
    e.stopImmediatePropagation();
    var date = $(this).val();
    var me = $(this);
    _dtObject = $(this).closest('.chdPickerRange');
    var fromDate = formatDate(_dtObject.find('.txtDateFrom').val());
    var toDate = formatDate(_dtObject.find('.txtDateTo').val());
    var splitDate = date.split("|");

    if (splitDate[0] == undefined) {

        BsDateRange(me, fromDate, toDate);
        var errMsg = "Invalid Date!";
        ErrorAlert(errMsg, 5000);
    }
    else if (splitDate[1] == undefined) {

        BsDateRange(me, fromDate, toDate)
        var errMsg = "Invalid Date!";
        ErrorAlert(errMsg, 5000);
    } else {
        SetDateRengeValue1(_dtObject, splitDate[0], splitDate[1])
    }

});

function SetDateRengeValue1(dtObject, from, to) {
    var DType = dtObject.find('.dpDFormatRange').find('#DateTypeRange').val();
    //var DType = dtObject.find('.calDateTypeRange').attr("id");
    var fromDate = formatDate(dtObject.find('.txtDateFrom').val());
    var toDate = formatDate(dtObject.find('.txtDateTo').val());
    var newDateFrom; var result; var newDateTo

    var ADCntrl = dtObject.find('.txtDateADRange');
    var BSCntrl = dtObject.find('.txtDateBSRange')
    var dateFrom = dtObject.find('.txtDateFrom');
    var dateTo = dtObject.find('.txtDateTo');


    $.ajax({
        url: "/DatePicker/CheckDateFormatRange",
        type: "Get",
        data: { dateType: DType, fromString: from, toDateString: to },
        dataType: "json",
        success: function (r) {

            result = r;
            newDateFrom = formatDate(result.FromDate)
            newDateTo = formatDate(result.ToDate)
            if (newDateTo < fromDate || newDateFrom > toDate) {
                var errMsg = "Invalid Date Range!<br> Date Must Between [" + fromDate + "] and [" + toDate + "]";
                ErrorAlert(errMsg, 5000);

                newDateFrom = fromDate;
                newDateTo = toDate;
                $.ajax({
                    url: "/DatePicker/GetDateBSAndADRange",
                    type: "Get",
                    data: { fromDate: newDateFrom, toDate: newDateTo },
                    dataType: "json",
                    success: function (r1) {

                        result = r1;
                        ADCntrl.val(result.FromDateAD + "|" + result.ToDateAD);
                        BSCntrl.val(result.FromDateBS + "|" + result.ToDateBS);
                        dateFrom.val(result.FromDate);
                        dateTo.val(result.ToDate);
                        return;
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        ErrorAlert("An error has occured:\n" + xhr.status + "\n" + ajaxOptions + "\n" + thrownError, 5000);
                    }
                });
            } else {


                ADCntrl.val(result.FromDateAD + "|" + result.ToDateAD);
                BSCntrl.val(result.FromDateBS + "|" + result.ToDateBS);
                dateFrom.val(result.FromDate);
                dateTo.val(result.ToDate);
                DateChangedFunctionRange(dtObject);
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            ErrorAlert("An error has occured:\n" + xhr.status + "\n" + ajaxOptions + "\n" + thrownError, 5000);
        }
    });
}

function SetDateRangeValue(dtObject, dtValue, type) {

    var DType = dtObject.parents('.date-range-default').find('.BsAdRange option:selected').val()
    var Date;

    if (type == 1) {
        Date = formatDate(dtObject.parents('.date-range-default').find(".fc-header-title").find("#EnglishDateTo").val());

    } else {
        Date = formatDate(dtObject.parents('.date-range-default').find(".fc-header-title").find("#EnglishDateFrom").val())
    }

    var newDate; var result;

    var fromDate = dtObject.parents('.date-range-default').find(".fc-header-title").find("#EnglishDateFrom");
    var toDate = dtObject.parents('.date-range-default').find(".fc-header-title").find("#EnglishDateTo");
    var fromDateAd = dtObject.parents('.date-range-default').find(".fc-header-title").find(".fromDateAd");
    var fromDateBs = dtObject.parents('.date-range-default').find(".fc-header-title").find(".fromDateBs");
    var toDateAd = dtObject.parents('.date-range-default').find(".fc-header-title").find(".toDateAd");
    var toDateBs = dtObject.parents('.date-range-default').find(".fc-header-title").find(".toDateBs");
    var Istrue = false;
    $.ajax({
        url: "/DatePicker/CheckDateFormat",
        type: "Get",
        data: { dateType: DType, DateString: dtValue },
        dataType: "json",
        success: function (r) {
            result = r;
            newDate = formatDate(result.Date)
            if (type == 1) {
                if (newDate > Date) {
                    Istrue = true;
                }
            } else {
                if (newDate < Date) {
                    Istrue = true;
                }
            }
            if (Istrue == true) {
                var errMsg = "Invalid Date Range!";
                ErrorAlert(errMsg, 5000);
                if (type == 1) {
                    newDate = fromDate.val();
                } else {
                    newDate = toDate.val();
                }

                $.ajax({
                    url: "/DatePicker/GetDateBSAndAD",
                    type: "Get",
                    data: { date: newDate },
                    dataType: "json",
                    success: function (r1) {

                        result = r1;
                        if (type == 1) {
                            fromDate.val(result.Date);
                            fromDateAd.val(result.DateAD);
                            fromDateBs.val(result.DateBS);
                        } else {
                            toDate.val(result.Date);
                            toDateAd.val(result.DateAD);
                            toDateBs.val(result.DateBS);
                        }
                        return;
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        ErrorAlert("An error has occured:\n" + xhr.status + "\n" + ajaxOptions + "\n" + thrownError, 5000);
                    }
                });
            } else {


                if (type == 1) {
                    fromDate.val(result.Date);
                    fromDateAd.val(result.DateAD);
                    fromDateBs.val(result.DateBS);
                } else {
                    toDate.val(result.Date);
                    toDateAd.val(result.DateAD);
                    toDateBs.val(result.DateBS);
                }
                // DateChangedFunctionRange(dtObject);
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            ErrorAlert("An error has occured:\n" + xhr.status + "\n" + ajaxOptions + "\n" + thrownError, 5000);
        }
    });
}

function BsDateRange(me, fromDate, toDate) {
    $.ajax({
        url: "/DatePicker/GetDateBSAndADRange",
        type: "Get",
        data: { fromDate: fromDate, toDate: toDate },
        dataType: "json",
        success: function (r1) {
            debugger;
            result = r1;
            me.val(result.FromDateBS + "|" + result.ToDateBS);
            return;
        },
        error: function (xhr, ajaxOptions, thrownError) {
            ErrorAlert("An error has occured:\n" + xhr.status + "\n" + ajaxOptions + "\n" + thrownError, 5000);
        }
    });
}

function SetDateRange(dtObject, dtValue, type) {

    var DType = dtObject.parents('.date-range-default').find('.BsAdRange option:selected').val()
    var Date;

    if (type == 1) {
        Date = formatDate(dtObject.parents('.date-range-default').find(".fc-header-title").find("#EnglishDateTo").val());

    } else {
        Date = formatDate(dtObject.parents('.date-range-default').find(".fc-header-title").find("#EnglishDateFrom").val())
    }

    var newDate; var result;

    var fromDate = dtObject.parents('.date-range-default').find(".fc-header-title").find("#EnglishDateFrom");
    var toDate = dtObject.parents('.date-range-default').find(".fc-header-title").find("#EnglishDateTo");
    var fromDateAd = dtObject.parents('.date-range-default').find(".fc-header-title").find(".fromDateAd");
    var fromDateBs = dtObject.parents('.date-range-default').find(".fc-header-title").find(".fromDateBs");
    var toDateAd = dtObject.parents('.date-range-default').find(".fc-header-title").find(".toDateAd");
    var toDateBs = dtObject.parents('.date-range-default').find(".fc-header-title").find(".toDateBs");
    var Istrue = false;
    $.ajax({
        url: "/DatePicker/CheckDateFormat",
        type: "Get",
        data: { dateType: DType, DateString: dtValue },
        dataType: "json",
        success: function (r) {
            result = r;
            newDate = formatDate(result.Date)
            if (type == 1) {
                if (newDate > Date) {
                    Istrue = true;
                }
            } else {
                if (newDate < Date) {
                    Istrue = true;
                }
            }
            if (Istrue == true) {
                var errMsg = "Invalid Date Range!";
                ErrorAlert(errMsg, 5000);
                if (type == 1) {
                    newDate = fromDate.val();
                } else {
                    newDate = toDate.val();
                }

                $.ajax({
                    url: "/DatePicker/GetDateBSAndAD",
                    type: "Get",
                    data: { date: newDate },
                    dataType: "json",
                    success: function (r1) {

                        result = r1;
                        if (type == 1) {
                            fromDate.val(result.Date);
                            fromDateAd.val(result.DateAD);
                            fromDateBs.val(result.DateBS);
                        } else {
                            toDate.val(result.Date);
                            toDateAd.val(result.DateAD);
                            toDateBs.val(result.DateBS);
                        }
                        return;
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        ErrorAlert("An error has occured:\n" + xhr.status + "\n" + ajaxOptions + "\n" + thrownError, 5000);
                    }
                });
            } else {


                if (type == 1) {
                    fromDate.val(result.Date);
                    fromDateAd.val(result.DateAD);
                    fromDateBs.val(result.DateBS);
                } else {
                    toDate.val(result.Date);
                    toDateAd.val(result.DateAD);
                    toDateBs.val(result.DateBS);
                }
                // DateChangedFunctionRange(dtObject);
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            ErrorAlert("An error has occured:\n" + xhr.status + "\n" + ajaxOptions + "\n" + thrownError, 5000);
        }
    });
}

function DateChangedFunctionRange(dtObject) {

    var functionName = dtObject.attr("datechangedfunction");
    if (dtObject.attr("readonlytag") == 'False') {
        if (functionName != "") {
            var DType = dtObject.find('.dpDFormatRange').find('#DateTypeRange').val();
            var toDate = dtObject.find('.txtDateTo').val();
            var fromDate = dtObject.find('.txtDateFrom').val();
            var DateBS = dtObject.find('.txtDateBSRange').val();
            var DateAD = dtObject.find('.txtDateADRange').val();
            var fn = window[functionName];
            try {
                fn(dtObject, DType, toDate, fromDate, DateAD, DateBS);
            } catch (err) {
                //ErrorAlert(err, 5000);
            }
        } else {
            dtObject.find(".txtDateTo").trigger("change");
            dtObject.find(".txtDateFrom").trigger("change");

        }
    }
}

function formatDate(dateString) {

    var date = new Date(dateString);
    var curr_date = date.getDate();
    if (curr_date < 10) {
        curr_date = "0" + curr_date;
    }
    var curr_month = date.getMonth() + 1; //Months are zero based
    if (curr_month < 10) {
        curr_month = "0" + curr_month;
    }
    var curr_year = date.getFullYear();
    var newDate = curr_year + "-" + curr_month + "-" + curr_date;
    return newDate;
}

function DateRageForFirst(yr, mn, dtype, current, type, date) {
    var URL;
    if (type == 1) {
        URL = "/DatePicker/DateByYearMonthChangeRange";
    } else {
        URL = "/DatePicker/DateByYearMonthChangeRange1";
    }
    $.ajax({
        url: URL,
        type: "Get",
        data: { dateType: dtype, year: yr, month: mn, day: SelectedDay, minMaxDay: date },
        dataType: "html",
        success: function (result) {

            if (type == 1) {
                $(current).parents('.date-range-default').find("#Months").val(mn);
                $(current).parents('.date-range-default').find("#Years").val(yr);
                $('.dpCalDetailsRange').html(result);
            } else {
                $(current).parents('.date-range-default').find("#MonthTo").val(mn);
                $(current).parents('.date-range-default').find("#YearTo").val(yr);
                $('.dpCalDetailsRange1').html(result);
            }


        },
        error: function (xhr, ajaxOptions, thrownError) {
            ErrorAlert("An error has occured:\n" + xhr.status + "\n" + ajaxOptions + "\n" + thrownError, 5000);
        }

    });
}

function DateRageForYearMonthChange(yr, mn, dtype, current, type, date) {
    var URL;
    if (type == 1) {
        URL = "/DatePicker/DateByYearMonthChangeRange";
    } else {
        URL = "/DatePicker/DateByYearMonthChangeRange1";
    }
    $.ajax({
        url: URL,
        type: "Get",
        data: { dateType: dtype, year: yr, month: mn, day: SelectedDay, minMaxDay: date },
        dataType: "html",
        success: function (result) {

            if (type == 1) {

                $('.dpCalDetailsRange').html(result);
            } else {

                $('.dpCalDetailsRange1').html(result);
            }


        },
        error: function (xhr, ajaxOptions, thrownError) {
            ErrorAlert("An error has occured:\n" + xhr.status + "\n" + ajaxOptions + "\n" + thrownError, 5000);
        }

    });
}

function DateByYearMonthChangeRange(year, month, dateType, day, minmaxDate, type) {
    var URL;
    if (type == 1) {
        URL = "/DatePicker/DateByYearMonthChangeRange1";
    } else {
        URL = "/DatePicker/DateByYearMonthChangeRange";
    }
    $.ajax({
        url: URL,
        type: "Get",
        data: { year: year, month: month, dateType: dateType, day: day, minMaxDay: minmaxDate },
        dataType: "html",
        success: function (result) {
            if (type == 1) {
                $('.dpCalDetailsRange1').html(result);
            } else {
                $('.dpCalDetailsRange').html(result);
            }



        },
        error: function (xhr, ajaxOptions, thrownError) {
            ErrorAlert("An error has occured:\n" + xhr.status + "\n" + ajaxOptions + "\n" + thrownError, 5000);
        }
    })
}