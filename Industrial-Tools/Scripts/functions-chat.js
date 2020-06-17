$(document).ready(function () {
    $(".body-chat").hide();

    $(".chat-header").on("click", function () {
        if (ban) {
            $(".body-chat").show(500);
            $(".chat-dot-numb").hide();
            //$(".div-chat > .body-chat > .messages > div:last-child > p")[0].scrollIntoView({ behavior: "smooth", block: "end", inline: "nearest" });
            $(".div-chat > .body-chat > .messages > div:last-child > p")[0].scrollIntoView(false);
            let noReading = $(".chat-header").attr("data-noreading");
            if (noReading > 0) {
                $.ajax({
                    url: "/Chat/MarcarLeidos",
                    type: 'POST',
                    success: function (data) {
                        $(".chat-header").attr("data-noreading", 0);
                        $(".chat-dot-numb").html("0");
                    }
                });
            }
        } else {
            $(".body-chat").hide(500);
            $(".chat-dot-numb").show();
        }
        ban = !ban;
    });

    $("#msgChat").on("keypress", function (e) {
        if ((e.keyCode || e.which) === 13) {
            e.preventDefault();
            sendMessage();
        }
    });

    $("#btnSend").on("click", function () {
        sendMessage();
    });

    function sendMessage() {
        let m = $("#msgChat").val().trim();
        if (m === "") {
            $("#msgChat").parent().addClass("has-error");
        } else {
            $.ajax({
                data: {
                    mensaje: m
                },
                type: 'POST',
                url: "/Chat/EnviarMensaje",
                success: function (data) {
                    let men = '<div class="msg-env"><p>' + m + '</p></div>';
                    $("#mensajes").append(men);
                    $("#msgChat").val("");
                }
            })
        }
    }
});