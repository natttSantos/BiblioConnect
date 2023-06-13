// Obtener la lista de países
function getCountries(auth_token) {
    $.ajax({
        url: "https://www.universal-tutorial.com/api/countries/",
        type: "GET",
        headers: {
            "Authorization": "Bearer " + auth_token,
            "Accept": "application/json"
        },
        success: function (data) {
            data.forEach(function (country, index) {
                $("<option>").attr({ "value": index }).text(country.country_name).appendTo("#Pais");
            });
        },
        error: function (error) {
            console.log("Error al obtener la lista de países:", error);
        }
    });
}
// Obtener la lista de estados en función del país seleccionado
function getStates(auth_token, selectedCountry) {
    $.ajax({
        url: "https://www.universal-tutorial.com/api/states/" + selectedCountry,
        type: "GET",
        headers: {
            "Authorization": "Bearer " + auth_token,
            "Accept": "application/json"
        },
        success: function (data) {
            data.forEach(function (state, index) {
                $("<option>").attr({ "value": index }).text(state.state_name).appendTo("#Estado");
            });
        },
        error: function (error) {
            console.log("Error al obtener la lista de estados:", error);
        }
    });
}
// Obtener la lista de ciudades en función del estado seleccionado
function getStates(auth_token, selectedCountry) {
    $.ajax({
        url: "https://www.universal-tutorial.com/api/states/" + selectedCountry,
        type: "GET",
        headers: {
            "Authorization": "Bearer " + auth_token,
            "Accept": "application/json"
        },
        success: function (data) {
            data.forEach(function (state, index) {
                $("<option>").attr({ "value": index }).text(state.state_name).appendTo("#Estado");
            });
        },
        error: function (error) {
            console.log("Error al obtener la lista de estados:", error);
        }
    });
}
// Obtener la lista de ciudades en función del estado seleccionado
function getCities(auth_token, selectedState) {
    $.ajax({
        url: "https://www.universal-tutorial.com/api/cities/" + selectedState,
        type: "GET",
        headers: {
            "Authorization": "Bearer " + auth_token,
            "Accept": "application/json"
        },
        success: function (data) {
            data.forEach(function (city, index) {
                $("<option>").attr({ "value": index }).text(city.city_name).appendTo("#Ciudad");
            });
        },
        error: function (error) {
            console.log("Error al obtener la lista de ciudades:", error);
        }
    });
}
