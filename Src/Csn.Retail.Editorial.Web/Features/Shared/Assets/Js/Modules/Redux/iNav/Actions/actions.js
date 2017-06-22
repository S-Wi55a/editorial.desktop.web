import * as ActionTypes from 'Js/Modules/Redux/iNav/Actions/actionTypes'

// Toggle IsSelected
export const toggleIsSelected = (isSelected, node, facet) => ({
    type: ActionTypes.TOGGLE_IS_SELECTED,
    isSelected,
    node, //is string but can probably be changed to Id
    facet //is string but can probably be changed to Id
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
