import { combineReducers } from 'redux'
import { localReducer } from 'redux-fractal';

// Because Redux needs at least one reducer or it will throw an error
const emptyReducer = (state = {}) => {
    return state
}

export function createReducer(asyncReducers = { emptyReducer }) {
    return combineReducers({
        local: localReducer,
        ...asyncReducers
    });
}

