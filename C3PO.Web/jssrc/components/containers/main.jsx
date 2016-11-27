import React, { Component } from 'react';

import shivIE from 'shivie8';

shivIE(document);

//import logo from './logo.svg';
//import './App.css';
//import './index.css';
//  <img src={logo} className="App-logo" alt="logo" />

class Main extends Component {
  render() {
    return (
      <div className="App">
        <div className="App-header">
          <h2>C3PO</h2>
        </div>
        <div>        
          { this.props.children }
        </div>
      </div>
    );
  }
};

export default Main;