import { Actions, ActionTypes } from 'carousel/Actions/actions'
import { IState } from 'carousel/Types'
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

function promotedReducer(state: IState, action: any): IState {
    
    try {
        const newState = update(state,
            {
                [action.payload.carouselId]: {
                    carouselItems: {
                        $splice: [[action.payload.location, 0, action.payload]]
                    }
                }
            })
        return newState
    } catch (e) {
        console.log(e)        
        return state
    }       
}