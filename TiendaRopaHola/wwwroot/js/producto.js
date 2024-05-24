let datatable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    datatable = $('#tblDatos').DataTable({
        "language": {
            "lengthMenu": "Mostrar _MENU_ Productos Por Pagina",
            "zeroRecords": "Ningun Registro",
            "info": "Mostrar pagina _PAGE_ de _PAGES_",
            "infoEmpty": "no hay registros",
            "infoFiltered": "(filtered from _MAX_ total registros)",
            "search": "Buscar",
            "paginate": {
                "first": "Primero",
                "last": "Último",
                "next": "Siguiente",
                "previous": "Anterior"
            }
        },
        "ajax": {
            "url": "/Producto/GetAll"
        },
        "columns": [
            { "data": "nombre" },
            { "data": "descripcion" },
            { "data": "talla" },
            { "data": "color" },
            {
                "data": "precio", "className": "text-end", "targets": 5, "render": function (data) {
                    var price = parseFloat(data);
                    var formattedNumber = price.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
                    return formattedNumber + " \u20AC";
                },
            },
            {
                "data": "id",
                "render": function (data) {
                    return `
                            <div class="text-center">
                                <a href="/Producto/Update/${data}" class="btn btn-success text-white" style="cursor:pointer">
                                    <i class="bi bi-pencil-square"></i>
                                <a/>
                                <a onclick=Delete("/Producto/Delete/${data}") class="btn btn-danger text-white" style="cursor: pointer">
                                    <i class="bi bi-trash3-fill"></i>                                
                                </a>
                            </div>
                        `;
                }, "width": "20%"
            }
        ],
        "lengthMenu": [[3, 5, 10, 25, 100], [3, 5, 10, 25, 100]],
        "pageLength": 3
    });
}