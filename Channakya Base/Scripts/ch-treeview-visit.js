$(document).ready(function () {

    $('.treeview-area').on('click', '.treeview-icon', function (e) {
        
        e.stopPropagation();
        var parentClass = $(this).closest("li");
        $(parentClass).find('ul:first').toggle();
        $(this).toggleClass("treeview-expand");
    });

    $('.treeview-area').on('click', '.treeview-image', function (e) {
      
        e.stopPropagation();
        var parentClass = $(this).closest("li");
        $(parentClass).find('ul:first').toggle();
        $(parentClass).find('.treeview-icon').toggleClass('treeview-expand');
    });

    $('.location-container').on('click', '.treeview-text', function (e) {
        debugger;
       // e.stopPropagation();
        e.stopImmediatePropagation();
        var parentClass = $(this).closest("li");

        var mainClass = $(this).closest('.treeview-area');

        var wrapperClass = $(mainClass).parent();


        var id = $(this).attr("nodeid");
        var pid = $(this).attr("nodepid");
        var text = $(this).attr("nodetext");
        var isGroup = $(this).attr("nodeisgroup");
        var allowSelectGroupNode = $(mainClass).attr("allowselectgroupnode")

        if (isGroup == "true") {
            var b = $(parentClass).find('.treeview-icon').attr('class');
            if (b.indexOf('treeview-expand') == 0) {
                $(parentClass).find('ul:first').toggle();
                $(parentClass).find('.treeview-icon').toggleClass('treeview-expand');
            }
        }
        $(mainClass).find('.treeview-selected').toggleClass('treeview-selected');
        $(this).toggleClass('treeview-selected');

        $(wrapperClass).trigger('nodeClick', [{ nodeId: id, parentNodeId: pid, noteText: text, isGroup: isGroup, allowSelectGroupNode: allowSelectGroupNode }]);

    });


    $('.treeview-area').on('change', '.treeview-checkbox', function (e) {
        
        var thisli = $(this).closest("li").find("ul").find(".treeview-checkbox").not(this).prop('checked', this.checked);
    });

    $('.treeview-button').on('click', function () {
        
        $(this).parent().parent().find('.treeview-area').find('li').slideToggle();
    });

    $.fn.selectNode = function (selectedNode) {
        $('.treeview-area').each(function () {
            $(this).find('.treeview-selected').toggleClass('treeview-selected');
            var div = $(this).find('.treeview-text').filter(function () {
                return $(this).attr('nodeid') == selectedNode;
            });
            $(div).toggleClass('treeview-selected');
            $(div).trigger('click');
        });
    };

    $.fn.updateTreeview = function (controller, action, mainDiv, withCheckBox, withImageIcon, selectedNode, allowSelectGroupNode, withOutMe, rootNode) {
        $(mainDiv).each(function () {
            debugger;
            var container = $(this).parent();
            var url = '/' + controller + '/' + action;
            $.ajax({
                url: url,
                traditional: true,
                data: { withCheckBox: withCheckBox, withImageIcon: withImageIcon, selectedNode: selectedNode, allowSelectGroupNode: allowSelectGroupNode, withOutMe: withOutMe, rootNode: rootNode },
                dataType: "html"
            }).success(function (data) {
                $(container).html(data);
            });
        });
    };  
});


