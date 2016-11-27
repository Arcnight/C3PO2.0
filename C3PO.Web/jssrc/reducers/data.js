import { Assign, CreateReducer } from 'Utils';
import { dataActionTypes } from 'Constants/actiontypes';

const initialState = {
    data: null,
    isFetching: false
};

export default createReducer(initialState, {
    [dataActionTypes.RECEIVE_PROTECTED_DATA]: (state, payload) => {
        return Assign(state, {
            'data': payload.data,
            'isFetching': false
        });
    },

    [dataActionTypes.FETCH_PROTECTED_DATA_REQUEST]: (state, payload) => {
        return Assign(state, {
            'isFetching': true
        });
    }
});