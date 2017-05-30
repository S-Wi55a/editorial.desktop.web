import React from 'react'
import { connect } from 'react-redux'
import ui from 'redux-ui';
import * as Actions from 'Js/Modules/Redux/iNav/Actions/actions'
import { uiReducer } from 'Js/Modules/Redux/iNav/Reducers/uiReducer'
import SearchBarCategory from 'Js/Modules/Redux/iNav/Components/searchBarCategory'
import SearchBarFormAction from 'Js/Modules/Redux/iNav/Components/searchBarFormAction'

//TODO: import styles
import 'Js/Modules/Redux/iNav/css/iNav.scss'

@ui({
    key: 'searchBar',
    state: {
        isActive: false,
    },
    reducer: uiReducer
})
class SearchBar extends React.Component {

    constructor(props) {
        super(props);
    }

    render() {

        const { iNav: { iNav: { nodes } } } = this.props
        const { toggleSelected, toggleActiveState } = this.props
        const { isActive } = this.props.ui 

        //Check if it has sub categories
        const nodesfiltered = nodes.filter(function (node) {
            return !!node.facets 
        })

        const categories = nodesfiltered.map((node) => {
            return (
                <SearchBarCategory key={node.displayName} {...node} toggleSelected={toggleSelected} />
            )
        })

        const activeClass = isActive ? 'active' : ''

        return (
            <div className={'searchbar ' + activeClass}>
                <div className="container">
                    <div className="searchbar__inner-wrapper">
                        <button className="searchbar__toggle-button" onClick={toggleActiveState}></button>
                        <div className="searchbar__category-container">
                            {categories}
                        </div>
                    </div>
                </div>
                <SearchBarFormAction/>
            </div>
        )    
    }
}


const mapStateToProps = (state) => {
    return {
        iNav: state.iNav
    }
}

const mapDispatchToProps = (dispatch) => {
    return {
        toggleSelected: (isSelected, node, facet) => {
            dispatch([
                Actions.toggleIsSelected(isSelected, node, facet),
                Actions.updateQuery()
                ])
        },
        toggleActiveState: () => {
            dispatch(Actions.toggleIsActive())
        }
    }
}

SearchBar = connect(
    mapStateToProps,
    mapDispatchToProps
)(SearchBar)

export default SearchBar