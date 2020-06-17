$(document).ready(function () {
    $("#spinner").hide();
    $(".usr-chat").click(function () {
        let th = $(this);
        let id_usr = $(this).attr("data-id");
        $(".usr-chat").each(function () {
            $(this).removeClass("active");
        })
        $("#btnSendAdmin").attr("data-id", id_usr);
        $(this).addClass("active");
        let noReading = $(this).attr("data-noreading");
        if (noReading > 0) {
            $.ajax({
                url: "/Chat/MarcarLeidosAdmin",
                data: {
                    id: id_usr
                },
                type: 'POST',
                success: function (data) {
                    th.attr("data-noreading", 0);
                    th.children(".msj-no-read").html("0");
                }
            });
        }
        $.ajax({
            url: "/Chat/GetMensajesUser",
            data: {
                id: id_usr
            },
            type: 'GET',
            dataType: "json",
            beforeSend: function () {
                $("#spinner").show();
            },
            success: function (data) {
                let contenido = $("#msgs");
                contenido.html("");
                let msjs = data.data;
                for (var i in msjs) {
                    let cadena = "";
                    if (msjs[i].tipo_mensaje == 1) {
                        cadena = '<div class="msg-env"><p>' + msjs[i].mensaje + '</p></div>';
                    } else {
                        cadena = '<div class="msg-resp"><p>' + msjs[i].mensaje + '</p></div>';
                    }
                    contenido.append(cadena);
                }
            },
            complete: function () {
                $("#spinner").hide();
            }
        });
    });

    $("#msgChatAdmin").on("keypress", function (e) {
        if ((e.keyCode || e.which) === 13) {
            e.preventDefault();
            sendMessageAdmin();
        }
    });

    $("#btnSendAdmin").on("click", function () {
        sendMessageAdmin();
    });


});

function sendMessageAdmin() {
    let m = $("#msgChatAdmin").val().trim();
    if (m === "") {
        $("#msgChatAdmin").parent().addClass("has-error");
    } else {
        let id_usr = $("#btnSendAdmin").attr("data-id");
        $.ajax({
            data: {
                mensaje: m,
                id: id_usr
            },
            type: 'POST',
            url: "/Chat/EnviarMensajeAdmin",
            success: function (data) {
                let men = '<div class="msg-env"><p>' + m + '</p></div>';
                $("#msgs").append(men);
                $("#msgChatAdmin").val("");
            }
        })
    }
}