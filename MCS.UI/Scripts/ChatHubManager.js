var connectionChat = $.hubConnection(signalROptions.WebApiUrl);
var chatHubOptions = {
    currentRoomName: null,
    currentRoomId: null,
    currentRoomMessageId: null,
    lastMessageReadId: null,
    isLoadingMessage: null,
    chatUsersPage: 0,
    chatConversationPage: 0,
    pageSize: 10,
    chatAvatarImage: signalROptions.BaseUri + '/Content/User/lib/images/avatar.svg',
    chatNotificationImage: signalROptions.BaseUri + '/Content/User/lib/images/logo-notifcation.png',
    isShareBusy: false,
    isShareEnabled: false,
    isShareLetterWindow: false,
    isTransGuest: false,
    currentRoomTransactionId: null,
    currentRoomTransactionNumber: null,
    shareRoomList: [],
    chatType: 'general'
};

//Create Hub Proxy
var chatHubService = connectionChat.createHubProxy('chatServiceHub');

connectionChat.disconnected(function () {
    console.log('disconnected connection to the signalr chat server start');
    setTimeout(function () {
        GetData();
    }, 5000); // Restart connection after 5 seconds.
});

chatHubService.on("addMessage", function (message, roomName) {
    if (chatHubOptions.currentRoomName == roomName) {
        AppendMessageChat(message, false);
        if (chatHubOptions.isShareEnabled && !chatHubOptions.isShareLetterWindow) {
            if ($(".popchat").css('display') == 'block')
                SetLastMessageRead(message.Id);
            return;
        }
       SetLastMessageRead(message.Id);
    }
});

chatHubService.on("updateConversationList", function (message, roomName) {
    GetConversation(0, chatHubOptions.pageSize, false);
});

chatHubService.on("newMessageRead", function (messageStatus, roomName) {
    if (chatHubOptions.currentRoomName == roomName) {
        if (messageStatus.UserId != signalROptions.UserId) {
            SetMessagesStatus(messageStatus.LastReadMessageId);
            GetConversation(0, chatHubOptions.pageSize, false);
        }
    }
});

chatHubService.on("chatNotifyFlag", function (isNotify) {
    if (isNotify) {
        $('#chatNotificationFlag').addClass('white-notification');
    }
    else {
        $('#chatNotificationFlag').removeClass('white-notification');
    }
});

chatHubService.on("chatPushNotification", function (notification) {
    if (notification != undefined && (chatHubOptions.isShareEnabled || chatHubOptions.chatType == 'transaction' || notification.RoomName != chatHubOptions.currentRoomName)) {
        Push.create(notification.Title, {
            body: notification.Body,
            icon: chatHubOptions.chatNotificationImage,
            onClick: function () {
                window.focus();
                this.close();
            }
        });
    }
});

chatHubService.on("leave", function (user, roomName) {
    if (chatHubOptions.currentRoomName == roomName) {
        if (chatHubOptions.isShareEnabled && chatHubOptions.isShareLetterWindow) {
            $('#shareLetterModal').modal('hide');
            chatHubOptions.isShareEnabled = false;
            chatHubOptions.isShareBusy = false;
            $('#shareChatMessageList').html('');
            $('.shareChatLetterText').html('');

            ShowWarningMessage(chatHubResources.inviteTerminated);
        }
    }
});

chatHubService.on("inviteShareLetter", function (userName, fromUserId) {
    if (!chatHubOptions.isShareBusy) {
        ShowCustomCloseConfirmMessage(chatHubResources.inviteRequest + userName, 'onInviteConfirm', fromUserId + '_' + userName, 'onCancelInvitation', fromUserId);
    }
    else {
        chatHubService.invoke('BusySharing', fromUserId).done(function () {
        });
    }
});

chatHubService.on("busySharing", function () {
    $('#chatModalInviteStatus').show();
    $('#chatModalInviteStatusText').html(chatHubResources.busySharing);
    $('#chatModalUsersList').hide();

    setTimeout(function () {
        $('#chatModalInviteStatus').hide();
        $('#chatModalInviteStatusText').html();
        $('#chatModalUsersList').show();
    }, 3000);
});

chatHubService.on("rejectShareLetter", function () {
    $('#chatModalInviteStatus').show();
    $('#chatModalInviteStatusText').html(chatHubResources.inviteReject);
    $('#chatModalUsersList').hide();

    setTimeout(function () {
        $('#chatModalInviteStatus').hide();
        $('#chatModalInviteStatusText').html();
        $('#chatModalUsersList').show();
    }, 3000);
});

chatHubService.on("acceptShareLetter", function (roomInfo) {
    if (roomInfo != undefined) {
        $('#messageList').html('');
        if (chatHubOptions.currentRoomName != undefined && chatHubOptions.currentRoomName != null && chatHubOptions.currentRoomName != '' && chatHubOptions.currentRoomName != roomInfo.Name) {
            LeaveChatRoom(chatHubOptions.currentRoomName);
        }

        chatHubOptions.shareRoomList.push(roomInfo.Id);
        chatHubOptions.currentRoomName = roomInfo.Name;

        GetConversationShare(roomInfo.Name);

        $('#shareEmpModal').modal('hide');
        InitChatWindow('share', null, null, false);
        chatHubOptions.isShareEnabled = true;
        chatHubOptions.isShareBusy = true;
        $('#chatShareWindowBtn').show();
    }
});

chatHubService.on("receiveLetterContent", function (content, roomName) {
    if (chatHubOptions.currentRoomName == roomName) {
        if (chatHubOptions.isShareEnabled && chatHubOptions.isShareLetterWindow) {
            $('.shareChatLetterText').html(content.toString());
        }
    }
});

chatHubService.on("createTransactionChatWindow", function (transactionId, transactionNumber, roomName) {
    InitChatWindow('transaction', transactionId, transactionNumber, true);
    chatHubOptions.currentRoomName = roomName;
    chatHubOptions.isTransGuest = true;
    GetConversationShare(roomName);
});

function BindRoomsToTransaction(transactionId) {
    if (chatHubOptions.shareRoomList != undefined && chatHubOptions.shareRoomList.length > 0) {
        var roomIds = chatHubOptions.shareRoomList.join();
        chatHubService.invoke('BindRoomsToTransaction', transactionId, roomIds).done(function () {
        });
    }
}

function BindChatRoomToTransaction(transactionId, roomId) {
    chatHubService.invoke('BindRoomsToTransaction', transactionId, roomId).done(function () {
    });
}

function onInviteConfirm(fromParam) {
    var fromUserId = fromParam.split('_')[0];
    var fromUserName = fromParam.split('_')[1];
    chatHubService.invoke('OneToOneRoom', fromUserId, null, null, true).done(function (roomInfo) {
        if (roomInfo != undefined) {
            $('#messageList').html('');
            if (chatHubOptions.currentRoomName != undefined && chatHubOptions.currentRoomName != null && chatHubOptions.currentRoomName != '' && chatHubOptions.currentRoomName != roomInfo.Name) {
                LeaveChatRoom(chatHubOptions.currentRoomName);
            }

            chatHubOptions.currentRoomName = roomInfo.Name;
            AcceptShareLetter(fromUserId, roomInfo.Name);
            chatHubOptions.isShareBusy = true;
            chatHubOptions.isShareEnabled = true;
            $('#shareChatFromUserName').html(fromUserName);
            $('#shareLetterModal').modal('show');

            if (roomInfo.RecentMessages != undefined && roomInfo.RecentMessages.length > 0) {
                chatHubOptions.currentRoomMessageId = roomInfo.RecentMessages[0].Id;

                for (var i = 0; i < roomInfo.UsersMessageStatus.length; i++) {
                    if (roomInfo.UsersMessageStatus[i].UserId != signalROptions.UserId) {
                        chatHubOptions.lastMessageReadId = roomInfo.UsersMessageStatus[i].LastReadMessageId;
                        break;
                    }
                }

                SetLastMessageRead(roomInfo.RecentMessages[roomInfo.RecentMessages.length - 1].Id);

                $.each(roomInfo.RecentMessages, function () {
                    var message = this;
                    AppendMessageChat(message, false);
                });
            }
        }
    }).fail(function (error) {
        console.log('Error: ' + error);
    });
}

function onCancelInvitation(fromUserId) {
    chatHubService.invoke('RejectShareLetterInvitation', fromUserId).done(function () {

    });
}

function AcceptShareLetter(fromUserId, roomName) {
    chatHubService.invoke('AcceptShareLetterInvitation', fromUserId, roomName).done(function () {
    });
}

function ShareLetterContent(content) {
    if (chatHubOptions.isShareEnabled) {
        chatHubService.invoke('ShareLetterContent', content, chatHubOptions.currentRoomName).done(function () {
        });
    }
}

function OpenShareModal() {
    if (chatHubOptions.isShareEnabled) {
        ShowConfirmMessage(chatHubResources.terminateConfirm, 'onTerminateShare');
        return;
    }
    $('#shareEmpModal').modal('show');
}

function onTerminateShare() {
    chatHubOptions.isShareEnabled = false;
    chatHubOptions.isShareBusy = false;
    $('#chatShareWindowBtn').hide();
    $('#shareEmpModal').modal('hide');

    LeaveChatRoom(chatHubOptions.currentRoomName);
}

function InitChatWindow(chatType, transactionId, transactionNumber, isToUser) {

    if (chatType == 'general') {
        $('#chatTransNumber').hide();
        $('#chatUserSearchDiv').show();
        $('#chatConversations').show();
        $('#chatTransSave').hide();
        $('#chatConversationsShare').hide();
        $('#chatUsers').hide();
        chatHubOptions.chatType = chatType;
        chatHubOptions.currentRoomTransactionId = null;
        chatHubOptions.currentRoomTransactionNumber = null;
    }
    else if (chatType == 'share') {
        $('#chatTransSave').hide();
        $('#chatTransNumber').hide();
        $('#chatUserSearchDiv').hide();
        $('#chatConversations').hide();
        $('#chatConversationsShare').show();
        $('#chatUsers').hide();
        chatHubOptions.chatType = chatType;
        chatHubOptions.currentRoomTransactionId = null;
        chatHubOptions.currentRoomTransactionNumber = null;
    }
    else if (chatType == 'transaction') {
        $('#chatTransNumber').show();
        $('#chatUserSearchDiv').show();
        $('#chatTransSave').show();
        $('#chatConversations').hide();
        if (isToUser) {
            $('#chatUserSearchDiv').hide();
            $('#chatTransSave').hide();
            $('#chatConversations').show();
        }
        $('#chatConversationsShare').hide();
        $('#chatUsers').hide();
        $('#chatTransNumber').html($('#chatTransNumber').html().replace('{0}', transactionNumber));
        chatHubOptions.chatType = chatType;
        chatHubOptions.currentRoomTransactionId = transactionId;
        chatHubOptions.currentRoomTransactionNumber = transactionNumber;
    }

    $(".carddiv").css("display", "flex");
    $(".firstcard").fadeIn();
}

function GetUserSearch(name, pageIndex, pageSize, isScroll) {
    chatHubService.invoke('GetAllCollaborationUsers', name, pageIndex, pageSize, signalROptions.CultureShortName).done(function (users) {
        if (pageIndex == 0)
            chatHubOptions.chatUsersPage = 0;
        if (!isScroll) {
            $('#chatUsersList').html('');
        }
        if (users == undefined || users.length == 0) {
            if (!isScroll) {
                $('#chatConversations').hide();
                $('#chatUsers').show();
                $('#chatUsersNoResultLabel').show();
            }
            return;
        }
        chatHubOptions.chatUsersPage++;
        $('#chatConversations').hide();
        $('#chatUsersNoResultLabel').hide();
        $('#chatUsers').show();
        $.each(users, function () {
            var user = this;
            $('#chatUsersList').append(CreateConversationChatItem(user.UserName, user.UserId, '', '', ''));
        });
    }).fail(function (error) {
        console.log('Error: ' + error);
    });
}

function GetUserSearchModal(name, pageIndex, pageSize, isScroll) {
    chatHubService.invoke('GetAllCollaborationUsers', name, pageIndex, pageSize, signalROptions.CultureShortName).done(function (users) {
        if (pageIndex == 0)
            chatHubOptions.chatUsersPage = 0;
        if (!isScroll) {
            $('#chatModalUsersList').html('');
        }
        if (users == undefined || users.length == 0) {
            return;
        }
        chatHubOptions.chatUsersPage++;

        $('#chatModalInviteStatus').hide();
        $('#chatModalUsersList').show();

        $.each(users, function () {
            var user = this;
            $('#chatModalUsersList').append(CreateModalUserChatItem(user.UserName, user.UserId, 0));
        });
    }).fail(function (error) {
        console.log('Error: ' + error);
    });
}

function GetConversation(pageIndex, pageSize, isScroll) {
    if (chatHubOptions.chatType == 'share' && !chatHubOptions.isShareLetterWindow) {
        GetConversationShare(chatHubOptions.currentRoomName);
        return;
    }
    else if (chatHubOptions.chatType == 'transaction') {
        GetConversationShare(chatHubOptions.currentRoomName);
        return;
    }
    chatHubService.invoke('GetConversations', null, null, pageIndex, pageSize, signalROptions.CultureShortName).done(function (conversations) {
        if (pageIndex == 0)
            chatHubOptions.chatConversationPage = 0;
        if (!isScroll) {
            $('#chatConversationsList').html('');
        }
        if (conversations == undefined || conversations.length == 0) {
            if (!isScroll) {
                //GetUserSearch("", 0, chatHubOptions.pageSize, false);
            }
            return;
        }
        chatHubOptions.chatConversationPage++;
        $('#chatConversations').show();
        $('#chatUsers').hide();
        $.each(conversations, function () {
            var chat = this;
            $('#chatConversationsList').append(CreateConversationChatItem(chat.Name, chat.UserId, chat.RoomName, chat.TotalNumberOfUnreadMessages, chat.LastMessage));
        });

        $('.chatConversationsList').each(function (i, obj) {
            var dataMessageId = $(obj).attr('data-messageId');
            var userId = $(obj).attr('data-userId');
            
        });

    }).fail(function (error) {
        console.log('Error: ' + error);
    });
}

function GetConversationShare(roomName) {
    chatHubService.invoke('GetConversationByName', null, null, roomName, signalROptions.CultureShortName).done(function (conversations) {

        $('#chatConversationsShareList').html('');
        if (conversations == undefined || conversations.length == 0) {
            return;
        }

        $('#chatConversationsShare').show();
        $('#chatConversations').hide();
        $('#chatUsers').hide();

        $.each(conversations, function () {
            var chat = this;
            $('#chatConversationsShareList').append(CreateConversationChatItem(chat.Name, chat.UserId, chat.RoomName, chat.TotalNumberOfUnreadMessages, chat.LastMessage));
            $('#chatShareWindowFlag').html(chat.TotalNumberOfUnreadMessages);
        });

    }).fail(function (error) {
        console.log('Error: ' + error);
    });
}

function GetChatMessages(lastMessageId) {

    chatHubService.invoke('GetPreviousMessages', lastMessageId).done(function (messages) {
        chatHubOptions.isLoadingMessage = true;
        if (messages != undefined && messages.length > 0) {
            chatHubOptions.currentRoomMessageId = messages[messages.length - 1].Id;

            $.each(messages, function () {
                var message = this;
                AppendMessageChat(message, true);
            });
        }
        chatHubOptions.isLoadingMessage = false;
    }).fail(function (error) {
        chatHubOptions.isLoadingMessage = false;
        console.log('Error: ' + error);
    });
}

function OnClickUserChatItem(element) {
    var toUserId = $(element).attr("data-userId");
    var roomName = $(element).attr("data-roomName");
    var isForShare = (chatHubOptions.chatType == 'share') ? true : false;
    chatHubService.invoke('OneToOneRoom', toUserId, chatHubOptions.currentRoomTransactionId, roomName, isForShare).done(function (roomInfo) {
        if (roomInfo != undefined) {
            $('#messageList').html('');
            if (chatHubOptions.currentRoomName != undefined && chatHubOptions.currentRoomName != null && chatHubOptions.currentRoomName != '' && chatHubOptions.currentRoomName != roomInfo.Name) {
                LeaveChatRoom(chatHubOptions.currentRoomName);
            }

            if (roomInfo.RecentMessages != undefined && roomInfo.RecentMessages.length > 0) {
                chatHubOptions.currentRoomMessageId = roomInfo.RecentMessages[0].Id;

                for (var i = 0; i < roomInfo.UsersMessageStatus.length; i++) {
                    if (roomInfo.UsersMessageStatus[i].UserId != signalROptions.UserId) {
                        chatHubOptions.lastMessageReadId = roomInfo.UsersMessageStatus[i].LastReadMessageId;
                        break;
                    }
                }

                SetLastMessageRead(roomInfo.RecentMessages[roomInfo.RecentMessages.length - 1].Id);

                $.each(roomInfo.RecentMessages, function () {
                    var message = this;
                    AppendMessageChat(message, false);
                });
            }

            chatHubOptions.currentRoomName = roomInfo.Name;
            chatHubOptions.currentRoomId = roomInfo.Id;
            if (chatHubOptions.chatType == 'transaction' && chatHubOptions.currentRoomTransactionId != null && !chatHubOptions.isTransGuest) {
                CreateTransactionChatWindow(chatHubOptions.currentRoomTransactionId, chatHubOptions.currentRoomTransactionNumber, roomInfo.Name);
            }

            OpenRoomChatWindow(element);
        }
    }).fail(function (error) {
        console.log('Error: ' + error);
    });
}

function CreateTransactionChatWindow(transactionId, transactionNumber, roomName) {
    chatHubService.invoke('CreateTransactionChatWindow', transactionId, transactionNumber, roomName).done(function () {
    });
}

function SendChatMessage(content, roomName, toUserId) {
    chatHubService.invoke('Send', content, roomName, toUserId).done(function (isDone) {
        if (isDone) {
            $('#txtChatMessage').val('');
            $('#txtShareChateMessage').val('');
        }
    });
}

function LeaveChatRoom(roomName) {
    chatHubOptions.lastMessageReadId = null;
    if (chatHubOptions.chatType != 'transaction') {
        chatHubOptions.currentRoomName = null;
        chatHubOptions.currentRoomId = null;
    }
    chatHubService.invoke('Leave', roomName).done(function () {
    });
}

function SetLastMessageRead(messageId) {
    chatHubService.invoke('SetLastMessageRead', messageId).done(function () {
        GetConversation(0, chatHubOptions.pageSize, false);
    });
}

function SetMessagesStatus(messageId) {
    $('.sendertext').each(function (i, obj) {
        var dataMessageId = $(obj).attr('data-messageId');
        var userId = $(obj).attr('data-userId');

        if (userId == signalROptions.UserId && dataMessageId <= messageId) {
            $(obj).children('.read-check').html('<i class="fas fa-check-double">');
        }
    });
}

function AppendMessageChat(message, topMessage) {
    var messageElement = (chatHubOptions.isShareLetterWindow) ? 'shareChatMessageList' : 'messageList';
    var messageContainerElement = (chatHubOptions.isShareLetterWindow) ? 'shareChatMessageContainer' : 'messageListContainer';
    var htmlTemp = '';
    var isRead = message.Id <= chatHubOptions.lastMessageReadId;
    if (message.User.Id == signalROptions.UserId) {
        htmlTemp = CreateSenderMessageItem(message.Content, message.SendDate + ' ' + message.SendTime, message.Id, message.User.Id, isRead);
    }
    else {
        htmlTemp = CreateRecievedMessageItem(message.Content, message.SendDate + ' ' + message.SendTime, message.Id, message.User.Id);
    }

    if (!topMessage) {
        $('#' + messageElement).append(htmlTemp);
        $('#' + messageContainerElement).scrollTop($('#' + messageContainerElement).prop("scrollHeight"));
    }
    else {
        $('#' + messageElement).prepend(htmlTemp);
    }
}

function OpenRoomChatWindow(element) {
    $('#popChatUserName').html($(element).attr('data-userName'));
    if ((screen.width >= 768)) {
        $(".carddiv").css("width", "50%");
        $(".sendfield").css("display", "flex");
        $(".firstcard").css("width", "50%");
        $(".popchat").fadeIn();
        $(".purple").css("width", "200%");
        $(".showpopchat").removeClass("highlight");
        $(this).toggleClass("highlight");
        $(".purple .la-close").css("left", "5px");
    }
    if ((screen.width < 768)) {
        $(".carddiv").css("width", "100%");
        $(".sendfield").css("display", "flex");
        $(".firstcard").css("display", "none");
        $(".popchat").css("width", "100%");
        $(".popchat").css("top", "0");
        $(".popchat").fadeIn();
        $(".showpopchat").removeClass("highlight");
        $(this).toggleClass("highlight");
        $(".card-header2").css("display", "block");
    }
    $('#messageListContainer').scrollTop($('#messageListContainer').prop("scrollHeight"));
}

function CreateConversationChatItem(userName, userId, roomName, unreadCount, lastMessage) {
    var notificationCount = '';
    if (unreadCount != undefined && unreadCount != '' && unreadCount != 0)
        notificationCount = '<label class="roundedred2 font-bold">' + unreadCount + '</label>';

    var highlightClass = '';
    if (roomName == chatHubOptions.currentRoomName)
        highlightClass = 'highlight';

    var htmlTemp = '<a href="#">' +
        '<div id="' + roomName + '" class="showpopchat d-flex py-3 px-3 ' + highlightClass + '" onclick="OnClickUserChatItem(this);" data-userId="' + userId + '"data-roomName="' + roomName + '"data-userName="' + userName + '">' +
        '<div class="h-avatar2 flex-shrink-1">' +
        '<img class="w-100 " src="' + chatHubOptions.chatAvatarImage + '">' +
        '</div>' +
        '<div class="flex-grow-1 ml-2">' +
            '<span class="dark-color">' +
                userName +
            '</span>' +
            '<p class="flex-column align-self-end chat_text">' + lastMessage + '</p>' +
        '</div>' +
        notificationCount +
        '</div>' +
        '</a>';

    return htmlTemp;
}

function CreateSenderMessageItem(content, date, messageId, userId, isRead) {
    var readText = isRead ? '<i class="fas fa-check-double">' : '';
    var htmlTemp = '<div class="sendertext" data-messageId="' + messageId + '" data-userId="' + userId + '">' +
                        '<div class="sender-p"   data-placement="top" title="' + date + '">' +
                            content +
                        '</div>' +
                        //'<label class="message-time">' + date + '</label>' +
                        '<span class="read-check">' + readText + '</span>' +
                    '</div>';

    return htmlTemp;
}

function CreateRecievedMessageItem(content, date, messageId, userId) {
    var htmlTemp = '<div class="recievedtext" data-messageId="' + messageId + '" data-userId="' + userId + '">' +
                        //'<label class="message-time" >'+ date + '</label>' +
                        '<div class="circle-img3">' +
                            '<img class="circle-img3 mx-1" src="' + chatHubOptions.chatAvatarImage + '">' +
                        '</div>' +
                        '<div class="recieve-p"   data-placement="top" title="' + date + '">' +
                            content +
                        '</div>'+
                    '</div>';

    return htmlTemp;
}

function CreateModalUserChatItem(userName, userId, status) {
    var userStatus = 'status offline';
    var htmlTemp = '<div class="chat-row">' +
                            '<div class="w-100" onclick="OnClickModalUserChat(this)" data-userId="' + userId + '">' +
                            '<img class="h-avatar2 flex-shrink-1" src = "' + chatHubOptions.chatAvatarImage + '">' +
                            '<span class="' + userStatus + '"></span>' +
                            '<span class="dark-color font-bold mx-1">' + userName + '</span>' +
                         '</div>' +
                    '</div>';

    return htmlTemp;
}

function OnClickModalUserChat(element) {
    var toUserId = $(element).attr("data-userId");

    chatHubService.invoke('InviteTransactionShareLetter', toUserId, signalROptions.CultureShortName).done(function () {
        $('#chatModalInviteStatus').show();
        $('#chatModalInviteStatusText').html(chatHubResources.waitingReply);
        $('#chatModalUsersList').hide();
    });
}

$(document).ready(function () {
    GetData();
});

function GetData() {
    $.ajax({
        url: signalROptions.ChatTokenUrl,
        type: 'GET',
        cache: false,
        contentType: "application/json",
        data: null,
        success: function (data) {
            StartConnection(data);
        },
        error: function (a, b, c, d) {
            ShowErrorMessage('حدث خطأ');
        }
    });
}

function StartConnection(data) {
    if (data.Token != undefined && data.Token != '') {
        connectionChat.qs = { 'Authorization': data.Token, 'TenantId': data.TanentId, '__TenantDatabaseName': data.TenantDatabaseName, 'TimeZone': new Date().getTimezoneOffset(), 'Culture': signalROptions.CultureShortName };
    }
    //connectionChat.logging = true;
    //Connecting the client to the signalr hub
    connectionChat.start({ jsonp: true })
        .done(function () {
            console.log("Connecting to the signalr chat server start");

            chatHubService.invoke("Join");

            GetConversation(0, chatHubOptions.pageSize, false);

            $('#chatUserSearch').on('input', function (e) {
                chatHubOptions.chatUsersPage = 0;
                GetUserSearch($('#chatUserSearch').val(), 0, chatHubOptions.pageSize, false);
            });

            $('#chatModalUserSearch').on('input', function (e) {
                chatHubOptions.chatUsersPage = 0;
                GetUserSearchModal($('#chatModalUserSearch').val(), 0, chatHubOptions.pageSize, false);
            });

            $('#chatConversations').bind('scroll', function () {
                if ($(this).scrollTop() == ($(this)[0].scrollHeight - $(this).innerHeight())) {
                    //Add something at the end of the page
                    GetConversation(chatHubOptions.chatConversationPage, chatHubOptions.pageSize, true);
                }
            });

            $('#chatUsers').bind('scroll', function () {
                if ($(this).scrollTop() == ($(this)[0].scrollHeight - $(this).innerHeight())) {
                    //Add something at the end of the page
                    GetUserSearch($('#chatUserSearch').val(), chatHubOptions.chatUsersPage, chatHubOptions.pageSize, true);
                }
            });

            $('#chatModalUsersList').bind('scroll', function () {
                if ($(this).scrollTop() == ($(this)[0].scrollHeight - $(this).innerHeight())) {
                    //Add something at the end of the page
                    GetUserSearchModal($('#chatModalUserSearch').val(), chatHubOptions.chatUsersPage, chatHubOptions.pageSize, true);
                }
            });

            $('#messageListContainer').bind('scroll', function () {
                if ($('#messageListContainer').scrollTop() < 60 && !chatHubOptions.isLoadingMessage) {
                    chatHubOptions.isLoadingMessage = true;
                    setTimeout(GetChatMessages(chatHubOptions.currentRoomMessageId), 1200);
                }
            });

            $('#shareChatMessageContainer').bind('scroll', function () {
                if ($('#shareChatMessageContainer').scrollTop() < 60 && !chatHubOptions.isLoadingMessage) {
                    chatHubOptions.isLoadingMessage = true;
                    setTimeout(GetChatMessages(chatHubOptions.currentRoomMessageId), 1200);
                }
            });

            $("#btnSendChatMessage").click(function () {
                SendChatMessage($('#txtChatMessage').val(), chatHubOptions.currentRoomName, null);
            });

            $('#txtChatMessage').on("keypress", function (e) {
                if (e.keyCode == 13) {
                    SendChatMessage($('#txtChatMessage').val(), chatHubOptions.currentRoomName, null);
                    return false;
                }
            });

            $("#btnSendShareChatMessage").click(function () {
                if (chatHubOptions.isShareLetterWindow) {
                    SendChatMessage($('#txtShareChateMessage').val(), chatHubOptions.currentRoomName, null);
                }
            });

            $('#txtShareChateMessage').on("keypress", function (e) {
                if (chatHubOptions.isShareLetterWindow) {
                    if (e.keyCode == 13) {
                        SendChatMessage($('#txtShareChateMessage').val(), chatHubOptions.currentRoomName, null);
                        return false;
                    }
                }
            });

            $('#shareEmpModal').on('show.bs.modal', function (e) {
                GetUserSearchModal(null, 0, chatHubOptions.pageSize, false);
            });

            $('#shareEmpModal').on('hidden.bs.modal', function (e) {
                $('#chatModalInviteStatus').hide();
                $('#chatModalUsersList').show();
            });

            $('#shareLetterModal').on('hidden.bs.modal', function (e) {
                chatHubOptions.isShareBusy = false;
                chatHubOptions.isShareEnabled = false;
                chatHubOptions.isShareLetterWindow = false;
            });

            $('#shareLetterModal').on('show.bs.modal', function (e) {
                chatHubOptions.isShareBusy = true;
                chatHubOptions.isShareEnabled = true;
                chatHubOptions.isShareLetterWindow = true;
            });

            $("#chatShareWindowBtn").click(function () {
                InitChatWindow('share', null, null, false);
            });

            $("#btnTransactionChatStart").click(function () {
                InitChatWindow('transaction', -1, $('.transaction-number').html(), false);
            });

            $("#btnSaveTransChat").click(function () {
                if (chatHubOptions.currentRoomId != undefined && chatHubOptions.currentRoomId != null && chatHubOptions.chatType == "transaction") {
                    BindChatRoomToTransaction($('#hdnTransactionId').val(), chatHubOptions.currentRoomId);
                }
            });
        })
        .fail(function () {
            console.log("failed in connecting to the signalr chat server");
        });
}

$(function () {

}());