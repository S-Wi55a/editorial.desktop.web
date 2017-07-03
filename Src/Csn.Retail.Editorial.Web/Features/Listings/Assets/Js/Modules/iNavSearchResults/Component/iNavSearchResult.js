import React from 'react'

const INavSearchResult = ({Headline, Media, DateAvailable}) => (
    <div className="iNavSearchResult">
        <a href={'#'}>
            <div className="iNavSearchResult__image-frame">
                <img className="iNavSearchResult__image" src={'https://editorial.li.csnstatic.com/carsales'+Media.Photos[0].PhotoPath} />
            </div>
            <div className="iNavSearchResult__content-wrapper">
                <div className="iNavSearchResult__heading">
                    {Headline}
                </div>
                <div className="iNavSearchResult__date">
                    {'April 24th'}
                </div>
            </div>
        </a>
    </div>
)

export default INavSearchResult