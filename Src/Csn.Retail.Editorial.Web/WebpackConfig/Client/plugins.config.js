import path from 'path'
import webpack from 'webpack'
import { IS_PROD, IS_DEV, VIEW_BUNDLE } from '../Shared/env.config.js'
import MiniCssExtractPlugin from 'mini-css-extract-plugin'
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
            SERVER: JSON.stringify(false)
        }),
        new MiniCssExtractPlugin({
            filename: IS_PROD ? '[name]-[contenthash].css' : '[name].css',
            chunkFilename: IS_PROD ?  '[id].[contenthash].css' : '[id].css',
        }),
        new webpack.IgnorePlugin(/^\.\/locale$/, /moment$/)
    ];

    if (VIEW_BUNDLE) {
        pluginsArr.push(new BundleAnalyzerPlugin())
    }
    if (IS_DEV) {
        pluginsArr.push(    
            new WatchMissingNodeModulesPlugin(path.resolve('node_modules'))
        )
        pluginsArr.push(
            new ForkTsCheckerWebpackPlugin({
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