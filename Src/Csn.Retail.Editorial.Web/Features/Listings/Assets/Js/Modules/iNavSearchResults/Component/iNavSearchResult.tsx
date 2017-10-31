import * as React from "react"
import { ISearchResults } from 'iNav/Types'
import Img from 'react-image'

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
                            <div className={`iNavSearchResult__type iNavSearchResult__type--${typeof this.props.type !== 'undefined' ? this.props.type.toLowerCase() : ''}`}>
                                {typeof this.props.type !== 'undefined' ? this.props.type.toUpperCase() : ''}
                            </div>
                            <div className="iNavSearchResult__heading">
                                <h2>{this.props.headline}</h2>
                            </div>
                            <div className="iNavSearchResult__sub-heading">
                                <span>{this.props.subHeading}</span>
                            </div>
                            <div className="iNavSearchResult__date">
                                {this.props.dateAvailable}
                            </div>
                            <div
                                className="disqus-comment-count iNavSearchResult__comment-count"
                                data-disqus-identifier={this.props.disqusArticleId}
                                data-disqus-url={this.props.articleDetailsUrl}
                            >
                            </div>
                        </div>
                    </a>
                </div>
    }
}

export default INavSearchResult;