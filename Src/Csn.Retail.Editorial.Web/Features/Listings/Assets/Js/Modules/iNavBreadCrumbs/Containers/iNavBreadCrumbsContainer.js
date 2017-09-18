import React from 'react'
import { connect } from 'react-redux'
import ReactCSSTransitionGroup from 'react-addons-css-transition-group';
import { fetchINav } from 'Redux/iNav/Actions/actions'


const INavBreadCrumb = ({removeBreadCrumb, aspect, facet, facetDisplay, removeAction}) => (
    <div className="iNavBreadCrumb" onClick={() =>removeBreadCrumb(removeAction)}>
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

// Redux Connect
const mapStateToProps = (state) => {
    return {
        breadCrumbs: state.iNav.iNav.breadCrumbs ? state.iNav.iNav.breadCrumbs : []
    }
}

const mapDispatchToProps = (dispatch) => {
    return {
        removeBreadCrumb: (query) => {
            dispatch(fetchINav(query))
        }
    }
}

// Connect the Component to the store
const INavBreadCrumbsContainer = connect(
    mapStateToProps,
    mapDispatchToProps
)(INavBreadCrumbs)

export default INavBreadCrumbsContainer

