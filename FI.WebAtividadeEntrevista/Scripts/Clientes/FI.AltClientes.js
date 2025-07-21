$(document).ready(function () {
    if (obj) {
        $('#formCadastro #Nome').val(obj.Nome);
        $('#formCadastro #CEP').val(obj.CEP);
        $('#formCadastro #CPF').val(obj.CPF).prop('disabled', true);
        $('#formCadastro #Email').val(obj.Email);
        $('#formCadastro #Sobrenome').val(obj.Sobrenome);
        $('#formCadastro #Nacionalidade').val(obj.Nacionalidade);
        $('#formCadastro #Estado').val(obj.Estado);
        $('#formCadastro #Cidade').val(obj.Cidade);
        $('#formCadastro #Logradouro').val(obj.Logradouro);
        $('#formCadastro #Telefone').val(obj.Telefone);

        // 🟡 Carregar beneficiarios (se existirem)
        if (obj.Beneficiarios && Array.isArray(obj.Beneficiarios)) {
            obj.Beneficiarios.forEach(function (dep) {
                let novaLinha = `
                    <tr data-id="${dep.Id}">
                        <td>${dep.Cpf}</td>
                        <td>${dep.Nome}</td>
                        <td>
                            <button type="button" class="btn btn-sm btn-primary btn-alterar-beneficiario">Alterar</button>
                            <button type="button" class="btn btn-sm btn-danger btn-excluir-beneficiario">Excluir</button>
                        </td>
                    </tr>
                `;
                $('#tabelaBeneficiarios').append(novaLinha);
            });
        }
    }

    $('#formCadastro').submit(function (e) {
        e.preventDefault();

        // 🟡 Coletar beneficiarios da tabela
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
                CEP: $('#CEP').val(),
                CPF: $('#CPF').val(),
                Email: $('#Email').val(),
                Sobrenome: $('#Sobrenome').val(),
                Nacionalidade: $('#Nacionalidade').val(),
                Estado: $('#Estado').val(),
                Cidade: $('#Cidade').val(),
                Logradouro: $('#Logradouro').val(),
                Telefone: $('#Telefone').val(),
                Beneficiarios: beneficiarios
            }),
            error: function (r) {
                if (r.status == 400)
                    ModalDialog("Ocorreu um erro", r.responseJSON);
                else if (r.status == 500)
                    ModalDialog("Ocorreu um erro", "Ocorreu um erro interno no servidor.");
            },
            success: function (r) {
                ModalDialog("Sucesso!", r);
                $("#formCadastro")[0].reset();
                $('#tabelaBeneficiarios').empty();
                window.location.href = urlRetorno;
            }
        });
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
