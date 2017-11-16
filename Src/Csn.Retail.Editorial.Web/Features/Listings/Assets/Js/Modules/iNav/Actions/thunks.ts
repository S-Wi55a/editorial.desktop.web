import { Dispatch } from 'redux';
import { Actions, ActionTypes } from 'iNav/Actions/actions'
import { iNav } from 'Endpoints/endpoints'
import queryString from 'query-string'

type fetchINav = (q?: string, f?: boolean) => (d: Dispatch<any>, getState: any) => any

export const fetchINav: fetchINav = (query?: string, forceEmpty?: boolean) =>  (dispatch: any, getState: any) => {
    
        dispatch({ type: ActionTypes.API.INAV.FETCH_QUERY_REQUEST })

        let q = typeof query !== 'undefined' ? query : getState().store.listings.pendingQuery
        const keyword = typeof getState().form.keywordSearch !== 'undefined' && 
                          typeof getState().form.keywordSearch.values !== 'undefined' &&
                          typeof getState().form.keywordSearch.values.keyword !== 'undefined' ? 
                          getState().form.keywordSearch.values.keyword : undefined
        
        // Parse Query
        q = queryString.parse(q)
        q.keywords = keyword
        q = queryString.stringify(q)

        // TODO: REMOVE FOR PHASE 2
        return window.location.assign(forceEmpty ? window.location.pathname : `?${q}`)

        // return fetch(`${iNav.api}?${q}`)
        //     .then(
        //     response => response.json(),
        //     error => dispatch({ type: ActionTypes.API.INAV.FETCH_QUERY_FAILURE, payload: { error } })

        //     )
        //     .then(data =>
        //         dispatch([
        //             { type: ActionTypes.API.INAV.FETCH_QUERY_SUCCESS, payload: { data } },
        //             { type: ActionTypes.INAV.UPDATE_PREVIOUS_STATE,  payload: { data } },
        //             { type: ActionTypes.INAV.EMIT_NATIVE_ADS_EVENT, payload: {event: 'csn_editorial.listings.fetchNativeAds'}}     //TODO: this can come from the action               
        //         ])
        //     )
    }

type fetchINavAspect = (a: string, q: string) => (d: Dispatch<any>) => Promise<any>
    
export const fetchINavAspect: fetchINavAspect = (aspect: string, query: string) => (dispatch: Dispatch<any>) => {

        dispatch({ type: ActionTypes.API.ASPECT.FETCH_QUERY_REQUEST })

        return fetch(`${iNav.aspect}${aspect}${query}`)
            .then(
            response => response.json(),
            error => dispatch({ type: ActionTypes.API.ASPECT.FETCH_QUERY_FAILURE, payload: { error } })

            )
            .then(data =>
                dispatch({ type: ActionTypes.API.ASPECT.FETCH_QUERY_SUCCESS, payload: { data, name: aspect } })
            )
    }


type fetchINavRefinement = (a: string, r: string, p: string, q: string, action?: Actions) => (d: any) => Promise<any>
    
export const fetchINavRefinement: fetchINavRefinement = (aspect: string, refinementAspect: string, parentExpression: string, query: string, action?: Actions) => (dispatch: any ) => {

        dispatch({ type: ActionTypes.API.REFINEMENT.FETCH_QUERY_REQUEST })

        return fetch(iNav.refinement(aspect, refinementAspect, parentExpression, query))
            .then(
                response => response.json(),
                error => dispatch({ type: ActionTypes.API.REFINEMENT.FETCH_QUERY_FAILURE, payload: { error } })
            )
            .then(data => {
                dispatch([
                    { type: ActionTypes.API.REFINEMENT.FETCH_QUERY_SUCCESS, payload: { data, name: aspect, parentExpression }},
                    action
                ])
            })
    }

export type Types = fetchINav & fetchINavAspect & fetchINavRefinement