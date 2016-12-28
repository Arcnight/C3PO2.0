import React from 'react';
import { render } from 'react-dom'
import { browserHistory } from 'react-router';
import { syncHistoryWithStore } from 'react-router-redux';

import shivIE from 'shivie8';

import { configureStore } from 'Stores';
import { getProvider } from 'Components';

shivIE(document);

const store = configureStore(window.__data, browserHistory);

render(getProvider(syncHistoryWithStore(browserHistory, store), store), document.getElementById('root'));