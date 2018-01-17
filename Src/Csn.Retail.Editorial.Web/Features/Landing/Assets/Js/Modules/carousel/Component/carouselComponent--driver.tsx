import React from 'react'
import { connect } from 'react-redux'
import { IState, ISimpleSlider } from 'carousel/Types'
import Slider from 'react-slick'
import NavButton from 'carousel/Component/carouselComponentArrows'

if (!SERVER) {
    require('Carousel/Css/carousel')
}

class SimpleSlider extends React.Component<ISimpleSlider> {

    constructor(props: any) {
        super(props)
    }

    render() {
        const settings = {
            infinite: false,
            speed: 500,
            slidesToShow: 6,
            slidesToScroll: 1,
            arrows: true,
            responsive: [ 
                { breakpoint: 1200, settings: { slidesToShow: 3 } },
                { breakpoint: 1600, settings: { slidesToShow: 4 } },
                { breakpoint: 2000, settings: { slidesToShow: 5 } }, 
            ],
            nextArrow: <NavButton text="Next" />,
            prevArrow: <NavButton text="Prev" />
        }
        return (
            <Slider {...settings}>
                {this.props.carouselItems.map((item, index) => 
                    <div key={index} className="">
                        <a href={item.itemUrl} data-webm-clickvalue={`item`}>
                            <img src={item.imageUrl}/>
                        </a>
                   </div>)}            
            </Slider>
        );
    }
}

// Redux Connect
const mapStateToProps = (state: IState, ownProps: any) => {
    return {
        carouselItems: state.carousels[ownProps.index] ? state.carousels[ownProps.index].carouselItems : [],
    }
}

export default connect(
    mapStateToProps
)(SimpleSlider)