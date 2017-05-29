import { combineReducers } from 'redux'
import * as ActionTypes from 'Js/Modules/Redux/iNav/Actions/actionTypes'
import { data } from 'Js/Modules/Redux/iNav/Data/data' //Test data //TODO: remove
import { iNavChildReducer } from 'Js/Modules/Redux/iNav/Reducers/iNavChildReducer'
import { iNavQueryReducer } from 'Js/Modules/Redux/iNav/Reducers/iNavQueryReducer'

const initState = window.iNavState || data

export const iNavParentReducer = (state = initState, action) => {
    switch (action.type) {
        case ActionTypes.TOGGLE_SELECTED:
            return {
                ...state,
                ...{
                    iNav: iNavChildReducer(state.iNav, action)
                   }
                }
        case ActionTypes.UPDATE_QUERY_STRING:
            return {
                ...state,
                ...{
                    iNavQuery: iNavQueryReducer(state.iNavQuery, action, state) //Pass it full state as this reducer will return the segment needed
                }
            }
        default: 
            return state
    }

}
