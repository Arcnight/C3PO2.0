var app = require('express')();
var React = require('react');
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
            reactHtml: '<!DOCTYPE html>' + React.renderToString(<Handler />)
        });
    });
});

//app.use((req, res) => {  
//    Router.run(routes, req.path, (Root, state) => {
//        res.send('<!DOCTYPE html>' + React.renderToString( <Root/> ));
//    });
//});