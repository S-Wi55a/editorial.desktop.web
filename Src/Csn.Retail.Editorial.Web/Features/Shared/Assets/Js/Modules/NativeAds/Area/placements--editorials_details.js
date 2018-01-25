// details page - related articles

const placements = [];
const events = ['csn_editorial.moreArticles.ready'];
const templates = {};

// Templates

// Note that if these handlebars need to be recompiled just go to http://plugin.mediavoice.com/
/*

   This function represents a pre-compiled Handlebars template. Pre-compiled
   templates are not pretty, but they provide a very significant performance
   boost, especially on mobile devices. For more information, see
   http://handlebarsjs.com/precompilation.html.

   Note that this code has been generated from the following markup:

    <div class="more-article" data-sponsor='{{sponsor.name}}' data-ad-type='{{custom.PlacementType}}'>
        <a class="more-article__link-container" href="{{link}}">
            <div class="more-article__image">
                <img width="140" height="93" src="{{getThumbHref width=140}}" />
            </div>
            <div class="more-article__content">
                <div class="more-article__title">
                    <h2>{{title}}</h2>
                </div>
                <p class="more-article__link">Read More</p>
                <div class="more-article__banner more-article__banner--{{custom.PlacementType}}">{{custom.PlacementType}}</div>
            </div>
        </a>
    </div>

*/

templates['more-article'] = (event) => function (Handlebars, depth0, helpers, partials, data) { this.compilerInfo = [4, '>= 1.0.0']; helpers = this.merge(helpers, Handlebars.helpers); data = data || {}; var buffer = "", stack1, stack2, options, functionType = "function", escapeExpression = this.escapeExpression, helperMissing = helpers.helperMissing; buffer += "<div class=\"more-article\" data-sponsor='" + escapeExpression(((stack1 = ((stack1 = depth0.sponsor), stack1 == null || stack1 === false ? stack1 : stack1.name)), typeof stack1 === functionType ? stack1.apply(depth0) : stack1)) + "' data-ad-type='" + escapeExpression(((stack1 = ((stack1 = depth0.custom), stack1 == null || stack1 === false ? stack1 : stack1.PlacementType)), typeof stack1 === functionType ? stack1.apply(depth0) : stack1)) + "'>\n                    <a class=\"more-article__link-container\" href=\""; if (stack2 = helpers.link) { stack2 = stack2.call(depth0, { hash: {}, data: data }); } else { stack2 = depth0.link; stack2 = typeof stack2 === functionType ? stack2.apply(depth0) : stack2; } buffer += escapeExpression(stack2) + "\">\n                        <div class=\"more-article__image\">\n    <img width=\"140\" height=\"93\" src=\""; options = { hash: { 'width': (140) }, data: data }; buffer += escapeExpression(((stack1 = helpers.getThumbHref || depth0.getThumbHref), stack1 ? stack1.call(depth0, options) : helperMissing.call(depth0, "getThumbHref", options))) + "\" />\n                        </div>\n                        <div class=\"more-article__content\">\n                            <div class=\"more-article__title\">\n                                <h2>"; if (stack2 = helpers.title) { stack2 = stack2.call(depth0, { hash: {}, data: data }); } else { stack2 = depth0.title; stack2 = typeof stack2 === functionType ? stack2.apply(depth0) : stack2; } buffer += escapeExpression(stack2) + "</h2>\n                            </div>\n                            <p class=\"more-article__link\">Read More</p>\n                            <div class=\"more-article__banner more-article__banner--" + escapeExpression(((stack1 = ((stack1 = depth0.custom), stack1 == null || stack1 === false ? stack1 : stack1.PlacementType)), typeof stack1 === functionType ? stack1.apply(depth0) : stack1)) + "\">" + escapeExpression(((stack1 = ((stack1 = depth0.custom), stack1 == null || stack1 === false ? stack1 : stack1.PlacementType)), typeof stack1 === functionType ? stack1.apply(depth0) : stack1)) + "</div>\n                        </div>\n                    </a>\n                </div>\n\n"; return buffer; };

// Placement Objects

/**
 * location: Enter jQuery selector. For example: .articles ul li:eq(2)
 * name: name
 * placementId: placementId from Media motive
 * template: handlebars precompiled template
 */


placements.push({
    location: '.more-articles .more-articles__slide:eq(2) .more-article',
    name: 'details-page-more-articles-1',
    placementId: '30',
    template: templates['more-article']
});

placements.push({
    location: '.more-articles .more-articles__slide:eq(6) .more-article',
    name: 'details-page-related-articles-2',
    placementId: '31',
    template: templates['more-article']
});


export { placements, events }