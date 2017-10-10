import React from 'react'
import { connect } from 'react-redux'
import Timer from 'ReactAnimations/Timer'
import { Fade } from 'ReactAnimations/Fade'
import INavSearchResult from 'iNavSearchResults/Component/iNavSearchResult'

if (!SERVER) {
    require('iNavSearchResults/Css/iNavSearchResults.scss')
}
    
class INavSearchResults extends React.Component {
    render() {
      return (
        <div className="iNavSearchResults">        
            {
                this.props.searchResults.map((searchResult,i) => {
                    let animationDuration = 75
                    let delay = (i % 2 === 0) ? animationDuration*i : animationDuration*(i-1) 
                    return  (
                        <Timer key={`${searchResult.headline}${Math.random()}`} delay={delay}>
                            <Fade duration={animationDuration}>                            
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
        searchResults: state.iNav.searchResults        
    }
}

// Connect the Component to the store
const INavSearchResultsContainer = connect(
    mapStateToProps
)(INavSearchResults);

export default INavSearchResultsContainer;