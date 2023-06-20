
function alertSuccess(titulo, subtitulo) {
    Swal.fire(titulo, subtitulo,'success')
}

//función que encapsula código y devuelva una promesa.
function alertConfirm(titulo, subtitulo) {
    return new Promise((resolve, reject) => {
        Swal.fire({
            title: titulo,
            text: subtitulo,
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Si',
        })
            .then((result) => {
                if (result.isConfirmed) {
                    resolve();
                } else {
                    reject();
                }
            });
    });
}