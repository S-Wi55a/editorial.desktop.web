import React from 'react'
import { connect } from 'react-redux'
import Timer from 'ReactAnimations/Timer'
import { Fade } from 'ReactAnimations/Fade'
import INavSearchResult from 'iNavSearchResults/Component/iNavSearchResult'
import UI from 'ReactReduxUI'
import { ActionTypes } from 'iNav/Actions/actions'
import update from 'immutability-helper'

if (!SERVER) {
    require('iNavSearchResults/Css/iNavSearchResults.scss')
}
    
class INavSearchResults extends React.Component {
    render() {
      return (
        <div className="iNavSearchResults">        
            {
                this.props.searchResults.map((searchResult,i) => {
                    let animationDuration = SERVER ? 0 : 75
                    let delay = (i % 2 === 0) ? animationDuration*i : animationDuration*(i-1) 
                    return  (
                        <Timer key={`${searchResult.headline}${Math.random()}`} delay={delay}>
                            <Fade duration={animationDuration} startingOpacity={this.props.isInsert ? 1 : 0}>                            
                                <INavSearchResult {...searchResult} />
                            </Fade>
                        </Timer>
                    )
                })
            }
            </div>
            )
    }
  }


// Redux Connect
const mapStateToProps = (state) => {
    return {
        searchResults: state.csn_search.iNav.searchResults,
        isInsert: state['ui/INavSearchResultsContainer'] ? state['ui/INavSearchResultsContainer'].isInsert : false      
    }
}

const componentRootReducer = (initUIState) => (state = initUIState, action) => {
    switch (action.type) {
      case ActionTypes.INAV.ADD_PROMOTED_ARTICLE:
        return {
          ...state,
          isInsert: true
        }
      case ActionTypes.INAV.EMIT_NATIVE_ADS_EVENT:
        return {
          ...state,
          isInsert: false
        }
      default:
        return state
    }
  }


 
  
export default connect(
  mapStateToProps
)(UI({
  key: 'ui/INavSearchResultsContainer',  
  state:{
    isInsert: false, // If true then we should not animate the search results because we are insteting just a tile, 
  },
  reducer: componentRootReducer
})(INavSearchResults))