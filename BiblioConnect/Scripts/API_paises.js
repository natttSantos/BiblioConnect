function insertarCiudadesSelect(){
    $.ajax({
        url: "https://www.universal-tutorial.com/api/getaccesstoken",
        type: "GET",
        headers: {
            "Accept": "application/json",
            "api-token": "4TtTboEhH8Qb-Xx3QHOeacz4RiEenMI7rqHDdjZt4oeosXPWeEsARjPVDgI5p1iaTys",
            "user-email": "nsantosbalterra@gmail.com"
        },
        success: function (response) {
            var selectedState;
            var auth_token = response.auth_token;

            $("#Estado").change(function () {
                selectedState = $(this).find("option:selected").text();
                getCities(auth_token, selectedState, '#Ciudad');
            });
            $("#EstadoBiblio").change(function () {
                selectedState = $(this).find("option:selected").text();
                getCities(auth_token, selectedState, '#CiudadBiblio');
            });
        },
        error: function (error) {
            console.log("Error al obtener el access token:", error);
        }
    });
}
function getCities(auth_token, selectedState, ciudadHTML) {
    $.ajax({
        url: "https://www.universal-tutorial.com/api/cities/" + selectedState,
        type: "GET",
        headers: {
            "Authorization": "Bearer " + auth_token,
            "Accept": "application/json"
        },
        success: function (data) {
            $(ciudadHTML).empty(); // Vaciar el elemento <select> antes de agregar nuevas opciones
            data.forEach(function (city, index) {
                $("<option>").attr({ "value": index }).text(city.city_name).appendTo(ciudadHTML);
            });
        },
        error: function (error) {
            console.log("Error al obtener la lista de ciudades:", error);
        }
    });
}
