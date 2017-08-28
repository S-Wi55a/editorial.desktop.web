import { createStore, applyMiddleware, compose } from 'redux';
import { createReducer } from 'Js/Modules/Redux/Global/Reducers/rootReducer'


// This is a light weight version of the client store
// We should not need any enhancers here, just pass data to store and render components
export function configureStore() {
    const store = createStore(createReducer())
    store.asyncReducers = {};
    return store;
}


export function injectAsyncReducer(store, name, asyncReducer) {
    store.asyncReducers[name] = asyncReducer;
    store.replaceReducer(createReducer(store.asyncReducers));
}

