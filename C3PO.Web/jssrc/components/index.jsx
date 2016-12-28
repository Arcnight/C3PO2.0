import React from 'react';
import { Provider } from 'react-redux';
import { Router, Route, Redirect } from 'react-router';

import { login } from './pages';
import routes from './routes.jsx';
import main from 'Containers/main.jsx';

export function getProvider(history, store) {
    return (
        <Provider store={ store }>
            <Router history={ history }>
                <Route path="/" component={ main }>
                    { routes }
                    <Route path="login" component={ login } />
                    <Redirect from='logout' to='login' />
                </Route>
            </Router>
        </Provider>
    );
};