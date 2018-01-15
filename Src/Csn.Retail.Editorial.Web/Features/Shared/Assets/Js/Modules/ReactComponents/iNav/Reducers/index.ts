import { Actions, ActionTypes } from '../Actions/actions'
import update from 'immutability-helper'
import { iNavReducer } from './iNav'
import { iNavHistoryReducer } from './history'

//We wrap the reducer to pass init data to it for it to work in ReactJS.NET
export const iNavParentReducer = (initState: any = null) => {

    return (
        state = {
            nav: initState,
            history: { nav: initState }
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
                    nav: iNavReducer(state.nav, action),
                    history: iNavHistoryReducer(state.history, action)
                }

        }
    }
}

function iNavMenuReducer(state: any, action: Actions) {
    
    try {
        return update(state,
            {
                nav: {
                    navResults: {
                        iNav: {
                            $set: state.history.nav.navResults.iNav
                        }
                    }
                }
            });
    } catch (e) {
        console.log(e);
        return state;
    }       
}