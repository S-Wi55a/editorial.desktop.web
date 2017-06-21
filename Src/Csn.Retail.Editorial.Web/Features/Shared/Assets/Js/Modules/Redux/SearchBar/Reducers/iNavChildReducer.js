import * as ActionTypes from 'Js/Modules/Redux/iNav/Actions/actionTypes'
import { nodesReducer } from 'Js/Modules/Redux/iNav/Reducers/nodesReducer'

export const iNavChildReducer = (state, action) => {
    switch (action.type) {
    case ActionTypes.TOGGLE_IS_SELECTED:
        return {
            ...state,
            ...{
                nodes: nodesReducer(state.nodes, action)
               }
            }
        default: 
            return state
    }

}
