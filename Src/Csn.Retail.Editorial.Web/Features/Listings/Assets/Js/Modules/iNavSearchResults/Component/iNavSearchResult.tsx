import * as React from "react"
import { ISearchResults } from 'iNav/Types'

if (!SERVER) {
    require('iNavSearchResults/Css/iNavSearchResult.scss')
}

const INavSearchResult = (props: ISearchResults) => (

        <a href={props.articleDetailsUrl} className="iNavSearchResult">
            <div className="iNavSearchResult__image-frame">
                <img className="iNavSearchResult__image" src={props.imageUrl} />
                {props.label ? <div className={`iNavSearchResult__image-label iNavSearchResult__image-label--${props.label}`}>{props.label}</div> : ''}
            </div>
            <div className="iNavSearchResult__content-wrapper">
                <div className={`iNavSearchResult__type iNavSearchResult__type--${props.type.toLowerCase()}`}>
                    {props.type.toUpperCase()}
                </div>
                <div className="iNavSearchResult__heading">
                    <h2>{props.headline}</h2>
                </div>
                <div className="iNavSearchResult__sub-heading">
                    <span>{props.subHeading}</span>
                </div>
                <div className="iNavSearchResult__date">
                    {props.dateAvailable}
                </div>
                <div
                    className="disqus-comment-count iNavSearchResult__comment-count"
                    data-disqus-identifier={props.disqusArticleId}
                    data-disqus-url={props.articleDetailsUrl}
                >
                </div>
            </div>
        </a>

);

export default INavSearchResult;