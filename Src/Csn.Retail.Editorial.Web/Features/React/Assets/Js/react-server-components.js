import {ReactServerConnect} from 'React/Assets/Js/Util/ReactServerConnect'
import {ReduxStore} from 'React/Assets/Js/Components/ReduxStore'

// Search Bar component
import SearchBar from 'Js/Modules/Redux/SearchBar/Containers/searchBarContainer'
import iNav from 'Js/Modules/iNav/Containers/iNavContainer'
import {iNavParentReducerPassInitData} from 'Js/Modules/Redux/iNav/Reducers/iNavReducer'

export const Components = {
    iNav: ReactServerConnect(iNav)('iNav', iNavParentReducerPassInitData),
    SearchBarContainer: ReactServerConnect(SearchBar)('iNav', iNavParentReducerPassInitData),
    ReduxStore: ReactServerConnect(ReduxStore)()
}


