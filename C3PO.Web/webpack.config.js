var path = require('path');
var webpack = require('webpack');
var HtmlWebPackPlugin = require('html-webpack-plugin');

var APP_DIR = path.resolve(__dirname, 'jssrc');
var BUILD_DIR = path.resolve(__dirname, 'jsbuild');

var config = {
    entry: APP_DIR + '/index.jsx',
    output: {
        path: BUILD_DIR,
        filename: '[name].[hash].js'
    },
    plugins: [new HtmlWebPackPlugin({
        title: 'C3PO',
        filename: BUILD_DIR + '/index.html',
        template: APP_DIR + '/templates/index.ejs'
    })],
    devtool: 'source-map',
    devServer: {
        contentBase: 'jsbuild'
    },
    module: {
        loaders: [
            {
                test: /\.jsx?/,
                loader: 'babel',
                include: APP_DIR
            },
            {
                test: /\.css/,
                loader: 'babel',
                include: APP_DIR
            }
        ]
    }
};

module.exports = config;