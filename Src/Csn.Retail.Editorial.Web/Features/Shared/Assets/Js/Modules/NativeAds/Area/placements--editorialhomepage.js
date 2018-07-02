import moment from 'moment';

// details page - related articles
const placements = [];
const events = ['csn_editorial.landing.fetchNativeAds'];
const templates = {};

const store = window.store || console.warn("No Redux store available")

// Templates
// We are highjacking the template function and using it to pass data to our Redux Store

templates['homepage'] = (location, placementId) => (event) => (Handlebars, depth0, helpers, partials, data) => { 
    
    var pubDate = new Date(depth0.pubDate);
    var now = new Date();
    var date = (pubDate.getFullYear() === now.getFullYear()) ? moment(pubDate).format('MMMM Do') : moment(pubDate).format('MMMM YYYY')

    function closestImageBasedOnRes(num, arr) {
        var curr = arr[0];
        var diff = Math.abs (num - curr.sourceWidth);
        for (var val = 0; val < arr.length; val++) {
            var newdiff = Math.abs (num - arr[val].sourceWidth);
            if (newdiff < diff) {
                diff = newdiff;
                curr = arr[val];
            }
        }
        return curr.href;
    }
    
    // Check if carousel id is present which is passed in event detail
    if (event.detail.carouselId !== null && event.detail.placementId === placementId) {
        store.dispatch(
            {
                type: "CAROUSELS/ADD_PROMOTED_ARTICLE",
                payload: {
                    isNativeAd: true,
                    imageUrl: closestImageBasedOnRes(480, depth0.image.instances),
                    headline: depth0.title,
                    subHeading: depth0.summary,
                    dateAvailable: date,
                    articleDetailsUrl: depth0.link,
                    label: depth0.custom.PlacementType,
                    type: depth0.custom.PlacementType,
                    location,
                    carouselId: event.detail.carouselId
                }
            }
        )
    }
}
    

// Placement Objects

/**
 * location: Enter jQuery selector. For example: .articles ul li:eq(2)
 * name: name
 * placementId: placementId from Media motive
 * template: handlebars precompiled template
 */

placements.push({
    location: 'body', // We need to location to always be true
    name: 'landing-page-1',
    placementId: '31',
    template: templates['homepage'](3, 31) // Set the placement location 
})
placements.push({
    location: 'body', // We need to location to always be true
    name: 'landing-page-2',
    placementId: '32',
    template: templates['homepage'](3, 32) // Set the placement location 
})
placements.push({
    location: 'body', // We need to location to always be true
    name: 'landing-page-3',
    placementId: '33',
    template: templates['homepage'](3, 33) // Set the placement location 
})

export { placements, events }