import {ReactServerConnect} from 'React/Assets/Js/Util/ReactServerConnect'
import {ReduxStore} from 'React/Assets/Js/Components/ReduxStore'

// Search Bar component
import SearchBar from 'Js/Modules/Redux/SearchBar/Containers/searchBarContainer'
import INav from 'Js/Modules/iNav/Containers/iNavContainer'
import INavBreadCrumbsContainer from 'Js/Modules/iNavBreadCrumbs/Containers/iNavBreadCrumbsContainer'
import {iNavParentReducerPassInitData} from 'Js/Modules/Redux/iNav/Reducers/iNavReducer'


//All Components are called and loaded into memorey
export const Components = {
    INavBreadCrumbs: ReactServerConnect(INavBreadCrumbsContainer)('iNav', 'iNavParentReducerPassInitData'),
    INav: ReactServerConnect(INav)('iNav', 'iNavParentReducerPassInitData'),
    SearchBarContainer: ReactServerConnect(SearchBar)('iNav', 'iNavParentReducerPassInitData'),
    PartOfStore: ReactServerConnect(ReduxStore)()
}


