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

import app from './components/app.jsx';
import routes from './components/routes.jsx';
import login from './components/pages/login.jsx';

// Creates the Redux reducer with the redux-thunk middleware, which allows us
// to do asynchronous things in the actions; need routerMiddleware to do pushes from dispatch

//const createStoreWithMiddleware = applyMiddleware(routerMiddleware(browserHistory), thunk)(createStore);
//const store = createStoreWithMiddleware(rootReducer);
const store = createStore(rootReducer, applyMiddleware(routerMiddleware(browserHistory), thunk));

render(
  /* Tell the Router to use our enhanced history */
	<Provider store={ store }>
      <Router history={ syncHistoryWithStore(browserHistory, store) }>
        <Route path="/" component={ app }>
            { routes }
            <Route path="login" component={ login } />
            <Redirect from='logout' to='login' />
        </Route>
      </Router>
	</Provider>,
	document.getElementById('root')
);