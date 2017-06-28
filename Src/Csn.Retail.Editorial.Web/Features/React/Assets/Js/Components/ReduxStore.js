import React from 'react'
import { connect } from 'react-redux'

const AddDataToStore = props => <div style={{ display: 'none' }}></div>

// WARNING: See the following for security issues around embedding JSON in HTML:
// http://redux.js.org/docs/recipes/ServerRendering.html#security-considerations

function createMarkup(preloadedState) {
    
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

export {ReduxStore, AddDataToStore}