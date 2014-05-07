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
        var app = "sales_tool";
        var datae = { 'name': usreg, 'pass': pass, 'app': app };
        $.ajax({
            type: "POST",
            url: "admin.aspx/validarIngresoAdmin",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(datae),
            dataType: "json",
            success: function (resultado) {
                if (resultado.d === "ok") {
                    document.location.href = "index.html";
                }
            }
        });
        return false;
    });

    $("#forgot").click(function () {
        showmessageDialog();
    })
});


function showmessageDialog() {

    BootstrapDialog.show({
        cssClass: 'type-danger',
        title: 'Recovery Password',
        message: $('<div class="control-group"> <label class="control-label" for="email">Type your email:</label> <div class="controls"> <input id="email" type="email" placeholder="Type your email here..." /> </div>  </div>'),
        buttons: [{
            label: 'Recovery',
            cssClass: 'btn-danger',
            action: function () {
                if ($("#email").val() !== "") {
                    var email = $("#email").val();
                    if (validaEmail(email)) {
                        recoveryPassword(email);
                    } else {
                        mensaje('Please check the email format', 'Alert', 'danger');
                    }
                } else {
                    mensaje('Please type your email', 'Alert', 'danger');
                }
            }
        }, {
            label: 'Close',
            action: function (dialogItself) {
                dialogItself.close();
            }
        }]

    });
}


function validaEmail(email) {
     var correo = /^[A-Za-z][A-Za-z0-9_\.-]*@[A-Za-z0-9_-]+\.[A-Za-z0-9_.]+[A-za-z]$/;
     if (!correo.exec(email)) {
            return false;
     } else {
         return true;
     }
}

function recoveryPassword(email) {
    $.prettyLoader();
    var datae = { 'email': email };
    $.ajax({
        type: "POST",
        url: "admin.aspx/recoveryPassword",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(datae),
        dataType: "json",
        success: function (resultado) {
            if (resultado.d === "ok") {
                $.each(BootstrapDialog.dialogs, function (id, dialog) {
                    dialog.close();
                });
                mensaje('We have sent an email with your login credentials .!', 'Attention', 'danger');
            } else {
                $.each(BootstrapDialog.dialogs, function (id, dialog) {
                    dialog.close();
                });
                mensaje('Please verify your email and try again', 'Alert', 'danger');
            }
        }
    });
    return false;

}
