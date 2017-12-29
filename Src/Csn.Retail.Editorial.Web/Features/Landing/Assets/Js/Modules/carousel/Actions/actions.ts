import { Action } from 'redux'
import * as ActionTypes from 'carousel/Actions/ActionTypes'
import * as Thunks from 'carousel/Actions/thunks'


interface IFetchQueryRequest extends Action {
    type: ActionTypes.API.CAROUSEL.FETCH_QUERY_REQUEST
    payload: {}
}

interface IFetchQueryError extends Action {
    type: ActionTypes.API.CAROUSEL.FETCH_QUERY_FAILURE
    payload: {
        error: string
    }
}

interface IFetchQuerySuccess extends Action {
    type: ActionTypes.API.CAROUSEL.FETCH_QUERY_SUCCESS
    payload: {
        data: string,
        index: number
    }
}

type APIActions = IFetchQueryRequest | IFetchQuerySuccess | IFetchQueryError

export type Actions = APIActions

export { ActionTypes, Thunks }