import * as React from "react"
import { ISearchResults } from 'ReactComponents/iNav/Types'
import Img from 'react-image'

if (!SERVER) {
    require('./searchResultCard.scss')
}

const Preloader = () => <div className="iNavSearchResult__image-loader"></div>

class INavSearchResult extends React.Component<ISearchResults> {
    render() {
        return <div className="iNavSearchResult" data-webm-clickvalue="search-result">
                    <a href={this.props.articleDetailsUrl}>
                        <div className="iNavSearchResult__image-frame">
                            <Img className="iNavSearchResult__image" src={this.props.imageUrl} loader={<Preloader/>}/>
                            {this.props.label ? <div className={`iNavSearchResult__image-label iNavSearchResult__image-label--${this.props.label}`}>{this.props.label}</div> : ''}
                        </div>
                    </a>
                        <div className="iNavSearchResult__content-wrapper">
                            <a href={this.props.articleDetailsUrl}>
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
                            </a>
                            <a className="disqus-comment-count iNavSearchResult__comment-count"
                                 data-disqus-identifier={this.props.disqusArticleId}
                                 data-disqus-url={this.props.articleDetailsUrl}
                                 data-webm-clickvalue="comments"
                                 href={`${this.props.articleDetailsUrl}#disqus_thread`}
                            >0
                            </a>
                        </div>
                </div>
    }
}

export default INavSearchResult;