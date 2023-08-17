
//USO DE PROMESAS
function alertSuccess(titulo, subtitulo) {
    return new Promise((resolve, reject) => {
    Swal.fire({
        title: titulo,
        text: subtitulo,
        icon: 'success',
        confirmButtonText: 'OK'
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