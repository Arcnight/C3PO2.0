import { combineReducers } from 'redux';
import { routerReducer } from 'react-router-redux';

import auth from './auth.js';

// Add the reducer to your store on the `routing` key
export default combineReducers({
	auth,
	routing: routerReducer
});