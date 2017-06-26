import React from 'react'
import { connect } from 'react-redux'
import ui from 'redux-ui';
import INavNodesContainer from 'Js/Modules/Redux/iNav/Containers/iNavNodesContainer'
import SearchBarFormActionContainer from 'Js/Modules/Redux/SearchBar/Containers/searchBarFormActionContainer'
import { getUI } from 'Js/Modules/Redux/Global/Helpers/UIHelpers'
import * as SearchBarActions from 'Js/Modules/Redux/SearchBar/Action/actionTypes'

const SearchBarComponent = ( {ui, toggleIsSelected }) => {

    const activeClass = ui.isActive ? 'isActive' : ''

    return (
        <div className={'searchbar ' + activeClass}>
                <div className="container">
                    <div className="searchbar__inner-wrapper">
                        <button className="searchbar__toggle-button" onClick={toggleIsSelected}></button>
                        <div className="searchbar__category-container">
                            <INavNodesContainer/>
                        </div>
                        <SearchBarFormActionContainer/>
                    </div>
                </div>
 
            </div>
    )    
}

//Connect
const mapDispatchToProps = (dispatch) => {
    return {
        toggleIsSelected: () => {
            dispatch({type:SearchBarActions.TOGGLE_IS_ACTIVE})
        }
    }
}

const SearchBarComponentConnect = connect(
    null,
    mapDispatchToProps
)(SearchBarComponent)


// Add the UI to the store
const SearchBar = ui({
    key: 'searchBar',
    state: {
        isActive: getUI('searchBar.isActive') || false,
    },
    // customReducer: you can handle the UI state for this component's scope by dispatching actions
    reducer: (state, action) => {
        // state represents *only* the UI state for this component's scope - not any children
        switch (action.type) {
        case SearchBarActions.TOGGLE_IS_ACTIVE:
            return state.set('isActive', !state.get('isActive'))
        default:
            return state
        }
    }
})(SearchBarComponentConnect);

export default SearchBar

