//tipo de mensajes ->  default, info,primary,sucess,warning,danger
function mensaje(texto, titulo, tipo) {
    BootstrapDialog.show({
        title:titulo,
        message:texto,
        cssClass: 'type-' + tipo

    })

}

$(document).keypress(function (e) {
    if (e.which == 13) {
        $.each(BootstrapDialog.dialogs, function (id, dialog) {
            dialog.close();
        });
        $("#login").click();
    }
});


$(document).ready(function () {
    
    $("#login").click(function () {
        $.prettyLoader();
        var pass = $("#UserPass").val();
        pass = $.sha256(pass);
        var usreg = $("#usuario").val();
        var app = "TRAD";
        var datae = { 'name': usreg, 'pass': pass, 'app': app };
        $.ajax({
            type: "POST",
            url: "admin.aspx/validarIngresoAdmin",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(datae),
            dataType: "json",
            success: function (resultado) {
                if (resultado.d === "solicitante") {
                    document.location.href = "solicitante.aspx";
                } else if (resultado.d == "traductor") {
                    document.location.href = "traductor.aspx";
                }  else if (resultado.d == "revisor"){
                    document.location.href = "revisor.aspx";
                }  else if (resultado.d == "administrador") {
                    document.location.href = "administrador.aspx";
                } else {
                    mensaje("Alert, please check your credentials", "Request Application", "danger");
                }
            }
        });
        return false;
    });
});