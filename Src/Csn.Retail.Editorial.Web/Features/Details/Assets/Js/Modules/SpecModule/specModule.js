import React from 'react'
import * as Ajax from 'Ajax/ajax.js'
import Slider from 'rc-slider/lib/Slider'; 
import {disclaimerTemplate} from 'Modal/modal-disclaimer-view'
import {tabOrModal} from 'SpecModule/specModule--tabOrModal.js'

const SpecificationsItem_DD = (props) => {

    const iconClassName = props.item.title ? props.item.title.replace(/\s+/g, "-").replace(/\(|\)/g, "").toLowerCase() : '';

    return <dd className={'spec-item__spec-item-value ' + iconClassName} data-value={encodeURI(props.item.value)}>{props.item.value}</dd>
    
}

const SpecificationsItem_DT = (props) => {

    const iconClassName = props.item.title ? props.item.title.replace(/\s+/g, "-").replace(/\(|\)/g, "").toLowerCase() : '';

    return (
        <dt className={'spec-item__spec-item-title ' + iconClassName}>{props.item.title}</dt>
    )
} 

const Specifications = (props) => {

    return (
        <div>
            <h3 className="spec-item__spec-item-list-heading">{props.data.title}</h3>
            <dl className="spec-item__spec-item-list">
                {props.data.items.map((item) => {
                        return [<SpecificationsItem_DT item={item} key={item.title}/>, <SpecificationsItem_DD item={item} key={item.value}/>]
                    }
                )}
            </dl>
        </div>
        )
}

const ThirdPartyOffers = (props) => {
    var offers = [];

    props.data.map((item, index) => {
        offers.push(<ThirdPartyOffer data={item} disclaimerHandler={props.disclaimerHandler} key={index} tabOrModal={tabOrModal} index={index}/>);
    });

    return (<div>{ offers }</div>);
} 

const ThirdPartyOffer = (props) => {

    const isDisabled = props.data.amount === 'N/A' ? true : false
    const titleNoSpace = props.data.companyName ? props.data.companyName.replace(/\s+/g, "-").toLowerCase() : '';

    let tabOrModal = ''

    if (props.tabOrModal) {
        if (props.tabOrModal[titleNoSpace] === 'iframe') {
            tabOrModal = <span 
                onClick={() => {
                    props.disclaimerHandler(`<iframe src=${props.data.formUrl}></iframe>`, titleNoSpace)
                }} 
                className="third-party-offer__link" 
                data-webm-clickvalue={'get-quote-'+titleNoSpace}>{props.data.getQuoteText}</span>
        } else {
            tabOrModal = <a href={props.data.formUrl} target="_blank" className="third-party-offer__link" data-webm-clickvalue={'get-quote-'+titleNoSpace}>{props.data.getQuoteText}</a>
        }
    }

    return (
        <div className={'spec-item__third-party-offer third-party-offer third-party-offer--'+titleNoSpace + (isDisabled ? ' third-party-offer--disabled' : '')}>
            <img className="third-party-offer__logo" src={props.data.logoUrl} />
            <div className="third-party-offer__content">
                <h3 className="third-party-offer__heading">{props.data.title}</h3>
                <div className="third-party-offer__price-container">
                    <span className="third-party-offer__price">
                        {props.data.amount}
                    </span>
                    <span data-webm-clickvalue={'disclaimer-'+titleNoSpace} className="third-party-offer__price-term" onClick={()=>props.disclaimerHandler(props.data.disclaimer)}>
                        {props.data.paymentFrequency}
                    </span>
                </div>
            </div>
            {tabOrModal}
            <div className="third-party-offer__terms-and-conditions">{props.data.termsAndConditions}</div>
                { props.data.impressionUrl ? <img src={props.data.impressionUrl} className="third-party-offer__impression-url"/> : ''}
        </div>
        )
}

// KmsTag
const KmsTag = (props) => {
    var kmsTextSplit = props.kmsText.split("{kmsValue}")
    return (
        <div>{kmsTextSplit[0]}<span className="spec-item__kms-label" onClick={()=>props.disclaimerHandler(props.data.specDataDisclaimerText)}>{props.data.priceUsed.averageKms}</span>{kmsTextSplit[1]}</div>
    )
}

// Price
const Price = (props) => {
    if (props.data.priceNew) {
        return (
            <div className="spec-item__price-container">
                <div className="spec-item__price-item">
                    <p className="spec-item__price-label spec-item__price-disclaimer" onClick={()=>props.disclaimerHandler(props.data.priceNew.disclaimerText)}>{props.data.priceNew.disclaimerTitle}</p>
                    <p className="spec-item__price spec-item__price--price-new">{props.data.priceNew.price}</p>
                </div>
           </div>
        )
    } else if (props.data.priceUsed) {
        return (
            <div className="spec-item__price-container">
                <div className="spec-item__price-item">
                    <div className="spec-item__price-label">{props.data.priceUsed.heading}</div>
                    <div className="spec-item__price spec-item__price--price-used">{props.data.priceUsed.text}</div>
                    <div className="spec-item__price-label">
                        <KmsTag data={props.data} disclaimerHandler={props.disclaimerHandler} kmsText={props.data
                            .kmsTitle} />
                    </div>
                    <div className="spec-item__price-redbook-info" onClick={()=>props.disclaimerHandler(props.data.specDataDisclaimerText)}>{props.data.specDataProviderText}</div>
                </div>
            </div>
        )
    } else {
        return (<div className="spec-item__price-container"></div>)
    }
}

// StockOffer - Egull
const StockOffer = (props) => {
    const { stockUrl, stockCount, stockCountLabel } = props.data.specStockCountData ? props.data.specStockCountData : {};
    const stockLabel = stockCount === 0 ? "0 Car for sale" : stockCountLabel;
    const className = "stock-offer__for-sale" + (stockCount === 0 ? " stock-offer__for-sale--disabled" : "");

    return <a href={stockUrl} target="_self" className={className}>{ stockLabel }</a>;
}

//Content
const SpecModuleItem = (props) => {

    let marks = {
        0:'Price Min'
    }
    marks[props.sliderLength - 1] = 'Price Max'

    return (
        <div className="spec-item ">
            <div className="spec-item__column-container">
                <div className="spec-item__column spec-item__column--1">
                    <h2 className="spec-item__make">{props.data.title1}</h2>
                    <p className="spec-item__model">{props.data.title2}</p>
                    <p className="spec-item__variant">{props.data.title3}</p>
                    <Price data={props.data} disclaimerHandler={props.disclaimerHandler} />
                    {
                        props.sliderLength > 1 ?
                        [
                            <div className="spec-item__stock-offers" data-webm-clickvalue="stock-for-sale-btn" key={0}>
                                <StockOffer data={props.data} />
                            </div>,
                            <div className="spec-item__selector" data-webm-clickvalue="change-variant" key={1}>
                                <p className="spec-item__selector-label">Model Selector</p>
                                <Slider dots min={0} max={props.sliderLength - 1} marks={marks} onAfterChange={props.sliderOnAfterChangeHandler} onChange={props.sliderOnChangeHandler} />
                            </div>
                        ]
                        : ''}
                </div>
                <div className="spec-item__column spec-item__column--2">
                    <Specifications data={props.data.specItems} /> 
                </div>
            </div>
            <div className={props.isFetchingQuotes || !props.data.quotes ? "spec-item__third-party-offers loading" : "spec-item__third-party-offers "}>
                {props.data.quotes ? <ThirdPartyOffers data={props.data.quotes} disclaimerHandler={props.disclaimerHandler} /> : ' '}
            </div>
        </div>
    )
}

class SpecModule extends React.Component {
    // Look into which  es version I can use to ignore this
    constructor(props) {
        super(props);

        this.state = {
            items: [],
            activeItemIndex: 0,
            isFetchingQuotes: false,
            pendingRequests: 0,
            specVariantsQuery: this.props.data,
            fetchingVariants: false,
            sliderLength: 0
        };

        this.sliderOnAfterChangeHandler = this.sliderOnAfterChangeHandler;
        this.sliderOnChangeHandler = this.sliderOnChangeHandler;
        this.disclaimerHandler = this.disclaimerHandler;
    }

    componentDidMount() {
        this.getVariantsData(this.state.specVariantsQuery);
    }

    componentDidUpdate(prevProps, prevState) {
        // If all ajax request are complete then set state
        if (prevState.isFetchingQuotes === true && this.state.pendingRequests <= 0) {
            // Set State
            this.setState({
                isFetchingQuotes: false
            });
        }
    }

    sliderOnAfterChangeHandler = (index) => {
        if (this.state.items[index] && this.state.items[index].quotes) {
            this.setState({
                activeItemIndex: index
            });
        } else {
            const url = this.props.path + this.state.items[index].specQuotesUrl;
            this.getQuotesData(url, index);
        }
    }
    sliderOnChangeHandler = (index) => {
        this.setState({
            activeItemIndex: index
        });
    }

    getVariantsData = (specVariantsQuery) => {
        // Set State
        this.setState((prevState, props) => {
            return {
                fetchingVariants: true,
                pendingRequests: prevState.pendingRequests + 1
            };
        });
        const url = this.props.path + specVariantsQuery;
        Ajax.get(url, (data) => {
            data = JSON.parse(data);
            // Cache data
            this.setState((prevState, props) => {
                return {
                    items: (data && data.specDataVariants && data.specDataVariants.length) ? data.specDataVariants : [],
                    sliderLength: (data && data.specDataVariants && data.specDataVariants.length) ? data.specDataVariants.length : 0,
                    pendingRequests: prevState.pendingRequests - 1,
                    fetchingVariants: false
                };
            });
        });
    }

    // Ajax
    getQuotesData = (url, index) => {

        // Set State
        this.setState((prevState, props) => {
            return {
                isFetchingQuotes: true,
                activeItemIndex: index,
                pendingRequests: prevState.pendingRequests + 1
            };
        });
        Ajax.get(url, (data) => {
            data = JSON.parse(data);
            // Cache data
            const itemsCopy = this.state.items;
            itemsCopy[index].quotes = data.quotes;
            itemsCopy[index].specStockCountData = data.specStockCountData;

            this.setState((prevState, props) => {
                return {
                    pendingRequests: prevState.pendingRequests - 1,
                    items: itemsCopy
                };
            });
        });
    }

    // Data disclaimer handler
    disclaimerHandler = (content, className) => {

        this.props.modal.show(disclaimerTemplate(content), className)

    }

    render() {

        if (this.state.items.length <= 0 && !this.state.fetchingVariants) return null;
        return (
            <div className={this.state.fetchingVariants ? "spec-module loading" : "spec-module"}>
                {this.state.items[this.state.activeItemIndex] ?
                    <SpecModuleItem
                        data={this.state.items[this.state.activeItemIndex]}
                        disclaimerHandler={this.disclaimerHandler}
                        sliderLength={this.state.sliderLength}
                        sliderOnAfterChangeHandler={this.sliderOnAfterChangeHandler}
                        sliderOnChangeHandler={this.sliderOnChangeHandler}
                        isFetchingQuotes={this.state.isFetchingQuotes}
                    />
                    : ''}
            </div>
        );
    }
}

export default SpecModule;