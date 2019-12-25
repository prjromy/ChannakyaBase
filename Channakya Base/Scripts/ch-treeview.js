

$(document).ready(function () {
    $('.treeview-area').on('click', '.treeview-icon', function (e) {
        //debugger;
        e.stopPropagation();
        var parentClass = $(this).closest("li");
        $(parentClass).find('ul:first').toggle();
        $(this).toggleClass("treeview-expand");
    });


    $('.ch-treeview').unbind('keyup').bind('keyup', '.text-filter-tree', function (event) {
        debugger;
        event.preventDefault();
        //ddd
        var findParent = $(this).closest('.treeview-popup');
        var checkevent = event.keyCode;

        if (checkevent == 40) {
            var findActive = $(this).closest('.ch-treeview').find('.treeview-text').hasClass('treeview-selected');

            if (findActive == true) {
                var activeClass = $(this).closest('.ch-treeview').find('.treeview-selected')
                var nextSibling = $(activeClass).closest('li').next('li');
                var anotherSibling = $(activeClass).closest('li').parent().parent().next('li');
                var nextSiblings = $(activeClass).closest('li').parent().parent().parent().parent().next('li');

                var hasNext = $(activeClass).closest('li').find('li');
                if ($(hasNext).length > 0) {
                    activeClass.removeClass('treeview-selected');
                    hasNext.find('.treeview-text').first().addClass('treeview-selected')

                }
                else {
                    if ($(nextSibling).length > 0) {
                        activeClass.removeClass('treeview-selected');
                        nextSibling.find('.treeview-text').first().addClass('treeview-selected');
                    }
                    else {
                        if ($(anotherSibling).length > 0) {
                            activeClass.removeClass('treeview-selected');
                            anotherSibling.find('.treeview-text').first().addClass('treeview-selected');
                        }
                        else
                            if ($(nextSiblings).length > 0) {
                                activeClass.removeClass('treeview-selected');
                                nextSiblings.find('.treeview-text').first().addClass('treeview-selected');
                            }
                    }

                }

            }

            //var clicked = $(event.target); // get the element clicked  
            //try {
            //        //$dlg_dpicker.dialog("close");
            //        //$dlg_dpicker.remove();
            //        //$dlg_dpicker = null;
            //        selectedObject = null;

            //} catch (err) {
            //    return;
            //}
        }
        if (checkevent == 38) {

            var findActive = $(this).closest('.ch-treeview').find('.treeview-text').hasClass('treeview-selected');

            if (findActive == true) {
                var activeClass = $(this).closest('.ch-treeview').find('.treeview-selected')
                var anotherClass = $(activeClass).closest('li').prev().find('li').last()
                var hasprevSibling = $(activeClass).closest('li').prev().last()

                var hasPrev = $(activeClass).closest('ul').closest('li');
                if ($(anotherClass).length > 0) {
                    activeClass.removeClass('treeview-selected');
                    anotherClass.find('.treeview-text').addClass('treeview-selected')

                }

                else {
                    if ($(hasprevSibling).length > 0) {
                        activeClass.removeClass('treeview-selected');
                        hasprevSibling.find('.treeview-text').addClass('treeview-selected')
                    }

                    else {
                        if ($(hasPrev).length > 0) {
                            activeClass.removeClass('treeview-selected');
                            hasPrev.find('.treeview-text').first().addClass('treeview-selected')

                        }
                    }
                }
            }

        }
        if (checkevent == 13) {
            debugger;
            var activeClass = $(this).closest('.ch-treeview').find('.treeview-selected')
            var selectedtext = $(activeClass).attr('nodetext');
            var textId = $(activeClass).attr('nodeId');
            var findParent = $(this).closest('.treeview-popup');
            var mainClass = $(activeClass).closest('.treeview-area');

            var wrapperClass = $(mainClass).closest('.ch-treeview').parent();
            var wrapperClass = $(mainClass).closest('.ch-treeview');
            var query = $(this).find('.input-group').find("#text-filter-tree").val();
            var div = $(this).closest('.menu-treeview').find('.treeview-area');
            if ($(findParent).length > 0) {
                $("div#PMenuId").find('.display-txt').val(selectedtext);
                $("div#PMenuId").find('.internal-value').val(textId);
                $dlg_dpicker.dialog("close");
            }
            else {
                $("div#PMenuId").find('.display-txt').val(selectedtext);
                $("div#PMenuId").find('.internal-value').val(textId);



                var TreeViewParam = {
                    Controller: '',
                    Action: '',
                    WithCheckBox: $(div).attr("withcheckbox"),
                    AllowSelectGroup: $(div).attr("allowselectgroup"),
                    WithImageIcon: $(div).attr("withimageicon"),
                    WithOutMe: $(div).attr("excludeme"),// data.withOutMe,
                    Title: '',
                    SelectedNodeId: textId,
                    Filter: query,
                    SelectedNodeText: ''
                }
               

                $(wrapperClass).trigger('filterTree', [{ fromPopUp: 'true', param: TreeViewParam }]);


            }
        }
    });

    $('.treeview-area').on('click', '.treeview-image', function (e) {
        //debugger;
        e.stopPropagation();
        var parentClass = $(this).closest("li");
        $(parentClass).find('ul:first').toggle();
        $(parentClass).find('.treeview-icon').toggleClass('treeview-expand');
    });

    $('.treeview-area').on('click', '.treeview-text', function (e) {
        debugger;

        e.stopPropagation();
        var selectedtext = $(this).attr('nodetext');
        var textId = $(this).attr('nodeId');
        var hasChilds = $(this).closest('li').find('ul');
        var allowSelect = "true";
        if ($(hasChilds).length > 0) {
            allowSelect = "false";
        }

        var parentClass = $(this).closest("li");

        var mainClass = $(this).closest('.treeview-area');

        var wrapperClass = $(mainClass).closest('.ch-treeview').parent();

        var id = $(this).attr("nodeid");
        var pid = $(this).attr("nodepid");

        var text = $(this).attr("nodetext");
        var isGroup = $(this).attr("nodeisgroup");
        var allowSelectGroupNode = $(mainClass).attr("allowselectgroupnode").replace('"', '').replace('"', '');

        if (isGroup == "true") {
            var b = $(parentClass).find('.treeview-icon').attr('class');
            if (b.indexOf('treeview-expand') == 0) {
                $(parentClass).find('ul:first').toggle();
                $(parentClass).find('.treeview-icon').toggleClass('treeview-expand');
            }
        }
        $(mainClass).find('.treeview-selected').toggleClass('treeview-selected');
        $(this).toggleClass('treeview-selected');

        var fromTree = true;
        $(wrapperClass).trigger('nodeClick', [{ nodeId: id, parentNodeId: pid, nodeText: text, isGroup: isGroup, allowSelectGroupNode: allowSelectGroupNode, allowSelect: allowSelect, fromTree: fromTree }]);


    });

    $('.treeview-area').on('change', '.treeview-checkbox', function (e) {
        //debugger;
        var thisli = $(this).closest("li").find("ul").find(".treeview-checkbox").not(this).prop('checked', this.checked);
    });

    $('.treeview-button').on('click', function () {
        //debugger;
        $(this).parent().parent().find('.treeview-area').find('li').slideToggle();
    });

    $.fn.updateTreeview = function (controller, action, selectedNode, rootNode) {
        debugger;
        $('.ch-treeview').each(function () {
            debugger;
            var mainClass = $(this).find('.treeview-area');
            //var pnode = ViewBag.parentNodeId;
            var _allowSelectGroupNode = $(mainClass).attr("allowselectgroupnode").replace('"', '').replace('"', '');
            var _withImageIcon = $(mainClass).attr("withimageicon").replace('"', '').replace('"', '');
            var _withCheckBox = $(mainClass).attr("withcheckbox").replace('"', '').replace('"', '');
            var _withOutMe = $(mainClass).attr("withoutme").replace('"', '').replace('"', '');

            var container = $(this).parent();
            var url = '/' + controller + '/' + action;
            $.ajax({
                url: url,
                data: { allowSelectGroupNode: _allowSelectGroupNode, withImageIcon: _withImageIcon, withCheckBox: _withCheckBox, selectedNode: selectedNode, withOutMe: _withOutMe, rootNode: rootNode },
                dataType: "html",
                success: function (data) {

                    debugger;
                    $(mainClass).html(data).closest('.ch-treeview').parent().SelectNode(selectedNode);

                }
            })

        });
    };
    $.fn.updateTreeview2 = function (controller, action, selectedNode, rootNode, isEdit, tempPnode) {
        debugger;
        $('.ch-treeview').each(function () {
            debugger;
            var mainClass = $(this).find('.treeview-area');
            var _allowSelectGroupNode = $(mainClass).attr("allowselectgroupnode").replace('"', '').replace('"', '');
            var _withImageIcon = $(mainClass).attr("withimageicon").replace('"', '').replace('"', '');
            var _withCheckBox = $(mainClass).attr("withcheckbox").replace('"', '').replace('"', '');
            var _withOutMe = $(mainClass).attr("withoutme").replace('"', '').replace('"', '');
            var pnode = tempPnode; //Dorje

            var container = $(this).parent();
            var url = '/' + controller + '/' + action;
            $.ajax({
                url: url,
                data: {selectedNode: selectedNode, allowSelectGroupNode: _allowSelectGroupNode, withImageIcon: _withImageIcon, withCheckBox: _withCheckBox,   withOutMe: _withOutMe, rootNode: rootNode },
                dataType: "html",
                success: function (data) {

                    var swapSelectedNode = 0;
                    debugger;
                    if (isEdit == 1)
                    {
                        swapSelectedNode = selectedNode;
                        selectedNode = tempPnode;
                    }
                    $(mainClass).html(data).closest('.ch-treeview').parent().SelectNodeWhileEdit(selectedNode, isEdit, swapSelectedNode);

                }
            })

        });
    };


    $.fn.moveUp = function () {
        debugger;
        //e.stopPropagation();
        $('.ch-treeview').each(function () {

            var me = $(this).find('.treeview-selected');

            if (me == null) {
                return;
            }
            var parentClass = $(me).closest("ul").parent().parent();

            var parentNode = $(parentClass).find('.treeview-text:first');// $(this).closest("li");

            var mainClass = $(parentClass).closest('.treeview-area');

            var wrapperClass = $(mainClass).closest('.ch-treeview').parent();

            var id = $(parentNode).attr("nodeid");

            if (id == null) {
                return;
            }

            var pid = $(parentNode).attr("nodepid");
            var text = $(parentNode).attr("nodetext");
            var isGroup = $(parentNode).attr("nodeisgroup");
            var allowSelectGroupNode = $(mainClass).attr("allowselectgroupnode").replace('"', '').replace('"', '');


            if (isGroup == "true") {
                var b = $(parentClass).find('.treeview-icon').attr('class');
                if (b.indexOf('treeview-expand') == 0) {
                    $(parentClass).find('ul:first').toggle();
                    $(parentClass).find('.treeview-icon').toggleClass('treeview-expand');
                }
            }

            $(parentNode).toggleClass('treeview-selected');
            $(me).toggleClass('treeview-selected');

            $(wrapperClass).trigger('nodeClick', [{ nodeId: id, parentNodeId: pid, nodeText: text, isGroup: isGroup, allowSelectGroupNode: allowSelectGroupNode }]);

        })
    };
 
    function SelectNodeNotHierarchy(selectNodeId) {
        debugger;
        $('.explore-details').each(function () {

            var mainClass = $(this).find('.box-body');
            var currentSelected = $(this).find('.box-header').find('#labelId').text();
            
            });
    }
  


        $.fn.SelectNode = function (selectNodeId) {
            debugger;
            //if (selectNodeId == "0") {

            //    $(wrapperClass).trigger('listtypeclick', [{ nodeId: id, parentNodeId: pid, nodeText: text, isGroup: isGroup, allowSelectGroupNode: allowSelectGroupNode }]);

            //}
            //else {

            $('.ch-treeview').each(function () {
                debugger;

                var mainClass = $(this).find('.treeview-area');
                var currentSelected = $(mainClass).find('.treeview-selected');
                var nodeToSelect = $(mainClass).find(".treeview-text[nodeid='" + selectNodeId + "']");
                var parentClass = $(nodeToSelect).closest("ul").parent();

                var wrapperClass = $(mainClass).closest('.ch-treeview').parent();

                var id = $(nodeToSelect).attr("nodeid");
                if (id == null) {
                    id=selectNodeId;
                }
                if (id ==null) {
                    return;
                }
                var pid = $(nodeToSelect).attr("nodepid");
                if (pid == null) {
                    pid= $('.Department-explore').closest('.panel-list').find('input#pDepartmentId').val()
                }
                var text = $(nodeToSelect).attr("nodetext");
                var isGroup = $(nodeToSelect).attr("nodeisgroup");
                var allowSelectGroupNode = $(mainClass).attr("allowselectgroupnode").replace('"', '').replace('"', '');


                if (isGroup == "true") {
                    var b = $(parentClass).find('.treeview-icon').attr('class');
                    if (b.indexOf('treeview-expand') == 0) {
                        $(parentClass).find('ul:first').toggle();
                        $(parentClass).find('.treeview-icon').toggleClass('treeview-expand');
                    }
                }

                $(nodeToSelect).toggleClass('treeview-selected');
                $(currentSelected).toggleClass('treeview-selected');

                $(wrapperClass).trigger('nodeClick', [{ nodeId: id, parentNodeId: pid, nodeText: text, isGroup: isGroup, allowSelectGroupNode: allowSelectGroupNode }]);

            });
            //}

        };

        $.fn.SelectNodeWhileEdit = function (selectNodeId,isEdit,pnode) {
            debugger;
            //if (selectNodeId == "0") {

            //    $(wrapperClass).trigger('listtypeclick', [{ nodeId: id, parentNodeId: pid, nodeText: text, isGroup: isGroup, allowSelectGroupNode: allowSelectGroupNode }]);

            //}
            //else {

            $('.ch-treeview').each(function () {
                debugger;

                var mainClass = $(this).find('.treeview-area');
                var currentSelected = $(mainClass).find('.treeview-selected');
                var nodeToSelect = $(mainClass).find(".treeview-text[nodeid='" + selectNodeId + "']");
                var parentClass = $(nodeToSelect).closest("ul").parent();

                var wrapperClass = $(mainClass).closest('.ch-treeview').parent();

                var id = $(nodeToSelect).attr("nodeid");
                if (id == null) {
                    id = selectNodeId;
                }
                if (id == null) {
                    return;
                }
                var pid = $(nodeToSelect).attr("nodepid");
                if (pid == null) {
                    pid = $('.Department-explore').closest('.panel-list').find('input#pDepartmentId').val()
                }
                var text = $(nodeToSelect).attr("nodetext");
                var isGroup = $(nodeToSelect).attr("nodeisgroup");
                var allowSelectGroupNode = $(mainClass).attr("allowselectgroupnode").replace('"', '').replace('"', '');


                if (isGroup == "true") {
                    var b = $(parentClass).find('.treeview-icon').attr('class');
                    if (b.indexOf('treeview-expand') == 0) {
                        $(parentClass).find('ul:first').toggle();
                        $(parentClass).find('.treeview-icon').toggleClass('treeview-expand');
                    }
                }

                $(nodeToSelect).toggleClass('treeview-selected');
                $(currentSelected).toggleClass('treeview-selected');
                if (isEdit == 1)
                {
                    pid = selectNodeId;
                    
                }
                $(wrapperClass).trigger('nodeClick', [{ nodeId: id, parentNodeId: pid, nodeText: text, isGroup: isGroup, allowSelectGroupNode: allowSelectGroupNode }]);

            });
            //}

        };

    ////code for popup

    $('.treeview-popup').on('nodeClick', function (e, data) {
        debugger;
        e.stopPropagation();
        var test = $(this);
        var parent = $(selectedObject).closest(".section-treeview");
        var allow = data.allowSelect;
        var toSelect = selectedObject;


        if ((data.isGroup == "true" && data.allowSelectGroupNode == "true") || data.isGroup == "false" || data.allowSelectGroupNode == "false" && allow == "true") {
            selectedObject = null;
            $(parent).find('.display-txt').val(data.nodeText);
            $(parent).find('.internal-value').val(data.nodeId);
            $dlg_dpicker.dialog("close");
            $(parent).find('.display-txt').focus();

            var wrapperClass = $(toSelect).closest('.section-treeview');
            $(wrapperClass).trigger('getType', [{ pid: data.nodeId }]);

        }

    });

    $('.ch-treeview').on('change', '.text-filter-tree', function (e) {
        debugger;
        var query = $(this).val();
        
        //if (query != "") {
            //var wrapperClass = $(selectedObject).closest('.section-treeview');

            ////var div = $(wrapperClass).find('.btn-find-tree');
            //var div = $(wrapperClass).find('.btn-treeview-popup');
            var wrapperClass = $(this).closest('.section-treeview').find('.ch-treeview');

            var div = $(wrapperClass).find('.treeview-area');

            var treeViewParam = {
                WithCheckBox: $(div).attr("withcheckbox"),
                AllowSelectGroup: $(div).attr("allowselectgroup"),
                WithImageIcon: $(div).attr("withimageicon"),
                WithOutMe: $(div).attr("excludeme"),
                Title: $(div).attr("poptitle"),
                SelectedNodeId: $(div).closest('.section-treeview').find('.internal-value').val(),
                Filter: query
            }

            $(wrapperClass).trigger('filterTree', [{ fromPopUp: 'true', param: treeViewParam }]);
        //}
        
    });

    $('.ch-treeview').on('click', ".filter-btn", function (e) {
        debugger;
        var query = $(this).closest('.input-group').find("#text-filter-tree").val();

        //if (query != "") {
            var wrapperClass = $(this).closest('.section-treeview').find('.ch-treeview');

            var div = $(wrapperClass).find('.treeview-area');

            var treeViewParam = {
                WithCheckBox: $(div).attr("withcheckbox"),
                AllowSelectGroup: $(div).attr("allowselectgroup"),
                WithImageIcon: $(div).attr("withimageicon"),
                WithOutMe: $(div).attr("excludeme"),
                Title: $(div).attr("poptitle"),
                SelectedNodeId: $(div).closest('.section-treeview').find('.internal-value').val(),
                Filter: query
            }

        //}
            $(wrapperClass).trigger('filterTree', [{ fromPopUp: 'true', param: treeViewParam }]);


    });
});
