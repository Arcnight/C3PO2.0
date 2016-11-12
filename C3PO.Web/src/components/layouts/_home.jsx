import { Link } from 'react-router';
import { connect } from 'react-redux';
import React, { Component } from 'react';

class Home extends Component {
    render() {
        return (
            <div>
                <div>
                    <ul>
                        <li><Link to="/">Home</Link></li>
                        <li><Link to="/event">Event</Link></li>
                        <li><Link to="/search">Search</Link></li>
                        <li><Link to="/logout">Logout</Link></li>
                    </ul>
                </div>
                <div>
                    { this.props.children }
                </div>
            </div>
        );
    }
}

var mapStateToProps = function(state){
    return { auth: state.auth };
};

export default connect(null)(Home);