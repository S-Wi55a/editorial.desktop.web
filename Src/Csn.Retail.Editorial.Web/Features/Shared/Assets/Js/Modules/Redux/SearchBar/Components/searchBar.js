import React from 'react'
import { connect } from 'react-redux'
import ui from 'redux-ui';
import INavNodesContainer from 'Js/Modules/Redux/iNav/Containers/iNavNodesContainer'
import SearchBarFormActionContainer from 'Js/Modules/Redux/iNav/Components/searchBarFormAction'

//TODO: import styles
if (!SERVER) {
    require('Js/Modules/Redux/iNav/css/iNav.scss')  
}

const SearchBarComponent = ( {ui, updateUI }) => {

    const activeClass = ui.isActive ? 'isActive' : ''

    return (
        <div className={'searchbar ' + activeClass}>
                <div className="container">
                    <div className="searchbar__inner-wrapper">
                        <button className="searchbar__toggle-button" onClick={() => { updateUI('isActive', !ui.isActive) }}></button>
                        <div className="searchbar__category-container">
                            <INavNodesContainer/>
                        </div>
                        <SearchBarFormActionContainer/>
                    </div>
                </div>
 
            </div>
    )    
}

// Add the UI to the store
// This must be after Connect because we use the ui props to in connect
const SearchBar = ui({
    key: 'searchBar',
    state: {
        isActive: false,
    }
})(SearchBarComponent);


export default SearchBar