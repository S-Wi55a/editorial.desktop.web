import { createStore, } from 'redux';
import { createReducer } from 'Redux/Global/Reducers/rootReducer'
import { enhancer, sagaMiddleware } from 'Redux/Global/Middleware/middleware'

export function configureStore(preloadedState) {
    return {
        ...createStore(createReducer(), preloadedState, enhancer),
        runSaga: sagaMiddleware.run,
        asyncReducers : {}
    }
}

export function injectAsyncReducer(store, name, asyncReducer) {
    store.asyncReducers[name] = asyncReducer;
    store.replaceReducer(createReducer(store.asyncReducers));
}

