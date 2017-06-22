import React from 'react'
import { connect } from 'react-redux'
import * as Actions from 'Js/Modules/Redux/iNav/Actions/actions'
import SearchBarFormAction from 'Js/Modules/Redux/SearchBar/Components/searchBarFormAction'

function getiNavCount(iNav) {
    return iNav.count
}

const mapStateToProps = (state) => {
    return {
        count: getiNavCount(state.iNav),
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

const SearchBarFormActionContainer = connect(
    mapStateToProps,
    mapDispatchToProps
)(SearchBarFormAction)

export default SearchBarFormActionContainer


//TODO: add loading spinner