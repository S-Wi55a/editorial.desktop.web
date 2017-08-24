﻿import React from 'react'
import { connect } from 'react-redux'
import ReactCSSTransitionGroup from 'react-addons-css-transition-group';
import * as Actions from 'Js/Modules/Redux/iNav/Actions/actions'

const INavBreadCrumb = ({removeBreadCrumb, aspect, facet, facetDisplay, removeAction}) => (
    <div className="iNavBreadCrumb" onClick={() =>removeBreadCrumb(removeAction, aspect, facet)}>
        {facetDisplay}
    </div>
)

const INavBreadCrumbs = ({ breadCrumbs, removeBreadCrumb }) => (
        <div className="iNavBreadCrumbs">
            <ReactCSSTransitionGroup
                transitionName="iNavBreadCrumbTransition"
                transitionEnterTimeout={300}
                transitionLeaveTimeout={300}>
                {breadCrumbs.map((breadCrumb) => {
                    return <INavBreadCrumb key={breadCrumb.facetDisplay} {...breadCrumb} removeBreadCrumb={removeBreadCrumb}/>
                })}
            </ReactCSSTransitionGroup>
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
        removeBreadCrumb: (query, aspect, facet) => {
            dispatch([
                Actions.fetchQueryRequest(query),
                Actions.removeBreadCrumb(facet), // This is to simulate a quick UI
                Actions.toggleIsSelected(aspect, facet), // This is to simulate a quick UI
            ])
        }
    }
}

// Connect the Component to the store
const INavBreadCrumbsContainer = connect(
    mapStateToProps,
    mapDispatchToProps
)(INavBreadCrumbs)

export default INavBreadCrumbsContainer
