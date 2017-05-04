// MOST OF THIS PART COULD BE BROUGHT SERVER SIDE - EVEN THO EXPOSING THIS IS NOT ENOUGH TO HACK THROUGH WOULD STILL BE BETTER.
var SecurityManager = {
    username: localStorage['SecurityManager.username'],
    ip: null,
    salt: 'YBBP8aEDXVO5+RxDAEes+cJlP3xAO4kw2GDnOYRm1T8=',
    key: localStorage['SecurityManager.key'],

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

    generateToken: function (username, password) {
        // Generates a token to be used for API calls. The first time during authentication, pass in a username/password. All subsequent calls can simply omit username and password, as the same token key (hashed password) will be used.
        //if (username && password) {
        //    // If the user is providing credentials, then create a new key.
        //    SecurityManager.logout();
        //}
        console.log('here');
        // Set the username.
        SecurityManager.username = SecurityManager.username || username;

        // Set the key to a hash of the user's password + salt.
        SecurityManager.key = SecurityManager.key || CryptoJS.enc.Base64.stringify(CryptoJS.HmacSHA256([password, SecurityManager.salt].join(':'), SecurityManager.salt));

        // Set the client IP address.
        SecurityManager.ip = SecurityManager.ip || SecurityManager.getIp();

        // Persist key pieces.
        if (SecurityManager.username) {
            localStorage['SecurityManager.username'] = SecurityManager.username;
            localStorage['SecurityManager.key'] = SecurityManager.key;
        }

        // Get the (C# compatible) ticks to use as a timestamp. http://stackoverflow.com/a/7968483/2596404
        var ticks = ((new Date().getTime() * 10000) + 621355968000000000);

        // Construct the hash body by concatenating the username, ip, and userAgent.
        var message = [SecurityManager.username, SecurityManager.ip, navigator.userAgent.replace(/ \.NET.+;/, ''), ticks].join(':');

        // Hash the body, using the key.
        var hash = CryptoJS.HmacSHA256(message, SecurityManager.key);

        // Base64-encode the hash to get the resulting token.
        var token = CryptoJS.enc.Base64.stringify(hash);

        // Include the username and timestamp on the end of the token, so the server can validate.
        var tokenId = [SecurityManager.username, ticks].join(':');

        // Base64-encode the final resulting token.
        var tokenStr = CryptoJS.enc.Utf8.parse([token, tokenId].join(':'));
        console.log('here2');
        return CryptoJS.enc.Base64.stringify(tokenStr);
    },

    logout: function () {
        // do on server invalidate
        SecurityManager.ip = null;

        localStorage.removeItem('SecurityManager.username');
        SecurityManager.username = null;

        localStorage.removeItem('SecurityManager.ip');
        SecurityManager.ip = null;

        localStorage.removeItem('SecurityManager.key');
        SecurityManager.key = null;

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