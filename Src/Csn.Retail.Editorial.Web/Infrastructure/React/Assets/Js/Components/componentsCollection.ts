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
    INavPreloadedState: ReactServerConnect(PreloadedState)('iNav', Reducers['iNav'], true),
    INavArticleCount: ReactServerConnect(INavArticleCountComponent)('iNav', Reducers["iNav"]),
    INavSearchResults: ReactServerConnect(INavSearchResultsContainer)('iNav', Reducers['iNav']),
    INavBreadCrumbs: ReactServerConnect(INavBreadCrumbsContainer)('iNav', Reducers['iNav']),
    INav: ReactServerConnect(INav)('iNav', Reducers['iNav'])
}


