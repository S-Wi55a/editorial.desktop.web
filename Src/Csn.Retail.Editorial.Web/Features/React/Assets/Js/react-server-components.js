import {ReactServerConnect} from './ReactServerConnect'
import {ReduxStore} from 'React/Assets/Js/ReduxStore'

// Search Bar component
import SearchBarContainer from 'Js/Modules/Redux/iNav/Containers/searchBarContainer'
import {iNavParentReducer} from 'Js/Modules/Redux/iNav/Reducers/iNavParentReducer'

export const Components = {
    SearchBarContainer: ReactServerConnect(SearchBarContainer)('iNav', iNavParentReducer),
    ReduxStore: ReactServerConnect(ReduxStore)()
}


