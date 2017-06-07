'use strict'

import * as ReduxStore  from 'Js/Modules/Redux/Global/Store/store.js'
import React from 'react'
import { Provider } from 'react-redux'

//import SearchBar from 'Js/Modules/Redux/iNav/Containers/searchBar'

//const EnhancedComponent = higherOrderComponent(WrappedComponent);

console.log(global)


export const ReactServerConnect = function(WrappedComponent) {

    return (data) => {

        let {state, ...props} = data;

        state = {emptyReducer:'hi'}

        console.log(data)

        //Enable Redux store 
        const store = ReduxStore.configureStore(state)
        
        //let injectAsyncReducer = store.injectAsyncReducer

        return (
            <Provider store={store}>
                <WrappedComponent {...props} />           
            </Provider>
        );

    }
    }


