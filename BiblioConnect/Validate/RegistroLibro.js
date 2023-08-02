
document.addEventListener("DOMContentLoaded", function () {
    const registroModal = document.querySelector("#registroLibroModal form");

    //BIBLIOTECA
    registroModal.addEventListener("submit", function (e) {
        const botonType = document.querySelector(".btn-guardar");
        const action = botonType.dataset.action;

        e.preventDefault();
        const tituloValido = validarInput("titulo-field");
        const nombreValido = validarInput("autor-field");
        const ISBNValido = validarInputMaxLength("ISBN-field", 13);
        const EditorialValido = validarInput("editorial-field");
        const EjemplaresValido = validarInput("ejemplares-field");
        const UbicacionValido = validarInput("ubicacion-field");
        const CategoriaValido = validarSelect("categoria-field");
        const IdiomaValido = validarSelect("idioma-field");
        const ImagenValido = validarImagen("imagen-field");

        if (nombreValido && tituloValido && ISBNValido && EditorialValido && EjemplaresValido
            && UbicacionValido && CategoriaValido && IdiomaValido && ImagenValido) {
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
    const imgSrc = $("#imglibro").attr("src");
    console.log("entra")

    if (imgSrc.trim() === "") {
        imagenField.classList.add("invalid");
        return false;
    } else {
        imagenField.classList.remove("invalid");
        return true;
    }
}