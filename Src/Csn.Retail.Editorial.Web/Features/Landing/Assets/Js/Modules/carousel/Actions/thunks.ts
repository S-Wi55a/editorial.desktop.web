import { Dispatch } from 'redux'
import { ActionTypes } from 'carousel/Actions/actions'

type fetchCarouselResults = (q: string, i: number) => (d: Dispatch<any>, getState: any) => any

//TDO: add endpoitn argumens
export const fetchCarouselResults: fetchCarouselResults = (query: string, index: number) => (dispatch: Dispatch<any>) => {

    dispatch({ type: ActionTypes.API.CAROUSEL.FETCH_QUERY_REQUEST });

    return fetch(query)
        .then(
            response => response.json(),
            error => dispatch({ type: ActionTypes.API.CAROUSEL.FETCH_QUERY_FAILURE, payload: { error } })
        )
        .then(data =>
            dispatch(
                { type: ActionTypes.API.CAROUSEL.FETCH_QUERY_SUCCESS, payload: { data, index } }
            )
        );
}


export type Types = fetchCarouselResults