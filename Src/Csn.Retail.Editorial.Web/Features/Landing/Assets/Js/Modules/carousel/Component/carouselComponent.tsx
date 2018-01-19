import React from 'react'
import { connect } from 'react-redux'
import { IState, ISimpleSlider, CarouselTypes } from 'carousel/Types'
import SearchResultCard from 'ReactComponents/SearchResultCard/searchResultCard'
import Slider from 'react-slick'
import { Thunks } from 'carousel/Actions/actions'
import CustomEvent from 'custom-event'
import NavButton from 'carousel/Component/carouselComponentArrows'

if (!SERVER) {
    require('Carousel/Css/carousel')
}

const DriverCard = (props: any) => <a href={props.itemUrl} data-webm-clickvalue={`item`}>
                                       <img src={props.imageUrl}/>
                                   </a>
                                 
class SimpleSlider extends React.Component<ISimpleSlider> {

    constructor(props: any) {
        super(props)
    }

    componentDidMount(): void {
        window.addEventListener('csn_editorial.nativeAds.ready', this.fetchNativeAds);
    }

    componentWillUnmount() {
        window.removeEventListener('csn_editorial.nativeAds.ready', this.fetchNativeAds);
    }

    fetchNativeAds = () => {
        if (this.props.polarAds !== null && !SERVER) {
            const customEvent = new CustomEvent('csn_editorial.landing.fetchNativeAds', { detail: {
                carouselId: this.props.index,
                placementId: this.props.polarAds.placementId
            } });
            window.dispatchEvent(customEvent);
        }
    }

    render() {
        const props = this.props
        const isShort = this.props.hasMrec || this.props.hasNativeAd 
        const settings = {
            infinite: false,
            speed: 500,
            slidesToShow: isShort ? 5 : 6,
            slidesToScroll: 1,
            arrows: true,
            responsive: [ 
                { breakpoint: 1200, settings: { slidesToShow: isShort ? 2 : 3 } },
                { breakpoint: 1600, settings: { slidesToShow: isShort ? 3 : 4 } },
                { breakpoint: 2000, settings: { slidesToShow: isShort ? 4 : 5 } }, 
            ],
            afterChange: function (newIndex: number) {                
                // Check if moving forward
                //if (newIndex > oldIndex) {
                    // Check if we are near the end 
                    if (newIndex >= props.carouselItems.length - this.slidesToShow - 2 && props.nextQuery !== null) {
                        //dispatch action
                        props.fetch(props.nextQuery, props.index)
                    }
                //}
            },
            nextArrow: <NavButton text="Next" />,
            prevArrow: <NavButton text="Prev" />

        }
        return (
            <Slider {...settings}>
                {this.props.carouselItems.map((item, index) => (
                    <div key={index}>
                        {this.props.carouselType !== CarouselTypes.Driver ? <SearchResultCard {...item}/> : <DriverCard {...item}/>}
                    </div>))}            
            </Slider>
        );
    }
}

// Redux Connect
const mapStateToProps = (state: IState, ownProps: any) => {
    return {
        carouselItems: state.carousels[ownProps.index] ? state.carousels[ownProps.index].carouselItems : [],
        hasMrec: state.carousels[ownProps.index] ? state.carousels[ownProps.index].hasMrec : false,
        hasNativeAd: state.carousels[ownProps.index] ? state.carousels[ownProps.index].hasNativeAd : false,
        polarAds: state.carousels[ownProps.index] ? state.carousels[ownProps.index].polarAds : null,
        nextQuery: state.carousels[ownProps.index] ? state.carousels[ownProps.index].nextQuery : '',
        carouselType: state.carousels[ownProps.index] ? state.carousels[ownProps.index].carouselType : CarouselTypes.Article
    }
}

const mapDispatchToProps = (dispatch: any) => {
    return {
        fetch: (query: string, index: number)=> {
            dispatch([
                Thunks.fetchCarouselResults(query, index)
            ]);
        }
    }
}

export default connect(
    mapStateToProps,
    mapDispatchToProps
)(SimpleSlider)