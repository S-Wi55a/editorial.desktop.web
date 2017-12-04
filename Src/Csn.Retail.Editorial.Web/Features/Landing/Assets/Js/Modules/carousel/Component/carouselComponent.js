import React from 'react'
import { connect } from 'react-redux'
import SearchResultCard from 'Components/SearchResultCard/searchResultCard'
import Slider from 'react-slick'

if (!SERVER) {
    require('Carousel/Css/carousel')
}

class SimpleSlider extends React.Component {

    constructor(props) {
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
            ]
        }
        return (
            <Slider {...settings}>
                {this.props.searchResults.map((data, index) => <div key={index}><SearchResultCard key={index} {...data} /></div>)}            
            </Slider>
        );
    }
}

// Redux Connect
const mapStateToProps = (state) => {
    return {    
        searchResults: state.store.categories.featured,
    }
}


export default connect(
    mapStateToProps
)(SimpleSlider)