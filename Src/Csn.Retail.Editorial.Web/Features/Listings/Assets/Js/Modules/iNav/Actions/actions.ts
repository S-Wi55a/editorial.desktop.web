import { Action } from 'redux'
import * as ActionTypes from 'iNav/Actions/actionTypes'
import * as INavTypes from 'Redux/iNav/Types'

interface IToggleIsActive extends Action {
    type: ActionTypes.UI.TOGGLE_IS_ACTIVE
    payload: {
        id: Number
        isActive: Boolean
    }
}

interface ICancel extends Action {
    type: ActionTypes.UI.CANCEL
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
// Fetch data
export type Actions = IToggleIsActive
    | ICancel
    | IIncrement
    | IDecrement

export { ActionTypes }