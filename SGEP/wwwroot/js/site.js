// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function gerarlist(nomes, lista,acao,relevanteid) {
    var html = '';
    html += '<thead class="thead-dark">';
    for (n of nomes) html += '<th>' + n + '</th>';
    html += '</thead> <tbody>';
    for (item of lista) {
        if (!(acao === undefined || relevanteid === undefined)) html += '<tr onclick="' + acao + '(' + item.id + ',' + relevanteid + ')">';
        else html += '<tr>';
        for (variavel in item) {
            if (verificarsemacentoecaixa(nomes,variavel)) {
            html += '<td>' + item[variavel] + '</td>';
            }
        }
        html += '</tr>';
    }
    html += '</tbody>';
    return html;
}
function tiraracento(str) {
    return str.normalize("NFD").replace(/[\u0300-\u036f]/g, "");
}
function gerardetailslist(nome, content) {
    var html = '',i=0;
    for (c in content) {
        html += '<tr>';
        html += '<td class="table-dark alert-link" style="width:250px">' + nome[i] +
            '</td>' +
            '<td>' + content[c] + '</td>';
        i++;
    }
    return html;
}
function verificarsemacentoecaixa(lista, palavra) {
    var nms = [];
    for (n of lista) {
        n = n.toUpperCase();
        n = tiraracento(n);
        nms.push(n);
    }
    if (nms.includes(palavra.toUpperCase())) return true;
    else return false;
}