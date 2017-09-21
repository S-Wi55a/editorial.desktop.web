import { createStore, } from 'redux';
import { createReducer } from 'Redux/Global/Reducers'
import { enhancer } from 'Redux/Global/Middleware/middleware'

export function configureStore(preloadedState) {
    return {
        ...createStore(createReducer(), preloadedState, enhancer),
        asyncReducers: {}
    }
}