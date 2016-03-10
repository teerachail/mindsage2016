/// <binding AfterBuild='build' Clean='clean' />
"use strict";

var $ = require('gulp-load-plugins')();
var argv = require('yargs').argv;
//var gulp = require('gulp');
//var rimraf = require('rimraf');
var router = require('front-router');
var sequence = require('run-sequence');

var ts = require('gulp-typescript');
var sourcemaps = require('gulp-sourcemaps');
var autoprefixer = require('gulp-autoprefixer');

var gulp = require("gulp"),
    rimraf = require("rimraf"),
    //concat = require("gulp-concat"),
    //cssmin = require("gulp-cssmin"),
    uglify = require("gulp-uglify");

// Check for --production flag
var isProduction = !!(argv.production);

// Ref tsconfig.json for typescript compiling
var tsProject = ts.createProject('scripts/tsconfig.json');


var paths = {
    webroot: "./wwwroot/",
    assets: [
      './client/**/*.*',
      '!./client/templates/**/*.*',
      '!./client/assets/{scss,js}/**/*.*'
    ],
    // Sass will check these folders for files when you use @import.
    sass: [
      'client/assets/scss',
      'wwwroot/lib/foundation-apps/scss'
    ],
    // These files include Foundation for Apps and its dependencies
    foundationJS: [
      'wwwroot/lib/fastclick/lib/fastclick.js',
      'wwwroot/lib/viewport-units-buggyfill/viewport-units-buggyfill.js',
      'wwwroot/lib/tether/tether.js',
      'wwwroot/lib/hammerjs/hammer.js',
      'wwwroot/lib/angular/angular.js',
      'wwwroot/lib/angular-animate/angular-animate.js',
      'wwwroot/lib/angular-ui-router/release/angular-ui-router.js',
      'wwwroot/lib/foundation-apps/js/vendor/**/*.js',
      'wwwroot/lib/foundation-apps/js/angular/**/*.js',
      '!wwwroot/lib/foundation-apps/js/angular/app.js'
    ],
    // These files are for your app's JavaScript
    appJS: [
      'client/assets/js/app.js'
    ]
};

paths.js = paths.webroot + "js/**/*.js";
paths.minJs = paths.webroot + "js/**/*.min.js";
paths.css = paths.webroot + "css/**/*.css";
paths.minCss = paths.webroot + "css/**/*.min.css";
paths.concatJsDest = paths.webroot + "js/site.min.js";
paths.concatCssDest = paths.webroot + "css/site.min.css";
//Add new paths
paths.genTmpl = paths.webroot + "templates";
paths.genAssets = paths.webroot + "assets";
paths.genJsDest = paths.webroot + "assets/js";
paths.genCssDest = paths.webroot + "assets/css";

paths.genRouteJs = paths.genJsDest + "/routes.js";

gulp.task("clean:js", function (cb) {
    rimraf(paths.concatJsDest, cb);
});

gulp.task("clean:css", function (cb) {
    rimraf(paths.concatCssDest, cb);
});

gulp.task("clean:tmpl", function (cb) {
    rimraf(paths.genTmpl, cb);
});

gulp.task("clean:assets", function (cb) {
    rimraf(paths.genAssets, cb);
});

gulp.task("clean", ["clean:js", "clean:css", "clean:tmpl", "clean:assets"]);

//gulp.task("min:js", function () {
//    return gulp.src([paths.js, "!" + paths.minJs], { base: "." })
//        .pipe(concat(paths.concatJsDest))
//        .pipe(uglify())
//        .pipe(gulp.dest("."));
//});

//gulp.task("min:css", function () {
//    return gulp.src([paths.css, "!" + paths.minCss])
//        .pipe(concat(paths.concatCssDest))
//        .pipe(cssmin())
//        .pipe(gulp.dest("."));
//});

//gulp.task("min", ["min:js", "min:css"]);


// Copies everything in the client folder except templates, Sass, and JS
gulp.task('copy', function () {
    return gulp.src(paths.assets, {
        base: './client/'
    })
      .pipe(gulp.dest(paths.webroot))
    ;
});

// Copies your app's page templates and generates URLs for them
gulp.task('copy:tmpl', function () {
    return gulp.src('./client/templates/**/*.html')
      .pipe(router({
          path: paths.genRouteJs,
          root: 'client'
      }))
      .pipe(gulp.dest(paths.genTmpl))
    ;
});

// Compiles the Foundation for Apps directive partials into a single JavaScript file
gulp.task('copy:foundation', function (cb) {
    gulp.src('wwwroot/lib/foundation-apps/js/angular/components/**/*.html')
      .pipe($.ngHtml2js({
          prefix: 'components/',
          moduleName: 'foundation',
          declareModule: false
      }))
      .pipe($.uglify())
      .pipe($.concat('templates.js'))
      .pipe(gulp.dest(paths.genJsDest))
    ;

    // Iconic SVG icons
    gulp.src('./wwwroot/lib/foundation-apps/iconic/**/*')
      .pipe(gulp.dest('./wwwroot/assets/img/iconic/'))
    ;

    cb();
});

// Compiles Sass
gulp.task('sass', function () {
    var minifyCss = $.if(isProduction, $.minifyCss());

    return gulp.src('client/assets/scss/app.scss')
      .pipe($.sass({
          includePaths: paths.sass,
          outputStyle: (isProduction ? 'compressed' : 'nested'),
          errLogToConsole: true
      }))
      //.pipe($.autoprefixer({
      //   browsers: ['last 2 versions', 'ie 10']
      //}))
      .pipe(minifyCss)
      .pipe($.concat('app.css'))
      .pipe(gulp.dest(paths.genCssDest))
    ;
});


// Compile TypeScript
gulp.task('compile:ts', function () {
    var tsResult = tsProject.src() // instead of gulp.src(...) 
        .pipe(sourcemaps.init())
		.pipe(ts(tsProject));

    return tsResult.js
        .pipe(sourcemaps.write('.'))
        .pipe(gulp.dest('.'));
});


// Compiles and copies the Foundation for Apps JavaScript, as well as your app's custom JS
gulp.task('uglify', ['uglify:foundation', 'uglify:app'])

gulp.task('uglify:foundation', function (cb) {
    var uglify = $.if(isProduction, $.uglify()
      .on('error', function (e) {
          console.log(e);
      }));

    return gulp.src(paths.foundationJS)
      .pipe(uglify)
      .pipe($.concat('foundation.js'))
      .pipe(gulp.dest(paths.genJsDest))
    ;
});

gulp.task('uglify:app', function () {
    var uglify = $.if(isProduction, $.uglify()
      .on('error', function (e) {
          console.log(e);
      }));

    return gulp.src(paths.appJS)
      .pipe(uglify)
      .pipe($.concat('app.js'))
      .pipe(gulp.dest(paths.genJsDest))
    ;
});

// Builds your entire app once, without starting a server
gulp.task('build', function (cb) {
    sequence('clean', 'compile:ts', ['copy', 'copy:foundation', 'sass', 'uglify'], 'copy:tmpl', cb);
});
