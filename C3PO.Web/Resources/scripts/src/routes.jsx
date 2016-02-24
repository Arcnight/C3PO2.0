var Route = require('react-router').Route;
var Redirect = require('react-router').Redirect;
var IndexRoute = require('react-router').IndexRoute;

var auth = require('./auth.js');

const routes = (
    <Route name="c3po" path="/" component={require('./pages/_layout.jsx')}>
        <IndexRoute component={require('./pages/dashboard.jsx')} onEnter={requireAuth} />
        <Route name="login" path="login" component={require('./pages/login.jsx')} />
        <Route name="search" path="search" component={require('./pages/search.jsx')} onEnter={requireAuth} />
        <Route name="dhowsearch" path="dhowsearch" component={require('./pages/dhowsearch.jsx')} onEnter={requireAuth} />
        <Route name="mapsearch" path="mapsearch" component={require('./pages/mapsearch.jsx')} onEnter={requireAuth} />
        <Route name="metrics" path="metrics" component={require('./pages/Metrics.jsx')} onEnter={requireAuth} />
        <Route name="event" path="event(/:id)" component={require('./pages/event.jsx')} onEnter={requireAuth} />
        <Route name="vessel" path="vessel(/:id)" component={require('./pages/vessel.jsx')} onEnter={requireAuth} />
        <Route name="person" path="person(/:id)" component={require('./pages/person.jsx')} onEnter={requireAuth} />
        <Route name="company" path="company(/:id)" component={require('./pages/company.jsx')} onEnter={requireAuth} />
        <Route name="links" path="links" component={require('./pages/links.jsx')} onEnter={requireAuth} />
        <Route name="templates" path="templates" component={require('./pages/templates.jsx')} onEnter={requireAuth} />
        <Route name="template" path="template(/:id)" component={require('./pages/template.jsx')} onEnter={requireAuth} />
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