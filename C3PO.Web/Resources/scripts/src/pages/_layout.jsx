var React = require('react');
var RouteLink = require('react-router').Link;

var auth = require('../auth.js');

module.exports = React.createClass({
    getInitialState: function() {
        return {
            isLoggedIn: auth.isLoggedIn()
        };
    },

    componentWillMount: function () {
        auth.onChange = function (loggedIn) {
            this.setState({ isLoggedIn: loggedIn });
        };
    },

    render: function () {
        return (
            <div>
                <div>
                    Header
                </div>
                <div>
                    <ul>
                        <li><RouteLink to="/">Home</RouteLink></li>
                        <li><RouteLink to="/event">Event</RouteLink></li>
                        <li><RouteLink to="/search">Search</RouteLink></li>
                    </ul>
                    Logout
                </div>
                {this.props.children}
            </div>
        );
        //return this.state.isLoggedIn ?
        //(
        //) :
        //(
        //    <div>
        //        <Login />
        //    </div>
        //);
    }
});