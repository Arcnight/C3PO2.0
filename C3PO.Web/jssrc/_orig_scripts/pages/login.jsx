var React = require('react');
var auth = require('../auth.js');

module.exports = React.createClass({
    login: function (e) {
        e.preventDefault();

        auth.login(this.formData, function (loggedIn) {
            if (loggedIn) {
                var location = this.props;

                if (location.state && location.state.nextPathname) {
                    this.context.router.replace(location.state.nextPathname);
                } else {
                    this.context.router.replace('/');
                }
            }
        });
    },

    render: function () {
        return
        (
            <div>
                <div>
                    <a>Login</a>
                </div>
                <div>
                    <form onSubmit={this.login}>
                        <input type="text" name="username" placeholder="Username" />
                        <input type="text" name="password" placeholder="Password" />
                        <input type="submit" value="login" />
                    </form>
                </div>
            </div>
        );
    }
});