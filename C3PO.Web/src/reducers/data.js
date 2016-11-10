import { Assign, CreateReducer } from '../utils';
import { dataActionTypes } from '../constants/actiontypes';

const initialState = {
    data: null,
    isFetching: false
};

export default createReducer(initialState, {
    [dataActionTypes.RECEIVE_PROTECTED_DATA]: (state = initialState, payload) => {
        return Assign({}, state, {
            'data': payload.data,
            'isFetching': false
        });
    },

    [dataActionTypes.FETCH_PROTECTED_DATA_REQUEST]: (state = initialState, payload) => {
        return Assign({}, state, {
            'isFetching': true
        });
    }
});