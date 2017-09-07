'use strict'

import React from 'react'
import { Provider } from 'react-redux'
import * as Store from 'Redux/Global/Store/store.server.js'
import * as Reducers from '../Reducers/reactServerReducersCollection.js'

function storesFactory() {

    const store = Store.configureStore()
    store.injectAsyncReducer = Store.injectAsyncReducer

    return store  

}

const ReactServerConnect = WrappedComponent => (reducerKey, reducerName, addReducerKey = false) => {
    return (props) => {

        const store = storesFactory()

        if ((typeof reducerKey !== 'undefined' && typeof reducerName !== 'undefined')) {

            //Pass the inital state to the reducer
            //we are reserving the props.state to pass init state             
            store.injectAsyncReducer(store, reducerKey, Reducers[reducerName](props.state || null))

        }
        // addReducerKey is for preload store only and will be ignore otherwise
        return (
            <Provider store={store} >
                <WrappedComponent {...props} reducerKey={addReducerKey && reducerKey}/>
            </Provider>

        );

    }
}
    
// Exports
export {ReactServerConnect}