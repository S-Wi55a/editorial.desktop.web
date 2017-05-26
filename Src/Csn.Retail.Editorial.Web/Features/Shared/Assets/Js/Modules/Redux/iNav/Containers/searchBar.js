import React from 'react'
import { connect } from 'react-redux'
import * as Actions from 'Js/Modules/Redux/iNav/Actions/actions'
import SearchBarCategory from 'Js/Modules/Redux/iNav/Components/searchBarCategory'

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
        const { filterTypes, toggleSelected } = this.props

        const nodesfiltered = nodes.filter(function (node) {
            return this.find((filterType) => {
                return node.name === filterType
            });
        }, filterTypes)

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
            </div>
        )    
    }
}


const mapStateToProps = (state) => {
    return {
        iNav: state.iNav,
        filterTypes: ['ArticleTypes', 'BodyType', 'Make', 'Model']
    }
}

const mapDispatchToProps = (dispatch) => {
    return {
        toggleSelected: (isSelected, node, facet) => {
            dispatch(Actions.toggleIsSelected(isSelected, node, facet))
        }
    }
}

SearchBar = connect(
    mapStateToProps,
    mapDispatchToProps
)(SearchBar)

export default SearchBar