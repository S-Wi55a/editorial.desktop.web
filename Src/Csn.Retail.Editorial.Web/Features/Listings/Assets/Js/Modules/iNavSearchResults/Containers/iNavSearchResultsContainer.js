import React from 'react'
import { connect } from 'react-redux'
import ReactCSSTransitionGroup from 'react-addons-css-transition-group';
import INavSearchResult from 'iNavSearchResults/Component/iNavSearchResult'

const INavSearchResults = ({ searchResults, count }) => (
    <div className="iNavSearchResults">        
        <ReactCSSTransitionGroup
            transitionName="iNavSearchResultsTransition"
            transitionEnterTimeout={300}
            transitionLeaveTimeout={300}>
            {
                searchResults.map((searchResult, index) => {
                    return <INavSearchResult key={index} {...searchResult} />;
                })
            }
        </ReactCSSTransitionGroup>
        <div>{JSON.stringify(searchResults)}</div>
    </div>
);

// Redux Connect
const mapStateToProps = (state) => {
    return {
        searchResults: state.iNav.searchResults        
    }
}

// Connect the Component to the store
const INavSearchResultsContainer = connect(
    mapStateToProps
)(INavSearchResults);

export default INavSearchResultsContainer;