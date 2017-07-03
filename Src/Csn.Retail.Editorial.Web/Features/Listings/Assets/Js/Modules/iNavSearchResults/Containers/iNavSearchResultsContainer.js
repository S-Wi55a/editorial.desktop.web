import React from 'react'
import { connect } from 'react-redux'
import ReactCSSTransitionGroup from 'react-addons-css-transition-group';
import INavSearchResult from 'Js/Modules/iNavSearchResults/Component/iNavSearchResult'

const INavSearchResults = ({ searchResults, count }) => (
    <div className="iNavSearchResults">
        <div className="iNavSearchResults__count">{count} Articles found {/* TODO: remove hardcoded text */}</div>
            <ReactCSSTransitionGroup
                transitionName="iNavSearchResultsTransition"
                transitionEnterTimeout={300}
                transitionLeaveTimeout={300}>
                {searchResults.map((searchResult) => {
                    return <INavSearchResult key={searchResult.UniqueId} {...searchResult} />
                })}
            </ReactCSSTransitionGroup>
        </div>
)

//Selectors
const getiNavSearchResults = (state) => {
    return state.searchResults        
}
const getiNavCount = (state) => {
    return state.count        
}

// Redux Connect
const mapStateToProps = (state) => {
    return {
        searchResults: getiNavSearchResults(state.iNav),
        count: getiNavCount(state.iNav)
    }
}

// Connect the Component to the store
const INavSearchResultsContainer = connect(
    mapStateToProps,
)(INavSearchResults)

export default INavSearchResultsContainer 


