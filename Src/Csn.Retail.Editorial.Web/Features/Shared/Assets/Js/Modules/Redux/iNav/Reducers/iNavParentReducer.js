import global from 'global-object' 

import { combineReducers } from 'redux'
import * as ActionTypes from 'Js/Modules/Redux/iNav/Actions/actionTypes'
import { iNavChildReducer } from 'Js/Modules/Redux/iNav/Reducers/iNavChildReducer'
import { iNavQueryReducer } from 'Js/Modules/Redux/iNav/Reducers/iNavQueryReducer'

let initState = global.__PRELOADED_STATE__ ? global.__PRELOADED_STATE__.iNav || {} : {}

//We wrap the reducer to pass init data to it for it to work in ReactJS.NET
export const iNavParentReducerPassInitData = initState => {

    return (state = initState, action) => {
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
        case ActionTypes.FETCH_QUERY_SUCCESS:
                console.log('Update view', action)
            return {
                ...state,
                ...{
                    iNav: iNavChildReducer(action.data.iNav, action),
                    count: action.data.count
                }
            }
        default: 
            return state
        }

    }
}

export const iNavParentReducer = iNavParentReducerPassInitData(initState)