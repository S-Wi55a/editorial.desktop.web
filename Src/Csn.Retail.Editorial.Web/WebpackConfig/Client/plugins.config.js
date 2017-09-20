import path from 'path'
import webpack from 'webpack'
import { IS_PROD, IS_DEV, VIEW_BUNDLE } from '../Shared/env.config.js'
import ExtractTextPlugin from 'extract-text-webpack-plugin'
import AssetsPlugin from 'assets-webpack-plugin'
import BrowserSyncPlugin from 'browser-sync-webpack-plugin'
import ForkTsCheckerWebpackPlugin from 'fork-ts-checker-webpack-plugin'
import ForkTsCheckerNotifierWebpackPlugin from 'fork-ts-checker-notifier-webpack-plugin'
import WebpackNotifierPlugin from 'webpack-notifier'
import WatchMissingNodeModulesPlugin from 'react-dev-utils/WatchMissingNodeModulesPlugin'

const BundleAnalyzerPlugin = require('webpack-bundle-analyzer').BundleAnalyzerPlugin;

const assetsPluginInstance = new AssetsPlugin({
    filename: 'webpack.assets.json',
    path: path.resolve('./'),
    prettyPrint: true,
    fullPath: false,
    update: true
});

export const plugins = (tenant, pageEntries) => {

    let pluginsArr = [
        assetsPluginInstance,
        new webpack.DefinePlugin({
            'process.env.NODE_ENV': IS_PROD ? '"production"' : '"development"', //TODO add to shared / Correct the logic
            SERVER: JSON.stringify(false)
        }),
        new ExtractTextPlugin({
            filename: IS_PROD ? '[name]-[contenthash].css' : '[name].css',
            allChunks: false
        }),
        //Per page -- pull chunks (from code splitting chunks) from each entry into parent(the entry)
        new webpack.optimize.CommonsChunkPlugin({
            names: pageEntries,
            children: true,
            minChunks: function(module) {
                // This prevents stylesheet resources with the .css or .scss extension
                // from being moved from their original chunk to the vendor chunk
                if (module.resource && (/^.*\.(css|scss)$/).test(module.resource)) {
                    return false;
                }
                return true;
            }
        }),
        //Manifest - Webpack Runtime
        new webpack.optimize.CommonsChunkPlugin({
            name: 'csn.manifest' + '--' + tenant,
            minChunks: Infinity
        }),
        new webpack.NamedModulesPlugin(),
        new webpack.EnvironmentPlugin({
            NODE_ENV: 'development', // use 'development' unless process.env.NODE_ENV is defined
            DEBUG: false
        })
    ];

    if (VIEW_BUNDLE) {
        pluginsArr.push(new BundleAnalyzerPlugin())
    }
    if (IS_PROD) {
        pluginsArr.push(
            new webpack.optimize.ModuleConcatenationPlugin()
        )
        pluginsArr.push(
            new webpack.optimize.UglifyJsPlugin({
                compress: {
                  warnings: false,
                  // Disabled because of an issue with Uglify breaking seemingly valid code:
                  // https://github.com/facebookincubator/create-react-app/issues/2376
                  // Pending further investigation:
                  // https://github.com/mishoo/UglifyJS2/issues/2011
                  comparisons: false,
                },
                output: {
                  comments: false,
                  // Turned on because emoji and regex is not minified properly using default
                  // https://github.com/facebookincubator/create-react-app/issues/2488
                  ascii_only: true,
                },
                sourceMap: false,
              })
        )
    }
    if (IS_DEV) {
        pluginsArr.push(    
            new WatchMissingNodeModulesPlugin(path.resolve('node_modules'))
        )
        pluginsArr.push(
            new ForkTsCheckerWebpackPlugin({
                watch: './Features/**/*', // optional but improves performance (less stat calls),
                checkSyntacticErrors: true
            }) 
        )
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
        pluginsArr.push(
            new WebpackNotifierPlugin({ 
                title: `${tenant} - Server - Webpack`,
            })
        )
        pluginsArr.push(
            new ForkTsCheckerNotifierWebpackPlugin({ 
                title: `${tenant} - Client - TypeScript`,
            })
        )  
    }

    return pluginsArr
}