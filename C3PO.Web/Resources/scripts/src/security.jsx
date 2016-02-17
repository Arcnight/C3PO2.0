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
        .post('/api/account/login', this.formData)
        .then(this.getLoginResponse)
        .catch(function (response) {
            alert(response);
        });
    },

    logout: function (e) {
        e.preventDefault();

        axios
        .get('/api/account/logout')
        .then(this.getLogoutResponse)
        .catch(function (response) {
            alert(response);
        });
    },

    getLoginResponse: function (response) {
        this.props.callback(true);
    },

    getLogoutResponse: function (response) {
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
                    <form onChange={ this.updateFormData } onSubmit={ this.login }>
                        <input type="text" name="username" placeholder="Username" />
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