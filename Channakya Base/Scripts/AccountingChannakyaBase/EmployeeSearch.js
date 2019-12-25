var _globalObject;

$(document).on("click", "#btn-employee-search", function (e) {
    e.stopImmediatePropagation();
    var searchValue = $("#SearchEmployee").val();
    var selectedOption = $("#EmployeeOption option:selected").val()
    var branchId = $("#BranchId option:selected").val()

    $.ajax({
        type: 'GET',
        url: '/Customer/_EmployeeDetails',
        data: {
            searchParam: searchValue,
            searchOption: selectedOption,
            branchId: branchId
        },
        traditional: true,
        success: function (result) {
            $("#employeeDetails").html("");
            $("#employeeDetails").html(result);
        },
    });
});

$(".btnemployeesearch").on('click', function (e) {
  
    e.stopImmediatePropagation();
    _globalObject = $(this).closest("div.CommonSearchDiv");
    var branchId = $(this).attr("branchId");
    $.ajax({
        type: 'GET',
        url: "/Customer/EmployeeList",
        data: { branchId: branchId },
        success: function (result) {
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
     e.stopImmediatePropagation();
    var employeeId = $(this).closest('tr').attr('id');
    var parent = $(this).parents();
    var id = _globalObject;
    $.ajax({
        type: 'GET',
        url: '/Customer/GetSelectedEmploye',
        data: {
            employeeId: employeeId
        },
        success: function (result) {
            $(id).find(".employeeName").val(result.EmployeeName);
            $(id).find(".employeeId").val(result.EmployeeId);
            $($(parent).find("#employee-pop-up-div")).modal('toggle');
        },
    });

});
$('.employeeName').on("change", function (event) {
   
    var currentEvent = $(this);
    var text = $(this).val();
    // var length = $(this).val().length;
    var Id = $(currentEvent).attr("id");
   // _globalObject = Id;
    $.ajax({
        url: '/Customer/GetEmployeeName',
        data: { text: text },
        dataType: "json",
        success: function (employeeName) {

            var count = employeeName.length;
            if (count == 1) {
                var currentText = employeeName[0].EmployeeName;
                var currentId = employeeName[0].EmployeeId;
                currentEvent.val(currentText);
                $(Id).find(".employeeId").val(currentId);
            }
            else {
                $.ajax({
                    type: 'GET',
                    url: "/Customer/EmployeeList",
                    data: { text: text },
                    success: function (result) {
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
  
    event.stopImmediatePropagation();
    var currentEvent = $(this);
    var text = $(this).val();
    // var length = $(this).val().length;
    var id = $(currentEvent).attr("id");
    _globalObject = id;

    if (event.keyCode == 8) {

        $(".employeeName#" + id).val("");
        $(".employeeId#"+id).val("");

    }
    var checkevent = event.keyCode;
    if (checkevent == 13) {
        $.ajax({
            url: '/Customer/GetEmployeeName',
            data: { text: text },
            dataType: "json",
            success: function (employeeName) {

                var count = employeeName.length;
                if (count == 1) {
                    var currentText = employeeName[0].EmployeeName;
                    var currentId = employeeName[0].EmployeeId;
                    currentEvent.val(currentText);
                    $(id).find(".employeeId").val(currentId);
                }
                else {
                    $.ajax({
                        type: 'GET',
                        url: "/Customer/EmployeeList",
                        data: { text: text },
                        success: function (result) {
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