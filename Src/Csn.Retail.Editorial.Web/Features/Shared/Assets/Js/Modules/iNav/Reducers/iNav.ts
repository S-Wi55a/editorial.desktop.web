import { Actions, ActionTypes } from 'iNav/Actions/actions'
import update from 'immutability-helper'
import { INode, INavResults } from 'iNav/Types'

// This is the entry Reducer and should be loaded with component

//We wrap the reducer to pass init data to it for it to work in ReactJS.NET
export const iNavReducer = (state: any, action: Actions) => {
    switch (action.type) {
        case ActionTypes.INAV.UPDATE_PENDING_ACTION:
            return updatePendingAction(state, action)
        case ActionTypes.API.INAV.FETCH_QUERY_SUCCESS:
            return navReducer(state, action);
        case ActionTypes.API.ASPECT.FETCH_QUERY_SUCCESS:
            return aspectReducer(state, action);
        case ActionTypes.API.REFINEMENT.FETCH_QUERY_SUCCESS:
            return refinementReducer(state, action);
        case ActionTypes.INAV.ADD_PROMOTED_ARTICLE:
            return promotedReducer(state, action);
        case ActionTypes.INAV.EMIT_NATIVE_ADS_EVENT:
            const e = new CustomEvent(action.payload.event);
            if(typeof window !== 'undefined'){dispatchEvent(e)}
            return state;
        default:
            return state;
    }
}

function navReducer(state: INavResults, action: Actions): INavResults {
    try {
        return update(state,
            {
                navResults: {
                    iNav: {
                        nodes: {
                            $set: action.payload.data.iNav.nodes
                        },
                        pendingQueryCount: {
                            $set: action.payload.data.count
                        },
                        keywordsPlaceholder: {
                            $set: action.payload.data.iNav.keywordsPlaceholder
                        }
                    }
                },
            });
    } catch (e) {
        console.log(e);
        return state;
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

function refinementReducer(state: INavResults, action: Actions): INavResults {
    
    try {
        const newState = update(state,
            {
                navResults: {
                    iNav: {
                        nodes: {
                            $set: action.payload.data.nav.nodes
                        },
                        pendingQueryCount: {
                            $set: action.payload.data.count
                        },
                        keywordsPlaceholder: {
                            $set: action.payload.data.nav.keywordsPlaceholder
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

function promotedReducer(state: INavResults, action: Actions): INavResults {
    
    try {
       if(state.navResults.count > action.payload.location) {
            const newState: any = update(state,
                {
                    navResults: {
                        searchResults: {
                            [action.payload.location]: {
                                $set : action.payload
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

function updatePendingAction (state: INavResults, action: Actions): INavResults {
    
    try {
        const newState = update(state,
            {
                navResults: {
                    iNav: {
                        pendingAction: {
                            $set: action.payload.action
                        },
                        pendingUrl: { 
                            $set:action.payload.url
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