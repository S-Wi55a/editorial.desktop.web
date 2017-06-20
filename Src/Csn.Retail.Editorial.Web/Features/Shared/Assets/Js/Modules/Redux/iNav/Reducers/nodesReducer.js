//Action Types
import * as iNav_ActionTypes from 'Js/Modules/Redux/iNav/Actions/actionTypes.js'

//Reducers
import facetsReducer from 'Js/Modules/Redux/iNav/Reducers/facetsReducer.js'

export const nodesReducer = (state, action) => {
    switch (action.type) {
    case iNav_ActionTypes.TOGGLE_IS_SELECTED:
        //loop through array to find which reducer to call
            return state.map((node) => {
            if (node.name === action.node) {
                return {
                    ...node,
                    ...{
                        facets: facetsReducer(node.facets, action)
                    }
                }
            }
            return node
        })
    default:
        return state
    }
}
