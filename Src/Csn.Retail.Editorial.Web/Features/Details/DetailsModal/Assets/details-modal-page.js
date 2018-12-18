// Details Page css files
require('./Css/details-modal-page.scss');

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
                import ( /* webpackChunkName: "More-Articles" */ 'MoreArticles/moreArticles-component.js').then(function(moreArticles) {disableExternalLinks('.more-articles-placeholder')}).catch(function(err) {
                    console.log('Failed to load More-Articles', err);
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
                import ( /* webpackChunkName: "Also-Consider" */ 'AlsoConsider/alsoConsider-component.js').then(function(alsoConsider) {disableExternalLinks('.also-consider-placeholder')}).catch(function(err) {
                    console.log('Failed to load alsoConsider', err);
                });
            })()
        }

    })(document)
});

//Lazy Native Ads
loaded.then(() => {
    (function nativeAds() {
        if (!!csn_editorial && !!csn_editorial.nativeAds) {
            (function nativeAds() {
                import
                (/* webpackChunkName: "Native Ads" */ 'NativeAds/nativeAds.js').then(function(nativeAds) {disableExternalLinks('.more-articles-placeholder')})
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
                    .then(function(sponsoredArticles) {disableExternalLinks('.article__type--sponsored')}).catch(function(err) {
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
if(!document.querySelector('body').classList.contains('ie') && !isMobile.tablet && !isMobile.phone){
    const aside = document.querySelector('.aside');
    loaded.then(function() {     
        if (aside) {
            require('Js/Modules/StickySidebar/stickySidebar.js').init(document, window, aside, 'article .article', 137);
        }
    })
}

//Disable external links
loaded.then(() => {
    disableExternalLinks(document);
});

let disableExternalLinks = (scope) => {
    console.log("scope: ", scope);
    const links = document.querySelector(scope).getElementsByTagName('a');
    console.log(links, links.length);
} 