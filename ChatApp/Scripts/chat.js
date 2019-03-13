'use strict';

var Chat = function () {
    this.initialize = function () {
        var messageBody = document.querySelector('.msg_history');
        messageBody.scrollTop = messageBody.scrollHeight - messageBody.clientHeight;
        initSignalR();
        registerEvents();
    };

    var initSignalR = function () {
        // Reference the auto-generated proxy for the hub.
        var chat = $.connection.chatHub;
        // Create a function that the hub can call back to display messages.
        chat.client.receiveMessage = function (name, message) {
            var html = '';
            if ($('#hidUserName').val() === name) {
                html = `<div class="outgoing_msg">
                        <div class="sent_msg">
                            <div>${message}</div>
                            <span class="time_date">${moment().format('DD/MM/YYYY, h:mm:ss A')}</span>
                        </div>
                    </div>`;

                $('.msg_history').append(html);
                var div = $(".msg_history");
                div.scrollTop(div.prop('scrollHeight'));
            } else {
                html = `<div class="incoming_msg">
                            <div class="incoming_msg_img"> <img src="https://ptetutorials.com/images/user-profile.png" alt="sunil"> </div>
                            <div class="received_msg">
                                <div class="received_withd_msg">
                                    <div>${message}</div>
                                    <span class="time_date">${name} - ${moment().format('DD/MM/YYYY, h:mm:ss A')}</span>
                                </div>
                            </div>
                        </div>`;

                $('.msg_history').append(html);
                if (isQuiteBottom()) {
                    gotoBottom();
                }
            }
        };

        chat.client.joined = function (arr) {
            var html = '';
            $.each(arr, function (i, item) {
                html += `<li id="li${item}">
                            <div class="user-online">
                                <img src="https://ptetutorials.com/images/user-profile.png" alt="sunil">
                                <span>${item}</span>
                            </div>
                        </li>`;
            });

            $('#listUserOnline').html('');
            $('#listUserOnline').html(html);
        };

        chat.client.leaved = function (name, message) {
            $(`#li${name}`).remove();
        };

        chat.client.typing = function (name, message) {
            var html = '';

            var checkIsBottom = isBottom();

            if (message !== '') {
                $(`#div${name}`).remove();
                html += `<div class="incoming_msg" id="div${name}">
                            ${name} đang nhập...
                        </div>`;
            } else {
                $(`#div${name}`).remove();
            }

            $('.msg_history').append(html);
            if (checkIsBottom) {
                gotoBottom();
            }
        };

        // Get the user name and store it to prepend to messages.
        $('#displayname').val(name);
        // Set initial focus to message input box.
        $('#message').focus();
        // Start the connection.
        $.connection.hub.start().done(function () {
            $('#sendmessage').click(function () {
                if ($('#message').val()) {
                    chat.server.sendMessage($('#hidUserName').val(), $('#message').val());
                    $('#message').val('').focus();
                    return true;
                }
                return false;
            });

            $('#message').on('keypress', function (e) {
                if (e.which === 13) {
                    if ($(this).val()) {
                        chat.server.sendMessage($('#hidUserName').val(), $(this).val());
                        $(this).val('').focus();
                        return true;
                    }
                    return false;
                }
            });

            $('#message').on('keyup', function (e) {
                chat.server.typing($('#hidUserName').val(), $(this).val());
            });
        });
    };

    var isBottom = function () {
        var current = Math.round($('.msg_history').scrollTop() + $('.msg_history').innerHeight(), 10);
        var total = Math.round($('.msg_history')[0].scrollHeight, 10);

        if (total === current) {
            return true;
        }

        return false;
    };

    var isQuiteBottom = function () {
        var current = Math.round($('.msg_history').scrollTop() + $('.msg_history').innerHeight(), 10);
        var total = Math.round($('.msg_history')[0].scrollHeight, 10);

        if (total - current <= 85) {
            return true;
        }

        return false;
    };

    var gotoBottom = function () {
        var div = $(".msg_history");
        div.scrollTop(div.prop('scrollHeight'));
    };

    var registerEvents = function () {
        $('#btnBottom').click(function () {
            gotoBottom();
        });
    };
};
