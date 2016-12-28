var path = require('path');
var webpack = require('webpack');
var HtmlWebPackPlugin = require('html-webpack-plugin');

const jsSrcDir = 'jssrc';

var APP_DIR = path.resolve(__dirname, jsSrcDir);
//var BUILD_DIR = path.resolve(__dirname, 'jsbuild');
var BUILD_DIR = path.join(__dirname, 'wwwroot');

var config = {
    entry: {
        client: [ APP_DIR + '/client.jsx' ],
        server: [ APP_DIR + '/server.jsx' ]
    },
    output: {
        path: BUILD_DIR,
        publicPath: '/',
        filename: '[name].js',
        libraryTarget: 'this'
        //filename: '[name].[hash].js'
    },
    plugins: [new HtmlWebPackPlugin({
        title: 'C3PO',
        filename: BUILD_DIR + '/index.html',
        template: APP_DIR + '/templates/index.ejs'
    })],
    devtool: 'source-map',
    devServer: {
        contentBase: BUILD_DIR
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
    },
    resolve: {
        extensions: ['', '.js', '.jsx'],
        alias: {
            Utils: path.join(__dirname, jsSrcDir, 'utils'),
            Stores: path.join(__dirname, jsSrcDir, 'stores'),
            Actions: path.join(__dirname, jsSrcDir, 'actions'),
            Reducers: path.join(__dirname, jsSrcDir, 'reducers'),
            Constants: path.join(__dirname, jsSrcDir, 'constants'),
            Components: path.join(__dirname, jsSrcDir, 'components'),
            Containers: path.join(__dirname, jsSrcDir, 'components', 'containers')
        }
    }
};

module.exports = config;