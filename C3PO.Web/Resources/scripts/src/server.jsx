var path = require('path');
var app = require('express')();
var Router = require('react-router');

var routes = require('./routes.jsx');

app.set('port', process.env.PORT || 8080);

app.use(function (req, res, next) {
    var context = {
        routes: routes,
        location: req.url
    };

    Router.create(context).run(function (Handler, state) {
        res.render('layout', {
            reactHtml: React.renderToString(<Handler />)
        });
    });
});