import { Dispatch } from 'redux';
import { Actions, ActionTypes } from 'iNav/Actions/actions'
import { iNav } from 'Endpoints/endpoints'

export function fetchINav(query: string) {
    
       return (dispatch: Dispatch<Actions>) => {
            dispatch({ type: ActionTypes.API.FETCH_QUERY_REQUEST })
    
           return fetch(`${iNav}${query}`)
                .then(
                    response => response.json(),
                    error => dispatch({ type: ActionTypes.API.FETCH_QUERY_FAILURE, payload:{error} })
                    
                )
                .then(data =>
                    dispatch({ type: ActionTypes.API.FETCH_QUERY_SUCCESS, payload: {data} })
                )
        }
    }