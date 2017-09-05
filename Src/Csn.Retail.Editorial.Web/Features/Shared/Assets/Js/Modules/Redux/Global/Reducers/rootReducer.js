import { combineReducers } from 'redux'
import { reducer as uiReducer } from 'redux-ui'

// Because Redux needs at least one reducer or it will throw an error
const emptyReducer = (state = {}) => {
    return state
}

export function createReducer(asyncReducers = { emptyReducer }) {
    return combineReducers({
        ui: uiReducer,
        ...asyncReducers
    });
}

