$(document).ready(function () {
    let idCounter = 0;
    document.getElementById("btnIncluirBeneficiario").addEventListener("click", function () {


        var cpf = document.getElementById("cpfBeneficiario").value.trim();
        var nome = document.getElementById("nomeBeneficiario").value.trim();
        const id = $("#beneficiarioId").val();

        if (!cpf || !nome) {
            alert("Preencha o CPF e o Nome do beneficiário.");
            return;
        }

        if (id != "") {
            // É uma alteração
            // Remove a linha antiga com o mesmo data-id
            $(`#tabelaBeneficiarios tr[data-id="${id}"]`).remove();
        }

        // Se for novo, cria um ID único temporário
        const novoId = id || `temp-${idCounter++}`;
        // Criação da nova linha
        var novaLinha = `
        <tr data-id="${novoId}">
            <td>${cpf}</td>
            <td>${nome}</td>
            <td>
                <button type="button" class="btn btn-sm btn-primary btn-alterar-beneficiario">Alterar</button>
                <button type="button" class="btn btn-sm btn-danger btn-excluir-beneficiario">Excluir</button>
            </td>
        </tr>
            `;
        document.getElementById("tabelaBeneficiarios").insertAdjacentHTML("beforeend", novaLinha);

        // Limpa campos
        document.getElementById("cpfBeneficiario").value = "";
        document.getElementById("nomeBeneficiario").value = "";
        document.getElementById("beneficiarioId").value = "";

    });

    // Evento para botão Alterar
    $(document).on('click', '.btn-alterar-beneficiario', function () {
        const linha = $(this).closest('tr');
        const cpf = linha.find('td').eq(0).text();
        const nome = linha.find('td').eq(1).text();
        const id = linha.data('id');

        $("#cpfBeneficiario").val(cpf);
        $("#nomeBeneficiario").val(nome);
        $("#beneficiarioId").val(id);
    });
    // Evento para botão Excluir
    $(document).on('click', '.btn-excluir-beneficiario', function () {
        var linha = $(this).closest('tr');
        linha.remove();
    });
})


function salvarBeneficiario() {
    var formData = {
        IdCliente: $('#IdCliente').val(),
        Nome: $('#Nome').val(),
        Cpf: $('#Cpf').val()
    };

    $.ajax({
        type: 'POST',
        url: '/Cliente/InserirBeneficiario',
        data: formData,
        success: function (response) {
            $('#modalBeneficiario').modal('hide');
            alert(response);
        },
        error: function (xhr) {
            $('#errosBeneficiario').html(xhr.responseText);
        }
    });
}
