'use strict';

var glob = require('glob'),
    path = require('path'),
    webpack = require('webpack'),
    ExtractTextPlugin = require('extract-text-webpack-plugin'),
    AssetsPlugin = require('assets-webpack-plugin'),
    rimraf = require('rimraf');

var assetsPluginInstance = new AssetsPlugin({
        filename: 'webpack.assets.json',
        path: __dirname,
        prettyPrint: true
    });

var argv = require('yargs').argv;


var isProd = process.env.NODE_ENV.trim() === 'production' ? true : false;

const listofTenants = ['carsales', 'constructionsales'];

const TENANTS = process.env.TENANT ?  [process.env.TENANT.trim()] : listofTenants;

var config = {
    entryPointMatch: './features/**/*-page.{js,ts}', // anything ends with -page.js
    outputPath: path.join(__dirname, isProd ? 'dist/retail/editorial' : 'dist'),
    publicPath: (argv.publicPath || './')
}


function getEntryFiles(tenant) {
    if (!tenant) {
        tenant = '';
    }
    let entries = {};

    let matchedFiles = glob.sync(config.entryPointMatch);

    let length = matchedFiles.length;

    for(let i = 0; i < length; i++){
        let filePath = matchedFiles[i];
        let ext = path.extname(filePath);
        let filename = path.basename(filePath, ext);
        entries[filename + '--' + tenant] = filePath;
    }

    entries['vendor'] = ['./Features/Shared/Assets/Js/vendor.js'];
    entries['csn.common' + '--' + tenant] = ['./Features/Shared/Assets/csn.common.js'];
    entries['fonts'] = ['./Features/Shared/Assets/Fonts/fonts.js'];

    return entries;
}

// Remove dist folder
rimraf('./dist',
    function(err) {
        if (err) {
            throw err;
        }
        // done
    });

module.exports = function () {

    let moduleExportArr = [];

    //run through list of tenants
    TENANTS.forEach((tenant) => {

        const prodLoaderCSSExtract = ExtractTextPlugin.extract({
            fallback: 'style-loader',
            use: [
                'css-loader', 'clean-css-loader', 'postcss-loader', 'resolve-url-loader', {
                    loader: 'sass-loader',
                    options: {
                        includePaths: ['Features/Shared/Assets/Css', 'node_modules'],
                        sourceMap: true,
                        data: '@import "variables/colors/_variables-colors--' + tenant + '.scss";'
                    }
                }
            ]
        });
        const devLoaderCSSExtract = ['style-loader', 'css-loader', 'clean-css-loader', 'postcss-loader', 'resolve-url-loader', {
            loader: 'sass-loader',
            options: {
                includePaths: ['Features/Shared/Assets/Css', 'node_modules'],
                sourceMap: true,
                data: '@import "variables/colors/_variables-colors--' + tenant + '.scss";'
            }
        }];

        moduleExportArr.push(
        {
            name: tenant,
            entry: getEntryFiles(tenant),
            output: {
                path: config.outputPath,
                publicPath: config.publicPath,
                filename: isProd ? '[name]-[chunkhash].js' : '[name].js'
            },
            module: {
                rules: [
                    {
                        test: [/\.js$/, /\.es6$/],
                        exclude: /(node_modules|bower_components|unitTest)/,
                        enforce: 'pre',
                        loader: 'eslint-loader'
                    },
                    {
                        test: [/\.js$/, /\.es6$/],
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
                        exclude: /(node_modules|bower_components|unitTest)/,
                        use: isProd ? prodLoaderCSSExtract : devLoaderCSSExtract

                    },
                    {
                        test: /\.scss$/,
                        exclude: [/(node_modules|bower_components|unitTest)/, /fonts\.scss/],
                        use: isProd ? prodLoaderCSSExtract : devLoaderCSSExtract
                    },
                    {
                        test: /fonts\.scss$/,
                        exclude: /(node_modules|bower_components|unitTest)/,
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
                    modernizr$: path.resolve(__dirname, './.modernizrrc.js')
                },
                descriptionFiles: ['package.json', 'bower.json'],
                modules: [path.resolve(__dirname, './'), 'node_modules', 'bower_components', 'Features/Shared/Assets/Js']
            },
            plugins: [
                assetsPluginInstance,
                new ExtractTextPlugin({
                    filename: isProd ? '[name]-[contenthash].css' : '[name].css',
                }),
                new webpack.optimize.CommonsChunkPlugin({
                    names: isProd ? ['csn.common' + '--' + tenant + '-[chunkhash]', 'vendor-[chunkhash]'] : ['csn.common' + '--' + tenant, 'vendor'],
                    minChunks: 2
                }),
                new webpack.NamedModulesPlugin()
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
                        target: 'http://' + (tenant || 'carsales').toString().toLowerCase() + '.editorial.csdev.com.au',
                        changeOrigin: true,
                        secure: false
                    }
                }
            }
        }
            )
    });
    return moduleExportArr
};

