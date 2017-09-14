import React from 'react'
import { connect } from 'react-redux'
import * as Actions from 'Redux/iNav/Actions/actions'
import * as GlobalActions from 'Redux/Global/Actions/actions' //TODO: change
import INavfacet from 'iNav/Components/iNavFacet'
import * as iNavTypes from 'Redux/iNav/Types'



//TODO: handle cb for switch panel UI
//TODO: Handle cb for toggle is selected which may be different

// This is separated for displaying sub lists
const INavNodePage:React.StatelessComponent<any> = ({ facets, name, toggleIsSelected }: {facets: iNavTypes.IFacet[]}) => (
    <div>
        <div className="iNav-category__container iNav-category__container--1">
            <ul>
                {facets.map((facet) => {
                    return <INavfacet key={facet.displayValue} {...facet}/>
                })}
            </ul>        
        </div>
    </div>
);

const INavNodes:React.StatelessComponent<any> = ({nodes}:{nodes:iNavTypes.INode[]}) => (
    <div className={'iNav__category iNav-category'}>
        {nodes.map((node) => {
            return <INavNodePage key={node.name} {...node} />
        })}
    </div>
);


// Redux Connect
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
const INavNodesContainer = connect(
    null,
    mapDispatchToProps
)(INavNodes);

export default INavNodesContainer