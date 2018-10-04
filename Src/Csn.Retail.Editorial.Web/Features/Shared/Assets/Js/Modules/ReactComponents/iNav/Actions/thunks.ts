import { Dispatch } from 'redux';
import { Actions, ActionTypes } from './actions'
import { iNavEndpoints } from 'Endpoints/endpoints'

type fetchINav = (q?: string) => (d: Dispatch<any>) => Promise<any>

export const fetchINav: fetchINav = (query?: string) => (dispatch: Dispatch<any>) => {

    dispatch({ type: ActionTypes.API.INAV.FETCH_QUERY_REQUEST });

    return fetch(`${iNavEndpoints.nav}${query}`)
        .then(
            response => response.json(),
            error => dispatch({ type: ActionTypes.API.INAV.FETCH_QUERY_FAILURE, payload: { error } })
        )
        .then(data =>
            dispatch(
                { type: ActionTypes.API.INAV.FETCH_QUERY_SUCCESS, payload: { data } }
                //{ type: ActionTypes.INAV.UPDATE_PREVIOUS_STATE,  payload: { data } },      
            )
        );
}

type fetchINavAndResults = (q?: string) => (d: Dispatch<any>, getState: any) => any

export const fetchINavAndResults: fetchINavAndResults = (query?: string) =>  (dispatch: any, getState: any) => {
    
        dispatch({ type: ActionTypes.API.INAV.FETCH_QUERY_REQUEST })

        const state = getState()

        const keyword = state.form.keywordSearch.values.keyword
        
        // If a keyword query is passed
        if (typeof query !== 'undefined') {
            if (decodeURI(query).includes('<!>')) {
                if (keyword !== '' && keyword !== undefined && keyword !== null) {
                    query = decodeURI(query).replace('<!>', keyword)
                } else {
                    query = state.store.nav.navResults.iNav.breadCrumbs.length > 0 && state.store.nav.navResults.iNav.breadCrumbs[0].type === 'KeywordBreadCrumb'
                        ? state.store.nav.navResults.iNav.breadCrumbs[0].removeAction
                        : (typeof state.store.nav.navResults.iNav.pendingUrl !== 'undefined' ? state.store.nav.navResults.iNav.pendingUrl : state.store.nav.navResults.iNav.currentUrl)
                }
            }
        } else {
            if (typeof state.store.nav.navResults.iNav.pendingUrl !== 'undefined') {
                query = state.store.nav.navResults.iNav.pendingUrl
            } else {
                query = state.store.nav.navResults.iNav.currentUrl
            }
        }

        // TODO: REMOVE FOR PHASE 2
        return window.location.assign(query)

        // return fetch(`${iNav.api}?${q}`)
        //     .then(
        //     response => response.json(),
        //     error => dispatch({ type: ActionTypes.API.INAV.FETCH_QUERY_FAILURE, payload: { error } })

        //     )
        //     .then(data =>
        //         dispatch([
        //             { type: ActionTypes.API.INAV.FETCH_QUERY_SUCCESS, payload: { data } },
        //             { type: ActionTypes.INAV.UPDATE_PREVIOUS_STATE,  payload: { data } },
        //             { type: ActionTypes.INAV.EMIT_NATIVE_ADS_EVENT, payload: {event: 'csn_editorial.nav.fetchNativeAds'}}     //TODO: this can come from the action               
        //         ])
        //     )
    }


type fetchINavRefinement = (r: string, p: string, q: string, u?: string, action?: Actions) => (d: any) => Promise<any>;
    
export const fetchINavRefinement: fetchINavRefinement = (refinementAspect: string, parentExpression: string, query: string, url?: string, reduxAction?: Actions) => (dispatch: any ) => {
        parentExpression = encodeURIComponent(parentExpression);
        dispatch({ type: ActionTypes.API.REFINEMENT.FETCH_QUERY_REQUEST })

    return fetch(iNavEndpoints.refinement(refinementAspect, parentExpression, query ? `${query}` : ''))
           // Try to parse the response
            .then(response =>
                response.json().then(json => ({
                status: response.status,
                json
                })
            ))
            .then(
                // Both fetching and parsing succeeded!
                ({ status, json }) => {
                  if (status >= 400) {
                    // Status looks bad
                    dispatch({ type: ActionTypes.API.REFINEMENT.FETCH_QUERY_FAILURE, payload: { error: `server status ${status}` } })
                  } else {
                    // Status looks good
                    dispatch([
                        { type: ActionTypes.API.REFINEMENT.FETCH_QUERY_SUCCESS, payload: { data: json, parentExpression }},
                        reduxAction
                    ])
                  }
                },
                // Either fetching or parsing failed!
                err => {
                    dispatch({ type: ActionTypes.API.REFINEMENT.FETCH_QUERY_FAILURE, payload: { err } })
                }
              )
    }

export type Types = fetchINav & fetchINavAndResults & fetchINavRefinement;