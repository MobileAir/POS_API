$(function () {
    if (localStorage['redirect'])
        localStorage.removeItem('redirect'); // TODO also removed double check on main page

    // Button click events.
    $('#btn-register').click(function () {
        var $accessCode = $('#access-code');
        var $pin = $('#pin');
        //var $name = $('#name');
        //var $email = $('#email');
        var keys = SecurityManager.generateRegisterKeys($accessCode.val(), $pin.val());
        
        $('#keys').val(keys);
        $('#sign-up').submit();

        //var uri = "/user/register?keys=" + keys + "&name=" + $name.val() + "&email=" + $email.val();
        //$.ajax({
        //    url: uri,
        //    type: "POST", // TODO change to post and poass token as data : {token:token}
        //    //data: {token : token},
        //    async: true,
        //    success: function (data) {

        //        //if (data.redirect) {
        //        //    localStorage['redirect'] = data
        //        //        .message +
        //        //        " Now please Sign In with those credentials."; // tell home iredirtect
        //        //    window.location.assign(data.redirect);
        //        //    //window.location.href = data.redirect; // THIS WORKS!!! with no alert... willl have to pass something to login page to know
        //        //} else {
        //        //    alert('An error occured please try registering again');
        //        //}
        //        localStorage['redirect'] = "Signup Successful - Now please Sign In with those credentials."; // tell home iredirtect
        //        window.location.assign("/home");

        //    },
        //    complete: function (xmlHttp) {
        //        console.log('complete');
        //        console.log(xmlHttp);
        //        if (xmlHttp.code == 200) {
        //            top.location.href = 'http://localhost:59727/home';
        //        }
        //        //redirect();
        //        //setTimeout(redirect(), 1500);
        //    },
        //    error: function (jqXHR, textStatus, errorThrown) {
        //        alert('Cannot sign up with those inputs... sorry!');
        //        SecurityManager.clear();
        //        console.log(jqXHR);
        //        console.log(textStatus);
        //        console.log(errorThrown);
        //    }
        //});
        //var jqxhr = $.post(uri)
        //  .done(function (data) {
        //      alert(data.message + " Now please Sign In with those credentials.");

        //  })
        //  .fail(function (jqXHR, textStatus, errorThrown) {
        //      alert('Cannot log you in with those creds... sorry!');
        //      console.log(jqXHR);
        //      console.log(textStatus);
        //      console.log(errorThrown);
        //  })
        //  .always(function (data) {
        //      alert('?');
        //      redirect();
        //  });
    });
});