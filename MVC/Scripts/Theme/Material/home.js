$(function () {
    // Set username in welcome message.
    if (SecurityManager.username) {
        $('#status').text('Welcome, ' + SecurityManager.username + '!');

        $('#btn-login').css("display", "none");
        $('#btn-logout').css("display", "inline");
    }
    else {
        $('#btn-login').css("display", "inline");
        $('#btn-logout').css("display", "none");
    }

    // Button click events.
    $('#btn-login').click(function () {
        // Login as the user and create a token key.
        //var uri = SecurityManager.generateUri('adminToken', 'AdminWebApiDi');
        //$.ajax({
        //    url: uri,
        //    type: "GET", // TODO change to post and poass token as data : {token:token}
        //    async: true, 
        //    success: function (data) {
        //        //alert('User logged in!');
        //        //window.location = "/tile/sale";
        //        window.location.assign("/tile/sale");
        //    },
        //    error: function (jqXHR, textStatus, errorThrown) {
        //        alert('Cannot log you in with those creds... sorry!');
        //        console.log(jqXHR);
        //        console.log(textStatus);
        //        console.log(errorThrown);
        //    }
        //});

        var token = SecurityManager.generateToken('adminToken', 'AdminWebApiDi');
        //alert(token);
        $.ajax({
            url: '/user/signin?token=' + token,
            type: "GET", // TODO change to post and poass token as data : {token:token}
            //data: {token : token},
            async: true,
            success: function (data) {
                //alert('User logged in!');
                //window.location = "/tile/sale";
                window.location.assign("/tile/sale");
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert('Cannot log you in with those creds... sorry!');
                console.log(jqXHR);
                console.log(textStatus);
                console.log(errorThrown);
            }
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