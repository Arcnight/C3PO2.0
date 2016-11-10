﻿import React, { Component } from 'react';
import { push } from 'react-router-redux';
import { Route, IndexRoute } from 'react-router';
import { UserAuthWrapper } from 'redux-auth-wrapper';

import _layout from './layouts/_home.jsx';

import { siteURL } from '../constants/urls';

import dashboard from './pages/dashboard.jsx';
import search from './pages/search.jsx';
import event from './pages/event.jsx';
/*
import dhowsearch from'./pages/dhowsearch';
import mapsearch from'./pages/mapsearch';
import metrics from'./pages/metrics';
import vessel from'./pages/vessel';
import person from'./pages/person';
import company from'./pages/company';
import links from'./pages/links';
import templates from'./pages/templates';
import template from'./pages/template';

            <Route path="dhowsearch" component={dhowsearch} />
            <Route path="mapsearch" component={mapsearch} />
            <Route path="metrics" component={metrics} />
            <Route path="vessel(/:id)" component={vessel} />
            <Route path="person(/:id)" component={person} />
            <Route path="company(/:id)" component={company} />
            <Route path="links" component={links} />
            <Route path="templates" component={templates} />
            <Route path="template(/:id)" component={template} />
*/

/*
const UserIsNotAuthenticated = UserAuthWrapper({
  allowRedirectBack: false,
  authSelector: state => state.user,
  redirectAction: (newLoc) => (dispatch) => {
     dispatch(push(newLoc));
     //dispatch(addNotification({ message: 'Sorry, you are not an administrator' }));
  },
  wrapperDisplayName: 'UserIsNotAuthenticated',
  // Want to redirect the user when they are done loading and authenticated
  predicate: user => !user.isAuthenticated,
  failureRedirectPath: (state, ownProps) => ownProps.location.query.redirect || '/'
});

component={ UserIsNotAuthenticated(login) } 
*/

// Redirects to /login by default
const UserIsAuthenticated = UserAuthWrapper({
    authSelector: state => state.auth, // how to get the user state
    predicate: auth => auth.isAuthenticated,
    wrapperDisplayName: 'UserIsAuthenticated', // a nice name for this auth check
    //redirectAction: push, // the redux action to dispatch for redirect
    redirectAction: (newLoc) => (dispatch) => {
        dispatch(push(newLoc));
        //dispatch(addNotification({ message: 'Sorry, you are not an administrator' }));
    }
});

const getRoutes = (
    <Route component={ UserIsAuthenticated(_layout) }>
        <IndexRoute component={ dashboard } />
        <Route path="search" component={ search } />
        <Route path="event(/:id)" component={ event } />
    </Route>
);

export default getRoutes;