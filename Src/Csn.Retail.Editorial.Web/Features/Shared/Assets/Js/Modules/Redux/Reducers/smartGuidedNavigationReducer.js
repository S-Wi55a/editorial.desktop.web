import { data } from '../Data/data' //Test data //TODO: remove

const smartGuidedNavigation = (state = data, action) => {
    switch (action.type) {
        case 'TEST_STATE':
            return state
        case 'CLEAR_STATE':
            return { now: 'clear' }
        case 'HOT_STATE':
            return { now: 'hot' }
        default: 
            return state
    }

}


export default smartGuidedNavigation