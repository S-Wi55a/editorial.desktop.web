import { Actions, ActionTypes } from 'iNav/Actions/actions'
import update from 'immutability-helper'
import { State } from 'iNav/Types'
import { iNavReducer } from 'iNav/Reducers/iNav'
import { iNavHistoryReducer } from 'iNav/Reducers/history'

//We wrap the reducer to pass init data to it for it to work in ReactJS.NET
export const iNavParentReducer = (initState: any = null) => {

    return (
        state = {
            iNav: initState,
            history: { iNav: initState }
        },
        action: Actions
    ) => {
        switch (action.type) {
            case ActionTypes.UI.CANCEL:
                return iNavMenuReducer(state, action)
            default:
                return {
                    iNav: iNavReducer(state.iNav, action),
                    history: iNavHistoryReducer(state.history, action)
                }

        }
    }
}

function iNavMenuReducer(state: any, action: Actions) {
    
    try {
        const newState = update(state,
            {
                iNav: {
                    iNav: {
                        $set : state.history.iNav.iNav
                    }
                }
            })
        return newState
    } catch (e) {
        console.log(e)        
        return state
    }       
}