import serialize from 'serialize-javascript';
import { renderToString } from 'react-dom/server';
import React, { Component, PropTypes } from 'react';

class Html extends Component {
    render() {
        return (
            <html lang="en-us">
                <head>
                    <title>React Webpack Sass Sample</title>
                    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
                </head>
                <body>
                    <div id="root" />
                    <script type="text/javascript" dangerouslySetInnerHTML={{ __html: 'window.__data = ' + serialize(this.props.store.getState()) + ';' }} />
                    <script type="text/javascript" src="client.js"></script>
                </body>
            </html>
        );
    }
};

Html.propTypes = {
    component: PropTypes.node,
    store: PropTypes.object
};

export default Html;