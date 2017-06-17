'use strict'

import global from 'global-object' 
import React from 'react'
import { Provider } from 'react-redux'
import * as Store from 'Js/Modules/Redux/Global/Store/store.server.js'

//TODO: remove
import { data } from 'Js/Modules/Redux/iNav/Data/data' //Test data 

//Enable Redux store globally
global.store = Store.configureStore()
global.injectAsyncReducer = Store.injectAsyncReducer

let ReactServerConnect = store => WrappedComponent => (reducerName, reducer) => {
         
        //TODO: can make this better with optional reducers vs props
        return (props) => {

            if ((typeof reducerName !== 'undefined' && typeof reducer !== 'undefined')) {
                //Pass the inital state to the reducer
                global.injectAsyncReducer(store, reducerName, reducer(data)) //TODO: Find alternative to adding globally here
            }

            return (
                <Provider store={store} >
                    <WrappedComponent {...props} />   
                </Provider>
            );

        }
        
}

ReactServerConnect = ReactServerConnect(global.store)

export {ReactServerConnect}