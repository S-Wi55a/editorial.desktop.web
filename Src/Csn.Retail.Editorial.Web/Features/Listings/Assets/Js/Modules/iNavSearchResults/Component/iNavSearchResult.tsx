import * as React from "react"
import { ISearchResults } from 'iNav/Types'
import Img from 'react-image'
import { CSSTransition }  from 'react-transition-group'


if (!SERVER) {
    require('iNavSearchResults/Css/iNavSearchResult.scss')
}

const Preloader = () => <div className="iNavSearchResult__image-loader"></div>

class INavSearchResult extends React.Component<ISearchResults> {
    render() {
        return <div className="iNavSearchResult">
                    <a href={this.props.articleDetailsUrl}>
                        <div className="iNavSearchResult__image-frame">
                            <Img className="iNavSearchResult__image" src={this.props.imageUrl} loader={<Preloader/>}/>
                            {this.props.label ? <div className={`iNavSearchResult__image-label iNavSearchResult__image-label--${this.props.label}`}>{this.props.label}</div> : ''}
                        </div>
                        <div className="iNavSearchResult__content-wrapper">
                            <div className="iNavSearchResult__heading">
                                {this.props.headline}
                            </div>
                            <div className="iNavSearchResult__date">
                                {this.props.dateAvailable}
                            </div>
                        </div>
                    </a>
                </div>
    }
}

export default INavSearchResult;