import React from 'react'
import { connect } from 'react-redux'

const AddDataToStore = props => <div style={{ display: 'none' }}></div>

// WARNING: See the following for security issues around embedding JSON in HTML:
// http://redux.js.org/docs/recipes/ServerRendering.html#security-considerations

function createMarkup(preloadedState) {
    
    const state = preloadedState ? {__html: `window.__PRELOADED_STATE__ = ${JSON.stringify(preloadedState).replace(/</g, '\\u003c')}`} : {}
    return state
};

function deleteStore(storeId) {
    delete global.stores[storeId]
}

const Store = props => {

    // Remove Reference to global.Stores to hopefully help with Garbage Colelction
    deleteStore(props.storeId)
    return <script dangerouslySetInnerHTML={createMarkup(props.store)} />
    }

const mapStateToProps = (state) => {
    return {
        store: state
    }
}

const ReduxStore = connect(
    mapStateToProps,
)(Store)

export {ReduxStore, AddDataToStore}