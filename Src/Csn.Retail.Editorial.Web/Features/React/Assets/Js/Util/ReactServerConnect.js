'use strict'

import global from 'global-object' 
import React from 'react'
import { Provider } from 'react-redux'
import * as Store from 'Js/Modules/Redux/Global/Store/store.server.js'
import * as Reducers from '../Reducers/ReactServerReducersCollection.js'
import uuidv4 from 'uuid/v4'

global.stores = {} 

function storesFactory(stores) {
    return storeId => {

        if (stores.hasOwnProperty(storeId)) {
            return stores[storeId]
        } else {
            stores[storeId] = Store.configureStore()
            stores[storeId].injectAsyncReducer = Store.injectAsyncReducer
            stores[storeId].id = storeId

            return stores[storeId]  
        }
    }
}


let ReactServerConnect = storesFactory => WrappedComponent => (reducerKey, reducerName) => {
    
        return (props) => {

            //Should always pass a storeId as using the uuidv4 could create a memory leak
            //TODO: protection for non storeIds memory managment
            const store = (typeof props.storeId !== 'undefined') ? storesFactory(props.storeId) : storesFactory(uuidv4()) 

            // In the scenerio where we want to use the 'AddDataToStore' component 
            // We need ot manually add the reducers and init data, so we leave the option to do so
            reducerKey = typeof props.reducerKey !== 'undefined' ?  props.reducerKey : reducerKey
            reducerName = typeof props.reducerName !== 'undefined' ?  props.reducerName : reducerName

            if ((typeof reducerKey !== 'undefined' && typeof reducerName !== 'undefined')) {

                //Pass the inital state to the reducer
                //we are reserving the props.state to pass init state             
                if (!store.asyncReducers.hasOwnProperty(props.reducerKey)) { //This is to prevent duplcating data passed to Store if it alread has it
                    store.injectAsyncReducer(store, reducerKey, Reducers[reducerName](props.state || null))
                }
            }
            return (
                <Provider store={store} >
                    <WrappedComponent {...props} storeId={store.id} />   
                </Provider>
            );

        }
}
    
ReactServerConnect = ReactServerConnect(storesFactory(global.stores))

// Exports
export {ReactServerConnect}