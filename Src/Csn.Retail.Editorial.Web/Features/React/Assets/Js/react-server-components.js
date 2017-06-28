import {ReactServerConnect} from 'React/Assets/Js/Util/ReactServerConnect'
import {ReduxStore, AddDataToStore} from 'React/Assets/Js/Components/ReduxStore'

// Search Bar component
import SearchBar from 'Js/Modules/Redux/SearchBar/Containers/searchBarContainer'
import INav from 'Js/Modules/iNav/Containers/iNavContainer'
import INavBreadCrumbsContainer from 'Js/Modules/iNavBreadCrumbs/Containers/iNavBreadCrumbsContainer'
import {iNavParentReducerPassInitData} from 'Js/Modules/Redux/iNav/Reducers/iNavReducer'

export const Components = {
    AddDataToStore: ReactServerConnect(AddDataToStore)(),
    INavBreadCrumbs: ReactServerConnect(INavBreadCrumbsContainer)('iNav', 'iNavParentReducerPassInitData'),
    INav: ReactServerConnect(INav)('iNav', 'iNavParentReducerPassInitData'),
    SearchBarContainer: ReactServerConnect(SearchBar)('iNav', 'iNavParentReducerPassInitData'),
    Store: ReactServerConnect(ReduxStore)()
}


