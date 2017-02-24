'use strict';

// Webpack build
var glob = require('glob'),
    path = require('path'),
    webpack = require('webpack'),
    ExtractTextPlugin = require('extract-text-webpack-plugin'),
    AssetsPlugin = require('assets-webpack-plugin'),
    HappyPack = require('happypack'),
    rimraf = require('rimraf');

//---------------------------------------------------------------------------------------------------------
// List of Tenants
// Make sure associated '_settings--tenant.scss' file is added to Features\Shared\Assets\Css\Settings\

const listofTenants = [
    'carsales',
    'constructionsales',
    'bikesales'
];

//---------------------------------------------------------------------------------------------------------
// list of path to search for files
const listOfPaths = [
    path.resolve(__dirname, './'),
    'node_modules',
    'bower_components',
    'Features/Shared/Assets',
    'Features/Details/Assets',
    'Features/SiteNav/Assets',
    'Features'
];
//---------------------------------------------------------------------------------------------------------

var assetsPluginInstance = new AssetsPlugin({
        filename: 'webpack.assets.json',
        path: __dirname,
        prettyPrint: true
    });

var isProd = process.env.NODE_ENV === 'production' ? true : false;

const TENANTS = process.env.TENANT ? [process.env.TENANT.trim()] : listofTenants;

// Error with sourcemaps b/c of css-loader. So inline URL to resolve issue (for development only)
const URL_LIMIT = isProd ? 1 : null;


var config = {
    entryPointMatch: './Features/**/*-page.{js,ts}', // anything ends with -page.js
    outputPath: path.join(__dirname, isProd ? 'dist/retail/editorial' : 'dist'),
    publicPath: './'
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

        const loaders = [
                    {
                        loader: 'css-loader',
                        options: {
                            sourceMap: isProd ? false : true,
                            minimize: isProd ? true : false
                        }
                    },
                    'postcss-loader?sourceMap',
                    {
                        loader: 'resolve-url-loader',
                        options: {
                            sourceMap: isProd ? false : true,
                            keepQuery: true
                        }
                    },
                    {
                        loader: 'sass-loader',
                        options: {
                            includePaths: listOfPaths,
                            sourceMap: true,
                            data: '@import "Css/Settings/_settings--' + tenant + '.scss";'
                        }
                    }
        ]

        const prodLoaderCSSExtract = ExtractTextPlugin.extract({
            fallback: 'style-loader',
            use: loaders
        });

        const devLoaderCSSExtract = ['style-loader'].concat(loaders);

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
                noParse: /jquery/,
                rules: [
                    {
                        test: [/\.js$/, /\.es6$/],
                        exclude: /(node_modules|bower_components|unitTest)/,
                        loaders: ['happypack/loader?id=babel']
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
                        exclude: [/(node_modules|bower_components|unitTest)/],
                        use: isProd ? prodLoaderCSSExtract : devLoaderCSSExtract
                    },
                    {
                        test: /.*\.(gif|png|jpe?g|svg)$/i,
                        exclude: [/(node_modules|bower_components|unitTest)/, /fonts/],
                        use: [
                            {
                                loader: 'url-loader',
                                options: {
                                    limit: URL_LIMIT,
                                    name: isProd ? 'images/[name]-[hash].[ext]' : 'images/[name].[ext]'
                                }
                            },
                            {
                                loader: 'image-webpack-loader',
                                options: {
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
                        use: [
                            {
                                loader: 'url-loader',
                                options: {
                                    limit: URL_LIMIT,
                                    name: isProd ? 'fonts/[name]-[hash].[ext]' : 'fonts/[name].[ext]'
                                }
                            }
                        ]
                    }
                ]
            },
            resolve: {
                extensions: ['.js','.scss'],
                alias: {
                    modernizr$: path.resolve(__dirname, './.modernizrrc.js')
                },
                descriptionFiles: ['package.json', 'bower.json'],
                modules: listOfPaths
            },
            plugins: [
                assetsPluginInstance,
                new ExtractTextPlugin({
                    filename: isProd ? '[name]-[contenthash].css' : '[name].css',
                }),
                new webpack.optimize.CommonsChunkPlugin({
                    names: ['csn.common' + '--' + tenant, 'vendor'],
                    minChunks: 2
                }),
                new webpack.NamedModulesPlugin(),
                new HappyPack({
                    // loaders is the only required parameter:
                    id: 'babel',
                    loaders: ['babel-loader?cacheDirectory=true']
                }),
                new HappyPack({
                    // loaders is the only required parameter:
                    id: 'sass',
                    loaders: devLoaderCSSExtract
                })
            ],
            devtool: isProd ? "cheap-source-map" : "eval",
            devServer: {
                stats: {
                    // Add asset Information
                    assets: true,
                    // Sort assets by a field
                    assetsSort: "field",
                    // Add information about cached (not built) modules
                    cached: true,
                    // Add children information
                    children: true,
                    // Add chunk information (setting this to `false` allows for a less verbose output)
                    chunks: false,
                    // Add built modules information to chunk information
                    chunkModules: true,
                    // Add the origins of chunks and chunk merging info
                    chunkOrigins: false,
                    // Sort the chunks by a field
                    chunksSort: "field",
                    // Context directory for request shortening
                    //context: "../src/",
                    // `webpack --colors` equivalent
                    colors: true,
                    // Add errors
                    errors: true,
                    // Add details to errors (like resolving log)
                    errorDetails: true,
                    // Add the hash of the compilation
                    hash: false,
                    // Add built modules information
                    modules: false,
                    // Sort the modules by a field
                    modulesSort: "field",
                    // Add public path information
                    publicPath: false,
                    // Add information about the reasons why modules are included
                    reasons: false,
                    // Add the source code of modules
                    source: false,
                    // Add timing information
                    timings: true,
                    // Add webpack version information
                    version: false,
                    // Add warnings
                    warnings: true
                },
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

