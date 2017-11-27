import React from 'react'
import { connect } from 'react-redux'

if (!SERVER) {
    require('iNavArticleCount/Css/iNavArticleCount.scss')
}

const INavArticleCount = ({ count, resultsMessage, noResultsMessage, noResultsInstructionMessage }) => {
    if (count > 0) {
        return <h1 className="iNavArticleCount">{resultsMessage}</h1>
    } else {
        return <div className="noResults">
                    <div className="noResults__message">{noResultsMessage}</div>
                    <div className="noResults__instructionMessage">{noResultsInstructionMessage}</div>
                </div>
    }
};

// Redux Connect
const mapStateToProps = (state) => {
    return {
        count: state.store.listings.navResults.count,
        resultsMessage: state.store.listings.navResults.resultsMessage,
        noResultsMessage: state.store.listings.navResults.noResultsMessage,
        noResultsInstructionMessage: state.store.listings.navResults.noResultsInstructionMessage
    }
}

const INavArticleCountComponent = connect(
    mapStateToProps
)(INavArticleCount);

export default INavArticleCountComponent;