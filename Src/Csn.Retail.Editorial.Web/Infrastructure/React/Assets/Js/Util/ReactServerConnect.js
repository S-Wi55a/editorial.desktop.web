'use strict'

import React from 'react'
import { Provider } from 'react-redux'
import { configureStore } from 'Redux/Global/Store/store.server.js'


function storesFactory() {
    return configureStore()  
}

const ReactServerConnect = WrappedComponent => (reducerKey, reducer, addReducerKey = false) => {
    return (props) => {

        const store = storesFactory()

        if ((typeof reducerKey !== 'undefined' && typeof reducer !== 'undefined')) {

            //Pass the inital state to the reducer
            //we are reserving the props.state to pass init state             
            store.addReducer(reducerKey, reducer(props.state || null))

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