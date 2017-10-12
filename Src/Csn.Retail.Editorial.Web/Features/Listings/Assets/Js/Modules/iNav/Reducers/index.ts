import { Actions, ActionTypes } from 'iNav/Actions/actions'
import update from 'immutability-helper'
import { INode, IINavResponse } from 'iNav/Types'
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
                return {
                    ...state,
                    iNav:{ ...state.history.iNav },
                }
            default:
                return {   
                        iNav: iNavReducer(state.iNav, action),
                        history: iNavHistoryReducer(state.history, action)
                    }
        
    }
}


}