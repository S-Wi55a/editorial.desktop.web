import * as ActionTypes from 'Js/Modules/Redux/iNav/Actions/actionTypes'


const constructQuery = (state) => {
    let iNavQuery = 'Service.CarSales.'
    //Loop through state and find everything that is slected and construct a query 


    //Logic for multi select would be here


    return iNavQuery   
}



export const iNavQueryReducer = (state = '', action, iNavState) => {
    switch (action.type) {
    case ActionTypes.UPDATE_QUERY_STRING:
        return constructQuery(iNavState)
        default: 
            return state
    }

}
