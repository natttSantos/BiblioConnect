
//document.addEventListener("DOMContentLoaded", function () {
//    const loginFormulario = document.querySelector("#LoginFormulario");

//    loginFormulario.addEventListener("submit", function (e) {
//        e.preventDefault();

//        const emailValido = validarEmail("email-field");
//        const passValido = validarPassword("password-field");

//        if (emailValido && passValido) {
//            console.log("credenciales ok")
//        }
//    });
//    function validarEmail(field) {
//        const emailField = document.getElementById(field);
//        const emailInput = emailField.querySelector(".form-control");
//        const errorElement = emailField.querySelector(".error-text");

//        if (emailInput.value.trim() === "") {
//            emailField.classList.add("invalid");
//            errorElement.textContent = "Introduce un email";
//            return false;
//        }
//         else {
//            emailField.classList.remove("invalid");
//            return true;
//        }
//    }
//    function validarPassword(fieldpass) {
//        const mensajeError = @Html.Raw(Json.Encode(ViewData["Mensaje"]));

//        const passwordField = document.getElementById(fieldpass);
//        const passwordInput = passwordField.querySelector(".form-control");
//        const errorElement = passwordField.querySelector(".error-text");

//        if (passwordInput.value.trim() === "") {
//            passwordField.classList.add("invalid");
//            errorElement.textContent = "Introduce una contrase\u00F1a";
//            return false;
//        } if (mensajeError !== "") {
//            passwordField.classList.add("invalid");
//            errorElement.textContent = mensajeError;
//            return false;
//        }
//        else {
//            passwordField.classList.remove("invalid");
//            return true;
//        }
//    }

//    //Hide and show password
//    const eyeIcons = document.querySelectorAll(".show-hide");

//    eyeIcons.forEach((eyeIcon) => {
//        eyeIcon.addEventListener("click", () => {
//            const pInput = eyeIcon.parentElement.querySelector("input");
//            if (pInput.type === "password") {
//                eyeIcon.classList.replace("bx-hide", "bx-show");
//                return pInput.type = "text"; 
//            }
//            eyeIcon.classList.replace("bx-show", "bx-hide");
//            return pInput.type = "password";
//        });
//    }); 
//});
