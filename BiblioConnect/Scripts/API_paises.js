var ciudad;  //VARIABLE GLOBAL

function insertarCiudadesSelect2() {
     return new Promise(function(resolve, reject) {
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
                console.log(selectedState)
                getCities(auth_token, selectedState, '#CiudadBiblio');
            });
            resolve();
        },
        error: function (error) {
            console.log("Error al obtener el access token:", error);
            reject(error);
        }
    });
     });
}
function insertarCiudadesSelect(estadoHTML, ciudadHTML) {
        $.ajax({
            url: "https://www.universal-tutorial.com/api/getaccesstoken",
            type: "GET",
            headers: {
                "Accept": "application/json",
                "api-token": "4TtTboEhH8Qb-Xx3QHOeacz4RiEenMI7rqHDdjZt4oeosXPWeEsARjPVDgI5p1iaTys",
                "user-email": "nsantosbalterra@gmail.com"
            },
            success: function (response) {
                var auth_token = response.auth_token;
                var selectedState = $(estadoHTML).find("option:selected").text();
                getCities(auth_token, selectedState, ciudadHTML);

                $(estadoHTML).change(function () {
                    selectedState = $(this).find("option:selected").text();
                    console.log(selectedState)
                    getCities(auth_token, selectedState, ciudadHTML);
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
            $("<option>").attr({ "value": "" }).text("Seleccione una opci\u00F3n").prop("selected", true).appendTo(ciudadHTML);
            data.forEach(function (city, index) {
                $("<option>").attr({ "value": city.city_name }).text(city.city_name).appendTo(ciudadHTML);
            });
            if (getCiudad() !== "") {
                $(ciudadHTML).val(getCiudad());
            }
        },
        error: function (error) {
            console.log("Error al obtener la lista de ciudades:", error);
        }
    });
}

function setCiudad(ciudadValue) {
    ciudad = ciudadValue
}

function getCiudad() {
    return ciudad;
}