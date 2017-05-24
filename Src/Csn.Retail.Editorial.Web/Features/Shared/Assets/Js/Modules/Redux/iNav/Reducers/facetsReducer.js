import * as iNav_ActionTypes from 'Js/Modules/Redux/iNav/Actions/actionTypes.js'


const facetsReducer = (state, action) => {
    switch (action.type) {
    case iNav_ActionTypes.TOGGLE_SELECTED:
        //loop through array to find which reducer to call
        return state.map((facet) => {
                if (facet.value === action.facet) {
                    return {
                        ...facet,
                        ...{isSelected: !facet.isSelected}
                    }
                }
                return facet
            })
    default:
        return state
    }
}


export default facetsReducer