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

function Upsert() {
    var producto = GetProductoForm();
    var url = "/Producto/Insert";
    if (producto.Id != null && producto.Id != 0)
        url = "/Producto/Update";
    var options = {
        contentType: "application/json; charset=utf-8",
        url: url,
        data: JSON.stringify(producto),
        type: "post",
        datatype: "json",
        success: function (result) {
            if (result.success) {
                toastr.success(result.message);
                setTimeout(function () {
                    window.location.href = "/Producto/Index";
                }, 1000);
            }
            else {
                toastr.error(result.message);
            }
        },
        error: function (xhr, status, error) {
            console.log(error);
        }
    };

    $.ajax(options);
}

function Delete(url) {
    swal({
        title: "Esta seguro de Eliminar el Producto?",
        text: "Este registro no podra ser rcuperado.",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((borrar) => {
        if (borrar) {
            $.ajax({
                method: "POST",
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        datatable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            })
        }
    });
}

function GetProductoForm() {
    var producto = {
        Id: $("#Id").val(),
        Nombre: $("#Nombre").val(),
        Descripcion: $("#Descripcion").val(),
        Talla: $("#Talla").val(),
        Color: $("#Color").val(),
        Precio: $("#precioInput").val()
    };
    return producto;
}