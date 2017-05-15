import * as ActionTypes from 'Js/Modules/Redux/Reducers/SmartGuidedNavigation/Actions/actionTypes.js'
import { data } from '../../../Data/data' //Test data //TODO: remove
import { nodesReducer } from 'Js/Modules/Redux/Reducers/SmartGuidedNavigation/Reducers/nodesReducer.js'

export const iNavReducer = (state = data.iNav, action) => {
    switch (action.type) {
        case ActionTypes.TOGGLE_SELECTED:
            
            return {
                ...state,
                ...nodesReducer(state.Nodes, action)
            }
        default: 
            return state
    }

}
