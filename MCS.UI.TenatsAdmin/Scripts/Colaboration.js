
function AddInstanceMessageToChatWindow(userId,design){

    $('*[data-userid-stage-chat="' + userId + '"]').append(design);

}

function PrependInstanceMessageToChatWindow(userId,design){

    $('*[data-userid-stage-chat="' + userId + '"]').prepend(design);

}

function CreateInstanceMessageDesign(userName , text , date){

    var htmlDesign= '<div class="i">' +
                '<div class="img_other">' +
                   '<span></span>' +
                '</div>' +
                '<div class="area_send">' +
                   '<div class="row">' +
                        '<span class="name_other">' + userName + '</span>' +
                        '<span class="time_other"> ' + ' ' + date + '</span>' +
                    '</div>' +
                    '<div class="row">' +
                        '<span class="text_other"> ' + text + '</span>' +
                    '</div>' +
                '</div>'+
            '</div>';
    return htmlDesign;
}

function ClearChatSection()
{
    $(".section_chat").html('')
}

function AddToChatSection(htmlElement) {
    $(".section_chat").append(htmlElement)
}

function CreateNewUserOnline(name, userId){

    var htmlDesign= '<div class="other_chat onlineUser " data-online-userName="'+ name +'" data-online-userId="'+userId+'">' +
                            '<a href="#" id="User'+ userId +'" >' +
                                '<div class="col">' +
                                '</div>'+
                                '<div class="col">'+
                                    '<span class="name_other">'+ name + '</span>'+
                                    '<span class="describe_other"></span>'+
                                '</div>' +
                                '<span class="notify_other" data-notify-online-userId="'+userId+'"></span>'+
                            '</a>'+
                        '</div>' ;

    $(".section_chat").append(htmlDesign);


}

function CreateNewUserOffline(name, userId, notifyCount) {

    var htmlDesign = '<div class="other_chat onlineUser" data-online-userName="' + name + '" data-online-userId="' + userId + '">' +
                            '<a href="#" id="User' + userId + '" >' +
                                '<div class="col">' +
                                '</div>' +
                                '<div class="col">' +
                                    '<span class="name_other" id="name_other_' + userId + '" >' + name + '   *****  ' + '</span>' +
                                    '<span class="describe_other"></span>' +
                                '</div>' +
                                '<span class="notify_other" data-notify-online-userId="' + userId + '"> ' + notifyCount + '</span>' +
                            '</a>' +
                        '</div>';

    $(".section_chat").append(htmlDesign);


}

function GetUserSection() {

}

function OpenNewChatWindow(chatWindow) {

    $(".conversations").append(chatWindow);
}

function AppenedConversations(userId,conversations){

    if(conversations.length==0)
    {
        SetlastConversationNumber(userId, 0);
        return;
    }

    var i=0;
    var lastConversationId =conversations[0].Id;

    for(i=conversations.length-1  ; i >=0; i--)
    {
        var instanceMessageDesign = CreateInstanceMessageDesign(conversations[i].ReceiverName, conversations[i].Text, conversations[i].Date);
        PrependInstanceMessageToChatWindow(userId,instanceMessageDesign);
    }

    SetlastConversationNumber(userId,lastConversationId);
}

function SetlastConversationNumber(userId, lastConversationId){

    var stagechat=$('*[data-userid-stage-chat="' + userId + '"]');

    stagechat.data("last-conversation" ,lastConversationId);
}

function IncreaseChatNotification(){

    if(!$(".chat-Notification").is(':visible'))
    {
        $(".chat-Notification").show();
    }

    var chatNotification = $(".chat-Notification").html();

    chatNotification=parseInt(chatNotification)  + 1 ;

    $(".informer-danger , .informer").html(chatNotification);

}



function UpdateChatNotification(notificationCount) {

    if (!$(".chat-Notification").is(':visible')) {
        $(".chat-Notification").show();
    }

    var chatNotification = $(".chat-Notification").html();

    chatNotification = parseInt(notificationCount);

    $(".informer-danger , .informer").html(chatNotification);

}


function HideNotificationChat(){

    var chatNotification = $(".chat-Notification").html();

    chatNotification=parseInt(chatNotification) ;

    if(parseInt(chatNotification) <= 0){
        $(".chat-Notification").hide();
    }
}

function DecreaseChatNotification(nofifyCount){

    if(!IsNumeric(parseFloat(nofifyCount)))

        return ;

    var chatNotification = $(".chat-Notification").html();

    chatNotification=parseInt(chatNotification) - nofifyCount ;

    if(parseInt(chatNotification) <= 0){
        $(".chat-Notification").hide();
        chatNotification=0;
    }

    $(".chat-Notification").html(chatNotification);

}

function IsUserChatWindowOpened(userId)
{
    var chatWindow=$('*[data-userid-chat-window="' + userId + '"]');

    if(chatWindow.is(':visible'))
    {
        return true;
    }

    return false;
}

function GetUserNotifysCount(userId){

    var notifysCount= $('*[data-notify-online-userId="' + userId + '"]').html();

    if(notifysCount==""){
        notifysCount=0;

    }

    return  parseInt(notifysCount);
}

function ResetUserNotifysCount(userId){

    $('*[data-notify-online-userId="' + userId + '"]').html("");
    $('*[data-notify-online-userId="' + userId + '"]').hide();
}

function IncreaseUserNotifysCount(userId) {

    $('*[data-notify-online-userId="' + userId + '"]').show();

    var notifysCount= $('*[data-notify-online-userId="' + userId + '"]').html();

    if(notifysCount=="")
    {
        notifysCount=0;
    }

    notifysCount=parseInt(notifysCount) + 1;

    $('*[data-notify-online-userId="' + userId + '"]').html(notifysCount);
}

function InitializeUserNotifysCount(userId, notifysCount) {

    $('*[data-notify-online-userId="' + userId + '"]').html(notifysCount);

    if (notifysCount == 0)
    {
        $('*[data-notify-online-userId="' + userId + '"]').hide();
    }

}

function SetScrollToTop(elemntId) {

    elemnt = document.getElementById(elemntId);
    elemnt.scrollTop = elemnt.scrollHeight;
}

function ScrollChatWindow(userId, element){
    if($(element).scrollTop() > 10)
        return ;

    var lastConversationId=$(element).data("last-conversation");

    if(lastConversationId==0){
        return ;
    }

    GetChatHistory(UrlChatData,userId, lastConversationId, pageSize);

    element.scrollTop=  20;

}

function GetChatHistory(url, userId, startIndex , pageSize) {
    $.ajax({
        type: 'get',
        url: url,
        async: false,
        cache: false,
        data: { toUserId: userId, pageSize: pageSize, startId: startIndex },
        success:
            function (data) {
                AppenedConversations(userId, data.Conversations.Result);
            }
    });

}

function GetIntitialChatHistory(url, userId, userName , func) {
    $.ajax({
        type: 'get',
        url: url,
        data: { toUserId: userId, pageSize: 5, toUserName: userName },
        cache: false,
        success:
            function (data) {
                OpenNewChatWindow(data.Html);
               
                if (func != undefined) {
                    func();
                }
            }
    });

}


