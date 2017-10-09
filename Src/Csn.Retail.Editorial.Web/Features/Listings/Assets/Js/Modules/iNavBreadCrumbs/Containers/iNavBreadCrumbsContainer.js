import React from 'react'
import { connect } from 'react-redux'
import { CSSTransitionGroup } from 'react-transition-group'
import { Thunks } from 'iNav/Actions/actions'

const INavBreadCrumb = ({removeBreadCrumb, facetDisplay, removeAction}) => (
    <div className="iNavBreadCrumb" onClick={() =>removeBreadCrumb(removeAction)}>
        {facetDisplay}
    </div>
)

const INavBreadCrumbs = ({ breadCrumbs, removeBreadCrumb }) => (
        <div className={breadCrumbs.length > 0 ? 'iNavBreadCrumbs' : ''}>
            <CSSTransitionGroup
                transitionName="iNavBreadCrumbTransition"
                transitionEnterTimeout={300}
                transitionLeaveTimeout={300}>
                {breadCrumbs.map((breadCrumb) => {
                    return <INavBreadCrumb key={breadCrumb.facetDisplay} {...breadCrumb} removeBreadCrumb={removeBreadCrumb}/>
                })}
            </CSSTransitionGroup>
        </div>
    )

// Redux Connect
const mapStateToProps = (state) => {
    return {
        breadCrumbs: state.iNav.iNav.breadCrumbs ? state.iNav.iNav.breadCrumbs : []
    }
}

const mapDispatchToProps = (dispatch) => {
    return {
        removeBreadCrumb: (query) => {
            dispatch(Thunks.fetchINav(query))
        }
    }
}

// Connect the Component to the store
const INavBreadCrumbsContainer = connect(
    mapStateToProps,
    mapDispatchToProps
)(INavBreadCrumbs)

export default INavBreadCrumbsContainer

