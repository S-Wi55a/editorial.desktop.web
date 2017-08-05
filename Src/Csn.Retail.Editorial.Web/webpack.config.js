'use strict';
// Webpack build
var glob = require('glob'),
    path = require('path'),
    webpack = require('webpack'),
    ExtractTextPlugin = require('extract-text-webpack-plugin'),
    AssetsPlugin = require('assets-webpack-plugin'),
    BrowserSyncPlugin = require('browser-sync-webpack-plugin'),
    HappyPack = require('happypack'),
    rimraf = require('rimraf'),
    BundleAnalyzerPlugin = require('webpack-bundle-analyzer').BundleAnalyzerPlugin,
    ForkTsCheckerWebpackPlugin = require('fork-ts-checker-webpack-plugin'),
    os = require('os'),
    ForkTsCheckerNotifierWebpackPlugin = require('fork-ts-checker-notifier-webpack-plugin'),
    WebpackNotifierPlugin = require('webpack-notifier');


console.log('CPU\'S', os.cpus().length);

//---------------------------------------------------------------------------------------------------------
// List of Tenants
// Make sure associated '_settings--tenant.scss' file is added to Features\Shared\Assets\Css\Settings\

const listofTenants = [
    'carsales',
    'constructionsales',
    'bikesales',
    'carpoint',
    'boatsales',
    'boatpoint',
    'trucksales',
    'caravancampingsales',
    'farmmachinerysales',
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
    'Features/Errors/Assets',
    'Features'
];

// list of path to search for files
const s3path = 'dist/retail/editorial/'; //transfer to s3 is handles with gulp

//---------------------------------------------------------------------------------------------------------
// Remove dist folder
rimraf('./dist', function (err) { if (err) { throw err; } });

const IS_PROD = process.env.NODE_ENV === 'production' ? true : false;
const TENANTS = process.env.TENANT.toLowerCase() === 'all' ? listofTenants : [process.env.TENANT.trim()];
const VIEW_BUNDLE = process.env.VIEW_BUNDLE === 'true ' ? true : false;

// Error with sourcemaps b/c of css-loader. So inline URL to resolve issue (for development only)
const URL_LIMIT = IS_PROD ? 1 : undefined;

const happyThreadPool = HappyPack.ThreadPool({ size: 4 });

var config = {
    entryPointMatch: './Features/**/*-page.js', // anything ends with -page.js
    outputPath: path.join(__dirname, s3path),
    publicPath: IS_PROD ? './' : s3path
}

//Plugins vars
const assetsPluginInstance = new AssetsPlugin({
        filename: 'webpack.assets.json',
        path: __dirname,
        prettyPrint: true,
        fullPath: false
    });

const ForkTsCheckerWebpackPluginInstance = tenant => {
        let arr = []

        if(!IS_PROD){
            arr.unshift(new ForkTsCheckerNotifierWebpackPlugin({ title: `${tenant} - TypeScript` }))
        }

        arr.push(
            new ForkTsCheckerWebpackPlugin({
                watch: './Features/**/*', // optional but improves performance (less stat calls)
            }))
        
        return arr

}

const CommonsChunkPlugin = (pageEntries, tenant) => [
            //Per page -- pull chunks (from code splitting chunks) from each entry into parent(the entry)
            new webpack.optimize.CommonsChunkPlugin({
                names: pageEntries,
                children: true,
                minChunks: 2
            }),
            // Common -- pull everything from pages entries and make global common chunk
            new webpack.optimize.CommonsChunkPlugin({
                name: 'csn.common' + '--' + tenant,
                chunks: pageEntries,
                minChunks: 2
            }),
            //Vendor - Will look through every entry and match against itself or if a library from node_module is used twice
            new webpack.optimize.CommonsChunkPlugin({
                name: 'csn.vendor' + '--' + tenant,
                minChunks: function(module, count) {
                    // This prevents stylesheet resources with the .css or .scss extension
                    // from being moved from their original chunk to the vendor chunk
                    if (module.resource && (/^.*\.(css|scss)$/).test(module.resource)) {
                        return false;
                    }
                    return module.context && module.context.indexOf("node_modules") !== -1;
                }
            }),
            //Manifest - Webpack Runtime
            new webpack.optimize.CommonsChunkPlugin({
                name: 'csn.manifest' + '--' + tenant,
                minChunks: Infinity
            })
]

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
    return entries;
}


module.exports = (env) => {

    process.env.BABEL_ENV = env;

    let moduleExportArr = [];

    //run through list of tenants
    TENANTS.forEach((tenant) => {

        const loaders = [
                    {
                        loader: 'css-loader',
                        options: {
                            sourceMap: IS_PROD ? false : true,
                            minimize: IS_PROD ? true : false
                        }
                    },
                    'postcss-loader?sourceMap',
                    {
                        loader: 'resolve-url-loader',
                        options: {
                            sourceMap: IS_PROD ? false : true,
                            keepQuery: true
                        }
                    },
                    {
                        loader: 'sass-loader',
                        options: {
                            includePaths: listOfPaths,
                            sourceMap: true,
                            data: '@import "Css/Tenants/' + tenant + '/' + tenant +'.scss";'
                        }
                    }
        ]

        const prodLoaderCSSExtract = ExtractTextPlugin.extract({
            fallback: 'style-loader',
            use: loaders
        });

        const devLoaderCSSExtract = ['style-loader'].concat(loaders);

        const entries = getEntryFiles(tenant);

        const pageEntries = Object.keys(getEntryFiles(tenant));

        entries['csn.vendor' + '--' + tenant] = ['./Features/Shared/Assets/Js/csn.vendor.js'];
        entries['csn.common' + '--' + tenant] = ['./Features/Shared/Assets/csn.common.js'];


        let plugins = [
            assetsPluginInstance,
            new HappyPack({
                    // loaders is the only required parameter:
                    id: 'babel',
                    threadPool: happyThreadPool,
                    loaders: ['babel-loader?cacheDirectory=true']
                }),
                new HappyPack({
                    // loaders is the only required parameter:
                    id: 'babelTypeScript',
                    threadPool: happyThreadPool,
                    loaders: ['babel-loader?cacheDirectory=true',
                    {
                        path: 'ts-loader',
                        query: {
                            // disable type checker - we will use it in fork plugin
                            transpileOnly: true,
                            happyPackMode: true
                        }
                    }],
                }),
                new HappyPack({
                    // loaders is the only required parameter:
                    id: 'sass',
                    threadPool: happyThreadPool,
                    loaders: devLoaderCSSExtract //TODO and see if this a bottle neck
                }),
            
            new ExtractTextPlugin({
                filename: IS_PROD ? '[name]-[contenthash].css' : '[name].css',
                allChunks: false
            })
        ];

        plugins = plugins.concat(ForkTsCheckerWebpackPluginInstance(tenant))

        //Dev
        if(!IS_PROD) {
            //WebpackNotifierPlugin
            plugins.push(new WebpackNotifierPlugin())
            //NamedModulesPlugin
            plugins.push(new webpack.NamedModulesPlugin())
            //CommonsChunkPlugin
            plugins = plugins.concat(CommonsChunkPlugin(pageEntries, tenant))
            //BrowserSync
            plugins.push(
                new BrowserSyncPlugin(
                    // BrowserSync options 
                    {
                        // browse to http://localhost:3000/ during development 
                        host: 'localhost',
                        port: 3000,
                        // proxy the Webpack Dev Server endpoint 
                        // through BrowserSync 
                        proxy: {
                            target: 'http://localhost:8080',
                            ws: true
                        },
                        logLevel: "info",
                        open: false

                    },
                    // plugin options 
                    {
                        // prevent BrowserSync from reloading the page 
                        // and let Webpack Dev Server take care of this 
                        reload: false
                    }
                )
            )
        }

        //Production
        if(IS_PROD) {

            //Uglify
            plugins.push(
                new webpack.optimize.UglifyJsPlugin({
                    beautify: false,
                    mangle: {
                        screw_ie8: true,
                        keep_fnames: true
                    },
                    compress: {
                        screw_ie8: true
                    },
                    comments: false
                })
            )
        }

        if (VIEW_BUNDLE) {
            plugins.push(new BundleAnalyzerPlugin())
        } 

        moduleExportArr.push(
        {
            name: tenant,
            entry: entries,
            output: {
                path: config.outputPath,
                publicPath: config.publicPath,
                filename: IS_PROD ? '[name]-[chunkhash].js' : '[name].js'
            },
            module: {
                noParse: IS_PROD ? /\A(?!x)x/ : /react|jquery|swiper|ScrollMagic|modernizr|TinyAnimate|circles/,
                rules: [
                    {
                        enforce: 'pre',
                        test: /\.jsx?$/,
                        loader: "source-map-loader"
                    },
                    {
                        enforce: 'pre',
                        test: /\.tsx?$/,
                        use: "source-map-loader"
                    },
                    {
                        test: [/\.jsx?$/, /\.es6$/],
                        exclude: /(node_modules|bower_components|unitTest)/,
                        loaders: ['happypack/loader?id=babel']
                    },
                    {
                        test: /\.modernizrrc.js$/,
                        exclude: /(node_modules|bower_components|unitTest)/,
                        loader: "modernizr-loader"
                    },
                    {
                        test: /\.tsx?$/,
                        exclude: /(node_modules|bower_components|unitTest)/,
                        loaders: ['happypack/loader?id=babelTypeScript']
                    },
                    {
                        test: /\.css$/,
                        exclude: /(node_modules|bower_components|unitTest)/,
                        use: IS_PROD ? prodLoaderCSSExtract : devLoaderCSSExtract

                    },
                    {
                        test: /\.scss$/,
                        exclude: [/(node_modules|bower_components|unitTest)/],
                        use: IS_PROD ? prodLoaderCSSExtract : devLoaderCSSExtract
                    },
                    {
                        test: /.*\.(gif|png|jpe?g|svg)$/i,
                        exclude: [/(node_modules|bower_components|unitTest)/, /fonts/],
                        use: [
                            {
                                loader: 'url-loader',
                                options: {
                                    limit: URL_LIMIT,
                                    name: IS_PROD ? 'images/[name]-[hash].[ext]' : 'images/[name].[ext]'
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
                                    name: IS_PROD ? 'fonts/[name]-[hash].[ext]' : 'fonts/[name].[ext]'
                                }
                            }
                        ]
                    }
                ]
            },
            resolve: {
                extensions: ['.tsx', '.ts','.jsx','.js','.scss'],
                alias: {
                    modernizr$: path.resolve(__dirname, './.modernizrrc.js'),
                    'debug.addIndicators': path.resolve('node_modules', 'scrollmagic/scrollmagic/uncompressed/plugins/debug.addIndicators.js'),
                    swiper$: path.resolve('node_modules', 'swiper/dist/js/swiper.min.js'),
                    ScrollMagic$: path.resolve('node_modules', 'scrollmagic/scrollmagic/minified/ScrollMagic.min.js'),
                    TinyAnimate$: path.resolve('node_modules', 'TinyAnimate/bin/TinyAnimate.js'),
                    react$: path.resolve('node_modules', 'react/dist/react.min.js'),
                    //Bower Components
                    circles$: path.resolve('bower_components', 'circles/circles.min.js'),
                },
                aliasFields: ["browser"],
                descriptionFiles: ['package.json', 'bower.json'],
                modules: listOfPaths
            },
            externals: {
                jquery: 'jQuery'
            },
            plugins: plugins,
            stats: {
                //Add asset Information
                assets: true,
                // Sort assets by a field
                assetsSort: "field",
                // Add information about cached (not built) modules
                cached: true,
                // Add children information
                children: true,
                // Add chunk information (setting this to `false` allows for a less verbose output)
                chunks: true,
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
            devtool: IS_PROD ? "cheap-source-map" : "eval",
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
                    '/': {
                        target: 'http://' + (tenant || 'carsales').toString().toLowerCase() + '.editorial.csdev.com.au',
                        changeOrigin: true,
                        secure: false
                    }
                }
            }
        })
    });
    return moduleExportArr
};




