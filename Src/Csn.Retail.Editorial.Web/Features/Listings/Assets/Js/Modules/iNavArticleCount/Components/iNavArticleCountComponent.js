import React from 'react'
import { connect } from 'react-redux'

if (!SERVER) {
    require('iNavArticleCount/Css/iNavArticleCount.scss');
}

const INavArticleCount = ({ count, resultsMessage, noResultsInstructionMessage }) => {
    var noResultsMsg;

    if (count < 1) {
        noResultsMsg = <div className="iNavArticleCount__instructionMessage">{noResultsInstructionMessage}</div>;
    }

    return (
        <div className="iNavArticleCount">
            <h1 className="iNavArticleCount__count">{resultsMessage}</h1>
            {noResultsMsg}
        </div>);
};

// Redux Connect
const mapStateToProps = (state) => {
    return {
        count: state.store.listings.navResults.count,
        resultsMessage: state.store.listings.navResults.resultsMessage,
        noResultsInstructionMessage: state.store.listings.navResults.noResultsInstructionMessage
    }
}

const INavArticleCountComponent = connect(
    mapStateToProps
)(INavArticleCount);

export default INavArticleCountComponent;