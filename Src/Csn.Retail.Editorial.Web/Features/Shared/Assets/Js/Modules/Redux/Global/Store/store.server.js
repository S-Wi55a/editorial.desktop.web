import { createStore } from 'redux';
import { createReducer } from 'Redux/Global/Reducers'

// This is a light weight version of the client store
// We should not need any enhancers here, just pass data to store and render components
export function configureStore() {
    const store = createStore(createReducer())
    store.asyncReducers = {};
    return store;
}

