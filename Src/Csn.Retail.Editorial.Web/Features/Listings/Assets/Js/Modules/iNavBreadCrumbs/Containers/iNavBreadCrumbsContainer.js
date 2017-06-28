import React from 'react'
import { connect } from 'react-redux'
import { createSelector } from 'reselect'
import * as Actions from 'Js/Modules/Redux/iNav/Actions/actions'

const INavBreadCrumb = ({fetchQuery, facetDisplay, removeAction}) => (
    <div className="iNavBreadCrumb" onClick={()=>fetchQuery(removeAction)}>
        {facetDisplay}
    </div>
)

const INavBreadCrumbs = ({ breadCrumbs, fetchQuery }) => (
        <div className="iNavBreadCrumbs">
            {breadCrumbs.map((breadCrumb) => {
                return <INavBreadCrumb key={breadCrumb.facetDisplay} {...breadCrumb}/>
            })}
        </div>
    )

//Selectors
const getiNavBreadCrumbs = (iNav) => {

    return [{
        "facetDisplay": "Clear All", //TODO: remove hard coded data
        "removeAction": ""
    }].concat(iNav.iNav.breadCrumbs);
        
}

// Redux Connect
const mapStateToProps = (state) => {
    return {
        breadCrumbs: getiNavBreadCrumbs(state.iNav)
    }
}

const mapDispatchToProps = (dispatch) => {
    return {
        fetchQuery: (query) => {
            dispatch(Actions.fetchQueryRequest(query))
        }
    }
}

// Connect the Component to the store
const INavBreadCrumbsContainer = connect(
    mapStateToProps,
    mapDispatchToProps
)(INavBreadCrumbs)

export default INavBreadCrumbsContainer

