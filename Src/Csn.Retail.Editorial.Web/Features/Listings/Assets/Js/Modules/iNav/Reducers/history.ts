import { Actions, ActionTypes } from 'iNav/Actions/actions'
import { INode, IINavResponse } from 'iNav/Types'
import update from 'immutability-helper'

export const iNavHistoryReducer = (state: any, action: Actions) => {
            switch (action.type) {
                case ActionTypes.INAV.UPDATE_PREVIOUS_STATE:
                    return {
                        ...state,
                        listings: { ...action.payload.data }
                    }
                default:
                    return state
            }
        }
    