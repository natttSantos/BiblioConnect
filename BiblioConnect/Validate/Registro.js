
document.addEventListener("DOMContentLoaded", function () {
    const form = document.querySelector("#registroModal form");

    function validarNombre() {
        const nombreField = document.getElementById("nombre-field");
        const nombreInput = nombreField.querySelector(".form-control");
        if (nombreInput.value.trim() === "") {
            nombreField.classList.add("invalid");
            return false;
        } else {
            nombreField.classList.remove("invalid");
            return true;
        }
    }
    function validarApellidos() {
        const apellidosField = document.getElementById("apellidos-field");
        const apellidosInput = apellidosField.querySelector(".form-control");
        if (apellidosInput.value.trim() === "") {
            apellidosField.classList.add("invalid");
            return false;
        } else {
            apellidosField.classList.remove("invalid");
            return true;
        }
    }
    function validarDni() {
        const dniField = document.getElementById("dni-field");
        const dniInput = dniField.querySelector(".form-control");
        const pattern = /^\d{8}[a-zA-Z]$/;

        if (!dniInput.value.match(pattern)) {
            dniField.classList.add("invalid");
            return false;
        } else {
            dniField.classList.remove("invalid");
            return true;
        }
    }
    function validarEmail() {
        const emailField = document.getElementById("email-field");
        const emailInput = emailField.querySelector(".form-control");
        const pattern = /^([a-zA-Z0-9_\-.]+)@([a-zA-Z0-9_\-.]+)\.([a-zA-Z]{2,5})$/;

        if (!emailInput.value.match(pattern)) {
            emailField.classList.add("invalid");
            return false;
        } else {
            emailField.classList.remove("invalid");
            return true;
        }
    }
    function validarTlf() {
        const tlfField = document.getElementById("tlf-field");
        const tlfInput = tlfField.querySelector(".form-control");

        if (tlfInput.value.trim() === "" || tlfInput.value > 9 || tlfInput < 9) {
            tlfField.classList.add("invalid");
            return false;
        } else {
            tlfField.classList.remove("invalid");
            return true;
        }
    }
    function validarEstado() {
        const estadoField = document.getElementById("estado-field");
        const estadoInput = estadoField.querySelector(".form-select");
        if (estadoInput.value.trim() === "Seleccione una opcion") {
            estadoField.classList.add("invalid");
            return false;
        } else {
            estadoField.classList.remove("invalid");
            return true;
        }
    }
    function validarCiudad() {
        const ciudadField = document.getElementById("ciudad-field");
        const ciudadInput = ciudadField.querySelector(".form-select");
        if (ciudadInput.value == "Seleccione una opcion" || ciudadInput.value.trim() === "") {
            ciudadField.classList.add("invalid");
            return false;
        } else {
            ciudadField.classList.remove("invalid");
            return true;
        }
    }
    function validarCalle() {
        const calleField = document.getElementById("calle-field");
        const calleInput = calleField.querySelector(".form-control");

        if (calleInput.value.trim() === "") {
            calleField.classList.add("invalid");
            return false;
        } else {
            calleField.classList.remove("invalid");
            return true;
        }
    }
    function validarCodPostal() {
        const codPostalField = document.getElementById("codPostal-field");
        const codPostalInput = codPostalField.querySelector(".form-control");

        if (codPostalInput.value.trim() === "" || codPostalInput.value > 5 || codPostalInput < 5) {
            codPostalField.classList.add("invalid");
            return false;
        } else {
            codPostalField.classList.remove("invalid");
            return true;
        }
    }
    function validarFecha() {
        const fechaField = document.getElementById("fecha-field");
        const fechaInput = fechaField.querySelector(".form-control");

        if (fechaInput.value.trim() === "") {
            fechaField.classList.add("invalid");
            return false;
        } else {
            fechaField.classList.remove("invalid");
            return true;
        }
    }
    function validarPassword() {
        const passwordField = document.getElementById("password-field");
        const confirmPasswordField = document.getElementById("confirmPassword-field");
        const passwordInput = passwordField.querySelector(".form-control");
        const confirmPasswordInput = confirmPasswordField.querySelector(".form-control");
        const errorElement = passwordField.querySelector(".error-text");

        if (passwordInput.value != confirmPasswordInput.value) {
            passwordField.classList.add("invalid");
            errorElement.textContent = "Las contrase\u00F1as no coinciden";
            return false;
        } if (passwordInput.value.trim() === "" || confirmPasswordInput.value.trim() === "") {
            passwordField.classList.add("invalid");
            errorElement.textContent = "Introduce una contrase\u00F1a";
            return false;
        }
        else if (passwordInput.value == confirmPasswordInput.value) {
            passwordField.classList.remove("invalid");
            return true;
        }
    }
    function validarImagen() {
        const imagenField = document.getElementById("imagen-field");
        const imagenInput = imagenField.querySelector(".form-control");

        if (imagenInput.value.trim() === "") {
            imagenField.classList.add("invalid");
            return false;
        } else {
            imagenField.classList.remove("invalid");
            return true;
        }
    }
    // Evento de envío del formulario
    form.addEventListener("submit", function (e) {
        e.preventDefault();

        // Realizar las validaciones necesarias antes de enviar el formulario
        const nombreValido = validarNombre();
        const emailValido = validarEmail();
        const apellidosValido = validarApellidos();
        const calleValido = validarCalle();
        const codPostalValido = validarCodPostal();
        const passValido = validarPassword();
        const dniValido = validarDni();
        const tlfValido = validarTlf();
        const estadoValido = validarEstado();
        const ciudadValido = validarCiudad();
        const fechaValido = validarFecha();
        const imagenValido = validarImagen();

        if (nombreValido && emailValido && apellidosValido && calleValido && codPostalValido && passValido
            && dniValido && tlfValido && estadoValido && ciudadValido && fechaValido && imagenValido) {
            form.submit();
        }
    });
});
