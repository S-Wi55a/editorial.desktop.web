import React from 'react'
import { connect } from 'react-redux'
import ReactCSSTransitionGroup from 'react-addons-css-transition-group';
import { Thunks } from 'iNav/Actions/actions'
import { iNav } from 'Endpoints/endpoints'

const INavBreadCrumb = ({facetDisplay, removeAction}) => (
    <a className="iNavBreadCrumb" href={iNav.home(removeAction)}>{facetDisplay}</a>    
)

const INavBreadCrumbs = ({ breadCrumbs, removeBreadCrumb }) => (
        <div className="iNavBreadCrumbs">
            <ReactCSSTransitionGroup
                transitionName="iNavBreadCrumbTransition"
                transitionEnterTimeout={300}
                transitionLeaveTimeout={300}>
                {breadCrumbs.map((breadCrumb) => {
                    return <INavBreadCrumb key={breadCrumb.facetDisplay} {...breadCrumb} />
                })}
            </ReactCSSTransitionGroup>
        </div>
    )

// Redux Connect
const mapStateToProps = (state) => {
    return {
        breadCrumbs: state.iNav.iNav.breadCrumbs ? state.iNav.iNav.breadCrumbs : []
    }
}

// Connect the Component to the store
const INavBreadCrumbsContainer = connect(
    mapStateToProps
)(INavBreadCrumbs)

export default INavBreadCrumbsContainer

