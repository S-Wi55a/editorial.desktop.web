//TODO: need to make this async


import { iNavReducer as iNav } from 'Js/Modules/Redux/iNav/Reducers/iNavParentReducer'
import { combineReducers } from 'redux'

// For now but should make this accept asyncReducers
const rootReducer = combineReducers({
    iNav, //This takes the slice which is from the key of smartGuidedNavigation
    //errorMessage,
})

export default rootReducer