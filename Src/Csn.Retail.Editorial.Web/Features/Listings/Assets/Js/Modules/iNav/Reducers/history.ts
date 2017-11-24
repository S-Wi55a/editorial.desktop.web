import { Actions, ActionTypes } from 'iNav/Actions/actions'
import { IStore } from 'iNav/Types'
import update from 'immutability-helper'

export const iNavHistoryReducer = (state: any, action: Actions) => {
            switch (action.type) {
                case ActionTypes.INAV.UPDATE_PREVIOUS_STATE:
                    return {
                        ...state,
                        listings: { ...action.payload.data }                        
                    }
                case ActionTypes.INAV.ADD_PROMOTED_ARTICLE:
                    return promotedReducer(state, action)
                default:
                    return state
            }
        }
    
function promotedReducer(state: IStore, action: Actions) {
    
    try {
        if(state.listings.navResults.count > action.payload.location) {      
            const newState = update(state,
                {
                    listings: {
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
