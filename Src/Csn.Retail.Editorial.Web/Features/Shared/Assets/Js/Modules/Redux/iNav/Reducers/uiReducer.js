import * as ActionTypes from 'Js/Modules/Redux/iNav/Actions/actionTypes'

//This Reducer uses Immutable js - https://facebook.github.io/immutable-js/
export const uiReducer = (state, action) => {
    // state represents *only* the UI state for this component's scope - not any children
    switch(action.type) {
    case ActionTypes.TOGGLE_IS_ACTIVE:
        return state.set('isActive', !state.get('isActive')) 
    }
    return state;
}