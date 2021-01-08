$(document).ready(function () {

    //displayToastr();

});

function displayToastr() {
    
    toastr.info('Info Toastr');

    toastr.warning('Toastr de Aviso');

    toastr.success('Tarefa Concluída', 'Sucesso');

    toastr.error('Ocorreu um erro', 'Contacte o administrador');
}