import React from 'react';
import update from 'immutability-helper';
import * as Ajax from 'Js/Modules/Ajax/ajax.js'
import Slider, { Range } from 'rc-slider';

const specPath = "/editorial/api/v1/spec/?uri=";
const GLOBAL_specModuleData = csn_editorial.specModule; //Set this to state


const SpecModuleSlider = (props) => {
    return (
        <div>
            {props.data.map((item, index) => 
                <div key={index}
                    className="slideshow__slide swiper-slide swiper-pagination-bullet"
                    onClick={(e) => { props.clickHandler(e, index, (specPath + item.uri)) }} />
            )}
        </div>
    )
}

const SpecificationsItem_DD = (props) => {
    return (
        <dd className="spec-item__spec-item-value" data-value="{props.item.value}">{props.item.value}</dd>
    )
}

const SpecificationsItem_DT = (props) => {
    return (
        <dt className="spec-item__spec-item-title">{props.item.title}</dt>
    )
} 
// TODO: Get specfication title added to Jira
const Specifications = (props) => {
    return (
        <div>
            <h3 className="spec-item__spec-item-list-heading">Specification</h3>
            <dl className="spec-item__spec-item-list">
                {props.data.map((item, index) =>
                    [<SpecificationsItem_DT item={item} />, <SpecificationsItem_DD item={item} />]
                )}
            </dl>
        </div>
        )
} 

// Budjest Direct
const BudgetDirect = (props) => {
    return (
        <div className="spec-item__third-party-offer spec-item__third-party-offer--stratton third-party-offer">
            <img className="third-party-offer__logo" src={props.data.logoUrl} />
            <div className="third-party-offer__content">
                <h3 className="third-party-offer__heading">{props.data.headings.title}</h3>
                <div className="third-party-offer__price-container">
                    <span className="third-party-offer__price">
                        {props.data.annualCost}
                    </span>
                    <span className="third-party-offer__price-term" data-disclaimer={encodeURI(props.data.disclaimer)}>
                        {props.data.headings.frequentPayment}
                    </span>
                </div>
            </div>
            <a href={props.data.formUrl} className="third-party-offer__link">{props.data.headings.getQuote}</a>
            <div className="third-party-offer__terms-and-conditions">{props.data.termCondition}</div>
        </div>
        )
} 

// Stratton
const Stratton = (props) => {
    return (
        <div className="spec-item__third-party-offer spec-item__third-party-offer--stratton third-party-offer">
            <img className= "third-party-offer__logo" src={props.data.logoUrl} />
            <div className="third-party-offer__content">
                <h3 className="third-party-offer__heading">{props.data.headings.title}</h3>
                <div className="third-party-offer__price-container">
                    <span className="third-party-offer__price">
                        {props.data.monthlyRepayments}
                    </span>
                    <span className="third-party-offer__price-term" data-disclaimer={encodeURI(props.data.disclaimer)}>
                        {props.data.headings.frequentPayment}
                    </span>
                </div>
            </div>
            <a href={props.data.formUrl} className="third-party-offer__link">{props.data.headings.getQuote}</a>
        </div>
        )
} 

//Content
const SpecModuleItem = (props) => {
    return (
        <div className="spec-item">
            <div className="spec-item__column spec-item__column--1">
                <h2 className="spec-item__heading">{props.data.title}</h2>
                <p className="spec-item__subheading">{props.data.description}</p>
                {/*Insert Price component*/}
                <div className="spec-item__image-container">
                    {props.data.image ? <img className="spec-item__image" src={props.data.image.url} alt={props.data.image.alternateText} /> : ''}
                </div>
                <div className="spec-item__third-party-offers">
                    {props.data.strattonData ? <Stratton data={props.data.strattonData} /> : ''}
                    {props.data.budgetDirectData ? <BudgetDirect data={props.data.budgetDirectData} /> : ''}
                </div>
            </div>
            <div className="spec-item__column spec-item__column--2">
                <Specifications data={props.data.items} /> 
            </div>
        </div>
    )
}

class SpecModule extends React.Component {
    // Look into which  es version I can use to ignore this
    constructor(props) {
        super(props);

        this.state = {
            urls: GLOBAL_specModuleData.items.slice(),
            items: new Array(GLOBAL_specModuleData.items.length),
            activeItemIndex: 0
        };

        this.sliderHandler = this.sliderHandler;
        this.ajaxHandler = this.ajaxHandler;

    }

    componentDidMount() {
        const url = specPath + this.state.urls[this.state.activeItemIndex].uri;
        this.ajaxHandler(url, this.state.activeItemIndex);

    }

    sliderHandler = (index) => {
        // Check if state.items has value
        // true: return data
        if (typeof this.state.items[index] !== 'undefined') {
            //console.log('using cached data')

            this.setState({
                activeItemIndex: index
            });
        } else {
            //console.log('not using cached data')
            const url = specPath + this.state.urls[index].uri
            this.ajaxHandler(url, index);
        }       
    }

    // Ajax
    ajaxHandler = (url, index) => {
        Ajax.get(url, (data) => {
            data = JSON.parse(data);

            // Cache data
            const newState = update(this.state.items, { $splice: [[index, 1, data]] });

            // Set State
            this.setState({
                items: newState,
                activeItemIndex: index
            });

        });
    }


    render() {

        return (
            <div className="spec-module">
                <div className="slideshow slideshow--spec-module">
                    <div className="slideshow__container swiper-container">

                        <div className="slideshow__slides swiper-wrapper">
                            {this.state.items[this.state.activeItemIndex] ? <SpecModuleItem data={this.state.items[this.state.activeItemIndex]} /> : '' }
                        </div>
                        <div className="slideshow__nav-wrapper">
                            <div className="slideshow__pagination-label slideshow__pagination-label--min"></div>
                            <div className="slideshow__pagination-wrapper">
                                <div className="slideshow__pagination swiper-pagination">
                                    <Slider dots min={0} max={this.state.urls.length} onAfterChange={this.sliderHandler}/>
                                </div>
                            </div>
                            <div className="slideshow__pagination-label slideshow__pagination-label--max"></div>
                        </div>
                    </div>
                </div>
            </div>
        );
    }
}

export default SpecModule;