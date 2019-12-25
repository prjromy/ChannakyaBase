var _globalObject;

$(document).on("click", "#btn-employee-search", function (e) {
    debugger;
    e.stopImmediatePropagation();
    var searchValue = $("#SearchEmployee").val();
    var selectedOption = $("#EmployeeOption option:selected").val()
    var branchId = $("#BranchId option:selected").val()
    var showTypeLoad = $(".register-user-show-type").val();
    var searchType = $(".register-user-search-type").val();
        $('.img-container').hide();
        $('.view-history-image').hide();
   
    $.ajax({
        type: 'GET',
        url: '/Customer/_EmployeeDetails',
        data: {
            searchParam: searchValue,
            searchOption: selectedOption,
            branchId: branchId,
            loadType: showTypeLoad,
            searchType: searchType
        },
        traditional: true,
        success: function (result) {
            debugger;
            $("#employeeDetails").html("");
            $("#employeeDetails").html(result);
        },
    });
});

$(".btnemployeesearch").on('click', function (e) {
    debugger;
    e.stopImmediatePropagation();
    _globalObject = $(this).closest("div.CommonSearchDiv");
    var employeeChange = $(_globalObject).find(".employeeId").attr("employeeChange");
    var branchId = $(this).attr("branchId");
    var showTypeLoad = $(this).attr("showType");
        $('.img-container').hide();
        $('.view-history-image').hide();
    $.ajax({
        type: 'GET',
        url: "/Customer/EmployeeList",
        data: {
            branchId: branchId,
            loadType: showTypeLoad,
            searchType: employeeChange
        },
        success: function (result) {
            debugger;
            $('#employee-pop-up-div').html(result).modal({
                'show': true,
                'backdrop': 'static'
            });

        },
        error: function () {

        }
    });
});
$(document).on('click', '.table-click-Employee table tr', function (e) {
    debugger;
    e.stopImmediatePropagation();
    var employeeId = $(this).closest('tr').attr('id');
    var UserId = $(this).closest('tr').attr('data-userid');
    var parent = $(this).parents();
    var id = _globalObject;
    var showTypeLoad = $(".register-user-show-type").val();
    var employeeChange = $(id).find(".employeeId").attr("employeeChange")
  

        $('.img-container').hide();
        $('.view-history-image').hide();
    $.ajax({
        type: 'GET',
        url: '/Customer/GetSelectedEmploye',
        data: {
            employeeId: employeeId,
            loadType: showTypeLoad
        },
        success: function (result) {
            debugger;
            if (employeeChange == "UserChangeTeller") {
                $.ajax({
                    type: 'GET',
                    url: '/FinanceParameter/GetUserRoleByEmployeeId',
                    data: {
                        employeeId: employeeId
                    },
                    success: function (userList) {
                        debugger;
                        var optionList = '';
                        $.each(userList, function (index, item) {
                            optionList = optionList + "<option value='" + item.UserId + "'>" + item.UserName + "</option>"
                        })
                        $('#stfid').html(optionList);
                    }
                });
            }

            if (employeeChange == "UserCashLimit") {
                $.ajax({
                    type: 'GET',
                    url: '/FinanceParameter/GetSingleCashLimit',
                    data: {
                        UserId: UserId
                    },
                    success: function (cashlimt) {
                        debugger;
                        $("#teller-cash-limit").html(cashlimt);
                    }
                })
            }
             if (employeeChange == "ReturnShare") {
                $.ajax({
                    type: 'GET',
                    url: '/Share/CustomerListShareReturn',
                    data: {
                        employeeId: employeeId
                    },
                    success: function (Customerlist) {
                        debugger;
                        $(".share-return-customer-list").html(Customerlist);
                    }
                })
            }
            if (employeeChange == "ImageChange") {
                debugger;
                $('.img-container').hide();
                $('.view-history-image').hide();
                var flag = $('.signature-display').find('#Display:checked').val();
                if (flag == 1) {
                    var sid = $('.display-account-signature').find('.account-signature-create').find('.account-id').val();
                }

                else if (flag == 3) {
                    var sid = employeeId;
                }
                else {
                    var sid = $('.display-account-signature').find('.customer-photo-create').find('.multiselectCustomer').find('option:selected').val();

                    //var id= link.parents().find('#Aname').val();
                }


                $.ajax({
                    url: "/Signature/GetAll",
                    type: "GET",

                    contentType: "application/json; charset=utf-8",
                    dataType: "html",
                    data: { id: sid ,flag: flag},
                    failure: function (response) {
                        console.log(response);
                    },
                    error: function (response) {
                        console.log(response);
                    },
                    success: function (data) {
                        debugger;
                        $('.img-container').html(data);

                        $('.img-container').fadeIn(3000);
                        //$('.view-history-image').show();

                    },


                });
}
            $(id).find(".employeeName").val(result.EmployeeName);
            $(id).find(".employeeId").val(result.EmployeeId);
           
            $($(parent).find("#employee-pop-up-div")).modal('toggle');
            $(id).find(".employeeName").focus()
            $(id).find(".employeeId").focus()
        },
    });

});
$('.employeeName').on("change", function (event) {
    debugger;
    var currentEvent = $(this);
    var text = $(this).val();
    if (text == "") {
        $('.img-container').hide();
        $('.view-history-image').hide();
        return;
    }
    // var length = $(this).val().length;
    var Id = $(this).closest("div.CommonSearchDiv");

    var showTypeLoad = $(Id).find('.btnemployeesearch').attr("showType");
    var branchId = $(Id).find('.btnemployeesearch').attr("branchId");
    var employeeChange = $(Id).find(".employeeId").attr("employeeChange")
  
    // _globalObject = Id;
    $.ajax({
        url: '/Customer/GetEmployeeName',
        data: {
            text: text,
            loadType: showTypeLoad,
            searchType: employeeChange
        },
        dataType: "json",
        success: function (employeeName) {
            debugger;
            var count = employeeName.length;
            if (count == 1) {
                var currentText = employeeName[0].EmployeeName;
                var currentId = employeeName[0].EmployeeId;
                currentEvent.val(currentText);
                $(Id).find(".employeeId").val(currentId);

                if (employeeChange == "UserChangeTeller") {
                    $.ajax({
                        type: 'GET',
                        url: '/FinanceParameter/GetUserRoleByEmployeeId',
                        data: {
                            employeeId: employeeId
                        },
                        success: function (userList) {
                            debugger;
                            var optionList = '';
                            $.each(userList, function (index, item) {
                                optionList = optionList + "<option value='" + item.UserId + "'>" + item.UserName + "</option>"
                            })
                            $('#stfid').html(optionList);
                        }
                    });
                }


            }
            else {
                $.ajax({
                    type: 'GET',
                    url: "/Customer/EmployeeList",
                    data: {
                        branchId: branchId,
                        loadType: showTypeLoad,
                        searchType: employeeChange
                    },
                    success: function (result) {
                        debugger;
                        $('#employee-pop-up-div').html(result).modal({
                            'show': true,
                            'backdrop': 'static'
                        });

                    },
                    error: function () {

                    }
                });
            }
           
        }
    });
});
$('.employeeName').on('keyup', function (event) {
debugger;
    event.stopImmediatePropagation();
    var currentEvent = $(this);
    var text = $(this).val();

    // var length = $(this).val().length;
    var id = $(currentEvent).attr("id");
    _globalObject = $(this).closest("div.CommonSearchDiv");
    if (text == "") {
      
        return;
    }
    if (event.keyCode == 8) {

        $(_globalObject).find(".employeeName").val("");
        $(_globalObject).find(".employeeId").val("");
        $('.img-container').hide();
        $('.view-history-image').hide();

    }
    var checkevent = event.keyCode;
    if (checkevent == 13) {
        var showTypeLoad = $(_globalObject).find('.btnemployeesearch').attr("showType");
        var branchId = $(_globalObject).find('.btnemployeesearch').attr("branchId");
        var employeeChange = $(_globalObject).find(".employeeId").attr("employeeChange")



        $.ajax({
            url: '/Customer/GetEmployeeName',
            data: {
                text: text,
                loadType: showTypeLoad,
                searchType: employeeChange
            },
            dataType: "json",
            success: function (employeeName) {
                debugger;
                var count = employeeName.length;
                if (count == 1) {
                    var currentText = employeeName[0].EmployeeName;
                    var currentId = employeeName[0].EmployeeId;
                    currentEvent.val(currentText);
                    $(_globalObject).find(".employeeId").val(currentId);

                    if (employeeChange == "UserChangeTeller") {
                        $.ajax({
                            type: 'GET',
                            url: '/FinanceParameter/GetUserRoleByEmployeeId',
                            data: {
                                employeeId: employeeId
                            },
                            success: function (userList) {
                                debugger;
                                var optionList = '';
                                $.each(userList, function (index, item) {
                                    optionList = optionList + "<option value='" + item.UserId + "'>" + item.UserName + "</option>"
                                })
                                $('#stfid').html(optionList);
                            }
                        });
                    }
                    if (employeeChange == "ImageChange") {
                        debugger;
                        $('.img-container').hide();
                        $('.view-history-image').hide();
                        var flag = $('.signature-display').find('#Display:checked').val();
                        if (flag == 1) {
                            var id = $('.display-account-signature').find('.account-signature-create').find('.account-id').val();
                        }

                        else if (flag == 3) {
                            //var id = $('.display-account-signature').find('.share-signature-create').find('.employeeId').val();
                            var id = employeeId;

                        }
                        else {
                            var id = $('.display-account-signature').find('.customer-photo-create').find('.multiselectCustomer').find('option:selected').val();
                        }


                        $.ajax({
                            url: "/Signature/GetAll",
                            type: "GET",
                            contentType: "application/json; charset=utf-8",
                            dataType: "html",
                            data: { id: id , flag: flag},
                            failure: function (response) {
                                console.log(response);
                            },
                            error: function (response) {
                                console.log(response);
                            },
                            success: function (data) {
                                debugger;
                                $('.img-container').html(data);

                                $('.img-container').fadeIn(3000);
                                //$('.view-history-image').show();

                            },


                        });
                    }
                }
                else {
                    $.ajax({
                        type: 'GET',
                        url: "/Customer/EmployeeList",
                        data: {
                            branchId: branchId,
                            loadType: showTypeLoad,
                            searchType: employeeChange
                        },
                        success: function (result) {
                            debugger;
                            $('#employee-pop-up-div').html(result).modal({
                                'show': true,
                                'backdrop': 'static'
                            });

                        },
                        error: function () {

                        }
                    });
                }

            }
        });

    }


});