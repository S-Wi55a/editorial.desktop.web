import { ActionTypes} from 'Redux/iNav/Actions/actions'

export const iNavUiReducer = (initState = null) => {
    
        return (state = initState, action) => {
            switch (action.type) {
                case ActionTypes.TOGGLE_IS_SELECTED:
                    return isSelectedToggle(state, action)
                default:
                    return state
            }
    
        }
    }