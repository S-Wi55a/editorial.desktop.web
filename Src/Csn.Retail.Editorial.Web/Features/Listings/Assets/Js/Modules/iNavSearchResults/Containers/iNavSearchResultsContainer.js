﻿import React from 'react'
import { connect } from 'react-redux'
import ReactCSSTransitionGroup from 'react-addons-css-transition-group';
import INavSearchResult from 'Js/Modules/iNavSearchResults/Component/iNavSearchResult'

const INavSearchResults = ({ searchResults, count }) => (
    <div className="iNavSearchResults">
        <div className="iNavSearchResults__count">{count} Articles found</div>
        <ReactCSSTransitionGroup
            transitionName="iNavSearchResultsTransition"
            transitionEnterTimeout={300}
            transitionLeaveTimeout={300}>
            {
                searchResults.map((searchResult) => {
                    return <INavSearchResult key={searchResult.id} {...searchResult} />;
                })
            }
        </ReactCSSTransitionGroup>
        <div>{JSON.stringify(searchResults)}</div>
    </div>
);

// Redux Connect
const mapStateToProps = (state) => {
    return {
        searchResults: state.iNav.searchResults,
        count: state.iNav.count
    }
}

// Connect the Component to the store
const INavSearchResultsContainer = connect(
    mapStateToProps
)(INavSearchResults);

export default INavSearchResultsContainer;