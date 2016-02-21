var React = require('react');
var ReactDOM = require('react-dom');
var ReactForm = require('react-form-data');

var Route = require('react-router').Route;
var Router = require('react-router').Router;
var RouteLink = require('react-router').Link;
var Redirect = require('react-router').Redirect;
var IndexRoute = require('react-router').IndexRoute;
var browserHistory = require('react-router').browserHistory;

var Dashboard = require('./pages/dashboard.jsx');
var Search = require('./pages/search.jsx');
var DhowSearch = require('./pages/dhowsearch.jsx');
var MapSearch = require('./pages/mapsearch.jsx');
var Metrics = require('./pages/Metrics.jsx');
var Event = require('./pages/event.jsx');
var Vessel = require('./pages/vessel.jsx');
var Person = require('./pages/person.jsx');
var Company = require('./pages/company.jsx');
var Links = require('./pages/links.jsx');
var Templates = require('./pages/templates.jsx');
var Template = require('./pages/template.jsx');

var auth = require('./auth.js');
var shivIE = require('shivie8');

shivIE(document);

var Login = React.createClass({
    mixins: [ReactForm],

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
                    <form onChange={ this.updateFormData } onSubmit={ this.login }>
                        <input type="text" name="username" placeholder="Username" />
                        <input type="text" name="password" placeholder="Password" />
                        <input type="submit" value="login" />
                    </form>
                </div>
            </div>
        );
    }
});

var C3PO = React.createClass({
    getInitialState: function() {
        return {
            isLoggedIn: auth.isLoggedIn()
        };
    },

    UpdateAuthState: function (loggedIn) {
        this.setState({
            isLoggedIn: loggedIn
        });
    },

    componentWillMount: function () {
        auth.onChange = this.UpdateAuthState;
        auth.login();
    },

    render: function () {
        return this.state.isLoggedIn ?
        (
            <div>
                <div>
                    Header
                </div>
                <div>
                    <ul>
                        <li>
                            <RouteLink to="/logout">Log out</RouteLink>
                        </li>
                    </ul>
                </div>
                <div>
                    {this.props.children}
                </div>
            </div>
        ) :
        (
            <div>
                HI THERE
            </div>
        );
    }
});

//var Logout = React.createClass({
//    componentDidMount() {
//        auth.logout();
//    },
//
//    render() {
//        return <p>You are now logged out</p>
//    }
//});

var routes = (
    <Router history={browserHistory}>
        <Route name="c3po" path="/" handler={C3PO}>
            <IndexRoute handler={Dashboard} />
            <Route name="login" path="/login" handler={Login} />
            <Route name="search" path="/search" handler={Search} />
            <Route name="dhowsearch" path="/dhowsearch" handler={DhowSearch} />
            <Route name="mapsearch" path="/mapsearch" handler={MapSearch} />
            <Route name="metrics" path="/metrics" handler={Metrics} />
            <Route name="event" path="/event" handler={Event} />
            <Route name="vessel" path="/vessel" handler={Vessel} />
            <Route name="person" path="/person" handler={Person} />
            <Route name="company" path="/company" handler={Company} />
            <Route name="links" path="/links" handler={Links} />
            <Route name="templates" path="/templates" handler={Templates} />
            <Route name="template" path="/template(/:rtid)" handler={Template} />
            <Redirect from='logout' to='/login' />
        </Route>
    </Router>
);

ReactDOM.render(routes, document.getElementById("app"));

function requireAuth(nextState, replace) {
    //if (!auth.isLoggedIn()) {
    //    replace({
    //        pathname: '/login',
    //        state: { nextPathname: nextState.location.pathname }
    //    })
    //}
}

//module.exports = C3PO;

//ReactDOM.render(<C3PO />, document.getElementById("app"));