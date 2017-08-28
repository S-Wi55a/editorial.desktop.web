import React from 'react'
import { connect } from 'react-redux'
import * as Actions from 'Js/Modules/Redux/iNav/Actions/actions'
import * as GlobalActions from 'Js/Modules/Redux/Global/Actions/actions'
import INavfacet from 'Js/Modules/Redux/iNav/Components/iNavFacet'


//TODO: handle cb for switch panel UI
//TODO: Handle cb for toggle is selected which may be different

// This is separated for displaying sub lists
const INavNodeList = ({ facets, name, toggleIsSelected }) => (
    <div>
        <div className="iNav-category__container iNav-category__container--1">
            <ul>
                {facets.map((facet) => {
                    return <INavfacet key={facet.displayValue} {...facet} name={name} toggleIsSelected={toggleIsSelected}/>
                })}
            </ul>        
        </div>
    </div>
);


const INavNode = ({ node, toggleIsSelected }) => (
    <div className={'iNav__category iNav-category'}>
        <INavNodeList {...node} toggleIsSelected={toggleIsSelected} />
    </div>
);


// Redux Connect
const mapStateToProps = (state, ownProps) => {
    return {
    
    }
};

const mapDispatchToProps = (dispatch) => {
    return {
        toggleIsSelected: (isSelected, node, facet, query) => {
            dispatch([
                Actions.fetchQueryRequest(query),
                Actions.toggleIsSelected(node, facet), // This is to simulate a quick UI
                isSelected ? Actions.removeBreadCrumb(facet) : GlobalActions.noop() // This is to simulate a quick UI
            ]);
        }
    }
}

// Connect the Component to the store
const INavNodeContainer = connect(
    null,
    mapDispatchToProps
)(INavNode);

export default INavNodeContainer