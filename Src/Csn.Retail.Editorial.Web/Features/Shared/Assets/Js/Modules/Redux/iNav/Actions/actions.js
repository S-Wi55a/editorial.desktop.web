import * as ActionTypes from 'Js/Modules/Redux/iNav/Actions/actionTypes'

// Toggle IsSelected
export const toggleIsSelected = (node, facet) => ({
    type: ActionTypes.TOGGLE_IS_SELECTED,
    node, 
    facet 
})

// Remove Bread Crumb
export const removeBreadCrumb = (facet) => ({
    type: ActionTypes.REMOVE_BREAD_CRUMB,
    facet 
})


// Fetch data
export const fetchQueryRequest = (query) => ({
    type: ActionTypes.FETCH_QUERY_REQUEST,
    payload: {
        query
    }
})

export const reset = (query) => ({
    type: ActionTypes.RESET,
    payload: {
        query
    }
})
