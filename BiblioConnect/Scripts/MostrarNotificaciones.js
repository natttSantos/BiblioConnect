
function actualizarNumeroNotificaciones(numero) {
    var countElement = document.getElementById("notificationCount");
    countElement.innerText = numero;
}

function ListarNotificaciones(idLector) {
    var notificationList = $("#notificationList");
    var numeroNotificaciones = 0;
    $.ajax({
        url: "/Biblioteca/ListarNotificacionPorLector",
        type: "GET",
        data: { id: idLector },
        success: function (data) {
            $.each(data.data, async function (index, notification) {
                numeroNotificaciones++;
                var listItem = $("<li>").addClass("dropdown-item").appendTo(notificationList);
                var mediaDiv = $("<div>").addClass("media").appendTo(listItem);
                var mediaBody = $("<div>").addClass("media-body").appendTo(mediaDiv);

                var libro = await obtenerImagenLibro(notification.idLibro);
                var image = $("<img>").attr("src", libro.Foto).addClass("mr-3").attr("alt", "Avatar").appendTo(mediaDiv);

                var biblioteca = await getBiblioteca(notification.idBiblioteca);
                $("<h5>").addClass("mt-0").text(biblioteca.Nombre).appendTo(mediaBody);

                $("<p>").text('El libro "' + libro.Titulo + '" ya esta disponible puede ir a recogerlo hoy mismo.').appendTo(mediaBody);
                $("<small>").text(getDate(notification.FechaEnvio)).appendTo(mediaBody);
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