
function getLatLong(Calle, Ciudad, Estado, Pais, CodPostal) {
    $.ajax({
        url: "https://nominatim.openstreetmap.org/search.php",
        data: {
            street: Calle,
            city: Ciudad,
            state: Estado,
            country: Pais,
            postalcode: CodPostal,
            format: "jsonv2"
        },
        type: "GET",
        success: function (data) {
            var latitud = data[0].lat;
            var longitud = data[0].lon;
            mostrarMapa(latitud, longitud); 
        },
        error: function (xhr, status, error) {
            console.log("Error:", error);
        }
    });
}
function mostrarMapa(latitud, longitud) {
    var map = L.map('map').setView([latitud, longitud], 17);
    L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
        maxZoom: 19,
        attribution: '&copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>'
    }).addTo(map);

    L.marker([latitud, longitud]).addTo(map)
        /*.bindPopup('A pretty CSS popup.<br> Easily customizable.')*/
        .openPopup();
}
