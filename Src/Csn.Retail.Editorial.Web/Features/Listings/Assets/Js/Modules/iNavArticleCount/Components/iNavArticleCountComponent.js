import React from 'react'
import { connect } from 'react-redux'


const INavArticleCount = ({ count }) => (
    <div className="iNavArticleCount">{count} Articles found</div>
);

// Redux Connect
const mapStateToProps = (state) => {
    return {
        count: state.iNav.count
    }
}

const INavArticleCountComponent = connect(
    mapStateToProps
)(INavArticleCount);

export default INavArticleCountComponent;