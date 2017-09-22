import { createStore } from 'redux-async-reducer';
import { enhancer } from 'Redux/Global/Middleware/middleware'

export function configureStore(preloadedState) {
    return createStore(null, preloadedState, enhancer)
}