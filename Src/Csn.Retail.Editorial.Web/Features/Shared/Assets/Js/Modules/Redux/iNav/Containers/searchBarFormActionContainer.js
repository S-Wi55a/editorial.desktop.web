import React from 'react'
import { connect } from 'react-redux'
import * as Actions from 'Js/Modules/Redux/iNav/Actions/actions'
import SearchBarFormAction from 'Modules/Redux/iNav/Components/searchBarFormAction'

function getiNavCount(iNav) {
    return iNav.count
}

const mapStateToProps = (state) => {
    return {
        count: getiNavCount(state),
        href: ''//TODO: get multiselected query
    }
}

const mapDispatchToProps = (dispatch) => {
    return {
        resetForm: () => {
            dispatch(Actions.resetForm())
        }
    }
}

SearchBarFormAction = connect(
    mapStateToProps,
    mapDispatchToProps
)(SearchBarFormAction)

export default SearchBarFormActionContainer


//TPo add loading spinner