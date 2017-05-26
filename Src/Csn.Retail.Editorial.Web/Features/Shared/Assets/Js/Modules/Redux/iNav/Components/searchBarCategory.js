import React from 'react'
import CategoryItem from 'Js/Modules/Redux/iNav/Components/categoryItem'

const SearchBarCategory = ({ name, displayName, facets, toggleSelected }) => {

    return (
        <div className={'searchbar__category '}>
            <div className="heading2">{displayName}</div>
            <ul>
            {
                facets.map((facet) => {
                    {/* We are adding displayName for the toggleSelected cb*/}
                    return <CategoryItem key={facet.displayValue} {...facet} name={name} toggleSelected={toggleSelected}/>
                })
            }
            </ul>
        </div>
        )
}

export default SearchBarCategory