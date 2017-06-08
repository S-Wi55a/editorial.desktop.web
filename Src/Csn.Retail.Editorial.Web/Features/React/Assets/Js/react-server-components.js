import {ReactServerConnect} from './ReactServerConnect'
import {testComponent} from './Test'
import SearchBarTest from './searchBar'

//import 'React/Assets/Js/storeClient'
//import * as s  from './storeClient'
//import CategoryItem from 'Js/Modules/Redux/iNav/Components/categoryItem'
//import SearchBarTest from './searchBar'


export const Components = {
    testComponent: ReactServerConnect(testComponent, 'TestComponent'),
    SearchBarTest: ReactServerConnect(SearchBarTest, 'SearchBar')

    //CategoryItem,
}


