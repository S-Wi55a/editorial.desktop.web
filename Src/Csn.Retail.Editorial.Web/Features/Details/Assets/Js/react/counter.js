import React from 'react';

class Counter extends React.Component {
    constructor(props) {
        super(props);

        this.state = { amount: 0 };
    }
    render() {
        return (
            <div>
                <span className="fa fa-hand-spock-o fa-1g">
                    Amount: {this.state.amount}
                </span>
                <button onClick={() => this.setState(addOne)}>
                    Add teo
        </button>
            </div>
        );
    }
}

const addOne = ({ amount }) => ({ amount: amount + 2 });

export default Counter;