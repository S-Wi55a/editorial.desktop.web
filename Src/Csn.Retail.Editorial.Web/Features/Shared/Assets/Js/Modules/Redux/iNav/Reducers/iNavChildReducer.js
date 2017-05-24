import * as ActionTypes from 'Js/Modules/Redux/iNav/Actions/actionTypes.js'
import { data } from 'Js/Modules/Redux/iNav/Data/data' //Test data //TODO: remove
import { nodesReducer } from 'Js/Modules/Redux/iNav/Reducers/nodesReducer.js'

export const iNavReducer = (state = data, action) => {
    switch (action.type) {
    case ActionTypes.TOGGLE_SELECTED:

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
