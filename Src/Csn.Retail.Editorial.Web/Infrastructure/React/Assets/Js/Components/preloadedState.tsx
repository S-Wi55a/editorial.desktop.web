import * as React from "react";
import { connect } from 'react-redux'

// WARNING: See the following for security issues around embedding JSON in HTML:
// http://redux.js.org/docs/recipes/ServerRendering.html#security-considerations

function createMarkup(preloadedState: any, key: any) {
    
    //The key should be a top level part of the state tree
    const state = preloadedState ? `
        window.__PRELOADED_STATE__ = window.__PRELOADED_STATE__ || {}
        window.__PRELOADED_STATE__${key} = ${JSON.stringify(preloadedState[key]).replace(/</g, '\\u003c')}
        ` : ''
    return state
}

const Store = (props: any) => <script dangerouslySetInnerHTML={{__html: createMarkup(props.store, props.reducerKey)}} />
    
const mapStateToProps = (state: any) => {
    return {
        store: state
    }
}

const PreloadedState = connect(
    mapStateToProps,
)(Store)

export default PreloadedState