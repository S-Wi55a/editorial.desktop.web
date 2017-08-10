import ScrollMagic from 'ScrollMagic';
import 'debug.addIndicators';

export default function(d, w, selector) {
    const aside = selector;
    if (aside) {

        // Load Rezise sensor
        const ResizeSensor = require('css-element-queries/src/ResizeSensor');

        let pinState = false
        let triggerHookValueCache = null

        const spacer = aside.parentNode.insertBefore(d.createElement('div'), aside)
        spacer.classList.add('scrollmagic-pin-spacer--aside');
        spacer.appendChild(aside);
        
        const references = {
            spacer,
            startingCoordinatesTop : () => { return spacer.getBoundingClientRect().top - d.querySelector('body').getBoundingClientRect().top },
            calcTriggerHookFromTop: (x) => 1 - (window.innerHeight - x)/window.innerHeight,
            siteNavHeight: d.querySelector('.site-nav-wrapper').offsetHeight,
            footerCoordinatesTop : () => { return d.querySelector('#page-footer').getBoundingClientRect().top - d.querySelector('body').getBoundingClientRect().top  }
        }
         
        // Set scene & controller
        w.scrollMogicController = w.scrollMogicController || new ScrollMagic.Controller();

        let stickySidebarScene = new ScrollMagic.Scene({
                triggerElement: spacer,
                triggerHook: (window.innerHeight - setTriggerHookValue())/window.innerHeight,
                offset: d.querySelector('body').offsetHeight // We do this to prevent the scene starting early
            })
            .on('update', (e)=>onUpdate(aside, references, e)) 
            .addIndicators({name: "Sticky Nav", colorEnd: "#FF0000"})
            .addTo(w.scrollMogicController);

        w.stickySidebarScene = stickySidebarScene //TODO: remove


        new ResizeSensor(aside, function() {
            stickySidebarScene.offset(aside.offsetHeight)
        });


        function onUpdate(el, r, e) {

            const SCENE = e.target
            const STATE = SCENE.state()
            const FORWARD = SCENE.controller().info('scrollDirection') === 'FORWARD'
            const REVERSE = !FORWARD
            const EL_COORDINATES = el.getBoundingClientRect()
            const left = r.spacer.getBoundingClientRect().left + 'px'
            const REACHED_BOTTOM = (e.scrollPos + EL_COORDINATES.bottom) >= r.footerCoordinatesTop()

            //We don't know when the more-article is shown or hidden but we can check the height on scroll
            if (FORWARD && !REACHED_BOTTOM) {

                //If the trigger hook changed, on next scroll set top pos
                if (SCENE.triggerHook() !== triggerHookValueCache) {
                    if (pinState) {
                        const css = {
                            top: (window.innerHeight - setTriggerHookValue()) - el.offsetHeight + 'px',
                            transition: 'top 0.333s'
                        }
                        setStyles(el, css)  
                    }
                    triggerHookValueCache = (window.innerHeight - setTriggerHookValue())/window.innerHeight
                }
                //Change Trigger Hook position, on happens in forward direction
                SCENE.triggerHook((window.innerHeight - setTriggerHookValue())/window.innerHeight)

            }
            
            if(el.style.position === 'static') {setStyles(el, {left})} //this ensures that when the window is resized the sidebar moves w/ it
            
            
            console.log(pinState, STATE, FORWARD, triggerHookValueCache)


            
            if (!pinState && STATE === 'DURING' && FORWARD && !REACHED_BOTTOM ) {
                console.log('1')
                pinState = true
                const css = {
                    top: (window.innerHeight - setTriggerHookValue()) - el.offsetHeight + 'px',
                    position: 'fixed',
                    transition: ''
                }
                setStyles(el, css)
            } 
            else if (pinState && STATE === 'DURING' && REVERSE && el.style.position === 'fixed') {
                console.log('2')

                const top = EL_COORDINATES.top
                pinState = false
                const css = {  
                    top: distance(r.spacer.getBoundingClientRect().top, top, r.startingCoordinatesTop()) + 'px',
                    position: 'absolute',
                    transition: ''

                }
                setStyles(el, css)
                SCENE.offset(top - r.spacer.getBoundingClientRect().top)
                SCENE.triggerHook(r.calcTriggerHookFromTop(r.siteNavHeight))
            }
            else if (!pinState && STATE === 'BEFORE' && REVERSE && el.style.position === 'absolute') {
                console.log('3')

                pinState = true
                const css = {
                    top: r.siteNavHeight + 'px',
                    position: 'fixed',
                    transition: ''
                }
                setStyles(el, css)
            }
            else if (pinState && STATE === 'BEFORE' && FORWARD && el.style.position === 'fixed') {
                console.log('4')

                const top = EL_COORDINATES.top
                pinState = false
                const css = {  
                    top: distance(r.spacer.getBoundingClientRect().top, top, r.startingCoordinatesTop()) + 'px',
                    position: 'absolute',
                    transition: ''
                }
                setStyles(el, css)
                SCENE.offset(top - r.spacer.getBoundingClientRect().top + el.offsetHeight)
                SCENE.triggerHook((window.innerHeight - setTriggerHookValue())/window.innerHeight)
            }
            //If starting Pos is reached
            else if (pinState && e.scrollPos < r.startingCoordinatesTop() - r.siteNavHeight) {
                console.log('5')

                pinState = false
                const css = {
                    top: 'auto',
                    position: 'static',
                    transition: ''
                }
                setStyles(el, css)
                SCENE.offset(el.offsetHeight)
                SCENE.triggerHook((window.innerHeight - setTriggerHookValue())/window.innerHeight)
            }
            //If footer is reached
            else if (pinState && REACHED_BOTTOM ) {
                console.log('6')
                pinState = false
                const css = {  
                    top: r.footerCoordinatesTop() - el.offsetHeight  + 'px',
                    position: 'absolute',
                    transition: ''
                }
                setStyles(el, css)
                SCENE.offset(r.footerCoordinatesTop() - el.offsetHeight - r.startingCoordinatesTop())
                SCENE.triggerHook(r.calcTriggerHookFromTop(r.siteNavHeight))
            }
        }

        function distance(startingPos, currentPos, offset) {
            return (currentPos + offset) - startingPos
        }

        function setStyles(el, css) {
            for (var prop in css) {
                if (css.hasOwnProperty(prop)) {
                    el.style[prop] = css[prop]
                } 
            }
        }

        function setTriggerHookValue() {
            //check if more articles is loaded if not use default values
            const moreArticles = document.querySelector('.more-articles')
            return  moreArticles ? Math.abs((moreArticles.getBoundingClientRect().top - window.innerHeight)) : 50 ;
        }
    }
}

Math.abs((document.querySelector('#page-footer').getBoundingClientRect().top - window.innerHeight))