import * as Utils from 'Features/Details/Assets/Js/Modules/StickySidebar/stickySidebar.utils.js'

// Locks are to prevent calling a bloack of code when ina current state
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

export function scrollingUp(el, ref, e) {

    const state = {
        STATE: e.target.state(),
        FORWARD: e.target.controller().info('scrollDirection') === 'FORWARD',
        REVERSE: e.target.controller().info('scrollDirection') === 'REVERSE',
        REACHED_TOP: e.scrollPos + ref.siteNavHeight <= Math.abs(document.querySelector('body').getBoundingClientRect().top - ref.wrapper.getBoundingClientRect().top),
        REACHED_BOTTOM: Utils.elementScreenRealEstate(ref.pageFooter).heightInPercentage <= ref.triggerHookDown       
    }

    if (state.FORWARD) {
        lock.up.HAS_REACHED_TOP = false;
        lock.up.IS_DURING = false;
        lock.up.IS_BEFORE = false;
        // the lock is for debouncing
    }

    if (state.REVERSE) {

        if (!state.REACHED_TOP) {
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
        else if (state.STATE === 'DURING' && !lock.up.IS_BEFORE && !lock.up.IS_DURING && !lock.up.HAS_REACHED_TOP) {
            if (process.env.DEBUG) { console.log('Up 3') }

            let css = {}
            if(state.REACHED_BOTTOM) {
                css = {  
                    top: ref.footerCoordinatesTop() - el.offsetHeight  + 'px',
                    position: 'absolute'
                }
            } else {
                css = {  
                    top: Utils.distanceFromStartingPoint(e.target, el.getBoundingClientRect().bottom , ref.wrapper, el.offsetHeight) + 'px',
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

export function scrollingDown(el, ref, e) {

    const state = {
        STATE: e.target.state(),
        FORWARD: e.target.controller().info('scrollDirection') === 'FORWARD', 
        REVERSE: e.target.controller().info('scrollDirection') === 'REVERSE',        
        REACHED_BOTTOM: Utils.elementScreenRealEstate(ref.pageFooter).heightInPercentage <= e.target.triggerHook()
    }

    if (state.REVERSE) {
        lock.down.HAS_REACHED_BOTTOM = false;
        lock.down.IS_DURING = false;
        lock.down.IS_BEFORE = false;
        // the lock is for debouncing
    }

    if (state.FORWARD) {

        // We are reversing the lock ere b/c of lazy laoded contnet. If page laods when user is already scrolled at the bottom then it will think its locked,
        // so we check on every update so when the user scrolls is corrects itself
        if (!state.REACHED_BOTTOM) {            
            lock.down.HAS_REACHED_BOTTOM = false;
        }
            
        //Reached footer
        if (state.REACHED_BOTTOM) {
            if (process.env.DEBUG) { console.log('Down 1') }
               
            const css = {  
                top: ref.footerCoordinatesTop() - el.offsetHeight  + 'px',
                position: 'absolute'
            }
            Utils.setStylesForElement(el, css)
            lock.down.HAS_REACHED_BOTTOM = true;
            lock.down.IS_DURING = false;
            lock.down.IS_BEFORE = false;

        }
        // The bottom trigger has passed the trigger hook
        else if (state.STATE === 'DURING' && !lock.down.IS_DURING && !lock.down.HAS_REACHED_BOTTOM) {
            if (process.env.DEBUG) { console.log('Down 2') }

            const css = {
                top: (window.innerHeight - 51) - el.offsetHeight + 'px',
                position: 'fixed'
            }
            Utils.setStylesForElement(el, css)
            lock.down.HAS_REACHED_BOTTOM = false;
            lock.down.IS_DURING = true;
            lock.down.IS_BEFORE = false;

        }
        else if (state.STATE === 'BEFORE' && el.style.position === 'fixed' && !lock.down.IS_BEFORE ) {
            if (process.env.DEBUG) { console.log('Down 3') }

            const css = {  
                top: Utils.distanceFromStartingPoint(e.target, window.innerHeight - (window.innerHeight - ref.triggerHookUp()*window.innerHeight) , ref.wrapper, 0) + 'px',
                position: 'absolute'
            }
            Utils.setStylesForElement(el, css)
            lock.down.HAS_REACHED_BOTTOM = false;
            lock.down.IS_DURING = false;
            lock.down.IS_BEFORE = true;

        }
    }

}

export function scrollingSimple(el, ref, e) {

    const state = {
        STATE: e.target.state(),
        FORWARD: e.target.controller().info('scrollDirection') === 'FORWARD',
        REVERSE: e.target.controller().info('scrollDirection') === 'REVERSE',
        REACHED_TOP: e.scrollPos + ref.siteNavHeight <= Math.abs(document.querySelector('body').getBoundingClientRect().top - ref.wrapper.getBoundingClientRect().top),
        REACHED_BOTTOM: e.scrollPos + el.offsetHeight >= Math.abs(document.querySelector('body').getBoundingClientRect().top - document.querySelector('#page-footer').getBoundingClientRect().top)     
    }

    if (state.FORWARD) {

        lock.up.HAS_REACHED_TOP = false;
        lock.up.IS_DURING = false;
        lock.up.IS_BEFORE = false;

        if (!state.REACHED_BOTTOM) {
            lock.down.HAS_REACHED_BOTTOM = false;
        }

        //Reached footer
        if (state.REACHED_BOTTOM && !lock.down.HAS_REACHED_BOTTOM) {
            if (process.env.DEBUG) { console.log('simple Down 1') }
               
            const css = {  
                top: ref.footerCoordinatesTop() - el.offsetHeight  + 'px',
                position: 'absolute'
            }
            Utils.setStylesForElement(el, css)
            lock.down.HAS_REACHED_BOTTOM = true;
            lock.down.IS_DURING = false;

        }
        else if (state.STATE === 'DURING' && !lock.down.IS_DURING && !lock.down.HAS_REACHED_BOTTOM) {
            if (process.env.DEBUG) { console.log('Simple Down 2') }

            const css = {
                top: ref.siteNavHeight + 'px',
                position: 'fixed'
            }
            Utils.setStylesForElement(el, css)
            lock.down.HAS_REACHED_BOTTOM = false;
            lock.down.IS_DURING = true;
        
        }
        
    }
    
    if (state.REVERSE) {

        lock.down.HAS_REACHED_BOTTOM = false;
        lock.down.IS_DURING = false;

        if (!state.REACHED_TOP) {
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
                    top: Utils.distanceFromStartingPoint(e.target, ref.triggerHookDown, ref.wrapper, el.offsetHeight , window) + 'px',
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
            
  





