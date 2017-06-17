import {ReactServerConnect} from './ReactServerConnect'
import {ReduxStore} from 'React/Assets/Js/Components/ReduxStore'

// Search Bar component
import SearchBarContainer from 'Js/Modules/Redux/iNav/Containers/searchBarContainer'
import {iNavParentReducerPassInitData} from 'Js/Modules/Redux/iNav/Reducers/iNavParentReducer'

export const Components = {
    SearchBarContainer: ReactServerConnect(SearchBarContainer)('iNav', iNavParentReducerPassInitData),
    ReduxStore: ReactServerConnect(ReduxStore)()
}


