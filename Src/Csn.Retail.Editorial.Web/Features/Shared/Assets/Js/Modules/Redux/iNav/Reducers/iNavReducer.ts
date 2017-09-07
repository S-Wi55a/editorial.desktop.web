import global from 'global-object'
import { Actions, ActionTypes} from 'Redux/iNav/Actions/actions'
import update from 'immutability-helper'


/// <reference path="../Types/iNav.types" />


// This is the entry Reducer and should be loaded with component

//We wrap the reducer to pass init data to it for it to work in ReactJS.NET
export const iNavParentReducerPassInitData = (initState: any) => {

    return (state = initState, action: Actions) => {
        switch (action.type) {
            case ActionTypes.TOGGLE_IS_SELECTED:
                return isSelectedToggle(state, action)
            case ActionTypes.REMOVE_BREAD_CRUMB:
                return RemoveBreadCrumbs(state, action)
            case ActionTypes.FETCH_QUERY_SUCCESS:
                //we are not doing a deep merge becasue we are replacing the complete state object
                //besides isSelected all UI is handled ina deifferent state branch and this replaced obj
                //won't affect the new state
                //TODO: use immutable.js instead
                return {
                    ...state,
                    ...action.data
                }
            default:
                return state
        }

    }
}

// Check if there is a preloaded state fro iNav
const initState: any = global.__PRELOADED_STATE__iNav

export const iNavParentReducer = iNavParentReducerPassInitData(initState)

// We use update from 'immutability-helper' because the item we are selecting is deply nested
// This ensures that we return a new obj and nto mutate the state
function isSelectedToggle(state: State, action: any): any {

    try {

        const nodeIndex = state.iNav.nodes.findIndex((node)=>node.name === action.node)
        const facetIndex = state.iNav.nodes[nodeIndex].facets.findIndex((facet) => facet.value === action.facet)
        const currentIsSelectedState = state.iNav.nodes[nodeIndex].facets[facetIndex].isSelected
        const newState = update(state,
            {

                iNav: {
                    nodes: {
                        [nodeIndex]: {
                            facets: {
                                [facetIndex]: {
                                    isSelected: { $set: !currentIsSelectedState }
                                } 
                                         
                            }
                        }
                    }
                }

            })
        return newState
        
    } catch (e) {
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
                        $splice: [[breadCrumbIndex,1]]                      
                    }
                }

            })
        return newState
        
    } catch (e) {
        return state
    } 


        
     
}