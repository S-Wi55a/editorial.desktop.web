import React from 'react'
import { connect } from 'react-redux'
import { Thunks } from 'iNav/Actions/actions'
import { iNav } from 'Endpoints/endpoints'

if (!SERVER) {
    require('iNavSorting/Css/iNavSorting.scss')  
}


const INavSorting = ({ sorting }) =>  {    
     return <div className='iNavSorting'>         
            <select>
                <option value="volvo">Volvo</option>
            </select>
        </div>
}

// Redux Connect
const mapStateToProps = (state) => {
    return {
        sorting: state.iNav.sorting
    }
}

// Connect the Component to the store
const INavSortingContainer = connect(
    mapStateToProps
)(INavSorting)

export default INavSortingContainer