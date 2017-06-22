import React from 'react'
import { connect } from 'react-redux'
import {Map} from 'immutable'

// WARNING: See the following for security issues around embedding JSON in HTML:
// http://redux.js.org/docs/recipes/ServerRendering.html#security-considerations

function createMarkup(preloadedState) {

    ////Because JSON.stringify does not convert functions to a string when reading on client this breaks
    ////It's not a problem because all we are concerned about is the state the reducers will be reinitilized 
    ////When build on the front end
    //preloadedState.ui = preloadedState.ui.set('__reducers', new Map({}))
    
    const state = preloadedState ? {__html: `window.__PRELOADED_STATE__ = ${JSON.stringify(preloadedState).replace(/</g, '\\u003c')}`} : {}
    return state
};

const Store = props => <script dangerouslySetInnerHTML={createMarkup(props.store)} />
  
const mapStateToProps = (state) => {
    return {
        store: state
    }
}

const ReduxStore = connect(
    mapStateToProps,
)(Store)


export {ReduxStore}