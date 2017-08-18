import React from 'react'

const INavSearchResult = (props) => (
    <div className="iNavSearchResult">
        <a href={'#'}>
            <div className="iNavSearchResult__image-frame">
                <img className="iNavSearchResult__image" src={'https://editorial.li.csnstatic.com/carsales' + props.photoPath} />
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