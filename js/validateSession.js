function validaSession() {
    var rta = 0;
    $.ajax({
        type: "POST",
        url: "admin.aspx/validaSession",
        dataType: "text",
        contentType: "application/json; charset=utf-8",
        data: '{}',
        dataType: "json",
        success: function (result) {
            if (result.d === "fail") {
                document.location.href = "admin.aspx";
            } else {
                $(document.body).show();
                //$("#userName").html(result.d);
            }
        }
    });
    return false;
}

function cerrarSession() {
    $.ajax({
        type: "POST",
        url: "admin.aspx/cerrarSession",
        contentType: "application/json; charset=utf-8",
        data: "{}",
        dataType: "json",
        success: function (resultado) {
            document.location.href = "admin.aspx";
        }
    });
    return false;
}

$(document).ready(function () {
    $(document.body).hide();
    validaSession();
    
    $("#logout").click(function (e) {
        cerrarSession();
    });

});