import React from 'react'
import SearchBarCategory from 'Js/Modules/Redux/iNav/Components/searchBarCategory'
import SearchBarFormActionContainer from 'Js/Modules/Redux/iNav/Components/searchBarFormAction'

//TODO: import styles
if (!SERVER) {
    require('Js/Modules/Redux/iNav/css/iNav.scss')  
}

const SearchBar = ({ nodes, toggleSelected, toggleIsActive, ui }) => {

        const activeClass = ui.isActive ? 'isActive' : ''

        return (
            <div className={'searchbar ' + activeClass}>
                <div className="container">
                    <div className="searchbar__inner-wrapper">
                        <button className="searchbar__toggle-button" onClick={toggleIsActive}></button>
                        <div className="searchbar__category-container">
                            {nodes.map((node) => {
                                return (<SearchBarCategory key={node.displayName} {...node} toggleSelected={toggleSelected} />)
                            })}
                        </div>
                        <SearchBarFormActionContainer/>
                    </div>
                </div>
 
            </div>
        )    
}

export default SearchBar