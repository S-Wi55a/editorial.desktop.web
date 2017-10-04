import * as React from "react"

if (!SERVER) {
    require('iNavSearchResults/Css/iNavSearchResult.scss')
}

interface IINavSearchResultProps {
    imageUrl: string,
    headline: string,
    dateAvailable: string,
    articleDetailsUrl: string
}

const INavSearchResult = ({ imageUrl, headline, dateAvailable, articleDetailsUrl }: IINavSearchResultProps) => (
    <div className="iNavSearchResult">
        <a href={articleDetailsUrl}>
            <div className="iNavSearchResult__image-frame">
                <img className="iNavSearchResult__image" src={imageUrl} />
            </div>
            <div className="iNavSearchResult__content-wrapper">
                <div className="iNavSearchResult__heading">
                    {headline}
                </div>
                <div className="iNavSearchResult__date">
                    {dateAvailable}
                </div>
            </div>
        </a>
    </div>
);

export default INavSearchResult;