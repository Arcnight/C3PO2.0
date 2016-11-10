import appCache from 'store';
import { push } from 'react-router-redux';

import { AnyNullElements } from '../utils';
import { get, post } from '../utils/httpMethods';
import { siteURL, apiURL } from '../constants/urls';
import userActions from '../constants/actiontypes/user';

export const sessionInfo = {
	isLoggedIn: () => { return !!appCache.get('token'); }
};

/* action creators */

export function LoginSuccess(token, username) {
	appCache.set('token', token);

	return {
		type: userActions.LOGIN_USER_SUCCESS,
		payload: {
  			username: username
		}
	};
}

export function LoginFailure(error) {
	appCache.remove('token');

	let errMsg = error.response.statusText;

	console.log('login failed - ' + errMsg);

	return {
		type: userActions.LOGIN_USER_FAILURE,
		payload: {
  			statusText: errMsg,
  			status: error.response.status
		}
	};
}

export function LoginRequest() {
	return { type: userActions.LOGIN_USER_REQUEST };
}

export function LogoutAndRedirect() {
    return (dispatch, state) => {
        dispatch(LogoutUser());
        dispatch(push('/login'));
    }
}

/**
 * Logs a user in
 * @param  {string} username - The username of the user to be logged in
 * @param  {string} password - The password of the user to be logged in
 * @param  {string} redirectUrl - The url to redirect to after successful login
 */
export function LoginUser(username, password, redirectUrl) {
	return (dispatch) => {
		// Show the loading indicator, hide the last error
		//dispatch(sendingRequest(true));

		//removeLastFormError();

		// If no username or password was specified, throw a field-missing error
		if (AnyNullElements({ username, password })) {
	  		//requestFailed({ type: "field-missing" });
	  		//dispatch(sendingRequest(false));
  			dispatch(
  				LoginFailure({
                    response: {
                        status: 403,
                        statusText: 'No username/password provided'
                    }
                })
            );
		  
			return;
		}

		// Generate salt for password encryption
		//const salt = genSalt(username);

		//dispatch(<animation event>)

		/*
		post(apiURL.LOGIN_URL, formData)
		.then((response) => {
			// Parse response from server and get token
		    dispatch(LoginResult(true));
		    
		    //dispatch(<stop animation event>)

		    ForwardTo(siteURL.DASHBOARD_URL);
		})
		.catch((response) => {
			console.log('login failed');
			dispatch(LoginResult(false));
		    alert(response);
		});
		*/

		dispatch(LoginSuccess('jkl;fdsjk;lfa', username));

		console.log(redirectUrl);
		dispatch(push(redirectUrl));
	}
}

/**
 * Logs a user out
 */
export function LogoutUser()
{
    appCache.remove('token');

	/*
    return (dispatch) => {
        get(LOGOUT_URL)
        .then(function (response) {
        	dispatch(GetLogoutState());
        })
        .catch(function (response) {
            alert(response);
        });

        //ForwardTo(siteURL.LOGIN_URL);
    */

    return { type: userActions.LOGOUT_USER };
}