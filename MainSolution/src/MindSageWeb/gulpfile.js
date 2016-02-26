/// <binding AfterBuild='build' />
/// <reference path="wwwroot/lib/angular/angular.min.js" />
/// <reference path="wwwroot/lib/angular/angular.js" />
/// <binding Clean='clean' />
"use strict";

var gulp = require("gulp"),
    rimraf = require("rimraf"),
    concat = require("gulp-concat"),
    cssmin = require("gulp-cssmin"),
    uglify = require("gulp-uglify");
var $ = require('gulp-load-plugins')();
var argv = require('yargs').argv;
var browser = require('browser-sync');
var gulp = require('gulp');
var panini = require('panini');
var sequence = require('run-sequence');
var sherpa = require('style-sherpa');
var ts = require('gulp-typescript');

// Check for --production flag
var isProduction = !!(argv.production);

// Port to use for the development server.
var PORT = 8000;

// Browsers to target when prefixing CSS.
var COMPATIBILITY = ['last 2 versions', 'ie >= 9'];

// File paths to various assets are defined here.
var PATHS = {
    assets: [
        'src/assets/**/*',
        '!src/assets/{img,js,scss}/**/*'
    ],
    sass: [
        'wwwroot/lib/foundation-sites/scss',
        'wwwroot/lib/motion-ui/src/'
    ],
    javascript: [
        'wwwroot/lib/jquery/dist/jquery.js',
        'wwwroot/lib/what-input/what-input.js',
        'wwwroot/lib/foundation-sites/js/foundation.core.js',
        'wwwroot/lib/foundation-sites/js/foundation.util.*.js',
    // Paths to individual JS components defined below
        'wwwroot/lib/foundation-sites/js/foundation.abide.js',
        'wwwroot/lib/foundation-sites/js/foundation.accordion.js',
        'wwwroot/lib/foundation-sites/js/foundation.accordionMenu.js',
        'wwwroot/lib/foundation-sites/js/foundation.drilldown.js',
        'wwwroot/lib/foundation-sites/js/foundation.dropdown.js',
        'wwwroot/lib/foundation-sites/js/foundation.dropdownMenu.js',
        'wwwroot/lib/foundation-sites/js/foundation.equalizer.js',
        'wwwroot/lib/foundation-sites/js/foundation.interchange.js',
        'wwwroot/lib/foundation-sites/js/foundation.magellan.js',
        'wwwroot/lib/foundation-sites/js/foundation.offcanvas.js',
        'wwwroot/lib/foundation-sites/js/foundation.orbit.js',
        'wwwroot/lib/foundation-sites/js/foundation.responsiveMenu.js',
        'wwwroot/lib/foundation-sites/js/foundation.responsiveToggle.js',
        'wwwroot/lib/foundation-sites/js/foundation.reveal.js',
        'wwwroot/lib/foundation-sites/js/foundation.slider.js',
        'wwwroot/lib/foundation-sites/js/foundation.sticky.js',
        'wwwroot/lib/foundation-sites/js/foundation.tabs.js',
        'wwwroot/lib/foundation-sites/js/foundation.toggler.js',
        'wwwroot/lib/foundation-sites/js/foundation.tooltip.js',
        'wwwroot/lib/angular/angular.js',
        'wwwroot/lib/angular-resource/angular-resource.js',
        'wwwroot/lib/ui-router/release/angular-ui-router.js',
        'src/assets/js/**/!(app).js',
        'src/assets/js/app.js'
    ],
    webroot: "./wwwroot/"
};

PATHS.js = PATHS.webroot + "js/**/*.js";
PATHS.minJs = PATHS.webroot + "js/**/*.min.js";
PATHS.css = PATHS.webroot + "css/**/*.css";
PATHS.minCss = PATHS.webroot + "css/**/*.min.css";
PATHS.jsDest = "./site.min.js";
PATHS.cssDest = "./site.min.css";
PATHS.pageTmpl = PATHS.webroot + "tmpl";
PATHS.webAssets = PATHS.webroot + "assets";
PATHS.assetJs = PATHS.webAssets + '/js';
PATHS.assetCss = PATHS.webAssets + '/css';

var tsProject = ts.createProject('scripts/tsconfig.json');

// Delete the "dist" folder
// This happens every time a build starts
gulp.task('clean:dist', function (done) {
    rimraf(PATHS.webAssets, done);
});
gulp.task('clean:tmpl', function (done) {
    rimraf(PATHS.pageTmpl, done);
});

gulp.task("clean:js", function (cb) {
    //rimraf(PATHS.jsDest, cb);
    rimraf(PATHS.assetJs, cb);
});

gulp.task("clean:css", function (cb) {
    //rimraf(PATHS.cssDest, cb);
    rimraf(PATHS.assetCss, cb);
});

gulp.task("clean", ["clean:js", "clean:css", "clean:dist", "clean:tmpl"]);

// Copy files out of the assets folder
// This task skips over the "img", "js", and "scss" folders, which are parsed separately
gulp.task('copy', function () {
    return gulp.src(PATHS.assets)
        .pipe(gulp.dest(PATHS.webAssets));
});

// Copy page templates into finished HTML files
gulp.task('pages', function () {
    return gulp.src('src/pages/**/*.{html,hbs,handlebars}')
        //.pipe(panini({
        //    root: 'src/pages/',
        //    layouts: 'src/layouts/',
        //    partials: 'src/partials/',
        //    data: 'src/data/',
        //    helpers: 'src/helpers/'
        //}))
        .pipe(gulp.dest(PATHS.pageTmpl))
        .on('finish', browser.reload);
});

gulp.task('pages:reset', function (done) {
    panini.refresh();
    gulp.run('pages');
    done();
});

gulp.task('styleguide', function (done) {
    sherpa('src/styleguide/index.md', {
        output: PATHS.pageTmpl + '/styleguide.html',
        template: 'src/styleguide/template.html'
    }, function () {
        browser.reload;
        done();
    });
});

// Compile Sass into CSS
// In production, the CSS is compressed
gulp.task('sass', function () {
    var uncss = $.if(isProduction, $.uncss({
        html: ['src/**/*.html'],
        ignore: [
            new RegExp('.foundation-mq'),
            new RegExp('^\.is-.*')
        ]
    }));

    var minifycss = $.if(isProduction, $.minifyCss());

    return gulp.src('src/assets/scss/app.scss')
        .pipe($.sourcemaps.init())
        .pipe($.sass({
            includePaths: PATHS.sass
        })
            .on('error', $.sass.logError))
        .pipe($.autoprefixer({
            browsers: COMPATIBILITY
        }))
        .pipe(uncss)
        .pipe(minifycss)
        .pipe($.if(!isProduction, $.sourcemaps.write()))
        .pipe(gulp.dest(PATHS.assetCss))
        .pipe(browser.reload({ stream: true }));
});

gulp.task("min:css", function () {
    return gulp.src([PATHS.css, "!" + PATHS.minCss])
        .pipe(concat(PATHS.cssDest))
        .pipe(cssmin())
        .pipe(gulp.dest(PATHS.assetCss));
});

gulp.task("min:js", function () {
    return gulp.src([PATHS.js, "!" + PATHS.minJs], { base: "." })
        .pipe(concat(PATHS.jsDest))
        .pipe(uglify())
        .pipe(gulp.dest(PATHS.assetJs));
});

gulp.task("min", ["min:js", "min:css"]);

// Compile TypeScript
gulp.task('typescripts', function () {
    var tsResult = tsProject.src() // instead of gulp.src(...) 
		.pipe(ts(tsProject));

    return tsResult.js.pipe(gulp.dest('.'));
});
// Combine JavaScript into one file
// In production, the file is minified
gulp.task('javascript', ['typescripts'], function () {
    var uglify = $.if(isProduction, $.uglify()
        .on('error', function (e) {
            console.log(e);
        }));

    return gulp.src(PATHS.javascript)
        .pipe($.sourcemaps.init())
        .pipe($.concat('app.js'))
        .pipe(uglify)
        .pipe($.if(!isProduction, $.sourcemaps.write()))
        .pipe(gulp.dest(PATHS.assetJs))
        .on('finish', browser.reload);
});

// Copy images to the "dist" folder
// In production, the images are compressed
gulp.task('images', function () {
    var imagemin = $.if(isProduction, $.imagemin({
        progressive: true
    }));

    return gulp.src('src/assets/img/**/*')
        .pipe(imagemin)
        .pipe(gulp.dest(PATHS.webAssets + '/img'))
        .on('finish', browser.reload);
});

// Build the "dist" folder by running all of the above tasks
gulp.task('build', function (done) {
    sequence('clean', ['pages', 'sass', 'javascript', 'min', 'images', 'copy'], 'styleguide', done);
});

// Start a server with LiveReload to preview the site in
gulp.task('server', ['build'], function () {
    browser.init({
        server: PATHS.webroot, port: PORT
    });
});

// Build the site, run the server, and watch for file changes
gulp.task('default', ['build', 'server'], function () {
    gulp.watch(PATHS.assets, ['copy']);
    gulp.watch(['src/pages/**/*'], ['pages']);
    gulp.watch(['src/{layouts,partials,helpers,data}/**/*'], ['pages:reset']);
    gulp.watch(['src/assets/scss/**/{*.scss, *.sass}'], ['sass']);
    gulp.watch(['src/assets/js/**/*.js'], ['javascript']);
    gulp.watch(['src/assets/img/**/*'], ['images']);
    gulp.watch(['src/styleguide/**'], ['styleguide']);
});
