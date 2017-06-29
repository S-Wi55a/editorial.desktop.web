'use strict'

import global from 'global-object' 
import React from 'react'
import { Provider } from 'react-redux'
import * as Store from 'Js/Modules/Redux/Global/Store/store.server.js'
import * as Reducers from '../Reducers/ReactServerReducersCollection.js'


//Enable Redux store globally
global.store = Store.configureStore()
global.store.injectAsyncReducer = Store.injectAsyncReducer

let ReactServerConnect = store => WrappedComponent => (reducerKey, reducerName) => {
         
        return (props) => {

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
                    <WrappedComponent {...props} pear={true}/>   
                </Provider>
            );

        }
        
}

ReactServerConnect = ReactServerConnect(global.store)

export {ReactServerConnect}