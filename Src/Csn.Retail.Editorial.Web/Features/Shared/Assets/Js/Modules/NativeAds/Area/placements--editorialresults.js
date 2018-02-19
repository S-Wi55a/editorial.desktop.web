import moment from 'moment';

// details page - related articles
const placements = [];
const events = ['csn_editorial.listings.fetchNativeAds'];
const templates = {};

const store = window.store || console.warn("No Redux store available")

// Templates
// We are highjacking the template function and using it to pass data to our Redux Store

templates['search-result'] = (location) => (event) => (Handlebars, depth0, helpers, partials, data) => { 
    
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

    store.dispatch(
         {
             type: "ADD_PROMOTED_ARTICLE",
             payload: {
                imageUrl: closestImageBasedOnRes(480, depth0.image.instances),
                headline: depth0.title,
                subHeading: depth0.summary,
                dateAvailable: date,
                articleDetailsUrl: depth0.link,
                label: depth0.custom.PlacementType,
                type: depth0.custom.PlacementType,
                location
             }
         }
       )
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
    name: 'listings-page-1',
    placementId: '30',
    template: templates['search-result'](4) // Set the placement location 
})
placements.push({
    location: 'body', // We need to location to always be true
    name: 'listings-page-2',
    placementId: '31',
    template: templates['search-result'](9) // Set the placement location 
})
placements.push({
    location: 'body', // We need to location to always be true
    name: 'listings-page-3',
    placementId: '32',
    template: templates['search-result'](14) // Set the placement location 
})



export { placements, events }