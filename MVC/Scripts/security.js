// MOST OF THIS PART COULD BE BROUGHT SERVER SIDE - EVEN THO EXPOSING THIS IS NOT ENOUGH TO HACK THROUGH WOULD STILL BE BETTER.
var SecurityManager = {
    username: localStorage['SecurityManager.username'],
    ip: null,

    generateUri: function (username, password) {
        // Set the username.
        SecurityManager.username = SecurityManager.username || username;

        // Set the client IP address.
        SecurityManager.ip = SecurityManager.ip || SecurityManager.getIp();

        if (!SecurityManager.username)
            return "";

        // Get the (C# compatible) ticks to use as a timestamp. http://stackoverflow.com/a/7968483/2596404
        var ticks = ((new Date().getTime() * 10000) + 621355968000000000);

        var userAgent = navigator.userAgent.replace(/ \.NET.+;/, '');

        // TODO: use custom routes
        var uri =
            '/user/signin?username=' + SecurityManager.username + '&password=' + password
                + '&ip=' + SecurityManager.ip + '&userAgent=' + userAgent + '&ticks=' + ticks;

        // Persist key pieces.
        if (SecurityManager.username) {
            localStorage['SecurityManager.username'] = SecurityManager.username;
        }
        return uri;
    },

    logout: function () {
        // do on server invalidate
        SecurityManager.ip = null;

        localStorage.removeItem('SecurityManager.username');
        SecurityManager.username = null;

        localStorage.removeItem('SecurityManager.ip');
        SecurityManager.ip = null;

        $.get('user/signout', function () {
            alert('User logged out!');
        }).fail(function (error) {
            alert('HTTP Error ' + error.status);
        });
    },

    getIp: function () {
        var result = '';

        $.ajax({
            url: 'station/ip',
            method: 'GET',
            async: false,
            success: function (ip) {
                result = ip;
            }
        });

        return result;
    }
};