(function () {
    var connection = $.hubConnection(signalROptions.WebApiUrl);
    var markAsReadTemplate = "<div id='new' class='notification-item'></div>";
    //Create Hub Proxy
    var notificationService = connection.createHubProxy('notificationService');
    var signalRNotificationService = connection.createHubProxy('signalRNotificationService');

    //handle the received message from the hub
    signalRNotificationService.on("displayNotification", function (message) {

        message.Body = message.Body.replace("{BaseUrl}", signalROptions.BaseUri);
        showSuccessToast(message.Body);
        SetNotification(message.Body);
    });

    notificationService.on("displayNotification", function (message) {

        showSuccessToast(message.Body);
    });

    notificationService.on("showMessage", function (fromUser, message, date, transactionId) {

        if (!IsUserChatWindowOpened(fromUser.UserId)) {
            if (GetUserNotifysCount(fromUser.UserId) == 0) {
                IncreaseChatNotification();
            }
            IncreaseUserNotifysCount(fromUser.UserId);
            return;
        }
        var stageChat = $("#stage_chat_userId_" + fromUser.UserId);
        var instanceMessageDesign = CreateInstanceMessageDesign(fromUser.UserName, message, date);
        AddInstanceMessageToChatWindow(fromUser.UserId, instanceMessageDesign);
        SetScrollToTop("stage_chat_userId_" + fromUser.UserId)
        if (transactionId > 0) {
            SetChatBinding(fromUser.UserId, transactionId);
        }
    });

    notificationService.on("getOnlineUsers", function (message) {
        message = eval(message);
        message.forEach(function (entry) {
            var id = signalROptions.UserId;
            if (entry.UserId != id) {
                if ($("#User" + entry.UserId).length < 1) {
                    CreateNewUserOnline(entry.UserName, entry.UserId);

                    AllUsers.push($('.onlineUser[data-online-userid="' + entry.UserId + '"]'));
                }
                else {

                    $("#name_other_" + entry.UserId).val("");

                    $("#name_other_" + entry.UserId).val(entry.UserName);
                }
                AppearOnlineUser(entry.UserId);
            }
        });
    });

    notificationService.on("newUserConnected", function (message) {

        message = eval(message);
        $("#name_other_" + message.UserId).html("");
        $("#name_other_" + message.UserId).html(message.UserName + '<div class="appear online"></div>');
        AppearOnlineUser(message.UserId);
    });

    notificationService.on("ShowMessageDate", function (date) {
        $('.time_other').each(function (index, value) {
            var text = $(this).text().trim();
            if (text == "") {
                $(this).text(date);
            }
        });
    });

    notificationService.on("disconnectUser", function (userId) {

        $("#name_other_" + userId).html($("#name_other_" + userId).html() + '<div class="appear"></div>');
        DisabledOnlineUser(userId);
    });

    //Show Success Toast
    function showSuccessToast(message) {
        $().toastmessage('showToast', {
            text: message,
            sticky: false,
            stayTime: 10000,
            position: 'top-right',
            type: 'success',
            closeText: '',
            close: function () {
                console.log("toast is closed ...");
            }
        });
    }

    //Configuration Cookies
    function setCookie(c_name, value, exdays) {
        var exdate = new Date();
        exdate.setDate(exdate.getDate() + exdays);
        var c_value = escape(value) + ((exdays == null) ? "" : "; expires=" + exdate.toUTCString());
        document.cookie = c_name + "=" + c_value;
    }

    function createCookie(name, value, days) {
        var expires = "";
        if (days) {
            var date = new Date();
            date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
            expires = "; expires=" + date.toGMTString();
        }
        document.cookie = name + "=" + value + expires + "; path=/";
    }

    function getCookie(c_name) {
        var i, x, y, cookies = document.cookie.split(";");
        for (i = 0; i < cookies.length; i++) {
            x = cookies[i].substr(0, cookies[i].indexOf("="));
            y = cookies[i].substr(cookies[i].indexOf("=") + 1);
            x = x.replace(/^\s+|\s+$/g, "");
            if (x == c_name) {
                return unescape(y);
            }
        }
    }

    function readCookie(name) {
        var nameEQ = name + "=";
        var ca = document.cookie.split(';');
        for (var i = 0; i < ca.length; i++) {
            var c = ca[i];
            while (c.charAt(0) == ' ') c = c.substring(1, c.length);
            if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
        }
        return null;
    }

    function eraseCookie(name) {
        document.cookie = name + '=; Max-Age=0';
    }

    function SetNotificationCookies(count) {
        var notificationCooies = readCookie("NotificationCount");
        if (notificationCooies == null) {
            createCookie("NotificationCount", "0", 10);
            return false;
        }
        setCookie("NotificationCount", count, 10);
    }

    function CheckNotificationCookies() {
        var notificationCooies = readCookie("NotificationCount");
        if (notificationCooies == null) {
            createCookie("NotificationCount", "0", 10);
            return false;
        }
        return true;
    }

    //Manage notification
    function GetInitialNotifications() {
        if (signalROptions.PageIndex == 0)
            return false;

        $.ajax({
            type: 'get',
            url: signalROptions.GetNotificationsUrl,
            data: { index: signalROptions.PageIndex, pageSize: signalROptions.NotificationPageSize, isRead: true },
            Async: true,
            global: false,
            success: function (data) {
                if (data == undefined) {
                    signalROptions.PageIndex = 0;
                    return;
                }
                $("#singnalRMessage").html(data.Html);
                $(".redBullon").show();
                $(".redBullon").html(data.Count);
                if (data.Count > 0) {
                    $('#dropdownMenuLink').addClass('white-notification');
                }
                else {
                    $('#dropdownMenuLink').removeClass('white-notification');
                }
                //signalROptions.PageIndex = signalROptions.PageIndex + 1;
            }
        });
    }

    function SetNotification(text) {
        var htmlNotification = $(".informer-danger").html();
        var notificationCount = parseInt(htmlNotification);
        notificationCount = notificationCount + 1;
        $(".redBullon").html(notificationCount);
        $(".redBullon").show();
        if (notificationCount > 0) {
            $('#dropdownMenuLink').addClass('white-notification');
        }
        else {
            $('#dropdownMenuLink').removeClass('white-notification');
        }

        SetNotificationCookies(notificationCount);
        $("#singnalRMessage").prepend(text);
    }

    function ShowNotification() {
        if (!CheckNotificationCookies()) {
            return false;
        }
        var notificationCount = getCookie("NotificationCount");

        $(".informer-danger").html(notificationCount);

        notificationCount = parseInt(notificationCount);

        if (notificationCount <= 0) {
            setCookie("NotificationCount", "0", 10);
            return false;
        }
        $(".redBullon").show();
        if (notificationCount > 0) {
            $('#dropdownMenuLink').addClass('white-notification');
        }
        else {
            $('#dropdownMenuLink').removeClass('white-notification');
        }
    }

    function BrowseNotifications(element) {
        AllowSend = true;
        setCookie("NotificationCount", "0", 10);
        $(".redBullon").hide();
        $(".redBullon").html("0");
        $('#dropdownMenuLink').removeClass('white-notification');
        GetInitialNotifications();
    }

    //Connecting the client to the signalr hub
    connection.start({ jsonp: true })
        .done(function () {
            console.log("Connecting to the signalr server start");
            debugger;
            notificationService.invoke("onConnected", signalROptions.UserId, signalROptions.UserName, signalROptions.OrgUnitId);

            $(document).on("click", ".onlineUser", function () {
                var userId = $(this).data("online-userid");

                if (IsUserChatWindowOpened(userId))
                    return;

                var notificationCount = GetUserNotifysCount(userId);

                ResetUserNotifysCount(userId);

                DecreaseChatNotification(notificationCount);

                var userName = $(this).data("online-username");

                GetIntitialChatHistory(UrlInitChatData, userId, userName, function () {

                    SetChatBinding(userId, SelectedTransactionId);
                    SelectedTransactionId = null;
                });
                notificationService.invoke("AddChatWindow", userId);
            });

            $(document).on("click", ".close_conversation", function () {
                var userId = $(this).data("userid-close-chat-window");
                $('*[data-userid-chat-window="' + userId + '"]').hide();
                notificationService.invoke("RemoveChatWindow", userId);
            });

            $(document).on("keypress", ".TransactionRowMessage", function () {
                var key = window.event.keyCode;

                var textValue = $(this).val();

                textValue = textValue.trim();

                if (key == 13 && textValue != "") {
                    var userId = $(this).data("userid-chat-window");
                    var transactionId = $(this).data("transactionId");
                    notificationService.invoke("onSendChatMessage", userId, textValue, transactionId);
                    var instanceMessageDesign = CreateInstanceMessageDesign(signalROptions.UserName, textValue, signalROptions.LocalDateTime);
                    AddInstanceMessageToChatWindow(userId, instanceMessageDesign);
                    $(this).val("");
                    SetScrollToTop("stage_chat_userId_" + userId);
                }
            });

            $(document).on("keypress", ".messageRow", function () {
                var key = window.event.keyCode;
                var textValue = $(this).val();
                textValue = textValue.trim();
                if (key == 13 && textValue != "") {
                    var userId = $(this).data("userid-chat-window");
                    notificationService.invoke("onSendChatMessage", userId, textValue, null);
                    var instanceMessageDesign = CreateInstanceMessageDesign(signalROptions.UserName, textValue, signalROptions.LocalDateTime);
                    AddInstanceMessageToChatWindow(userId, instanceMessageDesign);
                    $(this).val("");
                    SetScrollToTop("stage_chat_userId_" + userId);
                }
            });

            ShowNotification();

            GetInitialNotifications();

            $('#dropdownMenuLink').on("click", function () { BrowseNotifications(this); });
        })
        .fail(function () {
            console.log("failed in connecting to the signalr server");
        });
}());