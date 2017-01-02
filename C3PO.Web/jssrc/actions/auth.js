import qs from 'qs';
import appCache from 'store';
import { push } from 'react-router-redux';

import { get, post } from 'Utils/http';
import { AnyNullElements } from 'Utils';
import { TOKEN_KEY } from 'Constants/keynames';
import { SITEURL, APIURL } from 'Constants/urls';
import userActions from 'Constants/actiontypes/user';

export const sessionInfo = {
    isLoggedIn: () => { return !!appCache.get(TOKEN_KEY); }
};

/* action creators */

export function LoginSuccess(response) {
    // Parse response from server and get token


    let token = 'jkl;jfskl;a';
    appCache.set(TOKEN_KEY, token);

	return {
		type: userActions.LOGIN_USER_SUCCESS,
		payload: {
  			username: username
		}
	};
}

export function LoginFailure(error) {
    appCache.remove(TOKEN_KEY);

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
        dispatch(push(SITEURL.Login));
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

	    post(APIURL.LOGIN,
        {
	        username: username,
	        password: password,
	        grant_type: 'password'
	    },
	    {
			transformRequest: function(data) { return qs.stringify(data); },
	        headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
	    })
		.then((response) => {
		    dispatch(LoginSuccess(response));

		    //dispatch(<stop animation event>)

		    dispatch(push(redirectUrl));
		})
		.catch((response) => { LoginFailure({ response: response }) });
	}
}

/**
 * Logs a user out
 */
export function LogoutUser()
{
    appCache.remove(TOKEN_KEY);

	/*
    return (dispatch) => {
        get(LOGOUT_URL)
        .then(function (response) {
        	dispatch(GetLogoutState());
        })
        .catch(function (response) {
            alert(response);
        });

        //ForwardTo(SITEURL.LOGIN_URL);
    */

    return { type: userActions.LOGOUT_USER };
}