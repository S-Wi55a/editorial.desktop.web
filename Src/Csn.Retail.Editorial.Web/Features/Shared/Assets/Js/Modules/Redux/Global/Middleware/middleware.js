import { applyMiddleware, compose } from 'redux';

// Middleware
import reduxMulti from 'redux-multi'
import { batchedSubscribe } from 'redux-batched-subscribe'
import createSagaMiddleware from 'redux-saga'
import ReduxThunk from 'redux-thunk'

// we use this ref later
const sagaMiddleware = createSagaMiddleware()

const middlewareDev = [
    require('redux-immutable-state-invariant').default() 
    ]

const middlewareProd = [reduxMulti, ReduxThunk]

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


export { enhancer, sagaMiddleware }