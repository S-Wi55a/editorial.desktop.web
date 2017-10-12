import { Action } from 'redux'
import * as ActionTypes from 'iNav/Actions/ActionTypes'
import * as INavTypes from 'iNav/Types'
import * as Thunks from 'iNav/Actions/thunks'

interface IToggleIsActive extends Action {
    type: ActionTypes.UI.TOGGLE_IS_ACTIVE
    payload: {
        id: Number
        isActive: Boolean
    }
}

interface ICancel extends Action {
    type: ActionTypes.UI.CANCEL
    payload?: any    
}

interface ICloseINav extends Action {
    type: ActionTypes.UI.CLOSE_INAV
    payload?: any
}

interface IIncrement extends Action {
    type: ActionTypes.UI.INCREMENT
    payload: {
        name: string
    }
}

interface IDecrement extends Action {
    type: ActionTypes.UI.DECREMENT
    payload: {
        name: string
    }
}

interface IFetchQueryRequest extends Action {
    type: ActionTypes.API.INAV.FETCH_QUERY_REQUEST | ActionTypes.API.ASPECT.FETCH_QUERY_REQUEST | ActionTypes.API.REFINEMENT.FETCH_QUERY_REQUEST
    payload: {}
}

interface IFetchQueryError extends Action {
    type: ActionTypes.API.INAV.FETCH_QUERY_FAILURE | ActionTypes.API.ASPECT.FETCH_QUERY_FAILURE | ActionTypes.API.REFINEMENT.FETCH_QUERY_FAILURE
    payload: {
        error: string
    }
}

interface IFetchQuerySuccess extends Action {
    type: ActionTypes.API.INAV.FETCH_QUERY_SUCCESS | ActionTypes.API.ASPECT.FETCH_QUERY_SUCCESS | ActionTypes.API.REFINEMENT.FETCH_QUERY_SUCCESS
    payload: {
        name?: string
        data: INavTypes.IINavResponse
    }
}

interface IUpdatePendingQuery extends Action {
    type: ActionTypes.INAV.UPDATE_PENDING_QUERY
    payload: {
        query: string
    }
}

interface IUpdatePreviousState extends Action {
    type: ActionTypes.INAV.UPDATE_PREVIOUS_STATE
    payload: {
        data: INavTypes.IINavResponse
    }
}

interface IAddPromotedArticle extends Action {
    type: ActionTypes.INAV.ADD_PROMOTED_ARTICLE
    payload: {
        imageUrl: string,
        headline: string,
        dateAvailable: string,
        articleDetailsUrl: string,
        label: string,
        location: number
    }
}

interface ISwitchPage extends Action {
    type: ActionTypes.UI.SWITCH_PAGE_FORWARD | ActionTypes.UI.SWITCH_PAGE_BACK
    payload? : any
}

interface IFetchNativeAds extends Action {
    type: ActionTypes.INAV.EMIT_NATIVE_ADS_EVENT 
    payload: {
        event: string
    }
}


type UIActions = IToggleIsActive
    | ICancel
    | IIncrement
    | IDecrement
    | ICloseINav
    | ISwitchPage

type APIActions = IFetchQueryRequest | IFetchQuerySuccess | IFetchQueryError

type INavActions = IUpdatePendingQuery 
    | IUpdatePreviousState
    | IAddPromotedArticle
    | IFetchNativeAds

// Fetch data
export type Actions = UIActions | APIActions | INavActions

export { ActionTypes, Thunks }