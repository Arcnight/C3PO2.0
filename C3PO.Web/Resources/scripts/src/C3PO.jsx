var React = require('react');
var ReactDOM = require('react-dom');
var Home = require('./home.jsx');
var Security = require('./security.jsx');

var C3PO = React.createClass({
    getInitialState: function () {
        return ({
            isLoggedIn: false
        });
    },

    setLoginState: function (loggedIn) {
        this.setState({ isLoggedIn: loggedIn });
    },

    render: function () {
        return (
            <div>
                <div>
                    <Security isLoggedIn={ this.state.isLoggedIn } onLogin={ this.setLoginState } onLogout={ this.setLoginState } />
                </div>
                <div>
                    <Home isLoggedIn={ this.state.isLoggedIn } />
                </div>
            </div>
        );
    }
});

module.exports = C3PO;

ReactDOM.render(<C3PO />, document.getElementById("app"));