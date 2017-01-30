'use strict';

var glob = require('glob'),
    path = require('path'),
    webpack = require('webpack'),
    CleanWebpackPlugin = require('clean-webpack-plugin'),
    ExtractTextPlugin = require('extract-text-webpack-plugin'),
    AssetsPlugin = require('assets-webpack-plugin');

var argv = require('yargs').argv;

var isProd = process.env.NODE_ENV.trim() === 'production' ? true : false;

var config = {
    entryPointMatch: './features/**/{*-page,*-component}.{js,ts}', // anything ends with -page.js
    outputPath: path.join(__dirname, isProd ? 'dist/retail/editorial' : 'dist'),
    publicPath: (argv.publicPath || './')
}


function getEntryFiles(){
    let entries = {};

    let matchedFiles = glob.sync(config.entryPointMatch);

    let length = matchedFiles.length;

    for(let i = 0; i < length; i++){
        let filePath = matchedFiles[i];
        let ext = path.extname(filePath);
        let filename = path.basename(filePath, ext);
        entries[filename] = filePath;
    }

    entries['common'] = ['jquery', './Features/Shared/Assets/csn.common.js'];
    entries['fonts'] = ['./Features/Shared/Assets/Fonts/fonts.js'];


    return entries;
}


module.exports = {
    entry: getEntryFiles(),
    output: {
        path: config.outputPath,
        publicPath: config.publicPath,
        filename: isProd ? '[name]-[chunkhash].js' : '[name].js'
    },
    module: {
        preLoaders:[
            {
                test: [/\.js$/,/\.es6$/],
                exclude: /node_modules/,
                loader: 'eslint-loader'
            }
        ],
        loaders: [
            {
                test: /\.ts$/,
                exclude: /node_modules/,
                loaders: ['ts-loader']
            },
            {
                test: [/\.js$/,/\.es6$/],
                exclude: /node_modules/,
                loader: 'babel-loader',
                query: {
                    presets: ['es2015']
                }
            },
            {
                test: /\.modernizrrc.js$/,
                loader: "modernizr"
            },
            {
                test: [/\.css$/],
                exclude: /node_modules/,
                loaders: ['style-loader', 'css-loader?sourceMap', 'autoprefixer-loader']
            },
            {
                test: /\.scss$/,
                exclude: [/(node_modules|bower_components|unitTest)/, /fonts\.scss/],
                loaders: ['style-loader', 'css-loader?sourceMap', 'autoprefixer-loader', 'resolve-url-loader', 'sass-loader?sourceMap']
            },
            {
                test: /fonts\.scss$/,
                exclude: /node_modules/,
                loader: ExtractTextPlugin.extract('css-loader?sourceMap!autoprefixer-loader!resolve-url-loader!sass-loader?sourceMap')
            },
            {
                test: /.*\.(gif|png|jpe?g|svg)$/i,
                exclude: [/(node_modules|bower_components|unitTest)/, /fonts/],
                loaders: [
                    'file?hash=sha512&digest=hex&name=images/[name].[ext]',
                    'image-webpack'
                ]
            },
            {
                test: /\.(eot|svg|ttf|woff|woff2)$/,
                exclude: /(images|img)/,
                loader: 'file?name=fonts/[name].[ext]'
            }
        ]
    },
    imageWebpackLoader: {
        pngquant:{
            quality: "65-90",
            speed: 4
        },
        svgo:{
            plugins: [
                { removeViewBox: false },
                { removeEmptyAttrs: false }
            ]
        }
    },
    resolve: {
        extensions: ['', '.js', '.ts', '.es6', '.scss'],
        alias: {
            modernizr$: path.resolve(__dirname, "./.modernizrrc.js")
        }
    },
    plugins: [
        new AssetsPlugin({
            filename: 'webpack.assets.json',
            path: __dirname,
            prettyPrint: true
        }),
        new CleanWebpackPlugin(['dist'], {
            root: __dirname,
            verbose: true,
            dry: false
        }),
        new ExtractTextPlugin(isProd ? '[name]-[chunkhash].css' : '[name].css'),
        new webpack.optimize.CommonsChunkPlugin({
            name: "common",
            filename: isProd ? "csn.common-[chunkhash].js" : "csn.common.js",
            minChunks: Infinity
        })
    ],
    devtool: "cheap-module-source-map",
    devServer: {
        proxy: {
            '/dist/dist': {
                target: 'http://localhost:8080',
                changeOrigin: true,
                secure: false,
                pathRewrite: { '^/dist/dist': 'dist' }
            },
            '/': {
                target: 'http://' + (argv.tenet || 'carsales').toString().toLowerCase() + '.editorial.csdev.com.au',
                changeOrigin: true,
                secure: false
            }
        }
    }
};
