import * as React from "react"
import { ISearchResults } from 'iNav/Types'

if (!SERVER) {
    require('iNavSearchResults/Css/iNavSearchResult.scss')
}

const INavSearchResult = (props: ISearchResults) => (
    <div className="iNavSearchResult">
        <a href={props.articleDetailsUrl}>
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
            </div>
        </a>
    </div>
);

export default INavSearchResult;