import {ReactServerConnect} from './ReactServerConnect'
import {ReduxStore} from 'React/Assets/Js/Components/ReduxStore'

// Search Bar component
import SearchBar from 'Js/Modules/Redux/SearchBar/Components/searchBar'
import {iNavParentReducerPassInitData} from 'Js/Modules/Redux/iNav/Reducers/iNavParentReducer'

export const Components = {
    SearchBarContainer: ReactServerConnect(SearchBar)('iNav', iNavParentReducerPassInitData),
    ReduxStore: ReactServerConnect(ReduxStore)()
}


