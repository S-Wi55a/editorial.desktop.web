import React from 'react'
import { connect } from 'react-redux'
import * as Actions from 'Js/Modules/Redux/iNav/Actions/actions'
import SearchBarCategory from 'Js/Modules/Redux/iNav/Components/searchBarCategory'
import SearchBarFormAction from 'Js/Modules/Redux/iNav/Components/searchBarFormAction'


//import styles

class SearchBar extends React.Component {

    constructor(props) {
        super(props);
        this.state = {active: false};
        this.toggleActiveState = this.toggleActiveState
    }

    toggleActiveState = () => {
        this.setState(prevState => ({
            active: !prevState.active
        }));
    }

    render() {

        const { iNav: { iNav: { nodes } } } = this.props
        const { toggleSelected } = this.props

        //Check if it has sub categories
        const nodesfiltered = nodes.filter(function (node) {
            return !!node.facets 
        })

        const categories = nodesfiltered.map((node) => {
            return (
                <SearchBarCategory key={node.displayName} {...node} toggleSelected={toggleSelected} />
            )
        })

        const activeClass = this.state.active ? 'active' : ''

        return (
            <div className={'searchbar container ' + activeClass}>
                <button onClick={this.toggleActiveState}></button>
                <div className="searchbar__category-container">
                    {categories}
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
        }
    }
}

SearchBar = connect(
    mapStateToProps,
    mapDispatchToProps
)(SearchBar)

export default SearchBar