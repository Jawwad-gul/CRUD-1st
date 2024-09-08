$(document).ready(function () {
    console.log("connected");
    $.ajax({
        url: "https://localhost:44339/api/UsersApii",
        method: "GET",
        headers: {
            "X_API_KEY": "chachiApi12"
        },
        success: function (users) {
            console.log(users);
            users.forEach(function (user) {
                $("#userData").append(`<div>User: ${user.uname}, ID: ${user.uid}</div>`);
            })
        },
        error: function (xhr, status, error) {
            console.error("Error occurred: " + xhr.responseText);
            alert("Error: " + xhr.responseText);
        }

    })
})