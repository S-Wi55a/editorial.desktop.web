import React from 'react'
import { connect } from 'react-redux'
import ReactCSSTransitionGroup from 'react-addons-css-transition-group';
import INavSearchResult from 'iNavSearchResults/Component/iNavSearchResult'

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
        return <div className="iNavSearchResults">        
            <ReactCSSTransitionGroup
                transitionName="iNavSearchResultsTransition"
                transitionEnterTimeout={300}
                transitionLeaveTimeout={300}>
                {
                    this.props.searchResults.map((searchResult, index) => {
                        return <INavSearchResult key={index} {...searchResult} />;
                    })
                }
            </ReactCSSTransitionGroup>
        </div>
    }
    
} 
// Redux Connect
const mapStateToProps = (state) => {
    return {
        searchResults: state.store.listings.navResults.searchResults,
        shortname: state.store.listings.disqusSource 
    }
}

// Connect the Component to the store
const INavSearchResultsContainer = connect(
    mapStateToProps
)(INavSearchResults);

export default INavSearchResultsContainer;