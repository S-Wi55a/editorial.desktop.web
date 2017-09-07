import { Action } from 'redux'
import { ActionTypes } from 'Redux/iNav/Actions/actionTypes'
    
interface IToggleIsSelected extends Action {
    type: ActionTypes.TOGGLE_IS_SELECTED;
    node: any;
    facet: any;
}

interface IRemoveBreadCrumb extends Action {
    type: ActionTypes.REMOVE_BREAD_CRUMB;
    facet: any;
}

// Fetch data // TODO: make another type for API requests
interface IFetchQueryRequest extends Action {
    type: ActionTypes.FETCH_QUERY_REQUEST;
    payload: IPayload
}

interface IReset extends Action {
    type: ActionTypes.RESET;
    payload: IPayload
}

interface IFETCH_QUERY_SUCCESS extends Action {
    type: ActionTypes.FETCH_QUERY_SUCCESS
}

interface IPayload {
    query: string
}

// Fetch data
export type Actions =
| IToggleIsSelected
| IRemoveBreadCrumb
| IFetchQueryRequest
| IReset
| IFETCH_QUERY_SUCCESS

// Toggle IsSelected
export const toggleIsSelected = (node: any, facet: any) => ({
    type: ActionTypes.TOGGLE_IS_SELECTED,
    node, 
    facet 
})

// Remove Bread Crumb
export const removeBreadCrumb = (facet: any) => ({
    type: ActionTypes.REMOVE_BREAD_CRUMB,
    facet 
})

// Fetch data
export const fetchQueryRequest = (query: string): Actions => ({
    type: ActionTypes.FETCH_QUERY_REQUEST,
    payload: {
        query
    }
})

export const reset = (query: string) => ({
    type: ActionTypes.RESET,
    payload: {
        query
    }
})

export { ActionTypes }