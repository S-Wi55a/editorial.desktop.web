import global from 'global-object'
import update from 'immutability-helper'
import { combineReducers } from 'redux'
import * as ActionTypes from 'Js/Modules/Redux/iNav/Actions/actionTypes'
import { iNavChildReducer } from 'Js/Modules/Redux/iNav/Reducers/iNavChildReducer'

// This is the entry Reducer and should be loaded witht component

// Check if there is a preloaded state
let initState = global.__PRELOADED_STATE__ ? global.__PRELOADED_STATE__.iNav || {} : {}

//We wrap the reducer to pass init data to it for it to work in ReactJS.NET
export const iNavParentReducerPassInitData = initState => {

    return (state = initState, action) => {
        switch (action.type) {
            case ActionTypes.TOGGLE_IS_SELECTED:
                return updateIsSelected(state.iNav, action)
            case ActionTypes.FETCH_QUERY_SUCCESS:
                return {
                    ...state,
                    ... {
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



function updateIsSelected(state, action) {

    return update(iNav, {
        iNav: {
            [action.node]: {
                [action.facet]: { $apply: function(isSelected) { return !isSelected; } }
            }
        }
    })

}