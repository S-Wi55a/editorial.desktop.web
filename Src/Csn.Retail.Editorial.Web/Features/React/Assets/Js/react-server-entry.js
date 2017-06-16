import 'babel-polyfill'
import global from 'global-object' 
import {Components} from './react-server-components'

// Make compoents globally available for ReactJS.NET to pick it up
global.Components = Components
