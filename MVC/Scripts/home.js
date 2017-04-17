$(function () {
    // Set username in welcome message.
    if (SecurityManager.username) {
        $('#status').text('welcome, ' + SecurityManager.username + '!');

        $('#btnLogin').hide();
        $('#btnLogout').show();
    }

    // Button click events.
    $('#btnLogin').click(function () {
        // Login as the user and create a token key.
        var uri = SecurityManager.generateUri('adminToken', 'AdminWebApiDi');

        $.get(uri, function (data) {
            alert('User logged in!');
        }).fail(function (error) {
            alert('HTTP Error ' + error.status);
        });

        $('#btnLogin').hide();
        $('#btnLogout').show();
        $('#status').text('Welcome, ' + SecurityManager.username + '!');
    });

    $('#btnLogout').click(function () {
        // Clear the token key and delete localStorage settings.
        SecurityManager.logout();

        $('#btnLogin').show();
        $('#btnLogout').hide();
        $('#status').text('Welcome!');
        $('#result').text('');
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