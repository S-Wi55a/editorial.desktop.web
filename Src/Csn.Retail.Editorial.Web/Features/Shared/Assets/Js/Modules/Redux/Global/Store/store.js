import { createStore, applyMiddleware, compose } from 'redux';
import rootReducer from '../Reducers/rootReducer'

/**
 * This is a reducer, a pure function with (state, action) => state signature.
 * It describes how an action transforms the state into the next state.
 *
 * The shape of the state is up to you: it can be a primitive, an array, an object,
 * or even an Immutable.js data structure. The only important part is that you should
 * not mutate the state object, but return a new object if the state changes.
 *
 * In this example, we use a `switch` statement and strings, but you can use a helper that
 * follows a different convention (such as function maps) if it makes sense for your
 * project.
 */

// Dev tools //TODO make prod version
const composeEnhancers = window.__REDUX_DEVTOOLS_EXTENSION_COMPOSE__ || compose;

// Create a Redux store holding the state of your app.
// Its API is { subscribe, dispatch, getState }.
let store = createStore(
    rootReducer,
    /* preloadedState, */
    composeEnhancers()
)


if (module.hot) {
    // Enable Webpack hot module replacement for reducers
    module.hot.accept('../Reducers/rootReducer', () => {
        const nextRootReducer = require('../Reducers/rootReducer').default
        store.replaceReducer(nextRootReducer)
    })
}
