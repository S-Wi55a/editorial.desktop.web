import React from 'react'
import { connect } from 'react-redux'
import SearchResultCard from 'Components/SearchResultCard/searchResultCard'
import Slider from 'react-slick'



class SimpleSlider extends React.Component {

    constructor(props) {
        super(props)
    }

    render() {
        const settings = {
            dots: true,
            infinite: true,
            speed: 500,
            slidesToShow: 7,
            slidesToScroll: 1,
            arrows: true,
            responsive: [ 
                { breakpoint: 768, settings: { slidesToShow: 3 } }, 
                { breakpoint: 1024, settings: { slidesToShow: 5 } }, 
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