import React from 'react'

// This coponent is meant to be sued in conjuntion with tranistions compoents
// from the react-transition-group package

//TODO: add exist timers
export default class Timer extends React.Component {
    constructor() {
        super()
        this.state = { 
            in: false
        }
    }

    componentDidMount() {
        this.timer = setTimeout(() => {
            this.setState({ in: true });
        }, this.props.delay || 0);
    }

    componentWillUnmount() {
        clearTimeout(this.timer);
    }
    
    render() {
        return <span>{ React.cloneElement(this.props.children, { in: this.state.in }) }</span>
    }
}