var table = null;
var Departments = []

$(document).ready(function () {
    //debugger;
    table = $('#Division').DataTable({ //Nama table pada index
        "ajax": {
            url: "/Divisions/LoadDivision", //Nama controller/fungsi pada index controller
            type: "GET",
            dataType: "json",
            dataSrc: "",
        },
        "columnDefs": [
            { "orderable": false, "targets": 4 },
            { "searchable": false, "targets": 4 }
        ],
        "columns": [
            { "data": "DivisionName", "name": "Name" },
            { "data": "DepartmentName", "name": "Department" },
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

//Tampung dan tampilkan department kedalam dropdownlist
function LoadDepartment(element) {
    if (Departments.length == 0) {
        $.ajax({
            type: "Get",
            url: "/Departments/LoadDepartment",
            success: function (data) {
                Departments = data;
                renderDepartment(element);
            }
        })
    }
    else {
        renderDepartment(element);
    }
}

function renderDepartment(element) {
    //debugger;
    var $option = $(element);
    $option.empty();
    $option.append($('<option/>').val('0').text('Select Department').hide());
    $.each(Departments, function (i, val) {
        $option.append($('<option/>').val(val.Id).text(val.DepartmentName));
    })
}
LoadDepartment($('#DepartmentOption'));
//


function Save() {
    var Division = new Object();
    Division.DivisionName = $('#DivisionName').val();
    Division.DepartmentId = $('#DepartmentOption').val();
    $.ajax({
        type: 'POST',
        url: '/Divisions/InsertOrUpdate/',
        data: Division
    }).then((result) => {
        debugger;
        if (result.StatusCode == 200) {
            Swal.fire({
                position: 'center',
                type: 'success',
                title: 'Division Added Succesfully'
            }).then((result) => {
                if (result.value) {
                    table.ajax.reload()
                }
            });
        }
        else {
            Swal.fire('Error', 'Failed to Add Division', 'error');
            ShowModal();
        }
    })
}

function GetById(Id) {
    //debugger;
    $.ajax({
        url: "/Divisions/GetById/" + Id,
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: false,
        success: function (result) {
            const obj = JSON.parse(result);
            $('#Id').val(obj.Id);
            $('#DivisionName').val(obj.DivisionName);
            $('#DepartmentOption').val(obj.DepartmentId);
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
    var Division = new Object();
    Division.Id = $('#Id').val();
    Division.DivisionName = $('#DivisionName').val();
    Division.DepartmentId = $('#DepartmentOption').val();
    $.ajax({
        type: 'POST',
        url: '/Divisions/InsertOrUpdate/',
        data: Division
    }).then((result) => {
        //debugger;
        if (result.StatusCode == 200) {
            Swal.fire({
                position: 'center',
                type: 'success',
                title: 'Division Edit Succesfully'
            }).then((result) => {
                if (result.value) {
                    table.ajax.reload();
                }
            });
        }
        else {
            Swal.fire('Error', 'Failed to Edit Division', 'error');
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
                url: "/Divisions/Delete/",
                data: { Id: Id }
            }).then((result) => {
                if (result.StatusCode == 200) {
                    Swal.fire({
                        position: 'center',
                        type: 'success',
                        title: 'Division Deleted Succesfully'
                    }).then((result) => {
                        if (result.value) {
                            table.ajax.reload();
                        }
                    });
                }
                else {
                    Swal.fire('Error', 'Failed to Delete Division', 'error');
                    ShowModal();
                }
            })
        };
    });
}


function ShowModal() {
    $("#createModal").modal('show');
    $('#Id').val('');
    $('#DivisionName').val('');
    $("#Save").show();
    $("#Edit").hide();
}