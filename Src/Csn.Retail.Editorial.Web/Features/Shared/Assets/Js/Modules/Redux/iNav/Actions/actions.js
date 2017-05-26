import * as ActionTypes from 'Js/Modules/Redux/iNav/Actions/actionTypes.js'
import fetch from 'isomorphic-fetch'

// Test Action
//var t = {
//type: 'SGN_TOGGLE_SELECTED',
//isSelected: true,
//node:'ArticleTypes', 
//facet:'Review'
//}



// Toggle IsSelected
export const toggleIsSelected = (isSelected, node, facet) => ({
    type: ActionTypes.TOGGLE_SELECTED,
    isSelected,
    node, //is string but can probably be changed to Id
    facet //is string but can probably be changed to Id
})

function requestiNav() {
    return {
        type: ActionTypes.FETCH_QUERY_REQUEST
        //should send payload to update UI
    }
}

function receiveiNav(data) {
    return {
        type: ActionTypes.FETCH_QUERY_SUCCESS,
        data
    }
}

export function fetchiNav(query) {

    query = 'http://editorial.ryvuss.csprd.com.au/v4/editorialListing?q=Service.CarSales.&inav&count=true'
    // Thunk middleware knows how to handle functions.
    // It passes the dispatch method as an argument to the function,
    // thus making it able to dispatch actions itself.

    return function (dispatch) {

        // First dispatch: the app state is updated to inform
        // that the API call is starting.

        dispatch(requestiNav())

        // The function called by the thunk middleware can return a value,
        // that is passed on as the return value of the dispatch method.

        // In this case, we return a promise to wait for.
        // This is not required by thunk middleware, but it is convenient for us.

        return fetch(query) // TODO: make dynamic
            .then(response => response.json())
            .then(json =>

                // We can dispatch many times!
                // Here, we update the app state with the results of the API call.
                
                dispatch(receiveiNav(json))
                // TODO: clear loader from UI

            )

        // TODO: catch any error in the network call.
    }
}