//Action Types
import * as iNav_ActionTypes from 'Js/Modules/Redux/Reducers/SmartGuidedNavigation/Actions/actionTypes.js'

//Reducers
import facetsReducer from 'Js/Modules/Redux/Reducers/SmartGuidedNavigation/Reducers/facetsReducer.js'
import { articleTypeReducer } from 'Js/Modules/Redux/Reducers/SmartGuidedNavigation/Reducers/articleTypeReducer.js'
import { bodyTypeReducer } from 'Js/Modules/Redux/Reducers/SmartGuidedNavigation/Reducers/bodyTypeReducer.js'
import { makeModelReducer } from 'Js/Modules/Redux/Reducers/SmartGuidedNavigation/Reducers/makeModelReducer.js'


export const nodesReducer = (state, action) => {
    debugger
    switch (action.type) {
    case 'BODY_TYPE':
        return state
    case 'MAKE_MODEL':
        return { ...state, ...{ now: 'hot' } }
    case 'ARTICLE_TYPE':
        return { ...state, ...{ now: 'hot' } }
    case iNav_ActionTypes.TOGGLE_SELECTED:
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
