import { combineReducers } from 'redux'

// Because Redux needs at least one reducer or it will throw an error
const emptyReducer = (state = {}, action) => {
    return state
}

export function createReducer(asyncReducers = { emptyReducer }) {
    return combineReducers({
        ...asyncReducers
    });
}

