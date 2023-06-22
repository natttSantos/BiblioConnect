
function getDate(fechaUnix) {
    var fechaMilisegundos = parseInt(fechaUnix.substr(6)); // Extraer los milisegundos
    var fechaActual = new Date(fechaMilisegundos);

    var year = fechaActual.getFullYear(); // Obtener el año
    var mounth = String(fechaActual.getMonth() + 1).padStart(2, "0"); // Obtener el mes (se agrega 1 porque los meses en JavaScript son base 0) y se agrega el relleno '0' si es necesario
    var day = String(fechaActual.getDate()).padStart(2, "0"); // Obtener el día y se agrega el relleno '0' si es necesario

    var fechaFormateada = `${day}/${mounth}/${year}`;
    console.log(fechaFormateada); // Resultado: "11-07-2023"
    return fechaFormateada; 
}
function addDaysToDate(fechaUnix, numDias) {
    var fechaMilisegundos = parseInt(fechaUnix.substr(6)); // Extraer los milisegundos
    var fechaActual = new Date(fechaMilisegundos);

    // Sumar los días a la fecha actual
    fechaActual.setDate(fechaActual.getDate() + numDias);

    var year = fechaActual.getFullYear(); // Obtener el año
    var month = String(fechaActual.getMonth() + 1).padStart(2, "0"); // Obtener el mes (se agrega 1 porque los meses en JavaScript son base 0) y se agrega el relleno '0' si es necesario
    var day = String(fechaActual.getDate()).padStart(2, "0"); // Obtener el día y se agrega el relleno '0' si es necesario

    var fechaFormateada = `${day}/${month}/${year}`;
    console.log(fechaFormateada); // Resultado: "11/07/2023"
    return fechaFormateada;
}
