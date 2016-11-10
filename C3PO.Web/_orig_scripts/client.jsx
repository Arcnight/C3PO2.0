var React = require('react');
var Render = require('react-dom').render;
var Router = require('react-router').Router;
var browserHistory = require('react-router').browserHistory;

var routes = require('./routes.jsx');

Render(<Router history={browserHistory}>
            {routes}
       </Router>, document.getElementById("root"));