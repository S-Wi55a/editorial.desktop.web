// We use require here becasue the script needs to be evaluated (which happens in CommonJS modules)
// So the exports are available globally on ReactServerComponents(from expose laoder) for ReactJS.net

import {Components} from './react-server-components'

//import 'React/Assets/Js/storeClient'
//import * as s  from './storeClient'
//import CategoryItem from 'Js/Modules/Redux/iNav/Components/categoryItem'
//import SearchBarTest from './searchBar'


//export const Components = {
//    testComponent: ReactServerConnect(testComponent, 'TestComponent'),
//    SearchBarTest: ReactServerConnect(SearchBarTest, 'SearchBar')

//    //CategoryItem,
//}



global.window = {} //NOTE: there is no window on the serverside so this shim provides and empty object //TODO: find a better way
global.count = 0
console.log('The Start', global.count)

const store = require('Js/Modules/Redux/Global/Store/store.js')

//Enable Redux store globally
global.store = store.configureStore()
global.injectAsyncReducer = store.injectAsyncReducer

global.Components = Components

console.log(global)
