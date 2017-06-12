import React from 'react'
import SearchBarCategory from 'Js/Modules/Redux/iNav/Components/searchBarCategory'
import SearchBarFormAction from 'Js/Modules/Redux/iNav/Components/searchBarFormAction'

//TODO: import styles
if (!SERVER) {
    require('Js/Modules/Redux/iNav/css/iNav.scss')  
}

const SearchBar = (props) => {

        const { iNav: { iNav: { nodes } } } = props
        const { toggleSelected, toggleActiveState } = props
        const { isActive } = props.ui 

        //Check if it has sub categories //TODO do this in connect and then reselect
        const nodesfiltered = nodes.filter(function (node) {
            return !!node.facets 
        })

        const categories = nodesfiltered.map((node) => {
            return (
                <SearchBarCategory key={node.displayName} {...node} toggleSelected={toggleSelected} />
            )
        })

        const activeClass = isActive ? 'isActive' : ''

        return (
            <div className={'searchbar ' + activeClass}>
                <div className="container">
                    <div className="searchbar__inner-wrapper">
                        <button className="searchbar__toggle-button" onClick={toggleActiveState}></button>
                        <div className="searchbar__category-container">
                            {categories}
                        </div>
                        <SearchBarFormAction/>
                    </div>
                </div>
 
            </div>
        )    
}

export default SearchBar