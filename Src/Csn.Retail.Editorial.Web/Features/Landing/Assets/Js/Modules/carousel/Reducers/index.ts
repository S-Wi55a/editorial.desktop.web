import { Actions, ActionTypes } from 'carousel/Actions/actions'
import { IState, ICarouselViewModel } from 'carousel/Types'
import update from 'immutability-helper'

// we wrap the reducer to pass init data to it for it to work in ReactJS.NET
export const carouselParentReducer: any = (initState: IState = null) => {
    return (state = initState, action: any) => carouselReducer(state, action);
};

const carouselReducer: any = (state: IState, action: Actions) => {
    switch (action.type) {
    case ActionTypes.API.CAROUSEL.FETCH_QUERY_SUCCESS:
        return appendCarouselData(state, action)
    case ActionTypes.CAROUSELS.ADD_PROMOTED_ARTICLE:
        return promotedReducer(state, action);
    default:
        return state;
    }
}

function appendCarouselData (state: IState, action: any) {
    try {
        const newState = update(state,
            {
                [action.payload.index]: {
                    carouselItems: {
                        $push: action.payload.data.carouselViewModel.carouselItems
                    },
                    nextQuery: {
                        $set: action.payload.data.carouselViewModel.nextQuery
                    }
                }
            })
        return newState

    } catch (e) {
        console.log(e)        
        return state
    }
}

function promotedReducer(state: IState, action: Actions): IState {
    
    try {
        //if(state.carousels .count > action.payload.location) {
        //    const newState: any = update(state,
        //        {
        //            navResults: {
        //                searchResults: {
        //                    [action.payload.location]: {
        //                        $set : action.payload
        //                    }
        //                }
        //            }
        //        })
        //    return newState
        //}
        return state
    } catch (e) {
        console.log(e)        
        return state
    }       
}