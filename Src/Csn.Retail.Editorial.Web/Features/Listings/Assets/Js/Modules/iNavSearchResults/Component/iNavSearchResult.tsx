import * as React from "react"
import { ISearchResults } from 'iNav/Types'
import Img from 'react-image'

if (!SERVER) {
    require('iNavSearchResults/Css/iNavSearchResult.scss')
}

const Preloader = () => <div className="iNavSearchResult__image-loader"></div>

const INavSearchResult = (props: ISearchResults) => (
    <div className="iNavSearchResult">
        <a href={props.articleDetailsUrl}>
            <div className="iNavSearchResult__image-frame">
                <Img className="iNavSearchResult__image" src={props.imageUrl} loader={<Preloader/>}/>
                {props.label ? <div className={`iNavSearchResult__image-label iNavSearchResult__image-label--${props.label}`}>{props.label}</div> : ''}
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