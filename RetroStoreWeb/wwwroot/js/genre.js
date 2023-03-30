var dataTable;

$(document).ready(function () {
    loadDataTable();
})

// load data table using the api call to /Manager/Genres/GetAll
function loadDataTable() {
    dataTable = new DataTable("#tblData", {
        responsive: true,
        ajax: {
            "url": "/Manager/Genres/GetAll",
        },
        "columns": [
            { "data": "name" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="w-100 btn-group" role="group">
                        <a class="btn btn-info" href="/Manager/Genres/Edit/${data}"><i class="bi bi-pencil-square"></i></a>
                        <a class="btn btn-danger" onClick=Delete('/Manager/Genres/Delete/'+${data})><i class="bi bi-trash3"></i></a>
                        </div>
                    `
                },
            }
        ]
    })
}

// swal is sweetalert2 module and will display an error message as a modal if there is no image selected
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