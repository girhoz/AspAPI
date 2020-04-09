$(document).ready(function () {
    $('#DepartmentTable').dataTable({
        "ajax": loadDataDepartment(),
        "responsive": true,
    });
    $('[data-toogle="tooltip"]').tooltip();
});

function loadDataDepartment() {
    $.ajax({
        url: "/Department/LoadDepartment", //manggil controller, /Controller/NamaFunction
        type: "GET",
        contentType: "application/json;charset-utf-8",
        dataType: "json",
        success: function (result) {
            debugger;
            var html = '';
            $.each(result, function (key, Department) {
                html += '<tr>';
                html += '<td>' + Department.DepartmentName + '</td>';
                html += '<td>' + Department.CreateDate + '</td>';
                html += '<td>' + Department.UpdateDate + '</td>';
                html += '<td><button type = "button" class = "btn-warning" id ="Update" onclick = "return GetById(' + Department.Id + ')">Edit</td>';
                html += '<td><button type = "button" class = "btn-danger" id ="Delete" onclick = "return Delete(' + Department.Id + ')">Delete</td>';
                html += '</tr>';
            });
            $('.departmentbody').html(html);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}