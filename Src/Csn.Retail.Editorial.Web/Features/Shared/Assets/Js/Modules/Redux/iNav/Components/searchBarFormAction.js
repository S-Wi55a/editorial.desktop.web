import React from 'react'
import { connect } from 'react-redux'
import * as Actions from 'Js/Modules/Redux/iNav/Actions/actions'
import { ryvuss } from 'Js/Modules/Endpoints/endpoints'


class SearchBarFormAction extends React.Component { 

    constructor(props) {
        super(props);
        this.resetForm = this.resetForm
    }

    componentWillMount() {
        this.props.updateQuery()
    }

    resetForm = (e) => {
        e.preventDefault()
        this.props.resetForm()
    }

    render() {

        //TODO: remove hard coded text
        return (
            <div>
                <a onClick={this.resetForm}>clear</a>
                <a href={`${ryvuss.iNavWithCount}`}>{this.props.count}Articles</a>
            </div>
        )
    }
}

const mapStateToProps = (state) => {
    return {
        iNavQuery: state.iNav.iNavQuery,
        count: state.iNav.count
    }
}

const mapDispatchToProps = (dispatch) => {
    return {
        updateQuery: () => {
            dispatch(Actions.updateQuery())
        },
        resetForm: () => {
            dispatch(Actions.resetForm())
        }
    }
}

SearchBarFormAction = connect(
    mapStateToProps,
    mapDispatchToProps
)(SearchBarFormAction)

export default SearchBarFormAction