import React from 'react'
import { connect } from 'react-redux'
import Timer from 'ReactAnimations/Timer'
import { FadeIn } from 'ReactAnimations/Fade'
import INavSearchResult from 'iNavSearchResults/Component/iNavSearchResult'
import UI from 'ReactReduxUI'
import { ActionTypes } from 'iNav/Actions/actions'

if (!SERVER) {
    require('iNavSearchResults/Css/iNavSearchResults.scss')
}
class INavSearchResults extends React.Component {

    constructor() {
        super()
        this.scriptAdded = false
        this.disqusId = 'dsq-count-scr'
    }

    componentDidMount () {
        this._addDisqusScript()
        this._resetComments()
      }
    
    componentDidUpdate () {
        this._resetComments()
    }

    componentWillUnmount() {
        if (SERVER) {
            return
        }
        const parent = document.getElementsByTagName('body')[0]
        parent.removeChild(document.getElementById(this.disqusId))
        // count.js only reassigns this window object if it's undefined.
        DISQUSWIDGETS = undefined;
        
    }
    
    _resetComments () {        
        if (typeof DISQUSWIDGETS !== 'undefined') {
            DISQUSWIDGETS.getCount({ reset: true })
        }
    }

    _addDisqusScript () {
        if (SERVER || this.scriptAdded) {
            return
        }

        const parent = document.getElementsByTagName('body')[0]

        const script = document.createElement('script')
        script.async = true
        script.id = this.disqusId
        script.type = 'text/javascript'
        script.src = '//' + this.props.shortname + '.disqus.com/count.js'
        parent.appendChild(script)

        this.scriptAdded = true
    }

    render() {
        return <div className="iNavSearchResults" data-webm-section="search-results">        
                {
                    this.props.searchResults.map((searchResult,i) => {

                      //TODO: add Teads component
                        let animationDuration = SERVER ? 0 : 150
                        let delay = (i % 2 === 0) ? animationDuration*i : animationDuration*(i-1) 
                        return  (
                            <Timer key={`${searchResult.headline}${Math.random()}`} delay={delay}>
                                <FadeIn duration={animationDuration} startingOpacity={this.props.isInsert ? 1 : 0}>                            
                                    <INavSearchResult {...searchResult} />
                                </FadeIn>
                            </Timer>
                        )
                    })
                }
                </div>
    }
} 
// Redux Connect
const mapStateToProps = (state) => {
    return {
        searchResults: state.store.listings.navResults.searchResults,
        isInsert: state['ui/INavSearchResultsContainer'] ? state['ui/INavSearchResultsContainer'].isInsert : false,            
        shortname: state.store.listings.disqusSource 
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