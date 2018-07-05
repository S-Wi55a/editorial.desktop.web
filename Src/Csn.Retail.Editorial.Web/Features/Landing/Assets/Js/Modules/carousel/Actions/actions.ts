import { Action } from 'redux'
import * as ActionTypes from 'carousel/Actions/ActionTypes'
import * as Thunks from 'carousel/Actions/thunks'


interface IFetchQueryRequest extends Action {
    type: ActionTypes.API.CAROUSEL.FETCH_QUERY_REQUEST
    payload: {
        index: number
    }
}

interface IFetchQueryError extends Action {
    type: ActionTypes.API.CAROUSEL.FETCH_QUERY_FAILURE
    payload: {
        error: string
        index: number
    }
}

interface IFetchQuerySuccess extends Action {
    type: ActionTypes.API.CAROUSEL.FETCH_QUERY_SUCCESS
    payload: {
        data: string,
        index: number
    }
}

interface IAddPromotedArticle extends Action {
    type: ActionTypes.CAROUSELS.ADD_PROMOTED_ARTICLE
    payload: {
        imageUrl: string,
        headline: string,
        dateAvailable: string,
        articleDetailsUrl: string,
        label: string,
        location: number,
        isNativeAd: boolean
    }
}

type APIActions = IFetchQueryRequest | IFetchQuerySuccess | IFetchQueryError

type ICarouselActions = IAddPromotedArticle

export type Actions = APIActions | ICarouselActions

export { ActionTypes, Thunks }