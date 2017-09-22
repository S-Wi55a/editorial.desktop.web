import global from 'global-object'
import { Actions, ActionTypes } from 'iNav/Actions/actions'
import update from 'immutability-helper'
import { INode, IINavResponse } from 'iNav/Types'


// This is the entry Reducer and should be loaded with component

//We wrap the reducer to pass init data to it for it to work in ReactJS.NET
export const iNavParentReducer = (initState: any = null) => {

    const s = {
        ...initState,
        ...{ previousState: initState }
    }

    return (state = s, action: Actions) => {
        switch (action.type) {
            case ActionTypes.UI.CANCEL:
                return {
                    ...state,
                    ...state.previousState
                }
            case ActionTypes.INAV.UPDATE_PENDING_QUERY: {
                return {
                    ...state,
                    ...{ pendingQuery: action.payload.query }
                }
            }
            case ActionTypes.INAV.UPDATE_PREVIOUS_STATE:
            return {
                ...state,
                ...{ previousState: { ...action.payload.data } }
            }
            case ActionTypes.API.INAV.FETCH_QUERY_SUCCESS:
                return {
                    ...state,
                    ...action.payload.data
                }
            case ActionTypes.API.ASPECT.FETCH_QUERY_SUCCESS:
                return aspectReducer(state, action)
            default:
                return state
        }
    }
}

// We use update from 'immutability-helper' because the item we are selecting is deply nested
// This ensures that we return a new obj and nto mutate the state
function aspectReducer(state: IINavResponse, action: Actions): IINavResponse {

    try {
        const nodeIndex = state.iNav.nodes.findIndex((node: INode) => node.name === action.payload.name)
        const newState = update(state,
            {
                iNav: {
                    nodes: {
                        [nodeIndex]: {
                            $set : action.payload.data
                        }
                    },
                }
            })
        return newState

    } catch (e) {
        console.log(e)        
        return state
    }




}


function RemoveBreadCrumbs(state: any, action: any) {

    try {

        const breadCrumbIndex = state.iNav.breadCrumbs.findIndex((breadCrumb: any) => breadCrumb.facet === action.facet)

        const newState = update(state,
            {

                iNav: {
                    breadCrumbs: {
                        $splice: [[breadCrumbIndex, 1]]
                    }
                }

            })
        return newState

    } catch (e) {
        return state
    }




}
