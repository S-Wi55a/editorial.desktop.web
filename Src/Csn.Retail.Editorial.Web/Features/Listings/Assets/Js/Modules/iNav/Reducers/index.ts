import global from 'global-object'
import { Actions, ActionTypes } from 'iNav/Actions/actions'
import update from 'immutability-helper'
import { INode, IINavResponse } from 'iNav/Types'


// This is the entry Reducer and should be loaded with component

//We wrap the reducer to pass init data to it for it to work in ReactJS.NET
export const iNavParentReducer = (initState: any = null) => {

    return (state = initState, action: Actions) => {
        switch (action.type) {
            case ActionTypes.UI.TOGGLE_IS_ACTIVE:
                if(action.payload.isActive){
                    const previousState = { ...state }
                    return {
                        ...state,
                        previousState
                    }
                } else if(!action.payload.isActive && typeof state.previousState !== 'undefined'){
                    return {
                        ...state.previousState
                    }
                } else {
                    return state
                }
            case ActionTypes.UI.CANCEL:
                if(typeof state.previousState !== 'undefined') {
                    return {
                        ...state.previousState
                    }
                } else {return state}
            // case ActionTypes.TOGGLE_IS_SELECTED:
            //     return isSelectedToggle(state, action)
            // case ActionTypes.REMOVE_BREAD_CRUMB:
            //     return RemoveBreadCrumbs(state, action)
            case ActionTypes.API.INAV.FETCH_QUERY_SUCCESS:
                //we are not doing a deep merge becasue we are replacing the complete state object
                //besides isSelected all UI is handled ina deifferent state branch and this replaced obj
                //won't affect the new state
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


function RemoveBreadCrumbs(state, action) {

    try {

        const breadCrumbIndex = state.iNav.breadCrumbs.findIndex((breadCrumb) => breadCrumb.facet === action.facet)

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
