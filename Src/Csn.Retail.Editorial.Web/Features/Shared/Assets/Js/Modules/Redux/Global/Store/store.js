import { createStore, applyMiddleware, compose } from 'redux';
import { createReducer} from '../Reducers/rootReducer'


// Middleware
import thunkMiddleware from 'redux-thunk'
import reduxMulti from 'redux-multi'
import { batchedSubscribe } from 'redux-batched-subscribe'

//import { fetchiNav } from 'Js/Modules/Redux/iNav/Actions/actions.js'

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


export function configureStore(/*preloadedState*/) {
    const store = createStore(createReducer(),
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

// TO test ryvuss when it is public
//setTimeout(function() {
//    console.log('starting fetch')
//    store.dispatch(fetchiNav())
//}, 2000)
