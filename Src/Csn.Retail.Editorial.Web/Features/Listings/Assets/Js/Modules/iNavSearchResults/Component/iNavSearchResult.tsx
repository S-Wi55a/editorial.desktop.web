import * as React from "react";

interface IINavSearchResult {
    imageUrl: string,
    headline: string,
    dateAvailable: string
}

const INavSearchResult = ({imageUrl, headline, dateAvailable}: IINavSearchResult) => (
    <div className="iNavSearchResult">
        <a href={'#'}>
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