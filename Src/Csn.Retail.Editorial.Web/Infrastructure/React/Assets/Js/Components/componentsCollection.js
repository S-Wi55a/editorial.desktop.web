import { ReactServerConnect } from 'Util/reactServerConnect'

// Preloaded State Component
import PreloadedState from './preloadedState'

// Search Bar component
import INav from 'iNav/Containers/iNavContainer'
import INavBreadCrumbsContainer from 'iNavBreadCrumbs/Containers/iNavBreadCrumbsContainer'
import INavSearchResultsContainer from 'iNavSearchResults/Containers/iNavSearchResultsContainer'

//TODO: make reducer enum

//All Components are called and loaded into memorey
export const Components = {
    INavPreloadedState:  ReactServerConnect(PreloadedState)('iNav', 'iNavParentReducer', true),    
    INavSearchResults: ReactServerConnect(INavSearchResultsContainer)('iNav', 'iNavParentReducer'),
    INavBreadCrumbs: ReactServerConnect(INavBreadCrumbsContainer)('iNav', 'iNavParentReducer'),
    INav: ReactServerConnect(INav)('iNav', 'iNavParentReducer')
}


