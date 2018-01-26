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

    if (localStorage['redirect'])
        alert(localStorage['redirect']);

    // Button click events.
    $('#btn-login').click(function () {
        var $accessCode = $('#access-code');
        var $pin = $('#pin');

        //var token = SecurityManager.generateLoginToken('adminToken', 'AdminWebApiDi');
        var token = SecurityManager.generateLoginToken($accessCode.val(), $pin.val());

        $('#token').val(token);
        $('#sign-in').submit();
        //alert(token);
        //$.ajax({
        //    url: '/user/signin?token=' + token,
        //    type: "GET", // TODO change to post and poass token as data : {token:token}
        //    //data: {token : token},
        //    async: true,
        //    success: function (data) {
        //        //alert('User logged in!');
        //        //window.location = "/tile/sale";
        //        localStorage.removeItem('redirect'); // remove return form sign up
        //        window.location.assign("/tile/sale");
        //    },
        //    error: function (jqXHR, textStatus, errorThrown) {
        //        alert('Cannot log you in with those creds... sorry!');
        //        console.log(jqXHR);
        //        console.log(textStatus);
        //        console.log(errorThrown);
        //    }
        //});

        //$('#btn-login').css("display", "none");
        //$('#btn-logout').css("display", "inline");
        //$('#status').text('Welcome, ' + SecurityManager.username + '!');
        //$('#role').html('Employee, #1234.');
    });

    $('#btn-logout').click(function () {
        // Clear the token key and delete localStorage settings.
        SecurityManager.logout();

        $('#btn-login').css("display", "inline");
        $('#btn-logout').css("display", "none");
        $('#status').html('Alec Thompson.');
        $('#role').html('CEO / CO-FOUNDER.');
    });
});