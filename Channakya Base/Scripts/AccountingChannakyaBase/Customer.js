$(document).ready(function () {

    var lastcontactType = 0;


    $("#CtypeID").change(function () {

        var id = $("#CtypeID option:selected").val();
       
        $.ajax({
            url: "/Customer/GetCascadeCert",
            type: 'GET',
            data: { id: id },
            success: function success(data) {
                $("#CCCertID").html("----loading-----");
                var items;
                //= " <option>Choose Any</option>";

                $.each(data, function (i, state) {
                    items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
                });
                $('#CCCertID').html(items);
            }
        })
    });
    $("#CtypeID").change(function () {
        var id = $("#CtypeID option:selected").val();
        $.ajax({
            url: "/Customer/GetCascadeSect",
            data: { id: id },
            success: function success(data) {
                $("#CDepSector").html("----loading-----");
                var items;
                // = " <option>Choose Any</option>";

                $.each(data, function (i, state) {
                    items += "<option value='" + state.Value + "'>" + state.Text + "</option>";

                });
                $('#CDepSector').html(items);
            }
        })
    });

    $("#CtypeID").change(function () {

        var id = $("#CtypeID option:selected").val();
        var isind = 0;
        if (id == "") {
            $('.customer-create-main').find(".customer-type-create").html("");
        }
        else {
            $.ajax({
                type: "GET",
                url: "/Customer/GetCustypeByCTypeId",
                data: { ctypeId: id },
                success: function (data) {
                    $(".customer-create-main").find(".customer-type-create").show();
                    $('.customer-create-main').find(".customer-type-create").html("");
                    $('.customer-create-main').find(".customer-type-create").html(data);

                }
            })
        }
        $.ajax({
            type: "GET",
            url: "/Customer/_AttachedCertificate",
            data: { cTypeId: id },
            success: function (data) {
                $(".customer-create-main").find(".cust-type-certificate").show();
                $('.customer-create-main').find(".cust-type-certificate").html("");
                $('.customer-create-main').find(".cust-type-certificate").html(data);

            }
        })
    });

    $("#CtypeID").change(function abc() {
        var id = $("#CtypeID option:selected").val();
        $(".cnoType").html("----loading-----");
        debugger
        var result = "none";
        $.ajax({
            type: 'get',
            url: "/Customer/_CustContactCompulsory",
            data: { id: id },
            contentType: "application/json; charset=utf-8",
            dataType: "html",
            aysnc: false,
            success: function (html) {
               
                $(".custList").html("");
                $(".custList").html(html);
            }
        });
          
    });
    //$.ajax({
    //    url: "/Customer/GetCascadeContactType",
    //    data: { id: id},
    //    success: function success(data) {
    //        $(".cnoType").html("----loading-----");
    //        var items=""
    //            //= " <option>Choose Any</option>";
    //        items += "<option value='" + 3 + "'selected>" +" Email" + "</option>";
    //        $('.cnoType:first').html(items);
    //        $.ajax({
    //            url: "/Customer/GetCascadeContactType",
    //            data: { id: id },
    //            success: function success(data) {
    //                var items1=""
    //                    //= " <option>Choose Any</option>";
    //                items1 += "<option value='" + 5 + "'selected>" + "Mobile" + "</option>";
    //                $('.cnoType:last').append(items1);
    //            }

    //        });
    //    }

    //});
//$.ajax({
//    url: "/Customer/GetCascadeContactType",
//    data: { id: id, selectedId :3},
//    success: function success(data) {
//        $(".cnoType").html("----loading-----");
//        var items = " <option>Choose Any</option>";
//        $.each(data, function (i, state) {
//           
//            if (state.Value == 3 || state.Value==5) {
//                items += "<option value='" + state.Value + "'selected>" + state.Text + "</option>";
//            }
//            else
//            {
//                items += "<option value='" + state.Value + " '>" + state.Text + "</option>";
//            }
//        });
//        $('.cnoType').html(items);
//    }
//})


$('.add-cust-contact').on('click', function (e) {
   
    var id = $("#CtypeID").val();
    var container = $('.customer-type-create').find('.custContact-create').find(".contact_details");
    var container1 = $(this).closest('tr');
    $.ajax({
        type: 'get',
        url: '/Customer/_CustContactCreate',

        data: { id: id },
        success: function (data) {
            $(".cust-contact-table > tbody").append(data);
            var trCount = $("tbody > tr").length;
            if (trCount = 1) {
                $("tbody > tr").each(function (trCounttotal, obj) {
                    $(obj).find(".delete-cust-contact").removeClass("hidden");
                });
            }

        }
    })
});
$('.custContact-create').on('click', '.delete-cust-contact', function (control) {

    var checked = $(this).closest("tr").find(".isChecked").prop("checked");
    var sid = $(this).closest("tr").find(".cnoType").val();
    if (checked) {
        $(".cust-contact-table > tbody >tr").each(function (trCounttotal, obj) {

            if ($(obj).find('.cnoType').val() == sid) {
                {
                    $(obj).find(".isChecked").prop("disabled", false);
                }
            }
        });
    }
    var parent = $(this).closest("tr");
    var cnotypeid = $(this).closest("tr").find(".CCID").val();
   
    if (cnotypeid != 0) {
        $(this).closest("tr").find(".IsDeleted").val(true);
        $(parent).addClass('hidden');
    }
    else {
        $(this).closest("tr").remove();
    }
    var trCount = $(".cust-contact-table > tbody > tr:visible").length;


    if (trCount < 2) {
        $('.cust-contact-table   > tbody >tr ').find(".delete-cust-contact").addClass("hidden");
    }
});
function custcontactdeleteshowhide(container) {
   

    var hiddencount = $(container).find('.delete-cust-contact-div').filter(":hidden").size();
    var allcount = $(container).find('.delete-cust-contact-div hidden').length;
    if (allcount == 1) {
        $(container).find('.delete-cust-contact-div').hide();
    }
    else if (allcount != hiddencount) {

        $(container).find('.delete-cust-contact-div').removeClass("hidden");
    }
}
function DeleteContact(control) {

    var parent = $(control).closest("tr");
    $(parent).remove();
    var trCount = $(".cust-contact-table > tbody > tr").length;
    if (trCount < 3) {
        $('.cust-contact-table   > tbody >tr').find(".delete-cust-contact").addClass("hidden");
    }
}
$('.add-cust-contact-person').on('click', function (e) {
   
   // var container = $('.customer-type-create').find('.custContact-person-create').find(".contact_details");
    var container1 = $(this).closest('tr');
    $.ajax({
        type: 'get',
        url: '/Customer/_CustContactPerson',

       // data: { id: id },
        success: function (data) {
            $(".cust-contact-person-table > tbody").append(data);
            var trCount = $("tbody > tr").length;
            if (trCount = 1) {
                $("tbody > tr").each(function (trCounttotal, obj) {
                    $(obj).find(".delete-cust-contact-person").removeClass("hidden");
                });
            }

        }
    })
});
$('.custContact-person-create').on('click', '.delete-cust-contact-person', function (control) {

   
    var parent = $(this).closest("tr");
    var cnotypeid = $(this).closest("tr").find(".CPId").val();
   
    if (cnotypeid != 0) {
        $(this).closest("tr").find(".CPDeleted").val(true);
        $(parent).addClass('hidden');
    }
    else {
        $(this).closest("tr").remove();
    }
    $(parent).hide();
    var trCount = $(".cust-contact-person-table > tbody > tr").length;
    if (trCount < 2) {
        $('.cust-contact-person-table > tbody > tr').find(".delete-cust-contact-person").addClass("hidden");
    }
});

$(function () {
    $(".datepicker").datepicker();
});

$('.cust-contact-table').on('change', '.isChecked', function (control) {
   
    var checked = $(this).prop("checked");
    var sid = $(this).closest("tr").find(".cnoType").val();
    var me = $(this);

    $(".cust-contact-table > tbody >tr").each(function (trCounttotal, obj) {

        if ($(obj).find('.cnoType').val() == sid) {
            {
                $(obj).find(".isChecked").prop("disabled", checked);
            }
        }
    });
    $(me).prop("disabled", false);

})

// SIR WALA
$('.cust-contact-table').on('focus', '.cnoType', function (control) {
    lastcontactType = $(this).closest("tr").find(".cnoType").val();
});

$('.cust-contact-table').on('change', '.cnoType', function (control) {
    //var checked = $(this).closest("tr").prop("checked");
   
    var isChecked = false;
    if (lastcontactType != 0) {
        var checked = $(this).closest("tr").find(".isChecked").prop("checked");
        if (checked == true) {
            $(this).closest("tr").find(".isChecked").prop("checked", false)
            $(".cust-contact-table > tbody >tr").each(function (trCounttotal, obj) {

                if ($(obj).find('.cnoType').val() == lastcontactType) {
                    $(this).closest("tr").find(".isChecked").prop("disabled", false);

                    //if ($(obj).find(".isChecked").prop("checked")) {
                    //    isChecked = true;
                    //};
                }
            });
        }
    }
    var sid = $(this).closest("tr").find(".cnoType").val();
    // var me = $(this);
    $(".cust-contact-table > tbody >tr").each(function (trCounttotal, obj) {

        if ($(obj).find('.cnoType').val() == sid) {
            if ($(obj).find(".isChecked").prop("checked")) {
                isChecked = true;
            };
        }
    });
    $(this).closest("tr").find(".isChecked").prop("checked", false)
    $(this).closest("tr").find(".isChecked").prop("disabled", isChecked)
    lastcontactType = sid;
    // $(me).prop("disabled", isChecked);
})


})