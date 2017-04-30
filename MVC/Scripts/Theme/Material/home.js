$(function () {
    // Set username in welcome message.
    if (SecurityManager.username) {
        $('#status').text('Welcome, ' + SecurityManager.username + '!');

        $('#btn-login').css("display", "none");
        $('#btn-logout').css("display", "inline");
    }

    // Button click events.
    $('#btn-login').click(function () {
        // Login as the user and create a token key.
        var uri = SecurityManager.generateUri('adminToken', 'AdminWebApiDi');

        $.get(uri, function (data) {
            alert('User logged in!');
        }).fail(function (error) {
            alert('HTTP Error ' + error.status);
        });

        $('#btn-login').css("display", "none");
        $('#btn-logout').css("display", "inline");
        $('#status').text('Welcome, ' + SecurityManager.username + '!');
        $('#role').html('Employee, #1234.');
    });

    $('#btn-logout').click(function () {
        // Clear the token key and delete localStorage settings.
        SecurityManager.logout();

        $('#btn-login').css("display", "inline");
        $('#btn-logout').css("display", "none");
        $('#status').html('Alec Thompson.');
        $('#role').html('CEO / CO-FOUNDER.');
    });

    $('#btnSearch').click(function () {
        var query = $('#txtQuery').val();

        $.get('/home/find?q=' + query, function (data) {
            var names = data.join(', ');
            $("#result").append('<p>' + names + '</p>');
        }).fail(function (error) {
            alert('HTTP Error ' + error.status);
        });
    });
});