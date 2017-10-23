import { Actions, ActionTypes } from 'iNav/Actions/actions'
import update from 'immutability-helper'
import { INode, IINavResponse, INavResults } from 'iNav/Types'

// This is the entry Reducer and should be loaded with component

//We wrap the reducer to pass init data to it for it to work in ReactJS.NET
export const iNavReducer = (state: any, action: Actions) => {
    switch (action.type) {
        case ActionTypes.INAV.UPDATE_PENDING_QUERY: {
            return {
                ...state,
                ...{ pendingQuery: action.payload.query }
            }
        }
        case ActionTypes.API.INAV.FETCH_QUERY_SUCCESS:
            return {
                ...state,
                ...action.payload.data
            }
        case ActionTypes.API.ASPECT.FETCH_QUERY_SUCCESS:
            return aspectReducer(state, action)
        case ActionTypes.API.REFINEMENT.FETCH_QUERY_SUCCESS:
            return refinementReducer(state, action)
        default:
            return state
    }
}


// We use update from 'immutability-helper' because the item we are selecting is deply nested
// This ensures that we return a new obj and nto mutate the state
function aspectReducer(state: INavResults, action: Actions): INavResults {

    try {
        const nodeIndex = state.navResults.iNav.nodes.findIndex((node: INode) => node.name === action.payload.name)
        const newState = update(state,
            {
                navResults: {
                    iNav: {
                        nodes: {
                            [nodeIndex]: {
                                $set : action.payload.data
                            }
                        },
                    }
                }
            })
        return newState
    } catch (e) {
        console.log(e)        
        return state
    }
}

function refinementReducer(state: INavResults, action: Actions): INavResults {
    
    try {
        const nodeIndex = state.navResults.iNav.nodes.findIndex((node: INode) => node.name === action.payload.name)
        const newState = update(state,
            {
                navResults: {
                    iNav: {
                        nodes: {
                            [nodeIndex]: {
                                $set : action.payload.data
                            }
                        },
                    }
                }
            })
        return newState

    } catch (e) {
        console.log(e)        
        return state
    }
}