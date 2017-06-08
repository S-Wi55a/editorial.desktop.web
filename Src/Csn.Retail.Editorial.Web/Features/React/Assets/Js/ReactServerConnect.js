'use strict'

import React from 'react'
import { Provider } from 'react-redux'

//import SearchBar from 'Js/Modules/Redux/iNav/Containers/searchBar'

//const EnhancedComponent = higherOrderComponent(WrappedComponent);

const store = global.store

let ReactServerConnect = function(store) {

    return (WrappedComponent) => {

        console.log(global.count++)
        //console.log('Component ' + ReducerName, global.count)

        return (props) => {

            return (
                <Provider store={store} >
                    <WrappedComponent {...props} />   
                </Provider>
            );

        }
    }


}

ReactServerConnect = ReactServerConnect(store)

export {ReactServerConnect}