'use strict'

import global from 'global-object' 
import React from 'react'
import { Provider } from 'react-redux'
import * as Store from 'Js/Modules/Redux/Global/Store/store.js'

//Enable Redux store globally
global.store = Store.configureStore()
global.injectAsyncReducer = Store.injectAsyncReducer

let ReactServerConnect = store => WrappedComponent => (reducerName, reducer) => {

        if (!(reducerName === 'undefiend' && reducer === 'undefiend')) {
            global.injectAsyncReducer(store, reducerName, reducer) //TODO: Don't like this
        }
            
        return (props) => {

            return (
                <Provider store={store} >
                    <WrappedComponent {...props} />   
                </Provider>
            );

        }
        
}

ReactServerConnect = ReactServerConnect(global.store)

export {ReactServerConnect}