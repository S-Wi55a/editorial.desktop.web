import React from 'react'
import ReactDOM from 'react-dom'
import Redux from 'redux'
import ReactRedux from 'react-redux'
import Immutable from 'immutable'

import * as store from 'Js/Modules/Redux/Global/Store/store'

//Because ui uses Immutable or we get an error
window.__PRELOADED_STATE__.ui = Immutable.fromJS(window.__PRELOADED_STATE__.ui);

//Enable Redux store globally
window.store = store.configureStore(window.__PRELOADED_STATE__) //Init store
window.injectAsyncReducer = store.injectAsyncReducer