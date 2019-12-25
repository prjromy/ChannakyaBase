$.ajaxSetup({ cache: false });


var selectedObject = null;
var selectedDay = 1;
var objDPicker;
var voucherEvent;
var subsiEvent;

$("div").scroll(function () {
    debugger;

    if ($dlg_dpicker === 'undefined') {
        return;
    }
    if ($dlg_dpicker === null) {
        return;
    }
    $dlg_dpicker.remove();
    $dlg_dpicker = null;
});



$(document).mousedown(function (e) {
    var clicked = $(e.target); // get the element clicked  
    try {

        if ($dlg_dpicker === 'undefined') {
            return;
        }
        if ($dlg_dpicker === null) {
            return;
        }
        if (!clicked.is('.cl-dpicker') && !clicked.parents().is('.cl-dpicker')) {
            $dlg_dpicker.dialog("close");
            $dlg_dpicker.remove();
            $dlg_dpicker = null;
            selectedObject = null;
        }
    } catch (err) {
        return;
    }
});



$(document).on('click', '.btn-treeview-popup', function (e) {
    debugger;
    var windowwidth = $(window).width();
    var windowheight = $(window).height();

    var pos = $(this).offset();

    var treeViewParam = {        
        WithCheckBox: $(this).attr("withcheckbox"),
        AllowSelectGroup:  $(this).attr("allowselectgroup"),
        WithImageIcon: $(this).attr("withimageicon"),
        WithOutMe: $(this).attr("excludeme"),
        Title: $(this).attr("poptitle"),
        SelectedNodeId: $(this).closest('.section-treeview').find('.internal-value').val(),
        Filter: ""
    }
    var left = 0;
    var top = 0;
    selectedObject = $(this);

    if (windowwidth - pos.left < 320) {
        left = windowwidth - 320;
    }
    else {
        left = pos.left - 17;
    }
    if (windowheight - pos.top < 250) {
        top = pos.top - 150;
    }
    else {
        top = pos.top + 45;
    }

    $dlg_dpicker = $('<div></div>')
    .dialog({
        dialogClass: 'cl-dpicker',
        autoopen: false,
        close: function () {
            $(this).removeClass("cl-dpicker");
            $(this).dialog("destroy");
            $(this).remove();
            $dlg_dpicker = null;
            selectedObject = null;
        },
        open: function (event, ui) {
            $(event.target).parent().css('position', 'absolute');
            $(event.target).parent().css('top', top);
            $(event.target).parent().css('left', left);
            $(event.target).parent().css("border", "2px");
            $(event.target).parent().css("background-color", "transparent");
            $(event.target).parent().css("z-index", "9999");
            $(event.target).siblings('div.ui-dialog-titlebar').remove();
        },
        overlay: {
            backgroundColor: 'black',
            opacity: 0.5
        },
        modal: true,
        resizable: false
    });


    var query = $(this).closest('.section-treeview').find(".display-txt").val();
    if (query != null) {
        var wrapperClass = $(selectedObject).closest('.section-treeview');
        $(wrapperClass).trigger('filterTree', [{ fromPopUp: 'false', param: treeViewParam }]);
    }

});

//$(document).on('click', '.btn-find-tree', function (e) {
//    var query = $(this).closest('.section-treeview').find(".display-txt").val();

//    var controller = $(this).attr("controller");
//    var action = $(this).attr("action");
//    var allowselectgroup = $(this).attr("allowselectgroup");
//    var withimageicon = $(this).attr("withimageicon");
//    var withcheckbox = $(this).attr("withcheckbox");
//    var excludeme = $(this).attr("excludeme");
//    var title = $(this).attr("poptitle");
//    var selectedNodeId = $(this).closest('.section-treeview').find('.internal-value').val();
//    var filter = $(this).closest('.section-treeview').find('.display-txt').val();

//    var TreeViewParam = {
//        Controller: controller,
//        Action: action,
//        WithCheckBox: withcheckbox,
//        AllowSelectGroup: allowselectgroup,
//        WithImageIcon: withimageicon,
//        WithOutMe: excludeme,
//        Title: title,
//        SelectedNodeId: selectedNodeId,
//        Filter: filter,
//        SelectedNodeText: ''
//    }

//    if (query != null) {
//        var wrapperClass = $(selectedObject).closest('.section-treeview');
//        $(wrapperClass).trigger('filterTree', [{ filterString: query, fromPopUp:'false', obj: TreeViewParam }]);

//        //var wrapperClass = $(this).closest('.ch-treeview').parent();
//        //var mainClass = $(wrapperClass).find('.treeview-area');

//        //var _allowSelectGroupNode = $(mainClass).attr("allowselectgroupnode").replace('"', '').replace('"', '');
//        //var _withImageIcon = $(mainClass).attr("withimageicon").replace('"', '').replace('"', '');
//        //var _withCheckBox = $(mainClass).attr("withcheckbox").replace('"', '').replace('"', '');
//        //var _withOutMe = $(mainClass).attr("withoutme").replace('"', '').replace('"', '');

//        //$(wrapperClass).trigger('filterTree', [{ filterString: query, allowSelectGroupNode: _allowSelectGroupNode, withImageIcon: _withImageIcon, withCheckBox: _withCheckBox, withOutMe: _withOutMe }]);
//    }
//});




$(document).on('click', '.btn-cal-show', function (e) {
    //debugger;
    objDPicker = $(this).closest('.cl-dpicker');

    var windowwidth = $(window).width();
    var windowheight = $(window).height();


    var dtType = objDPicker.find('#date-type').val();
    var adDate = objDPicker.find('.date-value').val();

    var pos = objDPicker.offset();
    var left = 0;
    var top = 0;
    //Fortesting
    //define variable value here
    dtType = "1";
    adDate = '1/1/2001';
    // 
    if (windowwidth - pos.left < 320) {
        left = windowwidth - 320;

        //$('.cl-dpicker').css("left", windowwidth - 320);
    }
    else {
        left = pos.left - 17;
        //$('.cl-dpicker').css("left", pos.left - 17);
    }
    if (windowheight - pos.top < 250) {
        top = pos.top - 200;
        //$('.cl-dpicker').css("top", pos.top - 200);
    }
    else {
        //$('.cl-dpicker').css("top", pos.top + 15);
        top = pos.top + 15;
    }

    $dlg_dpicker = $('<div></div>')
    //.addClass("dlg-dpicker")
    //.appendTo('body')
    .dialog({
        dialogClass: 'cl-dpicker',
        autoopen: false,
        
        close: function () {
            $(this).removeClass("dlg-dpicker");
            $(this).dialog("destroy");
            $(this).remove();
        },
        //show: { effect: 'fade', duration: 500 }, //'blind',    //'fold'        
        open: function (event, ui) {
            $(event.target).parent().css('position', 'absolute');
            $(event.target).parent().css('top', top);
            $(event.target).parent().css('left', left);     
            $(event.target).css("z-index", "99999");
            $(event.target).parent().css("border", "0px");
            $(event.target).parent().css("background-color", "transparent");
            $(event.target).parent().css("z-index", "99999");
            $(event.target).siblings('div.ui-dialog-titlebar').remove();
            
           // $(event.target).animate({ left : left, top :top}, 'fade');
        },
        overlay: {
            backgroundColor: 'black',
            opacity: 0.1
        },        
        modal: true,
        resizable: false,
       
    }).load("/Calendar/GetCalendar", { dateAD: adDate, dateType: dtType.toString() });

  
});