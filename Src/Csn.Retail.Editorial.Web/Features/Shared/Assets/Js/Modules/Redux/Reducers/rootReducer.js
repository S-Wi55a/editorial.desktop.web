//import * as ActionTypes from '../actions'
import smartGuidedNavigation from './smartGuidedNavigationReducer'
import { combineReducers } from 'redux'

// Updates error message to notify about the failed fetches.
//const errorMessage = (state = null, action) => {
//    const { type, error } = action

//    if (type === ActionTypes.RESET_ERROR_MESSAGE) {
//        return null
//    } else if (error) {
//        return error
//    }

//    return state
//}


// For now but should make this accept asyncReducers
const rootReducer = combineReducers({
    smartGuidedNavigation, //This takes the slice which is from the key of smartGuidedNavigation
    //errorMessage,
})

export default rootReducer