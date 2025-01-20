$(function () {
    var connection = new signalR.HubConnectionBuilder()
        .withUrl("/NotificationHubService") // Replace with your hub URL
        .build();

    connection.on("NotificationMessage", function (message) {
        var currentPath = window.location.pathname.toLowerCase();
        if (currentPath === "/notification/index") {
            $("#notification-message").text("New message, please refresh").show();
        } else {
            $("#notification-message").text("There is a new notification, please visit the notification hub").show();
        }
    });

    connection.start().then(function () {
        console.log("SignalR started");
    }).catch(function (err) {
        return console.error(err.toString());
    });
});
