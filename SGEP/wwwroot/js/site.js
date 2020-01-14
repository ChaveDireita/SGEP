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
        .then((res) => callback());
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
    getList(controller, searchParams, () => lista.innerHTML = '<tr><td colspan=4 class="reloading"><i class="fas fa-sync reloading"></i></td></tr>',
    (json) => {
        callback(json);
        lista.innerHTML = '';
        for(const e of json)
        {
            let line = '<tr id="' + e.id + '" class="selectable loading" onclick="selectRow(' + e.id + ')">';
            for (const col of columnList) {
                line += '<td>' + e[col] + '</td>';
            }
            line += '</tr>';
            lista.innerHTML += line;
        }
    });
}