import React from 'react'
import { connect } from 'react-redux'
import Timer from 'ReactAnimations/Timer'
import { FadeIn } from 'ReactAnimations/Fade'
import INavSearchResult from 'ReactComponents/SearchResultCard/searchResultCard'
import UI from 'ReactReduxUI'
import { ActionTypes } from 'ReactComponents/iNav/Actions/actions'

if (!SERVER) {
    require('iNavSearchResults/Css/iNavSearchResults.scss')
}
class INavSearchResults extends React.Component {

    constructor() {
        super()
        this.disqusId = 'dsq-count-scr'
    }

    componentDidMount () {
        this._addDisqusScript()
        this._resetComments()
        this._injectTeads()
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
        if (SERVER || document.getElementById(this.disqusId)) {
            return
        }

        const parent = document.getElementsByTagName('body')[0]

        const script = document.createElement('script')
        script.async = true
        script.id = this.disqusId
        script.type = 'text/javascript'
        script.src = '//' + this.props.shortname + '.disqus.com/count.js'
        parent.appendChild(script)

    }

    _injectTeads () {
        if (SERVER) {
            return
        }
        const tile = document.querySelector('#Tile8')
        if (tile) {
            let el = document.querySelectorAll('.iNavSearchResults > span');
            el = (el.length >= 6) ? el[5] : undefined;
            if(el){
                document.querySelector('#teads-video-container').appendChild(tile)
            }        
        }
    }

    render() {

        const length = this.props.searchResults.length

        let r = this.props.searchResults.map((searchResult,i) => {
            let animationDuration = SERVER ? 0 : 150
            let delay = (i % 2 === 0) ? animationDuration*i : animationDuration*(i-1) 
            return <Timer key={`${searchResult.headline}${Math.random()}`} delay={delay}>
                        <FadeIn duration={animationDuration} startingOpacity={this.props.isInsert ? 1 : 0}>                            
                            <INavSearchResult imageWidth={405} height={270} {...searchResult} />
                        </FadeIn>
                    </Timer>
        })  
        const t = <div key="teads" id="teads-video-container" style={{clear:'both'}}></div>

        if(length >= 6){r.splice(6,0, t )}

        return <div className="iNavSearchResults">{r}</div>
    }
} 
// Redux Connect
const mapStateToProps = (state) => {
    return {
        searchResults: state.store.nav.navResults.searchResults,
        isInsert: state['ui/INavSearchResultsContainer'] ? state['ui/INavSearchResultsContainer'].isInsert : false,            
        shortname: state.store.nav.disqusSource 
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