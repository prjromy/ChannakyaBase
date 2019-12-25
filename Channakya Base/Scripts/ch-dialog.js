
function SuccessAlert(msg, duration) {
    if (msg != "") {
        debugger;
        duration = duration || 0;
        var dlg = document.createElement("div");
        dlg.setAttribute("id", "alert-success");
        dlg.setAttribute("class", "dialog-alert");
        //dlg.setAttribute("style", "z-index:100000;position:fixed;");
        dlg.style.zIndex = 100000;
        dlg.style.position = 'fixed';
        dlg.innerHTML = "<span class='alert-heading'>Success<" + "/span>";
        dlg.innerHTML = dlg.innerHTML + "<span class='dialog-alert-close'>&#x0058;<" + "/span>";
        dlg.innerHTML = dlg.innerHTML + "<hrline style='border-top: 1px solid #78C864;'>";
        dlg.innerHTML = dlg.innerHTML + "<div class='dialog-alert-body' >" + msg + "<" + "/div>"

        if (duration != 0) {
            setTimeout(function () {

                if (dlg.parentNode != null) {
                    dlg.parentNode.removeChild(dlg);
                }

            }, duration);
        }
        document.body.appendChild(dlg);
        $('.dialog-alert').hide().fadeIn(500);
    }
}


function ErrorAlert(msg, duration) {
    if (msg != "") {
        duration = duration || 0;
        var dlg = document.createElement("div");
        dlg.setAttribute("id", "alert-error");
        dlg.setAttribute("class", "dialog-alert");
        dlg.setAttribute("style", "z-index:100000;position:fixed;");
        dlg.style.zIndex = 100000;
        dlg.style.position = 'fixed';
        dlg.innerHTML = "<span class='alert-heading'>Error<" + "/span>";
        dlg.innerHTML = dlg.innerHTML + "<span class='dialog-alert-close'>&#x0058;<" + "/span>";
        dlg.innerHTML = dlg.innerHTML + "<hrline style='border-top: 1px solid #E1BEBE;'>";
        dlg.innerHTML = dlg.innerHTML + "<div class='dialog-alert-body' >" + msg + "<" + "/div>"

        if (duration != 0) {
            setTimeout(function () {
                if (dlg.parentNode != null) {
                    dlg.parentNode.removeChild(dlg);
                }
            }, duration);
        }
        document.body.appendChild(dlg);
        $('.dialog-alert').hide().fadeIn(500);
    }
}


function InfoAlert(msg, duration) {
    if (msg != "") {
        duration = duration || 0;
        var dlg = document.createElement("div");
        dlg.setAttribute("id", "alert-info");
        dlg.setAttribute("class", "dialog-alert");
        dlg.style.zIndex = 100000;
        dlg.style.position = 'fixed';
        dlg.innerHTML = "<span class='alert-heading'>Info<" + "/span>";
        dlg.innerHTML = dlg.innerHTML + "<span class='dialog-alert-close'>&#x0058;<" + "/span>";
        dlg.innerHTML = dlg.innerHTML + "<hrline style='border-top: 1px solid #DEE16C;'>";
        dlg.innerHTML = dlg.innerHTML + "<div class='dialog-alert-body'> " + msg + " <" + "/div>"


        if (duration != 0) {
            setTimeout(function () {
                if (dlg.parentNode != null) {
                    dlg.parentNode.removeChild(dlg);
                }
            }, duration);
        }
        document.body.appendChild(dlg);
        $('.dialog-alert').hide().fadeIn(500);
    }
}

$(document).on('click', '.dialog-alert-close', function (e) {
    $(this).parents(".dialog-alert").remove();
})