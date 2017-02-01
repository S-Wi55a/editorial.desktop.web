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
    entryPointMatch: './features/**/*-page.{js,ts}', // anything ends with -page.js
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

    entries['vendor'] = ['./Features/Shared/Assets/Js/vendor.js'];
    entries['common'] = ['./Features/Shared/Assets/csn.common.js'];
    entries['fonts'] = ['./Features/Shared/Assets/Fonts/fonts.js'];

    return entries;
}

const prodLoaderCSSExtract = ExtractTextPlugin.extract({
                                fallbackLoader: 'style-loader',
                                loader: 'css-loader!clean-css-loader!postcss-loader!resolve-url-loader!sass-loader?sourceMap'
                            })
const devLoaderCSSExtract =  ['style-loader', 'css-loader', 'clean-css-loader', 'postcss-loader', 'resolve-url-loader', 'sass-loader?sourceMap'];

module.exports = {
    entry: getEntryFiles(),
    output: {
        path: config.outputPath,
        publicPath: config.publicPath,
        filename: isProd ? '[name]-[chunkhash].js' : '[name].js'
    },
    module: {
        rules: [
            {
                test: [/\.js$/,/\.es6$/],
                exclude: /(node_modules|bower_components|unitTest)/,
                enforce: 'pre',
                loader: 'eslint-loader'
            },
            {
                test: [/\.js$/,/\.es6$/],
                exclude: /(node_modules|bower_components|unitTest)/,
                loader: 'babel-loader'
            },
            {
               test: /\.modernizrrc.js$/,
               exclude: /(node_modules|bower_components|unitTest)/,
               loader: "modernizr-loader"
            },
            {
                test: /\.css$/,
                exclude: /node_modules/,
                use: isProd ? prodLoaderCSSExtract : devLoaderCSSExtract

            },
            {
                test: /\.scss$/,
                exclude: [/(node_modules|bower_components|unitTest)/, /fonts\.scss/],
                use: isProd ? prodLoaderCSSExtract : devLoaderCSSExtract
            },
            {
                test: /fonts\.scss$/,
                exclude: /node_modules/,
                use: prodLoaderCSSExtract
            },
            {
                test: /.*\.(gif|png|jpe?g|svg)$/i,
                exclude: [/(node_modules|bower_components|unitTest)/, /fonts/],
                use: [
                    'file-loader?name=images/[name].[ext]',
                    {
                        loader: 'image-webpack-loader',
                        query: {
                            progressive: true,
                            optipng: {
                                optimizationLevel: 7,
                            },
                            gifsicle: {
                                interlaced: false,
                            },
                            pngquant: {
                                quality: '65-90',
                                speed: 4
                            }
                        }
                    }
                ]
            },
            {
                test: /\.(eot|svg|ttf|woff|woff2)$/,
                exclude: /(images|img)/,
                use: 'file-loader?name=fonts/[name].[ext]'
            }
        ]
    },
    resolve: {
        extensions: ['.js', '.scss'],
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
        new ExtractTextPlugin({
            filename: isProd ? '[name]-[contenthash].css' : '[name].css',
            disable: false
        }),
        new webpack.optimize.CommonsChunkPlugin({
            name: "common",
            filename: isProd ? "csn.common-[chunkhash].js" : "csn.common.js",
            minChunks: 2
        }),
        // new webpack.LoaderOptionsPlugin({
        //     minimize: true,
        //     debug: false
        // })
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
