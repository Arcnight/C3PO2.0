import React from 'react';
import { match } from 'react-router';
import { Provider } from 'react-redux';
import serialize from 'serialize-javascript';
import { renderToString } from 'react-dom/server';
import RouterContext from 'react-router/lib/RouterContext';
import createHistory from 'react-router/lib/createMemoryHistory';

import Html from 'Containers/html.jsx';
import { configureStore } from 'Stores';
import routes from 'Components/routes.jsx';

export function renderView(callback, path, model, viewBag) {
    const _result = {
        html: null,
        status: 404,
        redirect: null
    };

    const _initialState = {
        model: model,
        viewBag: viewBag
    };

    const history = createHistory(path);

    match({ history: history, routes: routes, location: path }, (error, redirectLocation, renderProps) => {
        if (error) {
            _result.status = 500;
            _result.html = '<!DOCTYPE html><html><body>Internal error: ' + error + '</body></html>';
        } else if (redirectLocation) {
            _result.status = 302;
            _result.redirect = redirectLocation.pathname + redirectLocation.search;
        } else if (renderProps) {
            const notFound = renderProps.routes.filter((route) => route.status === 404).length > 0;

            if (notFound)
            {
                _result.status = 404;
                _result.html = '<!DOCTYPE html><html><body>Could not find route</body></html>';
            }
            else
            {
                const store = configureStore(_initialState, history);
                const component = (
                        <RouterContext {...renderProps} />
                );

                _result.status = 202;
                _result.html = '<!DOCTYPE html>' + renderToString(<Html component={ component } store={ store } />);
            }
        } else {
            _result.status = 404;
            _result.html = '<!DOCTYPE html><html><body>Could not find route; renderProps is null: ' + (renderProps == null) + '; path: \'' + path + '\'; routes: ' + serialize(routes) + '</body></html>';
        }
    });

    callback(null, _result);
}