import React from 'react'

interface ICarouselNavButton {
    text: string
}

export default class NavButton extends React.Component<ICarouselNavButton> {
    render() {
        return <button {...this.props} data-webm-clickvalue={this.props.text}>{this.props.text}</button>
    }
}
