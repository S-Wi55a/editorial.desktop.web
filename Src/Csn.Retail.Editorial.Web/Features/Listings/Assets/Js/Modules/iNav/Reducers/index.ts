import { Actions, ActionTypes } from 'iNav/Actions/actions'
import update from 'immutability-helper'
import { iNavReducer } from 'iNav/Reducers/iNav'
import { iNavHistoryReducer } from 'iNav/Reducers/history'

//We wrap the reducer to pass init data to it for it to work in ReactJS.NET
export const iNavParentReducer = (initState: any = null) => {

    return (
        state = {
            listings: initState,
            history: { listings: initState }
        },
        action: Actions
    ) => {
        switch (action.type) {
            case ActionTypes.UI.CANCEL:
                {
                return iNavMenuReducer(state, action)
            }
            default:
                return {
                    listings: iNavReducer(state.listings, action),
                    history: iNavHistoryReducer(state.history, action)
                }

        }
    }
}

function iNavMenuReducer(state: any, action: Actions) {
    
    try {
        return update(state,
            {
                listings: {
                    navResults: {
                        iNav: {
                            $set: state.history.listings.navResults.iNav
                        }
                    }
                }
            });
    } catch (e) {
        console.log(e);
        return state;
    }       
}