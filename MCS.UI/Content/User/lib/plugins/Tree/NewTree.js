$(document).on('click', ".toggle-children-visibility", function (e) {
    if ($(this).find("i:first").hasClass("la la-plus")) {
        var div = $(this).parents(".hierarchy-node").addClass("dir-anchor");

        $(this).find("i:first").removeClass("la la-plus");
        $(this).find("i:first").addClass("la la-minus");

        var toggleitem = $(this).parents(".toggle-item:first");
        var selectedParty = $(this).data('node-id');
        var selectedPartyParent = $(this).data('node-parent-id');
        var getChildrenActionParameters = $(this).data('getchildrenparameters')

        if (getChildrenActionParameters == '' || getChildrenActionParameters == null || getChildrenActionParameters == undefined) {
            getChildrenActionParameters = {}
        }
        else if (getChildrenActionParameters.constructor == Object) {

        }
        else {
            getChildrenActionParameters = JSON.parse(getChildrenActionParameters.replace(/&quot;/g, '"').replace(/&#39;/g, '\''));
        }

        var processedSource = {};
        $.each(getChildrenActionParameters, function (key, val) {
            if (val.charAt(0) == '#') {
                val = val.replace('#', '');
                val = $(val).val();

                processedSource[key] = val;
            }
        });

        processedSource.GetChildrenActionParameters = $(this).data('getchildrenparameters');
        processedSource.GetChildrenActionURL = $(this).data('getchildrenurl');
        processedSource.CallbackFunction = $(this).data('callback-function');
        processedSource.TreeId = $(this).data('modal-container');
        processedSource.SelectedNode = selectedParty;
        processedSource.OrgUnitTreeMode = $(this).data('orgunittreemode');

        $.ajax({
            cache: false,
            type: 'get',
            url: $(this).data('getchildrenurl'),
            data: processedSource,
            success:
                function (data) {
                    $("div[data-node-parent-id='" + selectedParty + "']").remove();
                    $("div[data-node-parent-id='" + selectedPartyParent + "']:not([data-node-id='" + selectedParty + "'])").hide();

                    toggleitem.after(data);
                }
        });
    }
    else {
        var div = $(this).parents("div.hierarchy-node:first").removeClass("dir-anchor");

        var selectedPartyParent = $(this).data('node-parent-id');

        $(this).find("i:first").removeClass("la la-minus");
        $(this).find("i:first").addClass("la la-plus");

        var selectedParty = $(this).data('node-id')
        hideChildren(selectedParty);

        $("div[data-node-parent-id='" + selectedPartyParent + "']").show();
    }

    e.stopPropagation();
});

function hideChildren(parentId) {
    var directChildren = $("div[data-node-parent-id='" + parentId + "']");
    directChildren.hide();

    directChildren.each(function () {
        hideChildren($(this).data('node-id'));
    });
}

$(document).on("click", ".hierarchy-node", function (e) {
    
    var nodeId = $(this).data('node-id');
    var nodeNumber = $(this).data('node-number');
    var nodeName = $(this).data('node-name');
    var nodeIsYesserRegistered = $(this).data('node-isyesserregistered');
    var nodeEmail = $(this).data('node-email');
    var container = $(this).data('modal-container');
    var callbackFunction = $(this).data('callback-function');

    $("#" + container + "_hdnDepartmentId").val(nodeId);
    $("#" + container + "_txtDepartmentNumber").val(nodeNumber);
    $("#" + container + "_txtDepartmentName").val(nodeName);
    $("#" + container + "_hdnIsYesserRegistered").val(nodeIsYesserRegistered);
    $("#" + container + "_txtDepartmentEmail").val(nodeEmail);

    $('#orgHierarchyModal__' + container).modal('hide');

    if (callbackFunction != '' && callbackFunction != undefined) {
        window[callbackFunction](nodeId);
    }

    e.stopPropagation();
});

$(document).on("click", ".ShowHideOrgHierarchyDialog", function (e) {
    e.stopImmediatePropagation();

    var getTreeItemsUrl = $(this).data('getchildrenurl');
    var treeId = $(this).data('modal-container');
    var callBackFunction = $(this).data('onclickfunction');
    var getTreeItemsParameters = $(this).data('getchildrenparameters');
    var selectedNodeId = $("#" + treeId + "_hdnDepartmentId").val();

    if ($('#orgHierarchyModal__' + treeId).length > 0) {
        $('#orgHierarchyModal__' + treeId).remove();
    }

    if (getTreeItemsParameters == '' || getTreeItemsParameters == null || getTreeItemsParameters == undefined) {
        getTreeItemsParameters = {}
    }
    else if (getTreeItemsParameters.constructor == Object) {

    }
    else {
        getTreeItemsParameters = JSON.parse(getTreeItemsParameters.replace(/&quot;/g, '"').replace(/&#39;/g, '\''));
    }

    var processedSource = {};
    $.each(getTreeItemsParameters, function (key, val) {
        if (val.charAt(0) == '#') {
            val = val.replace('#', '');
            val = $(val).val();

            processedSource[key] = val;
        }
    });

    processedSource.GetChildrenActionParameters = JSON.stringify(getTreeItemsParameters);
    processedSource.GetChildrenActionURL = getTreeItemsUrl;
    processedSource.CallbackFunction = callBackFunction;
    processedSource.TreeId = treeId;
    processedSource.OrgUnitTreeMode = $(this).data('orgunittreemode');
    //if (selectedNodeId !== '' && selectedNodeId != undefined) {
    //    processedSource.SelectedNode = selectedNodeId;
    //}
    //else {
        processedSource.SelectedNode = null;
   // }

    $.ajax({
        cache: false,
        type: 'get',
        url: getTreeItemsUrl,
        data: processedSource,
        success:
            function (data) {
                $(data).modal('show');
            }
    });
});

function TreeChangeGetChildrenParameters(treeId, newParameters) {
    $("#" + treeId + "_hrefShowDialog").data('getchildrenparameters', newParameters);
}

function GetSelectedItemText(treeId) {
    return $("#" + treeId + "_txtDepartmentName").val();
}

function GetSelectedItemNumber(treeId) {
    return $("#" + treeId + "_txtDepartmentNumber").val();
}

function GetSelectedItemId(treeId) {
   
    return $("#" + treeId + "_hdnDepartmentId").val();
}

$(document).on("keydown", ".InboundDepNum", function (e) {
    var code = e.keyCode || e.which;
    if (code == '9') {

        var treeId = $(this).data("container");
        var DeptNumber = $("#" + treeId + "_txtDepartmentNumber").val();
        var callbackFunction = $(this).data('func');

        if (DeptNumber) {
            var getNodeInfoByNumActionURL = $(this).data('getnodeinfobynumactionurl');

            $.ajax({
                cache: false,
                type: 'get',
                url: getNodeInfoByNumActionURL,
                data: { partyNumber: DeptNumber },
                success:
                    function (data) {
                        if (data.IsSelectable) {
                            $("#" + treeId + "_hdnDepartmentId").val(data.Id);
                            $("#" + treeId + "_txtDepartmentNumber").val(data.DepartmentNumber);
                            $("#" + treeId + "_txtDepartmentName").val(data.Name);
                            $("#" + treeId + "_hdnIsYesserRegistered").val(data.IsYesserRegistered);
                            $("#" + treeId + "_txtDepartmentEmail").val(data.Email);

                            if (callbackFunction != '' && callbackFunction != undefined) {
                                window[callbackFunction](data.Id);
                            }
                        }
                    },
                error: function (xhr) {
                    ResetTree(treeId);
                }
            });
        }
        else {
            ResetTree(treeId);
        }
    }
});

function ResetTree(treeId) {
    $("#" + treeId + "_hdnDepartmentId").val("");
    $("#" + treeId + "_txtDepartmentNumber").val("");
    $("#" + treeId + "_txtDepartmentName").val("");
    $("#" + treeId + "_hdnIsYesserRegistered").val("");
    $("#" + treeId + "_txtDepartmentEmail").val("");
}

function SetTreeDefaultInfo(treeId, nodeId) {
     
    
    if (nodeId) {
        $("#" + treeId + "_hdnDepartmentId").val(nodeId);
    }

    var defaultId = $("#" + treeId + "_hdnDepartmentId").val();

    if (defaultId > 0) {
        
        var getNodeInfoByIdActionURL = $("#" + treeId + "_hrefShowDialog").data('getnodeinfobyidactionurl');

        $.ajax({
            cache: false,
            type: 'get',
            url: getNodeInfoByIdActionURL,
            data: { partyId: defaultId },
            success:
                function (data) {
                    $("#" + treeId + "_hdnDepartmentId").val(data.Id);
                    $("#" + treeId + "_txtDepartmentNumber").val(data.DepartmentNumber);
                    $("#" + treeId + "_txtDepartmentName").val(data.Name);
                    $("#" + treeId + "_hdnIsYesserRegistered").val(data.IsYesserRegistered);
                    $("#" + treeId + "_txtDepartmentEmail").val(data.Email);
                },
            error: function (xhr) {

            }
        });
    }
}

function SetTreeSelectedItem(treeId, nodeId, nodeNumber, nodeName) {
    $("#" + treeId + "_hdnDepartmentId").val(nodeId);
    $("#" + treeId + "_txtDepartmentNumber").val(nodeNumber);
    $("#" + treeId + "_txtDepartmentName").val(nodeName);
}