import os from 'os'
import path from 'path'
import webpack from 'webpack'
import { isProd, VIEW_BUNDLE } from '../Shared/env.config.js'
import ExtractTextPlugin from 'extract-text-webpack-plugin'
import AssetsPlugin from 'assets-webpack-plugin'
import BrowserSyncPlugin from 'browser-sync-webpack-plugin'
import HappyPack from 'happypack'
import ForkTsCheckerWebpackPlugin from 'fork-ts-checker-webpack-plugin'
import ForkTsCheckerNotifierWebpackPlugin from 'fork-ts-checker-notifier-webpack-plugin'
import WebpackNotifierPlugin from 'webpack-notifier'

const happyThreadPool = HappyPack.ThreadPool({ size: os.cpus().length - 1 });
const BundleAnalyzerPlugin = require('webpack-bundle-analyzer').BundleAnalyzerPlugin;

//From Server/
import { devLoaderCSSExtract } from './loaders.config.js'

const assetsPluginInstance = new AssetsPlugin({
    filename: 'webpack.assets.json',
    path: path.resolve('./'),
    prettyPrint: true,
    fullPath: false
});

export const plugins = (tenant, pageEntries) => {

    let pluginsArr = [
        assetsPluginInstance,
        new webpack.DefinePlugin({
            'process.env.NODE_ENV': isProd ? '"production"' : '"development"', //TODO add to shared / Correct the logic
            SERVER: JSON.stringify(false)
        }),
        new webpack.ProvidePlugin({}),
        new ExtractTextPlugin({
            filename: isProd ? '[name]-[contenthash].css' : '[name].css',
            allChunks: false
        }),
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
            minChunks: function(module) {
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
        }),
        new webpack.NamedModulesPlugin(),
        new HappyPack({
            // loaders is the only required parameter:
            id: 'babel',
            loaders: ['babel-loader?cacheDirectory=true'],
            threadPool: happyThreadPool
        }),
        new HappyPack({
            // loaders is the only required parameter:
            id: 'sass',
            loaders: devLoaderCSSExtract(tenant),
            threadPool: happyThreadPool
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
                    transpileOnly: isProd ? false : true,
                    happyPackMode: true
                }
            }],
        }),
        new webpack.EnvironmentPlugin({
            NODE_ENV: 'development', // use 'development' unless process.env.NODE_ENV is defined
            DEBUG: false
        }),
        new ForkTsCheckerWebpackPlugin({
            watch: './Features/**/*', // optional but improves performance (less stat calls),
        }) 
    ];

    if (VIEW_BUNDLE) {
        pluginsArr.push(new BundleAnalyzerPlugin())
    }
    if (isProd) {
        pluginsArr.push(
            new webpack.optimize.ModuleConcatenationPlugin()
        )
    }
    if (!isProd) {
        pluginsArr.push(
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
        pluginsArr.push(new WebpackNotifierPlugin(
            { 
                title: `${tenant} - Server - Webpack`,
            }
        ))
        pluginsArr.push(new ForkTsCheckerNotifierWebpackPlugin({ 
            title: `${tenant} - Client - TypeScript`,
        }))  
    }

    return pluginsArr
}