                  

$('.custContact-create').on("blur", '.contact-value', function () {
    debugger;
    var contactType = $(this).closest(".custContact-create").find(".cust-contact-header-table").find('tr').find('.cnoType').val();
    var emailtext = $(this).val();
    var emailReg;
    var mesg;
    var me = $(this);
    var abc;
    contactType = $(this).closest("tr").find(".cnoType").val();
    if (contactType == '4') {
        emailReg = /^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/;
        abc = emailReg.test(emailtext);
        if (abc == false) {
            mesg = 'Not a valid Email Address.'
        }
    }
    if (contactType == '6') {
        emailReg = /^\d{10}$/;
        abc = emailReg.test(emailtext)
        if (abc == false) {
            mesg = 'Not a valid Mobile Number.'
        }
    }

    if (abc == false) {
        me.closest("tr").find(".contact-type-mesg").html(mesg)
        me.closest("tr").find(".data-valmsg-replace").val(false)

    }
    if (abc == true) {
        me.closest("tr").find(".contact-type-mesg").html("")
        me.closest("tr").find(".data-valmsg-replace").val(true)



    }
})





$(".customer-create-main").on("change", "#CtypeID", function () {

    var id = $(".customer-create-main").find("#CtypeID option:selected").val();
    debugger;
    $.ajax({
        url: "/Customer/GetCascadeCert",
        type: 'GET',
        data: { id: id },
        success: function success(data) {
            debugger;
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

  

//$(".customer-create-main").on("change", "#CtypeID", function () {

//    var id = $(".customer-create-main").find("#CtypeID option:selected").val();
//    debugger;
//    $.ajax({
//        url: "/Customer/GetCompCert",
//        type: 'GET',
//        data: { id: id },
//        success: function success(data) {
//            $("#CCCertID").html("----loading-----");
//            var items;
//            //= " <option>Choose Any</option>";

//            $.each(data, function (i, state) {
//                items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
//            });
//            $('#CCCertID').html(items);
//        }
//    })
//});

$(document).ready(function () {

    var lastcontactType = 0;


    $(".customer-create-main").on("change", "#CtypeID", function () {
        debugger;
        var id = $(".customer-create-main").find("#CtypeID option:selected").val();
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


    $(".customer-create-main").on("change", "#CtypeID", function () {
        debugger;
        var id = $(".customer-create-main").find("#CtypeID option:selected").val();
        $.ajax({
            url: "/Customer/GetCustypeByIsIndividual",
            data: { id: id },
            success: function success(result) {
                //$("#CDepSector").html("----loading-----");
                //var items;
                //// = " <option>Choose Any</option>";

                //$.each(data, function (i, state) {
                //    items += "<option value='" + state.Value + "'>" + state.Text + "</option>";

                //});
                //$('#CDepSector').html(items);

                if(result==true){
                    $('#LabelDateOfBirth').text('Date Of Birth');
                }
                else {
                    $('#LabelDateOfBirth').text('Established Date');
                }
            }
        })
    });
    

    $(".customer-create-main").on("change", "#CtypeID", function () {
        debugger;

        var id = $(".customer-create-main").find("#CtypeID option:selected").val();
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
        debugger;
        var id = $(".customer-create-main").find("#CtypeID option:selected").val();
        var container = $('.customer-type-create').find('.custContact-create').find(".contact_details");
        var container1 = $(this).closest('tr');
        $.ajax({
            type: 'get',
            url: '/Customer/_CustContactCreate',

            data: { id: id },
            success: function (data) {
                debugger;
                $(".cust-contact-table > tbody").append(data);
                var trCount = $("tbody > tr").length;
                if (trCount = 1) {
                    $("tbody > tr").each(function (trCounttotal, obj) {
                        $(obj).find(".delete-cust-contact-add").removeClass("hidden");
                    });
                }

            }
        })
    });
    $('.custContact-create').on('click', '.delete-cust-contact-add', function (control) {
        debugger;
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
                debugger;
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
        debugger
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
        debugger
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
            debugger
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

$('.btn-cancel').click(function () {
    debugger;
    $.ajax({
        type: "GET",
        //  url: "/Customer/CustomerListIndex/",
        // url: "/Customer/CustomerListIndex/",
        url: "/Customer/_CustomerCreate/",

        success: function (data) {
            $('section.content').html(data);


        }
    })
});

$('.customer-create-main').on('click', '.btn-save', function () {
    debugger;
    $('#createSubmitForm').removeData("validator").removeData("unobtrusiveValidation");
    $.validator.unobtrusive.parse($('#createSubmitForm'));

    if ($("#createSubmitForm").valid()) {

        //var val = [];
        //$(':checkbox:checked').each(function (i) {
        //    val[i] = $(this).val();
        //});

        //$(':checkbox:checked').each(function (i) {
        //    val[i] = $(this).val();
        //});

        //var n = $('.checkboxCert:checked').length;
       // $(".checkboxCert:checked").removeAttr('disabled');
        if ($('.checkboxCrt').is(":checked")) {
            // nothing
            debugger;
            //var ary = [];
            //$('.cust-certificate-table tbody tr').each(function (a, b) {
            //    debugger;
            //    //var $tds = $(this).find('td');
            //    var name = $('#CCCertID').val();
            //       var value = $('.checkboxCrt:checked').val();
            //    //var name = $('#CCCertID', a).val();
            //    //var value = $('.checkboxCert', b).val;
            //    ary.push({ Name: name, Value: value });

            //});
            //alert(JSON.stringify(ary));
            var ifCompulsory = false;
            $(".cust-type-certificate").find("table > tbody > tr").each(function (trcount, obj) {
                debugger;
               
                var Status = $(this).find('span').text();
                if ($(obj).find(".checkboxCrt").prop('checked') == false && Status == 'Compulsory') {
                    //ErrorAlert('Please Check Compulsory  certificate',3000);
                    NewCustomerCreateIfError();
                    ifCompulsory = true;
                }
                
            });

            $(".table-certificate").find("table > tbody > tr").each(function (trcount, obj) {
                debugger;

                var Status = $(this).find('span').text();
                if ($(obj).find(".checkboxCrt").prop('checked') == false && Status == 'Compulsory') {
                    //ErrorAlert('Please Check Compulsory  certificate',3000);
                    NewCustomerCreateIfError();
                    ifCompulsory = true;
                }
            

            });
           


     
                  
       
            }
        else {
            InfoAlert("Please Select the Check box!!!", 5000);
            return false;
        }


          
       



       
    }
    if( ifCompulsory==false){
        NewCustomerCreate();
}
   
});


//$('#createSubmitForm').submit(function () {

//});

function NewCustomerCreateIfError() {
  
    $.MessageBox({
        buttonDone: "Yes",
        buttonFail: "No",
        message: "<span style='color:red;'><center>Please Check Compulsory  Certificate !!</center></span> <br/> <span style='padding:4px;'><center>Are you sure?</center></span>"
    }).done(function () {
        //$(".checkboxCert:checked").removeAttr('disabled');
        $('#createSubmitForm').ajaxSubmit({
            success: function (result) {
                //var url;

                debugger;
                if (result.Success) {
                    $.ajax({
                        url: "/Customer/CustomerListIndex",
                        type: 'post',
                        async: false,
                        success: function (data) {
                            debugger
                            $('section.content').html(data);
                            SuccessAlert(result.Msg, 5000);
                            document.getElementById('alert-success').scrollIntoView();
                        }
                    })


                } else {

                    ErrorAlert(result.Msg, 5000);
                    document.getElementById('alert-error').scrollIntoView();
                }
            },
            error: function (data) {
                ErrorAlert(data.responseText, 15000)
            }
        });
    }).fail(function () {

    });
}
function NewCustomerCreate(e) {
    $.MessageBox({
        buttonDone: "Yes",
        buttonFail: "No",
        message: " Are you sure?"
    }).done(function () {
        //$(".checkboxCert:checked").removeAttr('disabled');
        $('#createSubmitForm').ajaxSubmit({
            success: function (result) {
                //var url;

                debugger;
                if (result.Success) {
                    $.ajax({
                        url: "/Customer/CustomerListIndex",
                        type: 'post',
                        async: false,
                        success: function (data) {
                            debugger
                            $('section.content').html(data);
                            SuccessAlert(result.Msg, 5000);
                            document.getElementById('alert-success').scrollIntoView();
                        }
                    })


                } else {

                    ErrorAlert(result.Msg, 5000);
                    document.getElementById('alert-error').scrollIntoView();
                }
            },
            error: function (data) {
                ErrorAlert(data.responseText, 15000)
            }
        });
    }).fail(function () {

    });
}

