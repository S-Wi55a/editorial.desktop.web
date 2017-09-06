'use strict'

import React from 'react'
import { Provider } from 'react-redux'
import * as Store from 'Redux/Global/Store/store.server.js'
import * as Reducers from '../Reducers/ReactServerReducersCollection.js'

function storesFactory() {

    const store = Store.configureStore()
    store.injectAsyncReducer = Store.injectAsyncReducer

    return store  

}

const ReactServerConnect = WrappedComponent => (reducerKey, reducerName) => {
    
    return (props) => {

        const store = storesFactory()

        // In the scenerio where we want to use the 'AddDataToStore' component 
        // We need ot manually add the reducers and init data, so we leave the option to do so
        reducerKey = typeof props.reducerKey !== 'undefined' ?  props.reducerKey : reducerKey
        reducerName = typeof props.reducerName !== 'undefined' ?  props.reducerName : reducerName

        if ((typeof reducerKey !== 'undefined' && typeof reducerName !== 'undefined')) {

            //Pass the inital state to the reducer
            //we are reserving the props.state to pass init state             
            store.injectAsyncReducer(store, reducerKey, Reducers[reducerName](props.state || null))

        }

        return (
            <Provider store={store} >
                <WrappedComponent {...props} />   
            </Provider>

        );

    }
}
    
// Exports
export {ReactServerConnect}