import { Dispatch } from 'redux';
import { Actions, ActionTypes } from 'iNav/Actions/actions'
import { iNav } from 'Endpoints/endpoints'

type fetchINav = (q: string) => (d: Dispatch<any>) => Promise<any>

export const fetchINav: fetchINav = (query: string) =>  (dispatch: any) => {
    
        dispatch({ type: ActionTypes.API.INAV.FETCH_QUERY_REQUEST })

        return fetch(`${iNav.base}${query}`)
            .then(
            response => response.json(),
            error => dispatch({ type: ActionTypes.API.INAV.FETCH_QUERY_FAILURE, payload: { error } })

            )
            .then(data =>
                dispatch([
                    { type: ActionTypes.API.INAV.FETCH_QUERY_SUCCESS, payload: { data } },
                    { type: ActionTypes.INAV.UPDATE_PREVIOUS_STATE,  payload: { data } }
                ])
            )
    }

type fetchINavAspect = (a: string, q: string) => (d: Dispatch<any>) => Promise<any>
    
export const fetchINavAspect: fetchINavAspect = (aspect: string, query: string) => (dispatch: Dispatch<any>) => {

        dispatch({ type: ActionTypes.API.ASPECT.FETCH_QUERY_REQUEST })

        return fetch(`${iNav.aspect}${aspect}?q=${query}`)
            .then(
            response => response.json(),
            error => dispatch({ type: ActionTypes.API.ASPECT.FETCH_QUERY_FAILURE, payload: { error } })

            )
            .then(data =>
                dispatch({ type: ActionTypes.API.ASPECT.FETCH_QUERY_SUCCESS, payload: { data, name: aspect } })
            )
    }


type fetchINavRefinement = (a: string, r: string, p: string, q: string) => (d: any) => Promise<any>
    
export const fetchINavRefinement: fetchINavRefinement = (aspect: string, refinementAspect: string, parentExpression: string, query: string) => (dispatch: any ) => {

        dispatch({ type: ActionTypes.API.REFINEMENT.FETCH_QUERY_REQUEST })

        return fetch(iNav.refinement(aspect, refinementAspect, parentExpression, query))
            .then(
                response => response.json(),
                error => dispatch({ type: ActionTypes.API.REFINEMENT.FETCH_QUERY_FAILURE, payload: { error } })
            )
            .then(data => {
                dispatch({ type: ActionTypes.API.REFINEMENT.FETCH_QUERY_SUCCESS, payload: { data, name: aspect, parentExpression }})
            })
    }

export type Types = fetchINav & fetchINavAspect & fetchINavRefinement