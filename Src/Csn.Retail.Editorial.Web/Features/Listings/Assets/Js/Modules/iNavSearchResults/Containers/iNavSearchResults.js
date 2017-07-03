import React from 'react'
import { connect } from 'react-redux'
import ReactCSSTransitionGroup from 'react-addons-css-transition-group';

const INavSearchResult = ({removeBreadCrumb, facet, facetDisplay, removeAction}) => (
    <div className="iNavBreadCrumb" onClick={() =>removeBreadCrumb(removeAction, facet)}>
        {facetDisplay}
    </div>
)

const INavSearchResults = ({ searchResults }) => (
    <div className="iNavBreadCrumbs">
            <ReactCSSTransitionGroup
                transitionName="iNavBreadCrumbTransition"
                transitionEnterTimeout={300}
                transitionLeaveTimeout={300}>
                {searchResults.map((searchResult) => {
                    return <INavSearchResult key={searchResult.UniqueId} {...searchResult} />
                })}
            </ReactCSSTransitionGroup>
        </div>
)

//Selectors
const getiNavSearchResults = (searchResults) => {
    return searchResults        
}

// Redux Connect
const mapStateToProps = (state) => {
    return {
        searchResults: getiNavSearchResults(state.searchResults)
    }
}

// Connect the Component to the store
const INavSearchResultsContainer = connect(
    mapStateToProps,
)(INavSearchResults)

export default INavSearchResultsContainer 


