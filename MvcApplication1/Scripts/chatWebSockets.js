var socket,
           $txt = document.getElementById('submit_message'),
           $user = 'user6',//ВНИМАНИЕ! ЗАМЕНИТЬ!
           $messages = document.getElementById('messages');

if (typeof (WebSocket) !== 'undefined') {
    socket = new WebSocket("ws://localhost:59521/ChatHandler.ashx");
} else {
    socket = new MozWebSocket("ws://localhost:59521/ChatHandler.ashx");
}
socket.onmessage = function (msg) {
    var $el = document.createElement('p');
    var str = msg.data;
    var n = str.indexOf("}");
    var subStr = str.substring(0, n + 1);
    var obj = JSON.parse(subStr);

    if (fromUser == obj.FromUser) {
        $('#messages').append('<div class="chat_message_wrapper chat_message_right">' +
                '<div class="chat_user_avatar">' +
                    '<a href="#" target="_blank">' +
                       ' <img alt="' + fromUser + '" title="' + fromUser + '" src="' + userphotoImg + '" class="md-user-image">' +
                    '</a>' +
                '</div>' +
               ' <ul class="chat_message">' +
                   ' <li>' +
                       ' <p>' +
                            obj.Message +
                            '<span class="chat_message_time">' + obj.Time + '</span>' +
                        '</p>' +
                   ' </li>' +
               ' </ul>' +
            '</div>');
    }
    else {
        $('#messages').append('<div class="chat_message_wrapper">' +
                '<div class="chat_user_avatar">' +
                    '<a href="#" target="_blank">' +
                       '  <img alt="' + toUser + '" title="' + toUser + '" src="' + userphotoImg + '" class="md-user-image">' +
                    '</a>' +
                '</div>' +
               ' <ul class="chat_message">' +
                   ' <li>' +
                       ' <p>' +
                            obj.Message +
                            '<span class="chat_message_time">' + obj.Time + '</span>' +
                        '</p>' +
                   ' </li>' +
               ' </ul>' +
            '</div>');
    }
};

socket.onclose = function (event) {
    alert('Мы потеряли её. Пожалуйста, обновите страницу');
};

$('.md-user-image').attr('src', toUserImg);
$('.md-user-image').attr('alt', toUser);
$('.md-user-image').attr('title', toUser);
$('.pull-left').find('#Name').text(toUser);
$('#linkUser').attr('href', 'http://localhost:59521/Profile/UserPreview?user=' + toUser);

var d = new Date();

document.getElementById('send').onclick = function () {
    
    var data = {
        "FromUser": fromUser,
        "Message": $('#submit_message').val(),
        "Time": d.toString(),
        "ToUser": toUser
    };
    console.log(JSON.stringify(data));
    var str = JSON.stringify(data);
    socket.send(str);
    
    $txt.value = '';
};