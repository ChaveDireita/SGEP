// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

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
    $('#edit').on('click', editCallback);
    $('#details').attr('disabled', false);
    $('#details').on('click', detailsCallback);
}

function search()
{
    const fields = $('.search-field');
    pagination.itensPerPage = document.getElementById('search-item-pagina').value;
    pagination.page = 1;
    const searchParams = {pagina: pagination.page};
    for (const f of fields) {
        searchParams[f.name] = f.value;
    }
    list(searchParams);
}