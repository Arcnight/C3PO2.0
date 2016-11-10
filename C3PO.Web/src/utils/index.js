import { assign } from 'lodash';
import { browserHistory } from 'react-router';

/**
 * Checks if any elements of a JSON object are empty
 * @param  {object} elements The object that should be checked
 * @return {boolean}         True if there are empty elements, false if there aren't
 */
export function AnyNullElements(elements) {
  for (let element in elements) {
    if (!elements[element]) return true;
  }

  return false;
}

/**
 * Forwards application to location
 * @param {string} location The location (url) the application is going to
 */
export function ForwardTo(location) {
  console.log('ForwardTo(' + location + ')');
  
  browserHistory.push(location);
}

/**
 * Object.assign is not yet fully supported in all browsers, so we fallback to
 * a polyfill
 */
export function Assign()
{
	return Object.assign || assign;
}

/**
 * Creates key/value pairs based on array of strings
 */
export function CreateConstants(...constants) {
    return constants.reduce((acc, constant) => {
        acc[constant] = constant;
        return acc;
    }, {});
}

/**
 * Returns reducer based on key-type "action.type"
 */
export function CreateReducer(initialState, reducerMap) {
    return (state = initialState, action) => {
        const reducer = reducerMap[action.type];

        return reducer ? reducer(state, action.payload) : state;
    };
}