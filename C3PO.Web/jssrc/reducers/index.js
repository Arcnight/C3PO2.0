import { combineReducers } from 'redux';
import { routerReducer } from 'react-router-redux';

import auth from './auth.js';
import model from './model.js';
import viewbag from './viewbag.js';

// Add the reducer to your store on the `routing` key
export default combineReducers({
    auth,
    model,
    viewbag,
	routing: routerReducer
});