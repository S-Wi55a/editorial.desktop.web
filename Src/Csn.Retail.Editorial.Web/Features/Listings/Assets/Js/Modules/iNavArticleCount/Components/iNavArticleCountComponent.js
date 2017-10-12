import React from 'react'
import { connect } from 'react-redux'

if (!SERVER) {
    require('iNavArticleCount/Css/iNavArticleCount.scss')
}

const INavArticleCount = ({ count, noResultsMessage, noResultsInstructionMessage }) => {
    if (count > 0) {
        return <div className="iNavArticleCount">{count} Articles found </div>
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
        count: state.csn_search.iNav.count,
        noResultsMessage: state.csn_search.iNav.noResultsMessage,
        noResultsInstructionMessage: state.csn_search.iNav.noResultsInstructionMessage
    }
}

const INavArticleCountComponent = connect(
    mapStateToProps
)(INavArticleCount);

export default INavArticleCountComponent;