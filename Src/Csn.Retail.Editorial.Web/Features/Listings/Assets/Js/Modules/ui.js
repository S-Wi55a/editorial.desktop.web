import React from 'react';
import invariant from 'invariant';
import hoistNonReactStatics from 'hoist-non-react-statics';
import { connect } from 'react-redux';
import { injectAsyncReducer } from 'Redux/Global/Store/store.server.js'


export default (Config) => (Component) => {

  // TODO: make config where you can add reducer as func for init state or reducer w/o
  const componentRootReducer = Config.reducer

  class UI extends React.Component {
    constructor(props, context) {
      super(props, context)
      const compKey = typeof Config.key === 'function' ? Config.key(props, context) : Config.key
      const compState = typeof Config.state === 'function' ? Config.state(props, context) : Config.state      
      this.store = this.context.store
      invariant(Config.key, '[redux-ui-state] - You must supply a key to the component either as a function or string')
      invariant(Config.reducer, '[redux-ui-state] - You must supply a reducer function to the componenet')      
      
      this.compState = compState
      this.compKey = compKey
      this.unsubscribe = null
    }
    componentWillMount() {
      const existingState = this.store.getState()[this.compKey]
      //this.componentCleanup = this.store.cleanup; // TODO: change to component unmount callback

      //NOTE: need to add reducer her so that dispatch can be handled
      injectAsyncReducer(this.store, this.compKey, componentRootReducer(Config.persit ? existingState : this.compState));
      

      // Let us know the store has been attached
      this.store.dispatch({
        type: 'COMPONENT_ADDED',
      });
    }

    componentWillUnmount() {
      //const persist = typeof Config.persist === 'function' ? Config.persist(this.props, this.context) : Config.persist;
      setTimeout(() => {
        this.store.dispatch({
          type: 'DESTORY_COMPONENT'
        });
        // if (this.storeCleanup) {
        //   this.storeCleanup();
        // }
        // TODO: Add functionality to remove reducer by filtering out asyn reducer and apply replace reducer again
      }, 0);
    }
    render() {
      return <Component {...this.props}/>
    }
  }
  UI.contextTypes = Object.assign({}, Component.contextTypes, {
    store: React.PropTypes.shape({
      subscribe: React.PropTypes.func.isRequired,
      dispatch: React.PropTypes.func.isRequired,
      getState: React.PropTypes.func.isRequired,
    }),
  });
  const displayName = Component.displayName || Component.name || 'Component';
  UI.displayName = `UI(${displayName})`;
  return UI
};