
$("#mainModalSubmit").on("click",
    function (e) {
        var form = $(this).closest(".modal").find("form");
        var action = form.prop("action");
        var call_after_success_close = form.prop("callAfterSuccessClose");
        var fn = callAjax(action, form);

        fn.done(function () {
            eval(call_after_success_close.value);
        });

    }
);

function callAjax(action, form) {
    return $.ajax({
        method: "post",
        url: action,
        data: form.serialize(),
    })
        .done(function (data, textStatus, jqXHR) {
            // 200-as response
            if ((jqXHR.readyState == 4) && (jqXHR.status == 200)) {

                if (data && (data.isSuccess == undefined)) {
                    // nem json válasz
                    $("#mainModal .modal-body").html(data);
                }
                else {
                    // ha JSON válasz
                    $("#mainModal").modal("hide");
                }
            }



        })
        .fail(function (err) {
            console.log("error:" + err);
        });
}

function partnersTableRefresh() {
    $("#pertnersTable").DataTable().ajax.reload();
};