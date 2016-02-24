/*
 *  Copyright (c) 2015, Facebook, Inc.
 *  All rights reserved.
 *
 *  This source code is licensed under the BSD-style license found in the
 *  LICENSE file in the root directory of this source tree. An additional grant 
 *  of patent rights can be found in the PATENTS file in the same directory.
 */

var ROOT_DIR = 'Resources/scripts';
var OUTPUT_DIR = ROOT_DIR + '/build';

var paths = {
    src: {
        jsx: ROOT_DIR + '/src/*.jsx',
        app: ROOT_DIR + '/app/main.js',
        scripts: ROOT_DIR + '/**/*.js'
    },
    dest: {
        bundles: OUTPUT_DIR + '/dist',
        bundlesFilter: '!' + OUTPUT_DIR + '/dist/**/*.js',
        serverBundle: 'serverBundle.js',
        clientBundle: 'clientBundle.js',
        jsx: OUTPUT_DIR + '/app/Components'
    }
};

var gulp = require("gulp");
var gulpReact = require('gulp-react');
var source = require('vinyl-source-stream');
var streams = require('memory-streams');
var CombinedStream = require('combined-stream');
var os = require('os');
var browserify = require('browserify');
var babelify = require("babelify");

var createServerBundle = function (browserify, configPath) {
    var utils = {
        parseConfig: function (config) {
            if (config) {
                if (config.expose) {
                    var components = {};
                    //1. parse the configuration
                    config.expose.forEach(function (component) {
                        var path, name;

                        if (typeof component === 'string') {
                            path = component;
                        } else {
                            path = component.path;
                            if (component.name) name = component.name;
                        }

                        if (name === undefined) {
                            var splitted = path.split('/');
                            name = splitted[splitted.length - 1];
                        }

                        components[name] = path;
                    });

                    return components;
                }
            }
        },

        exposeReact: function (exposedVariables, requires) {
            requires.push({ file: "react" });
            exposedVariables.append('var React = require("react");' + os.EOL);
        }
    };

    if (configPath === undefined) configPath = './reactServerConfig.json';

    var config = require(configPath);
    var serverComponents = utils.parseConfig(config);

    browserify.transform(babelify, { presets: ['es2015', 'react'] });

    if (serverComponents) {
        var exposedVariables = CombinedStream.create();
        var requires = [];

        exposedVariables.append(';' + os.EOL);
        utils.exposeReact(exposedVariables, requires);

        for (var name in serverComponents) {
            var path = serverComponents[name];
            requires.push({ file: path, expose: name });
            exposedVariables.append('var ' + name + ' = require("' + name + '");');
        }

        browserify.require(requires);

        var bundleStream = CombinedStream.create();
        bundleStream.append(browserify.bundle());
        bundleStream.append(exposedVariables);

        return bundleStream;
    }
};

var gulpServerBundle = function () {
    var bundle = createServerBundle(browserify({ extensions: ['.jsx', '.js'] }));

    return bundle
      .pipe(source(paths.dest.serverBundle))
      .pipe(gulp.dest(paths.dest.bundles));
};

gulp.task('server-build', function () { return gulpServerBundle(); });

var gulpClientBundle = function () {
    var bundle = createServerBundle(browserify(paths.src.app, { extensions: ['.jsx', '.js'] }));

    return bundle
      .pipe(source(paths.dest.clientBundle))
      .pipe(gulp.dest(paths.dest.bundles));
};

gulp.task('client-build', function () { return gulpClientBundle(); });

gulp.task('full-build', ['server-build', 'client-build']);

//gulp.task('react', function () {
//    return gulp.src(paths.src.jsx)
//      .pipe(gulpReact())
//      .pipe(gulp.dest(paths.dest.jsx));
//});

/*
var gulp = require('gulp');
var named = require('vinyl-named');
var webpack = require('webpack');
var webpackStream = require('webpack-stream');
var uglify = require('gulp-uglify');

gulp.task('default', ['build']);
gulp.task('build', ['build-react-dev', 'build-deps-prod']);

gulp.task('build-react-dev', function() {
	return gulp.src('Resources/react.js')
		.pipe(webpackStream({
			output: {
				filename: 'react.generated.js',
				libraryTarget: 'this'
			},
			plugins: [
				new webpack.DefinePlugin({
					'process.env.NODE_ENV': '"development"'
				}),
				new webpack.optimize.OccurenceOrderPlugin(),
				new webpack.optimize.DedupePlugin()
			]
		}))
		.pipe(gulp.dest(OUTPUT_DIR));
});

gulp.task('build-deps-prod', function () {
	return gulp.src(['Resources/react.js', 'Resources/babel.js'])
		.pipe(named())
		.pipe(webpackStream({
			module: {
				loaders: [
					{
						exclude: /node_modules/,
						test: /\.js$/,
						loader: 'babel',
						query: {
							presets: ['es2015', 'stage-0']
						}
					},
				]
			},
			output: {
				filename: '[name].generated.min.js',
				libraryTarget: 'this'
			},
			plugins: [
				new webpack.DefinePlugin({
					'process.env.NODE_ENV': '"production"'
				}),
				new webpack.optimize.OccurenceOrderPlugin(),
				new webpack.optimize.DedupePlugin()
			]
		}))
		.pipe(uglify())
		.pipe(gulp.dest(OUTPUT_DIR));
});
*/