
function getDate(fechaUnix) {
    var fechaMilisegundos = parseInt(fechaUnix.substr(6)); 
    var fechaActual = new Date(fechaMilisegundos);

    var year = fechaActual.getFullYear(); 
    var mounth = String(fechaActual.getMonth() + 1).padStart(2, "0"); 
    var day = String(fechaActual.getDate()).padStart(2, "0"); 

    var fechaFormateada = `${day}/${mounth}/${year}`;
    return fechaFormateada; 
}
function getDateFormatCalendar(fechaUnix) {
    var fechaMilisegundos = parseInt(fechaUnix.substr(6));
    var fechaActual = new Date(fechaMilisegundos);

    var year = fechaActual.getFullYear();
    var mounth = String(fechaActual.getMonth() + 1).padStart(2, "0");
    var day = String(fechaActual.getDate()).padStart(2, "0");

    var fechaFormateada = `${year}-${mounth}-${day}`;
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
    return fechaFormateada;
}

function formatDateToReadable(fechaUnix) {
    const daysOfWeek = ['Domingo', 'Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado'];
    const months = ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'];

    // Crear un objeto de fecha a partir del timestamp Unix
    var fechaMilisegundos = parseInt(fechaUnix.substr(6)); // Extraer los milisegundos
    var date = new Date(fechaMilisegundos);

    // Obtener el día de la semana, día del mes y mes
    const dayOfWeek = daysOfWeek[date.getDay()];
    const dayOfMonth = date.getDate();
    const month = months[date.getMonth()];

    // Construir la cadena de fecha en el formato deseado
    const formattedDate = `${dayOfWeek}, ${dayOfMonth} de ${month} de ${date.getFullYear()}`;
    return formattedDate;
}
