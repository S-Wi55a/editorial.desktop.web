import { ReactServerConnect } from 'Util/ReactServerConnect'
import Reducers from '../Reducers/registeredServerReducerCollection'

// Preloaded State Component
import PreloadedState from './preloadedState'

// Search Bar component
import INav from 'iNav/Containers/iNavContainer'
import INavArticleCountComponent from 'iNavArticleCount/Components/iNavArticleCountComponent'
import INavBreadCrumbsContainer from 'iNavBreadCrumbs/Containers/iNavBreadCrumbsContainer'
import INavSortingContainer from 'iNavSorting/Containers/iNavSortingContainer'
import INavSearchResultsContainer from 'iNavSearchResults/Containers/iNavSearchResultsContainer'
import INavPaginationContainer from 'iNavPagination/Containers/iNavPaginationContainer'

// Carousels
import Carousel from 'carousel/Component/carouselComponent'

//All Components are called and loaded into memorey
export const Components = {
    INavPreloadedState: ReactServerConnect(PreloadedState)('store', Reducers['listings'], true),
    INavArticleCount: ReactServerConnect(INavArticleCountComponent)('store', Reducers['listings']),
    INavPagination: ReactServerConnect(INavPaginationContainer)('store', Reducers['listings']),
    INavSorting: ReactServerConnect(INavSortingContainer)('store', Reducers['listings']),
    INavSearchResults: ReactServerConnect(INavSearchResultsContainer)('store', Reducers['listings']),
    INavBreadCrumbs: ReactServerConnect(INavBreadCrumbsContainer)('store', Reducers['listings']),
    INav: ReactServerConnect(INav)('store', Reducers['listings']),

    //Carousel
    CarouselPreloadedState: ReactServerConnect(PreloadedState)('carousels', Reducers['carousels'], true),
    Carousel: ReactServerConnect(Carousel)('carousels', Reducers['carousels']),


}

