// details page - related articles
const placements = [];
const events = ['csn_editorial.moreArticles.ready'];
const templates = {};

const store = window.store || console.warn("No Redux store available")

// Templates
// We are highjacking the template function and using it to pass data to our Redux Store

templates['search-result'] = (location) => (Handlebars, depth0, helpers, partials, data) => { 
     store.dispatch(
         {
             type: "ADD_PROMOTED_ARTICLE",
             payload: {
                imageUrl: depth0.image.instances[0],
                headline: depth0.title,
                dateAvailable: depth0.pubDate,
                articleDetailsUrl: depth0.link,
                label: depth0.custom.PlacementType,
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
    name: 'details-page-more-articles-1',
    placementId: '30',
    template: templates['search-result'](3) // Set the placement location 
});

placements.push({
    location: 'body', // We need to location to always be true
    name: 'details-page-related-articles-2',
    placementId: '31',
    template: templates['search-result'](4) // Set the placement location 
});


export { placements, events }