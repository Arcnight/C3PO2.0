/*
import thunk from 'redux-thunk';
import { render } from 'react-dom'
import React, { Component } from 'react';
import { connect, Provider } from 'react-redux';
import { createStore, applyMiddleware } from 'redux';
import { browserHistory, Router, Route, Redirect } from 'react-router';
import { syncHistoryWithStore, routerMiddleware } from 'react-router-redux';

import rootReducer from './reducers';
import { siteURL } from './constants/urls';
import { sessionInfo } from './actions/auth';

import main from './components/main.jsx';
import routes from './components/routes.jsx';
import login from './components/pages/login.jsx';

// Creates the Redux reducer with the redux-thunk middleware, which allows us
// to do asynchronous things in the actions; need routerMiddleware to do pushes from dispatch
const store = createStore(rootReducer, applyMiddleware(routerMiddleware(browserHistory), thunk));

render(
	<Provider store={ store }>
      <Router history={ syncHistoryWithStore(browserHistory, store) }>
        <Route path="/" component={ main }>
            { routes }
            <Route path="login" component={ login } />
            <Redirect from='logout' to='login' />
        </Route>
      </Router>
	</Provider>,
	document.getElementById('root')
);
*/

import React from 'react';
import { render } from 'react-dom';

import { provider } from 'Components';

render(provider, document.getElementById('root'));