
//Coordenadas de biblioteca
function getLatLong(Calle, Ciudad, Estado, Pais, CodPostal) {
    return new Promise(function (resolve, reject) {
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
                resolve([latitud, longitud]);
            },
            error: function (xhr, status, error) {
                console.log("Error:", error);
                reject(error);
            }
        });
    });
}

//Coordenadas de Usuario
function getUserCurrentPosition() {
    return new Promise(function (resolve, reject) {
        navigator.geolocation.getCurrentPosition(function (position) {
            var latitud = position.coords.latitude;
            var longitud = position.coords.longitude;
            console.log(latitud + " y " + longitud)
            resolve([latitud, longitud]);
        }, function (error) {
            console.log("Error al obtener la posición del usuario:", error);
            reject(error);
        });
    });
}

//LEAFLET
function mostrarMapa(latitud, longitud) {
    var map = L.map('map').setView([latitud, longitud], 17);
    L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
        maxZoom: 19,
        attribution: '&copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>'
    }).addTo(map);

    L.marker([latitud, longitud]).addTo(map)
/*        .bindPopup('A pretty CSS popup.<br> Easily customizable.')*/
        .openPopup();
}
function mostrarMapa_withdistance(latitud, longitud, mensajePopUp) {
    var map = L.map('map').setView([latitud, longitud], 17);
    L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
        maxZoom: 19,
        attribution: '&copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>'
    }).addTo(map);

    L.marker([latitud, longitud]).addTo(map)
        .bindPopup(mensajePopUp)
        .openPopup();
}

//CÁLCULO DE LA DISTANCIA (fórmula de Haversine)
function calculateDistance(lat1, lon1, lat2, lon2) {
    const earthRadiusKm = 6371.0;
    const dLat = toRadians(lat2 - lat1);
    const dLon = toRadians(lon2 - lon1);
    const a =
        Math.sin(dLat / 2) * Math.sin(dLat / 2) +
        Math.cos(toRadians(lat1)) *
        Math.cos(toRadians(lat2)) *
        Math.sin(dLon / 2) *
        Math.sin(dLon / 2);

    const c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1 - a));
    const distance = earthRadiusKm * c;
    return distance;
}

function toRadians(degrees) {
    return (degrees * Math.PI) / 180;
}