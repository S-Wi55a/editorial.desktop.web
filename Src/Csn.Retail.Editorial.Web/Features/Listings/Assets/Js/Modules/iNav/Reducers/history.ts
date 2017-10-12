import { Actions, ActionTypes } from 'iNav/Actions/actions'
import { INode, IINavResponse } from 'iNav/Types'
import update from 'immutability-helper'

export const iNavHistoryReducer = (state: any, action: Actions) => {
            switch (action.type) {
                case ActionTypes.INAV.UPDATE_PREVIOUS_STATE:
                    return {
                        ...state,
                        iNav: { ...action.payload.data }
                    }
                case ActionTypes.INAV.ADD_PROMOTED_ARTICLE:
                    return promotedReducer(state, action)
                default:
                    return state
            }
        }
    
function promotedReducer(state: IINavResponse, action: Actions) {
    
    try {
        const newState = update(state,
            {
                iNav: {
                    searchResults: {
                        [action.payload.location]: {
                            $set : action.payload
                        }
                    } 
                }           
            })
        return newState
    } catch (e) {
        console.log(e)        
        return state
    }       
}