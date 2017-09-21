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

interface IFetchQuerySuccess extends Action {
    type: ActionTypes.API.INAV.FETCH_QUERY_SUCCESS | ActionTypes.API.ASPECT.FETCH_QUERY_SUCCESS
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


type UIActions = IToggleIsActive
    | ICancel
    | IIncrement
    | IDecrement
    | ICloseINav

type APIActions = IFetchQuerySuccess

type INavActions = IUpdatePendingQuery 
    | IUpdatePreviousState

// Fetch data
export type Actions = UIActions | APIActions | INavActions

export { ActionTypes, Thunks }