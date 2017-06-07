import { createStore, applyMiddleware, compose } from 'redux';
import { createReducer } from './rootReducer'

// Middleware
import thunkMiddleware from 'redux-thunk'
import reduxMulti from 'redux-multi'
import { batchedSubscribe } from 'redux-batched-subscribe'

// Dev tools //TODO make prod version
const composeEnhancers = compose;

// Create a Redux store holding the state of your app.
// Its API is { subscribe, dispatch, getState }.


export function configureStore(/*preloadedState*/) {
    const store = createStore(rootReducer,
        /* preloadedState, */
        composeEnhancers(
            applyMiddleware(thunkMiddleware, reduxMulti),
            batchedSubscribe((notify) => {
                notify();
            })
        ));
    store.asyncReducers = {};
    return store;
}


export function injectAsyncReducer(store, name, asyncReducer) {
    store.asyncReducers[name] = asyncReducer;
    store.replaceReducer(createReducer(store.asyncReducers));
}


function rootReducer(state = {apples:10}, action) {
    return state
}

export const store = createStore(rootReducer)


