import React from 'react';
import PropTypes from 'prop-types';
import invariant from 'invariant';
import { connect } from 'react-redux';
import hoistNonReactStatics from 'hoist-non-react-statics';
import { injectAsyncReducer } from 'Redux/Global/Reducers'


export default (Config) => (Component) => {
    const defaultMapStateToProps = (state, props) => {
        const compKey = typeof Config.key === 'function' ? Config.key(props) : Config.key    
        return state[compKey]
    };
        
    const ConnectComp = connect(
        Config.mapStateToProps || defaultMapStateToProps,
        Config.mapDispatchToProps,
        Config.mergeProps)((props) => {
            const newProps = Object.assign({}, props);
            return (<Component {...newProps} />);
        });

  class UI extends React.Component {
    constructor(props, context) {
      super(props, context)
      const compKey = typeof Config.key === 'function' ? Config.key(props, context) : Config.key
      const compState = typeof Config.state === 'function' ? Config.state(props, context) : Config.state      
      const componentRootReducer = Config.reducer

      invariant(Config.key, '[redux-ui-state] - You must supply a key to the component either as a function or string')
      invariant(Config.reducer, '[redux-ui-state] - You must supply a reducer function to the componenet')      
      
      this.componentRootReducer = componentRootReducer
      this.persist = Config.persit || false
      this.store = this.context.store      
      this.compState = compState
      this.compKey = compKey
    }
    componentWillMount() {
      const existingState = this.store.getState()[this.compKey]

      // Add the reduce and update the store
      injectAsyncReducer(this.store, this.compKey, this.componentRootReducer(this.persist ? existingState : this.compState));
      
    }

    componentWillUnmount() {
       !this.persist && injectAsyncReducer(this.store, this.compKey, this.componentRootReducer({}));
    }
    render() {
      return <ConnectComp {...this.props}/>
    }
  }
  UI.contextTypes = Object.assign({}, Component.contextTypes, {
    store: PropTypes.shape({
      subscribe: PropTypes.func.isRequired,
      dispatch: PropTypes.func.isRequired,
      getState: PropTypes.func.isRequired,
    }),
  });
  const displayName = Component.displayName || Component.name || 'Component';
  UI.displayName = `UI(${displayName})`;
  return hoistNonReactStatics(UI, Component);
};