
var deleteLinkEvent = document.createEvent('Event');
deleteLinkEvent.initEvent('deleteLinkEvent', true, true);

var deleteDepartmentEvent = document.createEvent('Event');
deleteDepartmentEvent.initEvent('deleteDepartmentEvent', true, true);

var noDepartmentSelectionEvent = document.createEvent('Event');
noDepartmentSelectionEvent.initEvent('noDepartmentSelectionEvent', true, true);

var changeOrgUnitViewEvent = document.createEvent('Event');
changeOrgUnitViewEvent.initEvent('changeOrgUnitViewEvent', true, true);

var saveOrgStructureEvent = document.createEvent('Event');
saveOrgStructureEvent.initEvent('saveOrgStructureEvent', true, true);

var createDepartmentEvent = document.createEvent('Event');
createDepartmentEvent.initEvent('createDepartmentEvent', true, true);

var removeSelectedDep = true;

//Counter for the departments key.
var countId = 1;
var countIdChange = 1; //Used when the dialog has an old departments.

var zoom = 100;

//End points options.
var targetDropOptions = {
    activeClass: 'dragActive'
};

var radius;
if (/Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent)) {

    radius = 10;
}
else {

    radius = 5.5;
}
var strokeStyle = '#F9F9F9';
var fillStyle = '#D5D5D5';

var topEndpoint = {
    anchor: 'TopCenter',
    endpoint: ['Dot', { radius: radius }],
    paintStyle: { fillStyle: fillStyle, strokeStyle: strokeStyle, lineWidth: 1 },
    isSource: true,
    scope: 'green dot',
    connectorStyle: { strokeStyle: fillStyle, lineWidth: 4 },
    connector: ['Flowchart'],
    maxConnections: -1,
    isTarget: true,
    dropOptions: targetDropOptions,
    connectorOverlays: [
        ['Arrow', { width: 10, length: 25, location: 1, id: 'arrow' }],
    ]
};
var bottomEndpoint = {
    anchor: 'BottomCenter',
    endpoint: ['Dot', { radius: radius }],
    paintStyle: { fillStyle: fillStyle, strokeStyle: strokeStyle, lineWidth: 1 },
    isSource: true,
    scope: 'green dot',
    connectorStyle: { strokeStyle: fillStyle, lineWidth: 4 },
    connector: ['Flowchart'],
    maxConnections: -1,
    isTarget: true,
    dropOptions: targetDropOptions,
    connectorOverlays: [
        ['Arrow', { width: 10, length: 25, location: 1, id: 'arrow' }],
    ]
};
var leftEndpoint = {
    anchor: 'LeftMiddle',
    endpoint: ['Dot', { radius: radius }],
    paintStyle: { fillStyle: fillStyle, strokeStyle: strokeStyle, lineWidth: 1 },
    isSource: true,
    scope: 'green dot',
    connectorStyle: { strokeStyle: fillStyle, lineWidth: 4 },
    connector: ['Flowchart'],
    maxConnections: -1,
    isTarget: true,
    dropOptions: targetDropOptions,
    connectorOverlays: [
        ['Arrow', { width: 10, length: 25, location: 1, id: 'arrow' }],
    ]
};
var rightEndpoint = {
    anchor: 'RightMiddle',
    endpoint: ['Dot', { radius: radius }],
    paintStyle: { fillStyle: fillStyle, strokeStyle: strokeStyle, lineWidth: 1 },
    isSource: true,
    scope: 'green dot',
    connectorStyle: { strokeStyle: fillStyle, lineWidth: 4 },
    connector: ['Flowchart'],
    maxConnections: -1,
    isTarget: true,
    dropOptions: targetDropOptions,
    connectorOverlays: [
        ['Arrow', { width: 10, length: 25, location: 1, id: 'arrow' }],
    ]
};

//Resize the control on touch devices.
function Repaint() {

    $('#dvOrgStructure').resize(function () {

        jsPlumb.repaintEverything(); //To recompute the location of the endpoints on that element after resize the element.
    });
}

//Edit on departments Checkboxes (add parent to the department, change the parent of department, delete department, create deparment on Initialize the control).
function LimitationBehavior(hdnIdToSaveDepartmentsData, hdnIdToSaveSettings, parentId, elementIndex, limitation, languageShortName) {

    if (typeof parentId != 'undefined')
        $('#' + parentId.toString() + ' tbody' + ' tr').remove();

    var departmentArray = $.parseJSON($('#' + hdnIdToSaveDepartmentsData).val());
    var settingArray = $.parseJSON($('#' + hdnIdToSaveSettings).val());
    var maxChildren = 0; //Number of children of parent department.

    for (var i = 0; i < departmentArray.length; i++) {

        if (!departmentArray[i].IsDeleted) {

            if (departmentArray[i].ParentId == parentId) {

                var iSettingsIndex = ElementIndex(settingArray, departmentArray[i].Key);

                if (settingArray[iSettingsIndex].children != null && settingArray[iSettingsIndex].children != "") {

                    maxChildren++;
                }
            }
        }
    }

    if (elementIndex != null) {

        var settingsIndex = ElementIndex(settingArray, departmentArray[elementIndex].Key);

        maxChildren = maxChildren + 1;
        settingArray[settingsIndex].children = maxChildren; //Add children record.        
        $('#' + hdnIdToSaveDepartmentsData).val(JSON.stringify(departmentArray)); //Save the changes on the hidden.
        $('#' + hdnIdToSaveSettings).val(JSON.stringify(settingArray)); //Save the changes on the hidden.
    }

    if (maxChildren > limitation) { //Show checkboxes.

        if (Math.ceil(maxChildren / limitation) >= 2) {

            if ($('#cb' + parentId.toString() + 'Level' + '1').length == 0) { //First checkbox will be added with the second one.

                $('#' + parentId.toString() + ' tbody').append('<tr><td><div class="checkbox"><label><input type="checkbox" checked="checked" id="cb' + parentId.toString() + 'Level' + '1' + '" name="' + '1' + '" value="' + '1' + '"/>' + '<label class="checkbox-margin-left" id="txt' + '1' + '">' + '1' + '</label>' + '</label></div></td></tr>');
                jsPlumb.repaintEverything();

                $(document).on("change", '#cb' + parentId.toString() + 'Level' + '1', function () {

                    var checked;

                    if ($(this).is(':checked')) {

                        checked = true;
                    }

                    else {

                        checked = false;
                    }

                    ManagementDepartmentsShowHide(parentId, hdnIdToSaveDepartmentsData, hdnIdToSaveSettings, '1', limitation, languageShortName, checked); //On this checkbox checked show the first group of children and hide the others.
                });
            }
        }

        for (var i = 2; i <= Math.ceil(maxChildren / limitation); i++) { //Add the others checkboxes.

            if ($('#cb' + parentId.toString() + 'Level' + i.toString()).length == 0) { //The checkbox is not exist.

                $('#' + parentId.toString() + ' tbody').append('<tr><td><div class="checkbox"><label><input type="checkbox" checked="checked" id="cb' + parentId.toString() + 'Level' + i.toString() + '" name="' + i + '" value="' + i + '"/>' + '<label class="checkbox-margin-left" id="txt' + i + '">' + i + '</label>' + '</label></div></td></tr>');

                //jsPlumb.repaintEverything();
            }

            $(document).on("change", '#cb' + parentId.toString() + 'Level' + i.toString(), function () {

                var l = this.id.indexOf('l') + 1;
                var lastChar = this.id.substr(this.id.length - (this.id.length - l)); //Limitation Level
                var checked;

                if ($(this).is(':checked')) {

                    checked = true;
                }

                else {

                    checked = false;
                }

                ManagementDepartmentsShowHide(parentId, hdnIdToSaveDepartmentsData, hdnIdToSaveSettings, lastChar, limitation, languageShortName, checked); //On this checkbox checked show the its group of children and hide the others.
            });
            //$('#cb' + parentId.toString() + 'Level' + i.toString()).change(function () {

            //    var l = this.id.indexOf('l') + 1;
            //    var lastChar = this.id.substr(this.id.length - (this.id.length - l)); //Limitation Level
            //    var checked;

            //    if ($(this).is(':checked')) {

            //        checked = true;
            //    }

            //    else {

            //        checked = false;
            //    }

            //    ManagementDepartmentsShowHide(parentId, hdnIdToSaveDepartmentsData, hdnIdToSaveSettings, lastChar, limitation, languageShortName, checked); //On this checkbox checked show the its group of children and hide the others.
            //});
        }

        jsPlumb.repaintEverything();
    }
}

//Detect the departments that will be shown or that will be hidden depends on department's Limitation Behavior
function ManagementDepartmentsShowHide(elementId, hdnIdToSaveDepartmentsData, hdnIdToSaveSettings, limitationLevel, limitation, languageShortName, checked) {

    //var elementIndex;
    var departmentArray = $.parseJSON($('#' + hdnIdToSaveDepartmentsData).val());
    var settingArray = $.parseJSON($('#' + hdnIdToSaveSettings).val());

    for (var i = 0; i < departmentArray.length; i++) {

        if (!departmentArray[i].IsDeleted) {

            if (elementId == departmentArray[i].ParentId) {

                var iSettingsIndex = ElementIndex(settingArray, departmentArray[i].Key);

                if (limitationLevel == Math.ceil(settingArray[iSettingsIndex].children / limitation).toString()) {

                    if (checked == true) {

                        ShowDepartment(departmentArray[i].Key, hdnIdToSaveDepartmentsData, hdnIdToSaveSettings, languageShortName, limitation);
                    }

                    else if (checked == false) {

                        HideDepartment(departmentArray[i].Key, hdnIdToSaveDepartmentsData, hdnIdToSaveSettings);
                    }
                }
            }
        }
    }
}

//Hide department and its child departments. 
function HideDepartment(elementId, hdnIdToSaveDepartmentsData, hdnIdToSaveSettings) {

    jsPlumb.hide(elementId.toString(), true); //Hide all links and endpoints of the current department.
    $('#' + elementId.toString()).hide(); //Hide the current department.

    //var elementIndex;
    var departmentArray = $.parseJSON($('#' + hdnIdToSaveDepartmentsData).val());
    //var elementIndex = ElementIndex(departmentArray, departmentKey);

    //Check if the department has childrin.
    for (var i = 0; i < departmentArray.length; i++) {

        if (!departmentArray[i].IsDeleted) {

            if (elementId == departmentArray[i].ParentId) {

                HideDepartment(departmentArray[i].Key, hdnIdToSaveDepartmentsData, hdnIdToSaveSettings);
            }
        }
    }
}

//Show department and its child departments. 
function ShowDepartment(elementId, hdnIdToSaveDepartmentsData, hdnIdToSaveSettings, languageShortName, limitation) {

    //var elementIndex;
    var departmentArray = $.parseJSON($('#' + hdnIdToSaveDepartmentsData).val());
    var elementIndex = ElementIndex(departmentArray, elementId);

    $('#' + departmentArray[elementIndex].Key).show(); //Show the current department.
    jsPlumb.show(departmentArray[elementIndex].Key.toString(), true); //Show all links and endpoints of the current department.

    LimitationBehavior(hdnIdToSaveDepartmentsData, hdnIdToSaveSettings, departmentArray[elementIndex].Key, null, limitation, languageShortName);

    //Check if the department has childrin.
    for (var i = 0; i < departmentArray.length; i++) {

        if (!departmentArray[i].IsDeleted) {

            if (elementId == departmentArray[i].ParentId) {

                ShowDepartment(departmentArray[i].Key.toString(), hdnIdToSaveDepartmentsData, hdnIdToSaveSettings, languageShortName, limitation);
            }
        }
    }
}

//Delete department and its child departments. 
function DeleteDepartment(elementId, hdnIdToSaveDepartmentsData, hdnIdToSaveSettings, limitation, languageShortName) {

    var isNew;
    var departmentArray = $.parseJSON($('#' + hdnIdToSaveDepartmentsData).val());
    var settingArray = $.parseJSON($('#' + hdnIdToSaveSettings).val());

    //Find the index of selected department and delete it from the department array.
    for (var i = 0; i < departmentArray.length; i++) {

        if (elementId == departmentArray[i].Key.toString()) {

            isNew = departmentArray[i].IsNew;

            for (var y = 0; y < departmentArray.length; y++) {

                if (!departmentArray[y].IsDeleted) {

                    if (departmentArray[y].ParentId.toString() == departmentArray[i].ParentId.toString()) {

                        var ySettings = ElementIndex(settingArray, departmentArray[y].Key);
                        var iSettingsIndex = ElementIndex(settingArray, departmentArray[i].Key);

                        if (settingArray[ySettings].children > settingArray[iSettingsIndex].children) {

                            settingArray[ySettings].children = settingArray[ySettings].children - 1;
                        }
                    }
                }
            }

            var parent;

            if (departmentArray[i].ParentId != 0) {//test

                $('#' + departmentArray[i].ParentId.toString() + ' tbody').html('');
                parent = departmentArray[i].ParentId.toString();
            }

            if (isNew) {

                settingArray.splice(ElementIndex(settingArray, departmentArray[i].Key), 1);
                departmentArray.splice(i, 1);
            }

            else
                departmentArray[i].IsDeleted = true;

            jsPlumb.repaintEverything(); //To recompute the location of the endpoints on that element after resize the element.
            $('#' + hdnIdToSaveDepartmentsData).val(JSON.stringify(departmentArray)); //Save the changes on DepartmentsData hidden.
            $('#' + hdnIdToSaveSettings).val(JSON.stringify(settingArray)); //Save the changes on the hidden.
            LimitationBehavior(hdnIdToSaveDepartmentsData, hdnIdToSaveSettings, parent, null, limitation, languageShortName);
        }
    }

    for (var i = 0; i < settingArray.length; i++) {

        for (var y = 0; y < settingArray[i].LinkUnitsKeys.length; y++) {

            if (settingArray[i].LinkUnitsKeys[y].Key.toString() == elementId) {

                if (isNew)
                    settingArray[i].LinkUnitsKeys[y] = '';
            }
        }
        settingArray[i].LinkUnitsKeys = CleanArray(settingArray[i].LinkUnitsKeys);
    }

    for (var i = 0; i < departmentArray.length; i++) {

        for (var y = 0; y < departmentArray[i].LinkUnitsKeys.length; y++) {

            if (departmentArray[i].LinkUnitsKeys[y].toString() == elementId) {

                if (isNew)
                    departmentArray[i].LinkUnitsKeys[y] = '';
            }
        }
        departmentArray[i].LinkUnitsKeys = CleanArray(departmentArray[i].LinkUnitsKeys);
    }

    jsPlumb.remove(elementId); //remove the selected element from the control and all links and endpoints associate with it.

    $('#' + hdnIdToSaveDepartmentsData).val(JSON.stringify(departmentArray)); //Save the changes on DepartmentsData hidden.
    $('#' + hdnIdToSaveSettings).val(JSON.stringify(settingArray));

    //Check if the department has childrin.
    for (var i = 0; i < departmentArray.length; i++) {

        if (departmentArray[i].ParentId == elementId && departmentArray[i].IsDeleted != true) {

            DeleteDepartment(departmentArray[i].Key.toString(), hdnIdToSaveDepartmentsData, hdnIdToSaveSettings, limitation, languageShortName); //Delete the child department.
        }
    }
}

//Used to remove array items
function CleanArray(array) {

    var newArray = new Array();

    for (var i = 0; i < array.length; i++) {
        if (array[i]) {
            newArray.push(array[i]);
        }
    }
    return newArray;
}

//Get department index by key
function ElementIndex(array, key) {
    var elementIndex;
    if (array == null) {
        array = [];
    }
    for (var i = 0; i < array.length; i++) {
        if (array[i].Key.toString() == key) {
            elementIndex = i;
        }
    }
    return elementIndex;
}

//Update element location with each mouse move or (touch move on toutch devices).
function SaveLocation(departmentKey, hdnIdToSaveDepartmentsData, hdnIdToSaveSettings) {

    $('#' + departmentKey).mousemove(function (event) {

        jsPlumb.repaintEverything(); //To recompute the location of the endpoints on that element after resize the element.

        var settingArray = $.parseJSON($('#' + hdnIdToSaveSettings).val());
        var elementIndex = ElementIndex(settingArray, departmentKey);
        var settingsIndex = ElementIndex(settingArray, departmentKey);

        var offset = $('#' + settingArray[settingsIndex].Key).offset();

        settingArray[settingsIndex].loc[0] = parseInt($('#' + settingArray[settingsIndex].Key)[0].style.left, 10);
        settingArray[settingsIndex].loc[1] = parseInt($('#' + settingArray[settingsIndex].Key)[0].style.top, 10);

        $('#' + hdnIdToSaveSettings).val(JSON.stringify(settingArray)); //Save the changes on the hidden.

        jsPlumb.repaintEverything();
    });

    $('#' + departmentKey).on('touchmove', function (e) {

        var settingArray = $.parseJSON($('#' + hdnIdToSaveSettings).val());
        var elementIndex = ElementIndex(settingArray, departmentKey);
        var settingsIndex = ElementIndex(settingArray, departmentKey);

        jsPlumb.repaintEverything();

        settingArray[settingsIndex].loc[0] = $('#' + settingArray[settingsIndex].Key).position().left;
        settingArray[settingsIndex].loc[1] = $('#' + settingArray[settingsIndex].Key).position().top;

        $('#' + hdnIdToSaveSettings).val(JSON.stringify(settingArray)); //Save the changes on the hidden.
    });
}

//Trigger delete department event
function ManagementDeleteDepartment(hdnIdToSaveSelectedDepartment) {
    var elementId = $('#' + hdnIdToSaveSelectedDepartment).val(); //Department key
    if (document.getElementById(elementId)) {
        document.getElementById(elementId).dispatchEvent(deleteDepartmentEvent);
    }
}

//Create contextmenu for department.
function DepartmentContextmenu(departmentKey, hdnIdToSaveSelectedDepartment, languageShortName, listOfActions) {

    $('#' + departmentKey).bind("contextmenu", function (e) {

        $('.administration').removeClass('select');
        $('#' + departmentKey).addClass('select');
        $('#' + hdnIdToSaveSelectedDepartment).val(departmentKey);

        return false;
    });

    var items = [
        {
            //Delete the department and all links and endpoints associate with that department.
            label: _language[languageShortName].Delete, //From language file depending on languageShortName.
            action: function (element) {

                if ($('#' + hdnIdToSaveSelectedDepartment).val() != '') {

                    ManagementDeleteDepartment(hdnIdToSaveSelectedDepartment)
                }
                else {

                    document.dispatchEvent(noDepartmentSelectionEvent);
                }
            }
        }
    ];

    for (var i = 0; i < listOfActions.length; i++) {

        items.push({

            label: listOfActions[i].value,

            actionName: listOfActions[i].label,

            action: function (element) {

                if ($('#' + hdnIdToSaveSelectedDepartment).val() != '') {

                    window[this.actionName.toString()]();
                }
                else {

                    document.dispatchEvent(noDepartmentSelectionEvent);
                }
            }
        });
    }

    $('#' + departmentKey).contextPopup({

        items: items
    });
}

//Draw link between two departments 
function DrawLink(hdnIdToSaveDepartmentsData, hdnIdToSaveSettings, fromKey, toKey) {

    var uuidFrom = '';
    var uuidTo = '';

    var addLink = true;

    var departmentArray = $.parseJSON($('#' + hdnIdToSaveDepartmentsData).val());

    var indexFrom = ElementIndex(departmentArray, fromKey);

    for (var i = 0; i < departmentArray[indexFrom].LinkUnitsKeys.length; i++) {

        if (departmentArray[indexFrom].LinkUnitsKeys[i] == toKey)
            addLink = false;
    }

    if (addLink) {

        var settingArray = $.parseJSON($('#' + hdnIdToSaveSettings).val());

        var settingIndexFrom = ElementIndex(settingArray, fromKey);

        var indexTo = ElementIndex(departmentArray, toKey);
        var settingIndexTo = ElementIndex(settingArray, toKey);

        var fromLeftLoc = settingArray[settingIndexFrom].loc[0];
        var fromTopLoc = settingArray[settingIndexFrom].loc[1];

        var toLeftLoc = settingArray[settingIndexTo].loc[0];
        var toTopLoc = settingArray[settingIndexTo].loc[1];

        var fromPos = 'left';
        var toPos = 'right';

        //Select the fit endpoints to draw the link by position
        if (Math.abs(fromLeftLoc - toLeftLoc) > Math.abs(fromTopLoc - toTopLoc)) {

            if (fromLeftLoc < toLeftLoc) {

                fromPos = 'right';
                toPos = 'left';
            }
        }
        else {

            fromPos = 'top';
            toPos = 'bottom';

            if (fromTopLoc < toTopLoc) {

                fromPos = 'bottom';
                toPos = 'top';
            }
        }

        if (uuidFrom == '') {

            uuidFrom = fromPos + fromKey;
            uuidTo = toPos + toKey;
        }

        departmentArray[indexFrom].LinkUnitsKeys.push(toKey);
        settingArray[settingIndexFrom].LinkUnitsKeys.push({ Key: toKey, EndPointFrom: uuidFrom, EndPointTo: uuidTo });

        $('#' + hdnIdToSaveDepartmentsData).val(JSON.stringify(departmentArray));
        $('#' + hdnIdToSaveSettings).val(JSON.stringify(settingArray));

        if ($('#' + fromKey).length != 0 && $('#' + toKey).length != 0)
            jsPlumb.connect({ uuids: [uuidFrom, uuidTo] });
    }
}

//Delete link between two departments 
function DeleteLink(hdnIdToSaveDepartmentsData, hdnIdToSaveSettings, fromKey, toKey) {

    var departmentArray = $.parseJSON($('#' + hdnIdToSaveDepartmentsData).val());
    var settingArray = $.parseJSON($('#' + hdnIdToSaveSettings).val());

    var listOfConnection = jsPlumb.getAllConnections();

    for (var i = 0; i < listOfConnection.length; i++) {

        if ((listOfConnection[i].targetId == toKey && listOfConnection[i].sourceId == fromKey)) {

            var confirmDeleteReverseLink = document.getElementById(listOfConnection[i].id).dispatchEvent(deleteLinkEvent);

            if (confirmDeleteReverseLink) {

                for (var y = 0; y < settingArray.length; y++) {

                    for (var z = 0; z < settingArray[y].LinkUnitsKeys.length; z++) {

                        if (toKey == settingArray[y].LinkUnitsKeys[z].Key && fromKey == settingArray[y].Key) {

                            settingArray[y].LinkUnitsKeys[z] = '';
                        }
                    }
                    settingArray[y].LinkUnitsKeys = CleanArray(settingArray[y].LinkUnitsKeys);
                }

                for (var y = 0; y < departmentArray.length; y++) {

                    for (var z = 0; z < departmentArray[y].LinkUnitsKeys.length; z++) {

                        if (toKey == departmentArray[y].LinkUnitsKeys[z] && fromKey == departmentArray[y].Key) {

                            departmentArray[y].LinkUnitsKeys[z] = '';
                        }
                    }
                    departmentArray[y].LinkUnitsKeys = CleanArray(departmentArray[y].LinkUnitsKeys);
                }

                if ($('#' + fromKey).length != 0 && $('#' + toKey).length != 0)
                    jsPlumb.detach(listOfConnection[i]); //remove the reverse selected element from the control. 
            }
        }
    }

    $('#' + hdnIdToSaveDepartmentsData).val(JSON.stringify(departmentArray));
    $('#' + hdnIdToSaveSettings).val(JSON.stringify(settingArray));

    return confirmDeleteReverseLink;
}

//Create contextmenu for link.
function LinkContextmenu(linkId, hdnIdToSaveDepartmentsData, hdnIdToSaveSettings, languageShortName) {

    $('#' + linkId).contextPopupLink({

        items: [
            {   //Delete selected link (onedirectional or bidirectional).
                label: _language[languageShortName].Delete,//From language file depending on languageShortName.
                action: function (element) {

                    var departmentArray = $.parseJSON($('#' + hdnIdToSaveDepartmentsData).val());
                    var settingArray = $.parseJSON($('#' + hdnIdToSaveSettings).val());
                    var conn = element.currentTarget._jsPlumb; //The link object.

                    var confirmDeleteLink = document.getElementById(conn.id).dispatchEvent(deleteLinkEvent);

                    if (confirmDeleteLink) {

                        //Find the index of selected link and delete it from the department array.
                        for (var i = 0; i < departmentArray.length; i++) {

                            for (var y = 0; y < departmentArray[i].LinkUnitsKeys.length; y++) {

                                if (conn.sourceId == departmentArray[i].Key && conn.targetId == departmentArray[i].LinkUnitsKeys[y]) {

                                    departmentArray[i].LinkUnitsKeys[y] = '';
                                }
                            }
                            departmentArray[i].LinkUnitsKeys = CleanArray(departmentArray[i].LinkUnitsKeys);
                        }

                        for (var i = 0; i < settingArray.length; i++) {

                            for (var y = 0; y < settingArray[i].LinkUnitsKeys.length; y++) {

                                if (conn.sourceId == settingArray[i].Key && conn.targetId == settingArray[i].LinkUnitsKeys[y].Key) {

                                    settingArray[i].LinkUnitsKeys[y] = '';
                                }
                            }
                            settingArray[i].LinkUnitsKeys = CleanArray(settingArray[i].LinkUnitsKeys);
                        }
                    }

                    var listOfConnection = jsPlumb.getAllConnections();

                    for (var i = 0; i < listOfConnection.length; i++) {

                        if ((listOfConnection[i].endpoints[0]._jsPlumb.uuid == conn.endpoints[1]._jsPlumb.uuid && listOfConnection[i].endpoints[1]._jsPlumb.uuid == conn.endpoints[0]._jsPlumb.uuid)) {

                            var confirmDeleteReverseLink = document.getElementById(listOfConnection[i].id).dispatchEvent(deleteLinkEvent);

                            if (confirmDeleteReverseLink) {

                                for (var y = 0; y < settingArray.length; y++) {

                                    for (var z = 0; z < settingArray[y].LinkUnitsKeys.length; z++) {

                                        if (conn.sourceId == settingArray[y].LinkUnitsKeys[z].Key && conn.targetId == settingArray[y].Key) {

                                            settingArray[y].LinkUnitsKeys[z] = '';
                                        }
                                    }
                                    settingArray[y].LinkUnitsKeys = CleanArray(settingArray[y].LinkUnitsKeys);
                                }

                                for (var y = 0; y < departmentArray.length; y++) {

                                    for (var z = 0; z < departmentArray[y].LinkUnitsKeys.length; z++) {

                                        if (conn.sourceId == departmentArray[y].LinkUnitsKeys[z] && conn.targetId == departmentArray[y].Key) {

                                            departmentArray[y].LinkUnitsKeys[z] = '';
                                        }
                                    }
                                    departmentArray[y].LinkUnitsKeys = CleanArray(departmentArray[y].LinkUnitsKeys);
                                }
                                jsPlumb.detach(listOfConnection[i]); //remove the reverse selected element from the control. 
                            }
                        }
                    }

                    if (confirmDeleteLink)
                        jsPlumb.detach(conn); //remove the selected element from the control. 

                    $('#' + hdnIdToSaveDepartmentsData).val(JSON.stringify(departmentArray));
                    $('#' + hdnIdToSaveSettings).val(JSON.stringify(settingArray));
                }
            },

            {   //Make the onedirectional selected link bidirectional(create another link with opposite direction).
                label: _language[languageShortName].Bidirectional,//From language file depending on languageShortName.
                action: function (element) {

                    var conn = element.currentTarget._jsPlumb //The link object.
                    var addBidirectionalArrow = true; //To check if we can add opposite link or not.

                    $.each(jsPlumb.getAllConnections(), function (idx, c) {

                        if (c.sourceId == conn.targetId && c.targetId == conn.sourceId) { //If the link already bidirectional.

                            addBidirectionalArrow = false;
                        }
                    });

                    if (addBidirectionalArrow) {

                        jsPlumb.connect({ uuids: [conn.endpoints[1]._jsPlumb.uuid, conn.endpoints[0]._jsPlumb.uuid] }); //Create new link.

                        var departmentArray = $.parseJSON($('#' + hdnIdToSaveDepartmentsData).val());
                        var settingArray = $.parseJSON($('#' + hdnIdToSaveSettings).val());

                        var index = ElementIndex(departmentArray, conn.targetId);
                        var settingIndex = ElementIndex(settingArray, conn.targetId);

                        departmentArray[index].LinkUnitsKeys.push(conn.sourceId);
                        settingArray[settingIndex].LinkUnitsKeys.push({ Key: conn.sourceId, EndPointFrom: conn.endpoints[1]._jsPlumb.uuid, EndPointTo: conn.endpoints[0]._jsPlumb.uuid });

                        $('#' + hdnIdToSaveDepartmentsData).val(JSON.stringify(departmentArray));
                        $('#' + hdnIdToSaveSettings).val(JSON.stringify(settingArray));
                    }
                }
            }
        ]
    });
}

//Create new department and return the new id.
function CreateNewDepartment(hdnIdToSaveDepartmentsData, hdnIdToSaveSettings) {
    var departmentArray = $.parseJSON($('#' + hdnIdToSaveDepartmentsData).val());
    var settingArray = $.parseJSON($('#' + hdnIdToSaveSettings).val());

    var count = 1; //The default text, arabic name, and english name of the created department.
    for (var i = 0; i < departmentArray.length; i++) {

        var c = parseInt(departmentArray[i].Name);

        if (/^(\-|\+)?([0-9]+|Infinity)$/.test(departmentArray[i].Name)) {

            if (c >= count) {

                count = c + 1;
            }
        }
    }

    var id = countId;

    departmentArray.push({ "Key": id, "Name": count.toString(), "Names": [], "Number": 0, "BarCode": "", "IsVirtualUnit": false, "TransactionsProcessingPeriod": 0, "ParentId": 0, "IdentifierId": 0, "Users": [], "Counter": null, "AssignmentPaper": null, "LinkUnitsKeys": [], "IsDeleted": false, "IsNew": true, "BarcodeDesigners": [] }); //Add default data for the new department.
    settingArray.push({ "Key": id, "loc": [0, 0], "children": "", "LinkUnitsKeys": [] }); //Add default sttings data for the new department.

    $('#' + hdnIdToSaveDepartmentsData).val(JSON.stringify(departmentArray)); //Save the changes on the hidden.
    $('#' + hdnIdToSaveSettings).val(JSON.stringify(settingArray)); //Save the changes on the hidden.

    countId++;

    return [id, count];
}

//Create new department on drop the toolbox department.
function CreateDepartment(hdnIdToSaveDepartmentsData, hdnIdToSaveSettings, hdnIdToSaveSelectedDepartment, languageShortName, listOfActions, leftPosition, topPosition, limitation) {
    
    var newDepartment = CreateNewDepartment(hdnIdToSaveDepartmentsData, hdnIdToSaveSettings);

    var id = newDepartment[0];
    var count = newDepartment[1];

    $('#dvOrgStructure').append('<div class="administration" style="left: 617px; top: 219px; display: block; z-index: 100;" id="' + id + '"><div>' + count + '</div><div class="level_of">' + _language[languageShortName].NoParent + '</div><div class="panel-body faq faq_stage"><div class="faq-item"><div class="faq-title"><span class="fa fa-angle-down"></span>' + _language[languageShortName].Groups + '</div><div class="faq-text"><div class="list_group"><div class="table-responsive"><table class="tbl-responsive table table-striped"><tbody></tbody></table></div></div></div></div></div></div>'); //Create div(department).

    SaveLocation(id, hdnIdToSaveDepartmentsData, hdnIdToSaveSettings); //To update the department location.
    DepartmentContextmenu(id, hdnIdToSaveSelectedDepartment, languageShortName, listOfActions); //Create contextmenu for this department.

    //Create the four endpoint.
    jsPlumb.addEndpoint($('#' + id), { uuid: 'top' + id }, topEndpoint);
    jsPlumb.addEndpoint($('#' + id), { uuid: 'bottom' + id }, bottomEndpoint);
    jsPlumb.addEndpoint($('#' + id), { uuid: 'left' + id }, leftEndpoint);
    jsPlumb.addEndpoint($('#' + id), { uuid: 'right' + id }, rightEndpoint);

    jsPlumb.draggable($('#' + id)); //Make the department draggable.

    $('#' + id).position({ //Put the created department on its location.

        of: $('.OrgStructure'),
        my: 'left top',
        at: 'left+' + leftPosition + ' ' + 'top+' + topPosition,
    });

    $('#' + id).click(function (e) {

        e.stopPropagation();
        $('.administration').removeClass('select');
        $('#' + id).addClass('select');
        $('#' + hdnIdToSaveSelectedDepartment).val(id);
    });

    $('#' + id).mousedown(function (e) {

        if (e.target.className == "fa fa-angle-down") {

            $('#' + this.id + ' .fa').removeClass('fa-angle-down');
            $('#' + this.id + ' .fa').addClass('fa-angle-up');
            $('#' + this.id + ' .faq-item').addClass('active');

            return
        }

        if (e.target.className == "fa fa-angle-up") {

            $('#' + this.id + ' .fa').removeClass('fa-angle-up');
            $('#' + this.id + ' .fa').addClass('fa-angle-down');
            $('#' + this.id + ' .faq-item').removeClass('active');

            return
        }
    });

    $('#' + id).mouseup(function (e) {

        jsPlumb.repaintEverything();
    });

    $('.select').removeClass('select');
    $('#' + id).addClass('select');
    $('#' + hdnIdToSaveSelectedDepartment).val(id);
    removeSelectedDep = false;

    jsPlumb.repaintEverything(); //To recompute the location of the endpoints on that element after resize the element.

    document.getElementById(id).dispatchEvent(createDepartmentEvent);
}

//Zoom in orgStructure view.
//TODO: active zoom in button in toolbox after fix it on chrome browser (jsPlumb library issue).
function ZoomIn(hdnIdToSaveDepartmentsData) {

    if (zoom >= 25 && zoom < 100) {

        zoom = zoom + 15;

        jsPlumb.setContainer("dvOrgStructure");

        $("#dvOrgStructure").css({
            "-webkit-transform": "scale(" + (zoom / 100).toString() + ")",
            "-moz-transform": "scale(" + (zoom / 100).toString() + ")",
            "-ms-transform": "scale(" + (zoom / 100).toString() + ")",
            "-o-transform": "scale(" + (zoom / 100).toString() + ")",
            "transform": "scale(" + (zoom / 100).toString() + ")"
        });

        jsPlumb.setZoom((zoom / 100).toString(), true);

        var departmentArray = $.parseJSON($('#' + hdnIdToSaveDepartmentsData).val());

        jsPlumb.repaintEverything(); //To recompute the location of the endpoints on that element after resize the element.
    }
}

//Zoom out orgStructure view.
//TODO: active zoom out button in toolbox after fix it on chrome browser (jsPlumb library issue).
function ZoomOut(hdnIdToSaveDepartmentsData) {

    if (zoom <= 100 && zoom > 25) {

        zoom = zoom - 15;

        jsPlumb.setContainer("dvDiagram");

        $("#dvDiagram").css({
            "-webkit-transform": "scale(" + (zoom / 100).toString() + ")",
            "-moz-transform": "scale(" + (zoom / 100).toString() + ")",
            "-ms-transform": "scale(" + (zoom / 100).toString() + ")",
            "-o-transform": "scale(" + (zoom / 100).toString() + ")",
            "transform": "scale(" + (zoom / 100).toString() + ")"
        });

        jsPlumb.setZoom((zoom / 100).toString(), true);

        var departmentArray = $.parseJSON($('#' + hdnIdToSaveDepartmentsData).val());


        jsPlumb.repaintEverything(); //To recompute the location of the endpoints on that element after resize the element.
    }
}

//Switch between org structure original view and other view by changeOrgUnitView button in the tool box. 
function ChangeOrgUnitView(hdnIdToSaveDepartmentsData, hdnIdToSaveSettings, hdnIdToSaveSelectedDepartment, languageShortName, listOfActions, limitation, endpointLocation, showToolBox) {

    if (!$('#aChangeOrgUnitView').hasClass('sel')) {

        $('#aChangeOrgUnitView').addClass('sel');
        $("#_dvOrgStructureContaner").css("display", "none");
        $("#dvRenderView").css("display", "block");

        document.dispatchEvent(changeOrgUnitViewEvent);
    }
    else {

        $('#aChangeOrgUnitView').removeClass('sel');
        $('#dvRenderView').html('');
        $("#dvRenderView").css("display", "none");
        $("#_dvOrgStructureContaner").css("display", "block");

        jsPlumb.repaintEverything();

        InitalizeOrgStructureView(hdnIdToSaveDepartmentsData, hdnIdToSaveSettings, hdnIdToSaveSelectedDepartment, languageShortName, listOfActions, limitation, endpointLocation, showToolBox);
    }
}

//Check if the type of orgUnit view is original.
function IsOrgUnitTypeOriginal() {

    return !$('#aChangeOrgUnitView').hasClass('sel');
}

//Replace orgStructure view with new html
function RenderView(html) {

    $('#dvRenderView').html(html);
}

//Trigger save orgStructure event.
function OnSaveOrgStructure() {

    

    document.dispatchEvent(saveOrgStructureEvent);
}

//Get root department id.
function GetRootKey() {
    return $("#jstree_default").jstree().get_selected(true)[0].li_attr.node;
}

//Build all html elements that will used in OrgStructure control. 
function PrepareOrgStructureLayout(hdnIdToSaveDepartmentsData, listOfActions, hdnIdToSaveSelectedDepartment, showToolBox, hdnIdToSaveSettings, languageShortName, limitation, endpointLocation) {

    //ToolBox.  
    if (showToolBox.toLowerCase() == "true") {
        $('.OrgStructure').append('<div id="_dvToolBox" class="panel-heading toolbar_structure"></div>');

        $('#_dvToolBox').append('<div class="buts_toolbar" id="_dvListOfActions"></div>');
    }

    for (var i = 0; i < listOfActions.length; i++) {

        $('#_dvListOfActions').append('<button class="example-p-02 btn btn-primary dialog_but" id="btnUnitCounter" onclick="' + listOfActions[i].label + '(); return false;">' + listOfActions[i].value + '</button>');
    }

    //if (showToolBox.toLowerCase() == "true") {

    //    $('#_dvToolBox').append('<div class="icon_toolbar"><div class="icon_group_left"><a id="aChangeOrgUnitView" class="op2" href="" ><img src="' + _imgCangeOrgUnitType + '"></a><a onclick="OnSaveOrgStructure(); return false;" class="" href=""><img src="' + _imgSave + '"></a></div><div class="icon_group_left"><a style="display : none;" href="#" onclick="ZoomIn(\'' + hdnIdToSaveDepartmentsData + '\')"><img src="' + _imgZoomIn + '"></a><a style="display : none;" href="#" onclick="ZoomOut(\'' + hdnIdToSaveDepartmentsData + '\')"><img src="' + _imgZoomOut + '"></a><a href="#"  id = "btnDeleteDepartment"><img src="' + _imgDelete + '"></a></div></div>');
    //}

    //$('#aChangeOrgUnitView').click(function () {

    //    ChangeOrgUnitView(hdnIdToSaveDepartmentsData, hdnIdToSaveSettings, hdnIdToSaveSelectedDepartment, languageShortName, listOfActions, limitation, endpointLocation, showToolBox)

    //    return false;
    //});

    $('#btnDeleteDepartment').click(function () {

        if ($('#' + hdnIdToSaveSelectedDepartment).val() != '')
            ManagementDeleteDepartment(hdnIdToSaveSelectedDepartment);

        else
            document.dispatchEvent(noDepartmentSelectionEvent);
    });

    //if (showToolBox.toLowerCase() == "true") {
    //    $('#_dvToolBox').append('<div class="icon_toolbar" id="_dvToolBoxDepartment"><div class="icon_group_right"><a href="#"><img src="' + _imgDepartment + '"></a></div></div>');
    //}

    $('.OrgStructure').append('<div id="_dvOrgStructureContaner" class="Diagram panel-body row col-md-12 stage_structure"><div id="dvOrgStructure" ></div></div><div style="display:none;" id="dvRenderView"></div>');
}


//TODO: Ehab Test 
function findKey(key, hdnIdToSaveSettings) {

    var settingArray = $.parseJSON($('#' + hdnIdToSaveSettings).val());

    for (var j = 0; j < settingArray.length; j++) {
        if (key == settingArray[j].Key) {
            return j;
        }
    }

    return -1;
}


//Draw departmets from department array
function InitalizeOrgStructureView(hdnIdToSaveDepartmentsData, hdnIdToSaveSettings, hdnIdToSaveSelectedDepartment, languageShortName, listOfActions, limitation, endpointLocation, showToolBox) {

    var departmentArray = $.parseJSON($('#' + hdnIdToSaveDepartmentsData).val());

    var settingArray = $.parseJSON($('#' + hdnIdToSaveSettings).val());




    //TODO: Ehab Test 
    if (departmentArray) {
        for (var i = 0; i < departmentArray.length; i++) {
            var index = findKey(departmentArray[i].Key, hdnIdToSaveSettings);
            if (index == -1) {
                settingArray.push({ "Key": departmentArray[i].Key, "loc": [0, 0], "children": "", "LinkUnitsKeys": [] });
            }
        }
    }


    $('#' + hdnIdToSaveSettings).val(JSON.stringify(settingArray));

    //Create the old department on control initialize from department data hidden.
    if (departmentArray) {
        for (var i = 0; i < departmentArray.length; i++) {
            countIdChange = departmentArray[i].Key;
            if (countIdChange >= countId) { //countId used for the departments that will created from toolbox.

                countId = countIdChange + 1;
            } s
            if (!departmentArray[i].IsDeleted && $('#' + departmentArray[i].Key).length == 0) {

                for (var y = 0; y < departmentArray[i].Names.length; y++) {

                    if (departmentArray[i].Names[y].language == languageShortName) {

                        departmentArray[i].Name = departmentArray[i].Names[y].name;
                    }
                }

                if (departmentArray[i].AssignmentPaper != null) {

                    if (departmentArray[i].AssignmentPaper.Beneficiaries != null) {
                        for (var y = 0; y < departmentArray[i].AssignmentPaper.Beneficiaries.length; y++) {
                            if (departmentArray[i].AssignmentPaper.Beneficiaries[y] != null) {
                                if (departmentArray[i].AssignmentPaper.Beneficiaries[y].Key == 0) {

                                    departmentArray[i].AssignmentPaper.Beneficiaries[y].Key = departmentArray[i].AssignmentPaper.Beneficiaries[y].Id;
                                }
                            }
                        }
                    }
                }

                $('#' + hdnIdToSaveDepartmentsData).val(JSON.stringify(departmentArray));

                var parentName;

                if (departmentArray[i].ParentId.toString() != "-1") {

                    var parentIndex = ElementIndex(departmentArray, departmentArray[i].ParentId);
                    parentName = departmentArray[parentIndex].Name;
                }

                else {

                    parentName = _language[languageShortName].TheRootDepartment;
                }

                $('#dvOrgStructure').append('<div class="administration" style="left: 617px; top: 219px; display: block; z-index: 100;" id="' + departmentArray[i].Key + '"><div>' + departmentArray[i].Name + '</div><div class="level_of">' + parentName + '</div><div class="panel-body faq faq_stage"><div class="faq-item"><div class="faq-title"><span class="fa fa-angle-down"></span>' + _language[languageShortName].Groups + '</div><div class="faq-text"><div class="list_group"><div class="table-responsive"><table class="tbl-responsive table table-striped"><tbody></tbody></table></div></div></div></div></div></div>'); //Create div(department).

                SaveLocation(departmentArray[i].Key.toString(), hdnIdToSaveDepartmentsData, hdnIdToSaveSettings); //To update the department location.
                DepartmentContextmenu(departmentArray[i].Key.toString(), hdnIdToSaveSelectedDepartment, languageShortName, listOfActions); //Create contextmenu for this department.

                //Create the four endpoints.
                if (endpointLocation == "") {
                    jsPlumb.addEndpoint($('#' + departmentArray[i].Key), { uuid: 'top' + departmentArray[i].Key }, topEndpoint);
                    jsPlumb.addEndpoint($('#' + departmentArray[i].Key), { uuid: 'bottom' + departmentArray[i].Key }, bottomEndpoint);
                    jsPlumb.addEndpoint($('#' + departmentArray[i].Key), { uuid: 'left' + departmentArray[i].Key }, leftEndpoint);
                    jsPlumb.addEndpoint($('#' + departmentArray[i].Key), { uuid: 'right' + departmentArray[i].Key }, rightEndpoint);
                }

                if (endpointLocation == 'bottom') {
                    jsPlumb.addEndpoint($('#' + departmentArray[i].Key), { uuid: 'bottom' + departmentArray[i].Key }, bottomEndpoint);
                }

                if (endpointLocation == 'left') {
                    jsPlumb.addEndpoint($('#' + departmentArray[i].Key), { uuid: 'left' + departmentArray[i].Key }, leftEndpoint);
                }

                if (endpointLocation == 'right') {
                    jsPlumb.addEndpoint($('#' + departmentArray[i].Key), { uuid: 'right' + departmentArray[i].Key }, rightEndpoint);
                }

                if (endpointLocation == 'top') {
                    jsPlumb.addEndpoint($('#' + departmentArray[i].Key), { uuid: 'top' + departmentArray[i].Key }, topEndpoint);
                }

                jsPlumb.draggable($('#' + departmentArray[i].Key)); //Make the department draggable.
                var iSettingsIndex = ElementIndex(settingArray, departmentArray[i].Key);

                $('#' + departmentArray[i].Key.toString()).position({ //Put the div(department) on its location.

                    of: $('#dvOrgStructure'),
                    my: 'left top',
                    at: 'left' + settingArray[iSettingsIndex].loc[0].toString() + ' ' + 'top' + settingArray[iSettingsIndex].loc[1].toString(),
                });

                $('#' + departmentArray[i].Key.toString()).css({ top: parseInt(settingArray[iSettingsIndex].loc[1].toString()), left: parseInt(settingArray[iSettingsIndex].loc[0].toString()), position: 'absolute' });

                //jsPlumb.repaintEverything();

                LimitationBehavior(hdnIdToSaveDepartmentsData, hdnIdToSaveSettings, departmentArray[i].Key, null, limitation, languageShortName);

                $('#' + departmentArray[i].Key.toString()).click(function (e) {

                    e.stopPropagation();
                    $('.administration').removeClass('select');
                    $('#' + this.id).addClass('select');
                    $('#' + hdnIdToSaveSelectedDepartment).val(this.id);
                });

                $('#' + departmentArray[i].Key.toString()).mousedown(function (e) {

                    $('#' + this.id).mouseup(function (e) {

                        jsPlumb.repaintEverything();
                    });

                    if (e.target.className == "fa fa-angle-down") {

                        $('#' + this.id + ' .fa').removeClass('fa-angle-down');
                        $('#' + this.id + ' .fa').addClass('fa-angle-up');
                        $('#' + this.id + ' .faq-item').addClass('active');

                        jsPlumb.repaintEverything();
                        return
                    }

                    if (e.target.className == "fa fa-angle-up") {

                        $('#' + this.id + ' .fa').removeClass('fa-angle-up');
                        $('#' + this.id + ' .fa').addClass('fa-angle-down');
                        $('#' + this.id + ' .faq-item').removeClass('active');

                        jsPlumb.repaintEverything();
                        return
                    }
                });
            }
        }
    }

    jsPlumb.repaintEverything();

    // var alllowUpdate

    // var settingArray = $.parseJSON($('#' + hdnIdToSaveSettings).val());

    for (var i = 0; i < settingArray.length; i++) {

        var depIndex = ElementIndex(departmentArray, settingArray[i].Key);

        if (depIndex != undefined) {

            if (!departmentArray[depIndex].IsDeleted) {

                for (var y = 0; y < settingArray[i].LinkUnitsKeys.length; y++) {

                    var linkIndex = ElementIndex(departmentArray, settingArray[i].LinkUnitsKeys[y].Key);

                    if (linkIndex != undefined) {

                        if (!departmentArray[linkIndex].IsDeleted) {

                            if (departmentArray.length > 0) {

                                var connect = jsPlumb.connect({ uuids: [settingArray[i].LinkUnitsKeys[y].EndPointFrom, settingArray[i].LinkUnitsKeys[y].EndPointTo] });
                                if (connect != undefined) {
                                    connect.connector.svg.id = connect.id;

                                    LinkContextmenu(connect.connector.svg.id, hdnIdToSaveDepartmentsData, hdnIdToSaveSettings, languageShortName); //Create contextmenu for each old link.
                                }
                            }
                        }
                    }

                }
            }

        }
    }
}

//Initialize the control. 
function OrgStructure(hdnIdToSaveDepartmentsData, hdnIdToSaveSettings, hdnIdToSaveSelectedDepartment, languageShortName, listOfActions, limitation, endpointLocation, showToolBox) {

    limitation = parseInt(limitation);
    listOfActions = $.parseJSON(listOfActions);
    endpointLocation = endpointLocation || "";

    listOfActions = $.map(listOfActions, function (value, key) {
        return {
            label: value,
            value: key
        };
    });

    //If the hiddens has no array create an empty array.
    if ($('#' + hdnIdToSaveDepartmentsData).val() == '' || $('#' + hdnIdToSaveDepartmentsData).val() == null) {

        $('#' + hdnIdToSaveDepartmentsData).val('[]');
    }

    if ($('#' + hdnIdToSaveSettings).val() == '' || $('#' + hdnIdToSaveSettings).val() == null) {

        $('#' + hdnIdToSaveSettings).val('[]');
    }

    PrepareOrgStructureLayout(hdnIdToSaveDepartmentsData, listOfActions, hdnIdToSaveSelectedDepartment, showToolBox, hdnIdToSaveSettings, languageShortName, limitation, endpointLocation); //Build all html elements.

    $(document).ready(function () {

        $('html').keyup(function (e) {
            if (e.keyCode == 46 && $('.modal-body').length === 0) {
                if ($('#' + hdnIdToSaveSelectedDepartment).val() != '')
                    ManagementDeleteDepartment(hdnIdToSaveSelectedDepartment);
            }
        });
        Repaint(); //Resize the control.

        $('#_dvToolBoxDepartment').draggable({ //Make the department in toolbox draggable.
            containment: '.OrgStructure',
            helper: 'clone'
        });

        $('.Diagram').droppable({ //Make the diagram droppable.
            accept: '#_dvToolBoxDepartment',
            drop: function (event, ui) {


                //Create new department on drop the toolbox department.
                CreateDepartment(hdnIdToSaveDepartmentsData, hdnIdToSaveSettings, hdnIdToSaveSelectedDepartment, languageShortName, listOfActions, ui.position.left, ui.position.top, limitation);
            }
        });
    });

    jsPlumb.ready(function () {

        $(document).on("click", "#_dvOrgStructureContaner:not(.administration)", function () {

            if (removeSelectedDep) {

                $('.administration').removeClass('select');
                $('#' + hdnIdToSaveSelectedDepartment).val('');
            }
            else {

                removeSelectedDep = true;
            }
        });

        InitalizeOrgStructureView(hdnIdToSaveDepartmentsData, hdnIdToSaveSettings, hdnIdToSaveSelectedDepartment, languageShortName, listOfActions, limitation, endpointLocation, showToolBox);

        jsPlumb.bind('connectionDragStop', function (connection) {

            var addArrow = true;

            var listOfConnection = jsPlumb.getAllConnections();

            for (var i = 0; i < listOfConnection.length - 1; i++) {

                if (((listOfConnection[i].sourceId == connection.targetId && listOfConnection[i].targetId == connection.sourceId) && !(listOfConnection[i].endpoints[0]._jsPlumb.uuid == connection.endpoints[1]._jsPlumb.uuid && listOfConnection[i].endpoints[1]._jsPlumb.uuid == connection.endpoints[0]._jsPlumb.uuid)) || (listOfConnection[i].sourceId == connection.sourceId && listOfConnection[i].targetId == connection.targetId) || (connection.sourceId == connection.targetId)) {

                    addArrow = false;
                }
            }

            if (!addArrow) {

                //delete the the ceated link(this link between two depatments already connected with each other).
                jsPlumb.detach(connection);
            }

            else {

                //Add the new created link to the link data array
                var departmentArray = $.parseJSON($('#' + hdnIdToSaveDepartmentsData).val());
                var settingArray = $.parseJSON($('#' + hdnIdToSaveSettings).val());
                var index = ElementIndex(departmentArray, connection.sourceId);
                var settingIndex = ElementIndex(settingArray, connection.sourceId);

                departmentArray[index].LinkUnitsKeys.push(connection.targetId);
                settingArray[settingIndex].LinkUnitsKeys.push({ Key: connection.targetId, EndPointFrom: connection.endpoints[0]._jsPlumb.uuid, EndPointTo: connection.endpoints[1]._jsPlumb.uuid });

                $('#' + hdnIdToSaveDepartmentsData).val(JSON.stringify(departmentArray));
                $('#' + hdnIdToSaveSettings).val(JSON.stringify(settingArray));
            }
        });

        //Create contextmenu for the new created link. 
        jsPlumb.bind('connection', function (connection, originalEvent) {

            connection.connection.connector.svg.id = connection.connection.id;

            LinkContextmenu(connection.connection.connector.svg.id, hdnIdToSaveDepartmentsData, hdnIdToSaveSettings, languageShortName);
        });
    });
}

function doReload() {
    if (__dialog) {
        __dialog.close();
    }
    return false;
}