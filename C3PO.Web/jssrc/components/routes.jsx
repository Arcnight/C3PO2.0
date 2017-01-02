import React, { Component } from 'react';
import { push } from 'react-router-redux';
import { UserAuthWrapper } from 'redux-auth-wrapper';
import { Route, IndexRoute, Redirect } from 'react-router';

import * as pages from './pages';
import main from 'Containers/main.jsx';
//import { SITEURL } from 'Constants/urls.js';
import authenticated from 'Containers/authenticated.jsx';
/*

    <Route path="dhowsearch" component={ pages.dhowsearch } />
    <Route path="mapsearch" component={ pages.mapsearch } />
    <Route path="metrics" component={ pages.metrics } />
    <Route path="vessel(/:id)" component={ pages.vessel } />
    <Route path="person(/:id)" component={ pages.person } />
    <Route path="company(/:id)" component={ pages.company } />
    <Route path="links" component={ pages.links } />
    <Route path="templates" component={ pages.templates } />
    <Route path="template(/:id)" component={ pages.template } />
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
    redirectAction: push,
    authSelector: state => state.auth, // how to get the user state
    predicate: auth => auth.isAuthenticated,
    wrapperDisplayName: 'UserIsAuthenticated', // a nice name for this auth check
});

const routes = (
    <Route path="/" component={ main }>
        <Route component={ UserIsAuthenticated(authenticated) }>
            <IndexRoute component={ pages.dashboard } />
            <Route path="search" component={ pages.search } />
            <Route path="event(/:id)" component={ pages.event } />
        </Route>
        <Route path="login" component={ pages.login } />
        <Redirect from='logout' to='login' />
    </Route>
);

export default routes;