// Details Page css files
require('./Css/Details-page.scss');

//------------------------------------------------------------------------------------------------------------------

import { loaded } from 'document-promises/document-promises.js'
import ScrollMagic from 'ScrollMagic'
import * as isMobile from 'ismobilejs'
if (process.env.DEBUG) { require('debug.addIndicators'); }

//------------------------------------------------------------------------------------------------------------------
// Hero
let aboveTheFold = require('Hero/hero.js').default;
aboveTheFold();

//Editors Rating
(function editorRatings() {
    if (document.querySelector('.editors-ratings')) {
        require('EditorsRatings/editorsRating-component.js');
    }
})();

// TEADS
loaded.then(() => {

    const tile7 = document.querySelector('#Tile7')
    if (tile7) {
        let el = document.querySelectorAll('.article__copy p');
        el = (el.length >= 2) ? el[1] : (el.length ? el[0] : undefined);
        if(el){
            el.insertAdjacentHTML('afterend', '<div id="teads-video-container" style="clear:both;"></div>')
            document.querySelector('#teads-video-container').appendChild(tile7)
        }        
    }
});

//Lazy load More articles JS
loaded.then(() => {
    (function moreArticles(d) {

        if (d.querySelector('.more-articles-placeholder')) {
            (function moreArticles() {
                import ( /* webpackChunkName: "More-Articles" */ 'MoreArticles/moreArticles-component.js').then(function(moreArticles) {}).catch(function(err) {
                    console.log('Failed to load More-Articles', err);
                });
            })()
        }
    })(document)
});

//Lazy load Stock For Sale JS
loaded.then(() => {
    (function stockForSale(d) {
        if (d.querySelector('.stock-for-sale-placeholder')) {
            (function stockForSale() {
                import ( /* webpackChunkName: "Stock-For-Sale" */ 'StockForSale/stockForSale-component.js').then(function(stockForSale) {}).catch(function(err) {
                    console.log('Failed to load Stock-For-Sale', err);
                });
            })()
        }
    })(document)
});

//Lazy load Spec Module
loaded.then(() => {
    (function specModule(d) {
    if (csn_editorial.specVariantsQuery) {
        // Add placeholder 
        let el = d.querySelectorAll('.article__copy p');
        el = (el.length >= 2) ? el[1] : (el.length ? el[0] : undefined);
            if (el) { el.insertAdjacentHTML('afterend', '<div class="spec-module-placeholder" data-webm-section="spec-module"></div>'); }

            (function specModule() {
                import ( /* webpackChunkName: "Spec-Module" */ 'SpecModule/specModule--container.js').then(function(specModule) {}).catch(function(err) {
                    console.log('Failed to load Spec-Module', err);
                });
            })()
        }
    })(document)
});

//Lazy load Also Consider JS
loaded.then(() => {
    (function alsoConsider(d) {

        if (d.querySelector('.also-consider-placeholder')) {
            (function alsoConsider() {
                import ( /* webpackChunkName: "Also-Consider" */ 'AlsoConsider/alsoConsider-component.js').then(function(alsoConsider) {}).catch(function(err) {
                    console.log('Failed to load alsoConsider', err);
                });
            })()
        }

    })(document)
});

//Lazy load Disqus
let disqus = function(d, w, selector) {
    const disqusSelector = d.querySelector(selector);
    if (disqusSelector) {

        // Set scene
        const triggerElement = selector
        const triggerHook = 1
        const offset = (-1 * w.innerHeight) * 2;
        w.scrollMogicController = w.scrollMogicController || new ScrollMagic.Controller();

        let scene = new ScrollMagic.Scene({
            triggerElement: triggerElement,
            triggerHook: triggerHook,
            offset: offset,
            reverse: false
        })
            .on("enter", () => {
                require('Disqus/disqus.js').default();
                scene.destroy(true)
                scene = null
            })
            .addTo(w.scrollMogicController);
    }
}
disqus(document, window, '#disqus_thread');

//Lazy Native Ads
loaded.then(() => {
    (function nativeAds() {
        if (!!csn_editorial && !!csn_editorial.nativeAds) {
            (function nativeAds() {
                import
                (/* webpackChunkName: "Native Ads" */ 'NativeAds/nativeAds.js').then(function(nativeAds) {})
                    .catch(function(err) {
                        console.log('Failed to load nativeAds', err);
                    });
            })();
        }
    })();
});

//Lazy load Sponsored articles JS
loaded.then(() => {
    (function sponsoredArticles(d) {
        if (d.querySelector('.article__type--sponsored')) {
            (function nativeAds() {
                import
                (/* webpackChunkName: "Sponsored Articles" */ 'SponsoredArticles/sponsoredArticles.js')
                    .then(function(sponsoredArticles) {}).catch(function(err) {
                        console.log('Failed to load sponsoredArticles', err);
                    });
            })();
        }
    })(document);
});

// display disclaimer on pricing guide
require('ArticlePricing/articlePricing.js');

// add hero-wide-video
if (document.querySelector('.article-type--widevideo')) {
    require('Js/Modules/Hero/hero-wide-video.js');
}

//Parallax
loaded.then(function () {
    if (document.querySelector('.csn-parallax')) {
        require.ensure(['rellax'],
            function () {
                const Rellax = require('rellax');
                let rellax = new Rellax('.csn-parallax', {});
                
                window.addEventListener('resize', function() {
                    rellax.destroy();
                    rellax = new Rellax('.csn-parallax', {});
                });
            },
            'Parallax - Rellax');
    }   
});

//Sticky Sidebar
if(!document.querySelector('body').classList.contains('ie') || !isMobile.tablet || !isMobile.phone){
    const aside = document.querySelector('.aside');
    loaded.then(function() {     
        if (aside) {
            require('StickySidebar/stickySidebar.js').init(document, window, aside);
        }
    })
}
