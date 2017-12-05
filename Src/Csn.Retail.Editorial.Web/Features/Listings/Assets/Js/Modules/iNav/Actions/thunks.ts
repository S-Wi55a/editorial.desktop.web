import { Dispatch } from 'redux';
import { Actions, ActionTypes } from 'iNav/Actions/actions'
import { iNav } from 'Endpoints/endpoints'

type fetchINav = (q?: string) => (d: Dispatch<any>) => Promise<any>

export const fetchINav: fetchINav = (query?: string) => (dispatch: Dispatch<any>) => {

    dispatch({ type: ActionTypes.API.INAV.FETCH_QUERY_REQUEST });

    return fetch(`${iNav.nav}${query}`)
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

        const keyword = 
            typeof getState().form.keywordSearch !== 'undefined' &&
            typeof getState().form.keywordSearch.values !== 'undefined' &&
            typeof getState().form.keywordSearch.values.keyword !== 'undefined' ?
                   getState().form.keywordSearch.values.keyword : undefined

        const q = 
            typeof query !== 'undefined' ? 
            decodeURI(query).replace('<!>', keyword) : //Will stil use query even if no match in replace
            getState().store.listings.navResults.iNav.pendingUrl ? getState().store.listings.navResults.iNav.pendingUrl : getState().store.listings.navResults.iNav.currentUrl
        
        // TODO: REMOVE FOR PHASE 2
        return window.location.assign(q)

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


type fetchINavRefinement = (r: string, p: string, q: string, u?: string, action?: Actions) => (d: any) => Promise<any>;
    
export const fetchINavRefinement: fetchINavRefinement = (refinementAspect: string, parentExpression: string, query: string, url?: string, reduxAction?: Actions) => (dispatch: any ) => {
        parentExpression = encodeURIComponent(parentExpression);
        dispatch({ type: ActionTypes.API.REFINEMENT.FETCH_QUERY_REQUEST })

        return fetch(iNav.refinement(refinementAspect, parentExpression, query ? `${query}` : ''))
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