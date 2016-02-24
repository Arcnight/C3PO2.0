var Render = require('react-dom').render;
var Router = require('react-router').Router;
var browserHistory = require('react-router').browserHistory;

var routes = require('./routes.jsx');
var shivIE = require('shivie8');

shivIE(document);

Render(<Router history={browserHistory}>
            {routes}
       </Router>, document.getElementById("app"));