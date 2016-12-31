import React from 'react';
import { Router } from 'react-router';
import { Provider } from 'react-redux';

import routes from './routes.jsx';

export function getProvider(history, store) {
    return (
        <Provider store={ store }>
            <Router history={ history }>
                { routes }
            </Router>
        </Provider>
    );
};