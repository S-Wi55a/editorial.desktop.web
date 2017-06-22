import React from 'react'
import ui from 'redux-ui';
import { connect } from 'react-redux'
import * as Actions from 'Js/Modules/Redux/iNav/Actions/actions'
import * as SearchBarActions from 'Js/Modules/Redux/SearchBar/Action/actionTypes'
import { getUI } from 'Js/Modules/Redux/Global/Helpers/UIHelpers'


const SearchBarFormAction = ({reset, count, href, ui, uiKey, updateUI}) => {

    const isLoadingClass = ui.isLoading ? 'isLoading' : ''

    console.log('Searchbar comp: ', ui.isLoading)
    //TODO: remove hard coded text
    return (
        <div className={'searchbar-form-action ' + isLoadingClass}>
            <div className="searchbar-form-action__loader"></div>
            <button className="searchbar-form-action__button searchbar-form-action__button--clear" onClick={reset}>Clear</button>
            <a className="searchbar-form-action__button searchbar-form-action__button--confirm" href={href}>{count} Articles</a>
        </div>
    )
}


// Selectors
function getiNavCount(iNav) {
    return iNav.count
}

const mapStateToProps = (state) => {
    return {
        count: getiNavCount(state.iNav),
        href: ''//TODO: get multiselected query
    }
}

const mapDispatchToProps = (dispatch) => {
    return {
        reset: () => {
            dispatch(Actions.reset())
        }
    }
}

const SearchBarFormActionConnect = connect(
    mapStateToProps,
    mapDispatchToProps
)(SearchBarFormAction)


// This must be after Connect because we use the ui props to in connect
// Add the UI to the store
const SearchBarFormActionContainer = ui({
    key: 'searchBarFormAction',
    state: {
        isLoading: getUI(['searchBar','searchBarFormAction','isLoading']) || false,
    },
    // customReducer: you can handle the UI state for this component's scope by dispatching actions
    reducer: (state, action) => {
        // state represents *only* the UI state for this component's scope - not any children
        switch (action.type) {
        case SearchBarActions.SHOW_LOADER:
            return state.set('isLoading', true)
        case SearchBarActions.HIDE_LOADER:
            return state.set('isLoading', false)
        default:
            return state
        }
    }
})(SearchBarFormActionConnect);

export default SearchBarFormActionContainer


//TODO: add loading spinner