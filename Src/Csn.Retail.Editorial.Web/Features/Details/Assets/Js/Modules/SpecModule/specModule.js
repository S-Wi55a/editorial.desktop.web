import React from 'react'
import update from 'immutability-helper'
import * as Ajax from 'Js/Modules/Ajax/ajax.js'
import Slider, { Range } from 'rc-slider'
import Modal from 'Js/Modules/Modal/modal.js'
import * as View from 'Js/Modules/SpecModule/specModule-view.js'


window.csn_modal = window.csn_modal || new Modal()

const specPath = "/editorial/api/v1/spec/?uri=";
const GLOBAL_specModuleData = csn_editorial.specModule; //Set this to state

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
            <h3 className="spec-item__spec-item-list-heading">Overview</h3>
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
                    <span className="third-party-offer__price-term" data-disclaimer={encodeURI(props.data.disclaimer)} onClick={props.disclaimerHandler}>
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
                    <span className="third-party-offer__price-term" data-disclaimer={encodeURI(props.data.disclaimer)} onClick={props.disclaimerHandler}>
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
                <h2 className="spec-item__make">{props.data.title}</h2>
                <p className="spec-item__model">{props.data.description}</p>
                <p className="spec-item__variant">{props.data.description}</p>
                {/* Price */}
                <div className="spec-item__selector">
                    <Slider dots min={0} max={props.sliderLength} onChange={props.sliderHandler} />
                </div>
            </div>
            <div className="spec-item__column spec-item__column--2">
                <Specifications data={props.data.items} /> 
            </div>
            <div className="spec-item__third-party-offers">
                {props.data.strattonData ? <Stratton data={props.data.strattonData} disclaimerHandler={props.disclaimerHandler}/> : ''}
                {props.data.budgetDirectData ? <BudgetDirect data={props.data.budgetDirectData} disclaimerHandler={props.disclaimerHandler}/> : ''}
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
            activeItemIndex: 0,
            isFetching: false
        };

        this.sliderHandler = this.sliderHandler;
        this.ajaxHandler = this.ajaxHandler;
        this.disclaimerHandler = this.disclaimerHandler;

    }

    componentDidMount() {
        this.sliderHandler(this.state.activeItemIndex)
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

        // Set State
        this.setState({
            isFetching: true
        });

        Ajax.get(url, (data) => {
            data = JSON.parse(data);

            // Cache data
            const newState = update(this.state.items, { $splice: [[index, 1, data]] });

            // Set State
            this.setState({
                items: newState,
                activeItemIndex: index,
                isFetching: false
            });

        });
    }

    // Data disclaimer handler
    disclaimerHandler = (e) => {
     
        const content = decodeURI(e.target.getAttribute('data-disclaimer'))
        window.csn_modal.show(View.disclaimer(content))

    }


    render() {

        return (
            <div className={this.state.isFetching ? "spec-module loading" : "spec-module"}>
                {this.state.items[this.state.activeItemIndex] ?
                    <SpecModuleItem
                        data={this.state.items[this.state.activeItemIndex]}
                        disclaimerHandler={this.disclaimerHandler}
                        sliderLength={this.state.urls.length}
                        sliderHandler={this.sliderHandler} />
                    : ''}
            </div>
        );
    }
}

export default SpecModule;