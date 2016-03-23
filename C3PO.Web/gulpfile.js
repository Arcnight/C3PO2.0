// Gulp + Browserify recipe
// ------------------------
// Includes react JSX, coffeescript, uglify & sourcemaps
// Supports multiple input & output files

var gulp = require('gulp');
var clean = require('gulp-clean');
var uglify = require('gulp-uglify');
var sourcemaps = require('gulp-sourcemaps');
var browserify = require('browserify');
//var coffeeify = require('coffeeify');
var reactify = require('reactify');
var source = require('vinyl-source-stream');
var buffer = require('vinyl-buffer');

const SCRIPTS_FOLDER = 'Resources/scripts/';

var paths = {
    src: SCRIPTS_FOLDER + 'src/',
    build: SCRIPTS_FOLDER + 'build/'
};

// Config that can be loaded externally, similar
// to [gulp-starter](https://github.com/greypants/gulp-starter)

var config = {
    javascript: {
        src: paths.src + '**/*.{js,jsx}', //'**/*.{js,jsx,coffee}'
        rootFiles: [
            {
                src: paths.src + 'client.jsx',
                dest: 'client.js'
            },
            {
                src: paths.src + 'server.jsx',
                dest: 'server.js'
            }
            //,{
            //    src: src + '/javascript_app/test.coffee',
            //    dest: 'coffee.js'
            //}
        ],
        dest: paths.build
    }
};

var build = function () {
    var files = config.javascript.rootFiles;

    files.forEach(function (file) {
        var b = browserify({
            entries: file.src, // Only need initial file, browserify finds the deps
            debug: true        // Enable sourcemaps
        });

        //b.transform(coffeeify); // Convert Coffeescript
        b.transform(reactify);  // Convert JSX

        b.bundle()
          .pipe(source(file.dest))
          .pipe(buffer())

          .pipe(sourcemaps.init({ loadMaps: true }))
          .pipe(uglify())
          .pipe(sourcemaps.write())

          .pipe(gulp.dest(config.javascript.dest));
    });
};

gulp.task('build', build);
gulp.task('clean', function () { return gulp.src([paths.build + '*'], { read: false }).pipe(clean()); });

gulp.task('default', ['clean'], build);