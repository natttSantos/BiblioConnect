
document.addEventListener("DOMContentLoaded", function () {
    const registroLectorModal = document.querySelector("#registroModal form");
    const registroBiblioModal = document.querySelector("#registroBiblioModal form");
    console.log(registroLectorModal)
    //LECTOR
    if (registroLectorModal ) {
        registroLectorModal.addEventListener("submit", function (e) {
            e.preventDefault();

            const nombreValido = validarNombre("nombre-field");
            const emailValido = validarEmail("email-field");
            const apellidosValido = validarApellidos("apellidos-field");
            const calleValido = validarCalle("calle-field");
            const codPostalValido = validarCodPostal("codPostal-field");
            const passValido = validarPassword("password-field", "confirmPassword-field");
            const dniValido = validarDni("dni-field");
            const tlfValido = validarTlf("tlf-field");
            const estadoValido = validarEstado("estado-field");
            const ciudadValido = validarCiudad("ciudad-field");
            const fechaValido = validarFecha("fecha-field");
            const imagenValido = validarImagen("imagen-field", "#imglector");

            if (nombreValido && emailValido && apellidosValido && calleValido && codPostalValido && passValido
                && dniValido && tlfValido && estadoValido && ciudadValido && fechaValido && imagenValido) {
                GuardarLector();
            }
        });
    }

    //BIBLIOTECA
    registroBiblioModal.addEventListener("submit", function (e) {
        e.preventDefault();
        console.log("registro modal")
        const nombreValido = validarNombre("nombreBiblio-field");
        const descripcionValido = validarDescripcion("descripcionBiblio-field");
        const emailValido = validarEmail("emailBiblio-field");
        const calleValido = validarCalle("calleBiblio-field");
        const codPostalValido = validarCodPostal("codPostalBiblio-field");
        const passValido = validarPassword("passwordBiblio-field", "confirmPasswordBiblio-field");
        const tlfValido = validarTlf("tlfBiblio-field");
        const estadoValido = validarEstado("estadoBiblio-field");
        const ciudadValido = validarCiudad("ciudadBiblio-field");
        const imagenValido = validarImagen("imagenBiblio-field", "#imgBiblio");

        if (nombreValido && emailValido && calleValido && codPostalValido && passValido
            && tlfValido && estadoValido && ciudadValido && imagenValido && descripcionValido) {
            GuardarBiblioteca();
        }
    });

    //Hide and show password
    const eyeIcons = document.querySelectorAll(".show-hide");

    eyeIcons.forEach((eyeIcon) => {
        eyeIcon.addEventListener("click", () => {
            const pInput = eyeIcon.parentElement.querySelector("input");
            if (pInput.type === "password") {
                eyeIcon.classList.replace("bx-hide", "bx-show");
                return pInput.type = "text"; 
            }
            eyeIcon.classList.replace("bx-show", "bx-hide");
            return pInput.type = "password";
        });
    }); 
});


function validarNombre(field) {
    const nombreField = document.getElementById(field);
    const nombreInput = nombreField.querySelector(".form-control");
    if (nombreInput.value.trim() === "") {
        nombreField.classList.add("invalid");
        return false;
    } else {
        nombreField.classList.remove("invalid");
        return true;
    }
}
function validarApellidos(field) {
    const apellidosField = document.getElementById(field);
    const apellidosInput = apellidosField.querySelector(".form-control");
    if (apellidosInput.value.trim() === "") {
        apellidosField.classList.add("invalid");
        return false;
    } else {
        apellidosField.classList.remove("invalid");
        return true;
    }
}
function validarDni(field) {
    const dniField = document.getElementById(field);
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
function validarEmail(field) {
    const emailField = document.getElementById(field);
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

function validarDescripcion(field) {
    const descripcionField = document.getElementById(field);
    const descripcionInput = descripcionField.querySelector(".form-control");
    if (descripcionInput.value.trim() === "") {
        descripcionField.classList.add("invalid");
        return false;
    } else {
        descripcionField.classList.remove("invalid");
        return true;
    }
}
function validarTlf(field) {
    const tlfField = document.getElementById(field);
    const tlfInput = tlfField.querySelector(".form-control");

    if (tlfInput.value.trim() === "" || tlfInput.value.length !== 9) {
        tlfField.classList.add("invalid");
        return false;
    } else {
        tlfField.classList.remove("invalid");
        return true;
    }
}
function validarEstado(field) {
    const estadoField = document.getElementById(field);
    const estadoInput = estadoField.querySelector(".form-select");
    if (estadoInput.value.trim() === "Seleccione una opcion") {
        estadoField.classList.add("invalid");
        return false;
    } else {
        estadoField.classList.remove("invalid");
        return true;
    }
}
function validarCiudad(field) {
    const ciudadField = document.getElementById(field);
    const ciudadInput = ciudadField.querySelector(".form-select");
    if (ciudadInput.value == "Seleccione una opcion" || ciudadInput.value.trim() === "") {
        ciudadField.classList.add("invalid");
        return false;
    } else {
        ciudadField.classList.remove("invalid");
        return true;
    }
}
function validarCalle(field) {
    const calleField = document.getElementById(field);
    const calleInput = calleField.querySelector(".form-control");

    if (calleInput.value.trim() === "") {
        calleField.classList.add("invalid");
        return false;
    } else {
        calleField.classList.remove("invalid");
        return true;
    }
}
function validarCodPostal(field) {
    const codPostalField = document.getElementById(field);
    const codPostalInput = codPostalField.querySelector(".form-control");

    console.log(codPostalInput.value.length)
    if (codPostalInput.value.trim() === "" || codPostalInput.value.length !== 5) {
        codPostalField.classList.add("invalid");
        return false;
    } else {
        codPostalField.classList.remove("invalid");
        return true;
    }
}
function validarFecha(field) {
    const fechaField = document.getElementById(field);
    const fechaInput = fechaField.querySelector(".form-control");

    if (fechaInput.value.trim() === "") {
        fechaField.classList.add("invalid");
        return false;
    } else {
        fechaField.classList.remove("invalid");
        return true;
    }
}
function validarPassword(fieldpass, fieldconfirm) {
    const passwordField = document.getElementById(fieldpass);
    const confirmPasswordField = document.getElementById(fieldconfirm);
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
function validarImagen(field, imgId) {
    const imagenField = document.getElementById(field);
    const imgSrc = $(imgId).attr("src");

    if (imgSrc.trim() === "") {
        imagenField.classList.add("invalid");
        return false;
    } else {
        imagenField.classList.remove("invalid");
        return true;
    }
}