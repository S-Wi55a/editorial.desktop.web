import { ReactServerConnect } from 'Util/ReactServerConnect'
import Reducers from '../Reducers/registeredServerReducerCollection'

// Preloaded State Component
import PreloadedState from './preloadedState'

// Search Bar component
import INav from 'iNav/Containers/iNavContainer'
import INavArticleCountComponent from 'iNavArticleCount/Components/iNavArticleCountComponent'
import INavBreadCrumbsContainer from 'iNavBreadCrumbs/Containers/iNavBreadCrumbsContainer'
import INavSearchResultsContainer from 'iNavSearchResults/Containers/iNavSearchResultsContainer'

//All Components are called and loaded into memorey
export const Components = {
    INavPreloadedState: ReactServerConnect(PreloadedState)('csn_search', Reducers['iNav'], true),
    INavArticleCount: ReactServerConnect(INavArticleCountComponent)('csn_search', Reducers["iNav"]),
    INavSearchResults: ReactServerConnect(INavSearchResultsContainer)('csn_search', Reducers['iNav']),
    INavBreadCrumbs: ReactServerConnect(INavBreadCrumbsContainer)('csn_search', Reducers['iNav']),
    INav: ReactServerConnect(INav)('csn_search', Reducers['iNav'])
}


