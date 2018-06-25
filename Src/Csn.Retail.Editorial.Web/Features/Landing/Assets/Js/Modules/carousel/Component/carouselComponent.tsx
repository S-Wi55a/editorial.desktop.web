import React from 'react'
import { connect } from 'react-redux'
import { IState, ISimpleSlider, CarouselTypes, ICarouselViewModel } from 'carousel/Types'
import SearchResultCard from 'ReactComponents/SearchResultCard/searchResultCard'
import Slider from 'react-slick'
import { Thunks } from 'carousel/Actions/actions'
import CustomEvent from 'custom-event'
import NavButton from 'carousel/Component/carouselComponentArrows'
import UI from 'ReactReduxUI'
import { Actions, ActionTypes } from 'carousel/Actions/actions'


if (!SERVER) {
    require('Carousel/Css/carousel')
}

const DriverCard = (props: any) => <a href={props.articleDetailsUrl} data-webm-clickvalue={`item`}>
                                       <img src={props.imageUrl}/>
                                   </a>
                                 
class SimpleSlider extends React.Component<ISimpleSlider> {

    private disqusId: string

    constructor(props: any) {
        super(props)
        this.disqusId = 'dsq-count-scr'
    }

    componentDidMount(): void {
        window.addEventListener('csn_editorial.nativeAds.ready', this.fetchNativeAds);
        this._addDisqusScript()
        this._resetComments()
    }

    componentWillUnmount() {
        window.removeEventListener('csn_editorial.nativeAds.ready', this.fetchNativeAds);
    }

    componentDidUpdate () {
        this._resetComments()
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

    _resetComments () {        
        if (typeof DISQUSWIDGETS !== 'undefined') {
            DISQUSWIDGETS.getCount({ reset: true })
        }
    }

    _addDisqusScript () {
        if (SERVER || document.getElementById(this.disqusId)) {
            return
        }

        const parent = document.getElementsByTagName('body')[0]

        const script = document.createElement('script')
        script.async = true
        script.id = this.disqusId
        script.type = 'text/javascript'
        script.src = '//' + this.props.shortname + '.disqus.com/count.js'
        parent.appendChild(script)

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
                    if (newIndex >= props.carouselItems.length - this.slidesToShow - 2 && props.nextQuery !== "") {
                        //dispatch action
                        props.fetch(props.nextQuery, props.index)
                    }
                //}
            },
            nextArrow: <NavButton text="Next" />,
            prevArrow: <NavButton text="Prev" />

        }
        return (
            <Slider {...settings} className={this.props.isLoading ? 'isLoading' : ''}>
                {this.props.carouselItems.map((item, index) => (
                    <div key={index}>
                        {this.props.carouselType !== CarouselTypes.Driver ? <SearchResultCard imageUrlParams='?width=405&height=270' {...item}/> : <DriverCard {...item}/>}
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
        carouselType: state.carousels[ownProps.index] ? state.carousels[ownProps.index].carouselType : CarouselTypes.Article,
        shortname: typeof state.store !== 'undefined' ? state.store.nav.disqusSource : ''
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

const componentRootReducer = (initUIState: any) => (state: any = initUIState, action: Actions): any => {
    switch (action.type) {
        case ActionTypes.API.CAROUSEL.FETCH_QUERY_REQUEST:
            if (action.payload.index === state.id) {
                return {
                    ...state,
                    isLoading: true
                }
            } else {
                return state
            }
    case ActionTypes.API.CAROUSEL.FETCH_QUERY_SUCCESS:
    case ActionTypes.API.CAROUSEL.FETCH_QUERY_FAILURE:
        if (action.payload.index === state.id) {
            return {
                ...state,
                isLoading: false
            }
        } else {
            return state
        }
    default:
        return state
    }
}

export default connect(
    mapStateToProps,
    mapDispatchToProps
)(UI({
    key: (props: ICarouselViewModel)=>`ui/carousel_${props.category}`,
    reducer: componentRootReducer,
    state: (props: ICarouselViewModel)=>({
        id: props.index,
        isLoading: false
})
})(SimpleSlider))