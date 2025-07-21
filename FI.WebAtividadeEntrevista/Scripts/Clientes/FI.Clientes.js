$('#formCadastro').submit(function (e) {
    e.preventDefault();

    // Extrair beneficiarios da tabela
    var beneficiarios = [];
    $('#tabelaBeneficiarios tr').each(function () {
        var rawId = $(this).data('id');
        var id = (rawId === undefined || rawId.toString().includes("temp")) ? 0 : rawId;
        var cpf = $(this).find('td:eq(0)').text().trim();
        var nome = $(this).find('td:eq(1)').text().trim();

        if (cpf && nome) {
            beneficiarios.push({ Id: id, CPF: cpf, Nome: nome });
        }

    });

    $.ajax({
        url: urlPost,
        method: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
            Nome: $('#Nome').val(),
            Sobrenome: $('#Sobrenome').val(),
            CPF: $('#CPF').val(),
            Nacionalidade: $('#Nacionalidade').val(),
            CEP: $('#CEP').val(),
            Estado: $('#Estado').val(),
            Cidade: $('#Cidade').val(),
            Logradouro: $('#Logradouro').val(),
            Email: $('#Email').val(),
            Telefone: $('#Telefone').val(),
            Beneficiarios: beneficiarios
        }),
        success: function (r) {
            ModalDialog("Sucesso!", r);
            $("#formCadastro")[0].reset();
            $('#tabelaBeneficiarios').empty();
        },
        error: function (r) {
            if (r.status == 400)
                ModalDialog("Erro de validação", r.responseJSON);
            else if (r.status == 500)
                ModalDialog("Erro interno", "Ocorreu um erro interno no servidor.");
        }
    });
});


function ModalDialog(titulo, texto) {
    var random = Math.random().toString().replace('.', '');
    var texto = '<div id="' + random + '" class="modal fade">                                                               ' +
        '        <div class="modal-dialog">                                                                                 ' +
        '            <div class="modal-content">                                                                            ' +
        '                <div class="modal-header">                                                                         ' +
        '                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>         ' +
        '                    <h4 class="modal-title">' + titulo + '</h4>                                                    ' +
        '                </div>                                                                                             ' +
        '                <div class="modal-body">                                                                           ' +
        '                    <p>' + texto + '</p>                                                                           ' +
        '                </div>                                                                                             ' +
        '                <div class="modal-footer">                                                                         ' +
        '                    <button type="button" class="btn btn-default" data-dismiss="modal">Fechar</button>             ' +
        '                                                                                                                   ' +
        '                </div>                                                                                             ' +
        '            </div><!-- /.modal-content -->                                                                         ' +
        '  </div><!-- /.modal-dialog -->                                                                                    ' +
        '</div> <!-- /.modal -->                                                                                        ';

    $('body').append(texto);
    $('#' + random).modal('show');
}
