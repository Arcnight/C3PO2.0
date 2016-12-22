import ReactDOM from 'react-dom/server';
import React, { PropTypes } from 'react';
import serialize from 'serialize-javascript';

const Html = ({ component, store }) => {
    const _content = component ? ReactDOM.renderToString(component) : '';
    //<link rel="stylesheet" href="/dist/style.css" />
    //<link rel="shortcut icon" href="/favicon.ico" />

    return (
        <html lang="en-us">
            <head>
                <title>React Webpack Sass Sample</title>
                <meta name="viewport" content="width=device-width, initial-scale=1.0" />
            </head>
            <body>
                <div id="content" dangerouslySetInnerHTML={{ __html: _content }} />
                <script dangerouslySetInnerHTML={{ __html: 'window.__data=${serialize(store.getState())};' }} />
                <script src="client.js"></script>
            </body>
        </html>
    );
};

Html.propTypes = {
    component: PropTypes.node,
    store: PropTypes.object
};

export default Html;