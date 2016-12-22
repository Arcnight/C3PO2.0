import thunk from 'redux-thunk';
import { Provider } from 'react-redux';
import React, { Component } from 'react';
import { createStore, applyMiddleware } from 'redux';
import { browserHistory, Router, Route, Redirect } from 'react-router';
import { syncHistoryWithStore, routerMiddleware } from 'react-router-redux';

import reducer from 'Reducers';
import { login } from './pages';
import routes from './routes.jsx';
import main from 'Containers/main.jsx';

// Creates the Redux reducer with the redux-thunk middleware, which allows us
// to do asynchronous things in the actions; need routerMiddleware to do pushes from dispatch
export const store = createStore(reducer, applyMiddleware(routerMiddleware(browserHistory), thunk));

export const provider = (
  /* Tell the Router to use our enhanced history */
	<Provider store={ store }>
      <Router history={ syncHistoryWithStore(browserHistory, store) }>
        <Route path="/" component={ main }>
            { routes }
            <Route path="login" component={ login } />
            <Redirect from='logout' to='login' />
        </Route>
      </Router>
	</Provider>
);