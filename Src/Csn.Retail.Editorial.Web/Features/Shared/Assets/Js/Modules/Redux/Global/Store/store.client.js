import { createStore, applyMiddleware, compose } from 'redux';
import { createReducer } from 'Js/Modules/Redux/Global/Reducers/rootReducer'

// Middleware
import reduxMulti from 'redux-multi'
import { batchedSubscribe } from 'redux-batched-subscribe'
import createSagaMiddleware from 'redux-saga'

const sagaMiddleware = createSagaMiddleware()

let middleware = [reduxMulti,sagaMiddleware]

const composeEnhancers =
    typeof window === 'object' &&
        window.__REDUX_DEVTOOLS_EXTENSION_COMPOSE__ ?   
        window.__REDUX_DEVTOOLS_EXTENSION_COMPOSE__({
            // Specify extension’s options like name, actionsBlacklist, actionsCreators, serialize...
        }) : compose;

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

