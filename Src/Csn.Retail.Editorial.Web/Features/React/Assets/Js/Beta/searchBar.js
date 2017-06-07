import { Provider } from 'react-redux'
import * as s  from './storeClient'



import React from 'react'
import { connect } from 'react-redux'
import ui from 'redux-ui';
import { uiReducer } from 'Js/Modules/Redux/iNav/Reducers/uiReducer'
import * as Actions from 'Js/Modules/Redux/iNav/Actions/actions'
//import SearchBarCategory from 'Js/Modules/Redux/iNav/Components/searchBarCategory'
//import SearchBarFormAction from 'Js/Modules/Redux/iNav/Components/searchBarFormAction'

//TODO: import styles
//import 'Js/Modules/Redux/iNav/css/iNav.scss'

//@ui({
//    key: 'searchBar',
//    state: {
//        isActive: false,
//    },
//    reducer: uiReducer
//})
class SearchBar extends React.Component {

    constructor(props) {
        super(props);
    }

    render() {



        return (
            <div className={'searchbar ' }>
                <div className="container">
                    Apples: {this.props.iNav}
                </div>
                
            </div>
        )    
    }
}


const mapStateToProps = (state) => {
    return {
        iNav: state.apples
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




const render = (props) => {
    return (
        <SearchBar store={s.store}/>
    );
};

var SearchBarTest = render

export default SearchBarTest