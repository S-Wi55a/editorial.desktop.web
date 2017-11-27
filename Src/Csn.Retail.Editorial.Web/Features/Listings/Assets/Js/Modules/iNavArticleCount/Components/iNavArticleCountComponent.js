import React from 'react'
import { connect } from 'react-redux'

if (!SERVER) {
    require('iNavArticleCount/Css/iNavArticleCount.scss')
}

const INavArticleCount = ({ count, noResultsMessage, noResultsInstructionMessage }) => {
    if (count > 0) {
        return <div className="iNavArticleCount">{count.toLocaleString()} Article{count > 1 ? 's' : ''} found </div>
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
        noResultsMessage: state.store.listings.navResults.noResultsMessage,
        noResultsInstructionMessage: state.store.listings.navResults.noResultsInstructionMessage
    }
}

const INavArticleCountComponent = connect(
    mapStateToProps
)(INavArticleCount);

export default INavArticleCountComponent;