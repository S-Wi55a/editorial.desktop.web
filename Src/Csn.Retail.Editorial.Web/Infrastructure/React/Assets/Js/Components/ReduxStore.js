import React from 'react'
import { connect } from 'react-redux'

// WARNING: See the following for security issues around embedding JSON in HTML:
// http://redux.js.org/docs/recipes/ServerRendering.html#security-considerations

function createMarkup(preloadedState, key) {
    
    //The key should be a top level part of the state tree
    const state = preloadedState ? {__html: `window.__PRELOADED_STATE__${key} = ${JSON.stringify(preloadedState[key]).replace(/</g, '\\u003c')}`} : {}
    return state
}

const Store = props => <script dangerouslySetInnerHTML={createMarkup(props.store, props.reducerKey)} />
    
const mapStateToProps = (state) => {
    return {
        store: state
    }
}

const ReduxStore = connect(
    mapStateToProps,
)(Store)

export {ReduxStore}