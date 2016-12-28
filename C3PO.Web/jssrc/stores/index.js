import { merge } from 'lodash';
import thunk from 'redux-thunk';
import { routerMiddleware } from 'react-router-redux';
import { createStore, applyMiddleware, compose } from 'redux';

import reducer from 'Reducers';
import { authInitialState } from './auth.js';
import { userInitialState } from './user.js';

export function getInitialAppStore() {
    let appStore = {};

    merge(appStore, authInitialState);
    merge(appStore, userInitialState);

    return appStore;
}

// Creates the Redux reducer with the redux-thunk middleware, which allows us
// to do asynchronous things in the actions; need routerMiddleware to do pushes from dispatch
export function configureStore(initialState, history) {
    const enhancer = compose(applyMiddleware(thunk), applyMiddleware(routerMiddleware(history)))(createStore);

    return enhancer(reducer, initialState);
};