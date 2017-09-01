import * as Utils from 'Features/Details/Assets/Js/Modules/StickySidebar/stickySidebar.utils.js'

const lock = {
    up: {
        HAS_REACHED_TOP: false,
        IS_DURING: false,
        IS_BEFORE: false
    },
    down: {
        HAS_REACHED_BOTTOM: false,
        IS_DURING: false,
        IS_BEFORE: false
    }
}

export function scrollingUp(pin, el, ref, e) {

    const state = {
        STATE: e.target.state(),
        FORWARD: e.target.controller().info('scrollDirection') === 'FORWARD',
        REVERSE: e.target.controller().info('scrollDirection') === 'REVERSE',
        REACHED_TOP: e.scrollPos + ref.siteNavHeight <= Math.abs(document.querySelector('body').getBoundingClientRect().top - ref.wrapper.getBoundingClientRect().top),
        REACHED_BOTTOM: Utils.elementScreenRealEstate('#page-footer').heightInPercentage <= ref.triggerHookDown()       
    }

    if (state.FORWARD) {
        lock.up.HAS_REACHED_TOP = false;
        lock.up.IS_DURING = false;
        lock.up.IS_BEFORE = false;
        // the lock is for debouncing
    }

    if (state.REVERSE) {

        if (process.env.DEBUG) { 
            //console.log("Scence Up:: pinState: " + pin.state + " State: " + state.STATE + " Reached Top: " + state.REACHED_TOP + " Lock Up: ", lock.up)
        }
        if (!state.REACHED_TOP) {
            //if (process.env.DEBUG) { console.log('Up 4') }

            lock.up.HAS_REACHED_TOP = false;
        }

        //Reached Top
        if (state.REACHED_TOP && !lock.up.HAS_REACHED_TOP) {
            if (process.env.DEBUG) { console.log('Up 1') }

            const css = {
                top: 'auto',
                position: 'static',
            }
            Utils.setStylesForElement(el, css)
            lock.up.HAS_REACHED_TOP = true;
            lock.up.IS_DURING = false;
            lock.up.IS_BEFORE = false;
        }
        // The top trigger hasnot  passed the trigger hook
        else if (state.STATE === 'BEFORE' && !lock.up.IS_BEFORE && !state.REACHED_TOP) {
            if (process.env.DEBUG) { console.log('Up 2') }

            const css = {
                top: ref.siteNavHeight +  'px',
                position: 'fixed'
            }
            Utils.setStylesForElement(el, css)
            lock.up.HAS_REACHED_TOP = false;
            lock.up.IS_DURING = false;
            lock.up.IS_BEFORE = true;
        
        }
        // The top trigger has passed the trigger hook
        else if (state.STATE === 'DURING' && !lock.up.IS_BEFORE && !lock.up.IS_DURING) {
            if (process.env.DEBUG) { console.log('Up 3') }

            let css = {}
            if(state.REACHED_BOTTOM) {
                css = {  
                    top: ref.footerCoordinatesTop() - el.offsetHeight  + 'px',
                    position: 'absolute'
                }
            } else {
                css = {  
                    top: Utils.distanceFromStartingPoint(e.target, ref.triggerHookDown(), ref.wrapper, el.offsetHeight , window) + 'px',
                    position: 'absolute',
                }
            }

            Utils.setStylesForElement(el, css)
            lock.up.HAS_REACHED_TOP = false;
            lock.up.IS_DURING = true;
            lock.up.IS_BEFORE = false;

        }

    } 

}

export function scrollingDown(pin, el, ref, e) {

    const state = {
        STATE: e.target.state(),
        FORWARD: e.target.controller().info('scrollDirection') === 'FORWARD', 
        REVERSE: e.target.controller().info('scrollDirection') === 'REVERSE',        
        REACHED_BOTTOM: Utils.elementScreenRealEstate('#page-footer').heightInPercentage <= e.target.triggerHook()
    }

    if (state.REVERSE) {
        lock.down.HAS_REACHED_BOTTOM = false;
        lock.down.IS_DURING = false;
        lock.down.IS_BEFORE = false;
        // the lock is for debouncing
    }

    if (state.FORWARD) {

        // Update Trigger hook when more article height changes
        e.target.triggerHook(Utils.elementScreenRealEstate('.more-articles').heightInPercentage)

        if (process.env.DEBUG) { 
        //console.log("Scence Down:: pinState: " + pin.state , " State: " + state.STATE , " Reached Bottom: " + state.REACHED_BOTTOM, " Lock: ", lock.down ) 
        }
        // We are reversing the lock ere b/c of lazy laoded contnet. If page laods when user is already scrolled at the bottom then it will think its locked,
        // so we check on every update so when the user scrolls is corrects itself
        if (!state.REACHED_BOTTOM) {
            //if (process.env.DEBUG) { console.log('Down 5') }
            
            lock.down.HAS_REACHED_BOTTOM = false;
        }
            
        //Reached footer
        if (state.REACHED_BOTTOM && !lock.down.HAS_REACHED_BOTTOM) {
            if (process.env.DEBUG) { console.log('Down 1') }
               
            const css = {  
                top: ref.footerCoordinatesTop() - el.offsetHeight  + 'px',
                position: 'absolute'
            }
            pin.state = false
            Utils.setStylesForElement(el, css)
            lock.down.HAS_REACHED_BOTTOM = true;
            lock.down.IS_DURING = false;
            lock.down.IS_BEFORE = false;

        }
        // The bottom trigger has passed the trigger hook
        else if (state.STATE === 'DURING' && !lock.down.IS_DURING && !lock.down.HAS_REACHED_BOTTOM) {
            if (process.env.DEBUG) { console.log('Down 2') }

            const css = {
                top: (window.innerHeight - Utils.elementScreenRealEstate('.more-articles').height) - el.offsetHeight + 'px',
                position: 'fixed'
            }
            pin.state = true
            Utils.setStylesForElement(el, css)
            lock.down.HAS_REACHED_BOTTOM = false;
            lock.down.IS_DURING = true;
            lock.down.IS_BEFORE = false;

        }
        else if (state.STATE === 'BEFORE' && el.style.position === 'fixed' && !lock.down.IS_BEFORE ) {
            if (process.env.DEBUG) { console.log('Down 3') }

            const css = {  
                top: Utils.distanceFromStartingPoint(e.target, ref.triggerHookUp(), ref.wrapper, 0 , window) + 'px',
                position: 'absolute'
            }
            pin.state = false
            Utils.setStylesForElement(el, css)
            lock.down.HAS_REACHED_BOTTOM = false;
            lock.down.IS_DURING = false;
            lock.down.IS_BEFORE = true;

        }
    }

}

// ((((sceneDown.controller().info().scrollPos + window.innerHeight)) - (( window.innerHeight - sceneDown.triggerHook()*window.innerHeight)))) - document.querySelector('.scrollmagic-pin-wrapper--aside').getBoundingClientRect().top - document.querySelector('.aside').offsetHeight
export function scrollingSimple(pin, el, ref, e) {

    const state = {
        STATE: e.target.state(),
        FORWARD: e.target.controller().info('scrollDirection') === 'FORWARD',
        REVERSE: e.target.controller().info('scrollDirection') === 'REVERSE',
        REACHED_TOP: e.scrollPos <= ref.startingCoordinatesTop,
        REACHED_BOTTOM: (e.scrollPos + el.getBoundingClientRect().bottom) >= ref.footerCoordinatesTop()
    }

    if (state.FORWARD) {
        lock.up = false; // the lock is for debouncing

        //Reached footer
        if (state.REACHED_BOTTOM) {
            if (process.env.DEBUG) { console.log('simple Down 1') }
               
            const css = {  
                top: ref.footerCoordinatesTop() - el.offsetHeight  + 'px',
                position: 'absolute'
            }
            pin.state = false
            Utils.setStylesForElement(el, css)

        }
        else if (state.STATE === 'DURING' && !state.REACHED_BOTTOM) {
            if (process.env.DEBUG) { console.log('Simple Down 2') }

            const css = {
                top: ref.siteNavHeight + 'px',
                position: 'fixed'
            }
            pin.state = true
            lock.up = true
            Utils.setStylesForElement(el, css)
        
        }
        
    }

    if (state.REVERSE) {

        if (process.env.DEBUG) { console.log("Scence Up Simple :: pinState: " + pin.state + " State: " + state.STATE + " Reached Top: " + state.REACHED_TOP + " Lock Up: " + lock.up)}

        //Reached Top
        if (state.REACHED_TOP) {
            if (process.env.DEBUG) { console.log('Simple Up 1') }

            const css = {
                top: 'auto',
                position: 'static',
            }
            pin.state = false
            lock.up = false
            Utils.setStylesForElement(el, css)
        }
        // The top trigger has passed the trigger hook
        else if (pin.state && state.STATE === 'DURING' && !lock.up) {
            if (process.env.DEBUG) { console.log('Simple Up 2') }

            const css = {  
                //top: Utils.distance(ref.wrapper.getBoundingClientRect().top, el.getBoundingClientRect().top, ref.startingCoordinatesTop) + 'px',
                position: 'absolute',
            }
            pin.state = false
            Utils.setStylesForElement(el, css)

        }
        // The top trigger has not passed the trigger hook
        else if (state.STATE === 'BEFORE' && !state.REACHED_TOP && !lock.up) {
            if (process.env.DEBUG) { console.log('Simple Up 3') }

            const css = {
                top: ref.siteNavHeight + 'px',
                position: 'fixed'
            }
            pin.state = true
            lock.up = true
            Utils.setStylesForElement(el, css)
        
        }
    } 

}
            
  





