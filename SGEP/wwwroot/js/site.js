const get =
{
    id: (id) => document.getElementById(id),
    cls: (cls) => document.getElementsByClassName(cls)[0],
    tag: (tag) => document.getElementsByTagName(tag)[0],
    name: (name) => document.getElementsByName(name)[0],
    body: () => document.body
};

const gets = 
{
    cls: (cls) => document.getElementsByClassName(cls),
    tag: (tag) => document.getElementsByTagName(tag),
    name: (name) => document.getElementsByName(name)
};

const utils = 
{
    get: get,
    gets: gets
}

var pagination = 
{
    size: 0,
    page: 1,
    itensPerPage: 10,
    next: () => pagination.page = Math.min (Math.max(Math.ceil (pagination.size/pagination.itensPerPage), 1), pagination.page + 1),
    prev: () => pagination.page = Math.max (1, pagination.page - 1)
};

function _search ()
{
    const fields = $('.search-field');
    pagination.itensPerPage = document.getElementById('search-item-pagina').value;
    if (pagination.page == 0)
        throw {message: 'asdsad'};
    const searchParams = { pagina: pagination.page };
    for (const f of fields) {
        searchParams[f.name] = f.value;
    }
    return searchParams;
}

const http = 
{
    GET: (resource, responseType, callback) => 
    {
        fetch(resource)
        .then((res) => res[responseType]())
        .catch((e) => console.log('Type error', e))
        .then((res) => callback(res));
    },
    POST: (resource, callback) => 
    {
        fetch(resource, { method: 'POST' })
        .then((res) => callback(res));
    }
}

function getList(controller, searchParams, before, callback)
{
    before();
    let route = '/' + controller + '/List?';
    for (const i in searchParams) {
        route += (i + '=' + searchParams[i] + '&');
    }
    http.GET(route, 'json', (json) => callback(json));
}

function refreshTable(controller, searchParams, columnList, callback)
{
    getList(controller, searchParams, () => 
    {
        lista.innerHTML = '<tr><td colspan=' + columnList.length + ' class="reloading"><i class="fas fa-sync reloading"></i></td></tr>';
    },
    (json) => {
        callback(json);
        lista.innerHTML = '';
        for(const e of json.entities)
        {
            let line = '<tr id="' + e.id + '" class="selectable loading" onclick="onClickSelectRow(' + e.id + ')">';
            for (const col of columnList) {
                line += '<td>' + e[col] + '</td>';
            }
            line += '</tr>';
            lista.innerHTML += line;
        }
    });
}

function selectRow (id, editCallback, detailsCallback)
{
    const selected = $('.selected');
    if (selected.length > 0) 
        selected[0].setAttribute('class', 'selectable');
    $('#' + id).attr('class', 'selected');
    $('#edit').attr('disabled', false);
    get.id('edit').onclick = editCallback;
    $('#details').attr('disabled', false);
    get.id('details').onclick = detailsCallback;
}

function add()
{
    $('#modal-create').modal('show');
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

function showAlert(msg, cls)
{
    const alertContainer = get.id('alert-container');
    alertContainer.innerHTML += `
        <div class="alert alert-${cls} alert-dismissible fade show" role="alert">
            ${msg}
            <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                <span aria-hidden="true">&times;</span>
            </button>
        </div>
        `;
}