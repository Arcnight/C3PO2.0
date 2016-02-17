var axios = require('axios');
var React = require('react');
var ReactForm = require('react-form-data');

var Security = React.createClass({
    mixins: [ReactForm],

    propTypes: {
        callback: React.PropTypes.func.isRequired,
        isLoggedIn: React.PropTypes.bool.isRequired
    },

    login: function (e) {
        e.preventDefault();

        axios
        .get('/api/account/login', { data: this.formData })
        .then(this.loginFromResponse)
        .catch(function (response) {
            alert(response);
        });
    },

    logout: function (e) {
        e.preventDefault();

        axios
        .get('/api/account/logout')
        .then(this.logoutFromResponse)
        .catch(function (response) {
            alert(response);
        });
    },

    loginFromResponse: function (response) {
        this.props.callback(true);
    },

    logoutFromResponse: function (response) {
        this.props.callback(false);
    },

    render: function () {
        return !this.props.isLoggedIn ?
        (
            <div>
                <div>
                    <a>Login</a>
                </div>
                <div>
                    <form onSubmit={ this.login }>
                        <input type="text" name="userName" placeholder="Username" />
                        <input type="text" name="password" placeholder="Password" />
                        <input type="submit" value="login" />
                    </form>
                </div>
            </div>
        ) :
        (
            <div>
                <input type="button" onClick={ this.logout } value="logout" />
            </div>
        );
    }
});

module.exports = Security;