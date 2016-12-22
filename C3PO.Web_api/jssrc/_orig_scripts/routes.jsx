var React = require('react');
var Router = require('react-router');

var Route = Router.Route;
var Redirect = Router.Redirect;
var IndexRoute = Router.IndexRoute;

var auth = require('./auth.js');

var _layout = require('./layouts/_home.jsx');
var dashboard = require('./pages/dashboard.jsx');
var login = require('./pages/login.jsx');
var search = require('./pages/search.jsx');
var dhowsearch = require('./pages/dhowsearch.jsx');
var mapsearch = require('./pages/mapsearch.jsx');
var metrics = require('./pages/metrics.jsx');
var event = require('./pages/event.jsx');
var vessel = require('./pages/vessel.jsx');
var person = require('./pages/person.jsx');
var company = require('./pages/company.jsx');
var links = require('./pages/links.jsx');
var templates = require('./pages/templates.jsx');
var template = require('./pages/template.jsx');

const routes = (
    <Route name="c3po" path="/" component={_layout}>
        <IndexRoute component={dashboard} onEnter={requireAuth} />
        <Route path="login" component={login} />
        <Route path="search" component={search} onEnter={requireAuth} />
        <Route path="dhowsearch" component={dhowsearch} onEnter={requireAuth} />
        <Route path="mapsearch" component={mapsearch} onEnter={requireAuth} />
        <Route path="metrics" component={metrics} onEnter={requireAuth} />
        <Route path="event(/:id)" component={event} onEnter={requireAuth} />
        <Route path="vessel(/:id)" component={vessel} onEnter={requireAuth} />
        <Route path="person(/:id)" component={person} onEnter={requireAuth} />
        <Route path="company(/:id)" component={company} onEnter={requireAuth} />
        <Route path="links" component={links} onEnter={requireAuth} />
        <Route path="templates" component={templates} onEnter={requireAuth} />
        <Route path="template(/:id)" component={template} onEnter={requireAuth} />
        <Redirect from='logout' to='login' />
    </Route>
);

function requireAuth(nextState, replace) {
    //if (!auth.isLoggedIn()) {
    //    replace({
    //        pathname: '/login',
    //        state: { nextPathname: nextState.location.pathname }
    //    })
    //}
}

module.exports = routes;