import React, { Component } from 'react';
import { Link } from 'react-router';

class Home extends Component {
    componentWillMount() {
//        auth.onChange = function (loggedIn) {
//            this.setState({ isLoggedIn: loggedIn });
//        };
    }

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

        //return this.state.isLoggedIn ?
        //(
        //) :
        //(
        //    <div>
        //        <Login />
        //    </div>
        //);
    }
}

export default Home;