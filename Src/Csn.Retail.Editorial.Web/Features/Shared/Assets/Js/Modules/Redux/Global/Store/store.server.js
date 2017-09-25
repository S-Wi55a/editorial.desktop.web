import { createStore } from 'redux-async-reducer';

// This is a light weight version of the client store
// We should not need any enhancers here, just pass data to store and render components
export function configureStore() {
    return createStore()
}

