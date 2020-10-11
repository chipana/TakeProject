function sendMessage() {
    var $message = $("#messageInput").val();
    socket.send($message);
    $("#messageInput").val("");
}

function appendMessage(message) {
    var $list = $("#messages");
    var $item = $("<li>");

    $item.text(message);
    $list.append($item);
}

$(document).ready(function () {
    var uri = "ws://localhost:5000/ws";
    socket = new WebSocket(uri);

    socket.onmessage = function (e) {
        appendMessage(event.data);
    };

    $("button#sendButton").on("click", sendMessage);

    $("input#messageInput").on("keyup", function (e) {
        //13 = Enter/Return
        if (e.keyCode == 13) {
            sendMessage();
        }
    });
});