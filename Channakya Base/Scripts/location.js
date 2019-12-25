$(document).ready(function () {

    $('.location-list').on('click', '.btnMoveConfirm', function () {
        debugger;
        var idObj = {};
        idObj.Checked = [];

        $('.location').find('input:checkbox:checked').each(function () {
            var intLocationId = $(this).closest('.location').attr("nodeId");
            idObj.Checked.push(intLocationId);
        });

        $.ajax({
            type: "GET",
            url: "/Location/_GetLocationMoveTree",
            dataType: "html",
            traditional: true,
            data: { selectedNodeId: idObj.Checked },
            success: function (data) {
                debugger;
                $('.location-list').find('.move-location-modal').modal('show');
                $('.location-container').updateTreeview('Location', '_GetLocationTree', '.child-body', false, true, 0, false, idObj.Checked, data);
            }
        });
    });

    $(".location-tree").on('nodeClick', function (e, data) {
        debugger;
        e.stopImmediatePropagation();
        var IsGroup = data.isGroup;
        if (IsGroup == "true") {
            var currentNodeId = data.nodeId;
            var container = $(this).closest('.location-container').find('.location-list');
            $.ajax({
                data: { parentNodeId: currentNodeId },
                url: "/Location/_ListChildLocation",
                dataType: "html",
                success: function (data) {
                    debugger;
                    $(container).html('');
                    $(container).append(data);
                }
            });
        }
    });

    $('.location-list').on('nodeClick', function (e, data) {
        debugger;
        var parentNodeId = data.nodeId;
        $('.location-list').find('.moveConfirmation').show();
        $('.location-list').find('.btnMoveLocation').attr("nodeid", parentNodeId);
        $('.btnMoveLocation').focus();
    });

    $('.location-list').on('click', '.btnDrill', function () {
        debugger;
        var container = $(this).closest('.location-list');
        var parentNodeId = $(this).closest(".location").attr("nodeId");

        $.ajax({
            data: { parentNodeId: parentNodeId },
            url: "/Location/_ListChildLocation",
            dataType: "html",
            success: function (data) {
                debugger;
                $('.btnDrill').tooltip('hide');
                $(container).html('');
                $(container).append(data);
            }
        });
        $('.location-tree').selectNode(parentNodeId);
    });

    $('.location-list').on('click', '.btnDetails', function () {
        debugger;
        var container = $(this).closest('.location-list');
        var parentNodeId = $(this).closest('.location').attr('nodeId');

        $.ajax({
            type: "GET",
            url: "/Location/_LocationDetail",
            data: { parentNodeId: parentNodeId },
            dataType: "html",
            success: function (data) {
                debugger;
                $('.btnDetails').tooltip('hide');
                $(container).find('.detail-location-modal').modal('show');
                $('.location-list').find('.detail-location-content').html('');
                $('.location-list').find('.detail-location-content').append(data);
            }
        });
    });

    $('.location-list').on('click', '.btnEdit', function (event) {
        debugger;
        event.stopImmediatePropagation();
        var container = $(this).closest('.location-list');
        var parentNodeId = $(this).closest('.location').attr('nodeid');

        $.ajax({
            data: { parentNodeId: parentNodeId },
            url: "/Location/_EditLocation",
            dataType: "html",
            success: function (data) {
                $('.btnEdit').tooltip('hide');
                $(".edit-location-content").html('');
                $(".edit-location-content").append(data);
                $(container).find('.edit-location-modal').modal('show');
                var result = $('.location-list').find('#result').val()
                if (result == 0) {
                    var container2 = $('.location-list').find('.location-form-body');
                    var locationName = $('.location-list').find('#locationName').val();
                    $(container2).find('.location-name').val(locationName);
                    $(container2).find('.location-name').prop("disabled", true);
                    $(container2).find('.submit-location-edit').prop("disabled", true);
                    $(container2).find('.error-message').text("Cannot edit location because it is being referenced by a customer.");
                }
                else {
                    var container2 = $('.location-list').find('.location-form-body');
                    var maxLevel = $('.location-list').find('#maxLevel').val();
                    var locationName = $('.location-list').find('#locationName').val();
                    $(container2).find('.location-max-level').val(maxLevel);
                    $(container2).find('.location-name').val(locationName);
                }
            }
        });
    });

    $('.location-list').on('click', '.btnback', function () {
        debugger;
        var container = $(this).closest('.location-list');
        var nodeId = $(container).find('#parentId').val();

        $.ajax({
            data: { nodeId: nodeId },
            url: "/Location/_GetParentId",
            dataType: "json",
            success: function (data) {
                debugger;
               // $('.location-tree').selectNode(data);
                $.ajax({
                    data: { parentNodeId: data },
                    url: "/Location/_ListChildLocation",
                    dataType: "html",
                    success: function (data) {

                        $(container).html('');
                        $(container).append(data);
                    }
                });
            }
        });
    });

    $('.location-list').on('click', '.btnMove', function () {
        debugger;
        $('.location-list').find('.boxMove').show();
        $(this).hide();
        $('.location-list').find('.btnCancelMove').show();
        $('.location-list').find('.btnMoveConfirm').show();
        $('.location-list').find('.createLocation').hide();
        $('.location-list').find('.btnDrill').prop("disabled", true);
        $('.location-list').find('.btnEdit').prop("disabled", true);
    });


    $('.location-list').on('click', '.btnCancelMove', function () {
        debugger;
        $('.location-list').find('.boxMove').hide();
        $(this).hide();
        $('.location-list').find('.btnMove').show();
        $('.location-list').find('.createLocation').show();
        $('.location-list').find('.btnMoveConfirm').hide();
        $('.location-list').find('.btnDrill').prop("disabled", false);
        $('.location-list').find('.btnEdit').prop("disabled", false);
        $('.location-list').find('input:checkbox:checked').attr("checked", false);
    });


    $('.location-list').on('click', '.btnCancelLocationMove', function () {
        debugger;
        $('.location-list').find('.moveConfirmation').hide();
    });


    $('.location-list').on('click', '.btnMoveOk', function () {
        debugger;
        var parentNodeId = $('.location-list').find('#parentId').val();
        $('.location-list').find('.modal').modal('hide');
        $('.modal-backdrop').remove();
        $('.location-tree').selectNode(parentNodeId);
    });

    $('.location-list').on('click', '.btnMoveLocation', function () {
        debugger;
        var parentNodeId = $(this).attr('nodeid');
        var container = $('.location-tree').find('.treeview-text').filter(function () {
            return $(this).attr('nodeid') == parentNodeId;
        });
        var idObj = {};
        idObj.Checked = [];

        $('.location').find('input:checkbox:checked').each(function () {
            var intLocationId = $(this).closest('.location').attr("nodeId");
            idObj.Checked.push(intLocationId);
        });

        $.ajax({
            type: "GET",
            url: "/Location/_MoveLocation",
            traditional: true,
            data: { selectedNodeId: idObj.Checked, parentNodeId: parentNodeId },
            success: function (data) {
                $('.location-list').find('.location-move-body').html('');
                $('.location-list').find('.confirmQuestion').hide();
                $('.location-list').find('.moveConfirmation').hide();
                $('.location-list').find('.btnMoveLocationCancel').hide();
                $('.location-list').find('.boxMove').hide();
                $('.location-list').find('.btnMoveOk').show();
                $('.location-list').find('input:checkbox:checked').attr("checked", false);
                $('.location-list').find('.btnMove').show();
                $('.location-list').find('.createLocation').show();
                $('.location-list').find('.btnMoveConfirm').hide();
                $('.location-list').find('.btnCancelMove').hide();

                var result = data.message;
                for (var i = 0; i < result.length; i++) {
                    var firstWord = result[i].slice(0, result[i].indexOf(" "));
                    if (firstWord == "Cannot") {
                        $('.location-list').find('.location-move-body').append('<span class="text-danger">' + result[i] + '</span><br />');
                    }
                    else {
                        $('.location-list').find('.location-move-body').append('<span class="text-success">' + result[i] + '</span><br />');
                    }
                }
                $('.location-container').updateTreeview('Location', '_GetLocationTree', '.treeview-area', false, true, parentNodeId, false, 0, data.rootNode);
            }
        });
    });

    $('.location-list').on('click', '.createLocation', function (event) {
        debugger;
        event.preventDefault();
        event.stopImmediatePropagation();
        var container = $(this).closest('.location-list');
        var parentNodeId = $(container).find('#parentId').val();

        $.ajax({
            data: { parentNodeId: parentNodeId },
            url: "/Location/_AddLocation",
            dataType: "html",
            success: function (data) {
                debugger;
                $(".add-location-content").html('');
                $(".add-location-content").append(data);
                $(container).find('.add-location-modal').modal('show');
                //SuccessAlert("Location Saved Successfully",3000);
            }
        });
    });

    $('.location-list').on('click', '.location-remove', function () {
        debugger;
        var container = $(this).closest('.location-list');
        var parentNodeId = $(this).closest('.location').attr('nodeid');
        $(container).find('.delete-location-modal').modal('show');

        $.ajax({
            data: { parentNodeId: parentNodeId },
            url: "/Location/_ConfirmDelete",
            dataType: "html",
            success: function (data) {
                $(".delete-location-content").html('')
                $(".delete-location-content").append(data)
            }
        });
    });


    $('.location-list').on('click', '.btn-delete', function (event) {
        debugger;
        event.stopImmediatePropagation();
        var container = $('.location-list').find('.location-form-body');
        var parentNodeId = $('.location-list').find('#parentNode').val();
        $(this).hide();
        $.ajax({
            data: { parentNodeId: parentNodeId },
            url: "/Location/_DeleteLocation",
            dataType: "Json",
            success: function (data) {
                var result = data.message;
                $(container).html('');
                $(container).append(result);
                $('.location-list').find('.btn-cancel').hide();
                $('.location-list').find('.btnDeleteDone').show();
                $('.close').prop("disabled", true);
                $('.location-container').updateTreeview('Location', '_GetLocationTree', '.treeview-area', false, true, 0, false, 0, 1);
            }
        });
    });

    $('.location-list').on('click', '.btnDeleteDone', function () {
        debugger;
        var parentNodeId = $('.location-list').find('#parentId').val();
        $('.location-list').find('.modal').modal('hide');
        $('.modal-backdrop').remove();
        $('.location-tree').selectNode(parentNodeId);
        InfoAlert("Location deleted Successfully", 3000);
    });

    $('.location-list').on('click', '.btn-cancel', function () {
        $('.location-list').find('.modal').modal('hide');
        $('.modal-backdrop').remove();
    });


    //$('.location-list').on('click', '.add-location-button', function () {
    //      
    //    $(this).hide();
    //    $('.btnCreateDone').show();
    //    $('.close').prop("disabled", true);
    //    var formData = new FormData($(this).closest("form")[0]);
    //    $.post("/Location/_AddLocation", formData, function (data) {
    //          
    //        $(this).closest("modal").hide();
    //    });
    //})

     //$.validator.unobtrusive.parse($("#location-form-body"));
    $('.location-list').on('click', '.add-location-button', function (event) {
        var element = $(this);
        $('#location-form-body').removeData("validator").removeData("unobtrusiveValidation");
        $.validator.unobtrusive.parse($("#location-form-body"));
        debugger;
        if ($('#location-form-body').valid() == false) {
            InfoAlert('Please Fill out the form correctly', 3000);
            return false;
        }
        event.preventDefault();
        event.stopImmediatePropagation();

        var LocationName = $('.LocationName').val();
        var LId = $('.LId').val();
        
        $.ajax({
            url: "/Location/CheckLocation",
            type: 'GET',
            async: false,
            data: { LocationName: LocationName, LId: LId },
            success: function (result) {
                // var message = $(this).closest("form").find(".field-validation-error").text();
                if (result == false) {
                    debugger;
                    InfoAlert("Location Name not valid", 3000);
                    return false;
                }
                else {
                    debugger;


                    if ($(element).closest("form").valid() == true) {
                        $(this).hide();
                        $('.btnCreateDone').show();
                        $('.add-location-button').hide();
                        $('.close').prop("disabled", true);
                    }
                }
            }

        });
    });


    $('.location-list').on('click', '.btnCreateDone', function (event) {
        debugger;
        //event.preventDefault();
        event.stopImmediatePropagation();
   //     var parentNodeId = $('.location-list').find('#parentId').val();
        $("#location-form-body").ajaxSubmit({
            beforeSubmit: function () {


            },

            success: function (result) {
                debugger;
                if (result.Success) {
                    debugger;
                    var container = $('.location-list').closest('.location-list');
              
                 //   var parentNodeId = $('.location-list').closest(".location").attr("nodeId");
                    var parentNodeId = $('.location-list').find('#parentId').val();
                    
                    $.ajax({
                        data: { parentNodeId: parentNodeId },
                        url: "/Location/_ListChildLocation",
                        dataType: "html",
                        success: function (data) {
                           
                            $(container).html('');
                            $(container).append(data);
                        }
                    });
              
                    //$.ajax({
                    //    url: "/Location/_ListChildLocation",
                    //    async: false,
                    //    success: function (result) {
                    //        debugger;
                    //        $('section.content').html(result);
                           
                    //    }
                    //});

                    SuccessAlert(result.Msg, 5000);
                    document.getElementById('alert-success').scrollIntoView();
                   
                }
                else {
                    ErrorAlert(result.Msg, 5000);
                    document.getElementById('alert-error').scrollIntoView();
                }
            },
            error: function () {
                ErrorAlert(data.responseText, 15000);
            }
        });

     
        $('.location-list').find('.modal').modal('hide');
        $('.modal-backdrop').remove();
        debugger;
        //$('.location-tree').selectNode(parentNodeId);
        $(".add-location-content").html("");


        debugger;
    


    });


    $('.location-list').on('click', '.btnRemoveLevelDiv', function () {
        debugger;
        var container = $(this).parent();
        var container2 = $('.location-list').find('.location-max-level');
        var locationName = $('.location-list').find('#locationName').val();
        var newLocationName = $('.location-list').find('.location-name').val();
        var maxLevel = $('.location-list').find('#maxLevel').val();
        $(container).remove();
        if (locationName != newLocationName) {
            $('.location-list').find('.submit-location-edit').prop("disabled", false);
        }
        else {
            $('.location-list').find('.submit-location-edit').prop("disabled", true);
        }
        $('.location-list').find('.edit-location-button').hide();
        $(container2).val(maxLevel);
    });


    $('.location-list').on('input', '.location-name', function () {
        debugger;
        var maxLevel = $('.location-list').find('#maxLevel').val();
        var newMaxLevel = $('.location-list').find('.location-max-level').val();
        var locationName = $('.location-list').find('#locationName').val();
        var newLocationName = $('.location-list').find('.location-name').val();
        $('.location-list').find('.submit-location-edit').prop("disabled", false);

        if (locationName != newLocationName) {
            if (maxLevel != "" && newMaxLevel != undefined) {
                if (maxLevel != newMaxLevel) {
                    $('.location-list').find('.submit-location-edit').prop("disabled", true);
                }
                else if (newMaxLevel = undefined) {
                    $('.location-list').find('.submit-location-edit').prop("disabled", false);
                }
                else {
                    $('.location-list').find('.submit-location-edit').prop("disabled", false);
                }
            }
            else {
                if (locationName != newLocationName) {
                    $('.location-list').find('.submit-location-edit').prop("disabled", false);
                }
                else {
                    $('.location-list').find('.submit-location-edit').prop("disabled", true);
                }
            }
        }
        else {
            $('.location-list').find('.submit-location-edit').prop("disabled", true);
        }
    });


    $('.location-list').on('input', '.location-max-level', function () {
        debugger;
        var maxLevel = $('.location-list').find('#maxLevel').val();
        var newMaxLevel1 = parseInt(maxLevel) - 2;
        var newMaxLevel2 = parseInt(maxLevel) + 2;
        var newMaxLevel = $('.location-list').find('.location-max-level').val();
        var locationName = $('.location-list').find('#locationName').val();
        var newLocationName = $('.location-list').find('.location-name').val();
        var container = $('.location-list').find('.select-level-div').val();
        if (maxLevel != newMaxLevel) {
            $('.location-list').find('.edit-location-button').show();
            if (newMaxLevel <= newMaxLevel1) {
                alert("Cannot decrement value more than one at a time");
                $('.location-list').find('.location-max-level').val(parseInt(maxLevel) - 1);
            }
            else if (newMaxLevel >= newMaxLevel2) {
                alert("Cannot increment value more than one at a time");
                $('.location-list').find('.location-max-level').val(parseInt(maxLevel) + 1);
            }
            if (container == undefined) {
                // nothing;
            }
            $('.location-list').find('.submit-location-edit').prop("disabled", true);
        }
        else {
            if (locationName != newLocationName) {
                $('.location-list').find('.submit-location-edit').prop("disabled", false);
            }
            $('.location-list').find('.edit-location-button').hide();
            $('.location-list').find('.select-level-div').remove();
            $('.location-list').find('.edit-location-button').hide();
        }
    });


    $('.location-list').on('click', '.edit-location-button', function () {
        debugger;
        var container = $('.location-list').find('.location-level-fix')
        var newMaxLevel = $('.location-list').find('.location-max-level').val();
        var parentNodeId = $('.location-list').find('#currentId').val();
        $(this).hide();
        $('.location-list').find('.submit-location-edit').prop("disabled", true);
        $.ajax({
            type: "GET",
            url: "/Location/_DeleteLevel",
            data: { parentNodeId: parentNodeId, newMaxLevel: newMaxLevel },
            dataType: "html",
            success: function (data) {
                debugger;
                $(container).html('');
                $(container).append(data);
            }
        });
    });


    $('.location-list').on('change', '.level-select', function () {
        debugger;
        var maxLevel = $('.location-list').find('#maxLevel').val();
        var newMaxLevel = $('.location-list').find('.location-max-level').val();
        var levelValue = $(this).attr('value');
        if (newMaxLevel < maxLevel) {
            $('.location-list').find('.btnDeleteLevel').attr('level', levelValue);
            $('.location-list').find('.btnDeleteLevel').prop("disabled", false);
        }
        else {
            $('.location-list').find('.btnCreateLevel').attr('level', levelValue);
            $('.location-list').find('.btnCreateLevel').prop("disabled", false);
        }
    });


    $('.location-list').on('click', '.btnDeleteLevel', function () {
        debugger;
        var container = $('location-list').find('.btnYes');
        var selectedLevel = $(this).attr('level');
        var selectedRoot = $('.location-list').find('#currentId').val();
        $(this).prop("disabled", true);
        $('.location-list').find('.btnRemoveLevelDiv').prop("disabled", true);
        $('.location-list').find('.delete-selected-level').toggleClass('display-none');
        $('.location-list').find('.btnYes').attr("selectedLevel", selectedLevel);
        $('.location-list').find('.btnYes').attr("selectedRoot", selectedRoot);
    });


    $('.location-list').on('click', '.btnCreateLevel', function () {
        debugger;
        var level = $(this).attr('level');
        $(this).prop("disabled", true);
        $('.location-list').find('.btnRemoveLevelDiv').prop("disabled", true);
        $('.location-list').find('.btnCreateLocation').prop("disabled", true);
        $('.location-list').find('.create-new-level').toggleClass('display-none');
        $('.location-list').find('.btnCreateLocation').attr("selectedLevel", level);
    });


    $('.location-list').on('input', '.new-level-name', function () {
        debugger;
        if ($(this).val() == "") {
            $('.location-list').find('.btnCreateLocation').prop("disabled", true);
        }
        else {
            $('.location-list').find('.btnCreateLocation').prop("disabled", false);
        }
    });


    $('.location-list').on('click', '.btnNo', function () {
        debugger;
        $('.delete-selected-level').toggleClass('display-none');
        $('.btnRemoveLevelDiv').prop("disabled", false);
        $('.btnDeleteLevel').prop("disabled", false);
    });


    $('.location-list').on('click', '.btnYes', function () {
         debugger;
        var level = $(this).attr("selectedLevel");
        var rootNode = $(this).attr("selectedRoot");
        var locationName = $('.location-list').find('#locationName').val();
        var newLocationName = $('.location-list').find('.location-name').val();
        $(this).prop("disabled", true);

        $.ajax({
            type: "GET",
            url: "/Location/_DeleteSelectedLevel",
            data: { level: level, rootNode: rootNode },
            dataType: "Json",
            success: function (data) {
                debugger;
                alert("The selected level has been deleted!");
                $('.location-list').find('#maxLevel').val(data);
                $('.location-list').find('.submit-location-edit').prop("disabled", false);
                $('.location-container').updateTreeview('Location', '_GetLocationTree', '.treeview-area', false, true, 0, false, 0, 1);
            }
        });
    });


$('.location-list').on('click', '.btnCancelCreate', function () {
    debugger;
        $('.create-new-level').toggleClass('display-none');
        $('.btnRemoveLevelDiv').prop("disabled", false);
        $('.btnCreateLevel').prop("disabled", false);
    });


    $('.location-list').on('click', '.btnCreateLocation', function () {
        debugger;
        var rootNode = $('.location-list').find('#currentId').val();
        var level = $(this).attr("selectedLevel");
        var locationName = $('.location-list').find('.new-level-name').val();
        $.ajax({
            type: "GET",
            url: "/Location/_CreateSelectedLevel",
            data: { rootNode: rootNode, level: level, locationName: locationName },
            dataType: "JSON",
            success: function (data) {
                debugger;
                alert("The selected level has been preceeded by a new level!");
                $('.location-list').find('#maxLevel').val(data);
                $('.location-list').find('.submit-location-edit').prop("disabled", false);
                $('.location-container').updateTreeview('Location', '_GetLocationTree', '.treeview-area', false, true, 0, false, 0, 1);
            }
        });
    });


    $('.location-list').on('click', '.submit-location-edit', function () {
        debugger;
        alert("Location edited successfully!");
        $(this).hide();
        $('.location-list').find('.modal').modal('hide');
        $('.modal-backdrop').remove();
        $('.location-list').find('.btnEditDone').show();
        $('.close').prop("disabled", true);
    });


    //$('.location-list').on('click', '.btnEditDone', function () {
    //    var parentNodeId = $('.location-list').find('#parentId').val();
    //    $('.location-list').find('.modal').modal('hide');
    //    $('.modal-backdrop').remove();
    //    $('.location-tree').selectNode(parentNodeId);
    //})


$('.location-list').on('', '.btnDetails', function () {
    debugger;
        $('.btnDetails').tooltip('hide');
    });
});
