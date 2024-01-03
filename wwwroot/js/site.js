function deleteTodo(i)
{
    $.ajax({
        url: '/Home/Delete',
        type: 'POST',
        data: {
            id: i
        },
        success: function () {
            window.location.reload();
        }
    });
}

function populateForm(i) {
    $.ajax({
        url: '/Home/PopulateForm',
        type: 'GET',
        data: {
            id: i
        },
        dataType: 'json',
        success: function (result) {
            $('#Todo_Name').val(result.name);
            $('#Todo_Id').val(result.id);
            $('#form-button').val("Actualizar");
            $('#form-action').attr("action", "/Home/Update");
            $('#Todo_Name').focus();
        }
    });
}