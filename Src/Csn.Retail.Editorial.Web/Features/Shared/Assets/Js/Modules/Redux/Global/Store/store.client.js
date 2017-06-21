import { createStore, applyMiddleware, compose } from 'redux';
import { createReducer } from 'Js/Modules/Redux/Global/Reducers/rootReducer'

// Middleware
import reduxMulti from 'redux-multi'
import { batchedSubscribe } from 'redux-batched-subscribe'
import createSagaMiddleware from 'redux-saga'

// we use this ref later
const sagaMiddleware = createSagaMiddleware()

const middlewareDev = [
    require('redux-immutable-state-invariant').default(),
    require('redux-unhandled-action').default((action) => console.error(`Action ${action} didn't lead to creation of a new state object`, action)) ]

const middlewareProd = [reduxMulti,sagaMiddleware]

//Array of middlewars to attach to the store
const middleware = process.env.NODE_ENV !== 'production' ? middlewareDev.concat(middlewareProd) : middlewareProd;

//Redux DevTools
const composeEnhancers = (process.env.NODE_ENV !== 'production' && window.__REDUX_DEVTOOLS_EXTENSION_COMPOSE__) ? 
    window.__REDUX_DEVTOOLS_EXTENSION_COMPOSE__ : compose;


// Compose our middleware for te store
const enhancer = composeEnhancers(
    applyMiddleware(...middleware),
    batchedSubscribe((notify) => {
        notify();
    })
)

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

