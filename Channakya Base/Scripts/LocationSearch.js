$(document).ready(function () {
    //.location-search-container
    $(document).on('change', '.locationname', function () {
        debugger;
        var container = $(this).closest('.location-search-container');
        var idField = $(container).find('.locationid');
        var nameField = $(this);
        var locationName = $(this).val();
        if (locationName == '') {
            $(container).find('.locationid').val(0);
        } else {
            $.ajax({
                type: "GET",
                url: '/Location/GetLocation',
                data: { locationName: locationName },
                dataType: "Json",
                success: function (data) {
                    $(nameField).val(data.LocationName);
                    $(idField).val(data.LId);
                },
                error: function (xhr, status, error) { alert(error); },
            });
        }
    });
    $('.cust-contact-individual-person').on('click', '#btnlocationsearch', function () {
        debugger;
    });

    $(document).on('click', '.btnlocationsearch', function () {
        // $('.btnlocationsearch').click(function(){

        debugger;
        //var resultContainer = $(this).closest('.location-search-container').find('.locationsearchresult');
        var resultContainer = $(this).parent();
        //var isActive = $(this).closest('.emplpyee-search-container').attr('isactive');
        var modalBody = $(resultContainer).find('.modal-body');
        var url = '/Location/_GetLocationList';
        var pageNo = 1;
        var pageSize = 5;
        var search = "";

        var sortOrder = "";
        $.ajax({
            type: "GET",
            url: url,
            data: { pageNo: pageNo, pageSize: pageSize, search: search, sortOrder: sortOrder },
            dataType: "Html",
            success: function (data) {
                //alert(data);
                debugger;
                SuccessCall(data, pageNo, pageSize, search, modalBody);
            },
            error: function (xhr, status, error) { alert(error); },

        });

        function SuccessCall(data, pageNo, pageSize, search, modalBody) {
            //alert(data);

            debugger;
            $(resultContainer).find('.dialoglocation').modal('show');
            $(modalBody).html('');
            $(modalBody).append(data);
            $('#pageSize').val(pageSize);
            $('#search').val(search);
            //$(container).dialog({             
            //    modal: true,
            //    width: 'auto'
            //});
        }
    });

    $(document).on('click', '.selectLocation', function () {
        //alert();

        debugger;
        //var p = $(this).closest('.b');
        //alert($(p).attr('class'));
        var tr = $(this).closest('tr');
        var LId = $(tr).find('#item_LId').val();
        var locationName = $(tr).find('#locationName').html().trim();
        var p = $(this).closest('.location-search-container');
        var eleLocationId = $(p).find('.locationname');
        //var eleLocationId = $(this).closest(':input').val(LId);
        $(p).find('.locationid').val(LId);
        $(eleLocationId).val(locationName);
        //$(container).find('.employeesearchresult').html('');
        $(eleLocationId).focus();
    })

});