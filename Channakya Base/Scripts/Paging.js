$(document).ready(function () {
    
    var filter;
    var filters;
    var full;
   
    /////sort/////
    $(".details").on('change', '#sortFilter', function () {
        var isActive = $(this).closest('.emplpyee-search-container').attr('isactive');
        if (isActive == undefined) {
            isActive = "";
        }
        var sortOrder = $(this).val();
        var parent = $(this).closest(".searchsorting");
        //var container = $(parent).attr('container');
        var container = $(parent).closest('.details');
        
   
        var controller = $(parent).attr('controller');
        var action = $(parent).attr('action');
        var url = '/' + controller + '/' + action;
        var elementCurrentPage = $(this).closest("#details").find("#pageNo");
        var elementRowCount = $(this).closest(container).find("#pageSize");
        var elementSearch = $(this).closest(container).find("#search");
        
        debugger;

        var filter = $(container).attr('filter');
    
        if (filter == undefined) {
            filter = "";
        }
        var container1 = $(container).parent();
        debugger;
        var custfilter = $(container1).find('input:radio[name=customer]:checked').val();
        var contractFilter = $(this).closest('.customer-search-container').attr('contract');
        if (custfilter == undefined) {
            custfilter = "";
        }
        if (contractFilter == undefined) {
            contractFilter = "";
        }
        //var rowCount = $(this).closest("#details").find("#rowCount").val();
        //alert(rowCount);
        //  var result = parseInt(elementCurrentPage.val());
        var pageNo = 1;//parseInt(elementCurrentPage.val());
        var pageSize = parseInt(elementRowCount.val());
        var search = elementSearch.val();
        //var page = result;
        //var pageSize = result1;
        $.ajax({
            type: "GET",
            url: url,
            data: { pageNo: pageNo, pageSize: pageSize, search: search,sortOrder:sortOrder,isActive:isActive,filter:filter,custfilter:custfilter,contract:contractFilter },
            dataType: "Html",
            success: function (data) { SuccessCall(data, pageNo, pageSize, search, container); },
            error: function (xhr, status, error) { alert(error); },

        });

        function SuccessCall(data, pageNo, pageSize, search, container) {
            //alert(data);
            $(container).html('');
            $(container).append(data);
            $('#pageSize').val(pageSize);
            $('#search').val(search);
        }
    });
    /////sort//////




    /////Search/////
    //$(".details").on('change', '#search', function () {
    //    debugger;
    //    var isActive = $(this).closest('.emplpyee-search-container').attr('isactive');
    //    var parent = $(this).closest(".searchsorting");
    //    var sortOrder = $(parent).find("#sortFilter").children("option").filter(':selected').val();
       
    //    var container = $(parent).closest('.details');
        
    //    var controller = $(parent).attr('controller');
    //    var action = $(parent).attr('action');
    //    var url = '/' + controller + '/' + action;
    
    //    var elementRowCount = $(this).closest(container).find("#pageSize");
    //    var elementSearch = $(this).closest(container).find("#search");
    //    var ele;
    //    debugger;
    //    var container1 = $(container).parent();
    //    debugger;
    //    var custfilter = $(container1).find('input:radio[name=customer]:checked').val();
    //    var contractFilter = $(this).closest('.customer-search-container').attr('contract');
    //    if (custfilter == undefined) {
    //        custfilter = "";
    //    }
    //    if (contractFilter == undefined) {
    //        contractFilter = "";
    //    }
    //    var filter = $(container).attr('filter');
    //    if (filter == undefined) {
    //        filter = "";
    //    }
      
    //    var pageSize = parseInt(elementRowCount.val());
    //    var search = elementSearch.val();
      
    //    $.ajax({
    //        type: "GET",
    //        url: url,
    //        data: { pageNo: 1, pageSize: pageSize,search:search,sortOrder:sortOrder,filter:filter,custfilter:custfilter,contract:contractFilter },
    //        dataType: "Html",
    //        success: function (data) { SuccessCall(data, pageNo, pageSize,search,container); },
    //        error: function (xhr, status, error) { alert(error); },

    //    });

    //    function SuccessCall(data, pageNo, pageSize,search ,container) {

    //        $(container).html('');
    //        $(container).append(data);
    //        $('#pageSize').val(pageSize);
    //        $('#search').val(search);
    //    }
    //});




    function delay(callback, ms) {
        var timer = 0;
        return function () {
            var context = this, args = arguments;
            clearTimeout(timer);
            timer = setTimeout(function () {
                callback.apply(context, args);
            }, ms || 0);
        };
    }


    //$(".details").on('change', '#search', delay(function () {
    //        debugger;
         
    //    var isActive = $(this).closest('.emplpyee-search-container').attr('isactive');
    //    var parent = $(this).closest(".searchsorting");
    //    var sortOrder = $(parent).find("#sortFilter").children("option").filter(':selected').val();
    //    //alert(sortOrder);
    //    //var container = $(parent).attr('container');
    //    var container = $(parent).closest('.details');

    //    var controller = $(parent).attr('controller');
    //    var action = $(parent).attr('action');
    //    var url = '/' + controller + '/' + action;
    //    // var elementCurrentPage = $(this).closest("#details").find("#pageNo");
    //    var elementRowCount = $(this).closest(container).find("#pageSize");
    //    var elementSearch = $(this).closest(container).find("#search");
    //    var ele;
    //    debugger;
    //    var container1 = $(container).parent();
    //    debugger;
    //    var custfilter = $(container1).find('input:radio[name=customer]:checked').val();
    //    var contractFilter = $(this).closest('.customer-search-container').attr('contract');
    //    if (custfilter == undefined) {
    //        custfilter = "";
    //    }
    //    if (contractFilter == undefined) {
    //        contractFilter = "";
    //    }
    //    var filter = $(container).attr('filter');
    //    if (filter == undefined) {
    //        filter = "";
    //    }
    //    //var rowCount = $(this).closest("#details").find("#rowCount").val();
    //    //alert(rowCount);
    //    //  var result = parseInt(elementCurrentPage.val());
    //    var pageSize = parseInt(elementRowCount.val());
    //    var search = elementSearch.val();
    //    //var page = result;
    //    //var pageSize = result1;
    //    $.ajax({
    //        type: "GET",
    //        url: url,
    //        data: { pageNo: 1, pageSize: pageSize, search: search, sortOrder: sortOrder, filter: filter, custfilter: custfilter, contract: contractFilter },
    //        dataType: "Html",
    //        success: function (data) { SuccessCall(data, pageNo, pageSize, search, container); },
    //        error: function (xhr, status, error) { alert(error); },

    //    });

    //    function SuccessCall(data, pageNo, pageSize, search, container) {

    //        $(container).html('');
    //        $(container).append(data);
    //        $('#pageSize').val(pageSize);
    //        $('#search').val(search);
    //    }
    //}, 3000));



    $(".details").on('keypress', '#search', delay(function (e) {
        debugger
        var keycode = (e.keyCode ? e.keyCode : e.which);
        if (keycode == 13)
        {
            debugger;
            var isActive = $(this).closest('.emplpyee-search-container').attr('isactive');
            var parent = $(this).closest(".searchsorting");
            var sortOrder = $(parent).find("#sortFilter").children("option").filter(':selected').val();
            //alert(sortOrder);
            //var container = $(parent).attr('container');
            var container = $(parent).closest('.details');

            var controller = $(parent).attr('controller');
            var action = $(parent).attr('action');
            var url = '/' + controller + '/' + action;
            // var elementCurrentPage = $(this).closest("#details").find("#pageNo");
            var elementRowCount = $(this).closest(container).find("#pageSize");
            var elementSearch = $(this).closest(container).find("#search");
            var ele;
            debugger;
            var container1 = $(container).parent();
            debugger;
            var custfilter = $(container1).find('input:radio[name=customer]:checked').val();
            var contractFilter = $(this).closest('.customer-search-container').attr('contract');
            if (custfilter == undefined) {
                custfilter = "";
            }
            if (contractFilter == undefined) {
                contractFilter = "";
            }
            var filter = $(container).attr('filter');
            if (filter == undefined) {
                filter = "";
            }
            //var rowCount = $(this).closest("#details").find("#rowCount").val();
            //alert(rowCount);
            //  var result = parseInt(elementCurrentPage.val());
            var pageSize = parseInt(elementRowCount.val());
            var search = elementSearch.val();
            //var page = result;
            //var pageSize = result1;
            $.ajax({
                type: "GET",
                url: url,
                data: { pageNo: 1, pageSize: pageSize, search: search, sortOrder: sortOrder, filter: filter, custfilter: custfilter, contract: contractFilter },
                dataType: "Html",
                success: function (data) { SuccessCall(data, pageNo, pageSize, search, container); },
                error: function (xhr, status, error) { alert(error); },

            });

        
        }
        e.preventDefault();
        function SuccessCall(data, pageNo, pageSize, search, container) {

            $(container).html('');
            $(container).append(data);
            $('#pageSize').val(pageSize);
            $('#search').val(search);
        }      
    }, 3000));


    /////Row Per Page/////
    $(".details").on('change', '#pageSize', function () {
        var isActive = $(this).closest('.emplpyee-search-container').attr('isactive');
        var parent = $(this).closest("#paging");
        //var container = $(parent).attr('container');
        var container = $(parent).closest('.details');
        var container1 = $(container).parent();
        debugger;
        var custfilter=$(container1).find('input:radio[name=customer]:checked').val();
       // alert(v);
        //var c = $(parent).closest('.details');
        var controller = $(parent).attr('controller');
        var action = $(parent).attr('action');
        var url = '/' + controller + '/' + action;
        // var elementCurrentPage = $(this).closest("#details").find("#pageNo");
        var elementRowCount = $(this).closest(container).find("#pageSize");
        var ele;
        var parent1 = $(container).find('.searchsorting');
        var sortOrder = $(parent1).find("#sortFilter").val();
        var search = $(parent1).find("#search").val();
        var contractFilter = $(this).closest('.customer-search-container').attr('contract');

        debugger;
        //alert($(container1).attr('class'));
        var filter = $(container).attr('filter');

        if (filter == undefined) {
            filter = "";
        }


        //var rowCount = $(this).closest("#details").find("#rowCount").val();
        //alert(rowCount);
        //  var result = parseInt(elementCurrentPage.val());
        var pageSize = parseInt(elementRowCount.val());
        //var page = result;
        //var pageSize = result1;
        var eror = false;
        if (pageSize < 1) {
            eror = true;
            pageSize = 5;
        }
        $.ajax({
            type: "GET",
            url: url,
            data: { pageNo: 1, pageSize: pageSize,isActive:isActive,filter:filter,sortOrder:sortOrder,custfilter:custfilter,contract:contractFilter },
            dataType: "Html",
            success: function (data) { SuccessCall(data, pageNo, pageSize, container); },
            error: function (xhr, status, error) { alert(error); },

        });

          function SuccessCall(data, pageNo, pageSize, container) {

              $(container).html('');
              if (eror == true) {
                  $(container).append(data).find('#erormsgpagesize').show();

              } else {
                  $(container).append(data).find('#erormsgpagesize').hide();

              }
              $('#pageSize').val(pageSize);

          }
    });
    /////PageNo//////
    $(".details").on('change', '#pageNo', function () {
        var isActive = $(this).closest('.emplpyee-search-container').attr('isactive');
        var parent = $(this).closest("#paging");
        var erorDivClass = $(parent).find('#erormsg').attr('class');
        //  var container = $(parent).attr('container');
        var container = $(parent).closest('.details');
        var container1 = $(container).parent();
        debugger;
        var custfilter = $(container1).find('input:radio[name=customer]:checked').val();
        var contractFilter = $(this).closest('.customer-search-container').attr('contract');

        var parent1 = $(container).find('.searchsorting');
        var sortOrder = $(parent1).find("#sortFilter").val();
        var search = $(parent1).find("#search").val();

        var controller = $(parent).attr('controller');
        var action = $(parent).attr('action');
        var pageCount = $(parent).attr('pageCount');
        var url = '/' + controller + '/' + action;
        var elementCurrentPage = $(this).closest(container).find("#pageNo");
        var elementRowCount = $(this).closest(container).find("#pageSize");
        var ele;
        var pageSize = parseInt(elementRowCount.val())
        var eror = false;
        debugger;
        var filter = $(container).attr('filter');

        if (filter == undefined) {
            filter = "";
        }

        var pageNo = parseInt(elementCurrentPage.val());
        if (pageNo > pageCount || pageNo < 1) {
            eror = true;
            pageNo = 1;

        }
           $.ajax({
                type: "GET",
                url: url,
                data: { pageNo: pageNo, pageSize: pageSize, isActive: isActive, sortOrder: sortOrder, search: search,filter:filter,custfilter:custfilter,contract:contractFilter },
                dataType: "Html",
                success: function (data) { SuccessCall(data, container); },
                error: function (xhr, status, error) { alert(error); },
            });
            function SuccessCall(data, container) {
                $(container).html('');
                if (eror == true)
                {
                    $(container).append(data).find('#erormsg').show();

                } else {
                    $(container).append(data).find('#erormsg').hide();

                }

            }


    });
    /////End PageNo//////
    ////for all button////
    $(".details").on('click', '.btnPaging', function () {
        ////
        //var time = {}
        //time.FromTo = [];
        //var customer = {};
        //customer.Checked = [];
        //var status = {};
        //status.Checked = [];
        //var services = {};
        //services.Checked = [];

        //$('.time-td').find('.date-box').each(function () {
        //    var intTime = $(this).val();
        //    time.FromTo.push(intTime);
        //})

        //$('.customer-td').find('input:checkbox:checked').each(function () {
        //    var customer1 = $(this).val();
        //    customer.Checked.push(customer1);
        //});

        //$('.status-td').find('input:checkbox:checked').each(function () {
        //    var status1 = $(this).val();
        //    status.Checked.push(status1);
        //})

        //$('.service-td').find('input:checkbox:checked').each(function () {
        //    var services1 = $(this).val();
        //    services.Checked.push(services1);
        //})

        ////
        debugger;
        var parent = $(this).closest(".paging");
        var container = $(parent).closest('.details');
        var isActive = $(this).closest('.emplpyee-search-container').attr('isactive');
        var id = $(this).attr('id');
        var parent1 = $(container).find('.searchsorting');
        //var sortOrder = $(parent).find("#sortFilter").val();
        debugger;
        var filter = $(container).attr('filter');

        if (filter == undefined) {
            filter = "";
        }
        var container1 = $(container).parent();
        debugger;
        var custfilter = $(container1).find('input:radio[name=customer]:checked').val();

        var contractFilter = $(this).closest('.customer-search-container').attr('contract');

        var sortOrder = $(parent1).find("#sortFilter").val();
        var search = $(parent1).find("#search").val();        
        
        var controller = $(parent).attr('controller');
        var action = $(parent).attr('action');
        var pageCount = $(parent).attr('pageCount');
        var url = '/' + controller + '/' + action;
        var elementCurrentPage = $(this).closest(container).find("#pageNo");
        var elementRowCount = $(this).closest(container).find("#pageSize");
        var currentPageNo = 1;
        if (id == "btnNxt") {
            currentPageNo = parseInt(elementCurrentPage.val()) + 1;

        }
        else if (id == "btnPrev") {
            currentPageNo = parseInt(elementCurrentPage.val()) - 1;
        }
        else if (id == "btnFirst") {
            currentPageNo = 1;
        }
        else if (id == "btnLast") {
            currentPageNo = pageCount;
        }        
        var pageSize = parseInt(elementRowCount.val())
        $.ajax({
            type: "GET",
            url: url,
            data: {
                //time: time.FromTo, customer: customer.Checked, status: status.Checked, service: services.Checked
                pageNo: currentPageNo, pageSize: pageSize, sortOrder: sortOrder, search: search, isActive: isActive, filter: filter, custfilter: custfilter, contract: contractFilter },
            dataType: "Html",
            traditional: true,
            success: function (data) { SuccessCall(data, container); },
            error: function (xhr, status, error) { alert(error); },
        });
        function SuccessCall(data,container) {
            $(container).html('');
            $(container).append(data);
            $('#search').val(search);
        }
    });
    ////exportto
  
    $(".details").on('click', '._exportTo', function () {
        debugger;
        var parent = $(this).closest(".searchsorting");
        var controller = $(parent).attr('controller');
        var action = $(parent).attr('action')+'_exportTo';
        var exportTo = $(parent).find('._exportTo').attr('id');
        var url = '/' + controller + '/'+action+ '?controllerName=' + controller;
        $.ajax({
                    type: "GET",
                    //url: url,
                    url:window.open(url),
                    data:{controllerName:controller}
                          });
    });
});