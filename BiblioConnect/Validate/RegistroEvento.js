
document.addEventListener("DOMContentLoaded", function () {
    const registroModal = document.querySelector("#registroEventoModal form");

    //BIBLIOTECA
    registroModal.addEventListener("submit", function (e) {
        const botonType = document.querySelector(".btn-guardar");
        const action = botonType.dataset.action;

        e.preventDefault();
        const AutorValido = validarInput("autor-field");
        const TipoValido = validarSelect("tipo-field");
        const FechaValido = validarInput("fecha-field");
        const HoraInicioValido = validarInput("horaInicio-field");
        const HoraFinValido = validarInput("horaFin-field");
        const DescricpionAutorValido = validarInput("descripcionAutor-field");
        const DescricpionEventoValido = validarInput("descripcionEvento-field");
        const ImagenValido = validarImagen("imagen-field");

        if (AutorValido && TipoValido && DescricpionAutorValido && DescricpionEventoValido && FechaValido
            && HoraInicioValido && HoraFinValido && ImagenValido) {
            if (action === "editar") {
                Modificar(); 
            } else if (action === "guardar") {
                Guardar();
            }
        }
    });
});

function validarInput(field) {
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
function validarInputMaxLength(field, maxLength) {
    const nombreField = document.getElementById(field);
    const nombreInput = nombreField.querySelector(".form-control");
    if (nombreInput.value.trim() === "" || nombreInput.value.length !== maxLength) {
        nombreField.classList.add("invalid");
        return false;
    } else {
        nombreField.classList.remove("invalid");
        return true;
    }
}
function validarSelect(field) {
    const selectField = document.getElementById(field);
    const selectInput = selectField.querySelector(".form-select");
    if (selectInput.value == "Seleccione una opcion" || selectInput.value.trim() === "") {
        selectField.classList.add("invalid");
        return false;
    } else {
        selectField.classList.remove("invalid");
        return true;
    }
}
function validarImagen(field) {
    const imagenField = document.getElementById(field);
    const imagenInput = imagenField.querySelector(".form-control");

    if (imagenInput.value.trim() === "") {
        imagenField.classList.add("invalid");
        return false;
    } else {
        imagenField.classList.remove("invalid");
        return true;
    }
}