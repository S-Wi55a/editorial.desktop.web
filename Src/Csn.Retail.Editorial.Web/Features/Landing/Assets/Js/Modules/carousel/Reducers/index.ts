import { Actions, ActionTypes } from 'carousel/Actions/actions'
import update from 'immutability-helper'

// we wrap the reducer to pass init data to it for it to work in ReactJS.NET
export const carouselParentReducer: any = (initState: any = null) => {
    return (state = initState, action: any) => carouselReducer(state, action);
};

const carouselReducer: any = (state: any, action: Actions) => {
    switch (action.type) {
    case ActionTypes.API.CAROUSEL.FETCH_QUERY_SUCCESS:
        return appendCarouselData(state, action)
    default:
        return state;
    }
}


function appendCarouselData (state: any, action: Actions) {
    try {
        const newState = update(state,
            {
                [action.payload.index]: {
                    articleSetItems: {
                        $push: action.payload.data.carouselViewModel.articleSetItems
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