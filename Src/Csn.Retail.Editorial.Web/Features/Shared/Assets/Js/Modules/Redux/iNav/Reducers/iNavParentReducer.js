import * as ActionTypes from 'Js/Modules/Redux/iNav/Actions/actionTypes'
import { data } from 'Js/Modules/Redux/iNav/Data/data' //Test data //TODO: remove
import { iNavChildReducer } from 'Js/Modules/Redux/iNav/Reducers/iNavChildReducer'

export const iNavParentReducer = (state = data, action) => {
    switch (action.type) {
    case ActionTypes.TOGGLE_SELECTED:
        return {
            ...state,
            ...{
                iNav: iNavChildReducer(state.iNav, action)
               }
            }
        default: 
            return state
    }

}
