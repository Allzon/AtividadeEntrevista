var BeneficiariosExcluidos = [];
let editRow = null;
$(document).ready(function (e) {

    $('#formBeneficiario #CPF').on("keyup", formatarCPF);

    $('#IncluirBeneficiarioBtn').on('click', function (e) {
        var cpf = $('#formBeneficiario').find('#CPF').val();
        var nome = $('#formBeneficiario').find('#Nome').val();

        // Pega a referência para o <tbody> da tabela
        var tableBody = document.querySelector('#TabelaBeneficiarios tbody');

        // Cria uma nova linha (<tr>)
        var newRow = document.createElement('tr');

        // Cria e adiciona a célula CPF à nova linha
        var cpfCell = document.createElement('td');
        cpfCell.textContent = maskCPF(cpf);
        newRow.appendChild(cpfCell);

        // Cria e adiciona a célula Nome à nova linha
        var nomeCell = document.createElement('td');
        nomeCell.textContent = nome;
        newRow.appendChild(nomeCell);

        // Cria e adiciona a célula para os botões de ação
        var actionCell = document.createElement('td');

        // Botão "Alterar"
        var editButton = document.createElement('button');
        editButton.type = 'button';
        editButton.className = 'btn btn-primary btn-sm';
        editButton.textContent = 'Alterar';

        // Botão "Excluir"
        var deleteButton = document.createElement('button');
        deleteButton.type = 'button';
        deleteButton.className = 'btn btn-danger btn-sm';
        deleteButton.textContent = 'Excluir';
        deleteButton.setAttribute('onclick', 'removeRow(this)');

        // Adiciona os botões na célula de ações
        actionCell.appendChild(editButton);
        actionCell.appendChild(deleteButton);
        newRow.appendChild(actionCell);

        // Adiciona a nova linha ao corpo da tabela
        tableBody.appendChild(newRow);

        // Limpa os campos de entrada
        $('#formBeneficiario').find('#CPF').val('');
        $('#formBeneficiario').find('#Nome').val('');
    });    

    $('#SalvarBeneficiarioBtn').click(function () {
        if (editRow) {
            const cpf = $('#formBeneficiario #CPF').val();
            const nome = $('#formBeneficiario #Nome').val();

            // Atualiza os dados na linha
            //editRow.find('td:eq(0)').text(cpf);
            editRow.cells[0].innerText = cpf;
            editRow.cells[1].innerText = nome;
            //editRow.find('td:eq(1)').text(nome);

            // Limpa os campos do formulário e volta os botões
            $('#formBeneficiario').trigger('reset');
            editRow = null;
            $('#IncluirBeneficiarioBtn').show();
            $('#SalvarBeneficiarioBtn').hide();
        }
    });


})

function formatarCPF(e) {
    e.target.value = maskCPF(e.target.value);
}

function maskCPF(cpf) {

    var v = cpf.replace(/\D/g, "");

    v = v.replace(/(\d{3})(\d)/, "$1.$2");

    v = v.replace(/(\d{3})(\d)/, "$1.$2");

    v = v.replace(/(\d{3})(\d{1,2})$/, "$1-$2");

    return v;
}

function SaveBeneficiarios(clientid) {

    var tableData = []  

    $('#TabelaBeneficiarios tbody tr').each(function () {

        var id = $(this).attr('beneficiarioId');
        var cpf = $(this).find('td:eq(0)').text();
        var nome = $(this).find('td:eq(1)').text();

        tableData.push({
            Id: id,
            CPF: cpf,
            Nome: nome
        });
    });

    if (tableData.length == 0 && BeneficiariosExcluidos.length >= 1) {        
        return new Promise(function (resolve, reject) {
            $.ajax({
                url: urldeleteBeneficiario,
                method: "POST",
                data: {                    
                    "listaId": BeneficiariosExcluidos
                },
                success: function (r) {
                    resolve(r);
                },
                error: function (r) {

                    if (r.status == 400) {
                        reject(r.responseJSON);
                    } else if (r.status == 500) {
                        reject("Ocorreu um erro interno no servidor.");
                    } else {
                        reject("Ocorreu um erro desconhecido.");
                    }
                }
            });
        });
    }

    return new Promise(function (resolve, reject) {
        $.ajax({
            url: urlPostBeneficiario,
            method: "POST",
            data: {
                "ClienteId": clientid,
                "Beneficiarios": tableData
            },
            success: function (r) {
                resolve(r);
            },
            error: function (r) {

                if (r.status == 400) {
                    reject(r.responseJSON);
                } else if (r.status == 500) {
                    reject("Ocorreu um erro interno no servidor.");
                } else {
                    reject("Ocorreu um erro desconhecido.");
                }
            }
        });
    });
}

function removeRow(button) {    
    var row = button.closest('tr');    
    BeneficiariosExcluidos.push(row.getAttribute('beneficiarioId'));
    row.remove();
}

function editarBeneficiario(button) {
    const row = button.closest('tr');
    const cpf = row.cells[0].innerText;
    const nome = row.cells[1].innerText;

    // Preencher os campos do formulário
    $('#formBeneficiario #CPF').val(cpf);
    $('#formBeneficiario #Nome').val(nome);

    // Esconde o botão Incluir e mostra o botão Salvar
    $('#IncluirBeneficiarioBtn').hide();
    $('#SalvarBeneficiarioBtn').show();

    // Define a linha para editar
    editRow = row;
}