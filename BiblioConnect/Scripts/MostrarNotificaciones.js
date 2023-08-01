
function actualizarNumeroNotificaciones(numero) {
    var countElement = document.getElementById("notificationCount");
    countElement.innerText = numero;
}

function ListarNotificaciones(idLector) {
    var notificationList = $("#notification-ui_dd-content");
    var numeroNotificaciones = 0;
    $.ajax({
        url: "/Biblioteca/ListarNotificacionPorLector",
        type: "GET",
        data: { id: idLector },
        success: function (data) {
            $.each(data.data, async function (index, notification) {
                numeroNotificaciones++;
                var listItem = $("<div>").addClass("notification-list notification-list--unread").appendTo(notificationList);
                var listItemContent = $("<div>").addClass("notification-list_content").appendTo(listItem);
                var listItemImage = $("<div>").addClass("notification-list_img").appendTo(listItemContent);
                var listItemDetail = $("<div>").addClass("notification-list_detail").appendTo(listItemContent);

                var libro = await obtenerImagenLibro(notification.idLibro);
                var image = $("<img>").attr("src", libro.Foto).attr("alt", "Avatar").appendTo(listItemImage);

                var biblioteca = await getBiblioteca(notification.idBiblioteca);
                $("<p>").text(biblioteca.Nombre).addClass("titulo").appendTo(listItemDetail);

                $("<p>").text('El libro "' + libro.Titulo + '" ya esta disponible puede ir a recogerlo hoy mismo.').addClass("descripcionNotificacion").appendTo(listItemDetail);
                $("<p>").text(getDate(notification.FechaEnvio)).addClass("fecha").appendTo(listItemDetail);

            });
            if (numeroNotificaciones === 0) {
                var listItem = $("<li>").addClass("dropdown-item").appendTo(notificationList);
                var mediaDiv = $("<div>").addClass("media").appendTo(listItem);
                var mediaBody = $("<div>").addClass("media-body").appendTo(mediaDiv);
                $("<p>").text("No hay notificaciones pendientes").addClass("descripcionNotificacion").appendTo(mediaBody);
            }
            actualizarNumeroNotificaciones(numeroNotificaciones);
        },
        error: function (error) {
            console.log("Error al obtener las notificaciones:", error);
        }
    });
}
function ContarNotificaciones(idLector) {
    var numeroNotificaciones = 0;
    $.ajax({
        url: "/Biblioteca/ListarNotificacionPorLector",
        type: "GET",
        data: { id: idLector },
        success: function (data) {
            $.each(data.data, async function (index, notification) {
                numeroNotificaciones++;              
            });
            actualizarNumeroNotificaciones(numeroNotificaciones);
        },
        error: function (error) {
            console.log("Error al obtener las notificaciones:", error);
        }
    });
}
function obtenerImagenLibro(idLibro) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: "/Lector/ObtenerLibro",
            type: "GET",
            data: { id: idLibro },
            success: function (data) {
                var libro = data.data;
                resolve(libro)
            },
            error: function (error) {
                console.log("Error al realizar el filtrado", error);
            }
        });
    });
}
function getBiblioteca(idBiblioteca) {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: "/Lector/ObtenerBiblio",
            type: "GET",
            data: { id: idBiblioteca },
            success: function (data) {
                var biblioteca = data.data;
                resolve(biblioteca);
            },
            error: function (error) {
                console.log("Error al obtener los datos de la Biblioteca:", error);
            }
        });
    });
}