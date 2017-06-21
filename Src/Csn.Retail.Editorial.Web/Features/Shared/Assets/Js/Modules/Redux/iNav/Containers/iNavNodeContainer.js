import React from 'react'
import { connect } from 'react-redux'
import * as Actions from 'Js/Modules/Redux/iNav/Actions/actions'
import INavfacet from 'Js/Modules/Redux/iNav/Components/iNavFacet'


//TODO: handle cb for switch panel UI
//TODO: Handle cb for toggle is selected which may be different


const INavNodeList = ({displayName, facets, name, toggleIsSelected}) => (  
    <div>
        <div className="searchbar-category__heading">{displayName}</div>
        <div className="searchbar-category__container searchbar-category__container--1">
            <ul>
                {facets.map((facet) => {
                    return <INavfacet key={facet.displayValue} {...facet} name={name} toggleIsSelected={toggleIsSelected}/>
                })}
            </ul>        
        </div>
    </div>

    )


const INavNode = ({ node, refinements, toggleIsSelected }) => (
        <div className={'searchbar__category searchbar-category'}>
            <INavNodeList {...node} toggleIsSelected={toggleIsSelected} />
            {
                (Object.getOwnPropertyNames(refinements).length > 0) ? 
                    <div className="searchbar-category__container searchbar-category__container--2">
                        <INavNodeList {...refinements} toggleIsSelected={toggleIsSelected} />
                    </div>
                : ''
            }
        </div>
    )


//Selectors
//NOTE: we are not memonizing here becuase we are repalcing the whole iNav obj on every fetch which results ina full rerender
const getRefinements = (state, props) => {

    let refinements = {}
    if (props.facets) {
        props.facets.forEach((facet) => {
        
            if (facet.refinements) {
                refinements = {
                    ...refinements,
                    ...facet.refinements
                }
            }
        })
    }

    return refinements.nodes ? refinements.nodes[0] : refinements
}

// Redux Connect
const mapStateToProps = (state, ownProps) => {
    return {
        refinements: getRefinements(state, ownProps.node)
    }
}

const mapDispatchToProps = (dispatch) => {
    return {
        toggleIsSelected: (isSelected, node, facet, query) => {
            dispatch([
                Actions.fetchQueryRequest(query),
                Actions.toggleIsSelected(isSelected, node, facet)
            ])
        }
    }
}

// Connect the Component to the store
const INavNodeContainer = connect(
    mapStateToProps,
    mapDispatchToProps
)(INavNode)

export default INavNodeContainer