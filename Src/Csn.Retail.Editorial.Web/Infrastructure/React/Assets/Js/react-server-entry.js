import '@babel/polyfill'
import React from 'react'
import ReactDOM from 'react-dom'
import ReactDOMServer from 'react-dom/server'
import global from 'global-object' 
import { Components } from './Components/componentsCollection'

// Make components globally available for ReactJS.NET to pick it up
global.React = React;
global.ReactDOM = ReactDOM;
global.ReactDOMServer = ReactDOMServer;
global.Components = Components;