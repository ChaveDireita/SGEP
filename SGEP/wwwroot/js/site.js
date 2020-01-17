// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function gerartabela(tabela, nomes, lista) {
    //tabela.innerHTML = '';
    tabela.innerHTML += '<thead>';
    for (n of nomes) tabela += '<th>' + n + '<\th>';
    tabela.innerHTML += '<\thead> <tbody>';
    for (item of lista) {
        tabela.innerHTML += '<tr>';
        for (variavel in item) {
            if (nomes.contains(variavel).ignoreCase) {
                tabela.innerHTML += '<td>' + variavel[item] + '<\td>';
            }
        }
        tabela.innerHTML += '<\tr>';
    }
    tabela.innerHTML += '<\tbody>';
    //Aqui ta uma forma alternativa de fazer a msm coisa
    /*
    var thead = document.createElement('thead');
    for (n of nomes) thead.appendChild(document.createElement('th').innerHTML = n);
    tabela.appendChild(thead);
    var tbody = document.createElement('tbody');
    for (item of lista) {
        var th = document.createElement('th');
        for (variavel in item) {
            if (nomes.contains(variavel).ignoreCase) {
                th.appendChild(document.createElement('td').innerHTML = variavel[item]);
                //tabela.innerHTML += '<td>' + variavel[item] + '<\td>';
            }
        }
        tbody.appendChild(th);
    }
    tabela.appendChild(tbody);*/
}