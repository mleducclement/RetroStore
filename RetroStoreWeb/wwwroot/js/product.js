var dataTable;

$(document).ready(function () {
    loadDataTable();
})

function loadDataTable() {
    dataTable = new DataTable("#tblData", {
        responsive: true,
        ajax: {
            "url": "/Admin/Products/GetAll",
        },
        "columns": [
            { "data": "name" },
            { "data": "genre.name" },
            { "data": "releaseDateWithoutTime" },
            { "data": "platformName" },
            { "data": "price" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="w-100 btn-group" role="group">
                        <a class="btn btn-info" href="/Admin/Products/Edit/${data}"><i class="bi bi-pencil-square"></i></a>
                        <a class="btn btn-danger" onClick=Delete('/Admin/Products/Delete/'+${data})><i class="bi bi-trash3"></i></a>
                        </div>
                    `
                },
            }
        ]
    })
}

const Delete = (url) => {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    if (data.success) {
                        dataTable.ajax.reload();
                        toastr.success(data.message);
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            })
        }
    })
}