var React = require('react');
var ReactForm = require('react-form-data');

var Security = React.createClass({
    mixins: [ReactForm],

    getInitialState: function () {
        return {
            isLoggedIn: false
        };
    },

    componentWillUpdate: function () {
        this.updateLoginState(this.props.isLoggedIn);
    },

    login: function () {
        var security = this;
        var formData = this.formData;

        xhr.request({
            method: 'GET',
            data: formData,
            uri: _rootUrl + 'api/account/login',
            callback: function (err, response) {
                if (!err)
                {
                    security.updateLoginState(true);

                    security.props.onLogin && typeof security.props.onLogin == "function" && this.props.onLogin(true);
                }
                else
                {
                    alert(err);
                }
            }
        });
    },

    logout: function (e) {
        var security = this;
        var formData = this.formData;

        e.preventDefault();

        xhr.request({
            method: 'GET',
            data: formData,
            uri: _rootUrl + 'api/account/logout',
            callback: function (err, response) {
                if (!err)
                {
                    security.updateLoginState(false);

                    security.props.onLogout && typeof security.props.onLogout == "function" && this.props.onLogout(false);
                }
                else
                {
                    alert(err);
                }
            }
        });
    },

    render: function () {
        return (
            <div>
                <div>
                    <a>Login</a>
                </div>
                <div>
                    <form onSubmit={ this.login }>
                        <input type="text" name="username" placeholder="Username" />
                        <input type="text" name="password" placeholder="Password" />
                        <input type="submit" value="login" />
                    </form>
                </div>
            </div>
        );
    },

    updateLoginState: function(loggedIn) {
        this.setState({ isLoggedIn: loggedIn });
    }
});

module.exports = Security;