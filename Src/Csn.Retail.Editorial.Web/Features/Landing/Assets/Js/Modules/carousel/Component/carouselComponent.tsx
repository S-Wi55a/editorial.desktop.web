import React from 'react'
import { connect } from 'react-redux'
import { IState, ICarouselItems } from 'carousel/Types'
import SearchResultCard from 'Components/SearchResultCard/searchResultCard'
import Slider from 'react-slick'
import { Thunks } from 'carousel/Actions/actions'

if (!SERVER) {
    require('Carousel/Css/carousel')
}

interface ISimpleSlider {
    carouselItems: ICarouselItems[]
    hasMrec: boolean
    nextQuery: string
    index: number
    fetch: (q:string, i:number) => any
    test: number
}
class SimpleSlider extends React.Component<ISimpleSlider> {

    constructor(props: any) {
        super(props)
    }

    render() {
        const props = this.props 
        const settings = {
            infinite: false,
            speed: 500,
            slidesToShow: this.props.hasMrec ? 5 : 6,
            slidesToScroll: 1,
            arrows: true,
            responsive: [ 
                { breakpoint: 1200, settings: { slidesToShow: this.props.hasMrec ? 2 : 3 } },
                { breakpoint: 1600, settings: { slidesToShow: this.props.hasMrec ? 3 : 4 } },
                { breakpoint: 2000, settings: { slidesToShow: this.props.hasMrec ? 4 : 5 } }, 
            ],
            beforeChange: function (oldIndex: number, newIndex: number) {                
                // Check if moving forward
                if (newIndex > oldIndex) {
                    // Check if we are near the end 
                    if (newIndex >= props.carouselItems.length - this.slidesToShow - 2) {
                        //dispatch action
                        props.fetch(props.nextQuery, props.index)
                    }
                }
            }
        }
        return (
            <Slider {...settings}>
                {this.props.carouselItems.map((item, index) => <div key={index}><SearchResultCard {...item}/></div>)}            
            </Slider>
        );
    }
}

// Redux Connect
const mapStateToProps = (state: IState, ownProps: any) => {
    return {
        carouselItems: state.carousels[ownProps.index] ? state.carousels[ownProps.index].carouselItems : [],
        hasMrec: state.carousels[ownProps.index] ? state.carousels[ownProps.index].hasMrec : false,
        nextQuery: state.carousels[ownProps.index] ? state.carousels[ownProps.index].nextQuery : ''
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