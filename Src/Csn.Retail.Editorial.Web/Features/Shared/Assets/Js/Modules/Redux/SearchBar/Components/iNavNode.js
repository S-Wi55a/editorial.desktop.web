import React from 'react'
import INavfacet from 'Js/Modules/Redux/iNav/Components/iNavfacet'


//TODO: check if isSelected for displaying panels

const INavNode = ({ name, displayName, facets, toggleIsSelected }) => {


    console.log('Category Rendered')
    let refinements = {}


    //TODO: don't like thsi, maybe use connect here instead
    const facetsArr = facets.map((facet) => {
        
        // We do this here to avoid looping through again to pull out the refinements
        if (facet.refinements) {
            refinements = {
                    ...refinements,
                    ...facet.refinements
                    }
        }

        {/* We are adding displayName for the toggleSelected cb*/}
        return <INavfacet key={facet.displayValue} {...facet} name={name} toggleIsSelected={toggleIsSelected}/>
    })

    return (
        <div className={'searchbar__category searchbar-category'}>
            <div className="searchbar-category__heading">{displayName}</div>
            <div className="searchbar-category__container searchbar-category__container--1">
                <ul>
                    {facetsArr}
                </ul>
            </div>
            <div className="searchbar-category__container searchbar-category__container--2">
                <ul>
                    {
                    (refinements.nodes) ? 
                        refinements.nodes.map((node) => {
                            if (node.facets) {
                                return facets.map((facet) => {
                                    {/* We are adding displayName for the toggleSelected cb*/}
                                    return <INavfacet key={facet.displayValue} {...facet} name={name} toggleIsSelected={toggleIsSelected}/> 
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

export default INavNode