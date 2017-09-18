import { combineReducers } from 'redux'

// Because Redux needs at least one reducer or it will throw an error
const emptyReducer = (state = {}) => {
    return state
}

export function createReducer(asyncReducers = { emptyReducer }) {
    return combineReducers({
        ...asyncReducers
    });
}

export function injectAsyncReducer(store, name, asyncReducer) {
    store.asyncReducers[name] = asyncReducer;
    store.replaceReducer(createReducer(store.asyncReducers));
}