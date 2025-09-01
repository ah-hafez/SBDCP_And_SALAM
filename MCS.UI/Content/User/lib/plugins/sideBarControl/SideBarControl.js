var IsAllChecked = false;
var selectedTrans = [];
var selectedEncryptedTrans = [];

$(document).ready(function () {
    $('.checkbox #CheckAllBoxes').on('change', function () {
        doCheckAll();
    });
});

function updateSelectedList(selectedId, selectedEncryptedId) {
    var id = parseInt(selectedId);
    index = selectedTrans.indexOf(id);
    if (index > -1) {
        selectedTrans.splice(index, 1);
        selectedEncryptedTrans.splice(index, 1);
    } else {
        selectedTrans.push(id);
        selectedEncryptedTrans.push(selectedEncryptedId);
    }
    var finalData = selectedTrans.join(",");
    var finalEncryptedData = selectedEncryptedTrans.join(",");
    $('#transactionsIds').val(finalData);
    $('#encryptedTransactionsIds').val(finalEncryptedData);
}

function getSelectedListLength() {
    return selectedTrans.length;
}

function holdDataFromBox(chk) {
    if ($('.boxes input:checked').length > 0) {       
        toggleSidebar("open");
        fillList($(chk).parents('.box-grid').attr('data-id'));
    }

    if (!$(chk).is(':checked')) {
        unfillList($(chk).parents('.box-grid').attr('data-id'));
        $('#CheckAllBoxes').prop('checked', false);
        
        IsAllChecked = false;
    }

    if ($('.boxes input:checkbox:checked').length == $('.boxes input:checkbox').length) {
        $('#CheckAllBoxes').prop('checked', true);
        
        IsAllChecked = true;
    }
    updateSelectedList($(chk).data('id'), $(chk).data('encryptedid'));
    if (!selectedTrans.length > 0) {
        toggleSidebar("close");
    } else {
        $('.paper_count').text(getSelectedListLength() + "");
    }
}


function doCheckAll() {
    var boxesIdsList = [];
    var boxesEncryptedIdsList = [];
    $('.boxes input:checkbox:checked').each(function () {
        boxesIdsList.push($(this).data("id"));
        boxesEncryptedIdsList.push($(this).data("encryptedid"));
    });

    if (!IsAllChecked) {
        $('.boxes input:checkbox').prop('checked', true);
        unfillAll(boxesIdsList, boxesEncryptedIdsList);
        boxesIdsList = [];
        boxesEncryptedIdsList = [];
        $('.boxes input:checkbox').each(function () {
            boxesIdsList.push($(this).data("id"));
            boxesEncryptedIdsList.push($(this).data("encryptedid"));
        });
        fillAll(boxesIdsList, boxesEncryptedIdsList);
        toggleSidebar("open");
        IsAllChecked = true;
    } else {
        $('.boxes input:checkbox').prop('checked', false);
        $('#CheckAllBoxes').prop('checked', false);
        
        unfillAll(boxesIdsList, boxesEncryptedIdsList);
        if ($('#transactionsIds').val() == "") {
            toggleSidebar("close");
        }
        IsAllChecked = false;
        $('.paper_count').text(getSelectedListLength() + "");
    }

}

function toggleSidebar(action) {
    if ($('.sidebarMultiOptions').length > 0) {
        var selectedCount = selectedTrans.length;
        if (action == 'close') {
            $('.paper_count').text(selectedCount);
            $('.sidebarMultiOptions:not(.invisible)').addClass('invisible');
        } else {
            $('.paper_count').text(selectedCount);
            $('.sidebarMultiOptions').removeClass('invisible');
        }

    }
}

function fillAll(ids, encryptedIds) {
    for (var i = 0; i < ids.length; i++) {
        fillList(ids[i]);
        updateSelectedList(ids[i], encryptedIds[i]);
    }
}

function unfillAll(ids, encryptedIds) {
    for (var i = 0; i < ids.length; i++) {
        unfillList(ids[i]);
        updateSelectedList(ids[i], encryptedIds[i]);
    }
}

function fillList(id) {
    $('ul.papers-selected').append('<li><a class="removeSideBarBox" data-id="' + id + '" ><i class="uicon icon_close"></i></a><span class="paper-name">' + $('#entityName_' + id).text() + '</span><span class="paper-number">' + $('#Number_' + id).text() + '</span></li>');
}

function unfillList(id) {
    $('ul.papers-selected li a.removeSideBarBox[data-id=' + id + ']').parent('li').remove();
}


function toggleCheckPaperBox() {
    $('.selected-papers-wrapper').slideToggle(160);
    $('.info-bottom .close-box').toggleClass('transform');

}

$(document).on('click', '.papers-selected li >a.removeSideBarBox', function () {
    $(this).parents('li').remove();
    //make selection of thetargeted box false as we remove it here
    $('div[data-id=' + $(this).attr('data-id') + ']').find('input:checkbox').prop('checked', false);
    $('#CheckAllBoxes').prop('checked', false);
    
    updateSelectedList($(this).attr('data-id'), $(this).attr('data-encryptedid'));
    var count = getSelectedListLength();
    $('.paper_count').text(count);
    IsAllChecked = false;
    //close the box whre no selected boxes lifted
    if ($('.papers-selected li').length == 0) {
        toggleCheckPaperBox();
        toggleSidebar("close");
    }
})

