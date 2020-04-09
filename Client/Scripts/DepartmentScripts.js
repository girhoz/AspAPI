var table = null;
$(document).ready(function () {
    //debugger;
    table = $('#Department').DataTable({ //Nama table pada index
        "ajax": {
            url: "/Departments/LoadDepartment", //Nama controller/fungsi pada index controller
            type: "GET",
            dataType: "json",
            dataSrc: "",
        },
        "columnDefs": [
            { "orderable": false, "targets": 3 },
            { "searchable": false, "targets": 3 }
        ],
        "columns": [
            { "data": "DepartmentName", "name": "Name" },
            {
                "data": "CreateDate", "render": function (data) {
                    return moment(data).format('DD/MM/YYYY');
                }
            },
            {
                "data": "UpdateDate", "render": function (data) {
                    var dateupdate = "Not Updated Yet";
                    var nulldate = null;
                    if (data == nulldate) {
                        return dateupdate;
                    } else {
                        return moment(data).format('DD/MM/YYYY');
                    }
                }
            },
            {
                data: null, render: function (data, type, row) {
                    return " <td><button type='button' class='btn btn-warning' id='Update' onclick=GetById('" + row.Id + "');>Edit</button> <button type='button' class='btn btn-danger' id='Delete' onclick=Delete('" + row.Id + "');>Delete</button ></td >";
                }
            },  
        ]
    });
});


function Save() {
    var Department = new Object();
    Department.DepartmentName = $('#DepartmentName').val();
    $.ajax({
        type: 'POST',
        url: '/Departments/InsertOrUpdate/',
        data: Department
    }).then((result) => {
        //debugger;
        if (result.StatusCode == 200) {
            Swal.fire({
                position: 'center',
                type: 'success',
                title: 'Department Added Succesfully'
            }).then((result) => {
                if (result.value) {
                    table.ajax.reload()
                }
            });
        }
        else {
            Swal.fire('Error', 'Failed to Add Department', 'error');
            ShowModal();
        }
    })
}

function GetById(Id) {
    //debugger;
    $.ajax({
        url: "/Departments/GetById/" + Id,
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: false,
        success: function (result) {
            const obj = JSON.parse(result);
            $('#Id').val(obj.Id);
            $('#DepartmentName').val(obj.DepartmentName);
            $("#createModal").modal('show');
            $("#Save").hide();
            $('#Edit').show();
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    })
}

function Edit() {
    var Department = new Object();
    Department.Id = $('#Id').val();
    Department.DepartmentName = $('#DepartmentName').val();
    $.ajax({
        type: 'POST',
        url: '/Departments/InsertOrUpdate/',
        data: Department
    }).then((result) => {
        //debugger;
        if (result.StatusCode == 200) {
            Swal.fire({
                position: 'center',
                type: 'success',
                title: 'Department Edit Succesfully'
            }).then((result) => {
                if (result.value) {
                    table.ajax.reload();
                }
            });
        }
        else {
            Swal.fire('Error', 'Failed to Edit Department', 'error');
            ShowModal();
        }
    })
}

function Delete(Id) {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        //debugger;
        if (result.value) {
            $.ajax({
                url: "/Departments/Delete/",
                data: { Id: Id }
            }).then((result) => {
                if (result.StatusCode == 200) {
                    Swal.fire({
                        position: 'center',
                        type: 'success',
                        title: 'Department Deleted Succesfully'
                    }).then((result) => {
                        if (result.value) {
                            table.ajax.reload();
                        }
                    });
                }
                else {
                    Swal.fire('Error', 'Failed to Delete Department', 'error');
                    ShowModal();
                }
            })
        };
    });
}


function ShowModal() {
    $("#createModal").modal('show');
    $('#Id').val('');
    $('#DepartmentName').val('');
    $("#Save").show();
    $("#Edit").hide();
}