import React from 'react'
import { connect } from 'react-redux'
import SearchResultCard from 'Components/SearchResultCard/searchResultCard'
import Slider from 'react-slick'
import { Actions, ActionTypes, Thunks } from 'carousel/Actions/actions'


if (!SERVER) {
    require('Carousel/Css/carousel')
}

class SimpleSlider extends React.Component {

    constructor(props) {
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
            beforeChange: function (oldIndex, newIndex) {                
                // Check if moving forward
                if (newIndex > oldIndex) {
                    // Check if we are near the end 
                    if (newIndex >= props.searchResults.length - this.slidesToShow) {
                        //dispatch action
                        props.fetch(props.nextQuery, props.index)
                    }
                }
            }
        }
        return (
            <Slider {...settings}>
                {this.props.searchResults.map((data, index) => <div key={index}><SearchResultCard key={index} {...data} /></div>)}            
            </Slider>
        );
    }
}

// Redux Connect
const mapStateToProps = (state, ownProps) => {
    return {    
        searchResults: state.carousels[ownProps.index] ? state.carousels[ownProps.index].carouselItems : [],
        hasMrec: state.carousels[ownProps.index] ? state.carousels[ownProps.index].hasMrec : false,
        nextQuery: state.carousels[ownProps.index] ? state.carousels[ownProps.index].nextQuery : ''
    }
}

const mapDispatchToProps = (dispatch) => {
    return {
        fetch: (query, index)=> {
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