import { Dispatch } from 'redux'
import { ActionTypes } from 'carousel/Actions/actions'

type fetchCarouselResults = (q: string, i: number) => (d: Dispatch<any>, getState: any) => any

export const fetchCarouselResults: fetchCarouselResults = (query: string, index: number) => (dispatch: Dispatch<any>) => {

    dispatch({ type: ActionTypes.API.CAROUSEL.FETCH_QUERY_REQUEST, payload: { index } });

    return fetch(query)
        .then(
            response => response.json(),
            error => dispatch({ type: ActionTypes.API.CAROUSEL.FETCH_QUERY_FAILURE, payload: { error, index } })
        )
        .then(
            data => dispatch({ type: ActionTypes.API.CAROUSEL.FETCH_QUERY_SUCCESS, payload: { data, index } }),
            error => dispatch({ type: ActionTypes.API.CAROUSEL.FETCH_QUERY_FAILURE, payload: { error, index } })

        );
}


export type Types = fetchCarouselResults