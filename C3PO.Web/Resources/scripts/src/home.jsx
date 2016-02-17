var React = require("react");

var Home = React.createClass({
    propTypes: {
        isLoggedIn: React.PropTypes.bool.isRequired
    },

    render: function () {
        return !this.props.isLoggedIn ?
        (
            <div>Please login</div>
        ) :
        (
            <div className="login-box auth0-box before">
                <img src="https://i.cloudup.com/StzWWrY34s.png" />
                <h3>Auth0 Example</h3>
                <p>Zero friction identity infrastructure, built for developers</p>
            </div>
        );
    }
});

module.exports = Home;