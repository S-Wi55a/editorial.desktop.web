import React from 'react'
import { connect } from 'react-redux'
import { CSSTransitionGroup } from 'react-transition-group'
import INavSearchResult from 'iNavSearchResults/Component/iNavSearchResult'

if (!SERVER) {
    require('iNavSearchResults/Css/iNavSearchResults.scss')
}

const INavSearchResults = ({ searchResults }) => (
    <div className="iNavSearchResults">        
        <CSSTransitionGroup 
            transitionName="iNavSearchResultsTransition"
            transitionEnterTimeout={500}
            transitionLeaveTimeout={500}>
            {
                searchResults.map((searchResult, index) => {
                    return <INavSearchResult key={index} {...searchResult} />;
                })
            }
        </CSSTransitionGroup>
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