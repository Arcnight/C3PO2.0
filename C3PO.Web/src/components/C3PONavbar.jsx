/**
* @jsx React.DOM
*/

'use strict';

var React = require('react');

var Navbar = React.createClass({
    render() {
        return (
            <div className="ui menu">
                <div className="header item">CEBOSS</div>
                <a className="item active">Home</a>
                <a className="item">Boarding</a>
                <div className="item">
                    Search
                    <div className="menu">
                        <a className="item">Basic Search</a>
                        <a className="item">Dhow Search</a>
                        <a className="item">Map Search</a>
                    </div>
                </div>
                <div className="item">
                    Administration
                    <div className="item">
                        <a className="item">User Management</a>
                        <a className="item">Metrics</a>
                        <a className="item">Templates</a>
                    </div>
                </div>
                <div className="item">
                    System
                    <div className="menu">
                        <a className="item">Links</a>
                        <a className="item">Status</a>
                    </div>
                </div>
                <a className="item">Metrics</a>
                <a className="item">Locations</a>
            </div>
        );
    }
});


module.exports = Navbar;