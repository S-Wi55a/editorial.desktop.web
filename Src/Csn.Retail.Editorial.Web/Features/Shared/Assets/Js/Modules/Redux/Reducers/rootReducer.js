import { iNavReducer as iNav } from './SmartGuidedNavigation/Reducers/iNavReducer'
import { combineReducers } from 'redux'

// For now but should make this accept asyncReducers
const rootReducer = combineReducers({
    iNav, //This takes the slice which is from the key of smartGuidedNavigation
    //errorMessage,
})

export default rootReducer