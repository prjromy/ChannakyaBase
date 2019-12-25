$.ajaxSetup({ cache: false });
$calanderDialog = "";


var SelectedDay = 1;
var _dtObject;
var _customSearchObject;
var _customerSearchObject;

var asbsMinDate;
var asbsMaxDate;
var date;


$(document).keydown(function (e) {

    e.stopImmediatePropagation();
    if (e.keyCode == 27) { // ESC            
        if ($calanderDialog != "") {
            if (!$(e.target).is('.dialog_calander') && !$(e.target).parents().is('.dialog_calander')) {
                CloseCalanderDialog();
            }
        }
       
    }
    //if ($calanderDialogRnge != "") {
    //    if (!clicked.is('.date-range-default') && !clicked.parents().is('.date-range-default')) {
    //        isOpen = 0;
    //        CloseCalanderDialogRange();

    //    }
    //}
});

$(document).mousedown(function (e) {
    e.stopImmediatePropagation();
    var clicked = $(e.target); // get the element clicked  

    if ($calanderDialog != "") {
        if (!clicked.is('.dialog_calander') && !clicked.parents().is('.dialog_calander')) {
            CloseCalanderDialog();
        }
    }
});

function CloseCalanderDialog() {
    $calanderDialog.parentNode.removeChild($calanderDialog);
    $calanderDialog = "";
}

function ShowCalander(html, left, top, dtype) {
    $calanderDialog = document.createElement("div");
    var title = "Calendar";

    $calanderDialog.setAttribute("id", "dialog_calander");
    $calanderDialog.setAttribute("class", "dialog_calander panel panel-default");
    $calanderDialog.setAttribute("style", "z-index:1000001;");

    $calanderDialog.setAttribute("style", "left:" + left + "px;top:" + top + "px;width:260px;");
    if (dtype == 1) {
        $calanderDialog.innerHTML = $calanderDialog.innerHTML + "<select class='form-header BsAd'> <option value='1'selected>AD</option><option value='2'>BS</option></select>";
    }
    else {
        $calanderDialog.innerHTML = $calanderDialog.innerHTML + "<select class='form-header BsAd'> <option value='1'>AD</option><option value='2'selected>BS</option></select>";
    }
    $calanderDialog.innerHTML = $calanderDialog.innerHTML + " <span class='fc-header-title'> <button type='button' class='btn btn-default chClosebtn'><i class='fa fa-close'></i></button></span>";
    $calanderDialog.innerHTML = $calanderDialog.innerHTML + "<hrLineCalendar>";
    $calanderDialog.innerHTML = $calanderDialog.innerHTML + "<div class='calander_container' >" + html + "<" + "/div>"

    document.body.appendChild($calanderDialog);

    $('.dialog_calander').hide().fadeIn(500);
}

$(document).on('click', '.chClosebtn', function (e) {
    e.stopImmediatePropagation();
    CloseCalanderDialog();
});

$('.dpCalBtn').on('click', function (e) {
    debugger
    e.stopImmediatePropagation();
    _dtObject = $(this).closest('.chdPicker');

    var windowwidth = $(window).width();
    var windowheight = $(window).height();

    var DType = _dtObject.find('.dpDFormat').find("#DateTypeADBS option:selected").val()

  //  var DType = _dtObject.find('.calDateType').attr("id");

    var AdDate = _dtObject.find('.txtDateValue').val();


    // new added part to change AD to BS
    var chunks = AdDate.split('/');
    var AdDate = chunks[1] + '-' + chunks[0] + '-' + chunks[2];



    var minDate = formatDate($(_dtObject).attr("minDate"));
    var maxDate = formatDate($(_dtObject).attr("maxDate"));
    asbsMinDate = minDate;
    asbsMaxDate = maxDate;
    date = AdDate;
    var pos = _dtObject.offset(); var left = 0; var top = 0; var right = 0; var bottom = 0;


    if (windowwidth - pos.left < 320) {
        left = windowwidth - 320;
    }
    else {
        left = pos.left;
    }
    if (windowheight - pos.top < 250) {
        top = pos.top - 231;
    }
    else {
        top = pos.top + 35;
    }
    bottom = top + 300;
    $.ajax({
        url: "/DatePicker/DatePickerIndex",
        type: "Get",
        data: { dateTime: AdDate, dateType: DType.toString(), minDate: minDate, maxDate: maxDate },
        dataType: "html",
        success: function (html) {
            ShowCalander(html, left, top, DType)
        },
        error: function (xhr, ajaxOptions, thrownError) {
            ErrorAlert("An error has occured:\n" + xhr.status + "\n" + ajaxOptions + "\n" + thrownError, 5000);

        }
    });
});

$(document).on('click', '.button-prev', function (e) {
    debugger;
    e.stopImmediatePropagation();
    var dType = _dtObject.find('.dpDFormat').find("#DateTypeADBS option:selected").val()
   // var dType = _dtObject.find('.calDateType').attr("id");
    var mn = $(this).parents('.dialog_calander').find("#Months").val();
    var yr = $(this).parents('.dialog_calander').find("#Years").val();
    var minDate = formatDate($(_dtObject).attr("minDate"));
    var maxDate = formatDate($(_dtObject).attr("maxDate"));
    var current = $(this);
    if (mn == '1') {
        yr = parseInt(yr) - 1;
        mn = 12;
    } else {
        mn = parseInt(mn) - 1
    }
    $.ajax({
        url: "/DatePicker/DateByYearMonthChange",
        type: "Get",
        data: { year: yr, month: mn, dateType: dType, day: SelectedDay, minDate: minDate, maxDate: maxDate },
        dataType: "html",
        success: function (result) {
            $(current).parents('.dialog_calander').find("#Months").val(mn);
            $(current).parents('.dialog_calander').find("#Years").val(yr);
            $('#dpCalDetails').html(result);
        },
        error: function (xhr, ajaxOptions, thrownError) {
            ErrorAlert("An error has occured:\n" + xhr.status + "\n" + ajaxOptions + "\n" + thrownError, 5000);
        }

    });
});

$(document).on('click', '.button-next', function (e) {
    debugger;
    e.stopImmediatePropagation();
    var dType = _dtObject.find('.dpDFormat').find("#DateTypeADBS option:selected").val()
   // var dType = _dtObject.find('.calDateType').attr("id");
    var mn = $(this).parents('.dialog_calander').find("#Months").val();
    var yr = $(this).parents('.dialog_calander').find("#Years").val();
    var minDate = formatDate($(_dtObject).attr("minDate"));
    var maxDate = formatDate($(_dtObject).attr("maxDate"));
    var current = $(this);
    if (mn == '12') {
        yr = parseInt(yr) + 1;
        mn = 1;
    } else {
        mn = parseInt(mn) + 1
    }
    $.ajax({
        url: "/DatePicker/DateByYearMonthChange",
        type: "Get",
        data: { year: yr, month: mn, dateType: dType, day: SelectedDay, minDate: minDate, maxDate: maxDate },
        dataType: "html",
        success: function (result) {
            $(current).parents('.dialog_calander').find("#Months").val(mn);
            $(current).parents('.dialog_calander').find("#Years").val(yr);
            $('#dpCalDetails').html(result);
        },
        error: function (xhr, ajaxOptions, thrownError) {
            ErrorAlert("An error has occured:\n" + xhr.status + "\n" + ajaxOptions + "\n" + thrownError, 5000);
        }

    });
});

$(document).on('click', '.calanderDay', function (e) {
    debugger;
    e.stopImmediatePropagation();
    $('.dialog_calander').fadeOut(300);

    if (_dtObject.attr("readonlytag") == "True") {
        InfoAlert("Date Object Is Read Only! Cannot Change Date.", 2000)
        CloseCalanderDialog();
        return;
    }

    var minDate = formatDate($(_dtObject).attr("minDate"));
    var maxDate = formatDate($(_dtObject).attr("maxDate"));
    var newDate;
    var result;

    var month; var year; var dType;
    SelectedDay = $(this).attr("id");
    var month = $(this).parents('.dialog_calander').find("#Months").val();
    var year = $(this).parents('.dialog_calander').find("#Years").val()
     dType = _dtObject.find('.dpDFormat').find("#DateTypeADBS option:selected").val()
  //  dType = _dtObject.find('.calDateType').attr("id");

    var ADCntrl = _dtObject.find('.txtDateAD');
    var BSCntrl = _dtObject.find('.txtDateBS')
    var DTCntrl = _dtObject.find('.txtDateValue')

    $.ajax({
        url: "/DatePicker/GetDate",
        type: "Get",
        data: { year: year, month: month, dateType: dType, day: SelectedDay },
        dataType: "json",
        success: function (data) {

            result = data;
            newDate = formatDate(result.Date)
            if (newDate < minDate || newDate > maxDate) {

                var errMsg = "Invalid Date Range!<br> Date Must Between [" + minDate + "] and [" + maxDate + "]";
                ErrorAlert(errMsg, 5000);

                newDate = DTCntrl.val();
                $.ajax({
                    url: "/DatePicker/GetDateBSAndAD",
                    type: "Get",
                    data: { date: newDate },
                    dataType: "json",
                    success: function (r1) {
                        result = r1;
                        ADCntrl.val(result.DateAD);
                        BSCntrl.val(result.DateBS);
                        DTCntrl.val(result.Date);

                        ADCntrl.focus();
                        BSCntrl.focus();
                        DTCntrl.focus();
                        return;
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        ErrorAlert("An error has occured:\n" + xhr.status + "\n" + ajaxOptions + "\n" + thrownError, 5000);
                    }
                });
            } else {
                ADCntrl.val(result.DateAD);
                BSCntrl.val(result.DateBS);
                DTCntrl.val(result.Date);
                DateChangedFunction(_dtObject);
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            CloseCalanderDialog();
            ErrorAlert("Error No:\n" + xhr.status + "\n, Error Type:" + ajaxOptions + "\n, Error Description:" + thrownError, 5000);
        }
    });


    CloseCalanderDialog();

    if (dType == 1) {
        ADCntrl.focus();
    } else {
        BSCntrl.focus();
    }
});

$(document).on('change', '.dpYMFontSize', function (e) {
    debugger;
    e.stopImmediatePropagation();
    var dType = _dtObject.find('.dpDFormat').find("#DateTypeADBS option:selected").val()
  //  var dType = _dtObject.find('.calDateType').attr("id");
    var mn = $(this).parents('.dialog_calander').find("#Months").val();
    var yr = $(this).parents('.dialog_calander').find("#Years").val();
    var minDate = formatDate($(_dtObject).attr("minDate"));
    var maxDate = formatDate($(_dtObject).attr("maxDate"));
    $.ajax({
        url: "/DatePicker/DateByYearMonthChange",
        type: "Get",
        data: { year: yr, month: mn, dateType: dType, day: SelectedDay, minDate: minDate, maxDate: maxDate },
        dataType: "html",
        success: function (result) {
            $('#dpCalDetails').html(result);
        },
        error: function (xhr, ajaxOptions, thrownError) {
            ErrorAlert("An error has occured:\n" + xhr.status + "\n" + ajaxOptions + "\n" + thrownError, 5000);
        }

    });
});

$('.txtDateAD#DateAD').on('change', function (e) {
  
    e.stopImmediatePropagation();
    _dtObject = $(this).closest('.chdPicker');
    SetDate(_dtObject, $(this).val());
});

$('.txtDateBS#DateBS').on('change', function (e) {
  
    e.stopImmediatePropagation();
    _dtObject = $(this).closest('.chdPicker');
    SetDate(_dtObject, $(this).val());
});

$('.dpDFormat').on("change", "#DateTypeADBS", function (e) {
    debugger;
    e.stopImmediatePropagation();
    _dtObject = $(this).closest('.chdPicker');
    var selected = $(this).val();
   // var selected = $(this).attr("id");
    var allowTypeChange = $(_dtObject).attr("allowdtypechange");
    if (allowTypeChange == "Yes") {
        if (selected == "1") {
            _dtObject.find('.txtDateAD').show();
            _dtObject.find('.txtDateBS').hide();
            _dtObject.find('.txtDateAD').focus();
           // $(this).html('AD');
          //  $(this).attr("id", "1");
        }
        else {
            _dtObject.find('.txtDateAD').hide();
            _dtObject.find('.txtDateBS').show();
            _dtObject.find('.txtDateBS').focus();
          //  $(this).html('BS');
            //$(this).attr("id", "2");
        }
        DateChangedFunction(_dtObject);
    }
    else {
        InfoAlert("You cannot change date type in this transaction mode!", 5000);
    }
});

function SetDate(dtObject, dtValue) {
    debugger;
    var DType = dtObject.find('.dpDFormat').find("#DateTypeADBS option:selected").val();
  //  var DType = dtObject.find('.calDateType').attr("id");
    var minDate = formatDate($(dtObject).attr("minDate"));
    var maxDate = formatDate($(dtObject).attr("maxDate"));
    var newDate; var result;

    var ADCntrl = dtObject.find('.txtDateAD');
    var BSCntrl = dtObject.find('.txtDateBS')
    var DTCntrl = dtObject.find('.txtDateValue')

    $.ajax({
        url: "/DatePicker/CheckDateFormat",
        type: "Get",
        data: { dateType: DType, DateString: dtValue },
        dataType: "json",
        success: function (r) {

            result = r;
            newDate = formatDate(result.Date)

            if (newDate < minDate || newDate > maxDate) {
                var errMsg = "Invalid Date Range!<br> Date Must Between [" + minDate + "] and [" + maxDate + "]";
                ErrorAlert(errMsg, 5000);

                newDate = DTCntrl.val();
                $.ajax({
                    url: "/DatePicker/GetDateBSAndAD",
                    type: "Get",
                    data: { date: newDate },
                    dataType: "json",
                    success: function (r1) {
                        result = r1;
                        ADCntrl.val(result.DateAD);
                        BSCntrl.val(result.DateBS);
                        DTCntrl.val(result.Date);
                        ADCntrl.focus();
                        BSCntrl.focus();
                        DTCntrl.focus();
                        return;
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        ErrorAlert("An error has occured:\n" + xhr.status + "\n" + ajaxOptions + "\n" + thrownError, 5000);
                    }
                });
            } else {
                ADCntrl.val(result.DateAD);
                BSCntrl.val(result.DateBS);
                DTCntrl.val(result.Date);
                DateChangedFunction(dtObject);
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            ErrorAlert("An error has occured:\n" + xhr.status + "\n" + ajaxOptions + "\n" + thrownError, 5000);
        }
    });
}

function DateChangedFunction(dtObject) {
    debugger;
    var functionName = dtObject.attr("datechangedfunction");
    if (dtObject.attr("readonlytag") == 'False') {
        if (functionName != "") {
            var DType = dtObject.find('.dpDFormat').find("#DateTypeADBS option:selected").val();
            //var DType = dtObject.find('.calDateType').attr("id");
            var AdDate = dtObject.find('.txtDateValue').val();
            var DateBS = dtObject.find('.txtDateBS').val();
            var DateAD = dtObject.find('.txtDateAD').val();
            var fn = window[functionName];
            try {
                fn(dtObject, DType, AdDate, DateAD, DateBS);
            } catch (err) {
                //ErrorAlert(err, 5000);
            }
        } else {
            dtObject.find(".txtDateValue").trigger("change");

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

$(document).on('change', '.BsAd', function (e) {
    debugger;
    e.stopImmediatePropagation();
    var selected = $(this).val();

    var allowTypeChange = $(_dtObject).attr("allowdtypechange");
    if (allowTypeChange == "Yes") {
        if (selected == "1") {
            _dtObject.find('.txtDateAD').show();
            _dtObject.find('.txtDateBS').hide();
            _dtObject.find('.txtDateAD').focus();
            _dtObject.find('.dpDFormat').find("#DateTypeADBS").val(1);
            //_dtObject.find('.calDateType').html('AD');
            //_dtObject.find('.calDateType').attr("id", "1");
        }
        else {
            _dtObject.find('.txtDateAD').hide();
            _dtObject.find('.txtDateBS').show();
            _dtObject.find('.txtDateBS').focus();
            _dtObject.find('.dpDFormat').find("#DateTypeADBS").val(2);
            //_dtObject.find('.calDateType').html('BS');
            //_dtObject.find('.calDateType').attr("id", "2");
        }
        DateChangedFunction(_dtObject);
    }
    else {
        InfoAlert("You cannot change date type in this transaction mode!", 5000);
        return;
    }
   
   

    var AdDate = date;
    var minDate = asbsMinDate;
    var maxDate = asbsMaxDate;
    $.ajax({
        url: "/DatePicker/DatePickerIndex",
        type: "Get",
        data: { dateTime: AdDate, dateType: selected.toString(), minDate: minDate, maxDate: maxDate },
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
