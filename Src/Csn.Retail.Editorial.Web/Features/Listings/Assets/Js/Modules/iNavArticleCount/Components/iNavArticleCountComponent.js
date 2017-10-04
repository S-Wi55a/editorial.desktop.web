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
        count: state.iNav.count,
        noResultsMessage: state.iNav.noResultsMessage,
        noResultsInstructionMessage: state.iNav.noResultsInstructionMessage
    }
}

const INavArticleCountComponent = connect(
    mapStateToProps
)(INavArticleCount);

export default INavArticleCountComponent;