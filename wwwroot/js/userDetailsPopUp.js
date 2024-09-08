$(document).ready(function () {
    console.log("connected");
    $('.viewDetails').on('click', function () {
        var userId = $(this).data('id');
        console.log(this);
        if (userId != undefined) {
            $.ajax({
                type: "GET",
                url: "/User/GetUserDetails",
                data: { id: userId },
                success: function (response) {
                    console.log("success ran");
                    $('#userDetailsContent').html(response);
                    $('#userDetailsPopup').show();
                }
            });
        } else {
            console.log("id is null");
        }

    })
    $('#userDetailsPopup').on('click', function () {
        $(this).hide();
    });
})