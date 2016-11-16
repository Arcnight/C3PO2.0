var axios = require('axios');

const auth = {
    isLoggedIn: function () {
        return localStorage.isLoggedIn;
    },

    login: function (formData, callback) {
        var onChange = this.onStateChange;

        if (localStorage.isLoggedIn) {
            DoCallbacks(true, callback, onChange);

            return;
        }

        axios
        .post('/api/account/login', formData)
        .then(function (response) {
            localStorage.isLoggedIn = true;
            DoCallbacks(true, callback, onChange);
        })
        .catch(function (response) {
            alert(response);
        });
    },

    logout: function (callback) {
        axios
        .get('/api/account/logout')
        .then(function (response) {
            localStorage.isLoggedIn = false;
            DoCallbacks(true, callback, onChange);
        })
        .catch(function (response) {
            alert(response);
        });
    },

    onStateChange: function () {}
};

module.exports = auth;

function DoCallbacks(success, callback1, callback2) {
    callback1 && typeof callback1 == 'function' && callback1(success);
    callback2 && typeof callback2 == 'function' && callback2(success);
};