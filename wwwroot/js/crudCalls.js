
$(document).ready(function () {
    $('#editForm, #deleteForm, #createForm').submit(function (event) {
        event.preventDefault();
        $.ajax({
            type: "POST",
            url: $(this).attr("action"),
            data: $(this).serialize(),

            success: function (response) {
                console.log("AJAX Response:", response);
                if (response.success) {
                    window.location.href = '/User/Index';
                } else {
                    alert("Edit failed, " + response.errors.join(', '));

                }
            },
            error: function (error) {
                alert("Error occured" + error);
            }

        });
    });
})



