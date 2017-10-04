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

            </div>
            <div className="iNavSearchResult__content-wrapper">
                <div className="iNavSearchResult__heading">
                    {props.headline}
                </div>
                <div className="iNavSearchResult__date">
                    {props.dateAvailable}
                </div>
            </div>
        </a>
    </div>
);

export default INavSearchResult;