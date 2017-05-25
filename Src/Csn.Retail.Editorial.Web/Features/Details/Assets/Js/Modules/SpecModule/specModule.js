﻿import React from 'react'
import update from 'immutability-helper'
import * as Ajax from 'Js/Modules/Ajax/ajax.js'
import Slider, { Range } from 'rc-slider'
import * as View from 'Js/Modules/SpecModule/specModule--view.js'
import {tabOrModal} from 'Js/Modules/SpecModule/specModule--tabOrModal.js'

const SpecificationsItem_DD = (props) => {

    const iconClassName = props.item.title ? props.item.title.replace(/\s+/g, "-").replace(/\(|\)/g, "").toLowerCase() : '';

    if (props.last) {
        return (
            <dd className={'spec-item__spec-item-value ' + iconClassName} data-value={props.item.value}>{props.item.value}</dd>
        )
    } else {
        return (
            <dd className={'spec-item__spec-item-value ' + iconClassName}>{props.item.value}</dd>
        )
    }

}

const SpecificationsItem_DT = (props) => {

    const iconClassName = props.item.title ? props.item.title.replace(/\s+/g, "-").replace(/\(|\)/g, "").toLowerCase() : '';

    return (
        <dt className={'spec-item__spec-item-title ' + iconClassName}>{props.item.title}</dt>
    )
} 

const Specifications = (props) => {
    const listLength = props.data.items.length;

    return (
        <div>
            <h3 className="spec-item__spec-item-list-heading">{props.data.title}</h3>
            <dl className="spec-item__spec-item-list">
                {props.data.items.map((item, index) => {
                        if (index === listLength - 1) {
                            return [<SpecificationsItem_DT item={item} />, <SpecificationsItem_DD item={item} last={true} />]
                        } else {
                            return [<SpecificationsItem_DT item={item} />, <SpecificationsItem_DD item={item} />]
                        }
                    }
                )}
            </dl>
        </div>
        )
}

const ThirdPartyOffers = (props) => {
    var offers = [];

    props.data.map((item, index) => {
        offers.push(<ThirdPartyOffer data={item} disclaimerHandler={props.disclaimerHandler} key={index} tabOrModal={tabOrModal}/>);
    });

    return (<div>{ offers }</div>);
} 

const ThirdPartyOffer = (props) => {

    const titleNoSpace = props.data.companyName ? props.data.companyName.replace(/\s+/g, "-").toLowerCase() : '';

    let tabOrModal = ''

    if (props.tabOrModal) {

        if (props.tabOrModal[titleNoSpace] === 'iframe') {
            const iframe = `<iframe src=${props.data.formUrl}"></iframe>`
            tabOrModal = <span data-disclaimer={encodeURI(iframe)} onClick={(p,e) => {
                props.disclaimerHandler(titleNoSpace, p, e)
            }} className="third-party-offer__link">{props.data.getQuoteText}</span>
        } else {
            tabOrModal = <a href={props.data.formUrl} target="_blank" className="third-party-offer__link">{props.data.getQuoteText}</a>
        }

    }

    return (
        <div className={'spec-item__third-party-offer third-party-offer third-party-offer--'+titleNoSpace}>
            <img className="third-party-offer__logo" src={props.data.logoUrl} />
            <div className="third-party-offer__content">
                <h3 className="third-party-offer__heading">{props.data.title}</h3>
                <div className="third-party-offer__price-container">
                    <span className="third-party-offer__price">
                        {props.data.amount}
                    </span>
                    <span className="third-party-offer__price-term" data-disclaimer={encodeURI(props.data.disclaimer)} onClick={props.disclaimerHandler}>
                        {props.data.paymentFrequency}
                    </span>
                </div>
            </div>
            {tabOrModal}
            <div className="third-party-offer__terms-and-conditions">{props.data.termsAndConditions}</div>
        </div>
        )
} 

// Price
const Price = (props) => {
    if (props.data.priceNew) {
        return (
            <div>
                <p className="spec-item__price spec-item__price--price-new">{props.data.priceNew.price}</p>
                <p className="spec-item__price-disclaimer" data-disclaimer={encodeURI(props.data.priceNew.disclaimerText)} onClick={props.disclaimerHandler}>{props.data.priceNew.disclaimerTitle}</p>
           </div>
        )
    } else {

        return (
            <div className="spec-item__price-container">
                <div className="spec-item__price-item">
                    <div className="spec-item__price-label">{props.data.pricePrivate.heading}</div>
                    <div className="spec-item__price spec-item__price--price-private">{props.data.pricePrivate.text}</div>

                </div>
                <div className="spec-item__price-item">
                    <div className="spec-item__price-label">{props.data.priceTradeIn.heading}</div>
                    <div className="spec-item__price spec-item__price--price-trade-in">{props.data.priceTradeIn.text}</div>
                </div>
            </div>
        )
    } 
}

//Content
const SpecModuleItem = (props) => {
    return (
        <div className="spec-item ">
            <div className="spec-item__column-container">
            <div className="spec-item__column spec-item__column--1">
                <h2 className="spec-item__make">{props.data.title1}</h2>
                <p className="spec-item__model">{props.data.title2}</p>
                <p className="spec-item__variant">{props.data.title3}</p>
                <Price data={props.data} disclaimerHandler={props.disclaimerHandler} />
                <div className="spec-item__selector">
                    <p className="spec-item__selector-label"></p>
                    <Slider dots min={0} max={props.sliderLength - 1} onChange={props.sliderHandler} />
                </div>
            </div>
            <div className="spec-item__column spec-item__column--2">
                <Specifications data={props.data.specItems} /> 
            </div>
                </div>
            <div className="spec-item__third-party-offers">
                {props.data.quotes ? <ThirdPartyOffers data={props.data.quotes} disclaimerHandler={props.disclaimerHandler}/> : ''}
            </div>
        </div>
    )
}

class SpecModule extends React.Component {
    // Look into which  es version I can use to ignore this
    constructor(props) {
        super(props);

        this.state = {
            urls: this.props.data.items.slice(),
            items: new Array(this.props.data.items.length),
            activeItemIndex: 0,
            pendingIndex: 0,
            isFetching: false,
            pendingRequests: 0
        };

        this.sliderHandler = this.sliderHandler;
        this.ajaxHandler = this.ajaxHandler;
        this.disclaimerHandler = this.disclaimerHandler;

    }

    componentDidMount() {
        this.sliderHandler(this.state.activeItemIndex);
    }

    componentDidUpdate(prevProps, prevState) {
        // If all ajax request are complete then set state
        if (prevState.isFetching === true && this.state.pendingRequests <= 0) {
            // Set State
            this.setState({
                activeItemIndex: this.state.pendingIndex,
                isFetching: false
            });
        } 
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
            const url = this.props.path + this.state.urls[index].uri;
            this.ajaxHandler(url, index);
        }       
    }

    // Ajax
    ajaxHandler = (url, index) => {
        // Set State
        this.setState((prevState, props) => {
            return {
                isFetching: true,
                pendingRequests: prevState.pendingRequests + 1
            };
        });

        Ajax.get(url, (data) => {
            data = JSON.parse(data);

            // Cache data
            const newState = update(this.state.items, { $splice: [[index, 1, data]] });

            this.setState((prevState, props) => {
                return {
                    items: newState,
                    pendingRequests: prevState.pendingRequests - 1,
                    pendingIndex: index
                };
            });
        });
    }

    // Data disclaimer handler
    disclaimerHandler = (...args) => {

        const synthEvt = args[args.length - 2]
        const className = args[args.length - 3]

        const content = decodeURI(synthEvt.target.getAttribute('data-disclaimer'))
        this.props.modal.show(View.disclaimer(content), className)

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