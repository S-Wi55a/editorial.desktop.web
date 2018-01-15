import { Actions, ActionTypes } from '../Actions/actions'
import { IStore } from '../Types'
import update from 'immutability-helper'

export const iNavHistoryReducer = (state: any, action: Actions) => {
            switch (action.type) {
                case ActionTypes.INAV.UPDATE_PREVIOUS_STATE:
                    return {
                        ...state,
                        nav: { ...action.payload.data }                        
                    }
                case ActionTypes.INAV.ADD_PROMOTED_ARTICLE:
                    return promotedReducer(state, action)
                default:
                    return state
            }
        }
    
function promotedReducer(state: IStore, action: Actions) {
    
    try {
        if(state.nav.navResults.count > action.payload.location) {      
            const newState = update(state,
                {
                    nav: {
                        navResults: {
                            searchResults: {
                                [action.payload.location]: {
                                    $set : action.payload
                                }
                            }
                        }
                    }
                })
            return newState
        }
        return state
    } catch (e) {
        console.log(e)        
        return state
    }       
}
