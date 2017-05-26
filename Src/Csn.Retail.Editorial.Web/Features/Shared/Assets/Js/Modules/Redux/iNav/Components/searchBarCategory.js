import React from 'react'
import CategoryItem from 'Js/Modules/Redux/iNav/Components/categoryItem'


//TODO: check if isSelected for displaying panels

const SearchBarCategory = ({ name, displayName, facets, toggleSelected }) => {

    let refinements = {}

    const facetsArr = facets.map((facet) => {
        
        // We do this here to avoid looping through again to pull out the refinements
        if (facet.refinements) {
            refinements = {
                    ...refinements,
                    ...facet.refinements
                    }
        }

        {/* We are adding displayName for the toggleSelected cb*/}
        return <CategoryItem key={facet.displayValue} {...facet} name={name} toggleSelected={toggleSelected}/>
    })

    return (
        <div className={'searchbar__category '}>
            <div className="heading2">{displayName}</div>
            <div className="searchbar__category-container--1">
                <ul>
                    {facetsArr}
                </ul>
            </div>
            <div className="searchbar__category-container--2">
                <ul>
                    {
                    (refinements.nodes) ? 
                        refinements.nodes.map((node) => {
                            if (node.facets) {
                                return facets.map((facet) => {
                                    {/* We are adding displayName for the toggleSelected cb*/}
                                    return <CategoryItem key={facet.displayValue} {...facet} name={name} toggleSelected={toggleSelected}/> 
                                })                          
                            }
                        })
                        : ''
                    }
                </ul>
            </div>
        </div>
        )
}

export default SearchBarCategory