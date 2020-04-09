$(document).ready(function () {
    $('#Corona').dataTable({
        "ajax": loadDataCorona(),
        "responsive": true,
    });
    $('[data-toogle="tooltip"]').tooltip();
});

function loadDataCorona() {
    $.ajax({
        url: "/Corona/LoadCorona", //manggil controller, /Controller/NamaFunction
        type: "GET",
        contentType: "application/json;charset-utf-8",
        dataType: "json",
        success: function (result) {
            debugger;
            var html = '';
            $.each(result, function (key, Corona) {
                html += '<tr>';
                html += '<td>' + Corona.Country + '</td>';
                html += '<td>' + Corona.Cases + '</td>';
                html += '<td>' + Corona.Deaths + '</td>';
                html += '<td>' + Corona.Recovered + '</td>';
                html += '<td><button type = "button" class = "btn-warning" id ="Update" onclick = "return GetById(' + Corona.Id + ')">Edit</td>';
                html += '<td><button type = "button" class = "btn-danger" id ="Delete" onclick = "return Delete(' + Corona.Id + ')">Delete</td>';
                html += '</tr>';
            });
            $('.coronabody').html(html);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}