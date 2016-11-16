import { Assign, CreateReducer } from '../utils';
import userActions from '../constants/actiontypes/user';

const initialState = {
    userName: null,
    statusText: null,
    isAuthenticated: false,
    isAuthenticating: false
};

const reducer = CreateReducer(initialState, {
    [userActions.LOGIN_USER_REQUEST]: (state, payload) => {
        return Assign(state, {
            'statusText': null,
            'isAuthenticating': true
        });
    },

    [userActions.LOGIN_USER_SUCCESS]: (state, payload) => {
        let userName = payload.username;

        console.log(userName + ' logged in');

        return Assign(state, {
            'userName': userName,
            'isAuthenticated': true,
            'isAuthenticating': false,
            'statusText': 'You have been successfully logged in.'
        });
    },

    [userActions.LOGIN_USER_FAILURE]: (state, payload) => {
        return Assign(state, {
            'userName': null,
            'isAuthenticated': false,
            'isAuthenticating': false,
            'statusText': `Authentication Error: ${payload.status} ${payload.statusText}`
        });
    },

    [userActions.LOGOUT_USER]: (state, payload) => {
        console.log(state.username + ' logged out');

        return Assign(state, {
            'userName': null,
            'isAuthenticated': false,
            'statusText': 'You have been successfully logged out.'
        });
    }
});

export default reducer;