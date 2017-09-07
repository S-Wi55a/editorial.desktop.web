import {ReactServerConnect} from 'Util/ReactServerConnect'
import {ReduxStore} from 'Components/ReduxStore'

// Search Bar component
import INav from 'iNav/Containers/iNavContainer'
import INavBreadCrumbsContainer from 'iNavBreadCrumbs/Containers/iNavBreadCrumbsContainer'
import INavSearchResultsContainer from 'iNavSearchResults/Containers/iNavSearchResultsContainer'

//All Components are called and loaded into memorey
export const Components = {
    INavSearchResults: ReactServerConnect(INavSearchResultsContainer)('iNav', 'iNavParentReducerPassInitData'),
    INavBreadCrumbs: ReactServerConnect(INavBreadCrumbsContainer)('iNav', 'iNavParentReducerPassInitData'),
    INav: ReactServerConnect(INav)('iNav', 'iNavParentReducerPassInitData'),
    PartOfStore: ReactServerConnect(ReduxStore)()
}


