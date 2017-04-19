//Make config

//Load Native ads
// Load native add pugin
// make tempaltes
// load templates
// Add event lsitener to native ads



(function ($) {
    if (!csn_editorial.nativeAds) {
        return;
    }

    if (typeof KruxSasIntegrator === 'undefined') {
        return;
    }

    //if(typeof jQuery().popover === 'undefined'){
    //    return;
    //}

    //var compiledTemplate0 = "";
    var templates = [];
    var placementsByUrl = [];
    var placementsByArea = [];
    populateTemplates();
    populatePlacements();

    window.NATIVEADS = window.NATIVEADS || {};
    window.NATIVEADS_QUEUE = window.NATIVEADS_QUEUE || [];

    var q = function () {
        return window.NATIVEADS_QUEUE;
    };

    // get required parameters from query string
    var siteName = csn_editorial.nativeAds.siteName

    var areaName = csn_editorial.nativeAds.areaName
    var makeModel = l

    q().push(["setPropertyID", csn_editorial.nativeAds.setPropertyID]);

    q().push(["enableMOAT", true, true]);

    // used for tracking internal clicks (sponsored and promoted content hosted internally). The Mediamotive team manually 
    // put the tracknativeads=true parameter onto our nativeAd links so we can track behaviour of users hitting these pages from the native ad links.
    // see https://support.polar.me/hc/en-us/articles/202275040-How-to-enable-MediaVoice-Analytics-Tracking-on-Internal-Content-Pages?flash_digest=bceb3d4343054cc7e0d4793ac3ceb9dd314c1beb
    q().push([
        "configureSecondaryPage",
        {
            track: function () {
                if (location.search.indexOf('tracknativeads=true') >= 0) {
                    return "inbound";
                }
            }
        }
    ]);

    var placements = getPlacements(areaName);

    if (!placements || placements.length === 0) {
        return;
    }

    var viewId = getRandomArbitrary();



    //TODO: Make func and add to event listner
    // registered events 
    const registeredEvents = ['csn_editorial.moreArticles.ready']
    registeredEvents.forEach((event) => {
        window.addEventListener(event, () => {
            //placements function
        })
    });
    //window.addEventListener()

    //placements array
    //did find
    //remaining



    // loop through each of the placements and check whether we need to push onto this page
    placements.forEach(function (placement) {


        // If tile is already applied, skip
        if (placement.active) {
            return
        }



        // no need to check whether placement exists...polar just ignores
        q().push(["insertPreview", {
            label: placement.name,
            unit: {
                "server": "sas",
                "host": "mm.carsales.com.au",
                "customer": "carsales",
                "site": siteName,
                "area": areaName,
                "size": "2x2",
                "custom": {
                    "tile": placement.placementId,
                    "kuid": KruxSasIntegrator.kuid,
                    "ksg": KruxSasIntegrator.ksg,
                    "random": getRandomArbitrary(),
                    "viewid": viewId,
                    "car": makeModel
                }
            },
            location: placement.location,
            template: placement.template,
            onRender: function ($element) {
                // at this point the next element is actually the moat script tag
                // have a check to make sure you are not removing the moat script accidentally here
                removeReplacedTile($element);
                //$element.fadeIn();

                $element.parents(".td-block-row").addClass('native-ad-block');

                var parentSpan = $element.parents("div[class^='td-block-span']");
                parentSpan.addClass('native-ad');
                parentSpan.addClass($element.data('ad-type'));

                var adTypeDiv = $element.find('.native-ad-type');
                //jQuery(adTypeDiv).popover({
                //    html: true,
                //    placement: 'top',
                //    content: getTooltipContent($element, $element.data('sponsor'), $element.data('ad-type')),
                //    template: '<div class="popover"><div class="arrow"></div><div class="popover-content"></div></div>'
                //});

                //jQuery(adTypeDiv).click(function(){
                //  jQuery('.native-ad-type').not(this).popover('hide');
                //});
            },
            onFill: function (data) {
                // console.dir(data);
            },
            onError: function (error) { }
        }]);
    });

    /**
     * Looks for the tile which needs to be hidden
     */
    function removeReplacedTile($element) {
        // loop through and look for the next element
        var maxLoops = 3;
        var loopNum = 1;

        var $nextElement = $element;

        // we find the element we want to remove using the class of the element added
        var elementClass = $element.attr('class');

        while (loopNum <= maxLoops) {
            $nextElement = $nextElement.next();

            // check to see whether this is a tile
            if ($nextElement.attr('class') === elementClass) {
                $nextElement.remove();
                break;
            }

            loopNum++;
        }
    }

    function getRandomArbitrary() {
        return Math.floor(Math.random() * (9999999 - 999999)) + 999999;
    }

    // render the content used for the tooltip. This HTML should be kept hidden by ensuring that 'tooltip-content' has display:none
    function getTooltipContent($element, sponsorName, type) {
        var typeSpecificContent;
        if (type == 'Sponsored') {
            typeSpecificContent = "This Ad and the content is sponsored. This is brought to you by " + sponsorName + ".";
        }
        else {
            typeSpecificContent = "Promoted content has been ranked relevant to you based on your preferences. This is independent editorial and the material contained within has not been edited or influenced by the advertiser.";
        }

        return "<h3>" + type + "</h3><a class='close' onclick='javascript: jQuery(this).parents(\".native-ad-label\").find(\".native-ad-type\").popover(\"hide\");'>x</a><div><p>" + typeSpecificContent + "</p><div class='learn-more'><a href='https://help.carsales.com.au/hc/en-gb/articles/208468026' target='_blank'>Learn more</a></div></div>";
    }

    function populatePlacementsByUrl() {
        // reviews listing page
        placementsByUrl['car-reviews'] = [];
        placementsByUrl['car-reviews'].push({
            location: '.td_block_4 .td-block-row:eq(2) .td-block-span6:eq(1) .td_module_4',
            name: 'reviews-listing-1',
            placementId: '30',
            template: templates['td_module_4']
        });
        placementsByUrl['car-reviews'].push({
            location: '.td_block_4 .td-block-row:eq(4) .td-block-span6:eq(1) .td_module_4',
            name: 'reviews-listing-2',
            placementId: '31',
            template: templates['td_module_4']
        });

        // advice listing page
        placementsByUrl['car-advice'] = [];
        placementsByUrl['car-advice'].push({
            location: '.td_block_4 .td-block-row:eq(2) .td-block-span6:eq(1) .td_module_4',
            name: 'advice-listing-1',
            placementId: '30',
            template: templates['td_module_4']
        });

        // news listing page
        placementsByUrl['car-news'] = [];
        placementsByUrl['car-news'].push({
            location: '.td_block_2:eq(0) .td-block-row:eq(0) .td-block-span6:eq(1) .td_module_4',
            name: 'news-listing-1',
            placementId: '30',
            template: templates['td_module_4']
        });
        placementsByUrl['car-news'].push({
            location: '.td_block_2:eq(1) .td-block-row:eq(0) .td-block-span6:eq(1) .td_module_4',
            name: 'news-listing-2',
            placementId: '31',
            template: templates['td_module_4']
        });

        // video listing page
        placementsByUrl['car-videos'] = [];
        placementsByUrl['car-videos'].push({
            location: '.td_block_5:eq(0) .td-block-row:eq(3) .td-block-span6:eq(1) .td_module_3',
            name: 'video-listing-1',
            placementId: '30',
            template: templates['td_module_3']
        });
        placementsByUrl['car-videos'].push({
            location: '.td_block_5:eq(0) .td-block-row:eq(5) .td-block-span6:eq(1) .td_module_3',
            name: 'video-listing-2',
            placementId: '31',
            template: templates['td_module_3']
        });
    }

    function populatePlacementsByArea() {
        // details page - related articles
        placementsByArea['editorials_details'] = [];
        placementsByArea['editorials_details'].push({
            location: '.csn-related-posts .td-block-row:eq(0) .td-block-span4:eq(2) .td_module_mx4',
            name: 'details-page-related-articles-1',
            placementId: '30',
            template: templates['td_module_mx4']
        });
        placementsByArea['editorials_details'].push({
            location: '.csn-related-posts .td-block-row:eq(1) .td-block-span4:eq(2) .td_module_mx4',
            name: 'details-page-related-articles-2',
            placementId: '31',
            template: templates['td_module_mx4']
        });

        // lifestyle page
        placementsByArea['lifestyle'] = [];
        placementsByArea['lifestyle'].push({
            location: '.td_block_4:eq(0) .td-block-row:eq(1) .td-block-span6:eq(1) .td_module_4',
            name: 'lifestyle-listing-news-block',
            placementId: '30',
            template: templates['td_module_4']
        });
        placementsByArea['lifestyle'].push({
            location: '.td_block_5:eq(0) .td-block-row:eq(1) .td-block-span6:eq(1) .td_module_3',
            name: 'lifestyle-listing-video-block',
            placementId: '31',
            template: templates['td_module_3']
        });
    }

    function getPlacements(areaName) {
        // need to get all the placements which are relevant
        var placements = [];
        var areaLookup = areaName;
        if (areaLookup.startsWith("lifestyle_")) {
            areaLookup = "lifestyle";
        }

        placements.push.apply(placements, placementsByArea[areaLookup]);

        var currentUrlContext = getCurrentUrlContext();
        if (currentUrlContext) {
            placements.push.apply(placements, placementsByUrl[currentUrlContext]);
        }

        return placements;
    }

    function getCurrentUrlContext() {
        // we're just interested in the last part of the URL path. The regular expression removes the leading and trailing '/' if there are any
        var splitPath = document.location.pathname.replace(/^\/?(.*?)\/?$/, '$1').split("/");

        return splitPath[splitPath.length - 1];
    }

    function populatePlacements() {
        // we populate placements by area and then we can easily look up the placements for a given area
        populatePlacementsByArea();
        populatePlacementsByUrl();
    }

    function populateTemplates() {
        // Note that if these handlebars need to be recompiled just go to http://plugin.mediavoice.com/v0.5.94/
        /*
  
           This function represents a pre-compiled Handlebars template. Pre-compiled
           templates are not pretty, but they provide a very significant performance
           boost, especially on mobile devices. For more information, see
           http://handlebarsjs.com/precompilation.html.
  
           Note that this code has been generated from the following markup:
  
        <div class="td_module_mx4 td_module_wrap td-animation-stack" data-sponsor='{{sponsor.name}}' data-ad-type='{{custom.PlacementType}}'>
          <div class="td-module-image" data-webm-clickvalue="click-{{custom.PlacementType}}-ad">
            <div class="td-module-thumb"><a href="{{link}}" rel="nofollow"><img width="300" height="194" class="entry-thumb" src="{{getThumbHref width=300 height=194}}"></a></div>
          </div>
          <div data-webm-clickvalue="click-{{custom.PlacementType}}-ad">
            <h3 class="entry-title td-module-title"><a href="{{link}}" rel="nofollow">{{title}}</a></h3>
          </div>
          <div class="native-ad-label"><div class="native-ad-type">{{custom.PlacementType}}</div></div>
        </div>
  
        */

        templates['td_module_mx4'] = function (Handlebars, depth0, helpers, partials, data) { this.compilerInfo = [4, '>= 1.0.0']; helpers = this.merge(helpers, Handlebars.helpers); data = data || {}; var buffer = "", stack1, stack2, options, functionType = "function", escapeExpression = this.escapeExpression, helperMissing = helpers.helperMissing; buffer += "<div class=\"td_module_mx4 td_module_wrap td-animation-stack\" data-sponsor='" + escapeExpression(((stack1 = ((stack1 = depth0.sponsor), stack1 == null || stack1 === false ? stack1 : stack1.name)), typeof stack1 === functionType ? stack1.apply(depth0) : stack1)) + "' data-ad-type='" + escapeExpression(((stack1 = ((stack1 = depth0.custom), stack1 == null || stack1 === false ? stack1 : stack1.PlacementType)), typeof stack1 === functionType ? stack1.apply(depth0) : stack1)) + "'>\n      <div class=\"td-module-image\" data-webm-clickvalue=\"click-" + escapeExpression(((stack1 = ((stack1 = depth0.custom), stack1 == null || stack1 === false ? stack1 : stack1.PlacementType)), typeof stack1 === functionType ? stack1.apply(depth0) : stack1)) + "-ad\">\n        <div class=\"td-module-thumb\"><a href=\""; if (stack2 = helpers.link) { stack2 = stack2.call(depth0, { hash: {}, data: data }); } else { stack2 = depth0.link; stack2 = typeof stack2 === functionType ? stack2.apply(depth0) : stack2; } buffer += escapeExpression(stack2) + "\" rel=\"nofollow\"><img width=\"300\" height=\"194\" class=\"entry-thumb\" src=\""; options = { hash: { 'width': (300), 'height': (194) }, data: data }; buffer += escapeExpression(((stack1 = helpers.getThumbHref || depth0.getThumbHref), stack1 ? stack1.call(depth0, options) : helperMissing.call(depth0, "getThumbHref", options))) + "\"></a></div>\n      </div>\n      <div data-webm-clickvalue=\"click-" + escapeExpression(((stack1 = ((stack1 = depth0.custom), stack1 == null || stack1 === false ? stack1 : stack1.PlacementType)), typeof stack1 === functionType ? stack1.apply(depth0) : stack1)) + "-ad\">\n        <h3 class=\"entry-title td-module-title\"><a href=\""; if (stack2 = helpers.link) { stack2 = stack2.call(depth0, { hash: {}, data: data }); } else { stack2 = depth0.link; stack2 = typeof stack2 === functionType ? stack2.apply(depth0) : stack2; } buffer += escapeExpression(stack2) + "\" rel=\"nofollow\">"; if (stack2 = helpers.title) { stack2 = stack2.call(depth0, { hash: {}, data: data }); } else { stack2 = depth0.title; stack2 = typeof stack2 === functionType ? stack2.apply(depth0) : stack2; } buffer += escapeExpression(stack2) + "</a></h3>\n      </div>\n      <div class=\"native-ad-label\"><div class=\"native-ad-type\">" + escapeExpression(((stack1 = ((stack1 = depth0.custom), stack1 == null || stack1 === false ? stack1 : stack1.PlacementType)), typeof stack1 === functionType ? stack1.apply(depth0) : stack1)) + "</div></div>\n    </div>"; return buffer; };

        /*
        <div class="td_module_4 td_module_wrap td-animation-stack" data-sponsor='{{sponsor.name}}' data-ad-type='{{custom.PlacementType}}'>
          <div class="td-module-image" data-webm-clickvalue="click-{{custom.PlacementType}}-ad">
            <div class="td-module-thumb">
              <a href="{{link}}" rel="nofollow">
                <img width="300" height="194" class="entry-thumb" src="{{getThumbHref width=300 height=194}}"/>
              </a>
            </div>
          </div>
          <div data-webm-clickvalue="click-{{custom.PlacementType}}-ad">
            <h3 class="entry-title td-module-title">
              <a href="{{link}}" rel="nofollow">{{title}}</a>
            </h3>
          </div>
          {{#if summary}}<div class="td-excerpt">{{summary}}</div>{{/if}}
          <div class="native-ad-label"><div class="native-ad-type">{{custom.PlacementType}}</div></div>
        </div>
        */

        templates['td_module_4'] = function (Handlebars, depth0, helpers, partials, data) { this.compilerInfo = [4, '>= 1.0.0']; helpers = this.merge(helpers, Handlebars.helpers); data = data || {}; var buffer = "", stack1, stack2, options, functionType = "function", escapeExpression = this.escapeExpression, helperMissing = helpers.helperMissing, self = this; function program1(depth0, data) { var buffer = "", stack1; buffer += "<div class=\"td-excerpt\">"; if (stack1 = helpers.summary) { stack1 = stack1.call(depth0, { hash: {}, data: data }); } else { stack1 = depth0.summary; stack1 = typeof stack1 === functionType ? stack1.apply(depth0) : stack1; } buffer += escapeExpression(stack1) + "</div>"; return buffer; } buffer += "<div class=\"td_module_4 td_module_wrap td-animation-stack\" data-sponsor='" + escapeExpression(((stack1 = ((stack1 = depth0.sponsor), stack1 == null || stack1 === false ? stack1 : stack1.name)), typeof stack1 === functionType ? stack1.apply(depth0) : stack1)) + "' data-ad-type='" + escapeExpression(((stack1 = ((stack1 = depth0.custom), stack1 == null || stack1 === false ? stack1 : stack1.PlacementType)), typeof stack1 === functionType ? stack1.apply(depth0) : stack1)) + "'>\n      <div class=\"td-module-image\" data-webm-clickvalue=\"click-" + escapeExpression(((stack1 = ((stack1 = depth0.custom), stack1 == null || stack1 === false ? stack1 : stack1.PlacementType)), typeof stack1 === functionType ? stack1.apply(depth0) : stack1)) + "-ad\">\n        <div class=\"td-module-thumb\">\n          <a href=\""; if (stack2 = helpers.link) { stack2 = stack2.call(depth0, { hash: {}, data: data }); } else { stack2 = depth0.link; stack2 = typeof stack2 === functionType ? stack2.apply(depth0) : stack2; } buffer += escapeExpression(stack2) + "\" rel=\"nofollow\">\n            <img width=\"300\" height=\"194\" class=\"entry-thumb\" src=\""; options = { hash: { 'width': (300), 'height': (194) }, data: data }; buffer += escapeExpression(((stack1 = helpers.getThumbHref || depth0.getThumbHref), stack1 ? stack1.call(depth0, options) : helperMissing.call(depth0, "getThumbHref", options))) + "\"/>\n          </a>\n        </div>\n      </div>\n      <div data-webm-clickvalue=\"click-" + escapeExpression(((stack1 = ((stack1 = depth0.custom), stack1 == null || stack1 === false ? stack1 : stack1.PlacementType)), typeof stack1 === functionType ? stack1.apply(depth0) : stack1)) + "-ad\">\n        <h3 class=\"entry-title td-module-title\">\n          <a href=\""; if (stack2 = helpers.link) { stack2 = stack2.call(depth0, { hash: {}, data: data }); } else { stack2 = depth0.link; stack2 = typeof stack2 === functionType ? stack2.apply(depth0) : stack2; } buffer += escapeExpression(stack2) + "\" rel=\"nofollow\">"; if (stack2 = helpers.title) { stack2 = stack2.call(depth0, { hash: {}, data: data }); } else { stack2 = depth0.title; stack2 = typeof stack2 === functionType ? stack2.apply(depth0) : stack2; } buffer += escapeExpression(stack2) + "</a>\n        </h3>\n      </div>\n      "; stack2 = helpers['if'].call(depth0, depth0.summary, { hash: {}, inverse: self.noop, fn: self.program(1, program1, data), data: data }); if (stack2 || stack2 === 0) { buffer += stack2; } buffer += "\n      <div class=\"native-ad-label\"><div class=\"native-ad-type\">" + escapeExpression(((stack1 = ((stack1 = depth0.custom), stack1 == null || stack1 === false ? stack1 : stack1.PlacementType)), typeof stack1 === functionType ? stack1.apply(depth0) : stack1)) + "</div></div>\n    </div>"; return buffer; };

        /*
        <div class="td_module_3 td_module_wrap td-animation-stack" data-sponsor='{{sponsor.name}}' data-ad-type='{{custom.PlacementType}}'>
          <div class="td-module-image" data-webm-clickvalue="click-{{custom.PlacementType}}-ad">
            <div class="td-module-thumb">
              <a href="{{link}}" rel="nofollow">
                <img width="300" height="194" class="entry-thumb" src="{{getThumbHref width=300 height=194}}" />
              </a>
            </div>
          </div>
          <div data-webm-clickvalue="click-{{custom.PlacementType}}-ad">
            <h3 class="entry-title td-module-title">
              <a href="{{link}}" rel="nofollow">{{title}}</a>
            </h3>
          </div>
          <div class="native-ad-label"><div class="native-ad-type">{{custom.PlacementType}}</div></div>
        </div>
        */

        templates['td_module_3'] = function (Handlebars, depth0, helpers, partials, data) { this.compilerInfo = [4, '>= 1.0.0']; helpers = this.merge(helpers, Handlebars.helpers); data = data || {}; var buffer = "", stack1, stack2, options, functionType = "function", escapeExpression = this.escapeExpression, helperMissing = helpers.helperMissing; buffer += "<div class=\"td_module_3 td_module_wrap td-animation-stack\" data-sponsor='" + escapeExpression(((stack1 = ((stack1 = depth0.sponsor), stack1 == null || stack1 === false ? stack1 : stack1.name)), typeof stack1 === functionType ? stack1.apply(depth0) : stack1)) + "' data-ad-type='" + escapeExpression(((stack1 = ((stack1 = depth0.custom), stack1 == null || stack1 === false ? stack1 : stack1.PlacementType)), typeof stack1 === functionType ? stack1.apply(depth0) : stack1)) + "'>\n      <div class=\"td-module-image\" data-webm-clickvalue=\"click-" + escapeExpression(((stack1 = ((stack1 = depth0.custom), stack1 == null || stack1 === false ? stack1 : stack1.PlacementType)), typeof stack1 === functionType ? stack1.apply(depth0) : stack1)) + "-ad\">\n        <div class=\"td-module-thumb\">\n          <a href=\""; if (stack2 = helpers.link) { stack2 = stack2.call(depth0, { hash: {}, data: data }); } else { stack2 = depth0.link; stack2 = typeof stack2 === functionType ? stack2.apply(depth0) : stack2; } buffer += escapeExpression(stack2) + "\" rel=\"nofollow\">\n            <img width=\"300\" height=\"194\" class=\"entry-thumb\" src=\""; options = { hash: { 'width': (300), 'height': (194) }, data: data }; buffer += escapeExpression(((stack1 = helpers.getThumbHref || depth0.getThumbHref), stack1 ? stack1.call(depth0, options) : helperMissing.call(depth0, "getThumbHref", options))) + "\" />\n          </a>\n        </div>\n      </div>\n      <div data-webm-clickvalue=\"click-" + escapeExpression(((stack1 = ((stack1 = depth0.custom), stack1 == null || stack1 === false ? stack1 : stack1.PlacementType)), typeof stack1 === functionType ? stack1.apply(depth0) : stack1)) + "-ad\">\n        <h3 class=\"entry-title td-module-title\">\n          <a href=\""; if (stack2 = helpers.link) { stack2 = stack2.call(depth0, { hash: {}, data: data }); } else { stack2 = depth0.link; stack2 = typeof stack2 === functionType ? stack2.apply(depth0) : stack2; } buffer += escapeExpression(stack2) + "\" rel=\"nofollow\">"; if (stack2 = helpers.title) { stack2 = stack2.call(depth0, { hash: {}, data: data }); } else { stack2 = depth0.title; stack2 = typeof stack2 === functionType ? stack2.apply(depth0) : stack2; } buffer += escapeExpression(stack2) + "</a>\n        </h3>\n      </div>\n      <div class=\"native-ad-label\"><div class=\"native-ad-type\">" + escapeExpression(((stack1 = ((stack1 = depth0.custom), stack1 == null || stack1 === false ? stack1 : stack1.PlacementType)), typeof stack1 === functionType ? stack1.apply(depth0) : stack1)) + "</div></div>\n    </div>"; return buffer; };
    }
})(jQuery);

(function (d, s, id) {
    var js, fjs = d.getElementsByTagName(s)[0], p = d.location.protocol;
    if (d.getElementById(id)) { return; }
    js = d.createElement(s);
    js.id = id; js.type = "text/javascript"; js.async = true;
    js.src = ((p == "https:") ? p : "http:") + "//plugin.mediavoice.com/plugin.js";
    fjs.parentNode.insertBefore(js, fjs);
})(document, "script", "nativeads-plugin");